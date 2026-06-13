// ==========================================
// Act1SceneDGenerator - Act 1 Scene D Content (Red String Reveal)
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEditor;
using UnityEngine;

public static class Act1SceneDGenerator
{
    // ==========================================
    // Output Path Configuration
    // ==========================================
    private const string SEQUENCE_FOLDER = "Assets/Data/Act1";
    private const string LINE_FOLDER = "Assets/Data/Act1/Lines";
    private const string NAME_PREFIX = "Act1SceneD";

    // ==========================================
    // Generate - Build Scene D DialogueLine/Sequence Assets
    // ==========================================
    [MenuItem("Frayed Red String/Act 1/Generate Scene D - Red String Reveal")]
    public static void Generate()
    {
        DialogueAssetGenerator.LineDef[] lines = new DialogueAssetGenerator.LineDef[]
        {
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                "The last train of the evening rushes through. Cherry blossoms spiral in its wake — pink, weightless, without destination.",
                bg: BackgroundID.TrainPlatformSunset, bgTrans: BackgroundTransitionMode.Crossfade
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "...Haru-kun. Do you believe in the red string of fate?",
                state: "Shy"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "The legend? The invisible thread connecting two people who are meant to find each other?",
                state: "WaitingToTalk"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "Yes. [reaches into her pocket] ...Will you let me tie it? Just as a reminder that I'm here.",
                state: "HappyBlush"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "...Sure. [holds out his hand]",
                state: "Shy"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "This isn't a thread. It's a leash. And he held out his wrist and smiled.",
                state: "InsaneSmile",
                innerMonologue: true,
                gameEvent: "ShowRedString"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                "She ties the knot slowly, deliberately. Far above the platform, one last blossom breaks free and falls."
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "There. Now you can't forget me even if you want to.",
                state: "HappyBlush"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "...I wasn't planning to.",
                state: "HappyBlush"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                "The platform empties. The string gleams in the dying light — fragile, insistent, already fraying at the ends."
            ),
        };

        DialogueAssetGenerator.BuildSequence(SEQUENCE_FOLDER, LINE_FOLDER, NAME_PREFIX, lines);
        Debug.Log($"[Act1SceneDGenerator] Built '{NAME_PREFIX}_Sequence' with {lines.Length} entries.");
    }
}