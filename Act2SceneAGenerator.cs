// ==========================================
// Act2SceneAGenerator - Act 2 Scene A Content (The Weight of Silence)
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEditor;
using UnityEngine;

public static class Act2SceneAGenerator
{
    // ==========================================
    // Output Path Configuration
    // ==========================================
    private const string SEQUENCE_FOLDER = "Assets/Data/Act2";
    private const string LINE_FOLDER = "Assets/Data/Act2/Lines";
    private const string NAME_PREFIX = "Act2SceneA";

    // ==========================================
    // Generate - Build Scene A DialogueLine/Sequence Assets
    // ==========================================
    [MenuItem("Frayed Red String/Act 2/Generate Scene A - The Weight of Silence")]
    public static void Generate()
    {
        DialogueAssetGenerator.LineDef[] lines = new DialogueAssetGenerator.LineDef[]
        {
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                "The next day. Afternoon light stretches long across the corridor floor, gold catching on drifting dust. Outside the windows, the last cherry blossoms of the season let go, one by one, toward nothing in particular."
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "...Haru-kun? You've been quiet since lunch. Is something wrong?",
                state: "Shy"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "Hm? Oh — sorry. I didn't mean to worry you. It's nothing, really.",
                state: "Pokerface"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "It doesn't have to be nothing. You can tell me, you know. I'd like to know — the things that sit heavy on you.",
                state: "HappyBlush"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "...There was someone. A friend. We met in second grade — stuck together through everything, all the way to ninth.",
                state: "SadImploring"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "Something happened to him, somewhere along the way. He went quiet — the kind of quiet that isn't really quiet, if you're paying attention. I wasn't. Not enough.",
                state: "SadImploring"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "I tried. Every day, I tried to get him to talk to me. But by tenth grade, whatever door I was knocking on was already locked from the inside.",
                state: "CryingEyesClosed"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "He left. Not transferred — left. Completely. And the worst part is I keep replaying every conversation, looking for the one moment I could have said something different.",
                state: "CryingEyesClosed"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "So... if you're ever going through something. Anything. Please don't go quiet on me. Promise me that much.",
                state: "SadImploring"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "...I promise, Haru-kun. I won't ever go quiet. Not with you.",
                state: "HappyBlush"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "Grief leaves a door propped open. He's been standing in that doorway for years, waiting for someone to walk through and tell him it wasn't his fault. I just have to make sure I'm the only one who ever does.",
                state: "InsaneSmile",
                innerMonologue: true,
                gameEvent: "FriendTraumaConfessed"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                "Somewhere down the hall, the bell echoes for the stragglers. Haru wipes his eyes and laughs, embarrassed — and Yua laughs with him, softly, already three steps ahead."
            ),
        };

        DialogueAssetGenerator.BuildSequence(SEQUENCE_FOLDER, LINE_FOLDER, NAME_PREFIX, lines);
        Debug.Log($"[Act2SceneAGenerator] Built '{NAME_PREFIX}_Sequence' with {lines.Length} entries.");
    }
}