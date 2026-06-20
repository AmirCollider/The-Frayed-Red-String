// ==========================================
// DialogueAssetGenerator - Editor Tool for Batch DialogueLine/DialogueSequence Creation
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEditor;
using UnityEngine;

public static class DialogueAssetGenerator
{
    // ==========================================
    // LineDef - Authoring Struct Mirroring DialogueLine Fields
    // ==========================================
    public struct LineDef
    {
        public string speakerId;
        public CharacterPosition speakerPosition;
        public string textEN;
        public string textJP;
        public string stateOverride;
        public BackgroundID backgroundId;
        public BackgroundTransitionMode backgroundTransition;
        public bool isInnerMonologue;
        public bool isFourthWall;
        public bool autoAdvance;
        public float autoAdvanceDelay;
        public string gameEventId;
        public bool isTitleCard;
        public string titleHeadingEN;
        public string titleHeadingJP;

        // ==========================================
        // Constructor - Positional Core Fields with Named-Optional Extras
        // ==========================================
        public LineDef(string speaker, CharacterPosition pos, string en, string jp = "",
            string state = "", BackgroundID bg = BackgroundID.None,
            BackgroundTransitionMode bgTrans = BackgroundTransitionMode.Crossfade,
            bool innerMonologue = false, bool fourthWall = false,
            bool autoAdv = false, float autoAdvDelay = 1.5f, string gameEvent = "")
        {
            speakerId = speaker;
            speakerPosition = pos;
            textEN = en;
            textJP = jp;
            stateOverride = state;
            backgroundId = bg;
            backgroundTransition = bgTrans;
            isInnerMonologue = innerMonologue;
            isFourthWall = fourthWall;
            autoAdvance = autoAdv;
            autoAdvanceDelay = autoAdvDelay;
            gameEventId = gameEvent;
            isTitleCard = false;
            titleHeadingEN = "";
            titleHeadingJP = "";
        }

        // ==========================================
        // TitleCard - Authoring Factory for a Fullscreen Black Act Intro/Outro Card
        // (heading = big act/image name; body = the line shown beneath it)
        // ==========================================
        public static LineDef TitleCard(string headingEN, string bodyEN,
            string headingJP = "", string bodyJP = "",
            bool autoAdv = false, float autoAdvDelay = 2.5f)
        {
            LineDef def = new LineDef(ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center, bodyEN, bodyJP);
            def.isTitleCard = true;
            def.titleHeadingEN = headingEN;
            def.titleHeadingJP = headingJP;
            def.autoAdvance = autoAdv;
            def.autoAdvanceDelay = autoAdvDelay;
            return def;
        }
    }

    // ==========================================
    // BuildSequence - Create/Update DialogueLine Assets and Wrapping DialogueSequence
    // ==========================================
    public static DialogueSequence BuildSequence(string sequenceFolder, string lineFolder, string namePrefix, LineDef[] lines)
    {
        EnsureFolder(sequenceFolder);
        EnsureFolder(lineFolder);

        DialogueSequence sequence = LoadOrCreate<DialogueSequence>($"{sequenceFolder}/{namePrefix}_Sequence.asset");
        sequence.entries.Clear();

        for (int i = 0; i < lines.Length; i++)
        {
            LineDef def = lines[i];
            string lineName = $"{namePrefix}_L{(i + 1):D2}";
            string linePath = $"{lineFolder}/{lineName}.asset";

            DialogueLine line = LoadOrCreate<DialogueLine>(linePath);
            line.speakerId = def.speakerId;
            line.speakerPosition = def.speakerPosition;
            line.textEN = def.textEN;
            line.textJP = def.textJP;
            line.characterStateOverride = def.stateOverride;
            line.backgroundId = def.backgroundId;
            line.backgroundTransition = def.backgroundTransition;
            line.isInnerMonologue = def.isInnerMonologue;
            line.isFourthWall = def.isFourthWall;
            line.autoAdvance = def.autoAdvance;
            line.autoAdvanceDelay = def.autoAdvanceDelay;
            line.gameEventId = def.gameEventId;
            line.isTitleCard = def.isTitleCard;
            line.titleCardHeadingEN = def.titleHeadingEN;
            line.titleCardHeadingJP = def.titleHeadingJP;
            EditorUtility.SetDirty(line);

            sequence.entries.Add(new DialogueEntry
            {
                type = DialogueEntryType.Line,
                line = line
            });
        }

        EditorUtility.SetDirty(sequence);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return sequence;
    }

