// ==========================================
// Act1SceneBGenerator - Act 1 Scene B Content (Bakery Walk)
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEditor;
using UnityEngine;

public static class Act1SceneBGenerator
{
    // ==========================================
    // Output Path Configuration
    // ==========================================
    private const string SEQUENCE_FOLDER = "Assets/Data/Act1";
    private const string LINE_FOLDER = "Assets/Data/Act1/Lines";
    private const string NAME_PREFIX = "Act1SceneB";

    // ==========================================
    // Generate - Build Scene B DialogueLine/Sequence Assets
    // ==========================================
    [MenuItem("Frayed Red String/Act 1/Generate Scene B - Bakery Walk")]
    public static void Generate()
    {
        DialogueAssetGenerator.LineDef[] lines = new DialogueAssetGenerator.LineDef[]
        {
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                "They step out into the golden afternoon. The warm smell of butter and sugar drifts from Usagi Bakery behind them.",
                bg: BackgroundID.BakeryStreet, bgTrans: BackgroundTransitionMode.Crossfade
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "Here — strawberry mochi. I made a fresh batch this morning. Tell me honestly if it's too sweet.",
                state: "HappyBlush"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "...[takes one] Yua-chan, this is incredible. You made all of this yourself?",
                state: "HappyBlush"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "Every morning before school. Baking settles my nerves.",
                state: "Shy"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "Comfort requires a routine. A routine requires me. A boy who starts each morning with something I made will feel the cold the moment I stop.",
                state: "InsaneSmile",
                innerMonologue: true
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "You seem really dedicated. Do you want to bake professionally someday?",
                state: "WaitingToTalk"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "...Maybe. For now I just like knowing someone's day started a little better because of me.",
                state: "Shy"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                "She says it softly, glancing away at exactly the right moment. Haru smiles and doesn't notice."
            ),
        };

        DialogueAssetGenerator.BuildSequence(SEQUENCE_FOLDER, LINE_FOLDER, NAME_PREFIX, lines);
        Debug.Log($"[Act1SceneBGenerator] Built '{NAME_PREFIX}_Sequence' with {lines.Length} entries.");
    }
}