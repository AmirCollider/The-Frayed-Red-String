// ==========================================
// Act2SceneBGenerator - Act 2 Scene B Content (Seven Candles)
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEditor;
using UnityEngine;

public static class Act2SceneBGenerator
{
    // ==========================================
    // Output Path Configuration
    // ==========================================
    private const string SEQUENCE_FOLDER = "Assets/Data/Act2";
    private const string LINE_FOLDER = "Assets/Data/Act2/Lines";
    private const string NAME_PREFIX = "Act2SceneB";

    // ==========================================
    // Generate - Build Scene B DialogueLine/Sequence Assets
    // ==========================================
    [MenuItem("Frayed Red String/Act 2/Generate Scene B - Seven Candles")]
    public static void Generate()
    {
        DialogueAssetGenerator.LineDef[] lines = new DialogueAssetGenerator.LineDef[]
        {
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                "A few days pass. After school, the bell above Usagi Bakery's café door chimes softly as they step inside — pastel walls, the low hum of conversation, the smell of butter and warm sugar.",
                bg: BackgroundID.CozyCafe, bgTrans: BackgroundTransitionMode.Crossfade
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "Look — they've put little candle charms on the seasonal cakes this month. Aren't they sweet?",
                state: "HappyBlush"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "...Yeah. Sweet.",
                state: "Pokerface"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "...Haru-kun? You just went somewhere. Your eyes did, anyway.",
                state: "Shy"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "It's nothing. Just an old memory. Birthdays aren't really my thing — haven't been for a long time.",
                state: "Annoyed"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "You don't have to say anything. But you don't have to carry it alone either. Not while I'm sitting right here.",
                state: "HappyBlush"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "...My seventh birthday. We were driving out to pick up a cake — strawberry shortcake, the kind with seven little candles on top. I'd asked for it for weeks.",
                state: "SadImploring"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "It was raining. I remember the wipers, and my mom humming something, and then — headlights. Too bright, too close. Then just noise. Metal, and glass, and then nothing at all.",
                state: "CryingEyesClosed"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "I was the only one who walked away from it. Seven years old, standing on the side of the road in the rain, and somehow already understanding the word 'alone' better than kids twice my age.",
                state: "CryingEyesClosed"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "The cake never got picked up. The candles never got lit. I've never told anyone the whole thing before — just pieces, here and there, to people who didn't really want to hear it.",
                state: "SadImploring"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "...Haru-kun. That was never something a seven-year-old could have stopped. None of it was yours to carry. Not then, and not now.",
                state: "HappyBlush"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                "...Thank you. I don't really understand it, but — it's a little easier to breathe, telling you.",
                state: "Shy"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                "Two wounds now, both still open, both facing the same direction — toward me. A boy who learns that the ache gets quieter near my voice will keep coming back to it. I just have to make sure no other voice ever gets the chance to compete.",
                state: "InsaneSmile",
                innerMonologue: true,
                gameEvent: "FamilyTraumaConfessed"
            ),
            new DialogueAssetGenerator.LineDef(
                ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                "Outside, the streetlights flicker on early against the gray afternoon. Inside, the cake in the display case still has its little candle charms — unlit, and exactly seven."
            ),
        };

        DialogueAssetGenerator.BuildSequence(SEQUENCE_FOLDER, LINE_FOLDER, NAME_PREFIX, lines);
        Debug.Log($"[Act2SceneBGenerator] Built '{NAME_PREFIX}_Sequence' with {lines.Length} entries.");
    }
}