// -----------------------------------------------------------------------------
//  The Frayed Red String
//  Act05Builder.cs  (Editor only)
//
//  Act five — Glass Shattering (ガラスが割れる) — written out as code.
//
//  Run it once from The Frayed Red String ▸ Build Act 05 From The Story
//  Document. It writes Assets/Story/Acts/Act05.asset, reusing the asset already
//  at that path if there is one, and then hands over to the Story Editor.
//
//  This is the act the first four were built to be interrupted by, and it is
//  the only one that touches the game itself rather than the story inside it.
//  Four things happen to the medium, in this order and a few lines apart:
//
//    • The dialogue box goes. Haru puts a hand on it and drags it off the
//      bottom of the screen. It does not fade; it travels, and it is opaque
//      most of the way down.
//    • The frame opens. The border every act has been played inside since the
//      first widens out to nothing, and the picture becomes the whole window.
//    • The veil lifts. The pastel wash over the scenery goes to zero, which is
//      the design document's "the graphics become realistic" in a game that was
//      never going to become three-dimensional: the same art, with nothing in
//      front of it any more.
//    • The voice arrives. Every line from the turn onward carries a VoiceClip
//      name. None of the recordings exist yet and the act plays correctly
//      without them — a missing clip logs one line and the typewriter reads it
//      as usual — so the names are reserved now and the files can be dropped
//      into Assets/Audio/Voice whenever they are made, with no rebuild.
//
//  What happens inside the story is shorter than what happens to it. Yua gives
//  her last performance of being the injured one. Haru does not take it, and
//  says so: he has known since the first week, and the story about Souta was
//  told to her on purpose, so that she would have something to work with. Then
//  her face comes apart, and then he kills her, and then he talks to the player
//  and there is one sound and the act ends.
//
//  Two notes on writing it.
//
//  The murder is four beats long and shows nothing. The design document asks
//  for blood on the screen and for the body and the rabbit to fall, and that is
//  exactly and only what is here: a stain, and a sentence about a doll. A game
//  that has spent four acts refusing to show the machine room does not get to
//  stage this one.
//
//  Haru is not triumphant anywhere in this act and must never be played that
//  way. Every line he has after the turn is flat, tired and still in love,
//  which is what makes it unbearable rather than satisfying.
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Narrative;
using UnityEditor;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Act five's script.</summary>
    public sealed class Act05Builder : ActScriptWriter
    {
        /// <summary>The colour the design document's blood is written as.</summary>
        /// <remarks>
        /// Dark, and nowhere near fully opaque. A screen of bright red reads as
        /// a game-over card from a different genre; this is meant to read as
        /// something having got onto the lens.
        /// </remarks>
        private static readonly Color Blood = new Color(0.34f, 0.02f, 0.05f, 0.62f);

        protected override int ActNumber => 5;

        protected override string AssetName => "Act05";

        protected override LocalizedLine Title =>
            L("Glass Shattering", "ガラスが割れる", "شکسته شدن شیشه");

        [MenuItem("The Frayed Red String/Build Act 05 From The Story Document")]
        public static void Build()
        {
            new Act05Builder().BuildAsset();
        }

        /// <summary>Builds the act under a given policy, for the one-press setup.</summary>
        public static void Build(ActScriptWriter.RebuildPolicy policy)
        {
            new Act05Builder().BuildAsset(policy);
        }

        /// <summary>The act, in order.</summary>
        protected override void Write()
        {
            WriteLastOrdinaryHour();
            WriteAlley();
            WriteTheTurn();
            WriteConfession();
            WriteYuaBreaks();
            WriteTheKill();
            WriteToThePlayer();
        }

        // ---------------------------------------------------------------------
        //  1 · The last hour in which this is a dating game
        // ---------------------------------------------------------------------

        /// <summary>
        /// One more ordinary afternoon, played entirely straight.
        /// </summary>
        /// <remarks>
        /// The frame is closed, the veil is on, the buttons are where they have
        /// always been, and Yua is at her best. Everything after this scene
        /// depends on this scene being pleasant.
        /// </remarks>
        private void WriteLastOrdinaryHour()
        {
            Maybe(MachineRoom, 0.30f);

            Place(
                Backgrounds.CorridorSunset,
                "The corridor", "廊下", "راهرو");

            Hold(2.4f);

            Narrate(
                "December, and the corridor took the last of the light the way it does in the last week of term — all along one wall, and gone in twenty minutes.",
                "十二月。学期末の一週間らしく、廊下は最後の光を片側の壁いっぱいに受けて、二十分でそれを手放した。",
                "آذر، و راهرو آخرین نور را همان‌طور گرفت که هفته‌ی آخرِ ترم می‌گیرد — تمامِ یک دیوار، و بیست دقیقه بعد هیچ.");

            Enter(Speaker.Yua, Portrait.Joyful);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Nine months tomorrow.",
                "明日で九ヶ月。",
                "فردا می‌شه نُه ماه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Since what?",
                "何から？",
                "از چی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Since I sat down next to you. Do not pretend you do not count it too.",
                "隣に座った日から。ハルぴだって数えてるくせに。",
                "از روزی که کنارت نشستم. وانمود نکن که تو هم نمی‌شمریش.");

            Say(Speaker.Haru, Portrait.Shy,
                "I count it too.",
                "……数えてる。",
                "می‌شمرمش.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Joyful,
                "Good. Say something nice about me. Quickly, before the light goes.",
                "よろしい。私のいいところ言って。早く、光が消える前に。",
                "خوبه. یه چیز خوب درباره‌م بگو. زود، قبل از اینکه نور بره.");

            Say(Speaker.Haru, Portrait.Neutral,
                "You never let anything drop.",
                "結愛ぴは、何ひとつ手放さない。",
                "تو هیچ‌وقت هیچی رو ول نمی‌کنی.");

            Hold(1.8f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Is that nice?",
                "……それ、いいこと？",
                "این خوبه؟");

            Say(Speaker.Haru, Portrait.Joyful,
                "It is the truest thing I know about you.",
                "僕が知ってる中で、いちばん本当のことだよ。",
                "درست‌ترین چیزیه که درباره‌ت می‌دونم.");

            Hold(2.6f);

            Narrate(
                "He is telling her, very kindly and for the ninth month running, that he can see her. She has never once heard it.",
                "彼は九ヶ月間ずっと、とても優しく、「見えているよ」と伝えつづけている。彼女は一度も聞き取っていない。",
                "او دارد، خیلی مهربان و برای نهمین ماهِ پیاپی، به یوآ می‌گوید که می‌بیندش. یوآ حتی یک بار هم نشنیده.");

            Hold(2.4f);

            Decide(
                "Ask him what he means by that.",
                "どういう意味か訊いて。",
                "ازش بپرس منظورش چیه.",

                "Take the compliment and move on.",
                "褒め言葉として受け取って、先に進む。",
                "تعریفش رو قبول کن و برو جلو.",

                "It was a compliment. It was.",
                "褒め言葉だったの。……そうだよ。",
                "تعریف بود. بود دیگه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Then I am very true. Walk me home.",
                "じゃあ私はすごく本当なんだ。送って。",
                "پس من خیلی درستم. برسونم خونه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(2.2f);
        }

        // ---------------------------------------------------------------------
        //  2 · The alley, and the last performance
        // ---------------------------------------------------------------------

        /// <summary>
        /// Yua's final display of being the injured party.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The design document calls it her last act of playing the victim, and
        /// the important thing about writing it is that it is not a lie. Every
        /// sentence she says here is true. She was hurt, she is frightened, she
        /// does need him — and she is deploying all of it deliberately, at a
        /// moment she chose, to close something down.
        /// </para>
        /// <para>
        /// Written so that a player who has never suspected her still hears
        /// nothing wrong, and a player who has can see the joins. Both readings
        /// have to survive to the end of the scene.
        /// </para>
        /// </remarks>
        private void WriteAlley()
        {
            ClearStage();

            Place(
                Backgrounds.AlleywayNight,
                "The alleyway", "路地", "کوچه");

            Hold(2.6f);

            Narrate(
                "The long way again, and empty. The lantern outside the corner house was lit and there was nobody in the whole street to see it.",
                "また遠回りで、誰もいない。角の家の提灯は灯っていて、それを見る人は通りに一人もいなかった。",
                "باز از راه دور، و خلوت. فانوسِ خانه‌ی سرِ کوچه روشن بود و در تمام خیابان کسی نبود که ببیندش.");

            Enter(Speaker.Haru, Portrait.Neutral);
            Enter(Speaker.Yua, Portrait.Neutral);

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Sad,
                "Can we stop for a second. I do not feel well.",
                "ちょっと止まってもいい？　気分がよくない。",
                "می‌شه یه لحظه وایسیم؟ حالم خوب نیست.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Of course.",
                "うん、もちろん。",
                "حتماً.");

            Hold(2.4f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "I keep thinking about what you said about Souta.",
                "颯太くんのこと、ハルぴが言ったこと、ずっと考えてる。",
                "همه‌ش دارم به چیزی که درباره‌ی سوتا گفتی فکر می‌کنم.");

            Say(Speaker.Yua, Portrait.Crying,
                "That he looked at you and decided you could not carry it.",
                "ちゃんと見て、それで、ハルぴには無理だって決めたって。",
                "اینکه نگاهت کرد و تصمیم گرفت که تو نمی‌تونی تحملش کنی.");

            Hold(2.2f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Everyone looks at me and decides that too. My mother did. I have never had anyone.",
                "みんな私を見て、同じことを決める。母親もそうだった。私には誰もいなかった。",
                "همه به من نگاه می‌کنن و همین رو تصمیم می‌گیرن. مادرم گرفت. من هیچ‌وقت هیچ‌کس رو نداشتم.");

            Say(Speaker.Yua, Portrait.Crying,
                "Except you. If you decide it too, there is nothing after that. Do you understand? There is nothing after that.",
                "ハルぴ以外は。ハルぴまでそう決めたら、そのあとには何もない。分かる？　何も残らないの。",
                "جز تو. اگه تو هم همین رو تصمیم بگیری، بعدش هیچی نیست. می‌فهمی؟ بعدش هیچی نیست.");

            Hold(3.0f);

            Narrate(
                "All of that is true. She has waited three weeks for the right night to say it.",
                "すべて本当のことだ。彼女はそれを言うのにふさわしい夜を、三週間待った。",
                "همه‌ی این‌ها راست است. سه هفته منتظرِ شبِ درستش مانده بود تا بگویدش.");

            Hold(2.6f);

            Decide(
                "Stop. He does not deserve this tonight.",
                "やめて。今夜のハルぴには、これはひどすぎる。",
                "بس کن. امشب حقش این نیست.",

                "Finish it. Make sure he can never leave.",
                "最後まで言って。二度と離れられないようにして。",
                "تمومش کن. مطمئن شو که دیگه هیچ‌وقت نمی‌تونه بره.",

                "I am not doing anything. I am telling him the truth.",
                "何もしてないよ。本当のことを言ってるだけ。",
                "من کاری نمی‌کنم. دارم بهش راستش رو می‌گم.");

            Say(Speaker.Yua, Portrait.Sad,
                "Promise me. Say you will never decide that about me.",
                "約束して。私のこと、絶対にそう決めないって言って。",
                "بهم قول بده. بگو که هیچ‌وقت درباره‌ی من همچین تصمیمی نمی‌گیری.");

            Hold(3.4f);

            ChildVoice(Speaker.HaruChild,
                "You promised me too.",
                "結愛ぴも、約束した。",
                "تو هم بهم قول دادی.");

            Hold(2.0f);

            Listen(
                "Haru did not answer. He had answered that one every time it had ever been asked of him, and this time he stood in the road and let it sit there.",
                "ハルは答えなかった。訊かれるたび必ず答えてきた問いだった。今夜はただ道に立って、そのままにした。",
                "هارو جواب نداد. هر بار که این را از او پرسیده بودند جواب داده بود. این‌بار توی خیابان ایستاد و گذاشت همان‌جا بماند.");

            Hold(3.4f);
        }

        // ---------------------------------------------------------------------
        //  3 · The turn — where the game stops being a dating sim
        // ---------------------------------------------------------------------

        /// <summary>
        /// Haru takes the interface apart, in three gestures and almost no
        /// words.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Deliberately quiet. Everything the player has learnt about how this
        /// game works stops being true across about twenty seconds, and the
        /// scene does not comment on it once — no character says "the frame",
        /// nobody explains. The only line spoken during the whole sequence is an
        /// apology.
        /// </para>
        /// <para>
        /// Order matters and is not the order it would be tempting to write.
        /// The box goes first, because the box is what the player has been
        /// reading and its absence is the thing they notice fastest; the border
        /// second; the veil last and slowest, because it is the one nobody was
        /// ever consciously aware of and it should lift under a picture they are
        /// already looking at.
        /// </para>
        /// </remarks>
        private void WriteTheTurn()
        {
            Hold(2.0f);

            Say(Speaker.Haru, Portrait.DeadEyes,
                "No.",
                "……いや。",
                "نه.");

            Hold(3.0f);

            Narrate(
                "Then he reached down and took hold of the bottom of the screen.",
                "それから彼は手を伸ばして、画面の下のふちをつかんだ。",
                "بعد دستش را دراز کرد و لبه‌ی پایینِ صفحه را گرفت.");

            // No hold before this one, and that is the whole trick. A Beat
            // fades the box out, so a pause here would leave him grabbing a box
            // that had already politely removed itself — and the player would
            // watch two seconds of an empty screen being dragged. Straight from
            // the line into the pull means the sentence about him taking hold of
            // the screen is still sitting in the thing he takes hold of.
            PullDownDialogue(2.2f);

            Hold(1.6f);

            // The border, and the one sound in the palette that is not in the
            // key of any of the others.
            Cue(SfxId.GlassBreak, 0.85f);

            OpenFrame(3.0f);

            Hold(1.4f);

            // And the veil, slowest of the three, under a picture the player is
            // already looking at.
            Grade(0f, 4.0f);

            Hold(2.6f);

            Narrate(
                "The picture was wider than it had ever been, and the soft pink thing that had been over all of it since the first morning of school was not there any more.",
                "画面は今までのどれより広く、入学初日からずっと全部にかかっていた淡い桃色のものは、もうなかった。",
                "تصویر از هر وقتِ دیگری بازتر بود، و آن چیزِ صورتیِ ملایمی که از اولین صبحِ مدرسه روی همه‌اش بود دیگر نبود.");

            Hold(3.0f);

            SayVoiced(Speaker.Haru, Portrait.DeadEyes, "Act05_Haru_001",
                "Sorry. I should have done that a long time ago.",
                "ごめん。……ずっと前にやっておくべきだった。",
                "ببخشید. خیلی وقت پیش باید این کار رو می‌کردم.");

            Hold(2.4f);
        }

        // ---------------------------------------------------------------------
        //  4 · What he has known since the first week
        // ---------------------------------------------------------------------

        /// <summary>
        /// The confession, voiced, and flat all the way through.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Broken into short lines at the places a person would actually stop,
        /// the same way act two's monologue is. The difference is that act two's
        /// was pleading and this one is not: he is not trying to reach her any
        /// more, he is telling her what happened, and the last two lines are the
        /// only ones with anything left in them.
        /// </para>
        /// <para>
        /// The design document's hardest line is in here — that he told her the
        /// story about Souta on purpose, so that she would have something to
        /// use. It costs him the audience's sympathy and it has to, because the
        /// point of this act is that there is nobody in it who is only a victim.
        /// </para>
        /// </remarks>
        private void WriteConfession()
        {
            SayVoiced(Speaker.Haru, Portrait.DeadEyes, "Act05_Haru_002",
                "I have known since the first week.",
                "最初の一週間から知ってた。",
                "از همون هفته‌ی اول می‌دونستم.");

            Hold(2.2f);

            SayVoiced(Speaker.Haru, Portrait.Unchanged, "Act05_Haru_003",
                "The counting. The schedules. Asking me things you already had the answers to.",
                "数えること。予定表。答えを知ってることを訊くこと。",
                "شمردن‌ها. برنامه‌ریزی‌ها. پرسیدنِ چیزهایی که جوابشون رو داشتی.");

            SayVoiced(Speaker.Haru, Portrait.Unchanged, "Act05_Haru_004",
                "Taking things back out of my hands and telling me afterwards that I had chosen them.",
                "僕の手から取り上げておいて、あとから僕が選んだことにするやり方も。",
                "چیزها رو از دستم درآوردن و بعدش گفتن که خودم انتخابشون کردم.");

            Hold(2.6f);

            SayVoiced(Speaker.Haru, Portrait.Unchanged, "Act05_Haru_005",
                "And the story about Souta. I told you that on purpose.",
                "颯太の話も。あれは、わざと話した。",
                "و داستان سوتا. اون رو عمداً بهت گفتم.");

            Hold(3.2f);

            SayVoiced(Speaker.Haru, Portrait.Unchanged, "Act05_Haru_006",
                "You were running out of things to hold. I gave you one.",
                "握れるものが尽きかけてた。だから、ひとつ渡した。",
                "داشت چیزهایی که بگیریشون تموم می‌شد. من یکی بهت دادم.");

            SayVoiced(Speaker.Haru, Portrait.Sad, "Act05_Haru_007",
                "I would rather you had that than nothing. I have thought about that a great deal and I still would.",
                "何もないより、それがあるほうがいいと思った。ずいぶん考えたけど、今でもそう思ってる。",
                "ترجیح دادم اون رو داشته باشی تا هیچی. خیلی بهش فکر کردم و هنوزم همین‌طوره.");

            Hold(3.0f);

            SayVoiced(Speaker.Haru, Portrait.DeadEyes, "Act05_Haru_008",
                "I love you. I want to be clear that this is not me stopping.",
                "好きだよ。これは、やめるって話じゃない。それははっきりさせておきたい。",
                "دوستت دارم. می‌خوام روشن باشه که این یعنی من دارم دست می‌کشم، نیست.");

            SayVoiced(Speaker.Haru, Portrait.Unchanged, "Act05_Haru_009",
                "I tried. For nine months, and before that for eight years. It did not work.",
                "やってみた。九ヶ月。その前の八年も。効かなかった。",
                "تلاشمو کردم. نُه ماه، و قبلش هشت سال. نشد.");

            Hold(2.6f);

            SayVoiced(Speaker.Haru, Portrait.Sad, "Act05_Haru_010",
                "If you wanted to be cruel to other people I would not have said anything. People are cruel.",
                "ほかの誰かに残酷でいたいなら、何も言わなかった。人は残酷だから。",
                "اگه می‌خواستی با بقیه بد باشی هیچی نمی‌گفتم. آدم‌ها بدن.");

            SayVoiced(Speaker.Haru, Portrait.Crying, "Act05_Haru_011",
                "Why me. I am the one who loves you with all of it. Why is it me you do this to.",
                "どうして僕なんだ。全部で好きなのは僕なのに。どうしてその僕にするんだ。",
                "چرا من؟ منی که با تمام وجودم عاشقتم. چرا با منی این کار رو می‌کنی؟");

            Hold(3.2f);

            SayVoiced(Speaker.Haru, Portrait.Sad, "Act05_Haru_012",
                "Unless you love me too.",
                "……もしかして、結愛ぴも好きなのかな。",
                "مگر اینکه توام عاشق من باشی.");

            SayVoiced(Speaker.Haru, Portrait.Unchanged, "Act05_Haru_013",
                "And you are so frightened of losing me that you would rather own me than be left.",
                "失うのが怖すぎて、置いていかれるくらいなら、持っていたい。",
                "و اون‌قدر از دست دادنم می‌ترسی که ترجیح می‌دی مالکم باشی تا اینکه ولت کنم.");

            Hold(3.4f);

            SayVoiced(Speaker.Haru, Portrait.DeadEyes, "Act05_Haru_014",
                "It does not matter. I cannot hold it any more. That is all this is.",
                "もう、どっちでもいい。これ以上は持てない。ただそれだけのことなんだ。",
                "مهم نیست. دیگه نمی‌تونم نگهش دارم. کلِ ماجرا همینه.");

            Hold(3.0f);
        }

        // ---------------------------------------------------------------------
        //  5 · The face coming apart
        // ---------------------------------------------------------------------

        /// <summary>
        /// Yua loses control for the first time in the game.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The design document gives it one line — shock and pure terror at
        /// losing control — and the way to write it is to have her reach for
        /// every tool she owns, in order, and watch each one fail. The counting,
        /// the schedule, the promise, the please. She gets through her entire
        /// repertoire in about fifteen seconds, and then there is nothing
        /// underneath it except a nine-year-old in a machine room.
        /// </para>
        /// <para>
        /// She is the most sympathetic she has ever been here, which is the
        /// reason the next scene is unbearable rather than satisfying.
        /// </para>
        /// </remarks>
        private void WriteYuaBreaks()
        {
            Say(Speaker.Yua, Portrait.DeadEyes,
                "…",
                "……",
                "…");

            Hold(3.0f);

            Say(Speaker.Yua, Portrait.Neutral,
                "You are tired. It is December, everyone is tired in December.",
                "疲れてるんだよ。十二月だもん、みんな疲れてる。",
                "خسته‌ای. آذره، توی آذر همه خسته‌ن.");

            Say(Speaker.Yua, Portrait.Joyful,
                "We will go home. I will make you a schedule. You like it when I make you a schedule.",
                "帰ろう。予定表つくってあげる。ハルぴ、あれ好きでしょ。",
                "می‌ریم خونه. برات برنامه می‌ریزم. تو دوست داری وقتی برات برنامه می‌ریزم.");

            Hold(2.4f);

            Say(Speaker.Haru, Portrait.DeadEyes,
                "I know.",
                "……知ってる。",
                "می‌دونم.");

            Hold(2.6f);

            Say(Speaker.Yua, Portrait.Angry,
                "Then take it back. Take all of it back and we will not have said any of it.",
                "じゃあ取り消して。全部取り消して、何も言わなかったことにしよう。",
                "پس پسش بگیر. همه‌ش رو پس بگیر و انگار هیچ‌کدومش رو نگفتیم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I can do that. I do that. That is the thing I do.",
                "私はそれができる。いつもやってる。それが私の得意なことなの。",
                "من می‌تونم. من همین کار رو می‌کنم. کارِ من همینه.");

            Hold(3.0f);

            Narrate(
                "It is. She has taken every kind thing out of the player's hands for three acts, and she reached for the button now with her whole body, and it was not there.",
                "そのとおりだ。三幕のあいだ、彼女は優しい選択をすべて取り上げてきた。今その手を全身で伸ばして、ボタンはもうなかった。",
                "همین‌طور است. سه پرده هر چیزِ مهربان را از دستِ بازیکن بیرون کشیده، و حالا با تمام تنش به دنبالِ آن دکمه رفت، و دکمه نبود.");

            Hold(3.2f);

            Say(Speaker.Yua, Portrait.Sad,
                "Haru-pi.",
                "ハルぴ。",
                "هارو‌پی.");

            Say(Speaker.Yua, Portrait.Crying,
                "Haru-pi, please. Please. I said please, that is rare, you know it is rare —",
                "ハルぴ、お願い。お願いだから。「お願い」って言った、珍しいでしょ、珍しいって知ってるでしょ——",
                "هارو‌پی، خواهش می‌کنم. خواهش می‌کنم. گفتم خواهش می‌کنم، کم پیش میاد، خودت می‌دونی کم پیش میاد —");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Crying,
                "I do not know how to be anything else. Nobody ever taught me another one. I do not know how —",
                "ほかのやり方を知らないの。誰も教えてくれなかった。どうすればいいのか、わからない——",
                "بلد نیستم چیزِ دیگه‌ای باشم. هیچ‌کس یه جورِ دیگه یادم نداد. نمی‌دونم چطوری —");

            Hold(3.6f);

            Narrate(
                "She is nine years old and there is a door behind her and this is the first time in her life she has said any of that out loud.",
                "彼女は九歳で、うしろには扉があって、それを声に出して言ったのは、生まれてはじめてだった。",
                "او نُه ساله است و پشت سرش دری هست و این اولین بارِ زندگی‌اش است که هیچ‌کدام از این‌ها را بلند گفته.");

            Hold(3.4f);

            Narrate(
                "Nine months earlier, that sentence would have ended this story a different way. She is four acts late with it.",
                "九ヶ月前なら、その一文はこの物語を別の終わり方に変えていた。彼女は四幕、遅れている。",
                "نُه ماه پیش، همان جمله این داستان را جورِ دیگری تمام می‌کرد. چهار پرده دیر گفتش.");

            Hold(3.0f);
        }

        // ---------------------------------------------------------------------
        //  6 · Four beats, and nothing shown
        // ---------------------------------------------------------------------

        /// <summary>
        /// The murder.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A hold, a stain, and two sentences, one of which is about a toy. The
        /// design document asks for it to be sudden and violent and for blood to
        /// reach the screen, and that is met — but a game that has refused to
        /// stage the machine room for six acts cannot stage this one and stay
        /// honest, so the camera does here exactly what it has always done.
        /// </para>
        /// <para>
        /// The rabbit is the thing to write. It has been on the bed since act
        /// two with one ear worn through, and it is the only object in the game
        /// that has been in every scene that mattered. It gets the last sentence
        /// and Yua does not.
        /// </para>
        /// </remarks>
        private void WriteTheKill()
        {
            CutMusic();

            Hold(2.6f);

            Say(Speaker.Haru, Portrait.Sad,
                "I know you do not.",
                "……知ってるよ。知らないってことも。",
                "می‌دونم که بلد نیستی.");

            Hold(3.0f);

            // The whole of it. Sudden, and off the end of a sentence.
            Cue(SfxId.Heartbeat, 1.0f);

            Stain(Blood);

            Hold(3.6f);

            Narrate(
                "Afterwards the street was as quiet as it had been before, which was the part he had not expected.",
                "そのあと、通りはさっきまでと同じだけ静かだった。彼が予想していなかったのは、そこだった。",
                "بعدش خیابان همان‌قدر ساکت بود که پیش از آن بود، و این همان چیزی بود که انتظارش را نداشت.");

            Hold(3.0f);

            Narrate(
                "The rabbit had come out of her bag and landed a little way from her hand, on the side with the worn ear, facing up.",
                "うさぎは鞄から出て、手から少し離れたところに落ちていた。擦り切れた耳のほうを上にして。",
                "خرگوش از کیفش بیرون افتاده بود و کمی دورتر از دستش روی زمین بود؛ روی همان طرفی که گوشش ساییده است، رو به بالا.");

            Hold(4.0f);

            ClearStage();

            Hold(2.4f);
        }

        // ---------------------------------------------------------------------
        //  7 · Haru, the player, and one sound
        // ---------------------------------------------------------------------

        /// <summary>
        /// The last scene of act five, addressed to the person playing.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The bookend of act four's opening, and he is standing in almost the
        /// same place. He asked the player to look after her; this is what
        /// happened. Nothing in the writing points at that and it does not need
        /// to.
        /// </para>
        /// <para>
        /// The butterfly is the design document's image and is left exactly as
        /// written: he opens one hand, there is a sound, nothing is shown, and
        /// the act stops mid-air. No fade, no card. Act six opens on a black
        /// screen anyway, and the join between them should feel like a cut and
        /// not like an ending.
        /// </para>
        /// </remarks>
        private void WriteToThePlayer()
        {
            Enter(Speaker.Haru, Portrait.DeadEyes);

            Hold(3.0f);

            BeginAside();

            SayVoiced(Speaker.Haru, Portrait.Unchanged, "Act05_Haru_015",
                "I loved her.",
                "好きだった。",
                "عاشقش بودم.");

            SayVoiced(Speaker.Haru, Portrait.Unchanged, "Act05_Haru_016",
                "She loved me. I am fairly sure. I was fairly sure for a long time.",
                "あの子も好きでいてくれた。……たぶん。ずっと、たぶんそうだと思ってた。",
                "عاشقم بود. تقریباً مطمئنم. مدت‌ها تقریباً مطمئن بودم.");

            Hold(2.6f);

            SayVoiced(Speaker.Haru, Portrait.Sad, "Act05_Haru_017",
                "Maybe none of it was right. I do not know. We could have built it. Between the two of us we could have made something out of it.",
                "全部が間違ってたのかもしれない。分からない。でも、二人でならつくれた。二人でなら、そこから何かにできた。",
                "شاید هیچ‌کدومش درست نبود. نمی‌دونم. باهم می‌ساختیمش. دو نفری می‌تونستیم ازش یه چیزی دربیاریم.");

            Hold(3.0f);

            SayVoiced(Speaker.Haru, Portrait.Crying, "Act05_Haru_018",
                "I tried as hard as I have ever tried at anything.",
                "人生でいちばん、力を尽くした。",
                "تمام تلاشمو کردم؛ به‌اندازه‌ی هر کاری که تو عمرم کردم.");

            SayVoiced(Speaker.Haru, Portrait.Unchanged, "Act05_Haru_019",
                "I was never strong enough. Not when I was nine. Not now. Not once, in between.",
                "一度も、じゅうぶん強くなかった。九歳のときも。今も。その間の一度も。",
                "هیچ‌وقت به‌اندازه‌ی کافی قوی نبودم. نه توی نُه‌سالگی. نه الآن. نه حتی یک بار، بینشون.");

            Hold(3.6f);

            SayVoiced(Speaker.Haru, Portrait.Neutral, "Act05_Haru_020",
                "Thank you for staying with us. I did mean it, that night, when I asked.",
                "そばにいてくれてありがとう。あの夜に頼んだの、本気だったんだ。",
                "ممنون که پیشمون موندی. اون شب که ازت خواستم، جدی گفتم.");

            Hold(3.4f);

            Narrate(
                "He opened one hand. It was the shape a child makes when they are showing you a butterfly, and he held it there for a moment as though somebody were going to look.",
                "彼は片手を開いた。子どもが蝶を見せるときのかたちだった。誰かが見てくれるとでもいうように、少しのあいだそのままにしていた。",
                "یکی از دست‌هایش را باز کرد. همان شکلی که بچه‌ها وقتی می‌خواهند پروانه نشانت بدهند درست می‌کنند، و لحظه‌ای همان‌طور نگهش داشت، انگار قرار بود کسی نگاه کند.");

            Hold(4.0f);

            EndAside();

            Cue(SfxId.Gunshot, 0.90f);

            // Nothing is shown. The act ends here and act six opens on black.
            Hold(4.6f);
        }
    }
}
