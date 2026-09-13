// -----------------------------------------------------------------------------
//  The Frayed Red String
//  Act02Builder.cs  (Editor only)
//
//  Act two — Maboroshi (幻) — written out as code.
//
//  Run it once from The Frayed Red String ▸ Build Act 02 From The Story
//  Document. It writes Assets/Story/Acts/Act02.asset, reusing the asset that is
//  already there so nothing pointing at it breaks.
//
//  ===========================================================================
//  WHAT ACT TWO IS FOR
//  ===========================================================================
//
//  Four jobs and no others:
//
//   1. GO DEEPER THAN ACT ONE, AND GO SLOWER DOING IT. The manual is exact
//      about what slower means, and it is not "shorter": a deeper scene has
//      FEWER changes of subject and MORE frames, longer lines, more comfortable
//      silence, and a narrator who works harder, because atmosphere is the only
//      thing that makes slowness read as deliberate rather than as a game that
//      has stalled. Act one's fourteen quick scenes become nine long ones, the
//      narration goes from about one in fifteen frames to about one in eight,
//      and the café scene on its own is longer than any three scenes of act one
//      put together.
//
//   2. HARU TELLS HER ABOUT HIS FRIEND. The only heavy scene in the first four
//      acts, written under the manual's seven rules for one: entered from the
//      side in the middle of something ordinary; told badly, out of order, with
//      one completely irrelevant detail stuck to it; the method never said, not
//      once, not obliquely; the horror in the listener and not in the story;
//      nothing resolved and nobody comforted; the scene does not end on it; and
//      it leaks into the next day as an absence nobody explains.
//
//   3. SEVERAL WHITE CHOICES AND NO BLUE OR GREEN ONES. Not one, anywhere in
//      this act. The mechanics table forbids a mechanic before its date and the
//      date for blue and green is the explanation — which is the last scene of
//      this act. A blue button before Yua has said what a blue button is would
//      spend the whole device for nothing.
//
//   4. YUA'S ROOM, AND THE FIRST TIME ANYTHING IN THIS STORY LOOKS BACK AT THE
//      PLAYER. It is the act's last scene, it is the top of the escalation
//      ladder, and it is where the buttons are named.
//
//  ===========================================================================
//  WHAT THIS REWRITE FIXED
//  ===========================================================================
//
//  The draft this replaces was written against manual five and narrative
//  document 3.1.2, and it was set in JUNE — the Japanese rainy season, wet
//  socks, "since April, two months". The document puts act two between the 7th
//  of October and the 22nd of December. Everything seasonal in it was five
//  months out.
//
//  It also ran its five days as one continuous week, which quietly threw away
//  the act's whole shape: the document has eleven weeks passing here, and the
//  friend story arriving in the last of them. Five consecutive days cannot
//  carry that, and no amount of dialogue can say it if the calendar on screen
//  says otherwise.
//
//  So the five days are now spread across the autumn, and the act has dates on
//  screen for the first time in the game. October is still warm enough to sit
//  outside. November is leaves and low sun. December is bare branches, breath
//  in the air, and a café with the windows steamed up — which is where he tells
//  her, and the room being warm is the reason the scene is unbearable.
//
//  ===========================================================================
//  WHAT ACT TWO ADDS TO THE GAME BEYOND ITS OWN SCRIPT
//  ===========================================================================
//
//    • THE TWO SILENCES, written out in full for the first time. One where the
//      machine room reaches her through a floor, one where his leg stops him
//      walking. Neither asks the player for anything and both can be clicked
//      straight past. Nothing is offered for sitting with them yet — the
//      endings need a blue or a green press on record before waiting means
//      anything, and there has not been one — so what these two are for, here,
//      is to teach the shape.
//
//    • YUA SPEAKING TO THE PLAYER. Act three answers it with "still there", act
//      four has Haru do the same thing without knowing she got there first.
//
//    • LADDER RUNGS FOUR AND FIVE. Act one stopped at three and did not skip
//      one. Four is the gap between what she says out loud and what is in her
//      head; five is the address. Rung six — the game itself doing something it
//      should not — belongs to act five.
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Narrative;
using TheFrayedRedString.Presentation;
using UnityEditor;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Act two's script.</summary>
    public sealed class Act02Builder : ActScriptWriter
    {
        protected override int ActNumber => 2;

        protected override string AssetName => "Act02";

        protected override LocalizedLine Title => L("Maboroshi", "幻", "مابوروشی");

        /// <summary>
        /// Monday the 7th of October 2024.
        /// </summary>
        /// <remarks>
        /// The document says the 3rd, which is a Thursday, and the act's first
        /// day is a Monday in its own dialogue. The 7th is the Monday of that
        /// week. See AboutProject/Acts.md, decision four.
        /// </remarks>
        protected override StoryDate StartDate => StoryCalendar.ActTwoBegins;

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

        // =====================================================================
        //  THE CONTINUITY TABLE
        //
        //  Nothing enters the dialogue below unless it is here.
        //
        //  Dates      Mon 7 October · Tue 5 November · Wed 18 December ·
        //             Thu 19 December · Sun 22 December, 2024. Eleven weeks.
        //             Every one of them goes on screen.
        //  Season     October: warm in the sun, cold in the shade, the maples
        //             at their reddest. November: most of it on the ground, low
        //             light, early dark. December: bare, breath visible, the
        //             café windows steamed up from the inside.
        //  Clothes    Her dress and his cardigan, as drawn, all five days. In
        //             December it is plainly not enough and neither of them
        //             says so.
        //  Class      1-A, second floor, by the window. Morita-sensei.
        //  From act 1 Tomo, the plant on the sill, named on the 7th of
        //             September. Anko, the cat outside the bakery. The machine
        //             on the corner that owes him nothing now. The window that
        //             does not shut. Morita-sensei's maybe.
        //  New here   His pencil case, whose zip does not close. Mentioned
        //             once, in passing, by him, and never explained. It is the
        //             whole of act four's 7/8/9.
        //             The friend. No name, ever, in any language.
        //  Sounds     The machine room, twice: through the floor of the corridor
        //             in November, and under the street in December. The school
        //             bell once. The café door twice.
        // =====================================================================

        /// <summary>
        /// Five days across eleven weeks.
        /// </summary>
        /// <remarks>
        /// The gaps are the act. Two scenes in October, two in November, and
        /// then a jump of six weeks into the shortest days of the year for the
        /// three that matter.
        /// </remarks>
        protected override void Write()
        {
            WriteOctoberMonday();
            WriteNovemberTuesday();
            WriteDecemberWednesday();
            WriteDecemberThursday();
            WriteDecemberSunday();
        }

        // ---------------------------------------------------------------------
        //  Shared helpers
        // ---------------------------------------------------------------------

        /// <summary>Characters per second for <see cref="InnerVoice"/> lines.</summary>
        private const float InnerMonologueTypeSpeed = 28f;

        /// <summary>Yua thinking, with nobody to hear it.</summary>
        private void InnerVoice(string english, string japanese, string persian)
        {
            Say(Speaker.Yua, Portrait.Unchanged, english, japanese, persian);
            Script[Script.Count - 1].TypeSpeed = InnerMonologueTypeSpeed;
        }

        /// <summary>How long Yua's face is somewhere else.</summary>
        private const float SlipSeconds = 0.2f;

        /// <summary>Yua's face goes flat and comes back.</summary>
        private void FaceSlips()
        {
            Enter(Speaker.Yua, Portrait.DeadEyes);
            Hold(SlipSeconds);
            Enter(Speaker.Yua, Portrait.Joyful);
        }

        /// <summary>Autumn, at the weight the month wants it.</summary>
        private void AutumnAir(float density)
        {
            Fall(FallKind.MapleLeaf, density, 3f);
        }

        // =====================================================================
        //  MONDAY 7 OCTOBER — five weeks after act one ended
        //
        //  Two scenes, both light, and the act's job here is to reassure. The
        //  player has just watched a title card say a new date and needs to be
        //  told, without being told, that nothing has gone wrong in the gap.
        // =====================================================================

        private void WriteOctoberMonday()
        {
            WriteOctoberClassroom();
            WriteOctoberRoof();
            WriteOctoberStairs();
        }

        // ---------------------------------------------------------------------
        //  Monday, morning — 1-A
        //
        //  ▣ Scene state
        //     Background   SunnyClassroomDay.
        //     Date         Monday 7 October 2024. Five weeks on.
        //     On stage     Desks, the chalkboard, the window that does not
        //                  shut, Tomo on the sill.
        //     From act 1   Tomo was named on the 7th of September, by her, over
        //                  his objection.
        //
        //  The gap is covered by having it already be over: they are mid-
        //  argument when the scene opens, about something that has evidently
        //  been running for weeks, and the player is left to work out that five
        //  weeks of this have happened off screen.
        // ---------------------------------------------------------------------

        private void WriteOctoberClassroom()
        {
            ClearStage();

            Place(
                Backgrounds.ClassroomDay,
                "1-A", "一年A組", "اول-الف");

            Date(StoryCalendar.ActTwoBegins);

            NoFall(0f);

            Hold(2.0f);

            Narrate(
                "The window still did not shut. Somebody had wedged a folded piece of card into the gap to stop it rattling, and the damp had swollen the card, and now neither the card nor the window did anything at all.",
                "窓は相変わらず閉まらない。ガタつき止めに誰かが厚紙を折って挟んだが、湿気で厚紙がふくらんで、いまは厚紙も窓も、なんの役にも立っていない。",
                "پنجره هنوز بسته نمی‌شد. یکی یه تکه مقوا تا کرده بود و چپونده بود تو درز که تلق‌تلق نکنه، بعد مقوا از نم باد کرده بود، و حالا نه مقوا کاری می‌کرد نه پنجره.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            // OPENING IN THE MIDDLE OF AN ARGUMENT IS ONLY A TECHNIQUE IF THE
            // SUBJECT ARRIVES IN THE NEXT LINE. The old version of this opened
            // on "—and that is still not a reason", and then never said what
            // the reason was for. The player's first frame of act two was a
            // conversation they had been locked out of.
            //
            // It now carries the date as well, which is the other thing act two
            // was failing at: five days spread over eleven weeks, and no way to
            // feel the gaps except a stamp in the corner.
            Say(Speaker.Yua, Portrait.Pout,
                "—and that is still not a reason.",
                "——それ、まだ理由になってない。",
                "—و این هنوزم دلیل نیست.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "It is a reason. It's been a month. Machines don't do the same thing twice in a month.",
                "理由になってる。もう一ヶ月。機械は一ヶ月に同じことを二度やらない。",
                "دلیله. یه ماه گذشته. دستگاه تو یه ماه یه کار رو دو بار نمی‌کنه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It has been four weeks and four days.",
                "四週間と四日。",
                "چهار هفته و چهار روزه.");

            Say(Speaker.Haru, Portrait.Surprised,
                "That's a month.",
                "それ、一ヶ月。",
                "این یعنی یه ماه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The seventh of September to the seventh of October is four weeks and four days, and what you said on the seventh of September was that you would try it again in a month.",
                "九月七日から十月七日は、四週間と四日。ハルぴが九月七日に言ったのは「一ヶ月したらもう一回やる」。",
                "از هفتِ سپتامبر تا هفتِ اکتبر می‌شه چهار هفته و چهار روز، و چیزی که هفتِ سپتامبر گفتی این بود که یه ماهِ دیگه دوباره امتحانش می‌کنی.");

            Say(Speaker.Haru, Portrait.Bored,
                "...It's the same reason as last week.",
                "……先週と同じ理由。",
                "...همون دلیلِ هفته‌ی پیشه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Last week it wasn't a reason either.",
                "先週も理由になってなかった。",
                "هفته‌ی پیشم دلیل نبود.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Then we're consistent.",
                "じゃあ一貫してる。",
                "پس ثابت‌قدمیم.");

            Hold(1.4f);

            Narrate(
                "Tomo had put out two new leaves since September and was leaning, very slightly, towards the gap in the window.",
                "トモは九月から葉を二枚出して、ほんの少しだけ、窓の隙間のほうへ傾いていた。",
                "تومو از سپتامبر دو تا برگِ جدید درآورده بود و خیلی کم، به سمتِ درزِ پنجره خم شده بود.");

            Say(Speaker.Yua, Portrait.Surprised,
                "It's leaning.",
                "傾いてる。",
                "داره خم می‌شه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Towards the light. They all do that.",
                "光のほう。植物はそうなる。",
                "به سمتِ نور. همه‌شون این‌کارو می‌کنن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Tomo does that.",
                "トモがそうなってる。",
                "تومو این‌کارو می‌کنه.");

            Say(Speaker.Haru, Portrait.Bored,
                "Tomo does that, yes.",
                "はい。トモがそうなってる。",
                "بله. تومو این‌کارو می‌کنه.");

            Say(Speaker.Yua, Portrait.Smug,
                "You said it.",
                "言った。",
                "گفتیش.");

            Say(Speaker.Haru, Portrait.Sheepish,
                "I've been saying it for a month.",
                "一ヶ月言ってる。",
                "یه ماهه دارم می‌گمش.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I know. I'm still enjoying it.",
                "知ってる。まだ楽しい。",
                "می‌دونم. هنوز لذت می‌برم.");

            Hold(1.6f);

            Narrate(
                "Morita-sensei came in and wrote one thing on the board and did not rub any of it out.",
                "森田先生が入ってきて、黒板にひとつ書いて、今度は何も消さなかった。",
                "خانمِ موریتا اومد تو، یه چیز رو تخته نوشت و این بار هیچیش رو پاک نکرد.");

            Say(Speaker.Yua, Portrait.Neutral,
                "She's stopped rubbing things out.",
                "消さなくなったね。",
                "دیگه چیزی رو پاک نمی‌کنه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "She worked out that we read them.",
                "読まれてるって気づいたんでしょ。",
                "فهمید که ما می‌خونیمشون.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You read them.",
                "ハルぴが読んでる。",
                "تو می‌خونیشون.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I read them and then you know what they said, which is the same thing arriving later.",
                "僕が読んで、そのあと結愛ぴが知ってる。届くのが遅いだけで、同じこと。",
                "من می‌خونمشون و بعد تو می‌دونی چی نوشته، که همون چیزه فقط دیرتر می‌رسه.");

            Say(Speaker.Yua, Portrait.Smug,
                "That's a very efficient system and I intend to keep it.",
                "とても効率的だから、このままでいく。",
                "سیستمِ خیلی کارآمدیه و قصد دارم حفظش کنم.");

            DecideIdly(
                "Read the board yourself for once", "たまには自分で読む", "یه بارم که شده خودت تخته رو بخون",
                () =>
                {
                    Narrate(
                        "She read the noticeboard herself. It took a while, because it is a noticeboard.",
                        "結愛は自分で掲示板を読んだ。掲示板なので、それなりに時間がかかった。",
                        "خودش تابلوی اعلانات رو خوند. یه‌کم طول کشید، چون خب، تابلوی اعلاناته.");

                    Say(Speaker.Yua, Portrait.Bored,
                        "Half of it is from July.",
                        "半分は七月のやつ。",
                        "نصفش مالِ ژوئیه‌ست.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "That's why I read them. So you don't have to find that out.",
                        "だから僕が読んでる。それを知らずにすむように。",
                        "واسه همینه که من می‌خونمشون. که تو مجبور نشی این رو بفهمی.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "...I've lost four minutes.",
                        "……四分損した。",
                        "...چهار دقیقه از دست دادم.");
                },
                "Let him do it", "ハルぴに任せる", "بذار اون بخونه",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Smug,
                        "No. Read it to me.",
                        "やだ。読んで。",
                        "نه. تو برام بخونش.");

                    Say(Speaker.Haru, Portrait.Bored,
                        "Sports day results, a lost water bottle, and the heating is going off at four.",
                        "体育祭の結果、水筒の落とし物、暖房は四時で切れる。",
                        "نتایجِ جشنِ ورزشی، یه قمقمه‌ی گم‌شده، و شوفاژ ساعتِ چهار خاموش می‌شه.");

                    Say(Speaker.Yua, Portrait.Surprised,
                        "Four?",
                        "四時?",
                        "چهار؟");

                    Say(Speaker.Haru, Portrait.Unchanged,
                        "Four.",
                        "四時。",
                        "چهار.");
                });

            Hold(1.8f);

            Narrate(
                "The card under the window frame moved about a centimetre and stayed where it was after that.",
                "窓枠の下の厚紙が一センチほどずれて、そのあとは動かなかった。",
                "مقوای زیرِ قابِ پنجره حدودِ یه سانت جابه‌جا شد و بعدش همون‌جا موند.");
        }

        // ---------------------------------------------------------------------
        //  Monday, lunch — the roof
        //
        //  ▣ Scene state
        //     Background   SchoolRooftopSunnyDay.
        //     Date         Monday 7 October, lunch. Warm in the sun.
        //     On stage     The fence, the bench, the cushion, the pots.
        //     From act 1   The left end of the bench, since the 4th of
        //                  September, without either of them ever saying so.
        //
        //  The first silence of the game, and it is his leg. It is written as a
        //  Listen line, which means a player who sits with it for five minutes
        //  is counted — and nothing at all happens if they do, because the
        //  endings need a coloured button on record first and there has not been
        //  one. What this is for, today, is to teach the shape.
        // ---------------------------------------------------------------------

        private void WriteOctoberRoof()
        {
            ClearStage();

            Place(
                Backgrounds.RooftopDay,
                "The roof", "屋上", "پشت‌بام");

            AutumnAir(0.20f);

            Hold(2.0f);

            Narrate(
                "In October the roof was warm in the sun and cold two steps into the shade. They sat in the sun. Neither of them said so; they just both went there.",
                "十月の屋上は、日なたはあたたかく、二歩で日陰は寒い。二人は日なたに座った。どちらも口には出さず、どちらもそっちへ行った。",
                "تو اکتبر پشت‌بوم تو آفتاب گرم بود و دو قدم اون‌ورتر، تو سایه، سرد. تو آفتاب نشستن. هیچ‌کدوم نگفتش؛ فقط هردو رفتن همون‌جا.");

            Enter(Speaker.Haru, Portrait.Neutral);

            Narrate(
                "Haru put his bag on the left end of the bench.",
                "ハルはベンチの左端に鞄を置いた。",
                "هارو کیفشو گذاشت سمتِ چپِ نیمکت.");

            Enter(Speaker.Yua, Portrait.Neutral);

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Neutral,
                "The trees by the gate are done.",
                "校門の木、終わった。",
                "درخت‌های دمِ در تموم شدن.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Not quite.",
                "まだ。",
                "هنوز نه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "There's four leaves on it.",
                "四枚しかない。",
                "چهار تا برگ روشه.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "Then it's not done.",
                "じゃあ終わってない。",
                "پس تموم نشده.");

            Say(Speaker.Yua, Portrait.Bored,
                "Four leaves.",
                "四枚。",
                "چهار تا برگ.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Four is not zero. That's the entire argument and I'm very comfortable with it.",
                "四はゼロじゃない。以上。反論は受け付ける気がない。",
                "چهار صفر نیست. کلِ استدلال همینه و منم کاملاً باهاش راحتم.");

            Hold(1.8f);

            Narrate(
                "A long way below them a door went somewhere in the building, and the sound arrived late and small.",
                "ずっと下のほうで校舎のどこかのドアが鳴って、その音は遅れて、小さく届いた。",
                "خیلی پایین‌تر، یه جایی تو ساختمون دری خورد، و صداش دیر و کوچیک رسید.");

            Say(Speaker.Yua, Portrait.Neutral,
                "I brought two anpan from the bakery.",
                "パン屋であんパン二個買ってきた。",
                "دو تا آن‌پان از نونوایی آوردم.");

            Say(Speaker.Haru, Portrait.Surprised,
                "The ones with the—",
                "あの、中に——",
                "همونایی که توش—");

            Say(Speaker.Yua, Portrait.Unchanged,
                "With the red bean paste in. Anko. Yes.",
                "中があんこのやつ。あんこ。うん。",
                "همونایی که توش خمیرِ لوبیای قرمزه. آنکو. آره.");

            Hold(1.0f);

            Say(Speaker.Haru, Portrait.Bored,
                "...You named a cat after the inside of a bread roll.",
                "……パンの中身の名前を猫につけたんだ。",
                "...تو اسمِ توی یه نون رو گذاشتی رو یه گربه.");

            Say(Speaker.Yua, Portrait.Smug,
                "I named a cat after the best part of a bread roll. There is a difference and it is the whole difference.",
                "パンのいちばんいいところの名前をつけたの。違いはあるし、それが全部。",
                "من اسمِ بهترین قسمتِ یه نون رو گذاشتم روش. فرق داره، و کلِ فرق همینه.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Joyful,
                "You don't even like anpan.",
                "結愛ぴ、あんパン好きじゃないでしょ。",
                "تو که اصلاً آن‌پان دوست نداری.");

            Say(Speaker.Yua, Portrait.Smug,
                "I brought two.",
                "二個持ってきた。",
                "دو تا آوردم.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Sheepish,
                "Thank you.",
                "ありがとう。",
                "ممنون.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Don't be weird about it.",
                "変にならないで。",
                "بابتش عجیب رفتار نکن.");

            Hold(2.0f);

            Narrate(
                "They ate on the bench with the city a long way down and the clouds doing nothing in particular, for a while.",
                "ベンチで食べた。街はずっと下で、雲はとくに何もしていなかった。しばらく、そのまま。",
                "رو نیمکت خوردن، شهر خیلی پایین بود و ابرها کارِ خاصی نمی‌کردن، یه مدت.");

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Four floors is a lot for a Monday.",
                "月曜に四階分はきつい。",
                "چهار طبقه واسه یه دوشنبه زیاده.");

            Say(Speaker.Haru, Portrait.Neutral,
                "It's four floors on Tuesday as well.",
                "火曜も四階分ある。",
                "سه‌شنبه هم چهار طبقه‌ست.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Don't.",
                "やめて。",
                "نگو.");

            Hold(1.8f);
        }

        // ---------------------------------------------------------------------
        //  Monday, the way down — the half-landing
        //
        //  ▣ Scene state
        //     Background   AutumnAfternoonSchoolStairs: two flights, a wide
        //                  half-landing between them, and one tall window with
        //                  the maples behind it.
        //     Date         Monday 7 October, after lunch.
        //     On stage     The rail, the window, the stairs.
        //     From act 1   Four floors, complained about on the way up.
        //
        //  ◆ THE ACT'S FIRST SILENCE, and it has its own picture now. It used
        //  to play out against a photograph of the roof they had already left,
        //  with the narrator saying they were on a staircase — the scene lock
        //  broken on the most careful twenty seconds in the act.
        //
        //  Nothing is asked for and nothing is offered. A player who sits here
        //  is counted, and is told nothing about it, now or ever.
        // ---------------------------------------------------------------------

        private void WriteOctoberStairs()
        {
            ClearStage();

            Place(
                Backgrounds.StairsAutumn,
                "The stairs", "階段", "راه‌پله");

            NoFall(0f);

            Hold(1.6f);

            Enter(Speaker.Haru, Portrait.Neutral);
            Enter(Speaker.Yua, Portrait.Neutral);

            Narrate(
                "On the way down he stopped on the half-landing between the third floor and the second, with one hand on the rail.",
                "下りる途中、三階と二階のあいだの踊り場で、ハルは手すりに手をかけて止まった。",
                "موقعِ پایین رفتن، تو پاگردِ بینِ طبقه‌ی سوم و دوم وایساد، یه دستش رو نرده بود.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.StrainedSmile,
                "Give me a second.",
                "ちょっとだけ。",
                "یه لحظه وایسا.");

            Hold(1.2f);

            Listen(
                "She did not ask. She stood on the step below him and waited, and the stairwell was very quiet, and after a while he came down.",
                "結愛は訊かなかった。一段下で待って、階段はとても静かで、しばらくして、ハルは下りてきた。",
                "نپرسید. یه پله پایین‌تر وایساد و منتظر موند، و راه‌پله خیلی ساکت بود، و بعد از یه مدت اومد پایین.");

            Hold(1.6f);

            // Not "Tuesday, then." The next scene in the act is a Tuesday,
            // four weeks later, and a parting that names the day made those two
            // read as consecutive — the gap the whole act is built out of,
            // closed by two words.
            Say(Speaker.Yua, Portrait.Neutral,
                "Tomorrow, then.",
                "じゃあ、また明日。",
                "پس فردا.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Tomorrow.",
                "また明日。",
                "فردا.");
        }

        // =====================================================================
        //  TUESDAY 5 NOVEMBER — four weeks later
        //
        //  The act's second silence, and the first time in the game that
        //  something underneath a building reaches her.
        // =====================================================================

        private void WriteNovemberTuesday()
        {
            WriteNovemberCorridor();
            WriteNovemberStreet();
        }

        // ---------------------------------------------------------------------
        //  Tuesday, after school — the corridor
        //
        //  ▣ Scene state
        //     Background   SchoolCorridorAutumnSunset. In November the sunset
        //                  arrives during cleaning, so the corridor is gold at
        //                  half past four instead of six. Indoors, so no fall.
        //     Date         Tuesday 5 November 2024.
        //     On stage     The windows, the lockers, the light, a grille in the
        //                  floor at the end of the corridor.
        //     From before  Tomo. The umbrella, which is the size of a sock.
        //
        //  ◆ The machine room, for the first time, and it is the act's second
        //  Listen. The design document's random event says she says out loud
        //  that she has been frightened of this sound since she was small, and
        //  he confirms that he knows — and she does not ask how he knows.
        //  That last part is the whole thing and it is done by nobody saying
        //  anything at all.
        // ---------------------------------------------------------------------

        private void WriteNovemberCorridor()
        {
            ClearStage();

            Place(
                Backgrounds.CorridorSunset,
                "The second-floor corridor", "二階の廊下", "راهروی طبقه‌ی دوم");

            Date(2024, 11, 5);

            // Indoors, and this used to be AutumnAir(0.42f) — the heaviest
            // fall anywhere in the game, running down a corridor with a roof on
            // it. The maples are on the far side of the glass, which is where
            // the picture already puts them.
            NoFall(0f);

            Hold(2.0f);

            Narrate(
                "In November the light arrived during cleaning, so the corridor was gold at half past four and dark by five.",
                "十一月は掃除の時間に光が来る。四時半には廊下が金色で、五時には暗い。",
                "تو نوامبر نور موقعِ نظافت می‌رسید، پس راهرو ساعتِ چهار و نیم طلایی بود و پنج، تاریک.");

            Narrate(
                "It had rained hard for about ten minutes at lunchtime and then stopped as though it had been asked to. Haru had produced, from the bottom of his bag, the smallest umbrella either of them had ever seen.",
                "昼休みに十分だけ本降りになって、頼まれたみたいにぴたりとやんだ。ハルは鞄の底から、二人が見たなかでいちばん小さい傘を出した。",
                "ظهر ده دقیقه بارونِ تندی گرفت و بعد، انگار ازش خواسته باشن، بند اومد. هارو از تهِ کیفش کوچیک‌ترین چتری رو که هرکدومشون تو عمرشون دیده بودن درآورد.");

            Hold(1.6f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Pout,
                "That is a sock.",
                "あれは靴下。",
                "اون یه جورابه.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "It is an umbrella.",
                "傘だよ。",
                "اون چتره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It folds down to the size of a sock and it is the colour of a sock. It is a sock.",
                "畳んだら靴下のサイズで、色も靴下。あれは靴下。",
                "تا می‌شه اندازه‌ی یه جوراب و رنگشم رنگِ جورابه. اون جورابه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It kept the rain off.",
                "雨はしのげた。",
                "بارون رو گرفت.");

            Say(Speaker.Yua, Portrait.Bored,
                "It kept some of the rain off one of us.",
                "二人のうち片方の、雨の一部をしのげた。",
                "یه بخشی از بارون رو، از رو یکی از ما، گرفت.");

            Say(Speaker.Haru, Portrait.Sheepish,
                "It was the correct one of us.",
                "しのぐべきほうだった。",
                "همون یکی‌ای که باید.");

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Joyful,
                "...Fine. It's a sock that works.",
                "……はい。使える靴下。",
                "...باشه. یه جورابِ کارآمده.");

            Hold(1.8f);

            Narrate(
                "There is a metal grille in the floor at the end of the corridor, over the part of the building nobody has ever been sent to.",
                "廊下の突きあたりの床には金属の格子があって、その下は、誰も行かされたことのない場所だった。",
                "تهِ راهرو یه شبکه‌ی فلزی تو کفِ زمینه، روی اون بخشی از ساختمون که هیچ‌کس تا حالا فرستاده نشده بهش.");

            Hold(1.4f);

            // The boiler. Twenty seconds of fade-in under nothing at all, and
            // deliberately boring — the manual is explicit that real triggers
            // are ordinary sounds.
            Cue(SfxId.BoilerRoom, 0.62f);

            Hold(2.4f);

            Narrate(
                "Something under the floor came on, and settled, and kept going.",
                "床の下でなにかが動きだして、落ち着いて、そのまま鳴りつづけた。",
                "یه چیزی زیرِ زمین روشن شد، جا افتاد، و همون‌جور ادامه داد.");

            Hold(1.6f);

            // HER FACE. This whole passage used to run on Portrait.Neutral,
            // which is her gentle smile, so she said she had been frightened of
            // something since she was a child while smiling pleasantly at the
            // floor. ColdHug is not a signature face and is not rationed: it is
            // her arms folded across herself, which in a cold corridor in
            // November is deniable, and which is the only thing on screen that
            // knows anything is wrong.
            Say(Speaker.Yua, Portrait.ColdHug,
                "...",
                "……",
                "...");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "I've never liked that sound.",
                "その音、昔から苦手。",
                "من هیچ‌وقت این صدا رو دوست نداشتم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Since I was small. I don't know why.",
                "小さいころから。理由はわかんない。",
                "از بچگی. نمی‌دونم چرا.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "I know.",
                "知ってる。",
                "می‌دونم.");

            Hold(2.0f);

            // THE NARRATOR DOES NOT GET TO SAY IT. This used to read "She did
            // not ask him how he knew", which hands the player the exact thing
            // they were supposed to notice on their own — and once it is said
            // out loud it is a fact about the scene instead of a hole in it.
            // What is left is the room: a sound, a light going, and two people
            // who say nothing. The absence does the work or nothing does.
            Listen(
                "The thing under the floor kept going. The light came off the lockers one at a time, and then off the last one.",
                "床の下のそれは鳴りつづけた。光はロッカーからひとつずつ引いて、やがて最後のひとつからも引いた。",
                "اون چیزِ زیرِ زمین ادامه داد. نور یکی‌یکی از رو کمدها رفت، و بعد از رو آخریش هم رفت.");

            Hold(1.8f);

            Say(Speaker.Haru, Portrait.Neutral,
                "There's a machine down there. It's on a timer.",
                "下に機械があるんだ。タイマーで動いてる。",
                "اون پایین یه دستگاه هست. رو تایمره.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Of course you know what it is.",
                "そりゃ知ってるよね。",
                "معلومه که می‌دونی چیه.");

            // AND THE PLAYER IS TOLD WHAT IT SAYS. The old version had Haru
            // say it was written on the grille and Yua say "of course it is",
            // and the sign itself was never read out — so the one piece of
            // information in the exchange never reached the person holding the
            // controller.
            Say(Speaker.Haru, Portrait.Sheepish,
                "It's written on the grille. BOILER ROOM. NO ENTRY EXCEPT AUTHORISED STAFF.",
                "格子に書いてあるんだ。「ボイラー室 関係者以外立入禁止」。",
                "رو خودِ شبکه نوشته. «موتورخانه. ورودِ افرادِ غیرمجاز ممنوع.»");

            Say(Speaker.Yua, Portrait.Smug,
                "Of course it does.",
                "書いてあるよね。",
                "معلومه که نوشته.");

            Hold(1.6f);

            DecideIdly(
                "Ask what's underneath", "下になにがあるのか訊く", "بپرس زیرش چیه",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "What's actually down there?",
                        "実際、下になにがあるの?",
                        "واقعاً اون پایین چی هست؟");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "Pipes, mostly. The boiler. And one room at the end they keep locked, because of the boiler.",
                        "だいたい配管。ボイラー。あと、突きあたりにずっと鍵のかかった部屋。ボイラーがあるから。",
                        "بیشترش لوله‌ست. دیگ. و یه اتاق تهش که همیشه قفله، به‌خاطرِ دیگ.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Locked from outside or from inside.",
                        "外から? 中から?",
                        "از بیرون قفله یا از تو.");

                    Say(Speaker.Haru, Portrait.Surprised,
                        "...Outside. Why would it be from inside.",
                        "……外から。なんで中からなの。",
                        "...از بیرون. چرا باید از تو باشه.");

                    Say(Speaker.Yua, Portrait.Neutral,
                        "No reason.",
                        "べつに。",
                        "همین‌جوری.");
                },
                "Leave it and go", "そのまま行く", "ولش کن و برو",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Neutral,
                        "Anyway.",
                        "まあ、いいや。",
                        "خب.");

                    Say(Speaker.Haru, Portrait.Surprised,
                        "Anyway?",
                        "いいの?",
                        "خب؟");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "It's cold and I've been standing on a grille listening to a boiler for four minutes. Anyway.",
                        "寒いし、四分も格子の上でボイラーを聞いてた。もう、いいや。",
                        "سرده و چهار دقیقه‌ست رو یه شبکه وایسادم و به صدای دیگ گوش می‌دم. خب دیگه.");
                });

            Hold(1.4f);

            Narrate(
                "They went down the other stairs.",
                "二人は反対の階段から下りた。",
                "از اون یکی پله‌ها رفتن پایین.");
        }

        // ---------------------------------------------------------------------
        //  Tuesday, evening — the street with the machine
        //
        //  ▣ Scene state
        //     Background   PastelStreetVendingAutumnDay. The sky in that
        //                  picture is already a low gold, which is what half
        //                  past four in November looks like.
        //
        //                  NOT the winter night one, which now has snow in it —
        //                  snow at this corner on the 5th of November in
        //                  Kanagawa is a claim nobody would believe. NOT the
        //                  September night one either, whose flowerbeds are in
        //                  full flower. The corner at dusk in autumn is the one
        //                  picture of this place the project does not have, and
        //                  it is the single item on the art request.
        //
        //                  There is no bench in this picture, so nobody sits on
        //                  one. See the can, below: the staging was rewritten
        //                  around what is drawn rather than the other way round.
        //     Date         Tuesday 5 November, on the way home.
        //     On stage     The machine, the bicycle, the beds the flowers have
        //                  gone out of. Two cans by the end.
        //     From act 1   The machine paid out twice on the 7th of September
        //                  and neither of them has stopped talking about it.
        //
        //  ◆ LADDER RUNG FOUR, once, and it is the reason this scene exists.
        //  Rung four is the gap between what she says out loud and what is in
        //  her head, and the game has never shown one: every monologue so far
        //  has agreed with her face. This one does not.
        // ---------------------------------------------------------------------

        private void WriteNovemberStreet()
        {
            ClearStage();

            Place(
                Backgrounds.VendingStreetDay,
                "The corner", "曲がり角", "سرِ نبش");

            NoFall(2f);

            Hold(2.0f);

            Narrate(
                "The fifth of November, and the sun was off this side of the street by half past four. The beds by the machine had gone over to stalks. The machine did not care and was the same colour it had been in September.",
                "十一月五日。四時半にはもうこちら側に陽が当たらない。自販機のそばの花壇は茎だけになっていた。自販機はそんなことは気にせず、九月と同じ色をしていた。",
                "پنجمِ نوامبر، و ساعتِ چهار و نیم آفتاب دیگه از این‌ورِ خیابون رفته بود. باغچه‌های کنارِ دستگاه شده بودن ساقه‌ی خشک. دستگاه اهمیتی نمی‌داد و همون رنگی بود که سپتامبر بود.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Joyful,
                "It hasn't done it again.",
                "あれ以来、一度もない。",
                "دیگه تکرار نکرده.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's done it once. That's enough for a machine.",
                "一回やった。機械にしては十分。",
                "یه بار کرده. واسه یه دستگاه کافیه.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "Once in two months is not a habit, it's an accident.",
                "二ヶ月に一回は、習慣じゃなくて事故。",
                "دو ماه یه بار عادت نیست، تصادفه.");

            Say(Speaker.Yua, Portrait.Pout,
                "It was saving up. You said so yourself.",
                "貯めてたんでしょ。ハルぴが言った。",
                "داشت جمع می‌کرد. خودت گفتی.");

            Say(Speaker.Haru, Portrait.Sheepish,
                "I said so in September. I've had time to think about it.",
                "九月に言った。そのあと考える時間があった。",
                "سپتامبر گفتم. از اون موقع وقت داشتم فکر کنم.");

            Say(Speaker.Yua, Portrait.Bored,
                "Two months of thinking and this is where you've got to.",
                "二ヶ月考えて、そこ。",
                "دو ماه فکر کردی و به اینجا رسیدی.");

            Hold(1.6f);

            Narrate(
                "He put a hundred yen in. The machine took it and gave him one can, which is what a machine is supposed to do and was somehow the least interesting outcome available.",
                "ハルは百円を入れた。自販機は受け取って、缶を一本出した。機械として正しく、そして考えうるかぎりいちばん面白くない結果だった。",
                "صد ین انداخت. دستگاه گرفتش و یه قوطی داد، که کارِ درستِ یه دستگاهه و یه‌جورایی بی‌مزه‌ترین نتیجه‌ی ممکن بود.");

            Cue(SfxId.CanDrop, 0.8f);

            Cel(Portrait.Unchanged, Portrait.HoldCan, 1.0f);

            Say(Speaker.Yua, Portrait.Bored,
                "Boring.",
                "つまんない。",
                "خسته‌کننده.");

            // Unchanged, so the can stays in his hands through the exchange.
            // The joke is in the word, not in his face.
            Say(Speaker.Haru, Portrait.Unchanged,
                "Reliable.",
                "信頼できる。",
                "قابل اعتماد.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Boring.",
                "つまんない。",
                "خسته‌کننده.");

            Hold(1.8f);

            Narrate(
                "They stood by the machine and passed the can back and forth, and did not talk for a while, which was comfortable.",
                "自販機のそばに立って、缶を回し飲みして、しばらく話さなかった。それは、居心地がよかった。",
                "کنارِ دستگاه وایسادن و قوطی رو دست‌به‌دست کردن، و یه مدت حرف نزدن، که راحت بود.");

            // The pass, shown rather than said: it leaves his hands and arrives
            // in hers, and nobody remarks on it.
            Cel(Portrait.HoldCan, Portrait.Neutral, 2.2f);

            // A NEW SUBJECT NEEDS A DOOR. This used to be "My pencil case
            // doesn't shut", arriving cold, straight after a silence, with
            // nothing in front of it — so the player got a pencil case out of
            // the dark and had no idea why. Two words fix it, and they are the
            // two words a real person uses.
            Say(Speaker.Haru, Portrait.Neutral,
                "Oh — before I forget.",
                "あ、そうだ。忘れないうちに。",
                "راستی — تا یادم نرفته.");

            Say(Speaker.Yua, Portrait.Surprised,
                "Mm?",
                "ん?",
                "هوم؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "My pencil case doesn't shut.",
                "筆箱、閉まらないんだ。",
                "جامدادیم بسته نمی‌شه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "...That is the thing you were saving?",
                "……それ、取っておいた話?",
                "...همینو نگه داشته بودی که بگی؟");

            // THE ZIP IS NOT DRAWN. "It goes about that far" points at a
            // gesture the player cannot see, on an object the player cannot
            // see. Said in words instead.
            Say(Speaker.Haru, Portrait.Sheepish,
                "The zip won't go past halfway. It's been like that since the summer. I was looking at the window in class and I thought about it.",
                "ファスナーが半分から先へ行かない。夏からずっと。授業中に窓を見てて、それを思い出した。",
                "زیپش از وسط جلوتر نمی‌ره. از تابستون همین‌جوریه. تو کلاس داشتم پنجره رو نگاه می‌کردم و یادش افتادم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Everything you own is broken in the same way.",
                "ハルぴの持ちもの、全部おなじ壊れ方する。",
                "همه‌ی وسایلت به یه شکل خرابن.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's not true and I can't prove it.",
                "それは違う。証明はできない。",
                "این درست نیست و نمی‌تونم ثابتش کنم.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Get a new one.",
                "新しいの買えば。",
                "یکی نو بگیر.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Mm.",
                "ん。",
                "هوم.");

            Hold(1.6f);

            // The narrator used to explain this exchange — that he said it the
            // way you say a thing you are not going to do, and that she let it
            // go. Both of those are in the "Mm." and in the silence after it.
            // Saying them out loud turns a moment the player reads into a
            // caption the player is handed.
            Hold(2.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "It's cold.",
                "寒い。",
                "سرده.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It's November.",
                "十一月だもん。",
                "نوامبره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It's November and you're in that dress.",
                "十一月で、その恰好。",
                "نوامبره و تو اون پیراهنی.");

            // Her arms are already folded across herself when she says it.
            // The line and the picture disagree, and neither of them mentions
            // that either.
            Say(Speaker.Yua, Portrait.ColdHug,
                "I'm fine.",
                "平気。",
                "من خوبم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You're not fine, you're stubborn, which is a different thing that looks the same from here.",
                "平気じゃなくて、意地張ってる。ここからだと同じに見えるだけ。",
                "خوب نیستی، لجبازی، که چیزِ دیگه‌ایه و از اینجا شبیهِ همونه.");

            Hold(1.4f);

            // ART NOTE. An earlier draft had him take his cardigan off and put
            // it round her, which is the better image and which this project
            // cannot show: both characters have exactly one outfit drawn, and a
            // narrator saying he is not wearing his cardigan while the sprite
            // plainly is, is the scene lock broken. The hot can does the same
            // work with an object that is already on stage, already established
            // by the machine, and contradicts no picture.
            Narrate(
                "He went back to the machine and put another hundred in, and took the hot one out of the tray, and held it for about ten seconds.",
                "ハルは自販機に戻って、もう百円入れて、あたたかいほうを取り出して、十秒くらい持っていた。",
                "برگشت سمتِ دستگاه، یه صد ینِ دیگه انداخت، داغیه رو از سینی برداشت، و حدودِ ده ثانیه نگهش داشت.");

            Cue(SfxId.CanDrop, 0.8f);

            Cel(Portrait.Unchanged, Portrait.HoldCan, 2.0f);

            // NOT ON A WALL. There is no wall in the autumn picture of this
            // corner. He holds it out at arm's length and looks at the machine
            // instead of at her, which does the same work — it is not an offer
            // if nobody is offering — and needs no furniture that is not drawn.
            Narrate(
                "He held it out sideways, at arm's length, and looked at the machine while he did it, so that it was not an offer and could not be refused.",
                "腕を伸ばして横に差し出したまま、目は自販機のほうを見ていた。差し出しているのでなければ、断られようがない。",
                "دستشو دراز کرد و قوطی رو از پهلو گرفت جلو، و تو همون حال به دستگاه نگاه می‌کرد — تا تعارف نباشه و نشه ردش کرد.");

            Hold(2.2f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "...That one's yours.",
                "……それ、ハルぴのでしょ。",
                "...اون مالِ توئه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's in the air.",
                "宙に浮いてるだけ。",
                "تو هواست.");

            Hold(2.4f);

            Narrate(
                "She took it. He put his hands in his pockets.",
                "結愛は受け取った。ハルは両手をポケットに入れた。",
                "برش داشت. هارو دست‌هاشو کرد تو جیبش.");

            // The trade, in one picture: the warm thing changes hands and he
            // has nowhere to put his.
            Cel(Portrait.HoldCan, Portrait.Pockets, 2.0f);

            // ◆ LADDER RUNG FOUR. Out loud, and then not out loud, and they do
            // not match. Every monologue before this one agreed with her face.
            // Unchanged: she says it holding the can he gave up.
            Say(Speaker.Yua, Portrait.Unchanged,
                "Thank you.",
                "ありがと。",
                "ممنون.");

            Hold(1.6f);

            // NOBODY ELSE IS IN SHOT WHEN SHE IS TALKING TO HERSELF. He walks
            // two steps ahead and leaves the frame, which is both a real thing
            // a cold person does and the only staging in which these three
            // lines are private. With him standing beside her they were not a
            // monologue, they were something she was saying in front of him.
            Narrate(
                "He went a couple of steps ahead, the way he did when he was cold and pretending not to be.",
                "ハルは二歩ぶん先を歩いた。寒くて、寒くないふりをしているときの歩き方だった。",
                "دو قدم جلوتر افتاد، همون‌جوری که وقتی سردش بود و به روی خودش نمی‌آورد راه می‌رفت.");

            Exit(Speaker.Haru);

            Hold(1.8f);

            InnerVoice(
                "His hands will be cold all the way home.",
                "帰るあいだ、ずっと手が冷たい。",
                "تا خونه دست‌هاش سرده.");

            InnerVoice(
                "They'll be cold all the way home and he won't say anything.",
                "ずっと冷たくて、なにも言わない。",
                "تا خونه سرده و هیچی نمی‌گه.");

            Hold(1.2f);

            InnerVoice(
                "...Good.",
                "……いい。",
                "...خوبه.");

            Hold(2.2f);

            Enter(Speaker.Haru, Portrait.Pockets);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Walk faster, then. You're shivering.",
                "じゃあ早く歩いて。震えてる。",
                "پس تندتر راه برو. داری می‌لرزی.");

            // Hands still in his pockets while he says it, which is the joke
            // she made three minutes ago with her arms folded.
            Say(Speaker.Haru, Portrait.Unchanged,
                "I'm walking at a normal speed.",
                "ふつうの速さだよ。",
                "با سرعتِ عادی راه می‌رم.");

            Say(Speaker.Yua, Portrait.Smug,
                "You're walking at a cold speed.",
                "寒い速さで歩いてる。",
                "با سرعتِ سرما راه می‌ری.");
        }

        // =====================================================================
        //  WEDNESDAY 18 DECEMBER — six weeks later, and the day it happens
        //
        //  The act's centre. One short scene to establish that the day is
        //  completely ordinary, and then the longest scene in the game so far.
        // =====================================================================

        private void WriteDecemberWednesday()
        {
            WriteDecemberClassroom();
            WriteDecemberCafe();
        }

        // ---------------------------------------------------------------------
        //  Wednesday, morning — 1-A
        //
        //  ▣ Scene state
        //     Background   OvercastClassroomRainy: grey light, rain on the
        //                  glass, the curtain moving, the bookshelf.
        //     Date         Wednesday 18 December 2024.
        //     On stage     Desks, the chalkboard, the window, Tomo, the card
        //                  under the window frame.
        //     From before  Tomo leans. The window does not shut. His pencil
        //                  case does not shut either, mentioned once in
        //                  November and not since.
        //
        //  Short, warm and entirely ordinary, and it has to be: what happens
        //  this afternoon is only unbearable if the morning was nothing at all.
        // ---------------------------------------------------------------------

        private void WriteDecemberClassroom()
        {
            ClearStage();

            Place(
                Backgrounds.ClassroomRainy,
                "1-A", "一年A組", "اول-الف");

            Date(StoryCalendar.TheFriendStory);

            NoFall(0f);

            Hold(2.2f);

            Narrate(
                "December rain does not arrive, it is simply there when you look up. The card under the window frame had swollen and was doing more than it used to.",
                "十二月の雨は降りだすというより、気づくと降っている。窓枠の厚紙はふくらんで、前より役に立っていた。",
                "بارونِ دسامبر نمیاد، فقط وقتی سرتو بالا می‌گیری هست. مقوای زیرِ قابِ پنجره باد کرده بود و بیشتر از قبل کار می‌کرد.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Surprised,
                "It's working.",
                "効いてる。",
                "داره کار می‌کنه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It swelled up.",
                "ふくらんだんだ。",
                "باد کرده.");

            Say(Speaker.Yua, Portrait.Joyful,
                "So it only works when it's wet.",
                "濡れてるときだけ効くってこと。",
                "یعنی فقط وقتی خیسه کار می‌کنه.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "Which is when you need it.",
                "必要なときだけ。",
                "که همون موقعیه که لازمش داری.");

            Say(Speaker.Yua, Portrait.Bored,
                "...I hate that that's correct.",
                "……正しいのが腹立つ。",
                "...بدم میاد که درسته.");

            Hold(1.8f);

            Narrate(
                "Tomo had stopped leaning towards the window in December and was leaning very slightly towards the room instead.",
                "十二月に入って、トモは窓のほうへ傾くのをやめ、かわりに少しだけ部屋のほうへ傾いていた。",
                "تومو تو دسامبر دیگه به سمتِ پنجره خم نمی‌شد و خیلی کم به سمتِ کلاس خم شده بود.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Tomo's turned round.",
                "トモ、向き変えた。",
                "تومو برگشته.");

            Say(Speaker.Haru, Portrait.Neutral,
                "There's more light in here than out there now.",
                "いまは外より中のほうが明るい。",
                "الآن این تو از اون بیرون روشن‌تره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That's bleak.",
                "それ、暗い話。",
                "این دلگیره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's December.",
                "十二月だから。",
                "دسامبره دیگه.");

            Say(Speaker.Yua, Portrait.Bored,
                "The eighteenth of December. Three days of term left and then it's dark until January.",
                "十二月十八日。学期はあと三日で、そのあとは一月まで暗い。",
                "هجدهِ دسامبر. سه روز از ترم مونده و بعدش تا ژانویه تاریکه.");

            Hold(1.6f);

            // THE OLD OPTION MOVED THE POT TO A WINDOWSILL THE PICTURE DOES
            // NOT HAVE, and moved it to a place the picture would then have to
            // show it in. Both roads now happen where Tomo already is, so what
            // the player presses is something the background can survive.
            DecideIdly(
                "Turn Tomo back round", "トモを元の向きに戻す", "تومو رو بچرخون سرِ جاش",
                () =>
                {
                    Narrate(
                        "She turned the pot a half circle so the leaning side faced the window again.",
                        "鉢を半周まわして、傾いているほうをまた窓に向けた。",
                        "گلدون رو نیم‌دور چرخوند تا اون سمتی که خم شده بود دوباره رو به پنجره باشه.");

                    Say(Speaker.Haru, Portrait.Surprised,
                        "It'll just turn back.",
                        "また戻るよ。",
                        "دوباره برمی‌گرده.");

                    Say(Speaker.Yua, Portrait.Smug,
                        "Then I'll turn it again. I've got until March.",
                        "じゃあまたまわす。三月まであるし。",
                        "خب دوباره می‌چرخونمش. تا مارس وقت دارم.");
                },
                "Leave Tomo alone", "トモはそのまま", "تومو رو ول کن",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Neutral,
                        "Leave it. It knows where the light is better than we do.",
                        "そのままでいい。光の場所は、あたしたちよりトモのほうが詳しい。",
                        "ولش کن. از ما بهتر می‌دونه نور کجاست.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "That is the nicest thing you have ever said about anything.",
                        "それ、結愛ぴが何かについて言ったいちばんやさしい台詞。",
                        "این مهربون‌ترین چیزیه که تو عمرت درباره‌ی هر چیزی گفتی.");

                    Say(Speaker.Yua, Portrait.Bored,
                        "It's a plant. Don't build anything on it.",
                        "植物だよ。そこに何も乗せないで。",
                        "یه گیاهه. روش چیزی بنا نکن.");
                });

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Neutral,
                "The café after.",
                "あと、喫茶店。",
                "بعدش کافه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "The café after.",
                "うん、喫茶店。",
                "بعدش کافه.");

            Hold(1.2f);

            Narrate(
                "It rained all the way through the afternoon and nobody in the room said anything worth writing down.",
                "午後じゅう雨で、この部屋で書き留めるようなことは誰も言わなかった。",
                "تمامِ بعدازظهر بارون اومد و هیچ‌کس تو اون کلاس چیزی نگفت که ارزشِ نوشتن داشته باشه.");
        }

        // ---------------------------------------------------------------------
        //  Wednesday, evening — the café
        //
        //  =====================================================================
        //  THE HEAVY SCENE
        //  =====================================================================
        //
        //  ▣ Scene state
        //     Background   CozyCafeDimRainy: the same café under low grey
        //                  light, rain sliding down the big windows, warm lamps
        //                  inside, a matcha cake and a dango tea set on the
        //                  front table.
        //     Date         Wednesday 18 December, evening. Dark outside by five.
        //     On stage     The window, the lamps, the counter, the table by the
        //                  door. Two cups.
        //     From before  She sits by the door. She has sat by the door every
        //                  time since September and it has never been mentioned.
        //
        //  The manual's seven rules, and this scene is nothing but them:
        //
        //   1 ENTER FROM THE SIDE. It arrives out of a conversation about
        //     nothing, through a question that is not about it.
        //   2 IT IS TOLD BADLY. Out of order. He goes back and corrects himself.
        //     He gets stuck on one completely irrelevant detail — a pencil case
        //     — because that is the thing that stayed. He minimises. He stops
        //     in the middle. He apologises for telling her.
        //   3 THE METHOD IS NEVER SAID. Not the detail, not obliquely, not the
        //     place, not the means. This is both the right thing to do and the
        //     better scene.
        //   4 THE HORROR IS IN THE LISTENER. The story is not frightening. What
        //     is frightening is that her reaction is half a degree wrong: one
        //     practical question too many, and a pause broken a second late.
        //   5 NOTHING IS RESOLVED. Nobody says the right thing. Nobody is
        //     comforted. Nobody is better afterwards.
        //   6 THE SCENE DOES NOT END ON IT. It ends on a cup.
        //   7 IT DOES NOT FINISH. It leaks into Thursday as an absence.
        //
        //  ◆ LOCKED FACT 1. On a first reading this must be completely
        //  honest — no wink anywhere. On a second reading, act five has already
        //  told the player there was never any friend, and every line here is a
        //  boy narrating himself in the third person and spoiling his own
        //  ending. Both readings are true at once and neither is the trick.
        //
        //  ◆ THE PLANT FOR ACT SIX is one clause, buried in the middle of a
        //  long list, with no pause after it and nothing marking it: dark, and
        //  full of sudden loud noises. First time through it is strange.
        //  Second time through it is the machine room.
        // ---------------------------------------------------------------------

        private void WriteDecemberCafe()
        {
            ClearStage();

            Place(
                Backgrounds.CafeRainy,
                "The café", "喫茶店", "کافه");

            NoFall(0f);

            Hold(2.4f);

            Narrate(
                "The windows had steamed up from the inside, so the street was a row of coloured smears and the café was the only place that existed.",
                "窓は内側から曇っていて、外の通りは色のにじみになり、この店だけが世界に残った。",
                "پنجره‌ها از تو بخار گرفته بودن، پس خیابون شده بود یه ردیف لکه‌ی رنگی و کافه تنها جایی بود که وجود داشت.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            // Four words. The old version explained that she always took this
            // table and that nobody had ever mentioned it — which is the
            // narrator doing the noticing for the player, on the one detail in
            // the act that is supposed to accumulate silently across five
            // scenes and pay off two acts later.
            Narrate(
                "She took the table by the door.",
                "結愛は入口のそばの席をとった。",
                "میزِ کنارِ در رو گرفت.");

            Hold(1.8f);

            Say(Speaker.Yua, Portrait.Joyful,
                "It's warmer than outside by about forty degrees.",
                "外より四十度くらいあったかい。",
                "از بیرون حدودِ چهل درجه گرم‌تره.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "It's warmer than outside by about eighteen.",
                "十八度くらい。",
                "حدودِ هجده درجه.");

            Say(Speaker.Yua, Portrait.Bored,
                "Nobody asked.",
                "誰も訊いてない。",
                "کسی نپرسید.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You gave a number. A number is an invitation.",
                "数字を出したら、それはもう誘い。",
                "تو یه عدد گفتی. عدد یعنی دعوت.");

            Hold(2.0f);

            Narrate(
                "The cups came. Hers had the thing on top that she takes off and puts on the saucer and never eats.",
                "カップが来た。結愛のには、いつも外して受け皿に置いて、けっきょく食べないものが載っている。",
                "فنجون‌ها اومدن. مالِ اون همون چیزی رو روش داشت که همیشه برمی‌داره، می‌ذاره تو نعلبکی، و هیچ‌وقت نمی‌خوره.");

            // Both cups full, both held. This frame exists so the last one in
            // the scene means something.
            Cel(Portrait.WarmCup, Portrait.WarmCup, 1.6f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Did you get a new one?",
                "新しいの、買った?",
                "نو گرفتی؟");

            Say(Speaker.Haru, Portrait.Surprised,
                "A new what?",
                "なにを?",
                "چیِ نو؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The pencil case. You said in November.",
                "筆箱。十一月に言ってたでしょ。",
                "جامدادی. نوامبر گفتی.");

            Hold(1.6f);

            // 1 ENTER FROM THE SIDE. It comes in here, sideways, and neither of
            // them notices the turn until it has happened.
            Say(Speaker.Haru, Portrait.Unchanged,
                "No.",
                "ううん。",
                "نه.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "It was a present.",
                "もらったやつだから。",
                "هدیه بود.");

            Hold(1.8f);

            // HER FACE THROUGH THIS SCENE.
            //
            // Every one of her lines from here to the end of the café used to
            // be Portrait.Neutral, which is her gentle smile — so she sat and
            // beamed pleasantly through the worst thing anybody has ever told
            // her, for ninety frames, and the scene the whole act is built
            // towards played out on a face that was not listening.
            //
            // The fix is not tears. LOCKED FACT 1 says what is happening in her
            // is satisfaction, not sympathy, so the correct picture is a girl
            // who has stopped smiling and does not start again until she has
            // what she came for. Three states, in order: looking down, arms
            // folded, and then — one beat too early — the smile back.
            //
            // None of the three is a rationed face. Sad and DeadEyes are not
            // spent here and are not needed here.
            Say(Speaker.Yua, Portrait.Shy,
                "Oh.",
                "そっか。",
                "آها.");

            Hold(2.0f);

            Narrate(
                "The rain went down the window in lines that joined each other and then stopped being lines.",
                "雨は窓を筋になって流れ、途中でつながって、筋ではなくなった。",
                "بارون از رو شیشه خط‌خطی می‌رفت پایین، خط‌ها به هم می‌رسیدن و دیگه خط نبودن.");

            Hold(1.6f);

            // 2 TOLD BADLY. Out of order from the first sentence.
            Say(Speaker.Haru, Portrait.Neutral,
                "Can I tell you something. It's not— it's fine, it's not a bad thing, I just haven't said it.",
                "ひとつ、話していい? 別に——わるい話じゃないんだけど、まだ言ってなかったから。",
                "می‌تونم یه چیزی بهت بگم. چیزِ— نه، چیزِ بدی نیست، فقط تا حالا نگفتم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "I had a friend. From school. Not here, the one before.",
                "友だちがいたんだ。学校の。ここじゃなくて、前の。",
                "یه دوست داشتم. از مدرسه. اینجا نه، قبلیش.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "We were in the same class. Seven years. The same class every year, which doesn't happen.",
                "ずっと同じクラスだった。七年。毎年おなじって、ふつうないんだけど。",
                "هم‌کلاسی بودیم. هفت سال. هر سال همون کلاس، که معمولاً پیش نمیاد.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Every day. We talked every day for seven years. That's the part I—",
                "毎日。七年、毎日しゃべってた。そこが——",
                "هر روز. هفت سال هر روز حرف می‌زدیم. همین بخشش که—");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Sheepish,
                "Sorry. I'm doing this out of order.",
                "ごめん。順番めちゃくちゃだ。",
                "ببخشید. دارم بی‌ترتیب می‌گم.");

            Say(Speaker.Yua, Portrait.ColdHug,
                "You're fine.",
                "いいよ。",
                "اشکالی نداره.");

            Hold(1.8f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "He wasn't a talker. When he was happy he'd talk for an hour about nothing. When he wasn't he'd go quiet, and I'd know, and I'd ask, and he'd say he was fine.",
                "しゃべるほうじゃなかった。機嫌がいいときは一時間くらいどうでもいい話をして、そうじゃないときは黙る。わかるから訊くんだけど、平気って言う。",
                "اهلِ حرف زدن نبود. وقتی حالش خوب بود یه ساعت درباره‌ی هیچی حرف می‌زد. وقتی نبود ساکت می‌شد، و من می‌فهمیدم، و می‌پرسیدم، و می‌گفت خوبم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "So I'd say all the things. Trust me. Tell me. I'm here. Who else are you going to tell. Don't keep it in.",
                "だから全部言った。信じてよ、話してよ、ここにいるよ、僕に言わないなら誰に言うの、ためこまないで。",
                "پس همه‌ی اون حرف‌ها رو می‌زدم. بهم اعتماد کن. بهم بگو. من کنارتم. به من نگی به کی می‌گی. نریز تو خودت.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "And he'd say he was fine.",
                "で、平気って言う。",
                "و می‌گفت خوبم.");

            Hold(2.0f);

            // The irrelevant detail that stayed. This is the pencil case, and
            // the player has already been told once, in November, that his own
            // does not shut. Nothing points at that. It is four frames.
            Say(Speaker.Haru, Portrait.Unchanged,
                "He had a pencil case with a zip that didn't shut. He'd had it since second year. It was the stupidest thing and he wouldn't get rid of it.",
                "ファスナーの閉まらない筆箱を持ってた。二年のときからずっと。どうしようもないやつなのに、捨てなかった。",
                "یه جامدادی داشت که زیپش بسته نمی‌شد. از کلاس دوم داشتش. مسخره‌ترین چیزِ ممکن بود و حاضر نبود بندازتش دور.");

            Say(Speaker.Haru, Portrait.StrainedSmile,
                "I don't know why that's the thing I remember.",
                "なんでそれを覚えてるのか、自分でもわからない。",
                "نمی‌دونم چرا همین یکی اونیه که یادم مونده.");

            Hold(2.2f);

            // Arms folded, completely still, and she stays exactly like this
            // for the next forty frames. It is the longest single picture in
            // act two and it is doing all of the work.
            Say(Speaker.Yua, Portrait.Unchanged,
                "Go on.",
                "つづけて。",
                "بگو.");

            Hold(1.6f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "I thought maybe he didn't trust me. Or he thought I was a kid. Or he thought I had my own things and no room for his. Or that if he said it out loud I'd start feeling sorry for him, and he'd rather have a friend than be somebody's sad thing.",
                "信用されてないのかと思った。子どもだと思われてるのかも。こっちにも事情があるから、入る隙がないと思ってるのかも。口に出したら僕が気の毒がるから、かわいそうな存在になるくらいなら友だちのままでいたい、とか。",
                "فکر کردم شاید بهم اعتماد نداره. یا فکر می‌کنه بچه‌ام. یا فکر می‌کنه من خودم مشکلاتِ خودمو دارم و جا واسه مالِ اون نیست. یا اگه بگه، من دلم براش می‌سوزه و ترجیح می‌ده دوست باشه تا چیزِ غم‌انگیزِ یکی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "So I used to complain to him. On purpose. About my day, about nothing, so it wasn't one-way. So he'd have something to—",
                "だから、わざとこっちの愚痴を言った。今日のこととか、どうでもいいこととか。一方通行にならないように。あいつにも——",
                "واسه همین عمداً واسش غر می‌زدم. از روزم، از هیچی، که یک‌طرفه نباشه. که اونم یه چیزی داشته باشه که—");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "It didn't work.",
                "効かなかった。",
                "جواب نداد.");

            Hold(2.4f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "We weren't in the same class after eighth year.",
                "八年のあと、同じクラスじゃなくなった。",
                "بعد از کلاس هشتم دیگه هم‌کلاسی نشدیم.");

            Hold(2.0f);

            // 3 THE METHOD IS NEVER SAID. This is the only sentence about it and
            // there will not be another, in this act or any other.
            Say(Speaker.Haru, Portrait.Unchanged,
                "Guess why.",
                "なんでだと思う。",
                "حدس بزن چرا.");

            Hold(2.4f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "Yeah.",
                "うん。",
                "آره.");

            Hold(2.6f);

            Narrate(
                "The door went and somebody came in shaking an umbrella, and the cold came in with them and went away again.",
                "ドアが鳴って、傘を振りながら誰かが入ってきた。冷たい空気も一緒に入って、また出ていった。",
                "در باز شد و یکی اومد تو و چترشو تکون داد، و سرما باهاش اومد تو و دوباره رفت.");

            Hold(2.2f);

            // 4 THE HORROR IS IN THE LISTENER. One practical question too
            // many, asked off the same unmoved picture she has been wearing
            // since "Go on." Unchanged is the point: nothing in her has
            // altered between "I'm sorry about your friend" and "does anyone
            // here know", and the frame does not alter either.
            Say(Speaker.Yua, Portrait.Unchanged,
                "How long ago?",
                "いつごろ?",
                "چند وقت پیش؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "A while.",
                "だいぶ前。",
                "یه مدتی هست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Does anyone here know?",
                "ここの人たち、知ってる?",
                "اینجا کسی می‌دونه؟");

            Say(Speaker.Haru, Portrait.Surprised,
                "...No.",
                "……ううん。",
                "...نه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Good.",
                "よかった。",
                "خوبه.");

            Hold(1.0f);

            // And the smile comes back here, on the line that is covering for
            // "Good." It is one beat too early to be a recovery and nobody in
            // the scene notices, including her.
            Say(Speaker.Yua, Portrait.Neutral,
                "I mean — it's yours. It should be yours.",
                "ううん、そうじゃなくて。ハルぴのものだから。ハルぴのままがいい。",
                "یعنی — مالِ خودته. باید مالِ خودت بمونه.");

            Hold(2.4f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "I don't know how to ask you this.",
                "どう頼めばいいか、わからないんだけど。",
                "نمی‌دونم چطوری ازت بخوام.");

            Hold(1.6f);

            // THE LONGEST LINE IN THE GAME, AND IT DID NOT FIT IN THE BOX.
            // In Persian it ran off the bottom of the dialogue panel and the
            // last third of it could not be read at all — on the one speech the
            // whole act is built towards. Split into four frames, each of which
            // fits, and the pauses between them are better than the single
            // block was: he is running out of breath, which is what this is.
            //
            // The plant for act six is still one clause in the middle of a
            // list, with nothing after it and no pause on it, and it is still
            // in the same frame as three other clauses so that it cannot be
            // isolated by anybody who is not looking for it.
            Say(Speaker.Haru, Portrait.Sad,
                "If something's wrong. Any time. If something from before is still sitting there.",
                "もしなにかあったら。いつでも。前のことがまだ残ってるなら。",
                "اگه یه چیزی درست نیست. هر وقتی. اگه چیزی از قبل هنوز مونده.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Even if it's small, even if it's old, even if it's my fault for not being strong enough, even if it isn't your fault at all, even if it was dark and full of loud sudden noises —",
                "小さくても、古くても、僕が弱かったせいでも、きみのせいじゃなくても、暗くて大きな音がいきなりする場所のことでも——",
                "حتی اگه کوچیک باشه، حتی اگه قدیمی باشه، حتی اگه تقصیرِ منه که به‌قدرِ کافی قوی نبودم، حتی اگه اصلاً تقصیرِ تو نبوده، حتی اگه تاریک بوده و پر از صداهای بلندِ ناگهانی —");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I'll get on the floor, I'll beg, I don't care. Just come and tell me you're not okay.",
                "土下座でもなんでもする。ただ、来て、大丈夫じゃないって言って。",
                "می‌افتم به پات، التماس می‌کنم، برام مهم نیست. فقط بیا و بگو حالت خوب نیست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You don't have to do it properly. You don't have to know the words. Just say it and I'll put the whole world down and sit there until you're better. Promise me.",
                "うまく言わなくていい。言葉なんてわからなくていい。言ってくれたら、僕は世界ぜんぶ置いて、よくなるまでそこにいる。約束して。",
                "لازم نیست درست بگیش. لازم نیست کلمه‌هاشو بلد باشی. فقط بگو، و من کلِ دنیا رو می‌ذارم زمین و می‌شینم تا حالت خوب بشه. قول بده.");

            Hold(3.0f);

            // 5 NOTHING IS RESOLVED. She does not say the right thing, because
            // there is no right thing, and because she is not trying to.
            //
            // Thinking rather than Neutral: the drawing is her with one hand at
            // her chest and the other raised, which in this project is not
            // "she is working it out" but "she has decided". Two words, and she
            // has already decided what they are for.
            Say(Speaker.Yua, Portrait.Thinking,
                "I promise.",
                "約束する。",
                "قول می‌دم.");

            Hold(2.0f);

            Say(Speaker.Haru, Portrait.StrainedSmile,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Sheepish,
                "Sorry. That was a lot.",
                "ごめん。重かったね。",
                "ببخشید. زیادی بود.");

            Say(Speaker.Yua, Portrait.Neutral,
                "It wasn't. Don't do that.",
                "重くないよ。やめて。",
                "نبود. این کارو نکن.");

            Hold(2.6f);

            // 6 THE SCENE DOES NOT END ON IT. It ends on a cup.
            Narrate(
                "Her cup had gone cold. She had not touched it and the thing on top was still on the saucer where she had put it.",
                "結愛のカップは冷めていた。一度も手をつけず、上に載っていたものは、置いたときのまま受け皿にあった。",
                "فنجونش سرد شده بود. دست نزده بود بهش و اون چیزی که روش بود هنوز همون‌جا تو نعلبکی بود، جایی که گذاشته بودش.");

            // Rule six, in one picture. His cup is finished and hers is the
            // same drawing it was ninety frames ago, because she has not moved
            // it. That is the whole scene, and nobody says a word over it.
            Cel(Portrait.WarmCup, Portrait.WarmCupEmpty, 2.4f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Do you want this? I'm not going to eat it.",
                "これ、いる? 食べないから。",
                "اینو می‌خوای؟ من نمی‌خورمش.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You never eat it.",
                "いつも食べないよね。",
                "هیچ‌وقت نمی‌خوریش.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I like that they put it on there. I've never once wanted to eat it.",
                "載せてくれるのが好きなだけ。食べたいと思ったことは一度もない。",
                "خوشم میاد که می‌ذارنش روش. تا حالا حتی یه بارم نخواستم بخورمش.");

            Hold(2.2f);

            Narrate(
                "He ate it. The rain kept going. Somebody at the counter laughed at something, and it sounded like it was happening in a different café, on a different evening, to other people.",
                "ハルはそれを食べた。雨はつづいていた。カウンターのほうで誰かが笑って、その笑い声は、べつの店の、べつの夜の、べつの誰かのもののように聞こえた。",
                "خوردش. بارون ادامه داشت. یکی دمِ پیشخون به یه چیزی خندید، و صداش جوری بود انگار تو یه کافه‌ی دیگه، تو یه شبِ دیگه، واسه یه آدم‌های دیگه داره اتفاق می‌افته.");

            Hold(2.4f);

            Narrate(
                "They walked home the long way, past the bakery, because it was the long way.",
                "帰りはパン屋のほうの遠回りで帰った。遠回りだから。",
                "از راهِ دور رفتن خونه، از جلوی نونوایی، چون راهِ دور بود.");

            Hold(2.0f);

            Exit(Speaker.Haru);

            Hold(2.4f);

            // The monologue after. LOCKED FACT 1 is precise about this: it is
            // satisfaction, not relief, and not sympathy. She has been handed
            // something and she knows it.
            InnerVoice(
                "He's never told anyone that.",
                "誰にも言ってない話。",
                "این رو به هیچ‌کس نگفته.");

            Hold(1.4f);

            InnerVoice(
                "He said so. Nobody here knows.",
                "本人がそう言った。ここでは誰も知らない。",
                "خودش گفت. اینجا کسی نمی‌دونه.");

            Hold(1.6f);

            InnerVoice(
                "And he asked me to promise.",
                "そのうえで、約束してって言った。",
                "و ازم خواست قول بدم.");

            Hold(2.0f);

            InnerVoice(
                "He asked me.",
                "あたしに、言った。",
                "از من خواست.");

            Hold(2.6f);

            InnerVoice(
                "...Okay.",
                "……うん。",
                "...باشه.");
        }

        // =====================================================================
        //  THURSDAY 19 DECEMBER — the day after
        //
        //  Rule seven: it does not finish, it leaks, and it leaks as an ABSENCE.
        //  Nobody refers to yesterday. Nothing is processed. One of them is
        //  quieter than usual and the other one is louder to cover it, and
        //  neither says why, and the player is left holding it.
        // =====================================================================

        private void WriteDecemberThursday()
        {
            WriteThursdayClassroom();
            WriteThursdayPlatform();
        }

        // ---------------------------------------------------------------------
        //  Thursday, morning — 1-A
        //
        //  ▣ Scene state
        //     Background   SunnyClassroomDay. It stopped raining in the night
        //                  and December sun through glass is a lie about
        //                  temperature.
        //     Date         Thursday 19 December 2024.
        //     On stage     Desks, the chalkboard, the window, Tomo.
        //     From before  Everything, and none of it out loud.
        //
        //  He is a beat quieter than he has been for two acts. She is louder,
        //  faster and funnier than usual, and it is not kindness — it is
        //  management. Neither of these is named.
        // ---------------------------------------------------------------------

        private void WriteThursdayClassroom()
        {
            ClearStage();

            Place(
                Backgrounds.ClassroomDay,
                "1-A", "一年A組", "اول-الف");

            Date(2024, 12, 19);

            NoFall(0f);

            Hold(2.2f);

            Narrate(
                "The rain had gone over to snow at some point in the night and then stopped. December sun through glass is a lie about temperature and everybody in the room fell for it every year.",
                "雨は夜のどこかで雪に変わって、それからやんだ。ガラス越しの十二月の陽射しは気温についての嘘で、毎年みんなそれに騙される。",
                "بارون یه‌جایی تو شب برف شده بود و بعد بند اومده بود. آفتابِ دسامبر از پشتِ شیشه دروغیه درباره‌ی دما و هر سال همه‌ی این کلاس بهش گول می‌خورن.");

            Enter(Speaker.Haru, Portrait.Neutral);
            Enter(Speaker.Yua, Portrait.Joyful);

            Say(Speaker.Yua, Portrait.Joyful,
                "Right, so the vending machine by the staff room takes two hundred and gives you one hundred and eighty back, which means somebody in this building is running a charity and nobody has told them.",
                "職員室のとこの自販機、二百円入れると百八十円返ってくる。この校舎の誰かが慈善事業やってて、本人が知らされてない。",
                "خب، دستگاهِ کنارِ دفتر دویست ین می‌گیره و صد و هشتاد پس می‌ده، یعنی یکی تو این ساختمون داره خیریه اداره می‌کنه و کسی بهش نگفته.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Mm.",
                "ん。",
                "هوم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "So obviously the correct move is to only ever buy things in pairs.",
                "だから、買うときは必ず二個ずつ。それが正解。",
                "پس معلومه که کارِ درست اینه که همیشه دوتایی بخری.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Mm.",
                "ん。",
                "هوم.");

            Hold(1.8f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "That was two mms.",
                "「ん」二回目。",
                "این دومین «هوم» بود.");

            Say(Speaker.Haru, Portrait.Sheepish,
                "Sorry. I'm listening.",
                "ごめん。聞いてる。",
                "ببخشید. دارم گوش می‌دم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "You're not, but carry on.",
                "聞いてないけど、いいよ。",
                "نمی‌دی، ولی ادامه بده.");

            Hold(2.0f);

            Narrate(
                "Morita-sensei handed back a set of papers and said one sentence about the holidays that everybody heard and nobody wrote down.",
                "森田先生がプリントを返して、休みについて一文だけ言った。全員が聞いて、誰も書かなかった。",
                "خانمِ موریتا یه سری برگه پس داد و یه جمله درباره‌ی تعطیلات گفت که همه شنیدن و هیچ‌کس ننوشت.");

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Did you sleep?",
                "寝た?",
                "خوابیدی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Yeah.",
                "うん。",
                "آره.");

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Okay.",
                "そっか。",
                "باشه.");

            Hold(2.2f);

            // RULE SEVEN IS AN ABSENCE, SO IT CANNOT BE NARRATED. The old
            // version had the narrator say that neither of them mentioned the
            // café then or later — which tells the player the exact thing the
            // scene is built to make them feel, and turns a hole into a caption.
            // What is here instead is the day going on, in objects.
            Narrate(
                "The bell went for fourth period. Somebody's phone buzzed twice inside a bag and the whole room decided together not to have heard it.",
                "四時間目のチャイムが鳴った。誰かの鞄の中で携帯が二回震えて、教室ぜんぶが、聞かなかったことにした。",
                "زنگِ زنگِ چهارم خورد. گوشیِ یکی دو بار تو کیفش لرزید و کلِ کلاس با هم تصمیم گرفت نشنیده باشدش.");

            Hold(1.8f);

            DecideIdly(
                "Ask him about yesterday", "昨日のこと、訊く", "درباره‌ی دیروز ازش بپرس",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Neutral,
                        "Hey. About yesterday—",
                        "ねえ。昨日のことだけど——",
                        "هی. درباره‌ی دیروز—");

                    Say(Speaker.Haru, Portrait.Surprised,
                        "Mm?",
                        "ん?",
                        "هوم؟");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "...Did you eat anything this morning.",
                        "……今朝、なんか食べた?",
                        "...امروز صبح چیزی خوردی؟");

                    Say(Speaker.Haru, Portrait.Sheepish,
                        "...No.",
                        "……ううん。",
                        "...نه.");

                    Say(Speaker.Yua, Portrait.Bored,
                        "Thought so.",
                        "だと思った。",
                        "حدس می‌زدم.");
                },
                "Talk about something else", "別の話をする", "درباره‌ی چیزِ دیگه‌ای حرف بزن",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Joyful,
                        "The staff room machine gives you a hundred and eighty back.",
                        "職員室の自販機、百八十円返ってくるんだって。",
                        "دستگاهِ کنارِ دفتر صد و هشتاد پس می‌ده.");

                    Say(Speaker.Haru, Portrait.Bored,
                        "You said.",
                        "さっき聞いた。",
                        "گفتی.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "I'm saying it again because you weren't listening the first time.",
                        "一回目に聞いてなかったから、もう一回言ってる。",
                        "دوباره می‌گم چون بارِ اول گوش نمی‌دادی.");

                    Say(Speaker.Haru, Portrait.Sheepish,
                        "I was listening.",
                        "聞いてたよ。",
                        "گوش می‌دادم.");

                    Say(Speaker.Yua, Portrait.Smug,
                        "You said \"mm\". Four times.",
                        "「ん」って言った。四回。",
                        "گفتی «هوم». چهار بار.");
                });

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Smug,
                "Station after. It's the last Thursday before the holidays and the stall does the seasonal one.",
                "あと、駅。休み前の最後の木曜だから、屋台に季節のやつ出る。",
                "بعدش ایستگاه. آخرین پنج‌شنبه قبل از تعطیلاته و دکه اون مدلِ فصلی رو می‌ذاره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You've had this planned since—",
                "これ、いつから決めて——",
                "از کِی نقشه‌شو کشیدی—");

            Say(Speaker.Yua, Portrait.Joyful,
                "November.",
                "十一月。",
                "نوامبر.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Of course.",
                "だよね。",
                "معلومه.");
        }

        // ---------------------------------------------------------------------
        //  Thursday, evening — the platform
        //
        //  ▣ Scene state
        //     Background   TrainPlatformWinterSunset. Not a sunset, whatever
        //                  the file is called: a flat white winter afternoon
        //                  with snow lying on the canopy, icicles along its
        //                  edge, snow on the seats and bare black trees down
        //                  the track. The narration follows the picture rather
        //                  than the file name — the snow is last night's, from
        //                  the rain in the café scene, and this is the only
        //                  place in the first two acts where the weather of one
        //                  day is visibly still there on the next.
        //     Date         Thursday 19 December, evening.
        //     On stage     The seats, the machine, the stall, the track, bare
        //                  trees along it.
        //     From act 1   The classmate asked about "-pi" on this platform in
        //                  September and was told it was theirs.
        //
        //  Warm, funny, and with a hole in the middle of it that neither of them
        //  goes near. The act's last light scene: everything after this is her
        //  bedroom.
        // ---------------------------------------------------------------------

        private void WriteThursdayPlatform()
        {
            ClearStage();

            Place(
                Backgrounds.TrainPlatformWinterSunset,
                "The platform", "ホーム", "سکوی ایستگاه");

            // Nothing falling yet. It starts again at the end of the scene, and
            // that is the only weather beat in act two that is not a leaf.
            NoFall(0f);

            Hold(2.2f);

            Narrate(
                "Last night's snow was still on the canopy and still on the seats, because nothing on this side of the station gets the sun. The trees along the track had nothing on them at all.",
                "昨夜の雪が屋根にも椅子にも残っていた。駅のこちら側には陽が当たらない。線路沿いの木には、もう何も残っていなかった。",
                "برفِ دیشب هنوز رو سایبون بود و هنوز رو صندلی‌ها، چون این‌ورِ ایستگاه اصلاً آفتاب نمی‌خوره. درخت‌های کنارِ ریل دیگه هیچی روشون نبود.");

            // ART CHECK. The dango stall is drawn into TrainPlatformAutumnSunset
            // and into NEITHER the winter nor the spring version of this
            // platform — so on the 19th of December there is no stall anywhere
            // in this picture. They buy it downstairs at the ticket gate, which
            // is off screen and always was, and carry it up. Nothing on the
            // platform now refers to a stall that is not on the platform.
            Narrate(
                "The stand by the ticket gate had the winter dango out, which are the ordinary dango in a different shape and cost thirty yen more for being it.",
                "改札のところの屋台に、冬の団子が出ていた。いつもの団子を別の形にしただけで、その形のぶん三十円高い。",
                "دکه‌ی کنارِ گیت دانگوی زمستونی گذاشته بود بیرون، که همون دانگوی همیشگیه با یه شکلِ دیگه و به‌خاطرِ همون شکل سی ین گرون‌تره.");

            Hold(1.4f);

            Enter(Speaker.Yua, Portrait.Joyful);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Surprised,
                "They're a different shape.",
                "形、ちがう。",
                "شکلشون فرق داره.");

            Say(Speaker.Haru, Portrait.Neutral,
                "They're seasonal.",
                "季節のやつだから。",
                "فصلی‌ان.");

            Say(Speaker.Yua, Portrait.Pout,
                "\"Seasonal\" isn't a flavour.",
                "「季節」は味じゃない。",
                "«فصلی» که طعم نیست.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "Nobody said it was a flavour. You said shape. It is a shape.",
                "味だなんて誰も言ってない。形って言ったのは結愛ぴ。で、形ではある。",
                "کسی نگفت طعمه. خودت گفتی شکل. خب، شکله دیگه.");

            Say(Speaker.Yua, Portrait.Bored,
                "I walked into that.",
                "自分で言った。",
                "خودم رفتم توش.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You did. From quite a long way away.",
                "けっこう遠くから、まっすぐ。",
                "آره. از یه فاصله‌ی نسبتاً دور.");

            Hold(2.0f);

            Narrate(
                "An orange train went through the far platform without stopping, which it does, and which by now was not worth either of them mentioning.",
                "オレンジの電車が向かいのホームを止まらずに通過した。いつものことで、もう二人とも口にしない。",
                "یه قطارِ نارنجی از سکوی روبه‌رو بدونِ توقف رد شد، که کارشه، و دیگه ارزشِ گفتن نداشت.");

            Hold(1.8f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Two weeks off.",
                "二週間休み。",
                "دو هفته تعطیلی.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Two weeks.",
                "二週間。",
                "دو هفته.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "What are you doing with them?",
                "なにするの?",
                "باهاشون چیکار می‌کنی؟");

            Say(Speaker.Haru, Portrait.Sheepish,
                "Nothing, probably.",
                "たぶん、なにも。",
                "احتمالاً هیچی.");

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Smug,
                "Wrong. You're doing things with them. I'll tell you which ones on Sunday.",
                "はずれ。なにかする。日曜に言う。",
                "غلط. یه کارایی باهاشون می‌کنی. یکشنبه بهت می‌گم کدوماش.");

            Say(Speaker.Haru, Portrait.Surprised,
                "Sunday?",
                "日曜?",
                "یکشنبه؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "Sunday. Come to mine. My mother's out all day.",
                "日曜。うち来て。母、一日いないから。",
                "یکشنبه. بیا خونه‌ی ما. مامانم کلِ روز نیست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Oh — and we've got a heater that actually works, so you can stop doing the thing where you're cold at me and won't say it.",
                "あ、それと、うちの暖房はちゃんと効くから。寒いのに寒いって言わないやつ、やらなくていいよ。",
                "راستی — یه بخاری هم داریم که واقعاً کار می‌کنه، پس دیگه لازم نیست اون کارو بکنی که سردته و نمی‌گی.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(2.2f);

            Narrate(
                "It got properly cold on the platform, the way it does once the light has gone, and he stood in it without doing anything about it, which he had not done before.",
                "光が消えたあとの本格的な寒さが来た。ハルはその中に、なにもせずに立っていた。今まではしなかったことだ。",
                "سکو حسابی سرد شد، همون‌جوری که بعد از رفتنِ نور می‌شه، و هارو توش وایساد و هیچ کاری براش نکرد، که قبلاً نمی‌کرد.");

            // Rule seven, as an absence, in one picture: she folds her arms the
            // way she did on the 5th of November, and this time nothing crosses
            // the gap. Six weeks ago he bought a second can for it. Neither of
            // them mentions that, and the frame does not linger.
            Cel(Portrait.ColdHug, Portrait.Unchanged, 1.8f);

            Say(Speaker.Haru, Portrait.StrainedSmile,
                "Hey.",
                "ねえ。",
                "هی.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Mm?",
                "ん?",
                "هوم؟");

            Hold(1.6f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "Nothing. Forget it.",
                "ううん。なんでもない。",
                "هیچی. ولش کن.");

            Hold(2.4f);

            // It starts again here, thinly, and nobody in the scene remarks on
            // it. Six seconds to come up, so it is at its lightest under the
            // last three lines and heaviest over the empty platform after he
            // has gone.
            Fall(FallKind.Snow, 0.10f, 6f);

            Say(Speaker.Yua, Portrait.Joyful,
                "Your train.",
                "電車来た。",
                "قطارت.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Sunday, then.",
                "じゃあ日曜。",
                "پس یکشنبه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Sunday, Haru-pi.",
                "日曜ね、ハルぴ。",
                "یکشنبه، هاروپی.");

            Exit(Speaker.Haru);

            Hold(2.6f);

            Narrate(
                "The platform emptied out the way platforms do, all at once and then not at all, and she stayed on it after her own train had been and gone.",
                "ホームは、ホームらしく一度にからっぽになって、それきり誰も来なかった。結愛は、自分の電車が来て行ったあとも、そこにいた。",
                "سکو همون‌جوری که سکوها خالی می‌شن خالی شد، یکهو و بعد هیچ، و یوآ بعد از اینکه قطارِ خودش اومد و رفت هم موند.");
        }

        // =====================================================================
        //  SUNDAY 22 DECEMBER — her room
        //
        //  The top of the ladder, and the last scene of the act.
        // =====================================================================

        private void WriteDecemberSunday()
        {
            WriteSundayRoom();
        }

        // ---------------------------------------------------------------------
        //  Sunday, afternoon — Yua's room
        //
        //  ▣ Scene state
        //     Background   YuaRoomSunnyDay: a patchwork bed with soft toys on
        //                  it and a floppy-eared rabbit at the front, a cracked
        //                  mirror on the wall, a desk with a laptop, shelves.
        //     Date         Sunday 22 December 2024.
        //     On stage     The bed, the rabbit, the mirror, the desk.
        //     In hand      Nothing.
        //     From before  He came, as arranged, and has gone home.
        //
        //  ◆ LADDER RUNG FIVE. The fourth wall, for the first time in the game.
        //
        //  The manual's six rules for this and it obeys all of them:
        //    1 Direct about the want, never about the reason. She gives orders,
        //      she makes a deal, she never explains herself. The word "control"
        //      is never said. The player works it out FROM HAVING BEEN GIVEN AN
        //      ORDER, which is the only way it lands.
        //    2 It has to be charming. The frightening version is the one where
        //      the player half wants to say yes. A visibly evil version gets
        //      disagreed with and no complicity forms.
        //    3 The player cannot answer. No choice appears. The game quietly
        //      takes the player's agency away and says nothing about it, and
        //      that is the most frightening thing in the scene.
        //    4 She assumes the player is already in on it. Not "will you help
        //      me" — "you've been watching, so you know".
        //    5 Short.
        //    6 Nobody mentions it afterwards. Act three opens completely
        //      normally.
        //
        //  And the mechanics table: the blue and green buttons are NAMED here
        //  and do not EXIST until act three. Not one appears in this act.
        // ---------------------------------------------------------------------

        private void WriteSundayRoom()
        {
            ClearStage();

            // The 22nd of December. The autumn version of this room has maples
            // through the window and this one has a bare tree and snow, which
            // is what is outside on the day the act ends.
            Place(
                Backgrounds.YuaRoomWinterDay,
                "Yua's room", "結愛の部屋", "اتاقِ یوآ");

            Date(2024, 12, 22);

            NoFall(0f);

            Hold(2.4f);

            Narrate(
                "The twenty-second of December. He had arrived at two in the afternoon and gone home at six, and the two cups they had drunk out of were still on the desk.",
                "十二月二十二日。ハルは午後二時に来て、六時に帰った。二人が使ったカップは、まだ机の上にあった。",
                "بیست‌ودوِ دسامبر. ساعتِ دوِ بعدازظهر اومده بود و ساعتِ شیش رفته بود خونه، و اون دو تا فنجونی که توش چای خورده بودن هنوز رو میز بود.");

            Enter(Speaker.Yua, Portrait.Neutral);

            Hold(2.0f);

            Narrate(
                "The rabbit at the front of the bed has one ear that has gone thin from being held, and it is the left one.",
                "ベッドの前のうさぎは、片方の耳だけ持たれすぎて薄くなっている。左の耳。",
                "خرگوشِ جلوی تخت یه گوشش از بس گرفته شده نازک شده، و همون گوشِ چپه.");

            Hold(2.2f);

            InnerVoice(
                "Haru-pi said yes to every single thing I asked him for today.",
                "今日あたしが頼んだこと、ハルぴはぜんぶ、うんって言った。",
                "هاروپی به همه‌ی چیزایی که امروز ازش خواستم گفت آره.");

            InnerVoice(
                "Two weeks of the holidays. Every day of it. He didn't even ask what we'd be doing.",
                "冬休みの二週間。全部の日。なにするかも訊かなかった。",
                "دو هفته‌ی تعطیلات. هر روزش. حتی نپرسید قراره چیکار کنیم.");

            Hold(2.0f);

            // THE TURN.
            //
            // It used to be BeginAside() and nothing else, so the only thing
            // that happened was that the room got slightly darker — and a
            // player who had been reading for an hour did not necessarily
            // notice that the game had just started talking to them.
            //
            // Four things now happen at once, and they are all things the
            // engine already had:
            //   · the music stops dead, in one frame, with no fade
            //   · the string goes taut (SfxId.StringPull — the one sound in the
            //     palette that is not in the key, and the one written for
            //     exactly this)
            //   · the room dims, which is BeginAside's own job
            //   · the typing slows to AsideTypeSpeed, which is also its job
            //
            // Then two and a half seconds of nothing before she says anything,
            // which is the longest silence in the first two acts.
            CutMusic();

            Cue(SfxId.StringPull, 0.85f);

            BeginAside();

            Hold(2.8f);

            Say(Speaker.Yua, Portrait.Neutral,
                "You're still there.",
                "まだ、いるよね。",
                "هنوز اونجایی.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's fine. I'm not angry about it.",
                "べつにいい。怒ってない。",
                "اشکالی نداره. ازش عصبانی نیستم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You've been watching since September. So you already know most of this.",
                "九月からずっと見てる。だから、だいたいもう知ってるでしょ。",
                "از سپتامبر داری نگاه می‌کنی. پس بیشترِ اینا رو از قبل می‌دونی.");

            Hold(2.2f);

            Say(Speaker.Yua, Portrait.Neutral,
                "I want him.",
                "ハルぴが、ほしい。",
                "من اونو می‌خوام.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Not in the way people say it. I want him the way you want a thing to stay where you put it.",
                "みんなが言う意味じゃなくて。置いたところに、ずっとある。そういうほしさ。",
                "نه اون‌جوری که مردم می‌گن. اون‌جوری می‌خوامش که آدم می‌خواد یه چیزی همون‌جایی که گذاشتتش بمونه.");

            Hold(2.4f);

            Say(Speaker.Yua, Portrait.Neutral,
                "From January there'll be two buttons.",
                "一月から、ボタンが二つ出る。",
                "از ژانویه دو تا دکمه میاد.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "A blue one and a green one. They'll come up when I'm talking. Not when he is — he doesn't get any.",
                "青と緑。あたしが話してるときに出る。ハルぴのときは出ない。あの子にはない。",
                "یکی آبی، یکی سبز. وقتی من حرف می‌زنم میان. وقتی اون حرف می‌زنه نه — اون هیچی نداره.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Smug,
                "Press the green ones.",
                "緑を押して。",
                "سبزها رو بزن.");

            Hold(2.4f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Don't worry about him. He likes me. It's fine if I steer a bit.",
                "心配しないで。あの子はあたしのこと好きだから。少しくらい動かしても平気。",
                "نگرانش نباش. اون منو دوست داره. اشکالی نداره اگه یه‌کم هدایتش کنم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Honestly I think he likes me enough to break his own leg for me. Don't you think so?",
                "正直、あたしのためなら自分の脚くらい折る子だと思う。そう思わない?",
                "راستش فکر می‌کنم اون‌قدری منو دوست داره که حاضره پاشو واسه من بشکنه. تو این‌طور فکر نمی‌کنی؟");

            Hold(3.0f);

            Say(Speaker.Yua, Portrait.Neutral,
                "And don't worry about him finding out about you.",
                "それと、あんたのこと、ハルぴにばれる心配もしないで。",
                "و نگرانِ این نباش که هارو از تو باخبر بشه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I don't think he knows you're watching our lives. I think it's only me.",
                "たぶん、見られてるって知らない。知ってるのは、たぶんあたしだけ。",
                "فکر نکنم بدونه داری زندگی‌مون رو می‌بینی. فکر کنم فقط من می‌دونم.");

            Hold(2.6f);

            Say(Speaker.Yua, Portrait.Smug,
                "Green ones. That's all I'm asking.",
                "緑。それだけ。",
                "سبزها. همین رو می‌خوام.");

            Hold(2.4f);

            // The invitation, which the act needed and did not have. Everything
            // before this is instructions; this is the first line in the game
            // that offers the player something back, and it is the line that
            // makes the next three acts feel like theirs.
            Say(Speaker.Yua, Portrait.Neutral,
                "And if you do it — if you keep pressing them — then some of what happens to him is yours.",
                "押してくれたら、ハルぴに起きることの一部は、あんたのものになる。",
                "و اگه بزنیشون — اگه همین‌جور بزنیشون — اون‌وقت یه بخشی از اتفاقایی که واسش می‌افته مالِ توئه.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Which means a little bit of him is yours too. Don't you think that's nice?",
                "ってことは、ハルぴの少しは、あんたのもの。いいと思わない?",
                "یعنی یه‌ذره‌ی خودشم مالِ توئه. فکر نمی‌کنی این قشنگه؟");

            Hold(2.6f);

            Say(Speaker.Yua, Portrait.Joyful,
                "Thank you.",
                "ありがと。",
                "ممنون.");

            Hold(2.2f);

            EndAside();

            // The room comes back, and so does the music, and the scene goes on
            // as though the last ninety seconds did not happen — which is rule
            // six of this kind of scene and is also what she would do.
            SetMusic(MusicTrack);

            Hold(2.0f);

            // 6 NOBODY MENTIONS IT AFTERWARDS — starting immediately.
            Narrate(
                "She took the two cups downstairs, and washed them, and put them on the rack the way they go, and that was the end of the year.",
                "結愛はカップを二つ下に持っていって、洗って、いつもの向きで水切りに置いた。その年は、それで終わった。",
                "دو تا فنجون رو برد پایین، شستشون، و همون‌جوری که باید گذاشتشون تو جاظرفی، و سال همین‌جا تموم شد.");

            Hold(2.4f);

            // =================================================================
            //  SELF-AUDIT — ACT TWO
            // =================================================================
            //
            //  SHAPE
            //    5 days across 11 weeks · 10 scenes · 282 written frames · 4
            //    white choices, ALL FOUR OF WHICH BRANCH. A playthrough reads
            //    about 264 of the 282.
            //
            //    Ten scenes rather than nine: the half-landing was split out of
            //    the roof scene and given the picture it always needed. It is
            //    five frames long and it is the act's first silence.
            //
            //    ZERO blue and ZERO green, as the mechanics table requires.
            //    Every white road is Yua's.
            //
            //  THE LADDER
            //    Act one reached rung three. This act takes four and five, in
            //    that order, one scene apart, and stops.
            //      rung 4  5 November, the wall by the flowerbeds. What she says
            //              out loud and what is in her head stop matching, for
            //              the first time in the game. "Thank you." / "...Good."
            //      rung 5  22 December, her room. The fourth wall.
            //    Rung six — the game itself doing something it should not — is
            //    act five's and is not touched.
            //
            //  THE TWO SILENCES  (Listen, i.e. MeasurePatience)
            //    7 October, the half-landing: his leg stops him and she does not
            //      ask.
            //    5 November, the corridor: the machine room reaches her through
            //      a floor, she says she has been frightened of that sound since
            //      she was small, he says "I know", AND SHE DOES NOT ASK HOW HE
            //      KNOWS. That last part is the act's best line and it is not a
            //      line, it is an absence.
            //    Nothing is offered for sitting with either, because no coloured
            //    button has been pressed yet and there is nothing to cash in.
            //
            //  THE HEAVY SCENE, AGAINST THE SEVEN RULES
            //    1 from the side      via a question about a pencil case
            //    2 told badly         out of order, corrected mid-way, stuck on
            //                         an irrelevant detail, minimised, stopped
            //                         in the middle, apologised for
            //    3 method             never stated. Not once, not obliquely,
            //                         not the place, not the means. The whole
            //                         of it is "Guess why." / "Yeah."
            //    4 horror in listener "How long ago?" — "Does anyone here know?"
            //                         — "Good." One practical question too many,
            //                         and a correction a second too late.
            //    5 unresolved         nobody says the right thing, nobody is
            //                         comforted, nobody is better
            //    6 ends elsewhere     on a cup, and on the thing she never eats
            //    7 leaks              Thursday, as an absence. Neither of them
            //                         refers to it, then or ever.
            //
            //  WHAT THE HEAVY SCENE PLANTS
            //    · "dark and full of loud sudden noises", one clause inside the
            //      longest line in the act, nothing after it.          (act 6)
            //    · a pencil case with a zip that has not shut since second year
            //      — his own was established in November, four weeks earlier,
            //      and nothing points at the pair.                     (act 4)
            //    · "seven years, every day" — the friend's whole span.  (act 5)
            //    · "Nobody here knows."                                (act 5)
            //    · the promise she makes and does not keep.            (act 4)
            //
            //  HARU WINS ONE UNIMPORTANT ARGUMENT PER SCENE
            //    the card under the window · four leaves is not zero · an
            //    umbrella is not a sock · once in two months is an accident ·
            //    it only works when it is wet · seasonal is a shape.
            //
            //  AND LOSES ONE REAL DECISION PER SCENE
            //    who reads the board · which stairs · that she takes the can
            //    off the wall · that he does not replace the pencil case · the long
            //    way home · that he comes on Sunday · that he has plans for the
            //    holidays now.
            //
            //  YUA DOES SOMETHING WITH NOTHING TO DO WITH HIM
            //    Tomo leaning · the sock argument · the charity vending machine
            //    · the thing on the saucer she never eats but likes arriving.
            //
            //  SEASON AND CALENDAR
            //    7 Oct warm in the sun, leaves at their reddest, fall 0.20 ·
            //    5 Nov no fall at all — the corridor is indoors and the
            //      corner is after dark with the beds already bare; the leaves
            //      are down, not coming down, and the winter picture of the
            //      corner says exactly that. Dark at half past five ·
            //    18 Dec bare, rain, no fall, windows steamed from inside ·
            //    19 Dec last night's rain fell as snow and stopped; sun in the
            //      morning, none on the north side of the station, and it
            //      starts again over the last three frames of the platform.
            //      The only snow in the act, and nobody mentions it ·
            //    22 Dec indoors. Every date is on screen. No blossom.
            //
            //  WHAT IS DELIBERATELY NOT HERE
            //    No blue or green button and no hint of one before the last
            //    scene names them. No override — that is act three. Nobody
            //    refers to the café scene after it happens. Nobody ever asks
            //    how the other one knows anything.
            // =================================================================
        }
    }
}
