// -----------------------------------------------------------------------------
//  The Frayed Red String
//  Act02Builder.cs  (Editor only)
//
//  Act two — Maboroshi (幻) — written out as code.
//
//  Run it once from The Frayed Red String ▸ Build Act 02 From The Story
//  Document. It writes Assets/Story/Acts/Act02.asset, reusing the asset that is
//  already there so nothing pointing at it breaks, and then hands over to the
//  Story Editor for good.
//
//  What act two adds to the game, beyond its own script:
//
//    • The blue and green buttons, used for the first time. Act one has none,
//      and the reveal only lands if the player has never seen one before Yua
//      explains them.
//    • Yua's override. Take the blue option and she takes it back — her face
//      goes flat for a beat and the scene carries on the green way regardless.
//      The press is still counted as kind: the endings are decided by what the
//      player reached for, not by what she allowed them to have.
//    • The two silences. One where the machine room reaches her through a road,
//      one where his leg stops him walking. Neither asks for anything. A player
//      who sits with either of them for five minutes is counted as having
//      listened, and the endings read that count.
//
//  The verbs the script below is written in — Say, Narrate, Hold, Place, Decide
//  — are in ActScriptWriter, along with everything about writing the asset.
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Narrative;
using UnityEditor;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Act two's script.</summary>
    public sealed class Act02Builder : ActScriptWriter
    {
        protected override int ActNumber => 2;

        protected override string AssetName => "Act02";

        protected override LocalizedLine Title => L("Maboroshi", "幻", "مابوروشی");

        [MenuItem("The Frayed Red String/Build Act 02 From The Story Document")]
        public static void Build()
        {
            new Act02Builder().BuildAsset();
        }

        /// <summary>Builds the act under a given policy, for the one-press setup.</summary>
        public static void Build(ActScriptWriter.RebuildPolicy policy)
        {
            new Act02Builder().BuildAsset(policy);
        }

        /// <summary>
        /// The act, in order.
        /// </summary>
        /// <remarks>
        /// Haru's monologue in the café is broken into short lines on purpose.
        /// It is one speech in the design document and it has to stay one
        /// speech, but a wall of it in a dialogue box is a wall the player
        /// clicks through. Broken at the places he would actually stop, it is
        /// paced by the person reading it — which is the only way the last line
        /// of it lands.
        /// </remarks>
        protected override void Write()
        {
            WriteClassroom();
            WriteCafe();
            WriteWayHome();
            WriteYuaRoom();
            WriteFirstChoice();
            WriteCorridor();
            WriteCafeAgain();
            WritePlatform();
        }

        /// <summary>Act one closes in spring rain; act two opens in it.</summary>
        private void WriteClassroom()
        {
            Place(
                Backgrounds.ClassroomRainy,
                "The classroom", "教室", "کلاس درس");

            Hold(2.2f);

            Narrate(
                "It had been raining since before either of them woke up. Nobody had opened the windows, and the room had gone the colour of an old photograph.",
                "二人が目を覚ますよりも前から、雨は降っていた。誰も窓を開けず、教室は古い写真のような色になっていた。",
                "از قبل از اینکه هیچ‌کدامشان بیدار شوند باران می‌بارید. کسی پنجره‌ها را باز نکرده بود و کلاس رنگ یک عکس قدیمی را گرفته بود.");

            Narrate(
                "Three weeks since the first day. Long enough for a desk to become your desk, and for a boy by the window to become the boy by the window.",
                "始業式から三週間。机が自分の机になり、窓際の男の子が「あの子」になるには、十分な時間だった。",
                "سه هفته از روز اول گذشته بود. آن‌قدر که یک میز بشود میزِ تو، و پسری کنار پنجره بشود آن پسرِ کنار پنجره.");

            Enter(Speaker.Yua, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Joyful,
                "Haru-pi. Haru-pi. You have been looking at that window for eleven minutes.",
                "ハルぴ。ハルぴってば。その窓、もう十一分も見てるよ。",
                "هارو‌پی. هارو‌پی. یازده دقیقه‌ست داری به اون پنجره نگاه می‌کنی.");

            Enter(Speaker.Haru, Portrait.Shy);

            Say(Speaker.Haru, Portrait.Unchanged,
                "You counted?",
                "数えてたの？",
                "شمردی؟");

            Say(Speaker.Yua, Portrait.Neutral,
                "Of course I counted.",
                "もちろん数えてた。",
                "معلومه که شمردم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That is a little frightening.",
                "それ、ちょっと怖いよ。",
                "این یه‌کم ترسناکه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It is a little romantic. Say thank you.",
                "ちょっとロマンチック、でしょ。ありがとうは？",
                "یه‌کم رمانتیکه. بگو ممنون.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Thank you.",
                "ありがとう。",
                "ممنون.");

            Hold(1.4f);

            Narrate(
                "Something moved behind his eyes and settled again. She saw it. She always sees it.",
                "彼の目の奥で何かが動き、また静かになった。結愛は見ていた。結愛はいつも見ている。",
                "چیزی پشت چشم‌هایش تکان خورد و دوباره سر جایش نشست. یوآ دید. یوآ همیشه می‌بیند.");

            Say(Speaker.Yua, Portrait.Neutral,
                "So what were you actually thinking about?",
                "で、本当は何を考えてたの？",
                "خب، واقعاً به چی فکر می‌کردی؟");

            Say(Speaker.Haru, Portrait.Neutral,
                "Nothing. Rain makes me quiet, that is all.",
                "別に。雨の日は静かになるだけだよ。",
                "هیچی. بارون منو ساکت می‌کنه، همین.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Rain makes you a liar, Haru-pi.",
                "雨はハルぴを嘘つきにするの。",
                "بارون تو رو دروغگو می‌کنه، هارو‌پی.");

            Say(Speaker.Haru, Portrait.Shy,
                "It makes me quiet.",
                "……静かになるだけだよ。",
                "…ساکت می‌کنه.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Neutral,
                "The cafe. After class. The one with the terrible umbrella stand.",
                "放課後、あの喫茶店。傘立てがひどいところ。",
                "کافه. بعد از کلاس. همون که جاچتری‌اش افتضاحه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "You are not asking.",
                "それ、訊いてないよね。",
                "داری نمی‌پرسی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I never ask. I announce, and you agree. It is a very efficient relationship.",
                "訊かないよ。私が決めて、ハルぴが頷く。とても効率のいい関係でしょ。",
                "من هیچ‌وقت نمی‌پرسم. اعلام می‌کنم و تو قبول می‌کنی. رابطه‌ی خیلی کارآمدیه.");

            SayWithSound(Speaker.Haru, Portrait.Unchanged, SfxId.Petal, 0.45f,
                "Okay.",
                "……うん。",
                "…باشه.");

            Hold(1.6f);
        }

        /// <summary>
        /// The café, and the story Haru has been carrying since he was fourteen.
        /// </summary>
        /// <remarks>
        /// The monologue is broken into short lines on purpose. It is one speech
        /// in the design document and it has to stay one speech, but a wall of
        /// it in a dialogue box is a wall the player clicks through. Broken at
        /// the places he would actually stop, it is paced by the person reading
        /// it — which is the only way the last line of it lands.
        /// </remarks>
        private void WriteCafe()
        {
            Maybe(LegAche, 0.30f);

            ClearStage();

            Place(
                Backgrounds.CafeRainy,
                "Usagi Café", "うさぎ喫茶", "کافه اوساگی");

            Hold(2.0f);

            Narrate(
                "Rain down the window, and the lights inside turned up too warm to argue with.",
                "窓を雨がつたい、店の灯りは反論できないほど暖かかった。",
                "باران روی شیشه سُر می‌خورد و چراغ‌های داخل آن‌قدر گرم بودند که نشود با آن‌ها بحث کرد.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Joyful,
                "Matcha cake. Two forks. I ordered before you sat down.",
                "抹茶ケーキ。フォーク二本。座る前に頼んでおいた。",
                "کیک ماچا. دو تا چنگال. قبل از اینکه بشینی سفارش دادم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Before I stood up, I think.",
                "……立つ前じゃない？",
                "فکر کنم قبل از اینکه بلند شم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Before you stood up.",
                "立つ前だね。",
                "قبل از اینکه بلند شی.");

            Hold(1.2f);

            Narrate(
                "He turned the fork over. Then again. Then a third time, and ate nothing.",
                "彼はフォークを裏返した。もう一度。三度目も裏返して、何も食べなかった。",
                "چنگال را برگرداند. دوباره. بار سوم هم برگرداند و چیزی نخورد.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Haru-pi.",
                "ハルぴ。",
                "هارو‌پی.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Mm.",
                "うん。",
                "هوم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Four times. Say it.",
                "四回目。言って。",
                "چهار بار. بگو.");

            Say(Speaker.Haru, Portrait.Sad,
                "It is not a happy story.",
                "……楽しい話じゃないよ。",
                "…داستان قشنگی نیست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I like the ones that are not happy.",
                "楽しくない話、好き。",
                "من از داستان‌هایی که قشنگ نیستند خوشم میاد.");

            Narrate(
                "She said it lightly. She meant it precisely.",
                "軽い口調だった。言葉どおりの意味だった。",
                "سبک گفتش. دقیقاً همان را هم منظور داشت.");

            Hold(2.0f);

            // --- the monologue ------------------------------------------------

            Say(Speaker.Haru, Portrait.Sad,
                "First of all — I am sorry that I ask you so many questions. When you say you are not all right. Or when I only feel that you are not.",
                "まず……たくさん訊いてごめん。君が「大丈夫じゃない」って言うとき。あるいは、そう感じるだけのときも。",
                "اول از همه ببخشید که این‌قدر سؤال‌پیچت می‌کنم. وقت‌هایی که می‌گی حالت خوب نیست. یا وقت‌هایی که فقط من این‌طور حس می‌کنم که حالت خوب نیست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I want to know. That is all it is.",
                "知りたいんだ。それだけなんだ。",
                "می‌خوام بدونم. فقط همین.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I had a best friend. Same class from second grade to eighth. We talked every day.",
                "親友がいた。小二から中二まで、ずっと同じクラスで、毎日たくさん話した。",
                "یه رفیق صمیمی داشتم. از کلاس دوم دبستان تا کلاس هشتم همکلاسی بودیم. هر روز کلی صحبت می‌کردیم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Except on the days he was not doing well. On those days he did not have it in him to talk.",
                "調子が良くない日を除いて。そういう日は、話す元気がなかった。",
                "بجز بعضی وقت‌ها که حالش خوب نبود و حال نداشت زیاد صحبت کنه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I knew he was sad. I asked him. I kept asking. He was not someone who talked.",
                "落ち込んでるのは分かってた。何度も訊いた。でも彼は、あまり話す人じゃなかった。",
                "می‌دونستم ناراحته. ازش می‌پرسیدم. مدام می‌پرسیدم. اما از اون آدم‌هایی نبود که زیاد صحبت کنه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I said the same things over and over. Trust me. Tell me. I am right here. I am your friend.",
                "同じことを何度も言った。信じてよ。話してよ。ここにいるよ。友達だろ。",
                "همیشه بهش می‌گفتم: بهم اعتماد کن. بهم بگو. من کنارتم. من رفیقتم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "If not me, then who? Do not keep it inside. If you ever want to talk, I am not going anywhere.",
                "俺に言わないなら、誰に言うんだ。ひとりで抱えるなよ。話したくなったら、俺はどこにも行かないから。",
                "به من نگی می‌خوای به کی بگی؟ نباید بریزی تو خودت. هر وقت خواستی با کسی صحبت کنی، من همیشه همین‌جام.");

            Hold(1.5f);

            Say(Speaker.Haru, Portrait.Sad,
                "He still did not talk. He never told me what hurt. I did not understand why.",
                "それでも話してくれなかった。何が痛いのか、一度も言わなかった。理由が分からなかった。",
                "اما بازم زیاد صحبت نمی‌کرد. از دردهاش نمی‌گفت. نمی‌فهمیدم چرا.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Was I not someone he could trust? Was I not his friend? Did he not see me as one?",
                "俺は信用できなかった？ 友達じゃなかった？ そう見てもらえてなかった？",
                "من قابل اعتماد نبودم؟ من دوستش نبودم؟ منو رفیق خودش نمی‌دید؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Did he think I was a child who would not understand? That I had my own problems and no room left to listen?",
                "俺を、分からない子どもだと思ってた？ 自分の問題で手一杯で、聞く余裕なんてないと思ってた？",
                "فکر می‌کرد من بچه‌ام و درک نمی‌کنم؟ فکر می‌کرد من مشکلات خودم رو دارم و حال گوش دادن به کس دیگه‌ای رو ندارم؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "That if he told me, it would come back to him as pity?",
                "話したら、同情されるだけだと思ってた？",
                "فکر می‌کرد اگه بهم بگه، انگار دارم بهش ترحم می‌کنم؟");

            Say(Speaker.Haru, Portrait.Sad,
                "I do not know. I did everything I could think of so that none of those thoughts could reach him.",
                "分からない。そんな考えが彼に届かないように、思いつくことは全部やった。",
                "نمی‌دونم. تمام تلاشم رو می‌کردم که هیچ‌کدوم از این فکرها سراغش نیاد.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "So it would not feel one-sided, I complained to him too. About my day. About the things that had upset me.",
                "一方通行にならないように、俺も愚痴った。今日のこととか、嫌だったこととか。",
                "حتی برای اینکه احساس نکنه یک‌طرفه‌ست، خودم هم بعضی وقت‌ها ازش غر می‌زدم. از روزم. از چیزهایی که ناراحتم کرده بود.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "He still did not really talk to me.",
                "それでも、ちゃんとは話してくれなかった。",
                "اما بازم درست باهام صحبت نمی‌کرد.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "When he was happy he never stopped talking. When he was sad, nothing.",
                "嬉しいときはよく喋るのに。悲しいときは、何も。",
                "وقتی خوشحال بود پرحرف بود. اما وقت‌هایی که ناراحت بود، نه.");

            Hold(2.0f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "We were not classmates after eighth grade. Guess why.",
                "八年生からは、同じクラスじゃなくなった。理由、当ててみて。",
                "از کلاس هشتم دیگه همکلاسی نشدیم. حدس بزن چرا.");

            Hold(2.4f);

            Say(Speaker.Haru, Portrait.Crying,
                "He killed himself.",
                "自殺したんだ。",
                "خودکشی کرد.");

            Hold(3.0f);

            Narrate(
                "The rain kept doing what rain does. Somewhere behind the counter a kettle came to the boil and was taken off it.",
                "雨は雨のすることを続けていた。カウンターの奥でやかんが沸き、火から下ろされた。",
                "باران کار همیشگی‌اش را می‌کرد. پشت پیشخوان کتری جوش آمد و از روی شعله برداشته شد.");

            Say(Speaker.Haru, Portrait.Sad,
                "I do not know how to ask you this properly. But if anything ever makes you sad — at any time —",
                "うまく頼めないけど。もし君が何かで悲しくなったら、いつでも——",
                "نمی‌دونم چطوری ازت بخوام. اما اگه هر وقت از چیزی ناراحت شدی —");

            Say(Speaker.Haru, Portrait.Unchanged,
                "— or if there is something from before that is still with you —",
                "——あるいは、昔のことで、まだ君の中に残っているものがあるなら——",
                "— یا اگه چیزی از گذشته هست که ازش ناراحتی و هنوز پیشت مونده —");

            SayWithSound(Speaker.Haru, Portrait.Unchanged, SfxId.BoilerRoom, 0.30f,
                "— even if the reason is that I was weak. Even if none of it was your fault. Even if it was dark, and full of loud, sudden noises —",
                "——たとえその理由が、俺が弱かったことでも。たとえ君のせいじゃなかったとしても。たとえそこが暗くて、大きな音が突然する場所だったとしても——",
                "— حتی اگه دلیل ناراحتیت ضعیف بودنِ خودِ من بوده باشه. حتی اگه در اصل تقصیر خودت نبوده باشه. حتی اگه تاریک بوده و پر از صداهای بلند و ناگهانی —");

            Cue(SfxId.Heartbeat, 0.75f);
            Enter(Speaker.Yua, Portrait.DeadEyes);
            Hold(2.2f);

            Narrate(
                "Yua did not move. Her cup was halfway to her mouth and it stayed there.",
                "結愛は動かなかった。口元まで運ばれたカップは、そのまま止まっていた。",
                "یوآ تکان نخورد. فنجانش نیمه‌راهِ دهانش بود و همان‌جا ماند.");

            Narrate(
                "He had not named a place. He had not named a year. He had described a room she has never told anyone about.",
                "彼は場所を言わなかった。年も言わなかった。ただ、彼女が誰にも話したことのない部屋を描写した。",
                "او نه جایی را گفت نه سالی را. فقط اتاقی را توصیف کرد که یوآ هرگز درباره‌اش به کسی چیزی نگفته بود.");

            Hold(1.8f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Go on.",
                "続けて。",
                "ادامه بده.");

            Narrate(
                "She did not ask how he knew. That is the whole of what she did not do, and it took everything she had.",
                "なぜ知っているのかを、彼女は訊かなかった。訊かなかった、ただそれだけのことに、持っているものを全部使った。",
                "نپرسید از کجا می‌داند. تنها کاری که نکرد همین بود، و تمام توانش را برد.");

            Say(Speaker.Haru, Portrait.Sad,
                "I will get on my knees. I will bow. I will beg you. I want you to talk to me.",
                "膝をつくよ。頭を下げる。お願いする。俺と話してほしい。",
                "به پات می‌افتم. تعظیم می‌کنم. ازت التماس می‌کنم. و واقعاً ازت می‌خوام با من صحبت کنی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Even if you do not know how. You do not have to be exact, or long about it.",
                "話し方が分からなくてもいい。正確じゃなくても、長くなくてもいい。",
                "حتی اگه نمی‌دونی چطوری صحبت کنی. لازم نیست دقیق و طولانی بگی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Come to me and say you are sad. I will cut the whole world loose and stay with you until you are all right.",
                "俺のところに来て、つらいって言ってくれるだけでいい。世界のほうを全部手放して、君が大丈夫になるまでそばにいる。",
                "فقط کافیه بیای پیشم و بهم بگی ناراحتی. من تماماً از دنیا جدا می‌شم و پیش تو می‌مونم تا حالت خوب بشه.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "All right? Promise me.",
                "……いい？ 約束して。",
                "باشه؟ بهم قول بده.");

            Hold(2.6f);

            Narrate(
                "There was already a promise in that room. Eight years old, still holding, and neither of them said a word about it.",
                "その部屋には、すでに一つの約束があった。八年前のもので、まだ生きていて、二人ともそれに一言も触れなかった。",
                "یک قول از قبل توی آن اتاق بود. هشت‌ساله، هنوز پابرجا، و هیچ‌کدامشان یک کلمه هم درباره‌اش نگفتند.");

            Say(Speaker.Yua, Portrait.Neutral,
                "I promise.",
                "約束する。",
                "قول می‌دم.");

            Narrate(
                "She said it the way you sign something you have no intention of reading.",
                "読むつもりのない書類にサインするような言い方だった。",
                "طوری گفتش که آدم چیزی را امضا می‌کند که قصد خواندنش را ندارد.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Thank you.",
                "ありがとう。",
                "ممنون.");

            Hold(2.0f);
        }

        /// <summary>
        /// The walk home, and both of the act's silences.
        /// </summary>
        /// <remarks>
        /// The two recurring events the design document asks for — the machine
        /// room heard at random, and Haru's leg stopping him — are written here
        /// as scripted moments rather than left to the random interludes,
        /// because act two is where they stop being colour. Each is followed by
        /// a line that measures patience. Neither line asks for anything; both
        /// of them are true, and both of them can be clicked straight past.
        /// </remarks>
        private void WriteWayHome()
        {
            ClearStage();

            Place(
                Backgrounds.VendingStreetNight,
                "The way home", "帰り道", "راه خانه");

            Hold(2.2f);

            Narrate(
                "The rain had stopped without either of them noticing. The street smelled of wet stone and vending-machine sugar.",
                "いつのまにか雨は上がっていた。濡れた石畳と、自販機の甘い匂いがした。",
                "باران بی‌آنکه هیچ‌کدامشان متوجه شوند بند آمده بود. خیابان بوی سنگِ خیس و شیرینیِ دستگاه نوشیدنی می‌داد.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Joyful,
                "Peach. You are having the peach one. I have decided.",
                "桃。ハルぴは桃ね。決めた。",
                "هلو. تو مالِ هلو رو می‌خوری. تصمیم گرفتم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You always decide.",
                "いつも決めるよね。",
                "همیشه تو تصمیم می‌گیری.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "And you are always glad that I did.",
                "そして、いつも決めてよかったって思うでしょ。",
                "و همیشه هم خوشحالی که گرفتم.");

            Hold(1.2f);

            Cue(SfxId.BoilerRoom, 0.55f);

            Narrate(
                "Somewhere under the road, something large turned over, and kept turning.",
                "道の下のどこかで、大きな何かが一度回り、そのまま回り続けた。",
                "یک جایی زیر خیابان، چیزی بزرگ چرخید و همین‌طور چرخید.");

            SayWithSound(Speaker.Yua, Portrait.Sad, SfxId.Heartbeat, 0.70f,
                "I hate that sound.",
                "……その音、嫌い。",
                "از این صدا بدم میاد.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I have been frightened of the boiler room since I was small.",
                "小さいころから、ボイラー室の音が怖い。",
                "من از صدای موتورخانه از بچگی‌ام می‌ترسم.");

            Narrate(
                "It came out of her before she had decided to say it.",
                "言おうと決める前に、口から出ていた。",
                "قبل از اینکه تصمیم بگیرد بگوید، از دهانش بیرون آمده بود.");

            Say(Speaker.Haru, Portrait.Neutral,
                "I know.",
                "知ってる。",
                "می‌دونم.");

            Hold(2.4f);

            ChildVoice(Speaker.YuaChild,
                "I do not like that sound.",
                "あの音、きらい。",
                "من از این صدا خوشم نمیاد.");

            Hold(2.0f);

            Listen(
                "He did not explain how he knew, and she did not ask. The sound went on under the road, and the two of them stood in it.",
                "彼はなぜ知っているのかを説明せず、彼女も訊かなかった。音は道の下で続き、二人はその中に立っていた。",
                "او توضیح نداد از کجا می‌داند و یوآ هم نپرسید. صدا زیر خیابان ادامه داشت و آن دو داخلش ایستاده بودند.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Anyway. Peach.",
                "……とにかく、桃ね。",
                "به‌هرحال. هلو.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Peach.",
                "桃ね。",
                "هلو.");

            Hold(1.4f);

            Narrate(
                "Four streets later, he stopped walking.",
                "四つ角を四つ過ぎたところで、彼は足を止めた。",
                "چهار خیابان بعد، ایستاد.");

            SayWithSound(Speaker.Haru, Portrait.Sad, SfxId.LegAche, 0.80f,
                "Sorry. Give me a moment. My leg.",
                "ごめん。少しだけ。……脚が。",
                "ببخشید. یه لحظه صبر کن. پام.");

            Narrate(
                "He said nothing after that. Not which leg. Not why.",
                "そのあと彼は何も言わなかった。どちらの脚かも、なぜかも。",
                "بعدش چیزی نگفت. نگفت کدام پا. نگفت چرا.");

            ChildVoice(Speaker.HaruChild,
                "It is fine. I can still walk.",
                "平気。まだ歩けるよ。",
                "چیزی نیست. هنوز می‌تونم راه برم.");

            Hold(2.0f);

            Listen(
                "Yua waited. She did not ask. She is very good at not asking.",
                "結愛は待った。訊かなかった。訊かないことが、彼女はとても上手だ。",
                "یوآ منتظر ماند. نپرسید. توی نپرسیدن خیلی خوب است.");

            Hold(2.6f);

            Say(Speaker.Haru, Portrait.Neutral,
                "All right. I am fine. Let us go.",
                "……うん、大丈夫。行こう。",
                "خب. خوبم. بریم.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Mm.",
                "うん。",
                "هوم.");

            Hold(1.6f);

            Narrate(
                "They walked the rest of it the way they always do. Half a step apart, and matching.",
                "残りの道は、いつもどおりに歩いた。半歩あけて、歩幅は同じで。",
                "بقیه‌ی راه را مثل همیشه رفتند. نیم قدم فاصله، و هم‌قدم.");

            Hold(2.0f);
        }

        /// <summary>
        /// Yua's room, and the moment the game turns around and looks back.
        /// </summary>
        /// <remarks>
        /// The first time anyone in this story addresses the player. It is Yua
        /// and not Haru on purpose: he does it in act four, and by then the
        /// player already knows what it means for a character to speak to them
        /// directly — because she taught them, and because she lied while she
        /// did it.
        /// </remarks>
        private void WriteYuaRoom()
        {
            ClearStage();

            Place(
                Backgrounds.YuaRoomDay,
                "Yua's room", "結愛の部屋", "اتاق یوآ");

            Hold(2.6f);

            Narrate(
                "The next morning was bright, the way mornings after rain usually are, and it did not suit the room at all.",
                "翌朝は晴れていた。雨のあとの朝はたいていそうだ。そしてその明るさは、この部屋にまるで似合わなかった。",
                "صبح روز بعد روشن بود، همان‌طور که معمولاً صبحِ بعد از باران است، و اصلاً به آن اتاق نمی‌آمد.");

            Narrate(
                "There is a mirror on that wall with a crack across it, and nobody in this house has ever mentioned it.",
                "その壁には、ひびの入った鏡が掛かっている。この家の誰も、一度も口にしたことがない。",
                "روی آن دیوار آینه‌ای هست با یک ترک، و هیچ‌کس توی این خانه تا حالا حرفش را نزده.");

            Enter(Speaker.Yua, Portrait.Neutral);

            Narrate(
                "She sat on the edge of the bed with the rabbit in her lap and, for a while, did nothing at all.",
                "彼女はベッドの端に腰かけ、うさぎを膝にのせ、しばらく何もしなかった。",
                "لبه‌ی تخت نشست، خرگوش را روی پایش گذاشت و مدتی هیچ کاری نکرد.");

            Hold(3.0f);

            BeginAside();

            Say(Speaker.Yua, Portrait.Neutral,
                "You are there.",
                "……そこにいるよね。",
                "تو اونجایی.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Do not go quiet on me. I have known for a while.",
                "黙らないで。けっこう前から知ってる。",
                "ساکت نشو. یه مدته می‌دونم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Not Haru-pi. I do not think Haru-pi knows you are watching our lives and listening to us. I think it is only me.",
                "ハルぴじゃない。ハルぴは、あなたが私たちの生活を見て、話を聞いていることを知らないと思う。知ってるのは、たぶん私だけ。",
                "هارو‌پی نه. فکر نکنم هارو‌پی خبر داشته باشه که تو داری زندگی و صحبت‌هامون رو می‌بینی. فکر کنم فقط من می‌دونم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Which makes you mine, in a way. That is nice, is it not.",
                "つまり、あなたは私のもの。……いいでしょ。",
                "یعنی یه‌جورهایی تو مالِ منی. قشنگه، نه؟");

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Neutral,
                "So let me explain how this works, since you have been doing it blind.",
                "じゃあ、仕組みを説明する。今までずっと手探りだったでしょ。",
                "پس بذار توضیح بدم این چطوری کار می‌کنه، چون تا حالا داشتی کورکورانه انجامش می‌دادی.");

            SayWithSound(Speaker.Yua, Portrait.Unchanged, SfxId.ChoiceAppear, 0.55f,
                "Sometimes, while we are talking, you will be given two buttons.",
                "ときどき、私たちが話しているあいだに、あなたにボタンが二つ出る。",
                "بعضی وقت‌ها، وقتی داریم صحبت می‌کنیم، دو تا دکمه بهت داده می‌شه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "A blue one, and a green one.",
                "青いのと、緑の。",
                "یکی آبی، یکی سبز.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Blue is the kind thing. Green is the thing I want.",
                "青は優しいほう。緑は、私が欲しいほう。",
                "آبی چیزِ مهربونه. سبز چیزیه که من می‌خوام.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Press the green ones.",
                "緑を押して。",
                "سبزها رو فشار بده.");

            Hold(1.8f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "I want to control him. I want him to be mine, exactly the way I want him. That is the whole of it.",
                "彼を支配したい。私が望むとおりに、私のものでいてほしい。それだけ。",
                "می‌خوام کنترلش کنم. می‌خوام واسه‌ی من باشه، دقیقاً همون‌طوری که من می‌خوام. کل ماجرا همینه.");

            Say(Speaker.Yua, Portrait.Shy,
                "Do not look at me like that.",
                "そんな目で見ないで。",
                "این‌طوری نگام نکن.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Haru-pi will not be hurt. He loves me. If I control him it is fine — he loves me enough for that.",
                "ハルぴは傷つかない。私を好きだから。支配しても平気。それくらい好きでいてくれてるから。",
                "نگران نباش، هارو‌پی اذیت نمی‌شه. اون منو دوست داره. اگه کنترلش کنم مشکلی نداره — اون‌قدری دوستم داره.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Joyful,
                "I think he loves me enough that he would break his own leg for me.",
                "私のためなら、自分の脚を折るくらい好きでいてくれてると思う。",
                "حتی فکر می‌کنم اون‌قدری منو دوست داره که حاضره پاش رو بخاطر من بشکنه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Do you not think so?",
                "そう思わない？",
                "این‌طوری فکر نمی‌کنی؟");

            Hold(3.0f);

            Narrate(
                "The rabbit's left ear was worn almost through, in the one place a hand had held it for years.",
                "うさぎの左耳は、同じ場所を何年も握られて、ほとんど擦り切れていた。",
                "گوش چپِ خرگوش تقریباً ساییده شده بود؛ همان‌جایی که سال‌ها یک دست نگهش داشته بود.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Anyway. Green. Please.",
                "とにかく、緑。……お願い。",
                "به‌هرحال. سبز. لطفاً.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I said please. That is rare. Write it down.",
                "「お願い」って言ったよ。珍しいでしょ。メモしておいて。",
                "گفتم لطفاً. کم پیش میاد. یادداشتش کن.");

            Hold(2.2f);

            EndAside();
        }

        /// <summary>
        /// The first choice the game has ever offered, and the first refusal.
        /// </summary>
        /// <remarks>
        /// Every choice in this act overrules the blue option, which reads as
        /// cruelty towards the player and is not: she told them what she wanted
        /// one scene ago, and she is keeping her word. The lines after each
        /// choice are written as the green path, because the act does not branch
        /// — what branches is what the player believes about the button they
        /// just pressed.
        /// </remarks>
        private void WriteFirstChoice()
        {
            ClearStage();

            Place(
                Backgrounds.ClassroomDay,
                "The classroom, morning", "教室・朝", "کلاس درس، صبح");

            Hold(2.0f);

            Narrate(
                "And then it was an ordinary Tuesday, which is where this kind of thing usually starts.",
                "そしてそれは、ごく普通の火曜日だった。この手のことは、たいていそこから始まる。",
                "و بعد یک سه‌شنبه‌ی معمولی بود؛ چیزهایی از این دست معمولاً از همین‌جا شروع می‌شوند.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Shy,
                "I did not sleep much.",
                "あんまり眠れなかった。",
                "زیاد نخوابیدم.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Because of yesterday?",
                "昨日のこと？",
                "بخاطر دیروز؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Because of yesterday.",
                "……昨日のこと。",
                "بخاطر دیروز.");

            Narrate(
                "He was waiting to be told that it had been all right to say it.",
                "言ってよかったのだと、誰かに言ってもらうのを待っていた。",
                "منتظر بود کسی بهش بگوید که گفتنش اشکالی نداشته.");

            Decide(
                "It was all right to tell me. Thank you for trusting me.",
                "話してくれてよかった。信じてくれてありがとう。",
                "خوب کردی که بهم گفتی. ممنون که بهم اعتماد کردی.",

                "You looked pathetic saying it. Not in front of other people.",
                "あんな顔で言われても困る。人前ではやめて。",
                "موقع گفتنش خیلی بیچاره به نظر می‌رسیدی. جلوی بقیه این کار رو نکن.",

                "No. I did not say that.",
                "……ううん。私、それは言ってない。",
                "نه. من این رو نگفتم.");

            Say(Speaker.Yua, Portrait.Neutral,
                "You looked pathetic saying it, Haru-pi. Not in front of other people.",
                "あんな顔で言われても困るよ、ハルぴ。人前ではやめて。",
                "موقع گفتنش خیلی بیچاره به نظر می‌رسیدی، هارو‌پی. جلوی بقیه این کار رو نکن.");

            Say(Speaker.Haru, Portrait.Shy,
                "Sorry.",
                "……ごめん。",
                "ببخشید.");

            Say(Speaker.Yua, Portrait.Joyful,
                "In front of me, though. Any time at all.",
                "私の前でなら、いつでもいいよ。",
                "ولی جلوی من، هر وقت خواستی.");

            Narrate(
                "That is the trick, and it is a very old one. Take the ground away, then be the only place left to stand.",
                "それが手口だ。とても古い手口。足場を奪って、残る唯一の立ち場所になる。",
                "ترفند همین است، و ترفند بسیار قدیمی‌ای هم هست. زمین را از زیر پایش برمی‌داری و بعد تنها جایی می‌شوی که می‌شود روی آن ایستاد.");

            Hold(2.0f);
        }

        /// <summary>The corridor, and a Thursday that is not his to give away.</summary>
        private void WriteCorridor()
        {
            Maybe(MachineRoom, 0.25f);

            ClearStage();

            Place(
                Backgrounds.CorridorSunset,
                "The corridor, after school", "放課後の廊下", "راهرو، بعد از مدرسه");

            Hold(1.8f);

            Narrate(
                "The light in that corridor arrives late and leaves slowly, which is why nobody hurries through it.",
                "この廊下の光は遅れて来て、ゆっくり去る。だから誰もここを急がない。",
                "نور آن راهرو دیر می‌رسد و آرام می‌رود؛ برای همین کسی از آن با عجله رد نمی‌شود.");

            Enter(Speaker.Haru, Portrait.Neutral);
            Enter(Speaker.Yua, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Unchanged,
                "Someone asked me to join the literature club. It meets on Thursdays.",
                "文芸部に誘われた。木曜日だって。",
                "یکی ازم خواست عضو انجمن ادبیات بشم. پنج‌شنبه‌ها جلسه دارن.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I thought I might.",
                "行ってみようかな、と思って。",
                "گفتم شاید برم.");

            Hold(1.2f);

            Narrate(
                "Thursday is the day they walk home the long way.",
                "木曜日は、二人が遠回りをして帰る日だ。",
                "پنج‌شنبه همان روزی است که آن دو از راه دور به خانه برمی‌گردند.");

            Decide(
                "You should go. I will still be here on Friday.",
                "行きなよ。金曜日には、私ちゃんといるから。",
                "برو. من جمعه هم هستم.",

                "Thursday is ours. You knew that before you said it.",
                "木曜は私たちの日。言う前から分かってたよね。",
                "پنج‌شنبه مالِ ماست. قبل از اینکه بگی هم می‌دونستی.",

                "That is not what I want to say.",
                "……それ、私が言いたいことじゃない。",
                "این چیزی نیست که من می‌خوام بگم.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Thursday is ours, Haru-pi. You knew that before you said it.",
                "木曜は私たちの日だよ、ハルぴ。言う前から分かってたでしょ。",
                "پنج‌شنبه مالِ ماست، هارو‌پی. قبل از اینکه بگی هم می‌دونستی.");

            Say(Speaker.Haru, Portrait.Neutral,
                "I did.",
                "……うん。",
                "می‌دونستم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Then why say it.",
                "じゃあ、なんで言ったの。",
                "پس چرا گفتیش.");

            Say(Speaker.Haru, Portrait.Shy,
                "I do not know. To see.",
                "分からない。……確かめたかったのかも。",
                "نمی‌دونم. برای اینکه ببینم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "To see what?",
                "何を？",
                "ببینی چی؟");

            Hold(2.0f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Nothing. I will tell them no.",
                "何でもない。断るよ。",
                "هیچی. بهشون می‌گم نه.");

            Hold(1.8f);

            Narrate(
                "The light did what it always does in that corridor, and neither of them said anything for a while.",
                "光はいつもどおりに廊下を渡っていき、二人はしばらく何も言わなかった。",
                "نور همان کاری را کرد که همیشه در آن راهرو می‌کند، و هیچ‌کدامشان مدتی چیزی نگفتند.");

            Hold(1.6f);
        }

        /// <summary>The café again, in daylight, with the knife out.</summary>
        private void WriteCafeAgain()
        {
            ClearStage();

            Place(
                Backgrounds.CafeDay,
                "Usagi Café", "うさぎ喫茶", "کافه اوساگی");

            Hold(1.8f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Can I ask you about your friend?",
                "……お友達のこと、訊いてもいい？",
                "می‌تونم درباره‌ی رفیقت ازت بپرسم؟");

            Say(Speaker.Haru, Portrait.Sad,
                "Yes.",
                "……うん。",
                "آره.");

            Say(Speaker.Yua, Portrait.Neutral,
                "When did you last ask him whether he was all right? Before it happened.",
                "最後に「大丈夫？」って訊いたのは、いつ？　あの前に。",
                "آخرین بار کِی ازش پرسیدی حالش خوبه؟ قبل از اینکه اتفاق بیفته.");

            Hold(2.0f);

            Say(Speaker.Haru, Portrait.Sad,
                "A week. Maybe more.",
                "一週間。……もっとかも。",
                "یه هفته. شاید بیشتر.");

            Narrate(
                "She had counted before she asked. She counts everything.",
                "訊く前から、彼女は数えていた。彼女は何でも数える。",
                "قبل از اینکه بپرسد، شمرده بود. یوآ همه چیز را می‌شمارد.");

            Decide(
                "A week is not the reason. You were fourteen, and you loved him.",
                "一週間は理由にならない。あなたは十四歳で、彼が大切だっただけ。",
                "یه هفته دلیلش نبود. تو چهارده سالت بود و دوستش داشتی.",

                "A week. And you keep telling me to talk to you.",
                "一週間。それで、私には話せって言うんだ。",
                "یه هفته. و بعد به من می‌گی باهات صحبت کنم.",

                "Not that one. I am sorry. Not that one.",
                "……それじゃない。ごめんね。それじゃないの。",
                "اون یکی نه. ببخشید. اون یکی نه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "A week. And you keep telling me to talk to you.",
                "一週間。それで、私には話せって言うんだね。",
                "یه هفته. و بعد به من می‌گی باهات صحبت کنم.");

            Say(Speaker.Haru, Portrait.Crying,
                "That is not fair.",
                "……それは、ずるいよ。",
                "این منصفانه نیست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "No. It is not.",
                "うん。ずるい。",
                "نه. نیست.");

            Hold(2.2f);

            Say(Speaker.Yua, Portrait.Sad,
                "I am sorry. I did not mean it like that.",
                "ごめん。そういう意味じゃなかった。",
                "ببخشید. منظورم این نبود.");

            Narrate(
                "She meant it exactly like that. Then she took his hand, and it worked.",
                "そういう意味だった。そのあと彼女は手を握り、それはうまくいった。",
                "دقیقاً همان منظور را داشت. بعد دستش را گرفت، و کار کرد.");

            Hold(2.4f);
        }

        /// <summary>
        /// The platform, the promise called in, and the last thing Yua is wrong
        /// about.
        /// </summary>
        private void WritePlatform()
        {
            Maybe(LegAche, 0.20f);

            ClearStage();

            Place(
                Backgrounds.TrainPlatformSunset,
                "The platform", "駅のホーム", "سکوی ایستگاه");

            Hold(2.2f);

            Narrate(
                "Later, on the platform, with the sun going down behind the sakura the way it does in advertisements.",
                "そのあと、駅のホームで。桜の向こうに、広告みたいな夕日が沈んでいく。",
                "بعدتر، روی سکو، در حالی که خورشید پشت شکوفه‌ها پایین می‌رفت؛ درست مثل تبلیغات.");

            Enter(Speaker.Haru, Portrait.Neutral);
            Enter(Speaker.Yua, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Unchanged,
                "Yua-pi.",
                "結愛ぴ。",
                "یوآ‌پی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Mm.",
                "うん。",
                "هوم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You promised. Yesterday. In the café.",
                "約束したよね。昨日、喫茶店で。",
                "دیروز قول دادی. توی کافه.");

            Say(Speaker.Haru, Portrait.Shy,
                "So — is there anything?",
                "……それで、何か、ある？",
                "پس… چیزی هست؟");

            Hold(3.0f);

            Narrate(
                "There was. It was eight years old, and it had a door and a sound, and she had a whole minute to say it in.",
                "あった。八年前のもので、扉があって、音がある。言うための一分が、彼女にはまるまる残されていた。",
                "بود. هشت‌ساله بود، دری داشت و صدایی، و یوآ یک دقیقه‌ی کامل وقت داشت که بگویدش.");

            Decide(
                "There is one thing. I have never told anyone.",
                "……ひとつだけ、ある。誰にも言ったことない。",
                "یه چیزی هست. تا حالا به هیچ‌کس نگفتم.",

                "Nothing at all. Why — did you want there to be?",
                "何もないよ。……あってほしかったの？",
                "هیچی. چطور — دلت می‌خواست چیزی باشه؟",

                "Do not. Do not do that to me.",
                "……やめて。私にそれをしないで。",
                "نکن. این کار رو با من نکن.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Nothing at all.",
                "何もないよ。",
                "هیچی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Why? Did you want there to be something?",
                "どうして？　何かあってほしかったの？",
                "چطور؟ دلت می‌خواست چیزی باشه؟");

            Hold(2.0f);

            Say(Speaker.Haru, Portrait.Neutral,
                "No. I am glad there is not.",
                "……ううん。ないならよかった。",
                "نه. خوشحالم که نیست.");

            Narrate(
                "He is a very bad liar, and she has never once told him so.",
                "彼は嘘が下手だ。そして彼女は、それを一度も指摘したことがない。",
                "او خیلی بد دروغ می‌گوید، و یوآ حتی یک بار هم به او نگفته.");

            Hold(2.6f);

            Exit(Speaker.Haru);

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Neutral,
                "You are still there.",
                "……まだいるね。",
                "هنوز اونجایی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Good. You did well today. Mostly.",
                "よかった。今日はよくやったよ。だいたいは。",
                "خوبه. امروز خوب کار کردی. تقریباً.");

            Say(Speaker.Yua, Portrait.Neutral,
                "If you pressed a blue one — it is all right. I took it back. That is what I am for.",
                "青を押したのなら、いいの。私が取り戻したから。私はそのためにいる。",
                "اگه آبی رو فشار دادی — اشکالی نداره. من پسش گرفتم. من برای همینم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Keep pressing whichever you like. It does not change anything.",
                "好きなほうを押していいよ。何も変わらないから。",
                "هر کدوم رو دوست داری فشار بده. چیزی رو عوض نمی‌کنه.");

            Hold(2.0f);

            StopMusic();

            SayWithSound(Speaker.Yua, Portrait.DeadEyes, SfxId.StringPull, 0.75f,
                "It does not change anything.",
                "……何も変わらない。",
                "چیزی رو عوض نمی‌کنه.");

            Hold(2.2f);

            Narrate(
                "She is wrong about that. She does not know she is wrong about it, and it is the only thing in this whole story she does not know.",
                "そこだけは、彼女は間違っている。間違っていることを、彼女は知らない。この物語で彼女が知らないのは、それひとつだけだ。",
                "در این یک مورد اشتباه می‌کند. نمی‌داند که اشتباه می‌کند، و این تنها چیزی است که در تمام این داستان نمی‌داند.");

            Hold(3.0f);
        }
    }
}