    // ==========================================
    // LoadOrCreate - Reuse Existing Asset at Path or Create New Instance
    // ==========================================
    public static T LoadOrCreate<T>(string path) where T : ScriptableObject
    {
        T asset = AssetDatabase.LoadAssetAtPath<T>(path);
        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
        }
        return asset;
    }

    // ==========================================
    // BranchChoiceDef - Single Choice Option Data for Branch Generator Entries
    // ==========================================
    public struct BranchChoiceDef
    {
        public string labelEN;
        public string labelJP;
        public int affectionDelta;
        public LineDef[] consequenceLines;

        public BranchChoiceDef(string en, string jp, int delta, LineDef[] lines)
        {
            labelEN = en;
            labelJP = jp;
            affectionDelta = delta;
            consequenceLines = lines != null ? lines : new LineDef[0];
        }
    }

    // ==========================================
    // GeneratorEntry - Discriminated Union: Line or Branch for Mixed BuildSequence
    // ==========================================
    public struct GeneratorEntry
    {
        public bool isBranch;
        public LineDef lineDef;
        public string branchId;
        public BranchChoiceDef[] choices;

        public static GeneratorEntry FromLine(LineDef def) =>
            new GeneratorEntry { isBranch = false, lineDef = def };

        public static GeneratorEntry FromBranch(string id, BranchChoiceDef[] c) =>
            new GeneratorEntry { isBranch = true, branchId = id, choices = c };
    }

    // ==========================================
    // BuildSequence (Mixed) - Handle GeneratorEntry Array Containing Lines and Branches
    // ==========================================
    public static DialogueSequence BuildSequence(string sequenceFolder, string lineFolder, string namePrefix, GeneratorEntry[] entries)
    {
        EnsureFolder(sequenceFolder);
        EnsureFolder(lineFolder);

        DialogueSequence sequence = LoadOrCreate<DialogueSequence>($"{sequenceFolder}/{namePrefix}_Sequence.asset");
        sequence.entries.Clear();

        int lineCounter = 0;
        int branchCounter = 0;

        for (int i = 0; i < entries.Length; i++)
        {
            GeneratorEntry entry = entries[i];

            if (!entry.isBranch)
            {
                // ==========================================
                // Line Entry — Create and Apply DialogueLine Asset
                // ==========================================
                lineCounter++;
                LineDef def = entry.lineDef;
                string lineName = $"{namePrefix}_L{lineCounter:D2}";
                string linePath = $"{lineFolder}/{lineName}.asset";

                DialogueLine line = LoadOrCreate<DialogueLine>(linePath);
                line.speakerId = def.speakerId;
                line.speakerPosition = def.speakerPosition;
                line.textEN = def.textEN;
                line.textJP = def.textJP;
                line.characterStateOverride = def.stateOverride;
                line.backgroundId = def.backgroundId;
                line.backgroundTransition = def.backgroundTransition;
                line.isInnerMonologue = def.isInnerMonologue;
                line.isFourthWall = def.isFourthWall;
                line.autoAdvance = def.autoAdvance;
                line.autoAdvanceDelay = def.autoAdvanceDelay;
                line.gameEventId = def.gameEventId;
                line.isTitleCard = def.isTitleCard;
                line.titleCardHeadingEN = def.titleHeadingEN;
                line.titleCardHeadingJP = def.titleHeadingJP;
                EditorUtility.SetDirty(line);

                sequence.entries.Add(new DialogueEntry { type = DialogueEntryType.Line, line = line });
            }
            else
            {
                // ==========================================
                // Branch Entry — Create Consequence Sequences and Assemble Choice List
                // ==========================================
                branchCounter++;
                string branchSeqFolder = $"{sequenceFolder}/Branches";
                string branchLineFolder = $"{lineFolder}/Branches";
                EnsureFolder(branchSeqFolder);
                EnsureFolder(branchLineFolder);

                DialogueEntry branchEntry = new DialogueEntry
                {
                    type = DialogueEntryType.Branch,
                    branchId = entry.branchId,
                    choices = new System.Collections.Generic.List<DialogueChoice>()
                };

                for (int c = 0; c < entry.choices.Length; c++)
                {
                    BranchChoiceDef choiceDef = entry.choices[c];
                    string conseqPrefix = $"{namePrefix}_Branch{branchCounter:D2}_Choice{(c + 1):D2}";

                    DialogueSequence conseqSeq = null;
                    if (choiceDef.consequenceLines != null && choiceDef.consequenceLines.Length > 0)
                        conseqSeq = BuildSequence(branchSeqFolder, branchLineFolder, conseqPrefix, choiceDef.consequenceLines);

                    branchEntry.choices.Add(new DialogueChoice
                    {
                        labelEN = choiceDef.labelEN,
                        labelJP = choiceDef.labelJP,
                        affectionDelta = choiceDef.affectionDelta,
                        consequenceSequence = conseqSeq
                    });
                }

                sequence.entries.Add(branchEntry);
            }
        }

        EditorUtility.SetDirty(sequence);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return sequence;
    }

    // ==========================================
    // EnsureFolder - Recursively Create Folder Path Segments if Missing
    // ==========================================
    public static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path)) return;

        string[] parts = path.Split('/');
        string current = parts[0];
        for (int i = 1; i < parts.Length; i++)
        {
            string next = $"{current}/{parts[i]}";
            if (!AssetDatabase.IsValidFolder(next))
                AssetDatabase.CreateFolder(current, parts[i]);
            current = next;
        }
    }
}