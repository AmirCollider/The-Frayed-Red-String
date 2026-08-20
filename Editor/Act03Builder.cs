// -----------------------------------------------------------------------------
//  The Frayed Red String
//  Act03Builder.cs  (Editor only)
//
//  Act three — Serenity (静けさ) — written out as code.
//
//  Run it once from The Frayed Red String ▸ Build Act 03 From The Story
//  Document. It writes Assets/Story/Acts/Act03.asset, reusing the asset already
//  at that path if there is one, and then hands over to the Story Editor.
//
//  The quiet act, and the design document is exact about what makes it quiet:
//  Haru and Yua go back to dating the way they did in act one, and three things
//  are different.
//
//    • The blue and green buttons are there from the first scene. Act two
//      introduced them and act two was where Yua explained what she wanted done
//      with them; here they are simply part of how the game is played, which is
//      worse.
//    • When Haru's leg stops him, Yua no longer waits it out in silence. She
//      asks whether he regrets it. He does not answer, and she lets it go — and
//      both of them file that away as kindness.
//    • On one of their dates they pass a bed of red spider lilies, and Yua says
//      that seeing them makes her eyes wet without her deciding to. Haru says he
//      knows. Neither of them says anything else, and neither of them has to:
//      act six is where the player finds out that garden was the only place she
//      had, and that he has known which flowers they were since he was nine.
//
//  Nothing is confronted here. That is the act.
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Narrative;
using UnityEditor;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Act three's script.</summary>
    public sealed class Act03Builder : ActScriptWriter
    {
        protected override int ActNumber => 3;

        protected override string AssetName => "Act03";

        protected override LocalizedLine Title => L("Serenity", "静けさ", "آرامش");

        [MenuItem("The Frayed Red String/Build Act 03 From The Story Document")]
        public static void Build()
        {
            new Act03Builder().BuildAsset();
        }

        /// <summary>Builds the act under a given policy, for the one-press setup.</summary>
        public static void Build(ActScriptWriter.RebuildPolicy policy)
        {
            new Act03Builder().BuildAsset(policy);
        }

        /// <summary>The act, in order.</summary>
        protected override void Write()
        {
            WriteWalkToSchool();
            WriteClassroom();
            WriteRooftop();
            WriteWayHome();
            WriteBakeryStreet();
            WriteFlowerBed();
            WriteAlleywayNight();
            WriteYuaRoom();
        }

        /// <summary>Autumn, and a routine that has become a shape.</summary>
        private void WriteWalkToSchool()
        {
            Place(
                Backgrounds.SchoolAlleyDay,
                "The way to school", "通学路", "مسیر مدرسه");

            Hold(2.2f);

            Narrate(
                "Autumn now. The blossom is long gone and the trees have turned, and the path looks like a different place wearing the same shape.",
                "秋になった。花はとうに散り、木は色を変え、道は同じかたちをした別の場所のように見える。",
                "حالا پاییز است. شکوفه‌ها خیلی وقت است رفته‌اند و درخت‌ها عوض شده‌اند، و مسیر مثل جای دیگری شده که همان شکل را پوشیده.");

            Narrate(
                "They walk it every morning. Same time, same side of the path, same half-step apart.",
                "二人は毎朝ここを歩く。同じ時間、同じ側、同じ半歩の距離で。",
                "هر روز صبح از این مسیر می‌روند. همان ساعت، همان طرف، همان نیم قدم فاصله.");

            Enter(Speaker.Yua, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Joyful,
                "Haru-pi. You are seven minutes early.",
                "ハルぴ。七分も早いよ。",
                "هارو‌پی. هفت دقیقه زودی.");

            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Joyful,
                "So are you.",
                "結愛ぴもね。",
                "تو هم همین‌طور.");

            Say(Speaker.Yua, Portrait.Neutral,
                "I am always seven minutes early. You are seven minutes early today.",
                "私はいつも七分早い。ハルぴが七分早いのは今日。",
                "من همیشه هفت دقیقه زودم. تو امروز هفت دقیقه زودی.");

            Say(Speaker.Haru, Portrait.Shy,
                "I could not sleep.",
                "眠れなかったんだ。",
                "خوابم نمی‌برد.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You said that on Tuesday.",
                "火曜日も同じこと言ってた。",
                "سه‌شنبه هم همین رو گفتی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It was true on Tuesday.",
                "火曜日も本当だったよ。",
                "سه‌شنبه هم راست بود.");

            Hold(1.4f);

            Narrate(
                "This is the calm part of the story. Everything before it was a rehearsal for it and everything after it is louder, and nobody standing on this path knows either of those things.",
                "ここが物語の静かな部分だ。ここまではその予行で、この先はもっと騒がしい。この道に立っている誰も、そのどちらも知らない。",
                "این بخشِ آرامِ داستان است. هر چه پیش از آن بود تمرینِ همین بود و هر چه پس از آن می‌آید بلندتر است، و هیچ‌کس که روی این مسیر ایستاده هیچ‌کدام از این دو را نمی‌داند.");

            Decide(
                "You can tell me why you cannot sleep.",
                "眠れない理由、話してくれてもいいんだよ。",
                "می‌تونی بهم بگی چرا خوابت نمی‌بره.",

                "Then sleep earlier. I do not like you tired.",
                "じゃあ早く寝て。疲れてるハルぴは嫌い。",
                "پس زودتر بخواب. دوست ندارم خسته باشی.",

                "No. Try again.",
                "……ううん。もう一回。",
                "نه. دوباره امتحان کن.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Then sleep earlier. I do not like you tired.",
                "じゃあ早く寝て。疲れてるハルぴは嫌い。",
                "پس زودتر بخواب. دوست ندارم خسته باشی.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Okay.",
                "うん。",
                "باشه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Say it properly.",
                "ちゃんと言って。",
                "درست بگو.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I will sleep earlier.",
                "早く寝ます。",
                "زودتر می‌خوابم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Good.",
                "よろしい。",
                "خوبه.");

            Hold(1.6f);
        }

        /// <summary>Fourth period, a schedule, and one choice that means nothing.</summary>
        private void WriteClassroom()
        {
            Maybe(MachineRoom, 0.25f);

            ClearStage();

            Place(
                Backgrounds.ClassroomDay,
                "The classroom", "教室", "کلاس درس");

            Hold(1.8f);

            Narrate(
                "Fourth period. The window was open, and somebody two rows down had been asleep since the bell.",
                "四時間目。窓は開いていて、二列前の誰かはチャイムからずっと寝ている。",
                "زنگ چهارم. پنجره باز بود و یکی دو ردیف جلوتر از همان زنگ خواب بود.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Unchanged,
                "There is a test on Friday.",
                "金曜日にテストがある。",
                "جمعه امتحان داریم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "There is. I made you a schedule.",
                "あるね。予定表つくっておいた。",
                "داریم. برات برنامه ریختم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You made me a schedule.",
                "予定表を、つくった。",
                "برام برنامه ریختی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Tuesday and Thursday at my house. Wednesday you rest. Friday you pass.",
                "火曜と木曜はうちで。水曜は休む。金曜は受かる。",
                "سه‌شنبه و پنج‌شنبه خونه‌ی من. چهارشنبه استراحت می‌کنی. جمعه قبول می‌شی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "And Saturday?",
                "土曜日は？",
                "و شنبه؟");

            Say(Speaker.Yua, Portrait.Neutral,
                "Saturday is mine.",
                "土曜日は私のもの。",
                "شنبه مالِ منه.");

            Hold(1.2f);

            DecideIdly(
                "Ask what happens on Wednesday.",
                "水曜日は何をするのか訊く。",
                "بپرس چهارشنبه چه خبره.",

                "Say nothing and let her keep going.",
                "何も言わず、話させておく。",
                "چیزی نگو و بذار ادامه بده.");

            Say(Speaker.Yua, Portrait.Joyful,
                "You are not listening. Wednesday you rest. I have said it twice.",
                "聞いてないでしょ。水曜は休む。二回言った。",
                "داری گوش نمی‌دی. چهارشنبه استراحت می‌کنی. دو بار گفتم.");

            Say(Speaker.Haru, Portrait.Shy,
                "Sorry.",
                "ごめん。",
                "ببخشید.");

            Narrate(
                "There is nothing behind that one. Not every move she makes is a move — which is precisely what makes the ones that are so hard to see.",
                "それには裏がない。彼女のすることが全部手というわけではない。だからこそ、手であるものが見えにくい。",
                "پشتِ آن یکی چیزی نیست. هر کاری که او می‌کند حرکت نیست — و دقیقاً همین است که دیدنِ آن‌هایی که حرکت‌اند را این‌قدر سخت می‌کند.");

            Hold(1.8f);
        }

        /// <summary>The roof at lunch, and the first thing neither of them says.</summary>
        private void WriteRooftop()
        {
            ClearStage();

            Place(
                Backgrounds.RooftopDay,
                "The roof", "屋上", "پشت‌بام");

            Hold(2.0f);

            Narrate(
                "Wind, a wire fence, and the whole town laid out flat and small enough to be nobody's problem.",
                "風と金網と、平らに小さく広がって誰の問題でもなくなった町。",
                "باد، توری سیمی، و تمام شهر پهن و آن‌قدر کوچک که مشکلِ هیچ‌کس نباشد.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Joyful,
                "I made two. Take the one with the egg in it, you always want the one with the egg in it.",
                "二つ作った。卵のほう取って。ハルぴいつも卵のほしがるから。",
                "دو تا درست کردم. اونی که تخم‌مرغ داره رو بردار، همیشه اونی رو می‌خوای که تخم‌مرغ داره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I do want the one with the egg in it.",
                "……卵のがいい。",
                "آره، اونی که تخم‌مرغ داره رو می‌خوام.");

            Hold(1.4f);

            Narrate(
                "They ate. The bell for fifth period was still a long way off and neither of them mentioned it.",
                "二人は食べた。五時間目のチャイムはまだ遠く、どちらもそれに触れなかった。",
                "خوردند. زنگ پنجم هنوز خیلی مانده بود و هیچ‌کدام حرفش را نزد.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Yua-pi.",
                "結愛ぴ。",
                "یوآ‌پی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Mm.",
                "うん。",
                "هوم.");

            Say(Speaker.Haru, Portrait.Shy,
                "Nothing. I just wanted to say it.",
                "……なんでもない。呼んでみただけ。",
                "هیچی. فقط خواستم بگمش.");

            Hold(2.2f);

            Narrate(
                "He had wanted to say something else. He got as far as her name and stopped, the way he has stopped at that exact place for eight years.",
                "本当は別のことを言いたかった。名前まで来て止まった。八年前から、いつもちょうどそこで止まる。",
                "می‌خواست چیز دیگری بگوید. تا اسمش رسید و ایستاد؛ همان‌جایی که هشت سال است دقیقاً همان‌جا می‌ایستد.");

            Decide(
                "You have gone quiet. I am here, if you want me.",
                "静かになったね。私、ここにいるよ。",
                "ساکت شدی. من اینجام، اگه بخوای.",

                "Whatever you are thinking about — think about me instead.",
                "何を考えてるにしても、代わりに私のことを考えて。",
                "به هر چی داری فکر می‌کنی — به‌جاش به من فکر کن.",

                "That is not the one.",
                "……それじゃない。",
                "اون یکی نیست.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Whatever you are thinking about, think about me instead.",
                "何を考えてるにしても、代わりに私のことを考えて。",
                "به هر چی داری فکر می‌کنی، به‌جاش به من فکر کن.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I was, actually.",
                "……実はそうだったよ。",
                "راستش داشتم همین کار رو می‌کردم.");

            Say(Speaker.Yua, Portrait.Shy,
                "Do not do that.",
                "そういうの、やめて。",
                "این کار رو نکن.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You told me to.",
                "言われたから。",
                "خودت گفتی.");

            Hold(2.0f);

            Narrate(
                "It went well. It goes well most days now, and that is the part she cannot leave alone.",
                "うまくいった。最近はたいていうまくいく。そして、彼女が放っておけないのはそこだ。",
                "خوب پیش رفت. این روزها بیشتر روزها خوب پیش می‌رود، و دقیقاً همین است که یوآ نمی‌تواند به حال خودش رهایش کند.");

            Hold(1.6f);
        }

        /// <summary>
        /// The walk home, the leg, and the question act one and act two never
        /// let her ask.
        /// </summary>
        /// <remarks>
        /// The centre of the act, and the one thing the design document spells
        /// out line by line: she does not stay unreactive this time. She asks
        /// whether he regrets it. He gives no answer at all — and the line that
        /// measures patience is the silence he leaves, because a player who sits
        /// through it is doing the thing the whole game is asking for.
        /// </remarks>
        private void WriteWayHome()
        {
            ClearStage();

            Place(
                Backgrounds.VendingStreetDay,
                "The way home", "帰り道", "راه خانه");

            Hold(2.0f);

            Narrate(
                "The light was going amber the way it does in late October, and the vending machine at the corner was already the brightest thing on the street.",
                "十月の終わりらしく光が琥珀色に傾き、角の自販機はもう通りでいちばん明るいものになっていた。",
                "نور داشت کهربایی می‌شد، همان‌طور که اواخر مهر می‌شود، و دستگاه نوشیدنیِ سرِ کوچه از همین حالا روشن‌ترین چیزِ خیابان بود.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Joyful,
                "Peach.",
                "桃。",
                "هلو.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Peach.",
                "桃ね。",
                "هلو.");

            Hold(1.2f);

            Narrate(
                "Four streets later, at the corner he always stops at, he stopped.",
                "四つ角を四つ過ぎた、いつも止まるその角で、彼は止まった。",
                "چهار خیابان بعد، سرِ همان کوچه‌ای که همیشه می‌ایستد، ایستاد.");

            SayWithSound(Speaker.Haru, Portrait.Sad, SfxId.LegAche, 0.80f,
                "Sorry. A moment.",
                "ごめん。少しだけ。",
                "ببخشید. یه لحظه.");

            Narrate(
                "The same corner. He has never once said that it is the same corner.",
                "同じ角。同じ角だと、彼は一度も言ったことがない。",
                "همان کوچه. حتی یک بار هم نگفته که همان کوچه است.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Haru-pi.",
                "ハルぴ。",
                "هارو‌پی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Mm.",
                "うん。",
                "هوم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Do you regret it? What happened to your leg.",
                "後悔してる？　その脚のこと。",
                "پشیمونی؟ بابت اتفاقی که واسه پات افتاد.");

            Hold(2.4f);

            Narrate(
                "A year ago she would have looked at the vending machine and waited for it to pass. She did not look away this time.",
                "一年前なら、自販機のほうを見て、それが過ぎるのを待っただろう。今日は目をそらさなかった。",
                "یک سال پیش به دستگاه نوشیدنی نگاه می‌کرد و منتظر می‌ماند تا بگذرد. این‌بار رویش را برنگرداند.");

            ChildVoice(Speaker.HaruChild,
                "Do not look at it.",
                "見ないで。",
                "بهش نگاه نکن.");

            Hold(2.0f);

            Listen(
                "Haru did not answer. He looked at the road. The light went a shade lower, and he still did not answer.",
                "ハルは答えなかった。道を見ていた。光が一段落ちて、それでも答えなかった。",
                "هارو جواب نداد. به خیابان نگاه کرد. نور یک درجه پایین‌تر رفت، و باز هم جواب نداد.");

            Hold(3.0f);

            Say(Speaker.Haru, Portrait.Neutral,
                "It is not far now. Let us go.",
                "もう近いよ。行こう。",
                "دیگه راه نمونده. بریم.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Okay.",
                "うん。",
                "باشه.");

            Narrate(
                "She had asked a question she already knew the answer to, and he had refused to hand it over, and both of them counted that as a kindness.",
                "答えを知っている質問を、彼女はした。彼はそれを渡さなかった。二人ともそれを優しさとして数えた。",
                "سؤالی پرسیده بود که جوابش را می‌دانست، و او حاضر نشد بدهدش، و هر دو این را مهربانی حساب کردند.");

            Hold(2.2f);

            Decide(
                "Leave it. He will tell you when he can.",
                "そっとしておいて。話せるようになったら話してくれる。",
                "ولش کن. هر وقت بتونه بهت می‌گه.",

                "Ask again. He gives way on the third time. He always does.",
                "もう一度訊いて。三回目で折れる。いつもそう。",
                "دوباره بپرس. بار سوم کوتاه میاد. همیشه میاد.",

                "You do not get to be kind on my behalf.",
                "私の代わりに優しくしないで。",
                "تو حق نداری به‌جای من مهربون باشی.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Haru-pi. Do you regret it?",
                "ハルぴ。後悔してる？",
                "هارو‌پی. پشیمونی؟");

            Say(Speaker.Haru, Portrait.Sad,
                "Yua-pi.",
                "結愛ぴ。",
                "یوآ‌پی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Do you.",
                "してるの。",
                "پشیمونی یا نه.");

            Say(Speaker.Haru, Portrait.Crying,
                "Please.",
                "……お願いだから。",
                "خواهش می‌کنم.");

            Hold(2.4f);

            Say(Speaker.Yua, Portrait.Joyful,
                "Okay. Okay. Forget it. Peach?",
                "うん。うん。忘れて。……桃、飲む？",
                "باشه. باشه. فراموشش کن. هلو می‌خوری؟");

            Say(Speaker.Haru, Portrait.Shy,
                "Peach.",
                "……桃。",
                "هلو.");

            Narrate(
                "The third time. She was right about the third time, and being right about it is the worst thing in this scene.",
                "三度目。三度目のことで彼女は正しかった。そして正しかったことが、この場面でいちばん悪い。",
                "بار سوم. درباره‌ی بار سوم حق با او بود. و همین حق داشتنش بدترین چیزِ این صحنه است.");

            Hold(2.4f);
        }

        /// <summary>A date, and the lightest scene in the act.</summary>
        private void WriteBakeryStreet()
        {
            Maybe(LegAche, 0.25f);

            ClearStage();

            Place(
                Backgrounds.BakeryStreetDay,
                "Usagi Bakery", "うさぎベーカリー", "نانوایی اوساگی");

            Hold(1.8f);

            Narrate(
                "Saturday. Two cats asleep on the bench outside the bakery, in the same two places they were asleep last Saturday.",
                "土曜日。ベーカリーの前のベンチで、先週と同じ場所に猫が二匹眠っている。",
                "شنبه. دو گربه روی نیمکتِ جلوی نانوایی خوابیده‌اند؛ همان دو جایی که شنبه‌ی پیش خوابیده بودند.");

            Enter(Speaker.Haru, Portrait.Joyful);
            Enter(Speaker.Yua, Portrait.Joyful);

            Say(Speaker.Yua, Portrait.Unchanged,
                "The melon one. Do not argue, you liked it last time and you will like it again.",
                "メロンパン。反論しないで。前も好きだったし今回も好きになる。",
                "همون نان ملونی. بحث نکن، دفعه‌ی پیش خوشت اومد و این دفعه هم خوشت میاد.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I was not going to argue.",
                "反論するつもりなかったよ。",
                "قصد بحث نداشتم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "You never are. It is very restful.",
                "いつもそう。すごく楽。",
                "هیچ‌وقت نداری. خیلی آرامش‌بخشه.");

            Hold(1.4f);

            Narrate(
                "This is what the act is called after. Not the quiet in the alley or the quiet on the roof — this. Two people and a bench and a cat, and nothing underneath it that anybody has to look at yet.",
                "この幕の名前はこれのことだ。路地の静けさでも屋上の静けさでもない。二人とベンチと猫、その下にまだ誰も見なくていいもの。",
                "اسم این پرده از همین است. نه سکوتِ کوچه، نه سکوتِ پشت‌بام — همین. دو نفر و یک نیمکت و یک گربه، و هیچ چیزی زیرش که کسی هنوز مجبور باشد نگاهش کند.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Yua-pi. Thank you for the schedule.",
                "結愛ぴ。予定表、ありがとう。",
                "یوآ‌پی. بابت برنامه ممنون.");

            Say(Speaker.Yua, Portrait.Shy,
                "It was not a favour. I want you to pass.",
                "親切じゃないよ。受かってほしいだけ。",
                "لطف نبود. می‌خوام قبول بشی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I know. Thank you for the schedule.",
                "うん。予定表、ありがとう。",
                "می‌دونم. بابت برنامه ممنون.");

            Hold(1.6f);

            Decide(
                "You could just say you are welcome.",
                "「どういたしまして」って言えばいいのに。",
                "می‌تونی فقط بگی خواهش می‌کنم.",

                "Say it again. Say thank you again.",
                "もう一回言って。ありがとうって、もう一回。",
                "دوباره بگو. دوباره بگو ممنون.",

                "Again.",
                "……もう一回。",
                "دوباره.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Say it again.",
                "もう一回言って。",
                "دوباره بگو.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Thank you for the schedule.",
                "予定表、ありがとう。",
                "بابت برنامه ممنون.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Again.",
                "もう一回。",
                "دوباره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Thank you for the schedule.",
                "予定表、ありがとう。",
                "بابت برنامه ممنون.");

            Hold(2.0f);

            Narrate(
                "He did not find it strange. That is not a thing that can be pointed at in a scene, so it goes here instead: he did not find it strange, and he has not found any of it strange, ever.",
                "彼はそれを変だと思わなかった。場面のなかで指させることではないので、ここに置く。彼はそれを変だと思わなかったし、これまでどれ一つ変だと思ったことがない。",
                "برایش عجیب نبود. این چیزی نیست که بشود در یک صحنه به آن اشاره کرد، پس همین‌جا نوشته می‌شود: برایش عجیب نبود، و هیچ‌وقت هیچ‌کدامش برایش عجیب نبوده.");

            Hold(2.2f);
        }

        /// <summary>
        /// The flower bed, and the one thing in this act that is said out loud.
        /// </summary>
        /// <remarks>
        /// Straight out of the design document: they pass red spider lilies on a
        /// date, she says that seeing them makes her eyes wet without her
        /// deciding to, and he says he knows. Nothing else is said, because
        /// there is nothing either of them can say next that does not open act
        /// six eight hours early.
        /// </remarks>
        private void WriteFlowerBed()
        {
            ClearStage();

            Place(
                Backgrounds.PlaygroundDay,
                "The park", "公園", "پارک");

            Hold(2.2f);

            Narrate(
                "There is a bed of flowers at the edge of the park, past the fountain, where the gardener has given up and let one thing take the whole plot.",
                "公園のはずれ、噴水の向こうに花壇がある。庭師が手を引いて、一種類だけに区画ごと明け渡した場所だ。",
                "کنارِ پارک، آن‌سوی فواره، یک باغچه هست؛ جایی که باغبان دست کشیده و گذاشته یک گل تمام قطعه را بگیرد.");

            Narrate(
                "Red spider lilies. A hundred of them, all the same red, all leafless, all opening at the same hour of the same week every year.",
                "彼岸花。百本の同じ赤が、葉も持たず、毎年同じ週の同じ時刻に一斉に開く。",
                "سوسن‌های عنکبوتیِ قرمز. صد تا، همه به یک قرمزی، همه بی‌برگ، همه در همان ساعت از همان هفته‌ی هر سال باز می‌شوند.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Hold(2.0f);

            Narrate(
                "Yua stopped walking. Not sharply. She simply arrived at the edge of the bed and did not go past it.",
                "結愛は歩くのをやめた。急にではない。花壇の縁まで来て、ただそこから先に行かなかった。",
                "یوآ ایستاد. نه با تکان. فقط به لبه‌ی باغچه رسید و از آن رد نشد.");

            Say(Speaker.Yua, Portrait.Sad,
                "You know that whenever I see these flowers my eyes get wet on their own.",
                "この花を見ると、勝手に目が濡れるの。知ってるでしょ。",
                "می‌دونی که هر وقت این گل‌ها رو می‌بینم ناخودآگاه چشمام خیس از اشک می‌شه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "I know.",
                "知ってる。",
                "می‌دونم.");

            Hold(2.8f);

            Narrate(
                "Two people standing in front of a flower bed, agreeing about a thing neither of them will name. Everything this act is called after is in that.",
                "花壇の前に立つ二人が、どちらも名指ししないものについて同意している。この幕の名前は、そのなかにある。",
                "دو نفر جلوی یک باغچه ایستاده‌اند و درباره‌ی چیزی توافق می‌کنند که هیچ‌کدام اسمش را نمی‌آورد. هر چه اسم این پرده از آن است، در همین است.");

            Narrate(
                "There is a garden he has never mentioned, where a girl used to sit and talk to the flowers because there was nobody else, and water them by crying. He has known which flowers they were since he was nine.",
                "彼が一度も口にしたことのない庭がある。ほかに誰もいないから花に話しかけ、泣いて水をやっていた女の子の庭だ。その花が何かを、彼は九つのときから知っている。",
                "باغچه‌ای هست که او هرگز حرفش را نزده؛ جایی که دختری می‌نشست و با گل‌ها حرف می‌زد چون کسِ دیگری نبود، و با اشک‌هایش آب‌شان می‌داد. از نُه‌سالگی می‌داند آن گل‌ها چه بودند.");

            Hold(2.4f);

            Decide(
                "Then let us not stand here. Come on.",
                "じゃあ、ここに立ってるのやめよ。行こ。",
                "پس اینجا وانستیم. بیا بریم.",

                "Stand here with me. I want you to watch me cry.",
                "一緒に立ってて。泣くところを見ていてほしいの。",
                "همین‌جا با من وایسا. می‌خوام گریه‌ام رو ببینی.",

                "No. I want him to look.",
                "……ううん。見ていてほしいの。",
                "نه. می‌خوام نگاه کنه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Stay. Look at me.",
                "いて。こっち見て。",
                "بمون. نگام کن.");

            Say(Speaker.Haru, Portrait.Sad,
                "Yua-pi—",
                "結愛ぴ——",
                "یوآ‌پی—");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Look at me.",
                "見て。",
                "نگام کن.");

            Hold(2.4f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "I am looking.",
                "見てるよ。",
                "دارم نگاه می‌کنم.");

            Say(Speaker.Yua, Portrait.Crying,
                "Do not say anything else. Just that.",
                "ほかは何も言わないで。それだけでいい。",
                "چیز دیگه‌ای نگو. فقط همین.");

            Hold(3.0f);

            Narrate(
                "She was not pretending. That is the part that matters later, and the part nobody watching this from where you are sitting is in any position to believe.",
                "演技ではなかった。あとで効いてくるのはそこだ。そして、あなたが座っている場所からそれを信じられる立場の人間は、誰もいない。",
                "تظاهر نمی‌کرد. همین است که بعدها مهم می‌شود، و همین است که هیچ‌کسی که از جایی که تو نشسته‌ای تماشا می‌کند در موقعیتی نیست که باورش کند.");

            Hold(2.2f);
        }

        /// <summary>
        /// The alley at night, the machine room, and the promise from the café
        /// being kept.
        /// </summary>
        private void WriteAlleywayNight()
        {
            ClearStage();

            Place(
                Backgrounds.AlleywayNight,
                "The alley", "路地", "کوچه");

            Hold(2.2f);

            Narrate(
                "The paper lantern outside the corner house was lit, the way it is every night, by somebody neither of them has ever seen.",
                "角の家の提灯には、毎晩そうであるように灯が入っていた。二人が一度も見たことのない誰かの手で。",
                "فانوس کاغذیِ جلوی خانه‌ی سرِ کوچه روشن بود، مثل هر شب، به دستِ کسی که هیچ‌کدامشان هرگز ندیده‌اند.");

            Enter(Speaker.Haru, Portrait.Neutral);
            Enter(Speaker.Yua, Portrait.Neutral);

            Cue(SfxId.BoilerRoom, 0.55f);

            Narrate(
                "The grate at the end of the alley was breathing.",
                "路地の突き当たりの格子が、息をしていた。",
                "دریچه‌ی انتهای کوچه داشت نفس می‌کشید.");

            SayWithSound(Speaker.Yua, Portrait.DeadEyes, SfxId.Heartbeat, 0.70f,
                "...",
                "……",
                "…");

            Say(Speaker.Haru, Portrait.Neutral,
                "Do you want to go round?",
                "回っていく？",
                "می‌خوای از دور بریم؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "No.",
                "ううん。",
                "نه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I want to stand here until it stops.",
                "止まるまで、ここに立ってたい。",
                "می‌خوام همین‌جا وایسم تا تموم بشه.");

            ChildVoice(Speaker.YuaChild,
                "If I keep very still it goes away.",
                "じっとしてれば、いなくなる。",
                "اگه خیلی بی‌حرکت بمونم می‌ره.");

            Hold(2.0f);

            Listen(
                "So they stood there. He did not ask why, and she did not explain, and the sound went on for exactly as long as it went on.",
                "だから二人はそこに立っていた。彼は理由を訊かず、彼女は説明せず、音は続くだけ続いた。",
                "پس همان‌جا ایستادند. او نپرسید چرا و یوآ توضیح نداد، و صدا دقیقاً تا هر وقت که ادامه داشت، ادامه داشت.");

            Hold(3.2f);

            Say(Speaker.Yua, Portrait.Neutral,
                "It stopped.",
                "止まった。",
                "تموم شد.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It stopped.",
                "止まったね。",
                "تموم شد.");

            Hold(1.6f);

            Decide(
                "Tell him why. He would stay.",
                "理由を話して。彼なら、いてくれる。",
                "بهش بگو چرا. می‌مونه.",

                "Tell him nothing. He stays anyway.",
                "何も話さなくていい。どうせいてくれる。",
                "هیچی بهش نگو. به‌هرحال می‌مونه.",

                "He stays anyway.",
                "……どうせ、いてくれる。",
                "به‌هرحال می‌مونه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "It is nothing. It is just a noise.",
                "なんでもない。ただの音。",
                "چیزی نیست. فقط یه صداست.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Okay.",
                "うん。",
                "باشه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You are not going to ask?",
                "訊かないの？",
                "نمی‌پرسی؟");

            Say(Speaker.Haru, Portrait.Neutral,
                "I asked once. You will tell me when you want to.",
                "一度訊いた。話したくなったら話してくれる。",
                "یه بار پرسیدم. هر وقت خودت خواستی می‌گی.");

            Hold(2.4f);

            Narrate(
                "That is the promise from the café, being kept — by the only one of the two of them who read it before signing.",
                "喫茶店の約束が守られている。二人のうち、署名する前に読んだほうの一人によって。",
                "قولِ کافه دارد نگه داشته می‌شود — توسط تنها کسی از آن دو که پیش از امضا خوانده بودش.");

            Hold(2.0f);
        }

        /// <summary>Her room, the player again, and the end of the quiet.</summary>
        private void WriteYuaRoom()
        {
            Maybe(MachineRoom, 0.20f);

            ClearStage();

            Place(
                Backgrounds.YuaRoomDay,
                "Yua's room", "結愛の部屋", "اتاق یوآ");

            Hold(2.6f);

            Narrate(
                "The crack in the mirror has not moved. Nothing in this room ever moves, which is most of what there is to say about the room.",
                "鏡のひびは動いていない。この部屋のものは何も動かない。部屋について言えることのほとんどはそれだ。",
                "ترکِ آینه تکان نخورده. هیچ چیزی در این اتاق هیچ‌وقت تکان نمی‌خورد، و بیشترِ آنچه درباره‌ی این اتاق می‌شود گفت همین است.");

            Enter(Speaker.Yua, Portrait.Neutral);

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Still there.",
                "……まだいるね。",
                "هنوز اونجایی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Good. That was a good month. Say it was a good month.",
                "よかった。いい一ヶ月だった。いい一ヶ月だったって言って。",
                "خوبه. ماه خوبی بود. بگو ماه خوبی بود.");

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Neutral,
                "He asked me nothing. I asked him one thing, he would not answer, and I let it go.",
                "彼は何も訊かなかった。私は一つだけ訊いて、答えてくれなくて、それで引いた。",
                "او هیچی از من نپرسید. من یک چیز پرسیدم، جواب نداد، و ولش کردم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "That is what people call trust, I think.",
                "それを世間では信頼って言うんだと思う。",
                "فکر کنم به این می‌گن اعتماد.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.DeadEyes,
                "It is not, though.",
                "……違うけどね。",
                "البته که نیست.");

            Hold(2.4f);

            Say(Speaker.Yua, Portrait.Neutral,
                "It is two people who have both decided that the other one cannot survive being asked.",
                "訊かれたら相手はもたない、って二人とも決めただけ。",
                "این دو نفرند که هر دو تصمیم گرفته‌اند آن یکی تابِ پرسیده‌شدن را ندارد.");

            Hold(2.2f);

            Decide(
                "Ask him tomorrow. Really ask him.",
                "明日、訊きなよ。ちゃんと訊いて。",
                "فردا ازش بپرس. واقعاً بپرس.",

                "Do not ask him anything. Keep it exactly like this.",
                "何も訊かないで。このままにしておいて。",
                "هیچی ازش نپرس. دقیقاً همین‌طوری نگهش دار.",

                "Keep it exactly like this.",
                "……このままにしておいて。",
                "دقیقاً همین‌طوری نگهش دار.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Keep it exactly like this.",
                "このままにしておいて。",
                "دقیقاً همین‌طوری نگهش دار.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Nothing is wrong. Do you understand me? Nothing is wrong.",
                "何も悪くない。分かる？　何も悪くないの。",
                "هیچی اشکالی نداره. می‌فهمی؟ هیچی اشکالی نداره.");

            Hold(2.0f);

            StopMusic();

            SayWithSound(Speaker.Yua, Portrait.Unchanged, SfxId.StringPull, 0.70f,
                "Say nothing is wrong.",
                "何も悪くないって言って。",
                "بگو هیچی اشکالی نداره.");

            Hold(3.0f);

            Narrate(
                "That was the quiet one. There are four left, and the next one starts with Haru turning round.",
                "これが静かな幕だった。あと四つある。次はハルが振り向くところから始まる。",
                "این پرده‌ی آرام بود. چهار تا مانده، و بعدی از جایی شروع می‌شود که هارو برمی‌گردد.");

            Hold(3.0f);
        }
    }
}
