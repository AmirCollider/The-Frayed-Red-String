// ==========================================
// Act1SceneCGenerator - Act 1 Scene C Content (Train Platform Sunset)
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
    // Generate - Build Scene C DialogueLine/Sequence Assets
    // ==========================================
    [MenuItem("Frayed Red String/Act 1/Generate Scene C - Train Platform Sunset")]
    public static void Generate()
    {
        DialogueAssetGenerator.GeneratorEntry[] entries = new DialogueAssetGenerator.GeneratorEntry[]
        {
            // Line 1 — Narration + Set Train Platform Sunset Background
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                    en: "The golden autumn sunset bathes the quiet train platform. The bustling noise of the first day of high school gradually fades away, leaving behind a warm, nostalgic breeze.",
                    jp: "夕暮れ時の静かな駅のホーム。放課後の心地よい風が、二人の間を通り抜けていく。高校生活初日の賑やかさは、次第に遠ざかっていく。",
                    bg: BackgroundID.TrainPlatformSunset,
                    bgTrans: BackgroundTransitionMode.Crossfade
                )
            ),

            // Line 2 — Haru expresses his utter exhaustion
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                    en: "Wow, I am completely exhausted. I feel like I used up an entire month's worth of energy just during that one homeroom session.",
                    jp: "はぁ…本当に疲れたよ。あのホームルームの時間だけで、一ヶ月分くらいエネルギーを使い果たした気がする。",
                    state: "Shy"
                )
            ),

            // Line 3 — Yua teases him playfully
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                    en: "Giggle... You're exaggerating, Haru-pi! Was it really that terrifying to just stand up and say your name?",
                    jp: "クスクス、大げさだなあ、ハルぴ！ただ立ち上がって自分の名前を言うのが、そんなに怖かったの？",
                    state: "HappyBlush"
                )
            ),

            // Line 4 — Haru explains his anxiety about the others
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                    en: "It really was! Didn't you see everyone else? They all looked so confident and mature, speaking without a single stutter. My hands wouldn't stop shaking when the teacher called my name.",
                    jp: "本当だよ！周りのみんなを見てよ。みんなすごく堂々として大人っぽく見えたし、噛まずに話してた。先生に名前を呼ばれた時、手の震えが止まらなかったんだ。",
                    state: "Shy"
                )
            ),

            // Line 5 — Yua brings up the window seats success
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                    en: "But hey, our strategy worked perfectly! We managed to get those two window seats side-by-side. That part went smoothly, right?",
                    jp: "でも、私たちの作戦は大成功だったでしょ？窓側の席を並んで取れたんだもん。そこは上手くいったよね？",
                    state: "HappyBlush"
                )
            ),

            // Line 6 — Haru expresses intense gratitude
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                    en: "Yeah, that was a lifesaver. Seriously, if I had been forced to sit next to a stranger today, I probably would have had a panic attack and passed out. I'm so grateful you moved so fast, Yua-pi.",
                    jp: "うん、本当に助かったよ。もし知らない人の隣だったら、緊張で一日中倒れてたかもしれない。ユアぴが素早く動いてくれて、感謝しかないよ。",
                    state: "HappyBlush"
                )
            ),

            // Line 7 — Yua's manipulative inner monologue
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                    en: "Look at him, trembling over something so minor. My assigned seats are his only safe haven in this world. Keep leaning on me, Haru-pi... until you can't even breathe without my permission.",
                    jp: "（ふふ、そんなに怯えて。私の用意した席だけが、この子の唯一の安全地帯なんだ。もっと私に依存して、私なしでは息もできないくらいにね、ハルぴ）",
                    state: "WaitingToTalk",
                    innerMonologue: true
                )
            ),

            // Branch Entry — Two-button romantic choice for the player
            DialogueAssetGenerator.GeneratorEntry.FromBranch(
                "SceneC_Station_Choice",
                new DialogueAssetGenerator.BranchChoiceDef[]
                {
                    // OPTION 1: Wipe a stray petal from his hair
                    new DialogueAssetGenerator.BranchChoiceDef(
                        "Wipe a stray petal from his hair.", "髪の花びらを取ってあげる。", 1,
                        new DialogueAssetGenerator.LineDef[]
                        {
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                                en: "Hold still for a second, Haru-pi. You still have a stray cherry blossom petal caught in your hair from this morning... There, got it.",
                                jp: "ほら、ハルぴ動かないで。今朝の桜の花びらが、まだ髪に残ってるよ。……はい、取れた。",
                                state: "Shy"
                            ),
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                                en: "Ah... y-you're so close, Yua-pi... But, thank you. You always notice the tiny things I completely miss.",
                                jp: "あっ…ち、近いよユアぴ…。で、でも、ありがとう。君はいつも、僕が気づかない小さなことまで見てるんだね。",
                                state: "Shy"
                            )
                        }
                    ),

                    // OPTION 2: Buy him a warm drink
                    new DialogueAssetGenerator.BranchChoiceDef(
                        "Buy him a warm drink.", "温かい飲み物を買ってあげる。", 2,
                        new DialogueAssetGenerator.LineDef[]
                        {
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                                en: "Here, take this! I bought a warm can of cocoa from the vending machine over there. Consider it a reward for surviving day one!",
                                jp: "はい、これ。自販機で温かいココア買ってきたよ。頑張ったハルぴへのご褒美！",
                                state: "HappyBlush"
                            ),
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                                en: "Wow, it's so warm... Thank you, Yua-pi. Holding this feels like it's melting away all the cold anxiety inside my chest.",
                                jp: "わあ、温かい…。ありがとう、ユアぴ。これを持ってるだけで、胸の中の冷たい緊張が溶けていくみたいだ。",
                                state: "HappyBlush"
                            )
                        }
                    )
                }
            ),

            // Line 8 — Convergence: Main plot line continues after choice resolution
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                    en: "Thinking about it now... I guess going to school tomorrow won't be so bad. Because I already know that when I walk into that room, you'll be sitting right next to me.",
                    jp: "そう考えると…明日学校に行くのは、少し怖くないかもしれない。教室に入れば、ユアぴが隣にいてくれるって分かってるからね。",
                    state: "HappyBlush"
                )
            ),

            // Line 9 — Yua demands absolute possession of the seat
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                    en: "Mmhmm! Tomorrow, the day after, and forever... the seat right next to Haru-pi belongs entirely to me. Don't you dare give it to anyone else!",
                    jp: "うん！明日も、明後日も、ずっとハルぴの隣は私の特等席だからね。他の誰にもあげちゃダメだよ？",
                    state: "Shy"
                )
            ),

            // Line 10 — Haru completely surrenders to her presence
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                    en: "I wouldn't dream of it. I really don't know what I would do if you weren't by my side, Yua-pi.",
                    jp: "もちろんさ。ユアぴが隣にいてくれないと、僕は本当にどうしていいか分からなくなっちゃうから。",
                    state: "HappyBlush"
                )
            ),

            // Line 11 — Closing Narration (Train arrives, establishing smooth transition for Scene D)
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                    en: "A distant rumble echoes as the train approaches, its bright headlights cutting through the darkening twilight. The doors slide open, inviting them into the carriage for the ride home.",
                    jp: "遠くからガタゴトと電車の音が響き、明かりがホームを照らす。家路へと向かう電車のドアが静かに開き、二人を迎え入れた。"
                )
            )
        };

        DialogueSequence seq = DialogueAssetGenerator.BuildSequence(SEQUENCE_FOLDER, LINE_FOLDER, NAME_PREFIX, entries);
        Debug.Log($"[Act1SceneCGenerator] Built '{NAME_PREFIX}_Sequence' with {seq.Count} entries at {SEQUENCE_FOLDER}.");
    }
}
