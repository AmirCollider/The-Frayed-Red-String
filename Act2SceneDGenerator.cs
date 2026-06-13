// ==========================================
// Act2SceneDGenerator - Act 2 Scene D Content (The Stare)
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEditor;
using UnityEngine;

public static class Act2SceneDGenerator
{
    // ==========================================
    // Output Path Configuration
    // ==========================================
    private const string SEQUENCE_FOLDER = "Assets/Data/Act2";
    private const string LINE_FOLDER     = "Assets/Data/Act2/Lines";
    private const string NAME_PREFIX     = "Act2SceneD";

    // ==========================================
    // Generate - Build Scene D DialogueLine/Sequence Assets
    // ==========================================
    [MenuItem("Frayed Red String/Act 2/Generate Scene D - The Stare")]
    public static void Generate()
    {
        DialogueAssetGenerator.LineDef[] lines = new DialogueAssetGenerator.LineDef[]
        {
            // ==========================================
            // L01 — Scene Opening: Haru Alone, That Night
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                "That night. Haru sits alone in his room. The city noise outside has faded to a low hum. His phone is face-down on the desk."
            ),

            // ==========================================
            // L02 — Haru's Introspection Begins
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "...She's always there. Every time I almost surface for air, she's there.",
                state: "Pokerface"
            ),

            // ==========================================
            // L03 — Haru Recognises His Own Dependency
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "I told her everything. Things I've never said out loud to anyone. And I keep finding myself... needing her more because of it.",
                state: "SadImploring"
            ),

            // ==========================================
            // L04 — Transition Beat: Fires BeginFourthWallBreak
            //        Auto-advances to give FourthWallBreaker time to engage
            //        (camera zoom, music fade, tears in, address style, Yua hidden, Haru → Center)
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "[long pause] ...I don't know if I'm grateful. Or if something else is happening.",
                state: "Pokerface",
                autoAdv: true,
                autoAdvDelay: 2.0f,
                gameEvent: "BeginFourthWallBreak"
            ),

            // ==========================================
            // L05 — Fourth-Wall: The Friend Parallel
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Center,
                "You know... I tried everything for him. Every day I knocked on that door. And then one day it just — wasn't there anymore.",
                state: "SadImploring",
                fourthWall: true
            ),

            // ==========================================
            // L06 — Fourth-Wall: The Pattern Repeating
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Center,
                "I can feel it happening again. The same silence. The same distance forming behind someone's eyes when they think I'm not looking.",
                state: "SadImploring",
                fourthWall: true
            ),

            // ==========================================
            // L07 — Fourth-Wall: Direct Acknowledgement of the Player
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Center,
                "You're watching all of this. You've been here the whole time. So let me ask you directly.",
                state: "SadImploring",
                fourthWall: true
            ),

            // ==========================================
            // L08 — Fourth-Wall: The Plea
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Center,
                "If it ever gets too heavy — if the world starts closing in — don't go quiet. Don't let it build until there's no door left to knock on. I couldn't live through that again.",
                state: "SadImploring",
                fourthWall: true
            ),

            // ==========================================
            // L09 — Fourth-Wall: Final Warning to the Player
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Center,
                "You're part of this story too. Every choice you've made here has weight. Don't forget that.",
                state: "Pokerface",
                fourthWall: true
            ),

            // ==========================================
            // L10 — Transition Beat: Fires EndFourthWallBreak
            //        Auto-advances so the camera, tears and address style restore
            //        before the final narration line displays
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                "He turns back to his desk. The space behind him holds for a moment — weightless, overlong.",
                autoAdv: true,
                autoAdvDelay: 2.0f,
                gameEvent: "EndFourthWallBreak"
            ),

            // ==========================================
            // L11 — Scene Closing Narration: Milestone HaruFullyIsolated
            // ==========================================
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                "Outside, the city keeps moving — indifferent, unaware, already forgetting.",
                gameEvent: "HaruFullyIsolated"
            ),
        };

        DialogueAssetGenerator.BuildSequence(SEQUENCE_FOLDER, LINE_FOLDER, NAME_PREFIX, lines);
        Debug.Log($"[Act2SceneDGenerator] Built '{NAME_PREFIX}_Sequence' with {lines.Length} entries.");
    }
}
