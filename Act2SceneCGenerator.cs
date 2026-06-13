// ==========================================
// Act2SceneCGenerator - Act 2 Scene C Content (The Planted Thorn)
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEditor;
using UnityEngine;

public static class Act2SceneCGenerator
{
    // ==========================================
    // Output Path Configuration
    // ==========================================
    private const string SEQUENCE_FOLDER = "Assets/Data/Act2";
    private const string LINE_FOLDER     = "Assets/Data/Act2/Lines";
    private const string NAME_PREFIX     = "Act2SceneC";

    // ==========================================
    // Generate - Build Scene C DialogueLine/Sequence Assets
    // ==========================================
    [MenuItem("Frayed Red String/Act 2/Generate Scene C - The Planted Thorn")]
    public static void Generate()
    {
        DialogueAssetGenerator.LineDef[] lines = new DialogueAssetGenerator.LineDef[]
        {
            // ==========================================
            // L01 — Scene Opening: Cafe, Rain, Three Days Later
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                "Three days later. The afternoon crowd at Cozy Cafe has thinned to nothing. Outside, rain sketches long grey lines down the windows.",
                bg: BackgroundID.CozyCafe, bgTrans: BackgroundTransitionMode.Crossfade
            ),

            // ==========================================
            // L02 — Yua Opens the Trap
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "...Haru-kun. I found something. I wasn't sure whether to show you. It might hurt.",
                state: "SadImploring"
            ),

            // ==========================================
            // L03 — Haru Unguarded
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "What is it?",
                state: "WaitingToTalk"
            ),

            // ==========================================
            // L04 — Yua Presents the Fabricated Evidence
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "It's from Kenji's old account. Someone in his class sent it to me — they recognized your name. I almost deleted it... but I thought you deserved to know. [slides phone across the table]",
                state: "SadImploring"
            ),

            // ==========================================
            // L05 — Haru Reads It
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "...What is this.",
                state: "Pokerface"
            ),

            // ==========================================
            // L06 — Yua Delivers the Hook
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "He wrote it a week before. He said there was one person who was supposed to call him that day. And didn't.",
                state: "SadImploring"
            ),

            // ==========================================
            // L07 — Haru's Guilt Lands
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "...That was me. I had practice. I told him I'd call after. I forgot.",
                state: "CryingEyesClosed"
            ),

            // ==========================================
            // L08 — Yua's False Comfort (Keeps the Wound Open)
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "Haru-kun, you can't know for certain that —",
                state: "HappyBlush"
            ),

            // ==========================================
            // L09 — Haru Refuses the Escape
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "Don't. Please don't say that. I forgot. One call. That's all it would have taken, and I forgot.",
                state: "CryingEyesClosed"
            ),

            // ==========================================
            // L10 — Inner Monologue: Milestone FakeEvidencePlanted
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "The message doesn't exist. I wrote it last night in a notebook and photographed it. But the way guilt settles into someone's eyes when it finally has a shape — you can't manufacture that. That's real.",
                state: "InsaneSmile",
                innerMonologue: true,
                gameEvent: "FakeEvidencePlanted"
            ),

            // ==========================================
            // L11 — Yua Closes the Distance
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "[reaches across the table] ...Stay with me a little longer. You shouldn't be alone with this right now.",
                state: "HappyBlush"
            ),

            // ==========================================
            // L12 — Haru Surrenders
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "...Yua. I don't know what I'd do without you.",
                state: "SadImploring"
            ),

            // ==========================================
            // L13 — Inner Monologue: Milestone HaruDoubtSeeded
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "And there it is. The knot pulled tight. Every thread I've laid down over these weeks — every morning, every confession, every soft word at exactly the right moment — converging here. He has no one left to turn to but me.",
                state: "InsaneSmile",
                innerMonologue: true,
                gameEvent: "HaruDoubtSeeded"
            ),

            // ==========================================
            // L14 — Scene Closing Narration
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                "The rain keeps falling. Haru stares at the table. Yua watches him with the patience of someone who has already won."
            ),
        };

        DialogueAssetGenerator.BuildSequence(SEQUENCE_FOLDER, LINE_FOLDER, NAME_PREFIX, lines);
        Debug.Log($"[Act2SceneCGenerator] Built '{NAME_PREFIX}_Sequence' with {lines.Length} entries.");
    }
}
