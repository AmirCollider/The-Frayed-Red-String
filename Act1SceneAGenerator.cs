// ==========================================
// Act1SceneAGenerator - Act 1 Scene A Content (Classroom Opening)
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEditor;
using UnityEngine;

public static class Act1SceneAGenerator
{
    // ==========================================
    // Output Path Configuration
    // ==========================================
    private const string SEQUENCE_FOLDER = "Assets/Data/Act1";
    private const string LINE_FOLDER = "Assets/Data/Act1/Lines";
    private const string NAME_PREFIX = "Act1SceneA";

    // ==========================================
    // Generate - Build Scene A DialogueLine/Sequence Assets
    // ==========================================
    [MenuItem("Frayed Red String/Act 1/Generate Scene A - Classroom Opening")]
    public static void Generate()
    {
        DialogueAssetGenerator.LineDef[] lines = new DialogueAssetGenerator.LineDef[]
        {
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                "The final bell rings. Cherry blossoms drift past the classroom windows, scattering pale pink light over the empty desks."
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "...Um — Haru-kun? Do you have a moment?",
                state: "Shy"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "Oh, Yua-chan. Sure — what's up?",
                state: "HappyBlush"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "I baked something at Usagi Bakery this morning. I... made too much, as usual. Would you like to share it with me on the way home?",
                state: "HappyBlush"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "Of course! I'd love that. Thanks, Yua-chan.",
                state: "HappyBlush"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "...Good. Step one: become indispensable. A boy who's used to someone bringing him something sweet starts to feel cold the moment that someone stops.",
                state: "InsaneSmile",
                innerMonologue: true
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "Come on — let's go before the bag gets cold!",
                state: "HappyBlush"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                "They gather their things. Outside, the cherry blossoms keep falling — soft, pink, and utterly without consequence."
            ),
        };

        DialogueSequence seq = DialogueAssetGenerator.BuildSequence(SEQUENCE_FOLDER, LINE_FOLDER, NAME_PREFIX, lines);
        Debug.Log($"[Act1SceneAGenerator] Built '{NAME_PREFIX}_Sequence' with {seq.Count} entries at {SEQUENCE_FOLDER}.");
    }
}