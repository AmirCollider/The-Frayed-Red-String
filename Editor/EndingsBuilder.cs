// -----------------------------------------------------------------------------
//  The Frayed Red String
//  EndingsBuilder.cs  (Editor only)
//
//  The two endings nobody is told about.
//
//  Run from The Frayed Red String ▸ Endings ▸ Build The Two Endings. It writes
//  Assets/Story/Acts/Ending_Good.asset and Ending_Normal.asset, reusing anything
//  already at those paths.
//
//  Neither of these is act eight. They have no act number and no scene, because
//  they do not come after anything — the design document is explicit that a
//  player might reach one from act three, or four, or wherever they happened to
//  be sitting when five real minutes went by without them touching anything. So
//  an ending is an act that replaces whatever was going to happen next, and
//  StoryDirector plays it in place of the rest of the scene.
//
//  How the game decides which one it is offering:
//
//    • Every counted press was blue, and there was at least one → Yua speaks,
//      and it is the secret ending.
//    • Every counted press was green, and there was at least one → Haru speaks,
//      and it is the open one.
//    • Anything else — one of each, or none at all — and no offer appears. The
//      five minutes buy nothing and nothing says so.
//
//  That is the whole condition and it is never displayed. Pressing blue and
//  being refused by Yua still counts as blue: the endings are decided by what
//  the player reached for, not by what she allowed them to have.
//
//  Both are written at a rush. The design document asks for the long speeches
//  here to pour out rather than being typed, which is what the TypeSpeed on
//  those beats is doing: about three times the normal rate, so the lines arrive
//  faster than they can comfortably be read and land as somebody who has stopped
//  choosing their words.
//
//  NEITHER OF THESE ENDS ON THE CLOSING WORDS. The two sentences about decisions
//  staying in the world and five minutes being enough belong to the bitter
//  ending alone. A player who reached one of these two has already been shown
//  what listening was worth, by being handed an ending for it; saying it out
//  loud afterwards would explain a thing that had just been demonstrated. The
//  player who needs it is the one who watched Haru shoot himself.
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Narrative;
using UnityEditor;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Builds both endings.</summary>
    public static class EndingsBuilder
    {
        /// <summary>Characters per second for the two long speeches.</summary>
        public const float RushSpeed = 140f;

        /// <summary>Writes both ending assets.</summary>
        public static void BuildBoth()
        {
            BuildBoth(ActScriptWriter.RebuildPolicy.Ask);
        }

        /// <summary>Writes both ending assets under a given policy.</summary>
        public static void BuildBoth(ActScriptWriter.RebuildPolicy policy)
        {
            new NormalEndingBuilder().BuildAsset(policy);
            new GoodEndingBuilder().BuildAsset(policy);
        }

        // ---------------------------------------------------------------------
        //  The open ending — Haru says it and leaves
        // ---------------------------------------------------------------------

        /// <summary>
        /// What happens when the player reached for kindness at least once but
        /// not always.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The design document calls it the normal ending and it is the harder
        /// of the two to write, because it has to be a relief and a loss in the
        /// same breath. Nobody dies. Haru says the true thing out loud, for the
        /// first and only time, and then he goes — and Yua is left alive with
        /// exactly what she spent five acts building, which is a life with
        /// nobody in it who can be made to stay.
        /// </para>
        /// <para>
        /// He is angry here in a way he is nowhere else in the game, including
        /// act five. That is the point of the branch: in act five he is beyond
        /// it, and here he is not, and being still able to be angry is what
        /// saves both their lives.
        /// </para>
        /// </remarks>
        private sealed class NormalEndingBuilder : ActScriptWriter
        {
            protected override int ActNumber => 0;

            protected override string AssetName => Endings.NormalAssetName;

            protected override LocalizedLine Title =>
                L("The Open Ending", "開かれた結末", "پایان باز");

            protected override string MusicTrack => string.Empty;

            protected override void Write()
            {
                CutMusic();

                Hold(3.0f);

                Narrate(
                    "He had been holding a breath for about nine months, and he let it go.",
                    "九ヶ月ぶんの息を、彼はようやく吐いた。",
                    "حدودِ نُه ماه بود نفسش را نگه داشته بود، و رهایش کرد.");

                Hold(2.6f);

                Enter(Speaker.Haru, Portrait.Angry);
                Enter(Speaker.Yua, Portrait.Neutral);

                Hold(2.0f);

                Rush(Speaker.Haru, Portrait.Angry,
                    "I am upset with you. I am so upset with you.",
                    "怒ってる。……すごく怒ってるんだ。",
                    "ازت ناراحتم. خیلی ازت ناراحتم.");

                Rush(Speaker.Haru, Portrait.Unchanged,
                    "You break my heart. You have been breaking it in small pieces for months and doing it on purpose.",
                    "心が壊れる。何ヶ月もかけて、少しずつ、わざと壊してきたよね。",
                    "دلم ازت می‌شکنه. ماه‌هاست داری تکه‌تکه می‌شکنیش و عمداً هم داری این کار رو می‌کنی.");

                Hold(1.6f);

                Rush(Speaker.Haru, Portrait.Angry,
                    "You made yourself into a toy for an old pain you cannot put down.",
                    "手放せない古い痛みの、おもちゃに自分をした。",
                    "خودت رو بازیچه‌ی دردِ قدیمیت کردی که نمی‌تونی بی‌خیالش بشی.");

                Rush(Speaker.Haru, Portrait.Unchanged,
                    "Do you think this is what strong is called? Is that the word you have been using for it?",
                    "これを強いって呼んでるの？　その言葉を、ずっとこれに使ってきたの？",
                    "فکر کردی اسمِ این کاری که داری می‌کنی قوی بودنه؟ همین کلمه رو براش به کار می‌بردی؟");

                Rush(Speaker.Haru, Portrait.Angry,
                    "Handling somebody. Using somebody who has loved you since he was nine. You think that is strength?",
                    "人を操ること。九歳のときから好きでいる相手を使うこと。それが強さ？",
                    "دستکاری کردن. استفاده از کسی که از نُه‌سالگی عاشقته. فکر کردی این قوی بودنه؟");

                Hold(2.2f);

                Rush(Speaker.Haru, Portrait.Crying,
                    "I have been waiting this whole time for you to see it. To just see it.",
                    "ずっと待ってた。結愛ぴが気づくのを。ただ、気づくのを。",
                    "همه‌ی این مدت منتظر بودم که ببینیش. فقط ببینیش.");

                Rush(Speaker.Haru, Portrait.Unchanged,
                    "I tried everything. Everything. I even told the person watching us.",
                    "できることは全部やった。全部。……見てる人にまで言った。",
                    "دست به هر کاری زدم. هر کاری. حتی به کسی که داره نگاهمون می‌کنه هم گفتم!");

                Hold(3.0f);

                Say(Speaker.Yua, Portrait.DeadEyes,
                    "…",
                    "……",
                    "…");

                Hold(3.0f);

                Say(Speaker.Haru, Portrait.Sad,
                    "That is enough. I am tired. I am going.",
                    "……もういい。疲れた。行くよ。",
                    "بسه. خسته شدم. می‌رم.");

                Hold(2.6f);

                Say(Speaker.Haru, Portrait.Unchanged,
                    "You keep on being strong exactly like this. Keep it up.",
                    "そのまま、ずっと強いままでいなよ。",
                    "توام سعی کن همیشه همین‌طوری قوی بمونی.");

                Say(Speaker.Haru, Portrait.DeadEyes,
                    "You will find out what it was worth by what it costs you.",
                    "それが何だったのか、失うもので分かるよ。",
                    "نتیجه‌ش رو با از دست دادنت می‌بینی.");

                Hold(3.4f);

                Exit(Speaker.Haru);

                Hold(4.0f);

                Say(Speaker.Yua, Portrait.Neutral,
                    "Haru-pi.",
                    "ハルぴ。",
                    "هارو‌پی.");

                Hold(3.6f);

                Say(Speaker.Yua, Portrait.Unchanged,
                    "Haru-pi. Come back and I will not do it again.",
                    "ハルぴ。戻ってきて。もうしないから。",
                    "هارو‌پی. برگرد، دیگه این کار رو نمی‌کنم.");

                Hold(4.0f);

                Narrate(
                    "He did not come back. He lived, and she lived, and one of those two things was the first good outcome anybody in this story had managed.",
                    "彼は戻らなかった。彼は生きて、彼女も生きた。そのうちの片方は、この物語で誰かが手にした初めてのまともな結末だった。",
                    "برنگشت. او زنده ماند و یوآ زنده ماند، و یکی از این دو، اولین نتیجه‌ی خوبی بود که کسی در این داستان به دست آورد.");

                Hold(4.0f);

                Narrate(
                    "She kept the rabbit. She is still, as far as anybody knows, waiting for somebody who cannot be made to stay.",
                    "彼女はうさぎを持ちつづけた。誰かを待っている。引き留められない誰かを、たぶん今も。",
                    "خرگوش را نگه داشت. تا جایی که کسی می‌داند، هنوز منتظرِ کسی است که نمی‌شود نگهش داشت.");

                Hold(4.0f);

                StopMusic();

                Hold(3.0f);

                EndGame();
            }

            /// <summary>One line, poured out rather than typed.</summary>
            private void Rush(
                Speaker speaker, Portrait portrait, string english, string japanese, string persian)
            {
                Say(speaker, portrait, english, japanese, persian);
                Script[Script.Count - 1].TypeSpeed = RushSpeed;
            }
        }

        // ---------------------------------------------------------------------
        //  The secret ending — Yua says it first
        // ---------------------------------------------------------------------

        /// <summary>
        /// What happens when the player never once took the controlling option.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The design document's good ending, and the thing to understand about
        /// it is that Yua is not rescued here. She confesses. Every act of this
        /// game has been her refusing to say one sentence — that she is ashamed,
        /// and that the control was the shame wearing armour — and this is the
        /// only version of the story where she says it before somebody makes her.
        /// </para>
        /// <para>
        /// The line the whole game turns on is hers and it is a question:
        /// "are you not my Haru-pi?" It is the same sentence he asked her in a
        /// primary-school corridor in act six, handed back eight years later, and
        /// it is the only thing in the story that ever gets through to either of
        /// them.
        /// </para>
        /// </remarks>
        private sealed class GoodEndingBuilder : ActScriptWriter
        {
            protected override int ActNumber => 0;

            protected override string AssetName => Endings.GoodAssetName;

            protected override LocalizedLine Title =>
                L("The Quiet Ending", "静かな結末", "پایان آرام");

            protected override string MusicTrack => string.Empty;

            protected override void Write()
            {
                Hold(3.0f);

                Narrate(
                    "She had been about to say something else. She stopped, and said this instead, and it took her nine months and eight years.",
                    "別のことを言いかけて、彼女はやめた。代わりにこれを言った。九ヶ月と八年かかった。",
                    "می‌خواست چیزِ دیگری بگوید. ایستاد، و به‌جایش این را گفت، و نُه ماه و هشت سال طول کشیده بود.");

                Hold(3.0f);

                Enter(Speaker.Yua, Portrait.Sad);
                Enter(Speaker.Haru, Portrait.Neutral);

                Hold(2.4f);

                Say(Speaker.Yua, Portrait.Unchanged,
                    "Your leg. After all this time, it still has not got better, has it.",
                    "……脚。こんなに経っても、まだ治ってないよね。",
                    "درد پات. بعد از این همه مدت خوب نشده، نه؟");

                Hold(2.6f);

                Say(Speaker.Haru, Portrait.Shy,
                    "What are you talking about?",
                    "何の話？",
                    "درباره‌ی چی صحبت می‌کنی؟");

                Say(Speaker.Yua, Portrait.Unchanged,
                    "You know exactly what I am talking about. Why do you do that.",
                    "分かってるくせに。どうしてそうするの。",
                    "خودت می‌دونی. چرا خودت رو می‌زنی به اون راه؟");

                Hold(2.0f);

                Say(Speaker.Yua, Portrait.Crying,
                    "I know your leg broke that day, behind that door. I have always known.",
                    "あの日、あの扉の向こうで脚が折れたこと、知ってる。ずっと知ってた。",
                    "می‌دونم اون روز پشتِ در پات شکست. همیشه می‌دونستم.");

                Say(Speaker.Yua, Portrait.Unchanged,
                    "Why do you hide it. Why have you never once blamed me for it.",
                    "どうして隠すの。どうして一度も、私のせいにしないの。",
                    "چرا قایمش می‌کنی؟ چرا حتی یک بار بخاطرش منو سرزنش نکردی؟");

                Hold(3.4f);

                Say(Speaker.Haru, Portrait.DeadEyes,
                    "Are you trying to buy my pity so you can use it, or are you saying this honestly?",
                    "同情を買って使おうとしてる？　それとも、本気で言ってる？",
                    "داری سعی می‌کنی ترحمِ من رو بخری و ازم استفاده کنی، یا صادقانه داری می‌گی؟");

                Hold(3.0f);

                Rush(Speaker.Yua, Portrait.Crying,
                    "I am saying it honestly.",
                    "本気で言ってる。",
                    "جدی دارم می‌گم.");

                Rush(Speaker.Yua, Portrait.Unchanged,
                    "Why do you think I want to control you? Why do you think I have ever wanted that?",
                    "どうして私が支配したがると思う？　どうしてそうしたがったと思う？",
                    "فکر می‌کنی چرا می‌خوام کنترلت کنم؟ فکر می‌کنی چرا اصلاً همچین چیزی خواستم؟");

                Rush(Speaker.Yua, Portrait.Crying,
                    "Because I want you to be mine. Because if you are mine then I am the one looking after you.",
                    "私のものでいてほしいから。私のものなら、守ってるのは私だから。",
                    "چون می‌خوام واسه‌ی من باشی. چون اگه مالِ من باشی، اون‌وقت منم که دارم مراقبت هستم.");

                Rush(Speaker.Yua, Portrait.Unchanged,
                    "Because I do not want anything to happen to you. Not again. Not ever again.",
                    "何かがあってほしくないから。もう二度と。絶対に。",
                    "چون نمی‌خوام آسیب ببینی. دیگه نه. هیچ‌وقت دیگه نه.");

                Rush(Speaker.Yua, Portrait.Crying,
                    "Because the shame is killing me. Every time I remember it. Every single time I see you favour that leg.",
                    "恥ずかしさで死にそうだから。思い出すたび。ハルぴがその脚をかばうのを見るたび。",
                    "چون حسِ شرمی که دارم من رو می‌کشه. هر بار که یادش می‌افتم. هر بار که دردِ پات رو می‌بینم.");

                Hold(3.6f);

                Say(Speaker.Haru, Portrait.Sad,
                    "…Then why.",
                    "……じゃあ、どうして。",
                    "خب.. چرا خب؟");

                Hold(2.4f);

                Say(Speaker.Yua, Portrait.Angry,
                    "What do you mean, why? Why did you break your leg?",
                    "どうしてって何。ハルぴこそ、どうして脚を折ったの？",
                    "یعنی چی چرا؟ خودت چرا پات رو شکستی؟");

                Say(Speaker.Yua, Portrait.Unchanged,
                    "You will say you would have done it for anybody. Fine. I will allow that.",
                    "誰にでもそうしたって言うんでしょ。いいよ。それは認める。",
                    "شاید بگی اگه هر کسِ دیگه‌ای بود هم همین کار رو می‌کردم. قبول.");

                Hold(2.2f);

                Say(Speaker.Yua, Portrait.Crying,
                    "But I do not know. I love you.",
                    "でも、分からない。……好きなの。",
                    "اما نمی‌دونم. من دوستت دارم.");

                Say(Speaker.Yua, Portrait.Unchanged,
                    "Do you not love me too? Are you not my Haru-pi?",
                    "ハルぴは、私のこと好きじゃないの？　私のハルぴじゃないの？",
                    "مگه توام منو دوست نداری؟ مگه تو هارو‌پیِ من نیستی؟");

                Hold(4.0f);

                Narrate(
                    "He did not move for a moment. Then he crossed the distance in about a second and a half and put both arms around her.",
                    "彼はしばらく動かなかった。それから、一秒半ほどでその距離を詰めて、両腕で彼女を抱きしめた。",
                    "لحظه‌ای خشکش زد. بعد در حدودِ یک ثانیه‌ونیم فاصله را طی کرد و با هر دو دست بغلش کرد.");

                Hold(3.0f);

                Rush(Speaker.Haru, Portrait.Crying,
                    "I am so glad to hear that. I am so glad. I love you too. I love you so much.",
                    "聞けてよかった。本当によかった。僕も好きだよ。すごく、好きだ。",
                    "خیلی خوشحالم که این‌ها رو می‌شنوم. خیلی خوشحالم. منم دوستت دارم. خیلی دوستت دارم.");

                Rush(Speaker.Haru, Portrait.Unchanged,
                    "I am sorry I could not look after you properly. I am sorry you were hurt that badly.",
                    "ちゃんと守れなくてごめん。あんなに傷つけてしまってごめん。",
                    "ببخشید که نتونستم درست مراقبت باشم. ببخشید که این‌قدر آسیب دیدی.");

                Rush(Speaker.Haru, Portrait.Crying,
                    "I am sorry it turned you into somebody who had to do all of that to keep hold of anything.",
                    "何かを手放さずにいるために、あんなことをしなきゃいけない人にしてしまって、ごめん。",
                    "ببخشید که کاری کرد دست به این رفتارها ببری تا چیزی رو نگه داری.");

                Hold(2.6f);

                Say(Speaker.Haru, Portrait.Sad,
                    "I am always here. All right? Always.",
                    "僕はずっとここにいる。いい？　ずっと。",
                    "من همیشه پیشتم. باشه؟ همیشه.");

                Say(Speaker.Haru, Portrait.Unchanged,
                    "You never have to do anything to keep me. You never have to be somebody else.",
                    "僕をつなぎとめるために、何もしなくていい。別の誰かにならなくていい。",
                    "لازم نیست بخاطرِ داشتنِ من دست به کاری بزنی. لازم نیست خودت نباشی.");

                Say(Speaker.Haru, Portrait.Joyful,
                    "Like when we were small. Exactly as you are. That is who I love.",
                    "小さいころみたいに。そのままの結愛ぴを。僕が好きなのは、それ。",
                    "مثلِ بچگی‌هات. همون‌طوری که هستی. من همون رو دوست دارم.");

                Hold(2.4f);

                Say(Speaker.Haru, Portrait.Crying,
                    "You are always going to be my Yua-pi. I promise I will look after you with everything I have.",
                    "結愛ぴは、ずっと僕の結愛ぴだよ。全力で守るって、約束する。",
                    "تو همیشه برای من یوآ‌پیِ خودمی. قول می‌دم با تمامِ تلاشم مراقبت باشم.");

                Say(Speaker.Haru, Portrait.Unchanged,
                    "I love you. I am in love with you.",
                    "好きだ。……愛してる。",
                    "دوستت دارم. منم عاشقتم.");

                Hold(3.6f);

                Narrate(
                    "They both cried, holding on to each other, and said it over and over with no space in between.",
                    "二人とも泣いて、抱き合ったまま、間を置かずに何度も何度もそれを言った。",
                    "هر دو گریه کردند، در بغلِ هم، و پشتِ سرِ هم و بی‌فاصله گفتندش.");

                Hold(2.6f);

                Rush(Speaker.Yua, Portrait.Crying,
                    "I love you. I love you. I love you.",
                    "好き。好き。好き。",
                    "دوست دارم. دوست دارم. دوست دارم.");

                Rush(Speaker.Haru, Portrait.Crying,
                    "I love you. I love you. I love you.",
                    "好き。好き。好き。",
                    "دوست دارم. دوست دارم. دوست دارم.");

                Hold(4.0f);

                Narrate(
                    "Nothing was fixed. Everything that had happened had still happened, and there was going to be a great deal of work.",
                    "何も直ってはいない。起きたことは全部そのままで、これからやることは山ほどある。",
                    "هیچ چیزی درست نشد. هر چه اتفاق افتاده بود همچنان افتاده بود، و کارِ زیادی در پیش بود.");

                Hold(3.0f);

                Narrate(
                    "But somebody had said the true thing out loud before anybody made them, and that turned out to be the whole of what either of them had ever needed.",
                    "けれど、誰に迫られるでもなく、本当のことが声に出された。二人に必要だったのは、結局それだけだった。",
                    "اما یکی حرفِ راست را بلند گفته بود، پیش از آنکه کسی مجبورش کند، و معلوم شد تمامِ آنچه هر دوشان لازم داشتند همین بود.");

                Hold(4.0f);

                EndGame();
            }

            /// <summary>One line, poured out rather than typed.</summary>
            private void Rush(
                Speaker speaker, Portrait portrait, string english, string japanese, string persian)
            {
                Say(speaker, portrait, english, japanese, persian);
                Script[Script.Count - 1].TypeSpeed = RushSpeed;
            }
        }
    }
}
