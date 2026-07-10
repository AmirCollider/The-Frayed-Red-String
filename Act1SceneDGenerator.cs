// ==========================================
// Act1SceneDGenerator - Act 1 Scene D Content (Night Walk)
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
    [MenuItem("Frayed Red String/Act 1/Generate Scene D - Night Walk")]
    public static void Generate()
    {
        DialogueAssetGenerator.GeneratorEntry[] entries = new DialogueAssetGenerator.GeneratorEntry[]
        {
            // Line 1 — Narration + Set Night Street Background (Value 13)
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                    en: "Leaving the train station, the twilight fades into a quiet, starry night. The neighborhood is wrapped in a comforting silence.",
                    jp: "駅を出ると、夕暮れは静かな星空へと移り変わっていく。住宅街は心地よい静けさに包まれていた。",
                    bg: BackgroundID.PastelStreetVendingNight,
                    bgTrans: BackgroundTransitionMode.Crossfade
                )
            ),

            // Line 2 — Haru expresses his gratitude
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                    en: "Thanks for walking me all the way here, Yua-pi. Today was so intense and full of anxiety... but I'm glad it ended like this.",
                    jp: "ここまで一緒に歩いてくれてありがとう、ユアぴ。今日は緊張してばかりで本当に疲れたけど……でも、こんな風に一日を終えられて良かったよ。",
                    state: "Shy"
                )
            ),

            // Line 3 — Yua smiles and normalizes it
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                    en: "Silly Haru-pi! We're neighbors, of course we walk home together. Plus, I like seeing you look so relaxed.",
                    jp: "もう、大げさだなあハルぴ！近所なんだから一緒に帰るの当たり前でしょ。それに、ハルぴがホッとした顔になるのを見るのが好きなんだから。",
                    state: "HappyBlush"
                )
            ),

            // Line 4 — Yua's manipulative internal thought
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                    en: "Look at him... completely letting his guard down. He honestly thinks this is just a sweet, innocent high school romance. How beautifully naive...",
                    jp: "（ふふ、完全に油断しちゃって。これがただの甘くて純粋な高校のロマンスだとでも思ってるのかな。なんて愛おしいほどに無邪気なんだろう）",
                    state: "WaitingToTalk",
                    innerMonologue: true
                )
            ),

            // Branch Entry — Two-button choice for the player
            DialogueAssetGenerator.GeneratorEntry.FromBranch(
                "SceneD_NightWalk_Choice",
                new DialogueAssetGenerator.BranchChoiceDef[]
                {
                    // OPTION 1: Tease him playfully
                    new DialogueAssetGenerator.BranchChoiceDef(
                        "Tease him playfully.", "からかってみる。", 1,
                        new DialogueAssetGenerator.LineDef[]
                        {
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                                en: "But if your hands start shaking in class again tomorrow, I'll just stand up and hold them tightly for everyone to see, okay?",
                                jp: "でも、明日またクラスで手が震え出したら、私が立ち上がってみんなの前で手をギュって握ってあげる。いい？",
                                state: "HappyBlush"
                            ),
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                                en: "Oh no, please don't do that! But... thanks, Yua-pi. Your jokes always make me feel so much better.",
                                jp: "うわあ、それだけは本当に勘弁してよ！……でも、ありがとうユアぴ。君의冗談にはいつも救われるよ。",
                                state: "Shy"
                            )
                        }
                    ),

                    // OPTION 2: Step closer to him
                    new DialogueAssetGenerator.BranchChoiceDef(
                        "Step closer to him.", "そっと近づく。", 2,
                        new DialogueAssetGenerator.LineDef[]
                        {
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                                en: "Then, to make sure you'll be fine tomorrow too, you have to promise to always stay this close to me.",
                                jp: "じゃあ、明日もハルぴが大丈夫なように、これからもずっと私の近くにいるって約束してね。",
                                state: "Shy"
                            ),
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                                en: "Yeah... I promise. Honestly, I don't think I could even go anywhere without you anymore.",
                                jp: "うん……約束するよ。正直、もうユアぴが隣にいてくれないと、どこにも行けない気がしね。",
                                state: "HappyBlush"
                            )
                        }
                    )
                }
            ),

            // Line 5 — Arriving at Haru's gate
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                    en: "Well, this is my house. I'll be waiting for you at the gate tomorrow morning, Yua-pi. Goodnight!",
                    jp: "よし、僕の家に着いたね。明日の朝、門の前で待ってるよ、ユアぴ。おやすみ！",
                    state: "HappyBlush"
                )
            ),

            // Line 6 — Yua's parting words
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                    en: "Goodnight, Haru-pi! Sleep well, and make sure to dream of me!",
                    jp: "おやすみ、ハルぴ！ゆっくり休んで、私の夢を見てね！",
                    state: "HappyBlush"
                )
            ),

            // Line 7 — Narration (Haru exits)
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                    en: "Haru walks towards his house with a peaceful smile, gradually disappearing into the night.",
                    jp: "ハルは安心しきった笑顔で家へと歩いていき、夜の闇の中に消えていった。"
                )
            ),

            // Line 8 — Yua's dark final monologue (Corrected: Stopped at "choke him in the end")
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                    en: "He's completely trapped. Every ounce of his trust is now in my hands. The poor thing has no idea how this sweet, innocent bond is going to choke him in the end.",
                    jp: "（ふふ、完全に罠にかかった。あの子の信頼はすべて私の手の中。この甘くて純粋な関係が、最後にどうやってあの子の首を絞めることになるのか、夢にも思わないでしょうね……）",
                    state: "Pokerface",
                    innerMonologue: true
                )
            ),

            // Outro Title Card
            DialogueAssetGenerator.GeneratorEntry.FromLine(
    DialogueAssetGenerator.LineDef.TitleCard(
        headingEN: "The First Knot: Fading into the Quiet Night",
        bodyEN:    "Even the sweetest dreams must eventually awaken to the dark.",
        headingJP: "最初の結び目：静寂の夜へ",
        bodyJP:    "どんなに甘い夢も、やがて闇の中で目覚めなければならない。",
        autoAdv:   true,
                    autoAdvDelay: 3f
                )
)
        };

        DialogueSequence seq = DialogueAssetGenerator.BuildSequence(SEQUENCE_FOLDER, LINE_FOLDER, NAME_PREFIX, entries);
        Debug.Log($"[Act1SceneDGenerator] Built '{NAME_PREFIX}_Sequence' with {seq.Count} entries at {SEQUENCE_FOLDER}.");
    }
}
