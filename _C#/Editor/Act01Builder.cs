// -----------------------------------------------------------------------------
//  The Frayed Red String
//  Act01Builder.cs  (Editor only)
//
//  Act one — Cherry Blossom Mirage (桜の幻) — written out as code.
//
//  Run it once from The Frayed Red String ▸ Build Act 01 From The Story
//  Document. It writes Assets/Story/Acts/Act01.asset, reusing the asset already
//  at that path if there is one, and then hands over to the Story Editor.
//
//  Act one had no builder until now. Every other act in this folder rebuilds
//  itself from a script; act one was the one hand-written proof that the Story
//  Editor alone was enough, and PrepareWholeGame's one-press setup still knows
//  it by name and logs a note about it instead of touching it. This is the
//  same act — the first day of a Japanese high school, told from behind Yua's
//  eyes — written out so it can be rebuilt the way the rest of the game is.
//
//  What the document it was written from is quiet about, on purpose:
//
//    • The player is meant to believe this is the first time Yua and Haru have
//      ever stood next to each other. They do not act like it. They are
//      already "-pi" to one another before the first line is spoken, and
//      neither the script nor the game explains why.
//    • No blue or green button anywhere in this act. Act two is where Yua
//      hands the player that choice and explains what it is for; here there is
//      only one road through every scene, same as there will only seem to be.
//    • Once — for about two-tenths of a second, right when Haru's leg catches
//      the desk — Yua's face goes somewhere else before she puts the smile
//      back on. Nothing in the scene remarks on it. It is the first of many.
//
//  Every line said out loud is broken into short, single-thought beats rather
//  than paragraphs — nobody's wording changed, only how many clicks it takes
//  to get through it. A split only happens where the line already had a full
//  stop, an exclamation, a question mark, or a trailing "…" of its own; nothing
//  is cut open in the middle of a clause to make that true.
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Narrative;
using UnityEditor;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Act one's script.</summary>
    public sealed class Act01Builder : ActScriptWriter
    {
        protected override int ActNumber => 1;

        protected override string AssetName => "Act01";

        protected override LocalizedLine Title => L("Cherry Blossom Mirage", "桜の幻", "سرابِ شکوفه‌های گیلاس");

        [MenuItem("The Frayed Red String/Build Act 01 From The Story Document")]
        public static void Build()
        {
            new Act01Builder().BuildAsset();
        }

        /// <summary>Builds the act under a given policy, for the one-press setup.</summary>
        public static void Build(ActScriptWriter.RebuildPolicy policy)
        {
            new Act01Builder().BuildAsset(policy);
        }

        /// <summary>
        /// The act, in order: the alley, the classroom, the rooftop at lunch,
        /// the corridor at the end of the day, the café, and the walk home.
        /// </summary>
        protected override void Write()
        {
            WriteAlley();
            WriteClassroom();
            WriteRooftop();
            WriteCorridor();
            WriteCafe();
            WriteWayHome();
        }

        // ---------------------------------------------------------------------
        //  Yua talking to no one
        // ---------------------------------------------------------------------

        /// <summary>Characters per second for <see cref="InnerVoice"/> lines.</summary>
        /// <remarks>The normal rate is 45. This is slow enough to read as thinking rather than speaking, without dragging.</remarks>
        private const float InnerMonologueTypeSpeed = 28f;

        /// <summary>
        /// Yua talking to no one but herself — the line that opens the act and
        /// the line that closes it, and nothing else in between.
        /// </summary>
        /// <remarks>
        /// Typed slower than anything either of them says out loud. The design
        /// document does not give an interior thought its own beat kind the way
        /// a fourth-wall aside eventually gets one from act two on, and this act
        /// has no business reaching for that mode early — so this borrows
        /// <see cref="BeatData.TypeSpeed"/> instead, which is the smallest
        /// change that reads as quieter without being anything but an ordinary
        /// line underneath it.
        /// </remarks>
        private void InnerVoice(Portrait portrait, string english, string japanese, string persian)
        {
            Say(Speaker.Yua, portrait, english, japanese, persian);
            Script[Script.Count - 1].TypeSpeed = InnerMonologueTypeSpeed;
        }

        // ---------------------------------------------------------------------
        //  The alley
        // ---------------------------------------------------------------------

        private void WriteAlley()
        {
            Place(
                Backgrounds.SchoolAlleyDay,
                "The school path", "桜並木", "کوچه‌ی مدرسه");

            Hold(1.8f);

            Narrate(
                "Spring had turned the walk to school into a tunnel of pink and white, and somewhere past the blossoms, the first day of high school was waiting.",
                "春は通学路を桜色のトンネルに変えていた。花びらの向こうで、高校生活の初日が待っていた。",
                "بهار کوچه‌ی مدرسه را به تونلی از شکوفه‌های صورتی و سفید تبدیل کرده بود، و آن‌طرفِ شکوفه‌ها، اولین روز دبیرستان منتظر بود.");

            InnerVoice(Portrait.Unchanged,
                "Finally — the first day of high school!",
                "やっと、高校の初日だ!",
                "بالاخره، اولین روز دبیرستان!");

            Enter(Speaker.Yua, Portrait.Neutral);

            // Beat 2.
            Say(Speaker.Yua, Portrait.Unchanged,
                "A new school, and a whole bunch of new people...",
                "新しい学校に、たくさんの新しい顔ぶれ。",
                "یه مدرسه‌ی تازه و کلی آدم جدید...");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Though honestly, I'm only looking for one particular friend.",
                "まあ、私はただ一人の特別な友達を探してるだけなんだけどね。",
                "هرچند من فقط دنبال یه دوستِ خاصم.");

            // Beat 3.
            Say(Speaker.Yua, Portrait.Joyful,
                "Look at him.",
                "見て。",
                "نگاش کن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Waiting all alone under the blossoms...",
                "一人で花びらの下で待ってる……",
                "تنها زیر شکوفه‌ها منتظره...");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Exactly the way I wanted it.",
                "まさに私が望んでた通り!",
                "دقیقاً همون‌طوری که می‌خواستم!");

            Enter(Speaker.Haru, Portrait.Neutral);

            // Beat 4.
            Say(Speaker.Yua, Portrait.Unchanged,
                "Hey, Haru-pi!",
                "ハルぴ、おはよう!",
                "سلام هارو‌پی!");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Want to hurry to class together so we can grab the window seats?",
                "一緒に急いで教室行かない? 窓際の席、取れるように!",
                "می‌خوای زودی باهم بریم تو کلاس تا جاهای کنار پنجره رو بگیریم؟");

            // Beat 5.
            Say(Speaker.Haru, Portrait.Shy,
                "Hey, Yua-pi!",
                "結愛ぴ、おはよう!",
                "سلام یوآ‌پی!");

            Say(Speaker.Haru, Portrait.Unchanged,
                "That was a little sudden...",
                "ちょっと急だったけど……",
                "یه‌کم یهویی بود...");

            Say(Speaker.Haru, Portrait.Unchanged,
                "But okay!",
                "うん、いいよ!",
                "ولی باشه!");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's still early, so we can definitely get the window seats.",
                "まだ早いから、窓際の席、絶対取れるよ。",
                "الآن زوده، پس حتماً می‌تونیم صندلی‌های کنار پنجره رو بگیریم!");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Oh — you got here early, by the way!",
                "あ、そういえば結愛ぴこそ早かったね!",
                "راستی، زود اومدی‌ها!");

            Enter(Speaker.Haru, Portrait.Joyful);

            // Beat 6.
            Say(Speaker.Yua, Portrait.Shy,
                "I didn't come that early.",
                "そんなに早くないよ。",
                "خیلی هم زود نیومدم؛");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Honestly, you're the one who's always trying to be early for everything!",
                "ていうか、いつも何でも早く済ませようとしてるのはハルぴの方でしょ!",
                "راستش تو همیشه سعی می‌کنی خودتو به همه‌چیز زود برسونی!");

            // Beat 7.
            Say(Speaker.Haru, Portrait.Unchanged,
                "Yeah, maybe you've got a point.",
                "うん、まあそうかもね。",
                "آره، شاید حق با تو باشه؛");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I guess I'm a little afraid of not making it in time, so I always rush.",
                "ちょっと、間に合わないのが怖いっていうか……だからいつも急いじゃって、",
                "کلاً یه‌کم می‌شه گفت ترسِ نرسیدن دارم، واسه همینه که همیشه عجله می‌کنم؛");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's turned into a habit.",
                "癖になってるんだ。",
                "عادتم شده!");

            // Beat 8.
            Say(Speaker.Yua, Portrait.Joyful,
                "That's an interesting habit.",
                "面白い癖だね。",
                "عادتِ جالبیه،");

            Say(Speaker.Yua, Portrait.Unchanged,
                "But try not to be too hard on yourself.",
                "でも、あんまり自分を追い詰めないでよ。",
                "اما سعی کن خیلی به خودت سخت نگیری.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Sometimes strange things happen and you won't make it no matter how much you rush.",
                "時々、思いがけないことが起きて、どれだけ急いでも間に合わないことだってあるんだから。",
                "بعضی موقع‌ها اتفاقای عجیبی پیش میاد، ممکنه به هر حال نرسی؛");

            Say(Speaker.Yua, Portrait.Unchanged,
                "A habit like that just makes moments like those so much harder on you.",
                "そんな癖のままだと、そういう時に自分がすごく辛くなっちゃうよ!",
                "اگه این عادت رو داشته باشی، اون‌جور موقع‌ها خیلی بهت سخت می‌گذره!");

            // Beat 9.
            Say(Speaker.Haru, Portrait.Shy,
                "You're right.",
                "そうだね。",
                "حق با توئه؛");

            Say(Speaker.Haru, Portrait.Unchanged,
                "We'd better get to class before we're late!",
                "遅れないうちに、早く教室行こう!",
                "بهتره زودتر بریم تو کلاس تا دیر نشده!");
        }

        // ---------------------------------------------------------------------
        //  The classroom
        // ---------------------------------------------------------------------

        private void WriteClassroom()
        {
            ClearStage();

            Place(
                Backgrounds.ClassroomDay,
                "Class 1-A", "1年A組", "کلاس اول الف");

            Hold(1.6f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Joyful);

            // Beat 10.
            Say(Speaker.Haru, Portrait.Unchanged,
                "Look at that — we both ended up in class 1-A, and we got the two window seats.",
                "見て、二人とも一年A組になったよ! しかも窓際の席、二つとも取れた!",
                "ببین، جفتمون تو کلاس اول‌الف افتادیم و دوتا صندلی‌های کنار پنجره رو هم گرفتیم؛");

            Say(Speaker.Haru, Portrait.Unchanged,
                "That worked out perfectly!",
                "すごくうまくいったね!",
                "خیلی خوب شد!");

            // Beat 11.
            Say(Speaker.Yua, Portrait.Joyful,
                "I didn't think you'd be this excited about the window seat, just like me!",
                "ハルぴがそんなに窓際の席に喜んでくれるなんて思わなかった、私と同じくらい!",
                "فکر نمی‌کردم مثل من اینقدر ذوقِ صندلی کنار پنجره رو داشته باشی!");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Well — I'm pretty thrilled myself.",
                "まあ、私もすっごく嬉しいけどね!",
                "خب، خودمم که حسابی خوشحالم!");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            // Beat 12 — the narrator, not split further: it was already one short sentence.
            SayWithSound(Speaker.Narrator, Portrait.Unchanged, SfxId.LegAche, 0.5f,
                "Right as Haru goes to sit down, his left leg catches the corner of the desk.",
                "ハルが座ろうとした瞬間、左足が机の角にぶつかる。",
                "همین‌که هارو می‌خواد بشینه، پای چپش کوبیده می‌شود به میز.");

            Enter(Speaker.Haru, Portrait.Injured);

            // Beat 13.
            Say(Speaker.Yua, Portrait.Sad,
                "Are you okay?",
                "大丈夫?",
                "خوبی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "What happened?",
                "どうしたの?",
                "چی شد؟");

            // Beat 14.
            Say(Speaker.Haru, Portrait.Shy,
                "I'm fine.",
                "大丈夫、",
                "خوبم؛");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I just got a little distracted and banged my leg on the desk.",
                "ちょっとぼーっとしてて机に足ぶつけただけ。",
                "فقط یه‌کم حواسم پرت شد و پامو کوبیدم به میز.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "So clumsy of me!",
                "……不器用だなあ、僕。",
                "دست‌وپا چلفتیم!");

            // The mask slips for two-tenths of a second — the same tell the
            // game will spend the next several acts training the player to
            // watch for — before she catches it and puts the smile back on.
            Enter(Speaker.Yua, Portrait.DeadEyes);
            Hold(0.2f);
            Enter(Speaker.Yua, Portrait.Joyful);

            // Beat 15.
            Say(Speaker.Yua, Portrait.Unchanged,
                "Haha, it's fine.",
                "ははっ、大丈夫、",
                "هاها، اشکالی نداره؛");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I know you, I already knew that about you!",
                "知ってるよ、ハルぴのそういうとこ!",
                "می‌شناسمت، می‌دونم اینو!");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Just be more careful next time, okay?",
                "次はもうちょっと気をつけてね?",
                "دفعه‌ی بعد بیشتر مراقب باش، باشه؟");

            // Beat 16.
            Say(Speaker.Haru, Portrait.Joyful,
                "Got it, I will!",
                "うん、気をつける!",
                "چشم، حتماً!");
        }

        // ---------------------------------------------------------------------
        //  The rooftop, at lunch
        // ---------------------------------------------------------------------

        private void WriteRooftop()
        {
            ClearStage();

            Place(
                Backgrounds.RooftopDay,
                "The rooftop", "屋上", "پشت‌بام مدرسه");

            Hold(2.0f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            // Beat 17.
            Say(Speaker.Yua, Portrait.Unchanged,
                "Finally — after all those classes, it's lunchtime!",
                "やっと、あれだけの授業のあとにお昼だ!",
                "بالاخره بعد از اون همه کلاس وقتِ ناهار شد!");

            Say(Speaker.Yua, Portrait.Unchanged,
                "So, tell me, what did you bring?",
                "ねえ、お昼何持ってきたの?",
                "بگو ببینم، ناهار چی آوردی؟");

            // Beat 18.
            Say(Speaker.Haru, Portrait.Joyful,
                "I didn't bring anything today.",
                "今日は持ってきてないんだ。",
                "ناهار امروز نیاوردم؛");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I wanted to stay focused on my classes.",
                "授業に集中したくて……",
                "می‌خواستم تمرکزم بیشتر روی درسم باشه،");

            Say(Speaker.Haru, Portrait.Unchanged,
                "If I eat, I get sleepy and can't concentrate.",
                "食べると眠くなって、勉強できなくなるから。",
                "اگه غذا می‌خوردم خوابم می‌گرفت و نمی‌تونستم درس بخونم.");

            Enter(Speaker.Haru, Portrait.Neutral);

            // Beat 19.
            Say(Speaker.Yua, Portrait.Joyful,
                "You're a terrible liar, Haru-pi!",
                "ハルぴ、嘘つくの下手だね!",
                "دروغ‌گوی خوبی نیستی، هارو‌پی!");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I bet you rushed out the door and forgot your lunch again.",
                "どうせ急いでて、お弁当忘れたんでしょ。",
                "مطمئنم بخاطر عجله کردن غذاتو جا گذاشتی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Come on, you can share mine!",
                "ほら、私のシェアしていいよ!",
                "بیا، می‌تونی غذامو باهام شریک بشی!");

            Enter(Speaker.Yua, Portrait.Neutral);

            // Beat 20.
            Say(Speaker.Haru, Portrait.Shy,
                "Okay, honestly?",
                "……正直に言うと、",
                "اگه بخوام راستشو بگم،");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Yeah, you're right.",
                "うん、その通り。",
                "آره، حق با توئه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Sorry.",
                "ごめん、",
                "شرمنده؛");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I'll try not to forget it again starting tomorrow.",
                "明日からは忘れないようにする。",
                "از فردا سعی می‌کنم غذامو جا نذارم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Thanks for sharing.",
                "……ご飯、ありがとう。",
                "بابت غذا ممنون!");

            Hold(2.2f);

            Enter(Speaker.Haru, Portrait.Joyful);

            // Beat 21.
            Say(Speaker.Yua, Portrait.Joyful,
                "Now that you've eaten, come on!",
                "食べ終わったんだから、早く教室戻ろう、",
                "حالا که غذاتو خوردی، بدو بریم سرِ کلاس؛");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Let's get to class, we're going to be late!",
                "遅れちゃう!",
                "نمی‌رسیم!");

            // Beat 22.
            Say(Speaker.Haru, Portrait.Unchanged,
                "Thanks for lunch, it was really good!",
                "ご飯、本当に美味しかった、ありがとう!",
                "ممنون بابتِ غذا، فوق‌العاده بود!");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Let's go!",
                "行こう!",
                "بریم!");

            // Beat 23.
            Say(Speaker.Yua, Portrait.Shy,
                "Let's go!",
                "行こう!",
                "بریم!");

            Hold(2.2f);
        }

        // ---------------------------------------------------------------------
        //  The corridor, at the end of the day
        // ---------------------------------------------------------------------

        private void WriteCorridor()
        {
            ClearStage();

            Place(
                Backgrounds.CorridorSunset,
                "The corridor", "廊下", "راهروی مدرسه");

            Hold(1.8f);

            Enter(Speaker.Yua, Portrait.Joyful);
            Enter(Speaker.Haru, Portrait.Shy);

            // Beat 24.
            Say(Speaker.Yua, Portrait.Unchanged,
                "The first day is finally over, Haru-pi!",
                "やっと初日終わったね、ハルぴ!",
                "بالاخره اولین روز تموم شد، هارو‌پی!");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Come on, let's head home!",
                "さ、帰ろう!",
                "بدو بریم سمتِ خونه!");

            Say(Speaker.Yua, Portrait.Unchanged,
                "...Also, what's with that flustered face you're making?",
                "……ていうか、その照れた顔なに?",
                "بعدشم، این چه قیافه‌ی خجالتی‌ایه که گرفتی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Are you some kind of pervert?",
                "もしかして変態?",
                "منحرفی چیزی هستی؟");

            // Beat 25.
            Say(Speaker.Haru, Portrait.Joyful,
                "A pervert?",
                "変態?",
                "منحرف؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You say the strangest things!",
                "変なこと言うなあ!",
                "حرفای عجیب می‌زنی!");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's nothing, really.",
                "何でもないよ、",
                "چیزی نیست،");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I just get nervous every time I'm about to walk somewhere with you.",
                "ただ……一緒に歩くとなると、いつも緊張しちゃうだけで!",
                "فقط هر وقت می‌خوام باهات برم استرس می‌گیرم!");

            Enter(Speaker.Haru, Portrait.Shy);

            // Beat 26.
            Say(Speaker.Yua, Portrait.Shy,
                "Okay, okay!",
                "はいはい、",
                "باشه باشه،");

            Say(Speaker.Yua, Portrait.Unchanged,
                "If you've got a crush on me, you don't have to be shy about it!",
                "私に片想いしてるなら、恥ずかしがらなくていいのに!",
                "اگه روم کراش داری لازم نیست خجالت بکشی!");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Come on, let's get home before you melt from embarrassment.",
                "ほら、恥ずかしさで溶けちゃう前に帰ろう!",
                "بدو بریم سمتِ خونه تا از خجالت آب نشدی!");

            Enter(Speaker.Haru, Portrait.Neutral);

            // Beat 27.
            Say(Speaker.Yua, Portrait.Unchanged,
                "I know that came out of nowhere, but forget going straight home.",
                "急にごめんね、でも家のことは置いといて!",
                "می‌دونم یهویی شد، اما خونه رو ولش کن!");

            Say(Speaker.Yua, Portrait.Unchanged,
                "If you're that stressed, I want to fix it.",
                "そんなに緊張してるなら、はやくどうにかしてあげたい。",
                "اگه استرس داری، من می‌خوام زودتر این استرس از بین بره؛");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I know a café.",
                "……知ってる喫茶店があるんだけど、",
                "یه کافه می‌شناسم،");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Want to go?",
                "行く?",
                "می‌خوای بریم؟");

            Enter(Speaker.Yua, Portrait.Joyful);

            // Beat 28.
            Say(Speaker.Haru, Portrait.Joyful,
                "I honestly have no idea how you got to that conclusion.",
                "どうしてそうなるのか、正直全然わからないけど……",
                "واقعاً نمی‌دونم چطوری به این نتیجه رسیدی،");

            Say(Speaker.Haru, Portrait.Unchanged,
                "But sure — let's go wherever you say.",
                "まあいいよ、結愛ぴが言うとこに行こう。",
                "ولی قبوله؛ بریم اونجایی که تو می‌گی!");

            Hold(2.2f);
        }

        // ---------------------------------------------------------------------
        //  The café
        // ---------------------------------------------------------------------

        /// <remarks>
        /// The same café, in fact — a sunny first visit to the place act two
        /// will send them back to in the rain and call by name. Backgrounds.md
        /// lists <see cref="Backgrounds.CafeDay"/> and
        /// <see cref="Backgrounds.CafeRainy"/> as the same pastel interior under
        /// different weather, so this act is where the two of them find it.
        /// </remarks>
        private void WriteCafe()
        {
            ClearStage();

            Place(
                Backgrounds.CafeDay,
                "Usagi Café", "うさぎ喫茶", "کافه اوساگی");

            Hold(1.6f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Joyful);

            // Beat 29 — already one short line.
            Say(Speaker.Haru, Portrait.Unchanged,
                "Thanks for bringing me here!",
                "ここに連れてきてくれてありがとう!",
                "ممنونم که منو دعوت کردی این‌جا!");

            Enter(Speaker.Haru, Portrait.Neutral);

            // Beat 30.
            Say(Speaker.Yua, Portrait.Joyful,
                "You're welcome!",
                "どういたしまして!",
                "خواهش می‌کنم!");

            Say(Speaker.Yua, Portrait.Unchanged,
                "So, what do you want?",
                "何飲む?",
                "چی می‌خوای بخوری؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "...Actually, never mind.",
                "……ううん、いいや、",
                "ولش کن،");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I'll just order you a matcha, and a bubble tea for me.",
                "ハルぴには抹茶、私にはタピオカ、勝手に頼んじゃうね!",
                "خودم واست ماچا سفارش می‌دم و واسه‌ی خودم بابل‌تی!");

            Enter(Speaker.Yua, Portrait.Neutral);

            // Beat 31.
            Say(Speaker.Haru, Portrait.Joyful,
                "Thank you so much.",
                "本当にありがとう、",
                "خیلی ممنونم؛");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Those are really good choices!",
                "すごくいい選択だね!",
                "واقعاً انتخابای خوبین!");

            Hold(2.2f);
        }

        // ---------------------------------------------------------------------
        //  The way home
        // ---------------------------------------------------------------------

        private void WriteWayHome()
        {
            ClearStage();

            Place(
                Backgrounds.VendingStreetDay,
                "The way home", "帰り道", "مسیرِ خونه");

            Hold(2.0f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            // Beat 32.
            Say(Speaker.Yua, Portrait.Unchanged,
                "We finally made it home!",
                "やっと家に着いたね!",
                "بالاخره دیگه رسیدیم خونه!");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Today was such a good day.",
                "今日は本当にいい日だった、",
                "امروز روزِ خیلی خوبی بود؛");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Don't you think so, Haru-pi?",
                "そう思わない? ハルぴ。",
                "این‌طور فکر نمی‌کنی، هارو‌پی؟");

            // Beat 33.
            Say(Speaker.Haru, Portrait.Joyful,
                "You're right, it was a great first day!",
                "だね、初日にしてはすごく良かった!",
                "همین‌طوره؛ واسه‌ی روزِ اول خیلی خوب بود!");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I should get going — I'll head home now.",
                "じゃあ、僕はもう家に帰るね!",
                "من دیگه می‌رم خونمون!");

            Enter(Speaker.Haru, Portrait.Neutral);

            // Beat 34.
            Say(Speaker.Yua, Portrait.Unchanged,
                "See you tomorrow.",
                "また明日ね、",
                "فردا می‌بینمت؛");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Take care!",
                "気をつけて!",
                "مراقبِ خودت باش!");

            Hold(1.8f);

            Exit(Speaker.Haru);

            Hold(1.4f);

            StopMusic();

            // Beat 35 — the closing monologue, taken slowly on purpose.
            InnerVoice(Portrait.Joyful,
                "Today was such a good day.",
                "今日は本当にいい日だった。",
                "امروز روزِ خیلی خوبی بود!");

            InnerVoice(Portrait.Unchanged,
                "At school, I got to feed him.",
                "学校では彼にご飯をあげられたし、",
                "هم توی مدرسه تونستم بهش غذا بدم،");

            InnerVoice(Portrait.Unchanged,
                "And at the café...",
                "喫茶店でも――",
                "هم توی کافه؛");

            InnerVoice(Portrait.Unchanged,
                "Even though he can't stand the taste of matcha...",
                "抹茶の味、彼は本当は苦手なくせに――",
                "با این‌که از مزه‌ی ماچا اصلاً خوشش نمی‌اومد،");

            InnerVoice(Portrait.Unchanged,
                "He let me order for him anyway, and he even complimented my choice.",
                "私に勝手に頼ませてくれて、しかも選んだものを褒めてくれた。",
                "گذاشت من واسش سفارش بدم و حتی از انتخابم تعریف کرد!");

            InnerVoice(Portrait.Unchanged,
                "It's so nice when he listens like that.",
                "こうやって言うことを聞いてくれるの、すごくいいな。",
                "خیلی خوبه وقتی این‌طوری حرف گوش می‌کنه؛");

            InnerVoice(Portrait.Unchanged,
                "I wish he'd always stay exactly this way.",
                "ずっとこのままでいてくれたらいいのに。",
                "ای‌کاش همیشه همین‌طوری بمونه!");

            Hold(2.6f);
        }
    }
}