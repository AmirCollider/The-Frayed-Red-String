// ==========================================
// Act1SceneCGenerator - Act 1 Scene C Content (Affection Choices)
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEditor;
using UnityEngine;

public static class Act1SceneCGenerator
{
    // ==========================================
    // Output Path Configuration
    // ==========================================
    private const string SEQUENCE_FOLDER = "Assets/Data/Act1";
    private const string LINE_FOLDER = "Assets/Data/Act1/Lines";
    private const string NAME_PREFIX = "Act1SceneC";

    // ==========================================
    // Generate - Build Scene C Mixed Lines and Branch Assets
    // ==========================================
    [MenuItem("Frayed Red String/Act 1/Generate Scene C - Affection Choices")]
    public static void Generate()
    {
        // ==========================================
        // Branch 01 Consequence Lines — Haru's Reply to Yua
        // ==========================================
        DialogueAssetGenerator.LineDef[] conseqC01A = new DialogueAssetGenerator.LineDef[]
        {
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "...You're going to make it very hard for me to stay composed.",
                state: "HappyBlush"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "Hook set. Compliments without provocation. Now let him wonder if he said too much.",
                state: "InsaneSmile",
                innerMonologue: true
            ),
        };

        DialogueAssetGenerator.LineDef[] conseqC01B = new DialogueAssetGenerator.LineDef[]
        {
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "...Friend. Right.",
                state: "Pokerface"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "Neutral. Not deep enough yet. Patience — this takes the time it takes.",
                state: "InsaneSmile",
                innerMonologue: true
            ),
        };

        DialogueAssetGenerator.LineDef[] conseqC01C = new DialogueAssetGenerator.LineDef[]
        {
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "Sorry. Forget I asked.",
                state: "Annoyed"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "Resistance noted. He's more guarded than expected. Pull back, create absence, let him miss the warmth.",
                state: "InsaneSmile",
                innerMonologue: true
            ),
        };

        // ==========================================
        // Branch 02 Consequence Lines — Walk Home
        // ==========================================
        DialogueAssetGenerator.LineDef[] conseqC02A = new DialogueAssetGenerator.LineDef[]
        {
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "...Really? Thank you, Haru-kun.",
                state: "HappyBlush"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "He volunteered. He chose proximity. Let him believe every step toward me was his own decision.",
                state: "InsaneSmile",
                innerMonologue: true
            ),
        };

        DialogueAssetGenerator.LineDef[] conseqC02B = new DialogueAssetGenerator.LineDef[]
        {
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "Of course. Goodnight then.",
                state: "Pokerface"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "He chose distance. Increase scarcity tomorrow — fewer messages, shorter replies. Make the cold air remind him of what it felt like when I was warm.",
                state: "InsaneSmile",
                innerMonologue: true
            ),
        };

        // ==========================================
        // Main Sequence Entries — Lines and Branch Nodes
        // ==========================================
        DialogueAssetGenerator.GeneratorEntry[] entries = new DialogueAssetGenerator.GeneratorEntry[]
        {
            DialogueAssetGenerator.GeneratorEntry.FromLine(new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                "They pass the vending machine at the corner of Shinonome Avenue, its blue light humming in the late afternoon.",
                bg: BackgroundID.PastelStreetVending, bgTrans: BackgroundTransitionMode.Crossfade
            )),
            DialogueAssetGenerator.GeneratorEntry.FromLine(new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "...Haru-kun? What do you honestly think of me?",
                state: "Shy"
            )),
            DialogueAssetGenerator.GeneratorEntry.FromLine(new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "What do I think of you...?",
                state: "WaitingToTalk"
            )),
            DialogueAssetGenerator.GeneratorEntry.FromBranch("Choice_Act1_HaruReply",
                new DialogueAssetGenerator.BranchChoiceDef[]
                {
                    new DialogueAssetGenerator.BranchChoiceDef(
                        "You're kind, thoughtful... I feel lucky to know you.",
                        "やさしくて、思いやりがある…知り合えてよかったと思う。",
                        2, conseqC01A
                    ),
                    new DialogueAssetGenerator.BranchChoiceDef(
                        "You're a good friend.",
                        "いい友達だよ。",
                        0, conseqC01B
                    ),
                    new DialogueAssetGenerator.BranchChoiceDef(
                        "That's... kind of a sudden question.",
                        "それは……急な質問だね。",
                        -1, conseqC01C
                    ),
                }
            ),
            DialogueAssetGenerator.GeneratorEntry.FromLine(new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                "The moment passes. They walk on through the amber light."
            )),
            DialogueAssetGenerator.GeneratorEntry.FromLine(new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "It's getting late. I should head home.",
                state: "Shy"
            )),
            DialogueAssetGenerator.GeneratorEntry.FromBranch("Choice_Act1_WalkHome",
                new DialogueAssetGenerator.BranchChoiceDef[]
                {
                    new DialogueAssetGenerator.BranchChoiceDef(
                        "I'll walk you home.",
                        "家まで送るよ。",
                        2, conseqC02A
                    ),
                    new DialogueAssetGenerator.BranchChoiceDef(
                        "I'd better head my own way. Stay safe.",
                        "僕は反対方向だから。気を付けてね。",
                        -1, conseqC02B
                    ),
                }
            ),
        };

        DialogueAssetGenerator.BuildSequence(SEQUENCE_FOLDER, LINE_FOLDER, NAME_PREFIX, entries);
        Debug.Log($"[Act1SceneCGenerator] Built '{NAME_PREFIX}_Sequence' with {entries.Length} entries.");
    }
}