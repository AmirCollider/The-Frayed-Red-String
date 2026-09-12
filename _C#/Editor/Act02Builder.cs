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
            Place(
                Backgrounds.ClassroomDay,
                "1-A", "一年A組", "اول-الف");

            Date(StoryCalendar.ActTwoBegins);

            NoFall(0f);

            Hold(2.0f);

            Narrate(
                "The window still did not shut. Somebody had put a folded card under the frame at some point and given up on it.",
                "窓は相変わらず閉まらない。いつのまにか誰かが厚紙を折って挟んで、そのまま放ってあった。",
                "پنجره هنوز بسته نمی‌شد. یه جایی توی این مدت یکی یه مقوا تا کرده بود و زیرِ قابش گذاشته بود و بعد بی‌خیالش شده بود.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Pout,
                "—and that is still not a reason.",
                "——それ、まだ理由になってない。",
                "—و این هنوزم دلیل نیست.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "It's the same reason as last week.",
                "先週と同じ理由だよ。",
                "همون دلیلِ هفته‌ی پیشه.");

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
                "Let him do it", "ハルぴに任せる", "بذار اون بخونه");

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
            Place(
                Backgrounds.RooftopDay,
                "The roof", "屋上", "پشت‌بام");

            AutumnAir(0.20f);

            Hold(2.0f);

            Narrate(
                "October on the roof was warm in the sun and cold two steps into the shade, and neither of them mentioned which one they had sat in.",
                "十月の屋上は、日なたはあたたかく、日陰に二歩入れば寒い。どちらに座ったかは、どちらも言わなかった。",
                "اکتبرِ روی پشت‌بام تو آفتاب گرم بود و دو قدم اون‌ورتر، تو سایه، سرد — و هیچ‌کدوم نگفتن کدومش نشستن.");

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
                "I brought two of those things you like from the bakery.",
                "パン屋の、ハルぴが好きなやつ二個持ってきた。",
                "دو تا از اون چیزایی که دوست داری از نونوایی آوردم.");

            Say(Speaker.Haru, Portrait.Surprised,
                "The ones with the—",
                "あの、中に——",
                "همونایی که توش—");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The ones with the thing in, yes.",
                "中に入ってるやつ。うん。",
                "همونایی که توش یه چیزی داره، آره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You don't like those.",
                "結愛ぴは好きじゃないでしょ。",
                "تو که اینا رو دوست نداری.");

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

            // The first of the game's two silences. Nothing is asked for and
            // nothing is offered. A player who sits here is counted and is told
            // nothing about it, now or ever.
            Listen(
                "She did not ask. She stood on the step below him and waited, and the stairwell was very quiet, and after a while he came down.",
                "結愛は訊かなかった。一段下で待って、階段はとても静かで、しばらくして、ハルは下りてきた。",
                "نپرسید. یه پله پایین‌تر وایساد و منتظر موند، و راه‌پله خیلی ساکت بود، و بعد از یه مدت اومد پایین.");

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Tuesday, then.",
                "じゃあ火曜。",
                "پس سه‌شنبه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Tuesday.",
                "火曜。",
                "سه‌شنبه.");
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
        //     Background   SchoolCorridorSunset. In November the sunset arrives
        //                  during cleaning, so the corridor is gold at half
        //                  past four instead of six.
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
            Place(
                Backgrounds.CorridorSunset,
                "The second-floor corridor", "二階の廊下", "راهروی طبقه‌ی دوم");

            Date(2024, 11, 5);

            AutumnAir(0.42f);

            Hold(2.0f);

            Narrate(
                "In November the light arrived during cleaning, so the corridor was gold at half past four and dark by five.",
                "十一月は掃除の時間に光が来る。四時半には廊下が金色で、五時には暗い。",
                "تو نوامبر نور موقعِ نظافت می‌رسید، پس راهرو ساعتِ چهار و نیم طلایی بود و پنج، تاریک.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Pout,
                "It is a sock.",
                "あれは靴下。",
                "اون جورابه.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "It is an umbrella.",
                "傘だよ。",
                "چتره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It folds down into a sock. It is a sock.",
                "靴下みたいに畳めるやつでしょ。あれは靴下。",
                "تا می‌شه اندازه‌ی یه جوراب. اون جورابه.");

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
            Cue(SfxId.BoilerRoom, 0.34f);

            Hold(2.4f);

            Narrate(
                "Something under the floor came on, and settled, and kept going.",
                "床の下でなにかが動きだして、落ち着いて、そのまま鳴りつづけた。",
                "یه چیزی زیرِ زمین روشن شد، جا افتاد، و همون‌جور ادامه داد.");

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Neutral,
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

            // She does not ask how he knows. Nobody asks. This is the whole of
            // it, and the player is meant to notice while the characters do not.
            Listen(
                "She did not ask him how he knew. They stood in the corridor while it kept going, and the light went off the lockers one at a time, and neither of them said anything else about it.",
                "どうして知っているのか、結愛は訊かなかった。音が鳴りつづけるあいだ二人は廊下に立っていて、光はロッカーからひとつずつ引いていき、それについては、どちらももう何も言わなかった。",
                "نپرسید از کجا می‌دونه. تو راهرو وایسادن و صدا ادامه داشت، و نور یکی‌یکی از رو کمدها رفت، و هیچ‌کدوم دیگه چیزی دربارش نگفتن.");

            Hold(1.8f);

            Say(Speaker.Haru, Portrait.Neutral,
                "There's a machine down there. It's on a timer.",
                "下に機械があるんだ。タイマーで動いてる。",
                "اون پایین یه دستگاه هست. رو تایمره.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Of course you know what it is.",
                "そりゃ知ってるよね。",
                "معلومه که می‌دونی چیه.");

            Say(Speaker.Haru, Portrait.Sheepish,
                "It's written on the grille.",
                "格子に書いてある。",
                "رو شبکه نوشته.");

            Say(Speaker.Yua, Portrait.Smug,
                "Of course it is.",
                "書いてあるよね。",
                "معلومه که نوشته.");

            Hold(1.6f);

            DecideIdly(
                "Go and look at the grille", "格子を見に行く", "برو شبکه رو نگاه کن",
                "Go down the other stairs", "反対の階段から下りる", "از اون یکی پله‌ها برو پایین");

            Hold(1.4f);

            Narrate(
                "They went down the other stairs. Nobody decided to; it was just the way they went.",
                "二人は反対の階段から下りた。決めたわけではなく、そちらから下りただけだった。",
                "از اون یکی پله‌ها رفتن پایین. کسی تصمیم نگرفت؛ فقط همون‌طوری رفتن.");
        }

        // ---------------------------------------------------------------------
        //  Tuesday, evening — the street with the machine
        //
        //  ▣ Scene state
        //     Background   PastelStreetVendingNight. Dark at half past five in
        //                  November.
        //     Date         Tuesday 5 November, evening.
        //     On stage     The machine, the bicycle, the streetlamps, the beds
        //                  the flowers have gone out of.
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
            Place(
                Backgrounds.VendingStreetNight,
                "The corner", "曲がり角", "سرِ نبش");

            NoFall(2f);

            Hold(2.0f);

            Narrate(
                "It was dark at half past five now. The machine had the street to itself again.",
                "五時半でもう暗い。自販機はまた通りを独り占めしていた。",
                "ساعتِ پنج و نیم دیگه هوا تاریک بود. دستگاه دوباره کلِ خیابون رو واسه خودش داشت.");

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

            Say(Speaker.Yua, Portrait.Bored,
                "Boring.",
                "つまんない。",
                "خسته‌کننده.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Reliable.",
                "信頼できる。",
                "قابل اعتماد.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Boring.",
                "つまんない。",
                "خسته‌کننده.");

            Hold(1.8f);

            Narrate(
                "They sat on the low wall by the beds the flowers had gone out of, and passed the can back and forth, and did not talk for a while, which was comfortable.",
                "花の抜けた花壇のそばの低い塀に座って、缶を回し飲みして、しばらく話さなかった。それは、居心地がよかった。",
                "رو دیوارِ کوتاهِ کنارِ باغچه‌ای که گل‌هاش رفته بودن نشستن، قوطی رو دست‌به‌دست کردن، و یه مدت حرف نزدن، که راحت بود.");

            Hold(2.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "My pencil case doesn't shut.",
                "筆箱、閉まらないんだ。",
                "جامدادیم بسته نمی‌شه.");

            Say(Speaker.Yua, Portrait.Surprised,
                "What?",
                "え?",
                "چی؟");

            Say(Speaker.Haru, Portrait.Sheepish,
                "The zip. It goes about that far and then it stops. I was thinking about the window.",
                "ファスナー。そこまで行って止まる。窓のこと考えてた。",
                "زیپش. تا اینجا می‌ره و وایمیسته. داشتم به پنجره فکر می‌کردم.");

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

            Narrate(
                "He said it the way you say a thing you are not going to do, and she let it go, and the conversation went somewhere else.",
                "やらないことを言うときの言い方だった。結愛はそれ以上つつかず、話はよそへ行った。",
                "همون‌جوری گفتش که آدم چیزی رو می‌گه که قرار نیست انجامش بده، و یوآ ولش کرد، و حرف رفت جای دیگه.");

            Hold(1.8f);

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

            Say(Speaker.Yua, Portrait.Pout,
                "I'm fine.",
                "平気。",
                "من خوبم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You're not fine, you're stubborn, which is a different thing that looks the same from here.",
                "平気じゃなくて、意地張ってる。ここからだと同じに見えるだけ。",
                "خوب نیستی، لجبازی، که چیزِ دیگه‌ایه و از اینجا شبیهِ همونه.");

            Hold(1.4f);

            Narrate(
                "He took his cardigan off and put it on the wall between them, without offering it, so that it was not an offer and could not be refused.",
                "ハルはカーディガンを脱いで、差し出さずに、二人のあいだの塀に置いた。差し出さなければ、断られようがない。",
                "ژاکتشو درآورد و بدونِ اینکه تعارف کنه، گذاشتش رو دیوار، بینِ خودشون — تا تعارف نباشه و نشه ردش کرد.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Neutral,
                "...You'll be cold.",
                "……ハルぴが寒い。",
                "...خودت سردت می‌شه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I'll be cold.",
                "うん、寒い。",
                "آره سردم می‌شه.");

            Hold(2.4f);

            Narrate(
                "She put it on.",
                "結愛はそれを着た。",
                "پوشیدش.");

            Hold(2.0f);

            // ◆ LADDER RUNG FOUR. Out loud, and then not out loud, and they do
            // not match. Every monologue before this one agreed with her face.
            Say(Speaker.Yua, Portrait.Joyful,
                "Thank you.",
                "ありがと。",
                "ممنون.");

            Hold(1.6f);

            InnerVoice(
                "He'll be cold all the way home.",
                "帰るあいだ、ずっと寒い。",
                "تا خونه سردش می‌شه.");

            InnerVoice(
                "He'll be cold all the way home and he won't say anything.",
                "ずっと寒くて、なにも言わない。",
                "تا خونه سردش می‌شه و هیچی نمی‌گه.");

            Hold(1.2f);

            InnerVoice(
                "...Good.",
                "……いい。",
                "...خوبه.");

            Hold(2.2f);

            Say(Speaker.Yua, Portrait.Joyful,
                "Walk faster, then. You're shivering.",
                "じゃあ早く歩いて。震えてる。",
                "پس تندتر راه برو. داری می‌لرزی.");

            Say(Speaker.Haru, Portrait.Joyful,
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
            Place(
                Backgrounds.ClassroomRainy,
                "1-A", "一年A組", "اول-الف");

            Date(2024, 12, 18);

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

            Hold(1.6f);

            DecideIdly(
                "Move Tomo to the other sill", "トモを反対の窓辺に移す", "تومو رو ببر لبِ اون یکی پنجره",
                "Leave Tomo alone", "トモはそのまま", "تومو رو ول کن");

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

            Narrate(
                "She took the table by the door, the way she had every time since September, and nobody had ever remarked on it, including her.",
                "結愛は九月からいつもそうしているように、入口のそばの席をとった。それについては誰も、本人も、言ったことがない。",
                "میزِ کنارِ در رو گرفت، همون‌جوری که از سپتامبر هر بار گرفته بود، و هیچ‌وقت کسی بهش اشاره نکرده بود، از جمله خودش.");

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

            Hold(1.6f);

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

            Say(Speaker.Yua, Portrait.Neutral,
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

            Say(Speaker.Yua, Portrait.Neutral,
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

            Say(Speaker.Yua, Portrait.Neutral,
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
                "یه جامدادی داشت که زیپش بسته نمی‌شد. از کلاس دوم داشتش. مسخره‌ترین چیزِ ممکن بود و حاضر نبود بندازتش.");

            Say(Speaker.Haru, Portrait.StrainedSmile,
                "I don't know why that's the thing I remember.",
                "なんでそれを覚えてるのか、自分でもわからない。",
                "نمی‌دونم چرا همین یکی اونیه که یادم مونده.");

            Hold(2.2f);

            Say(Speaker.Yua, Portrait.Neutral,
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

            // 4 THE HORROR IS IN THE LISTENER. One practical question too many.
            Say(Speaker.Yua, Portrait.Neutral,
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

            // The long line the manual asks every scene for, and the only place
            // in the first four acts where Haru is allowed one. The plant for
            // act six is one clause inside it, with nothing after it.
            Say(Speaker.Haru, Portrait.Sad,
                "If something's wrong. Any time. If something from before is still sitting there. Even if it's small, even if it's old, even if it's my fault for not being strong enough, even if it isn't your fault at all, even if it was dark and full of loud sudden noises — I'll get on the floor, I'll beg, I don't care. Just come and tell me you're not okay. You don't have to do it properly. You don't have to know the words. Just say it and I'll put the whole world down and sit there until you're better. Promise me.",
                "もしなにかあったら。いつでも。前のことがまだ残ってるなら。小さくても、古くても、僕が弱かったせいでも、きみのせいじゃなくても、暗くて大きな音がいきなりする場所のことでも——土下座でもなんでもする。ただ、来て、大丈夫じゃないって言って。うまく言わなくていい。言葉なんてわからなくていい。言ってくれたら、僕は世界ぜんぶ置いて、よくなるまでそこにいる。約束して。",
                "اگه یه چیزی درست نیست. هر وقتی. اگه چیزی از قبل هنوز مونده. حتی اگه کوچیک باشه، حتی اگه قدیمی باشه، حتی اگه تقصیرِ منه که به‌قدرِ کافی قوی نبودم، حتی اگه اصلاً تقصیرِ تو نبوده، حتی اگه تاریک بوده و پر از صداهای بلندِ ناگهانی — می‌افتم به پات، التماس می‌کنم، برام مهم نیست. فقط بیا و بگو حالت خوب نیست. لازم نیست درست بگیش. لازم نیست کلمه‌هاشو بلد باشی. فقط بگو، و من کلِ دنیا رو می‌ذارم زمین و می‌شینم تا حالت خوب بشه. قول بده.");

            Hold(3.0f);

            // 5 NOTHING IS RESOLVED. She does not say the right thing, because
            // there is no right thing, and because she is not trying to.
            Say(Speaker.Yua, Portrait.Neutral,
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
                "It wasn't.",
                "重くない。",
                "نبود.");

            Hold(2.6f);

            // 6 THE SCENE DOES NOT END ON IT. It ends on a cup.
            Narrate(
                "Her cup had gone cold. She had not touched it and the thing on top was still on the saucer where she had put it.",
                "結愛のカップは冷めていた。一度も手をつけず、上に載っていたものは、置いたときのまま受け皿にあった。",
                "فنجونش سرد شده بود. دست نزده بود بهش و اون چیزی که روش بود هنوز همون‌جا تو نعلبکی بود، جایی که گذاشته بودش.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Do you want this? I'm not going to eat it.",
                "これ、いる? 食べないから。",
                "اینو می‌خوای؟ من نمی‌خورمش.");

            Say(Speaker.Haru, Portrait.Neutral,
                "You never eat it.",
                "いつも食べないよね。",
                "هیچ‌وقت نمی‌خوریش.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I like that it comes.",
                "載ってくるのが好きなの。",
                "دوست دارم که میاد.");

            Hold(2.2f);

            Narrate(
                "He ate it. The rain kept going. Somebody at the counter laughed at something, a long way off, in a different evening.",
                "ハルはそれを食べた。雨はつづいていた。カウンターのほうで誰かが笑った。ずっと遠く、べつの夜のことのように。",
                "خوردش. بارون ادامه داشت. یکی دمِ پیشخون به یه چیزی خندید، خیلی دور، تو یه شبِ دیگه.");

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
            Place(
                Backgrounds.ClassroomDay,
                "1-A", "一年A組", "اول-الف");

            Date(2024, 12, 19);

            NoFall(0f);

            Hold(2.2f);

            Narrate(
                "It had stopped in the night. December sun through glass is a lie about temperature and everybody in the room fell for it every year.",
                "夜のうちに雨はやんだ。ガラス越しの十二月の陽射しは気温についての嘘で、毎年みんなそれに騙される。",
                "شب بند اومده بود. آفتابِ دسامبر از پشتِ شیشه دروغیه درباره‌ی دما و هر سال همه‌ی این کلاس بهش گول می‌خورن.");

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

            // Nobody mentions yesterday. Nobody is going to. That is the point,
            // and the player is the only one in the room who is holding it.
            Narrate(
                "Neither of them said anything about the café, then or later, and the day went on being a Thursday.",
                "喫茶店のことは、そのときも、あとでも、どちらも口にしなかった。木曜は木曜のまま進んだ。",
                "هیچ‌کدوم درباره‌ی کافه چیزی نگفتن، نه اون موقع نه بعدش، و روز به پنج‌شنبه بودنش ادامه داد.");

            Hold(1.8f);

            DecideIdly(
                "Ask him again", "もう一度訊く", "دوباره ازش بپرس",
                "Talk about something else", "別の話をする", "درباره‌ی چیزِ دیگه‌ای حرف بزن");

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
        //     Background   TrainPlatformSunset. In December the sunset is at
        //                  half past four and is over before the trains thin
        //                  out.
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
            Place(
                Backgrounds.TrainPlatformSunset,
                "The platform", "ホーム", "سکوی ایستگاه");

            NoFall(0f);

            Hold(2.2f);

            Narrate(
                "The trees along the track had nothing on them at all now, and the light was finished by five past five.",
                "線路沿いの木にはもう何も残っていなくて、光は五時五分には終わっていた。",
                "درخت‌های کنارِ ریل دیگه هیچی روشون نبود، و نور ساعتِ پنج و پنج دقیقه تموم شده بود.");

            Enter(Speaker.Yua, Portrait.Joyful);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Surprised,
                "It's a different shape.",
                "形、ちがう。",
                "شکلش فرق داره.");

            Say(Speaker.Haru, Portrait.Neutral,
                "It's seasonal.",
                "季節のやつだから。",
                "فصلیه.");

            Say(Speaker.Yua, Portrait.Pout,
                "Seasonal isn't a flavour.",
                "「季節」は味じゃない。",
                "«فصلی» که طعم نیست.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "It's a shape. You just said so.",
                "形でしょ。いま自分で言った。",
                "شکله. خودت الآن گفتی.");

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
                "Sunday. Come to mine. My mother's out and the heating actually works.",
                "日曜。うち来て。母いないし、暖房はちゃんと効く。",
                "یکشنبه. بیا خونه‌ی ما. مامانم نیست و شوفاژش واقعاً کار می‌کنه.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(2.2f);

            Narrate(
                "It got properly cold on the platform, the way it does after the light has gone, and he did not put the cardigan back on for a while.",
                "光が消えたあとの、本格的な寒さが来た。ハルはしばらくカーディガンを着直さなかった。",
                "سکو حسابی سرد شد، همون‌جوری که بعد از رفتنِ نور می‌شه، و هارو یه مدت ژاکتشو دوباره نپوشید.");

            Hold(1.8f);

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
            Place(
                Backgrounds.YuaRoomDay,
                "Yua's room", "結愛の部屋", "اتاقِ یوآ");

            Date(2024, 12, 22);

            NoFall(0f);

            Hold(2.4f);

            Narrate(
                "He had come at two and left at six, and the room still had two cups in it.",
                "二時に来て、六時に帰った。部屋にはまだカップが二つ残っていた。",
                "ساعتِ دو اومده بود و شیش رفته بود، و هنوز دو تا فنجون تو اتاق بود.");

            Enter(Speaker.Yua, Portrait.Neutral);

            Hold(2.0f);

            Narrate(
                "The rabbit at the front of the bed has one ear that has gone thin from being held, and it is the left one.",
                "ベッドの前のうさぎは、片方の耳だけ持たれすぎて薄くなっている。左の耳。",
                "خرگوشِ جلوی تخت یه گوشش از بس گرفته شده نازک شده، و همون گوشِ چپه.");

            Hold(2.2f);

            InnerVoice(
                "He said yes to all of it.",
                "ぜんぶ、うんって言った。",
                "به همه‌ش گفت آره.");

            InnerVoice(
                "Two weeks. Every day of it. He didn't even ask what we'd be doing.",
                "二週間。全部の日。なにするかも訊かなかった。",
                "دو هفته. هر روزش. حتی نپرسید چیکار قراره بکنیم.");

            Hold(2.0f);

            // The turn. The room goes quiet behind her and the line comes slower
            // than anything else in the game so far.
            BeginAside();

            Hold(2.6f);

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

            Say(Speaker.Yua, Portrait.Joyful,
                "Thank you.",
                "ありがと。",
                "ممنون.");

            Hold(2.2f);

            EndAside();

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
            //    5 days across 11 weeks · 9 scenes · 232 frames · 4 white
            //    choices · ZERO blue and ZERO green, as the mechanics table
            //    requires. Narration runs about one frame in eight, against act
            //    one's one in fifteen: the act is slower, and slower is made of
            //    fewer subject changes and more atmosphere, not fewer frames.
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
            //    who reads the board · which stairs · that she keeps the
            //    cardigan · that he does not replace the pencil case · the long
            //    way home · that he comes on Sunday · that he has plans for the
            //    holidays now.
            //
            //  YUA DOES SOMETHING WITH NOTHING TO DO WITH HIM
            //    Tomo leaning · the sock argument · the charity vending machine
            //    · the thing on the saucer she never eats but likes arriving.
            //
            //  SEASON AND CALENDAR
            //    7 Oct warm in the sun, leaves at their reddest, fall 0.20 ·
            //    5 Nov leaves heavy, fall 0.42, dark at half five ·
            //    18 Dec bare, rain, no fall, windows steamed from inside ·
            //    19 Dec bare, clear, light gone by five past five ·
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
