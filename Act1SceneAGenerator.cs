// ==========================================
// Act1SceneAGenerator - Act 1 Scene A Content (Schoolyard Encounter)
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEditor;
using UnityEngine;

public static class Act1SceneAGenerator
{
    private const string SEQUENCE_FOLDER = "Assets/Data/Act1";
    private const string LINE_FOLDER = "Assets/Data/Act1/Lines";
    private const string NAME_PREFIX = "Act1SceneA";

    [MenuItem("Frayed Red String/Act 1/Generate Scene A - Schoolyard Encounter")]
    public static void Generate()
    {
        DialogueAssetGenerator.GeneratorEntry[] entries = new DialogueAssetGenerator.GeneratorEntry[]
        {
            // Intro Title Card
            DialogueAssetGenerator.GeneratorEntry.FromLine(
    DialogueAssetGenerator.LineDef.TitleCard(
        headingEN: "A Crimson Thread in a Pink Spring",
        bodyEN:    "Under the cherry blossoms, our story entangles once more.",
        headingJP: "桃色の春、紅い糸",
        bodyJP:    "桜の下で、私たちの物語がまた絡み合う。",
        autoAdv:   true,
                    autoAdvDelay: 3f
                )
),

            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                    "Wow... the cherry blossoms have turned the whole schoolyard pink. It's beautiful.",
                    jp: "わあ…桜が校庭を真っピンクに染めてる。本当に綺麗。", // اصلاح شد: 染めてろ -> 染めてる
                    state: "WaitingToTalk",
                    bg: BackgroundID.CherryBlossomAlley,
                    bgTrans: BackgroundTransitionMode.Crossfade,
                    innerMonologue: true
                )
            ),

            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                    "...There he is, Haru-pi is standing under the tree. He's totally lost in watching the petals.",
                    jp: "…いた。ハルぴ、あそこの木の下に立ってる。花びらに夢中になっちゃって。",
                    state: "WaitingToTalk",
                    innerMonologue: true
                )
            ),

            DialogueAssetGenerator.GeneratorEntry.FromBranch(
                "SceneA_Approach_Choice",
                new DialogueAssetGenerator.BranchChoiceDef[]
                {
                    new DialogueAssetGenerator.BranchChoiceDef(
                        "Surprise him from behind.", "後ろからびっくりさせる。", 1,
                        new DialogueAssetGenerator.LineDef[]
                        {
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                                "Baa! Haru-pi, what are you looking at?",
                                jp: "ばあ！ハルぴ、何を見てるの？",
                                state: "HappyBlush"
                            ),
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                                "Waa...! Geez, Yua-pi, don't scare me like that... I thought my heart stopped.",
                                jp: "わっ…！もう、ユアぴ、驚かさないでよ…心臓が止まるかと思った。",
                                state: "Shy"
                            )
                        }
                    ),
                    new DialogueAssetGenerator.BranchChoiceDef(
                        "Gently step up next to him.", "そっと隣に並ぶ。", 1,
                        new DialogueAssetGenerator.LineDef[]
                        {
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                                "Beautiful cherry blossoms, right, Haru-pi? Mind if I join you?",
                                jp: "きれいな桜だね、ハルぴ。隣、いい？",
                                state: "Shy"
                            ),
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                                "Ah, Yua-pi... yeah, of course. Let's watch together.",
                                jp: "あっ、ユアぴ…うん、もちろん。一緒に見よう。",
                                state: "Shy"
                            )
                        }
                    )
                }
            ),

            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                    "High school life is finally starting. Nervous?",
                    jp: "いよいよ高校生活が始まるね。緊張してる？",
                    state: "HappyBlush"
                )
            ),

            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                    "Hmm, a little. But if Yua-pi is next to me, I feel like I'll be fine.",
                    jp: "うーん、ちょっとね。でも、ユアぴが隣にいてくれれば大丈夫な気がする。",
                    state: "Shy"
                )
            ),

            DialogueAssetGenerator.GeneratorEntry.FromBranch(
                "SceneA_Reassure_Choice",
                new DialogueAssetGenerator.BranchChoiceDef[]
                {
                    new DialogueAssetGenerator.BranchChoiceDef(
                        "Hold his hand tightly.", "手をぎゅっと握る。", 2,
                        new DialogueAssetGenerator.LineDef[]
                        {
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                                "See? Safe now, right?",
                                jp: "ほら、これで安心でしょ？",
                                state: "HappyBlush"
                            ),
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                                "... Thanks. Your hand is warm, Yua-pi.",
                                jp: "っ…ありがとう。ユアぴの手、あったかいね。",
                                state: "HappyBlush"
                            )
                        }
                    ),
                    new DialogueAssetGenerator.BranchChoiceDef(
                        "Give him a warm smile.", "優しく微笑む。", 1,
                        new DialogueAssetGenerator.LineDef[]
                        {
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                                "I'm right here with you, so no need to worry.",
                                jp: "私がついてるんだから、心配いらないよ。",
                                state: "HappyBlush"
                            ),
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                                "Yeah, when I see that smile, I can finally breathe easy.",
                                jp: "うん、その笑顔を見ると、ホッとするよ。",
                                state: "HappyBlush"
                            )
                        }
                    )
                }
            ),

            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                    "The first school bell echoed through the yard. Time to head inside the hall.",
                    jp: "最初のチャイムが校庭に響いた。そろそろ、中に入らなきゃ。"
                )
            ),

            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                    "Ah, the bell! Let's go, Haru-pi. We'll be late on our first day.",
                    jp: "あ、チャイムだ！行こう、ハلぴ。初日から遅れちゃう。",
                    state: "HappyBlush"
                )
            ),

            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                    "Yeah, let's go. I don't want to be late on day one.",
                    jp: "うん、いこう。初日から遅刻はしたくないしね。",
                    state: "HappyBlush"
                )
            ),

            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                    "Walking shoulder to shoulder. Everything is going perfectly natural and sweet... exactly as planned.",
                    jp: "肩を並べて歩き出す。すべては自然に、甘く進んでる…計画通りにね。"
                )
            )
        };

        DialogueSequence seq = DialogueAssetGenerator.BuildSequence(SEQUENCE_FOLDER, LINE_FOLDER, NAME_PREFIX, entries);
        Debug.Log($"[Act1SceneAGenerator] Built '{NAME_PREFIX}_Sequence' with {seq.Count} entries at {SEQUENCE_FOLDER}.");
    }
}