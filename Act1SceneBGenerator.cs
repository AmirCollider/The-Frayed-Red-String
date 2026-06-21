// ==========================================
// Act1SceneBGenerator - Act 1 Scene B Content (Classroom Introduction)
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
    [MenuItem("Frayed Red String/Act 1/Generate Scene B - Classroom Introduction")]
    public static void Generate()
    {
        DialogueAssetGenerator.GeneratorEntry[] entries = new DialogueAssetGenerator.GeneratorEntry[]
        {
            // Line 1 — Narration + Set Classroom Background
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                    en: "The classroom is filled with the lively chatter of unfamiliar faces and the rustling of new uniforms. Moving swiftly through the crowd, Yua effortlessly secures two desks side-by-side right next to the window.",
                    jp: "教室は知らない顔ばかりで、新しい制服の擦れる音と賑やかな雑音に包まれている。ユアぴは人混みをすんなりとすり抜け、窓際の特等席を２つ、いとも簡単に確保した。",
                    bg: BackgroundID.SunnyClassroomDay,
                    bgTrans: BackgroundTransitionMode.Crossfade
                )
            ),

            // Line 2 — Haru expresses his anxiety
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                    en: "Wow, Yua-pi... you moved so fast. I was completely frozen just looking at how many people are in this room. My chest actually feels a bit tight... Thanks for saving a spot for me.",
                    jp: "わあ、ユアぴ…動くの早いね。僕はこの部屋にいる人の多さを見ただけで、完全に圧倒されて固まっちゃってたよ。なんだか胸が少し苦しくて…席を取っておいてくれて本当にありがとう。",
                    state: "Shy"
                )
            ),

            // Line 3 — Yua reassures him with a team dynamic
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                    en: "Hehe, of course! Leave the strategy and planning to me. There's absolutely no way I'd let anyone else sit next to my Haru-pi on our very first day of high school. We're a team, remember?",
                    jp: "へへ、当然だよ！作戦と計画は私に任せて。高校生活の最初の日に、他の誰かをハルぴの隣に座らせるわけないじゃん。私達はチームでしょ、忘れたの？",
                    state: "HappyBlush"
                )
            ),

            // Line 4 — Haru worries about self-introductions
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                    en: "Haha, yeah, we are... But look at everyone else. They already seem so grown-up and confident, talking to each other like it's nothing. My hands are sweating just thinking about the self-introductions later... What if I stutter and ruin it?",
                    jp: "はは、うん、チームだね…。でも周りのみんなを見てよ。もうすごく大人っぽくて自信満々に見えるし、何事もないみたいに楽しそうに話してる。この後の自己紹介のことを考えると、手に汗を握っちゃうな…もし噛んじゃって、台無しにしたらどうしよう。",
                    state: "Shy"
                )
            ),

            // Line 5 — Yua's inner monologue (manipulative / controlling nature)
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                    en: "Look at how anxious he gets over something so small... Look at how his eyes desperately search for my approval. You're so fragile, Haru-pi... so beautifully weak. Just keep leaning on me. Don't look at anyone else in this room. You only need me.",
                    jp: "（震えてる…なんて脆くて、愛おしいほどに弱い。私だけを見ていてね、ハルぴ。他の誰のことも見ちゃダメだよ。君には私だけでいいの）",
                    state: "WaitingToTalk",
                    innerMonologue: true
                )
            ),

            // Branch Entry — Two-button choice for the player[cite: 1, 2]
            DialogueAssetGenerator.GeneratorEntry.FromBranch(
                "SceneB_Classroom_Choice",
                new DialogueAssetGenerator.BranchChoiceDef[]
                {
                    // OPTION 1: Tease him playfully[cite: 2]
                    new DialogueAssetGenerator.BranchChoiceDef(
                        "Tease him playfully.", "からかってみる。", 1,
                        new DialogueAssetGenerator.LineDef[]
                        {
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                                en: "If your voice shakes, I'll just jump up from my seat and shout, 'He's mine, so don't scare him!' How about that? That'll definitely distract them!",
                                jp: "もし声が震えちゃったら、私が席から立ち上がって『この子は私のものだから、脅かさないで！』って叫んであげようか？どう？それなら絶対にみんなの気を引けるよ！",
                                state: "HappyBlush"
                            ),
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                                en: "W-Wait, please don't do that! That would be a hundred times more embarrassing than stuttering! But... haha, thanks, Yua-pi. I feel a bit more relaxed now.",
                                jp: "ちょ、ちょっと、それだけは本当にやめてよ！噛むより百倍恥ずかしいから！…でも、はは、ありがとうユاぴ。少し緊張がほぐれた気がするよ。",
                                state: "Shy"
                            )
                        }
                    ),

                    // OPTION 2: Comfort him warmly[cite: 2]
                    new DialogueAssetGenerator.BranchChoiceDef(
                        "Comfort him warmly.", "優しく励ます。", 2,
                        new DialogueAssetGenerator.LineDef[]
                        {
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_YUA, CharacterPosition.Left,
                                en: "No matter what happens, just look straight at me and ignore the rest. I'll be right here next to you. If you need me to, I can even hold your hand under the desk to keep you safe.",
                                jp: "何があっても、まっすぐ私だけを見て、他の人は気にしないで。私がすぐ隣にいるから。もし必要なら、机の下で手を握っててあげる。安心できるでしょ？",
                                state: "Shy"
                            ),
                            new DialogueAssetGenerator.LineDef(
                                ConstantsConfig.SPEAKER_HARU, CharacterPosition.Right,
                                en: "Yua-pi... thank you. Seriously, just hearing you say that makes the pounding in my chest slow down. I really don't know what I'd do without you by my side.",
                                jp: "ユアぴ…ありがとう。本当に、その言葉を聞くだけで胸の激しいバクバクが収まっていくよ。君が隣にいてくれないと、僕はどうしたらいいか分からなくなっちゃうな。",
                                state: "HappyBlush"
                            )
                        }
                    )
                }
            ),

            // Line 6 — Closing Narration (Teacher arrives)[cite: 2]
            DialogueAssetGenerator.GeneratorEntry.FromLine(
                new DialogueAssetGenerator.LineDef(
                    ConstantsConfig.SPEAKER_SYSTEM, CharacterPosition.Center,
                    en: "Right at that moment, the heavy classroom door slides open with a sharp rattle. The teacher steps inside, and a sudden silence falls over the room. The first homeroom session officially begins.",
                    jp: "その瞬間、ガラガラと鋭い音を立てて教室の重いドアが開いた。先生が中に入ってくると、部屋は一瞬にして静まり返った。最初のホームルームが正式に始まる。"
                )
            )
        };

        DialogueSequence seq = DialogueAssetGenerator.BuildSequence(SEQUENCE_FOLDER, LINE_FOLDER, NAME_PREFIX, entries);
        Debug.Log($"[Act1SceneBGenerator] Built '{NAME_PREFIX}_Sequence' with {seq.Count} entries at {SEQUENCE_FOLDER}.");
    }
}