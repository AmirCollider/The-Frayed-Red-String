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
//  This is the third act one, and the second rewrite. The first was one day and
//  too short. The second was five days and long enough, and failed for a
//  reason worth writing down at the top of the file so it is not repeated:
//
//      Subtext needs text.
//
//  Every rule the second draft followed was a rule about hiding something —
//  eight words a line, name no feeling, resolve no scene, cut the last line.
//  Applied to the lines whose job was to hide something, they worked. Applied
//  to every line in the act, including the ones whose only job was to say where
//  we are and what is in whose hand, they produced an hour of people gesturing
//  at furniture the player could not see. Thirty-two separate places where a
//  reader could not tell what was being talked about.
//
//  So the governing ratio here is seventy-thirty. Seven frames in ten are
//  ordinary, legible and carry no trick at all: they say who is talking, where
//  they are standing, what they are holding and what just happened. The
//  remaining three are where this game lives. An iceberg only works with a tip
//  above the water.
//
//  The concrete rules that ratio turned into, all of which this file obeys:
//
//    • Scene lock. A line may only point at something visible in the current
//      background, visible in the current sprite, or named in full earlier in
//      this same scene. Nothing is referred to by "it" until it has had a name.
//    • Going somewhere is discussed before the background changes, never after.
//      Nobody stands on the roof discussing whether to go up to the roof.
//    • Every physical action has three beats — intent, act, result. A drink is
//      ordered, arrives, and is drunk, in that order, and nobody comments on a
//      drink that has already been finished.
//    • Numbers match pictures. The lunch is six pieces of sushi and four
//      sausages because that is what the drawing shows; seven go to Haru and
//      three stay with Yua because that is what the next two drawings show.
//    • One trick per scene at most: one answer to a question nobody asked, one
//      frame of silence, one sentence left unfinished. The second draft used
//      the first of those four times in an act, which stops being a technique
//      and becomes a tic.
//    • The narrator speaks once every fifteen frames at the outside, twelve
//      words at the outside, only about things the player cannot see, and in
//      the same spoken Persian the characters use. No similes, no summaries, no
//      confirming that a character was right, no restating an order.
//    • Nobody names the technique. A character who says "you just answered a
//      different question" is the script explaining its own joke.
//
//  Six school days, Monday to Saturday. Twenty-five scenes. The white buttons
//  now have script behind them — each road runs for a few lines of its own
//  before the two rejoin — because a choice the game does not answer teaches
//  the player across an hour that pressing things is pointless, which is the
//  one belief act two's refused blue button cannot afford them to hold.
//
//  Three languages, three performances, two things held constant: every version
//  carries the same information, and every version leaves the power in the same
//  hands. "Move." is an order in English, an order in Persian, and 早く in
//  Japanese — not 行くよ, which invites.
//
//  What the design document is quiet about, and this script keeps quiet:
//  the player is meant to believe these two met this morning. They do not
//  behave like it, they are already "-pi" to one another in the first exchange,
//  and nothing explains why for another five acts.
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
        /// Six school days, Monday to Saturday.
        /// </summary>
        /// <remarks>
        /// Saturday is a half day and they spend it out of school, which is the
        /// only day of the six where the two of them are somewhere neither of
        /// them has to be. It is also where the act ends, and where the last of
        /// the three moments happens.
        /// </remarks>
        protected override void Write()
        {
            WriteMonday();
            WriteTuesday();
            WriteWednesday();
            WriteThursday();
            WriteFriday();
            WriteSaturday();
        }

        // ---------------------------------------------------------------------
        //  Two things this act does often enough to name
        // ---------------------------------------------------------------------

        /// <summary>Characters per second for <see cref="InnerVoice"/> lines.</summary>
        /// <remarks>The normal rate is 45. Slow enough to read as thinking rather than speaking.</remarks>
        private const float InnerMonologueTypeSpeed = 28f;

        /// <summary>Yua thinking, with nobody to hear it.</summary>
        private void InnerVoice(string english, string japanese, string persian)
        {
            Say(Speaker.Yua, Portrait.Unchanged, english, japanese, persian);
            Script[Script.Count - 1].TypeSpeed = InnerMonologueTypeSpeed;
        }

        /// <summary>How long Yua's face is somewhere else.</summary>
        private const float SlipSeconds = 0.2f;

        /// <summary>
        /// Yua's face goes flat and comes back.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Three times in six days, and the three are one subject: the day his
        /// leg hits a desk, the day he says he cannot run, and the day he asks
        /// her permission for something that needed none. The player has no way
        /// to connect them until act six, and nothing in any of the three scenes
        /// remarks on it.
        /// </para>
        /// <para>
        /// It lands on her brightest face rather than her neutral one, because
        /// the recovery is the frightening half. Act two's refusal of a blue
        /// button uses the same expression for the same length of time.
        /// </para>
        /// </remarks>
        private void FaceSlips()
        {
            Enter(Speaker.Yua, Portrait.DeadEyes);
            Hold(SlipSeconds);
            Enter(Speaker.Yua, Portrait.Joyful);
        }

        // =====================================================================
        //  MONDAY — day one of six
        //
        //  ▣ Scene state, carried through all five scenes of the day
        //     Uniform:  school uniform, both of them, all six days. Never
        //               mentioned — there is no other sprite.
        //     Carrying: a school bag each. Yua also has a wrapped lunch box
        //               from the roof scene on.
        //     Class:    1-A, second floor.
        //     Planted:  the third cherry tree from the gate · the window that
        //               will not shut · the desk his knee catches · the juice
        //               machine on the corner · the Usagi café.
        // =====================================================================

        private void WriteMonday()
        {
            WriteMondayPath();
            WriteMondayClassroom();
            WriteMondayRoof();
            WriteMondayCorridor();
            WriteMondayCafe();
        }

        // ---------------------------------------------------------------------
        //  Monday, 8:00 — the school path
        //
        //  ▣ Background CherryBlossomSchoolAlleyDay: paved path, cherry trees
        //    both sides, school building on the right, wooden benches, lanterns
        //    set into the ground. Everything referred to below is one of those.
        //
        //  Yua counts the people at the gate and the ways in. That is not a
        //  quirk and it is not decoration — hypervigilance in a teenager is
        //  cataloguing exits on entering a space, and she does it in four of the
        //  six days without one line of comment from anybody, including her.
        // ---------------------------------------------------------------------

        private void WriteMondayPath()
        {
            Place(
                Backgrounds.SchoolAlleyDay,
                "The path to school", "通学路", "راهِ مدرسه");

            Hold(1.8f);

            Narrate(
                "Today is the first day of high school.",
                "今日は高校の初日だ。",
                "امروز اولین روزِ دبیرستانه.");

            Enter(Speaker.Yua, Portrait.Neutral);

            InnerVoice(
                "Eight in the morning.",
                "朝の八時。",
                "ساعت هشتِ صبحه.");

            InnerVoice(
                "Forty minutes until the bell.",
                "チャイムまで四十分。",
                "تا زنگ چهل دقیقه مونده.");

            InnerVoice(
                "There are seventeen people at the gate.",
                "門のところに十七人いる。",
                "جلوی دروازه هفده نفرن.");

            // Two ways in, counted on the first morning, by a girl nobody has
            // asked to count anything.
            InnerVoice(
                "Two ways in. The big gate, and the side door by the wall.",
                "入り口は二つ。正門と、塀のところの通用口。",
                "دو تا راه هست واسه رفتن تو: دروازه‌ی بزرگ، و درِ کنارِ دیوار.");

            InnerVoice(
                "Coming in from the gate, the third cherry tree is on the right.",
                "門から入って、右側の三本目が桜。",
                "از دروازه که بیای تو، سومین درختِ گیلاس دستِ راسته.");

            InnerVoice(
                "Haru-pi is standing under that tree.",
                "ハルぴは、その木の下に立ってる。",
                "هارو‌پی زیرِ همون درخت وایساده.");

            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Joyful,
                "Haru-pi!",
                "ハルぴ!",
                "هارو‌پی!");

            Say(Speaker.Haru, Portrait.Shy,
                "Morning, Yua-pi.",
                "おはよう、結愛ぴ。",
                "سلام، یوآ‌پی.");

            Say(Speaker.Haru, Portrait.Neutral,
                "You're here early.",
                "早いね。",
                "زود اومدی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You're earlier.",
                "そっちの方が早い。",
                "تو زودتر اومدی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Yeah.",
                "うん。",
                "آره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "What time did you get here?",
                "何時に来たの?",
                "کِی رسیدی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Half past seven.",
                "七時半。",
                "ساعت هفت و نیم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The bell is at twenty to nine.",
                "チャイムは八時四十分。",
                "زنگ ساعت هشت و چهل دقیقه‌ست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I know.",
                "うん、知ってる。",
                "می‌دونم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "So you came an hour and ten minutes early.",
                "一時間十分も早く来たんだ。",
                "یعنی یه ساعت و ده دقیقه زودتر اومدی.");

            Say(Speaker.Haru, Portrait.Shy,
                "Yeah.",
                "うん。",
                "آره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Why?",
                "なんで?",
                "چرا؟");

            // Not a deflection. He genuinely does not know — somebody who has
            // spent his life reading other people's needs is often unable to
            // name one of his own, and this is the first of several places the
            // act shows that plainly.
            Say(Speaker.Haru, Portrait.Unchanged,
                "I don't know.",
                "わかんない。",
                "نمی‌دونم.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "The benches are covered in petals.",
                "ベンチ、花びらだらけだね。",
                "رو نیمکت‌ها پُرِ گلبرگه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "They sweep them in the morning.",
                "朝、掃いてるよ。",
                "صبح‌ها جارو می‌زننشون.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "By noon they'll be covered again.",
                "昼にはまた埋まる。",
                "تا ظهر دوباره پُر می‌شه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "How do you know that?",
                "なんで知ってるの?",
                "از کجا می‌دونی؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "I'm guessing.",
                "勘。",
                "حدس می‌زنم.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Do these ground lanterns come on at night?",
                "この足元の灯籠、夜はつくのかな。",
                "این فانوس‌های کفِ زمین، شبا روشن می‌شن؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I don't know. I've never seen them at night.",
                "知らない。夜に見たことない。",
                "نمی‌دونم. شب ندیدمشون.");

            Cue(SfxId.Petal, 0.5f);

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Our class is on the second floor. Class 1-A.",
                "教室は二階。一年A組。",
                "کلاسمون طبقه‌ی دومه. کلاسِ یک-الف.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I saw the class lists by the door.",
                "入り口のところの名簿、見たよ。",
                "تابلوی اسم‌ها رو دمِ در دیدم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Then let's go up.",
                "じゃあ上がろ。",
                "پس بریم بالا.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Now? There's still forty minutes.",
                "今から? まだ四十分あるけど。",
                "الان؟ هنوز چهل دقیقه وقت داریم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The seats by the window go first.",
                "窓際の席から埋まる。",
                "صندلی‌های کنارِ پنجره زودتر پُر می‌شن.");

            // White button one. The question is legible because the two lines
            // above it are about how much of a hurry there is, and each road
            // runs five lines of its own before they rejoin.
            DecideIdly(
                "Tell him to hurry", "急がせる", "بگو عجله کنه",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Hurry up.",
                        "早く。",
                        "زودتر.");

                    Say(Speaker.Haru, Portrait.Unchanged,
                        "Okay.",
                        "うん。",
                        "باشه.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "Could we go a bit slower?",
                        "もうちょっとゆっくりでもいいかな。",
                        "می‌شه یه‌کم آروم‌تر بریم؟");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "No.",
                        "だめ。",
                        "نه.");

                    Say(Speaker.Haru, Portrait.Unchanged,
                        "Okay.",
                        "うん。",
                        "باشه.");
                },
                "Tell him there's time", "まだ時間あると言う", "بگو وقت هست",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "We've got time.",
                        "時間はある。",
                        "وقت داریم.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "Are you sure?",
                        "ほんとに?",
                        "مطمئنی؟");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Forty minutes.",
                        "四十分。",
                        "چهل دقیقه.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "Then let's take our time.",
                        "じゃあ、ゆっくり行こ。",
                        "پس آروم می‌ریم.");

                    // She asked, she was answered, and she cancels the answer.
                    // Same ending as the other road, arrived at more politely.
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "You take your time. I'm going ahead.",
                        "そっちはゆっくりでいいよ。あたしは先に行く。",
                        "تو آروم برو. من جلوتر می‌رم.");
                });

            Say(Speaker.Haru, Portrait.Unchanged,
                "Wait for me.",
                "待ってよ。",
                "وایسا واسم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Then walk faster.",
                "じゃあ早く歩いて。",
                "پس تندتر راه بیا.");

            Hold(1.4f);
        }

        // ---------------------------------------------------------------------
        //  Monday, 8:15 — class 1-A
        //
        //  ▣ Background SunnyClassroomDay: wooden desks in rows, chalkboard,
        //    pink curtains at an open window, pastel pencil cases on the desks.
        //
        //  Two things are planted here and nothing else is asked of the scene.
        //  The window latch is broken, said plainly and at boring length,
        //  because Tuesday's rain and Friday's repair are both worthless if the
        //  first mention was clever. And his knee catches the underside of a
        //  desk, which is the only time in six days anybody's leg is mentioned
        //  at all.
        // ---------------------------------------------------------------------

        private void WriteMondayClassroom()
        {
            ClearStage();

            Place(
                Backgrounds.ClassroomDay,
                "Class 1-A", "一年A組", "کلاسِ یک-الف");

            Hold(1.8f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            InnerVoice(
                "Thirty-two desks. One door.",
                "机が三十二。ドアは一つ。",
                "سی‌ودو تا میز. یه در.");

            InnerVoice(
                "Four of them are by the window.",
                "そのうち四つが窓際。",
                "چهارتاشون کنارِ پنجره‌ن.");

            Say(Speaker.Haru, Portrait.Neutral,
                "There's nobody here yet.",
                "まだ誰もいないね。",
                "هنوز هیچ‌کس نیومده.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That's why we came up.",
                "だから先に来た。",
                "واسه همین زودتر اومدیم بالا.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Right.",
                "たしかに。",
                "آها.");

            // Motif, stage one. Said flatly and at length on purpose: Tuesday's
            // rain and Friday's repair are both worth nothing if the player has
            // to work out what window anybody is talking about.
            Say(Speaker.Haru, Portrait.Neutral,
                "This window doesn't close all the way.",
                "この窓、ちゃんと閉まらないね。",
                "این پنجره تا آخر بسته نمی‌شه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Why not?",
                "なんで?",
                "چرا؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "The catch is broken.",
                "留め金が壊れてる。",
                "گیره‌ش شکسته.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Since when?",
                "いつから?",
                "از کِی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I don't know. Last year, maybe.",
                "わかんない。去年からかな。",
                "نمی‌دونم. شاید از پارسال.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "So the rain will come in.",
                "じゃあ雨が入るね。",
                "پس بارون میاد تو.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It will.",
                "入ると思う。",
                "میاد.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Leave it.",
                "ほっとこ。",
                "ولش کن.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Two of the window seats are free.",
                "窓際、二つ空いてる。",
                "دوتا از صندلی‌های کنارِ پنجره خالیه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "The front one and the back one.",
                "前のと、後ろの。",
                "جلویی و عقبی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The back one gets the sun all afternoon.",
                "後ろの席は午後ずっと日が当たる。",
                "عقبیه بعدازظهر تمامش آفتاب می‌خوره.");

            // White button two. Legible because the line above says what the
            // difference between the seats is, and the roads differ in who
            // ends up in the sun.
            DecideIdly(
                "Take the back seat yourself", "後ろの席を取る", "عقبیه رو خودت وردار",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "I'll take the back one.",
                        "後ろ、あたしが座る。",
                        "عقبیه رو من می‌شینم.");

                    Say(Speaker.Haru, Portrait.Unchanged,
                        "Then I'm at the front.",
                        "じゃあ僕は前だ。",
                        "پس من جلو می‌شینم.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "You're in front of me.",
                        "あたしの前ね。",
                        "جلوی منی.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "I can't see you from there.",
                        "そこからだと見えないけど。",
                        "از اونجا نمی‌بینمت.");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "I can see you.",
                        "あたしは見える。",
                        "من می‌بینمت.");
                },
                "Give him the back seat", "後ろの席を譲る", "عقبیه رو بده به هارو",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "You take the back one.",
                        "後ろ、座って。",
                        "عقبیه رو تو بشین.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "Are you sure? You said it gets the sun.",
                        "いいの? 日が当たるって言ってたのに。",
                        "مطمئنی؟ خودت گفتی آفتاب می‌خوره.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Sit down, Haru-pi.",
                        "座って、ハルぴ。",
                        "بشین، هارو‌پی.");

                    Say(Speaker.Haru, Portrait.Shy,
                        "Thank you.",
                        "ありがとう。",
                        "ممنون.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Now I'm in front of you.",
                        "これであたしが前ね。",
                        "حالا من جلوی توام.");
                });

            // The one physical event of the scene, in three beats: he goes to
            // sit, the desk catches him, and nothing is said about it.
            Say(Speaker.Yua, Portrait.Unchanged,
                "Sit down.",
                "座って。",
                "بشین.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Cue(SfxId.ChairScrape, 0.7f);

            Narrate(
                "His knee caught the leg of the desk.",
                "膝が机の脚に当たった。",
                "زانوش خورد به پایه‌ی میز.");

            Enter(Speaker.Haru, Portrait.Injured);

            Cue(SfxId.DeskKnock, 0.85f);

            // The one silent frame this scene gets.
            Say(Speaker.Haru, Portrait.Unchanged,
                "…",
                "……",
                "…");

            // Slip one of three. Nothing before it and nothing after it.
            FaceSlips();

            Say(Speaker.Yua, Portrait.Unchanged,
                "Did that hurt?",
                "痛かった?",
                "درد گرفت؟");

            Say(Speaker.Haru, Portrait.Neutral,
                "No.",
                "ううん。",
                "نه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It made a sound.",
                "音したよ。",
                "صداش اومد.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "The desk made the sound.",
                "机が鳴っただけ。",
                "صدای میز بود.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Okay.",
                "そっか。",
                "باشه.");

            Hold(1.4f);

            Narrate(
                "By twenty to nine the other thirty were in the room.",
                "八時四十分には、残り三十人が教室にいた。",
                "تا ساعت هشت و چهل دقیقه، اون سی‌تای دیگه هم اومدن.");

            Cue(SfxId.SchoolBell, 0.6f);

            Hold(1.6f);

            Say(Speaker.Haru, Portrait.Neutral,
                "We have to say our names one by one.",
                "一人ずつ名前を言うんだって。",
                "باید یکی‌یکی اسممون رو بگیم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "In order, down the rows.",
                "列の順番でね。",
                "به ترتیبِ ردیف.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I'm eleventh.",
                "僕は十一番目。",
                "من یازدهمی‌ام.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I'm nineteenth.",
                "あたしは十九番目。",
                "من نوزدهمی‌ام.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You counted already.",
                "もう数えたんだ。",
                "الان شمردیش؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "It took two seconds.",
                "二秒で数えられる。",
                "دو ثانیه طول کشید.");

            Cue(SfxId.ChairScrape, 0.5f);

            Narrate(
                "Eleventh, he stood up a beat after his name.",
                "十一番目。名前のあと、一拍おいて立った。",
                "یازدهمی که شد، یه لحظه بعدِ اسمش بلند شد.");

            Say(Speaker.Yua, Portrait.Neutral,
                "You were slow standing up.",
                "立つの、遅かったね。",
                "دیر بلند شدی.");

            Say(Speaker.Haru, Portrait.Shy,
                "Was I?",
                "そう?",
                "جدی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "A bit.",
                "ちょっとね。",
                "یه‌کم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Sorry.",
                "ごめん。",
                "ببخشید.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "For what?",
                "何が?",
                "واسه چی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I don't know.",
                "わかんない。",
                "نمی‌دونم.");

            Hold(1.8f);
        }

        // ---------------------------------------------------------------------
        //  Monday, 12:20 — up to the roof, and the lunch
        //
        //  Opens in the classroom, because deciding to go somewhere happens
        //  before the picture changes. The numbers are the drawings' numbers:
        //  the box holds six pieces of sushi and four sausages, seven go onto
        //  his lid and three stay in the box, and when the ten pictures of the
        //  meal are over he has four pieces left — which is exactly what the
        //  last drawing of him shows.
        // ---------------------------------------------------------------------

        private void WriteMondayRoof()
        {
            Say(Speaker.Yua, Portrait.Neutral,
                "Where do you eat lunch?",
                "お昼、どこで食べるの?",
                "ناهار رو کجا می‌خوری؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I don't know. In here, I suppose.",
                "わかんない。教室かな。",
                "نمی‌دونم. همین‌جا، فکر کنم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The roof is open.",
                "屋上、開いてるよ。",
                "پشت‌بوم بازه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "How do you know?",
                "なんで知ってるの?",
                "از کجا می‌دونی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I went up this morning and looked.",
                "朝、上まで行って見てきた。",
                "صبح رفتم بالا نگاه کردم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's four floors.",
                "四階分あるけど。",
                "چهار طبقه‌ست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It is.",
                "そうだね。",
                "آره هست.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Alright. Let's go up.",
                "うん。上がろっか。",
                "باشه. بریم بالا.");

            ClearStage();

            Place(
                Backgrounds.RooftopDay,
                "The roof", "屋上", "پشت‌بوم");

            Hold(2.0f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            InnerVoice(
                "One bench. One door.",
                "ベンチが一つ。ドアが一つ。",
                "یه نیمکت. یه در.");

            Say(Speaker.Haru, Portrait.Neutral,
                "You can see the whole town from here.",
                "ここから町が全部見える。",
                "از اینجا کلِ شهر پیداست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You can't hear it, though.",
                "でも音は聞こえない。",
                "ولی صداش نمیاد.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's true.",
                "ほんとだ。",
                "راست می‌گی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "There are plant pots along the fence.",
                "フェンスのところ、植木鉢が並んでる。",
                "کنارِ نرده گلدون چیدن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Somebody waters them.",
                "誰かが水やってるんだね。",
                "یکی بهشون آب می‌ده.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Do you think it's a teacher?",
                "先生かな。",
                "فکر می‌کنی معلمه؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's someone who comes up here at lunch.",
                "お昼にここへ来る人でしょ。",
                "یکیه که ظهرها میاد این بالا.");

            Hold(1.4f);

            // Intent. The box, and what is in it, before anybody opens anything.
            Enter(Speaker.Yua, Portrait.LunchOpen);

            Say(Speaker.Yua, Portrait.Unchanged,
                "I brought lunch for you.",
                "お弁当、持ってきた。",
                "واست ناهار آوردم.");

            Say(Speaker.Haru, Portrait.Neutral,
                "For me?",
                "僕に?",
                "واسه من؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Yes.",
                "うん。",
                "آره.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You didn't know I hadn't brought any.",
                "僕が持ってきてないって、知らなかったよね。",
                "تو که نمی‌دونستی من ناهار نیاوردم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Well, I brought it anyway.",
                "まあ、持ってきちゃった。",
                "خب حالا آوردم دیگه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "There are six pieces of sushi and four sausages in it.",
                "中に寿司が六つと、ソーセージが四つ。",
                "شیش تا سوشی و چهار تا سوسیس توشه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's ten things.",
                "十個か。",
                "می‌شه ده تا.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Seven of them are yours.",
                "そのうち七つがそっちの。",
                "هفت‌تاش مالِ توئه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Seven is too many.",
                "七つは多いって。",
                "هفت تا زیاده.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Four sushi and three sausages. Take them.",
                "寿司四つとソーセージ三つ。取って。",
                "چهار تا سوشی و سه تا سوسیس. وردار.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Then you only have three.",
                "そっち、三つしかなくなる。",
                "پس واسه تو فقط سه تا می‌مونه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Two sushi and one sausage. That's enough.",
                "寿司二つとソーセージ一つ。足りる。",
                "دو تا سوشی و یه سوسیس. کافیه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Are you sure?",
                "ほんとにいいの?",
                "مطمئنی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Hold the lid out.",
                "ふた、出して。",
                "درِ جعبه رو بگیر جلو.");

            // Act. Ten pictures, two seconds each, nothing said over any of them.
            Cel(Portrait.LunchOpen, Portrait.Unchanged);
            Cel(Portrait.LunchOffer);
            Cel(Portrait.LunchShared);
            Cel(Portrait.LunchFirstLift);
            Cel(Portrait.LunchFirstBite);
            Cel(Portrait.LunchSecondLift);
            Cel(Portrait.LunchSecondBite);
            Cel(Portrait.LunchThirdLift);
            Cel(Portrait.LunchThirdBite);
            Cel(Portrait.LunchFinished);

            // Result.
            Enter(Speaker.Haru, Portrait.Joyful);

            Say(Speaker.Haru, Portrait.Unchanged,
                "That was really good.",
                "すごくおいしかった。",
                "خیلی خوشمزه بود.");

            Say(Speaker.Yua, Portrait.Neutral,
                "You didn't finish it.",
                "全部は食べてないよね。",
                "همه‌شو نخوردی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I ate three.",
                "三つ食べた。",
                "سه تا خوردم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "So four are left.",
                "四つ残ってる。",
                "پس چهار تا مونده.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Three sushi and a sausage.",
                "寿司三つとソーセージ一つ。",
                "سه تا سوشی و یه سوسیس.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Why did you stop?",
                "なんでやめたの?",
                "چرا نخوردی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I'm full.",
                "お腹いっぱい。",
                "سیر شدم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Put them in your bag, then.",
                "じゃあ鞄に入れといて。",
                "پس بذارشون تو کیفت.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Okay. Thank you.",
                "うん。ありがとう。",
                "باشه. ممنون.");

            Hold(1.6f);

            InnerVoice(
                "He wasn't full.",
                "お腹いっぱいじゃなかった。",
                "سیر نشده بود.");

            Hold(2.0f);
        }

        // ---------------------------------------------------------------------
        //  Monday, 15:40 — the corridor
        //
        //  ▣ Background SchoolCorridorSunset: empty hallway, low gold light,
        //    long shadows, floor-to-ceiling windows onto the sakura, lockers.
        //
        //  Two things are named in full here and both are somewhere else: the
        //  juice machine on the corner and the café next to it. Naming a thing
        //  before walking to it is the only way the walk means anything, and
        //  the machine in particular has to be boring and complete the first
        //  time — it carries an argument across four of the six days.
        // ---------------------------------------------------------------------

        private void WriteMondayCorridor()
        {
            ClearStage();

            Place(
                Backgrounds.CorridorSunset,
                "The corridor", "廊下", "راهرو");

            Hold(1.8f);

            Narrate(
                "Lessons ended at half past three.",
                "授業は三時半に終わった。",
                "کلاسا ساعت سه و نیم تموم شد.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Neutral,
                "The lockers are all still empty.",
                "ロッカー、まだ全部空だね。",
                "کمدها هنوز همه خالی‌ان.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's the first day.",
                "初日だからね。",
                "روز اوله دیگه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Mine's number forty-one.",
                "僕のは四十一番。",
                "مالِ من شماره چهل‌ویکه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Mine is fifty-two.",
                "あたしは五十二番。",
                "مالِ من پنجاه‌ودو.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "They're not next to each other.",
                "隣じゃないんだ。",
                "کنارِ هم نیستن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "No.",
                "うん。",
                "نه.");

            Hold(1.2f);

            // The machine, named completely, on its first mention. It is pink,
            // it takes juice money, it is on the corner of the way home, and it
            // is broken. Every one of those facts gets used later.
            Say(Speaker.Haru, Portrait.Neutral,
                "There's a juice machine on the corner on the way home.",
                "帰り道の角に、ジュースの自販機があるんだ。",
                "سرِ نبشِ خیابونِ برگشت یه دستگاهِ آب‌میوه هست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The pink one?",
                "ピンクの?",
                "همون صورتیه؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Yes, the pink one.",
                "うん、ピンクの。",
                "آره، همون صورتیه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I've seen it.",
                "見たことある。",
                "دیدمش.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I put a hundred yen in it this morning.",
                "今朝、百円入れたんだ。",
                "صبح صد ین انداختم توش.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "And?",
                "それで?",
                "خب؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Nothing came out.",
                "何も出てこなかった。",
                "هیچی نداد.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "So it's broken.",
                "壊れてるんだ。",
                "پس خرابه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's broken.",
                "壊れてる。",
                "خرابه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Don't put money in it again.",
                "もうお金入れないでね。",
                "دیگه پول نندازش توش.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.4f);

            // The café, named in full before anybody walks to it.
            Say(Speaker.Yua, Portrait.Neutral,
                "There's a café on the same corner. The Usagi.",
                "同じ角にカフェもある。うさぎってお店。",
                "سرِ همون نبش یه کافه هم هست. کافه‌ی اوساگی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I've never seen it.",
                "見たことないな。",
                "من ندیدمش.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You don't walk down that side.",
                "そっち側、通らないでしょ。",
                "تو از اون‌ور رد نمی‌شی.");

            Say(Speaker.Haru, Portrait.Neutral,
                "How do you know which side I walk down?",
                "どっち側通るか、なんで知ってるの?",
                "از کجا می‌دونی من از کدوم‌ور می‌رم؟");

            // The one answer to a question nobody asked that this scene gets.
            Say(Speaker.Yua, Portrait.Unchanged,
                "It shuts at six.",
                "六時に閉まるよ。",
                "ساعت شیش می‌بنده.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's four now.",
                "今、四時だけど。",
                "الان ساعت چهاره.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Then we have two hours. Come on.",
                "じゃあ二時間ある。行こ。",
                "پس دو ساعت وقت داریم. بریم.");

            Hold(1.6f);
        }

        // ---------------------------------------------------------------------
        //  Monday, 16:10 — the corner, and the café on it
        //
        //  ▣ Background UsagiBakeryStreetDay: cobbles, the Usagi shopfront, a
        //    window full of pastries, a wooden bench with cats asleep on it,
        //    flowers in pots.
        //  ▣ Then CozyCafeDay: pink interior, string lights, wooden tables,
        //    counter on the right, a chalkboard above it.
        //
        //  The scene the production book rebuilt as its worked example, built
        //  to that shape: five frames outside before the door, a chalkboard so
        //  the order is about something the player can see, the drinks arriving
        //  as an event of their own, and "finish it" said while there is still
        //  something in the glass.
        // ---------------------------------------------------------------------

        private void WriteMondayCafe()
        {
            ClearStage();

            Place(
                Backgrounds.BakeryStreetDay,
                "The corner", "角の通り", "سرِ نبش");

            Hold(1.8f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Here.",
                "ここ。",
                "همین‌جا.");

            Say(Speaker.Haru, Portrait.Neutral,
                "The one with the rabbit on the sign?",
                "うさぎの看板の?",
                "همونی که خرگوش رو تابلوشه؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That's the Usagi.",
                "それがうさぎ。",
                "همون کافه‌ی اوساگیه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "There are two cats asleep on the bench.",
                "ベンチで猫が二匹寝てる。",
                "دو تا گربه رو نیمکت خوابیدن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "They're there every afternoon.",
                "毎日午後いる。",
                "هر روز بعدازظهر همون‌جان.");

            Say(Speaker.Haru, Portrait.Neutral,
                "You said you'd never been in.",
                "入ったことないって言ってなかった?",
                "گفتی تا حالا نرفتی تو.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I said it shuts at six.",
                "六時に閉まるって言った。",
                "گفتم ساعت شیش می‌بنده.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's not the same thing.",
                "それ、違う話だよ。",
                "این که یه چیزِ دیگه‌ست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Open the door.",
                "ドア開けて。",
                "درو باز کن.");

            ClearStage();

            Place(
                Backgrounds.CafeDay,
                "Inside the Usagi", "うさぎの中", "داخلِ اوساگی");

            Hold(2.0f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Narrate(
                "There was a chalkboard above the counter.",
                "カウンターの上に黒板があった。",
                "بالای پیشخون یه تابلوی گچی بود.");

            Say(Speaker.Haru, Portrait.Neutral,
                "What have they got?",
                "何があるの?",
                "چی دارن؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Bubble tea and matcha. That's the whole board.",
                "タピオカと抹茶。それだけ。",
                "بابل‌تی و ماچا. کلِ تابلو همینه.");

            // White button three, and the one the production book built its
            // worked example around: it is about who does the ordering, which
            // is the only thing in this café that is actually at stake.
            DecideIdly(
                "Let Haru choose", "ハルぴに選ばせる", "بذار هارو انتخاب کنه",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Which one do you want?",
                        "どっちがいい?",
                        "کدومش رو می‌خوای؟");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "What are you having?",
                        "そっちは何にするの?",
                        "تو چی می‌گیری؟");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Bubble tea.",
                        "タピオカ。",
                        "بابل‌تی.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "Then I'll have bubble tea too.",
                        "じゃあ僕もタピオカ。",
                        "پس منم بابل‌تی می‌خورم.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "No. You're having the matcha.",
                        "だめ。そっちは抹茶。",
                        "نه. تو ماچا می‌خوری.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "Then why did you ask me?",
                        "じゃあ、なんで聞いたの?",
                        "پس چرا ازم پرسیدی؟");
                },
                "Order for both of you", "自分で頼む", "خودت سفارش بده",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "One bubble tea and one matcha.",
                        "タピオカ一つと、抹茶一つ。",
                        "یه بابل‌تی، یه ماچا.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "You didn't ask me.",
                        "僕には聞いてないよね。",
                        "از من که نپرسیدی.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "You'd have said whatever I said.",
                        "あたしと同じの頼んでたでしょ。",
                        "هرچی من می‌گفتم رو می‌گفتی.");

                    Say(Speaker.Haru, Portrait.Unchanged,
                        "Probably.",
                        "たぶんね。",
                        "احتمالاً.");
                });

            // The join. It answers his "then why did you ask me" on one road
            // and his "probably" on the other, and it is the same sentence.
            Say(Speaker.Yua, Portrait.Joyful,
                "I wanted to hear it.",
                "聞きたかったから。",
                "دوست داشتم بشنومش.");

            // Running joke, first of four. He offers, she has already paid, and
            // the detail that does the work is when she paid.
            Say(Speaker.Haru, Portrait.Neutral,
                "I'll pay.",
                "僕が払うよ。",
                "من حساب می‌کنم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's paid.",
                "もう払った。",
                "حساب شد.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "When?",
                "いつ?",
                "کِی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "While you were reading the board.",
                "黒板見てる間に。",
                "وقتی داشتی تابلو رو می‌خوندی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Next time it's me.",
                "次は僕ね。",
                "دفعه‌ی بعد نوبتِ منه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Alright.",
                "いいよ。",
                "باشه.");

            // The drinks arriving is an event, not an assumption.
            Narrate(
                "The woman brought the glasses over. The matcha was still steaming.",
                "店員さんがグラスを持ってきた。抹茶はまだ湯気が立っていた。",
                "خانومه لیوونا رو آورد. ماچا هنوز بخار داشت.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Thank you.",
                "ありがとう。",
                "ممنون.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Drink your matcha.",
                "抹茶、飲んで。",
                "ماچات رو بخور.");

            Cel(Portrait.DrinkFull);

            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's good.",
                "おいしい。",
                "خوبه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You don't like matcha.",
                "抹茶、嫌いでしょ。",
                "از ماچا خوشت نمیاد.");

            // The one silent frame of the scene.
            Say(Speaker.Haru, Portrait.Unchanged,
                "…",
                "……",
                "…");

            Cel(Portrait.DrinkReluctant);

            Enter(Speaker.Haru, Portrait.Neutral);

            // Said while there is still something in the glass.
            Say(Speaker.Yua, Portrait.Unchanged,
                "Finish it.",
                "全部飲んで。",
                "تا آخرش رو بخور.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Cel(Portrait.DrinkFinished);

            Hold(1.6f);

            InnerVoice(
                "He didn't leave a drop.",
                "一口も残さなかった。",
                "یه قطره هم نذاشت.");

            Hold(2.2f);
        }

        // =====================================================================
        //  TUESDAY — day two, and it rains
        //
        //  The window pays its first instalment: it was named on Monday as
        //  broken, and today the rain has come through it onto the desks. That
        //  is deliberately not a joke, a reveal or a beat — it is the middle
        //  step of a three-step plant and it is supposed to pass unremarked.
        //
        //  The day also holds the only scene in the act with a third voice in
        //  it. A girl from their class hears Yua call him something and asks
        //  what it was. Persian and English need that scene: ぴ is real Japanese
        //  slang and a Japanese player has already heard everything it means,
        //  where the other two languages have heard a nickname. So the scene
        //  exists in all three and does a different job in Japanese — there,
        //  the classmate is not asking what it means. She knows.
        // =====================================================================

        private void WriteTuesday()
        {
            WriteTuesdayClassroom();
            WriteTuesdayCorridor();
            WriteTuesdayCafe();
            WriteTuesdayWalkHome();
        }

        // ---------------------------------------------------------------------
        //  Tuesday, 8:20 — class 1-A in the rain
        //
        //  ▣ Background OvercastClassroomRainy: same room, grey light, the pink
        //    curtains moving at the open window, desks, chalkboard, a bookshelf.
        // ---------------------------------------------------------------------

        private void WriteTuesdayClassroom()
        {
            ClearStage();

            Place(
                Backgrounds.ClassroomRainy,
                "Class 1-A, Tuesday", "火曜日の一年A組", "کلاسِ یک-الف، سه‌شنبه");

            Hold(2.0f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            // Motif, stage two. Flat, unemphasised, and over in four frames.
            Say(Speaker.Haru, Portrait.Neutral,
                "The rain came in through the window.",
                "窓から雨が入ってる。",
                "بارون از پنجره اومده تو.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Which desks are wet?",
                "どの机が濡れてる?",
                "کدوم میزا خیس شدن؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "The two by the window. Ours.",
                "窓際の二つ。僕らの。",
                "همون دوتای کنارِ پنجره. مالِ ما.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Of course they are.",
                "そうだろうね。",
                "معلومه دیگه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Mine's worse than yours.",
                "僕のほうがひどい。",
                "مالِ من از مالِ تو بدتره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Show me.",
                "見せて。",
                "نشونم بده.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "The whole top of it.",
                "上、全部。",
                "کلِ روش.");

            // Intent, act, result — and the act is narrated, because there is
            // no sprite of a girl moving a bag.
            Say(Speaker.Yua, Portrait.Unchanged,
                "Give me your bag.",
                "鞄、貸して。",
                "کیفت رو بده من.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Why?",
                "なんで?",
                "چرا؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Give it to me.",
                "いいから貸して。",
                "بده دیگه.");

            Narrate(
                "She put his bag on her desk and sat down at his.",
                "彼女は鞄を自分の机に置いて、彼の席に座った。",
                "کیفشو گذاشت رو میزِ خودش و نشست سرِ میزِ اون.");

            Cue(SfxId.ChairScrape, 0.6f);

            Say(Speaker.Haru, Portrait.Neutral,
                "That one's the wet one.",
                "そっち、濡れてるほうだよ。",
                "اون که خیسه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I know which one it is.",
                "知ってて座ってる。",
                "می‌دونم کدومه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You'll get wet.",
                "濡れちゃうよ。",
                "خیس می‌شی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Sit down, Haru-pi.",
                "座って、ハルぴ。",
                "بشین، هارو‌پی.");

            Say(Speaker.Haru, Portrait.Shy,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "You brought two umbrellas.",
                "傘、二本持ってきてる。",
                "دو تا چتر آوردی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "One of them is yours.",
                "一本はそっちの。",
                "یکیش مالِ توئه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "How did you know it would rain?",
                "降るって、どうしてわかったの?",
                "از کجا می‌دونستی بارون میاد؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It said so on the news last night.",
                "昨日の夜、ニュースで言ってた。",
                "دیشب تو اخبار گفتن.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I don't watch the news.",
                "ニュース見ないんだよね。",
                "من اخبار نمی‌بینم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I know. That's why I brought two.",
                "知ってる。だから二本持ってきた。",
                "می‌دونم. واسه همین دو تا آوردم.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "The rain's running down the glass in lines.",
                "雨が線みたいにガラスを流れてる。",
                "بارون رو شیشه خط‌خطی می‌ره پایین.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "There are nine of them.",
                "九本ある。",
                "نُه تاشون هست.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You counted the rain.",
                "雨を数えたんだ。",
                "بارون رو شمردی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It took a second.",
                "一秒で済む。",
                "یه ثانیه طول کشید.");

            // White button four. Legible because the umbrellas were established
            // eight frames ago, and it decides who ends up wet.
            DecideIdly(
                "Walk him to his classroom door", "教室まで送る", "تا درِ کلاس ببرش",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "I'll walk you to the door at lunch.",
                        "お昼、ドアまで送る。",
                        "ظهر تا درِ کلاس می‌برمت.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "It's four metres away.",
                        "四メートルしかないよ。",
                        "چهار متر بیشتر نیست.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Then it won't take long.",
                        "じゃあすぐ終わる。",
                        "پس طول نمی‌کشه.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "Alright.",
                        "うん。",
                        "باشه.");
                },
                "Give him the second umbrella now", "今、傘を渡す", "همین الان چترِ دومی رو بده بهش",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Take the second umbrella now.",
                        "二本目、今持ってて。",
                        "چترِ دومی رو همین الان وردار.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "We're going home together, though.",
                        "帰り、一緒でしょ?",
                        "ولی که باهم برمی‌گردیم.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Take it anyway.",
                        "いいから持ってて。",
                        "بازم وردارش.");

                    Say(Speaker.Haru, Portrait.Shy,
                        "Okay. Thank you.",
                        "うん。ありがとう。",
                        "باشه. ممنون.");
                });

            Hold(1.8f);
        }

        // ---------------------------------------------------------------------
        //  Tuesday, 15:45 — outside the staff room
        //
        //  ▣ Background SchoolCorridorSunset: the light is wrong for a rainy
        //    day and that is a real limitation of the art; nothing in the
        //    dialogue mentions the light, which is the honest way to spend a
        //    background you do not have.
        //
        //  The only scene in act one with a third person in it. Six frames of
        //  her, no sprite, no name — the plate says what she is. What she does
        //  is notice a word, and be told it is not hers.
        // ---------------------------------------------------------------------

        private void WriteTuesdayCorridor()
        {
            ClearStage();

            Place(
                Backgrounds.CorridorSunset,
                "Outside the staff room", "職員室の前", "جلوی دفتر");

            Hold(1.8f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Hold my bag a second.",
                "ちょっと鞄持ってて。",
                "یه لحظه کیفم رو بگیر.");

            Say(Speaker.Haru, Portrait.Neutral,
                "What are you doing?",
                "何するの?",
                "چیکار می‌کنی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Getting the club form from the board.",
                "掲示板から部活の紙もらってくる。",
                "برگه‌ی باشگاه رو از تابلو ورمی‌دارم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Which club?",
                "何部?",
                "کدوم باشگاه؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "Hold the bag, Haru-pi.",
                "鞄、持ってて、ハルぴ。",
                "کیف رو بگیر، هارو‌پی.");

            Say(Speaker.Haru, Portrait.Shy,
                "Okay.",
                "うん。",
                "باشه.");

            Narrate(
                "A girl from their class stopped next to them.",
                "同じクラスの子が、横で足を止めた。",
                "یکی از بچه‌های کلاسشون کنارشون وایساد.");

            // Persian and English: she does not know what she heard.
            // Japanese: she knows exactly, and is asking whether Yua means it.
            Say(Speaker.Classmate, Portrait.Unchanged,
                "What did you just call him?",
                "ねえ今、ぴって言った?",
                "الان چی صداش کردی؟");

            Say(Speaker.Yua, Portrait.Neutral,
                "Nothing.",
                "別に。",
                "هیچی.");

            Say(Speaker.Classmate, Portrait.Unchanged,
                "You said Haru-pi. What does that mean?",
                "好きぴってこと? そういう意味?",
                "گفتی هارو‌پی. یعنی چی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's ours.",
                "うちらの。",
                "مالِ خودمونه.");

            Say(Speaker.Classmate, Portrait.Unchanged,
                "Right.",
                "ふーん。",
                "آها.");

            Narrate(
                "She went into the staff room.",
                "彼女は職員室に入っていった。",
                "رفت تو دفتر.");

            Hold(1.6f);

            // The one silent frame of the scene.
            Say(Speaker.Haru, Portrait.Unchanged,
                "…",
                "……",
                "…");

            Say(Speaker.Yua, Portrait.Unchanged,
                "What?",
                "何?",
                "چیه؟");

            // Her word, in his mouth, on day two.
            Say(Speaker.Haru, Portrait.Neutral,
                "Nothing.",
                "別に。",
                "هیچی.");

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "You can give me my bag back.",
                "鞄、返していいよ。",
                "کیفم رو می‌تونی پس بدی.");

            Say(Speaker.Haru, Portrait.Shy,
                "Oh. Here.",
                "あ。はい。",
                "آها. بیا.");

            Say(Speaker.Yua, Portrait.Neutral,
                "You were holding it with both hands.",
                "両手で持ってた。",
                "با هر دو دست گرفته بودیش.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's your bag.",
                "結愛ぴの鞄だし。",
                "کیفِ توئه دیگه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It's a bag.",
                "ただの鞄。",
                "یه کیفه.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Which club was the form for?",
                "結局、何部の紙?",
                "بالاخره برگه‌ی کدوم باشگاه بود؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I didn't take one.",
                "取らなかった。",
                "ورنداشتم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You went all the way over there.",
                "わざわざ行ったのに。",
                "تا اون‌ور رفتی که.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I read the list and came back.",
                "一覧見て、戻ってきた。",
                "لیست رو خوندم و برگشتم.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Was there nothing you liked?",
                "いいのなかった?",
                "چیزِ خوبی نبود؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "They all meet after school.",
                "全部、放課後にやってる。",
                "همه‌شون بعد از مدرسه‌ن.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "So?",
                "それが?",
                "خب؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "So I'd be there and not here.",
                "そしたら、ここにいられない。",
                "خب اون‌وقت اونجا بودم، نه اینجا.");

            Hold(2.0f);
        }

        // ---------------------------------------------------------------------
        //  Tuesday, 16:20 — the Usagi, with the lights on at four
        //
        //  ▣ Background CozyCafeDimRainy: same room as Monday under grey light,
        //    rain on the big windows, warm lamps on inside, a matcha cake and a
        //    dango set on the table in front.
        // ---------------------------------------------------------------------

        private void WriteTuesdayCafe()
        {
            ClearStage();

            Place(
                Backgrounds.CafeRainy,
                "The Usagi, in the rain", "雨のうさぎ", "کافه‌ی اوساگی، زیر بارون");

            Hold(2.0f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Neutral,
                "They've turned the lamps on already.",
                "もう電気ついてる。",
                "چراغا رو الان روشن کردن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's dark outside.",
                "外が暗いからね。",
                "بیرون تاریکه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It's twenty past four.",
                "まだ四時二十分だよ。",
                "ساعت چهار و بیست دقیقه‌ست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "And it's dark outside.",
                "それでも暗い。",
                "بازم بیرون تاریکه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "There's cake on the board today.",
                "今日は黒板にケーキがある。",
                "امروز رو تابلو کیک هم هست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Matcha cake and a dango set.",
                "抹茶のケーキと、だんごのセット。",
                "کیکِ ماچا و یه ستِ دانگو.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You've read it already.",
                "もう読んだんだ。",
                "الان خوندیش؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "I read it from the door.",
                "ドアのところから読んだ。",
                "از دمِ در خوندمش.");

            // White button five. Two ways to spend the same hundred yen, and
            // the roads differ in who ends up eating.
            DecideIdly(
                "Order the cake", "ケーキを頼む", "کیک سفارش بده",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "One matcha cake, two forks.",
                        "抹茶ケーキ一つ、フォーク二本。",
                        "یه کیکِ ماچا، دو تا چنگال.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "Two forks for one cake?",
                        "一つに二本?",
                        "دو تا چنگال واسه یه کیک؟");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Yes.",
                        "うん。",
                        "آره.");

                    Say(Speaker.Haru, Portrait.Shy,
                        "Alright.",
                        "うん、わかった。",
                        "باشه.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "You take the side with the cream.",
                        "クリームのほう、そっちね。",
                        "سمتی که خامه داره مالِ توئه.");
                },
                "Order the dango", "だんごを頼む", "دانگو سفارش بده",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "One dango set.",
                        "だんごセット一つ。",
                        "یه ستِ دانگو.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "How many are in a set?",
                        "セットって何本?",
                        "تو یه ست چند تاست؟");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Three sticks. Three on each.",
                        "三本。一本に三つずつ。",
                        "سه تا سیخ. رو هر کدوم سه تا.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "So nine.",
                        "じゃあ九つ。",
                        "پس نُه تا.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Six are yours.",
                        "六つがそっちの。",
                        "شیش‌تاش مالِ توئه.");
                });

            // Running joke, second of four. Same shape, and she has moved the
            // moment earlier: on Monday she paid while he read the board.
            Say(Speaker.Haru, Portrait.Neutral,
                "Today it's mine. I said so yesterday.",
                "今日は僕の番。昨日そう言った。",
                "امروز نوبتِ منه. دیروز گفتم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's paid.",
                "もう払った。",
                "حساب شد.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "We only just came in.",
                "今入ったばっかりだよ。",
                "همین الان اومدیم تو.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You were shaking out your umbrella.",
                "傘、たたんでたでしょ。",
                "داشتی چترت رو تکون می‌دادی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That took four seconds.",
                "四秒だよ。",
                "چهار ثانیه طول کشید.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It was enough.",
                "十分だった。",
                "کافی بود.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Tomorrow, then.",
                "じゃあ明日。",
                "پس فردا.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Tomorrow.",
                "明日ね。",
                "فردا.");

            Hold(1.6f);

            Say(Speaker.Haru, Portrait.Neutral,
                "The rain's stopped hitting the window.",
                "窓に当たる音、やんだね。",
                "صدای بارون رو شیشه بند اومد.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's still raining. The wind changed.",
                "まだ降ってる。風が変わっただけ。",
                "هنوز می‌باره. بادش عوض شد.");

            Say(Speaker.Haru, Portrait.Joyful,
                "How do you know that?",
                "なんでわかるの?",
                "از کجا می‌دونی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The hydrangeas outside are leaning the other way.",
                "外のあじさい、逆に傾いてる。",
                "گل‌های بیرون به سمتِ دیگه خم شدن.");

            Say(Speaker.Haru, Portrait.Neutral,
                "You were looking at the flowers.",
                "花、見てたんだ。",
                "داشتی به گلا نگاه می‌کردی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I was looking out of the window.",
                "外を見てただけ。",
                "داشتم بیرون رو نگاه می‌کردم.");

            Hold(2.0f);
        }

        // ---------------------------------------------------------------------
        //  Tuesday, 18:00 — the long way home
        //
        //  ▣ Background TraditionalAlleywayNight: paved alley, crescent moon, a
        //    paper lantern lit at the entrance, lit windows above, hydrangeas in
        //    pots along the wall.
        //
        //  The long way is named plainly, measured out loud, and given a reason
        //  that is entirely true and not the reason.
        // ---------------------------------------------------------------------

        private void WriteTuesdayWalkHome()
        {
            ClearStage();

            Place(
                Backgrounds.AlleywayNight,
                "The old alley", "古い路地", "کوچه‌ی قدیمی");

            Hold(2.0f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Neutral,
                "This isn't the way to the station.",
                "これ、駅の方じゃないよね。",
                "این که راهِ ایستگاه نیست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "No, it isn't.",
                "うん、違う。",
                "نه، نیست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "How much longer is it?",
                "どれくらい遠回り?",
                "چقدر بیشتر طول می‌کشه؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Twenty-two minutes more than the short way.",
                "近道より二十二分長い。",
                "بیست و دو دقیقه بیشتر از راهِ کوتاه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You've timed it.",
                "測ったんだ。",
                "وقت گرفتی ازش؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Yes.",
                "うん。",
                "آره.");

            Say(Speaker.Haru, Portrait.Neutral,
                "So why are we going this way?",
                "じゃあ、なんでこっち?",
                "پس چرا از این‌ور می‌ریم؟");

            // True, and not the reason.
            Say(Speaker.Yua, Portrait.Unchanged,
                "The lanterns are lit.",
                "提灯がついてるから。",
                "چون فانوسا روشنن.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's a good reason.",
                "いい理由だ。",
                "دلیلِ خوبیه.");

            Hold(1.6f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Someone waters these hydrangeas too.",
                "このあじさいも、誰かが世話してる。",
                "این گل‌ها رو هم یکی آب می‌ده.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "In the evening.",
                "夕方にね。",
                "عصرها.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Not in the morning?",
                "朝じゃなくて?",
                "صبح نه؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You water them in the evening.",
                "水は夕方にやるの。",
                "به گل عصر باید آب داد.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Who told you that?",
                "誰に教わったの?",
                "کی بهت گفته؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Nobody.",
                "別に。",
                "هیچ‌کس.");

            Hold(1.4f);

            // Apologising for nothing, second of three. He has not done
            // anything, and he can hear that something went past him.
            Say(Speaker.Haru, Portrait.Unchanged,
                "Sorry.",
                "……ごめん。",
                "ببخشید.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "For what?",
                "何が?",
                "واسه چی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I don't know.",
                "わかんない。",
                "نمی‌دونم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Then don't say it.",
                "じゃあ言わないで。",
                "پس نگو.");

            Say(Speaker.Haru, Portrait.Shy,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.8f);

            Say(Speaker.Haru, Portrait.Neutral,
                "My street's at the end of this one.",
                "この先が僕んちの通り。",
                "کوچه‌ی ما تهِ همینه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I know.",
                "知ってる。",
                "می‌دونم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I've never told you where I live.",
                "住んでるとこ、言ったことないけど。",
                "من که نگفته بودم کجا زندگی می‌کنم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Then walk faster and show me.",
                "じゃあ早く歩いて、案内して。",
                "پس تندتر راه برو و نشونم بده.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Alright.",
                "うん。",
                "باشه.");

            Hold(1.4f);

            InnerVoice(
                "Twenty-two minutes.",
                "二十二分。",
                "بیست و دو دقیقه.");

            InnerVoice(
                "He didn't ask again.",
                "もう聞かなかった。",
                "دیگه نپرسید.");

            Hold(2.4f);
        }

        // =====================================================================
        //  WEDNESDAY — day three
        //
        //  The day she is told there is something he cannot do. He does not say
        //  why, she does not ask why, and eleven frames later she tells him to
        //  walk faster. Nothing in the scene marks any of that as strange, and
        //  nothing is allowed to: the scene that marks it is act six.
        //
        //  It also holds the vending machine in full — three minutes about
        //  nothing, which is the shape of scene the production book asks for by
        //  name and the shape the second draft had almost none of.
        // =====================================================================

        private void WriteWednesday()
        {
            WriteWednesdayPath();
            WriteWednesdayClassroom();
            WriteWednesdayRoof();
            WriteWednesdayMachine();
        }

        // ---------------------------------------------------------------------
        //  Wednesday, 8:05 — the school path
        // ---------------------------------------------------------------------

        private void WriteWednesdayPath()
        {
            ClearStage();

            Place(
                Backgrounds.SchoolAlleyDay,
                "The path to school, Wednesday", "水曜日の通学路", "راهِ مدرسه، چهارشنبه");

            Hold(1.8f);

            Enter(Speaker.Haru, Portrait.Neutral);
            Enter(Speaker.Yua, Portrait.Joyful);

            Say(Speaker.Yua, Portrait.Unchanged,
                "You're under the third tree again.",
                "また三本目の木の下だ。",
                "بازم زیرِ درختِ سومی وایسادی.");

            Say(Speaker.Haru, Portrait.Neutral,
                "It's the one with the bench under it.",
                "ベンチがある木だから。",
                "همونیه که نیمکت زیرشه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "There are benches under four of them.",
                "ベンチのある木、四本あるよ。",
                "زیرِ چهارتاشون نیمکت هست.");

            Say(Speaker.Haru, Portrait.Shy,
                "This one's nearest the gate.",
                "ここが門に一番近い。",
                "این یکی به دروازه نزدیک‌تره.");

            Say(Speaker.Yua, Portrait.Joyful,
                "That's a better answer.",
                "そっちのほうが本当っぽい。",
                "این جوابِ بهتریه.");

            InnerVoice(
                "Nineteen people at the gate today.",
                "今日は門に十九人。",
                "امروز نوزده نفر جلوی دروازه‌ن.");

            InnerVoice(
                "Still two ways in.",
                "入り口はやっぱり二つ。",
                "هنوز دو تا راه هست.");

            Say(Speaker.Haru, Portrait.Neutral,
                "It's ten past eight.",
                "八時十分だ。",
                "ساعت هشت و ده دقیقه‌ست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Thirty minutes until the bell.",
                "チャイムまで三十分。",
                "تا زنگ سی دقیقه مونده.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You always know without looking.",
                "見なくてもわかるんだね。",
                "همیشه بی‌اینکه نگاه کنی می‌دونی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "There's a clock on the gate.",
                "門に時計がある。",
                "رو دروازه ساعت هست.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Oh. So there is.",
                "あ。ほんとだ。",
                "آها. راست می‌گی.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Joyful,
                "Race me to the gate.",
                "門まで競争。",
                "تا دروازه مسابقه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "I can't run.",
                "僕、走れない。",
                "من نمی‌تونم بدوم.");

            // Slip two of three. Nothing before it, nothing after it, and the
            // next line is about something else.
            FaceSlips();

            Say(Speaker.Yua, Portrait.Unchanged,
                "Since when?",
                "いつから?",
                "از کِی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I just can't.",
                "なんか、無理なんだ。",
                "همین‌جوری نمی‌تونم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Alright.",
                "そっか。",
                "باشه.");

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Joyful,
                "Then walk faster.",
                "じゃあ、もっと速く歩いて。",
                "پس تندتر راه برو.");

            Say(Speaker.Haru, Portrait.Joyful,
                "How fast?",
                "どれくらい?",
                "چقدر تند؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Faster than me.",
                "あたしより速く。",
                "تندتر از من.");

            Say(Speaker.Haru, Portrait.Neutral,
                "That's the same as a race.",
                "それ、競争と同じだよ。",
                "این که همون مسابقه‌ست.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It's walking.",
                "歩いてるでしょ。",
                "داری راه می‌ری دیگه.");

            // White button six. Legible because he has just said he cannot run
            // and she has told him to hurry anyway.
            DecideIdly(
                "Go on ahead", "先に行く", "جلوتر برو",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "I'll go ahead and hold the door.",
                        "先に行って、ドア押さえてる。",
                        "من جلوتر می‌رم و درو نگه می‌دارم.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "You don't have to.",
                        "そこまでしなくていいよ。",
                        "لازم نیست.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "It's on my way.",
                        "通り道だし。",
                        "سرِ راهمه.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "It's a door.",
                        "ただのドアだよ。",
                        "یه دره فقط.");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "Then it won't be heavy.",
                        "じゃあ重くない。",
                        "پس سنگین نیست.");
                },
                "Stay level with him", "横を歩く", "کنارش راه برو",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "I'll walk next to you.",
                        "横、歩く。",
                        "کنارت راه می‌رم.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "You'll be slower than usual.",
                        "いつもより遅くなるよ。",
                        "از همیشه آروم‌تر می‌شی.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Then be quicker than usual.",
                        "じゃあ、いつもより速く。",
                        "پس تو تندتر از همیشه راه بیا.");

                    Say(Speaker.Haru, Portrait.Shy,
                        "I'll try.",
                        "がんばる。",
                        "سعی می‌کنم.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "You will.",
                        "できるよ。",
                        "می‌تونی.");
                });

            Cue(SfxId.Petal, 0.45f);

            Hold(1.8f);
        }

        // ---------------------------------------------------------------------
        //  Wednesday, 10:30 — class 1-A
        //
        //  Nothing happens in this scene and nothing is meant to. It is here so
        //  that Friday costs something.
        // ---------------------------------------------------------------------

        private void WriteWednesdayClassroom()
        {
            ClearStage();

            Place(
                Backgrounds.ClassroomDay,
                "Class 1-A, Wednesday", "水曜日の一年A組", "کلاسِ یک-الف، چهارشنبه");

            Hold(1.8f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Narrate(
                "The seating chart went up on the board before second period.",
                "二限の前に、席替えの表が黒板に貼られた。",
                "قبل از زنگ دوم، جدولِ صندلی‌ها رفت رو تخته.");

            Say(Speaker.Haru, Portrait.Neutral,
                "We're where we already were.",
                "今の席のままだ。",
                "همون‌جایی‌ایم که بودیم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Yes.",
                "うん。",
                "آره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's lucky.",
                "運がいいね。",
                "شانس آوردیم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Very lucky.",
                "すごく運がいい。",
                "خیلی شانس آوردیم.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Your handwriting is terrible.",
                "字、下手すぎ。",
                "خطت خیلی بده.");

            Say(Speaker.Haru, Portrait.Shy,
                "It's fast, though.",
                "でも速いよ。",
                "ولی تنده.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's terrible and it's fast.",
                "下手で、速い。",
                "هم بده، هم تنده.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's two things.",
                "二つ言われた。",
                "دو تا چیز گفتی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Give me your notebook.",
                "ノート、貸して。",
                "دفترت رو بده من.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Why?",
                "なんで?",
                "چرا؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I'll copy the board into it.",
                "黒板、写してあげる。",
                "تخته رو واست توش می‌نویسم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Then it'll be your handwriting.",
                "結愛ぴの字になっちゃう。",
                "اون‌وقت خطِ تو می‌شه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It'll be readable.",
                "読める字になる。",
                "خونا می‌شه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "The teacher will notice.",
                "先生、気づくんじゃないかな。",
                "معلم می‌فهمه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "He collects them on Fridays.",
                "回収は金曜だよ。",
                "جمعه‌ها جمعشون می‌کنه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "You know his collection day.",
                "回収日、知ってるんだ。",
                "روزِ جمع کردنش رو می‌دونی؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "It's written on the board.",
                "黒板に書いてある。",
                "رو تخته نوشته.");

            Say(Speaker.Haru, Portrait.Joyful,
                "So it is.",
                "ほんとだ。",
                "آها، راست می‌گی.");

            Narrate(
                "She copied the whole board into his notebook in eleven minutes.",
                "彼女は十一分で黒板を全部、彼のノートに写した。",
                "تو یازده دقیقه کلِ تخته رو تو دفترش نوشت.");

            Say(Speaker.Haru, Portrait.Neutral,
                "You didn't stop once.",
                "一回も止まらなかったね。",
                "یه بار هم وایسا نکردی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "There wasn't much on it.",
                "そんなに書いてなかった。",
                "چیزِ زیادی روش نبود.");

            Say(Speaker.Haru, Portrait.Joyful,
                "There were four blackboards.",
                "四回書き換えてたよ。",
                "چهار تا تخته پُر شد.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Then there wasn't much on each one.",
                "じゃあ一枚ずつは少なかった。",
                "پس رو هر کدوم چیزِ زیادی نبود.");

            Hold(1.6f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Can I ask you something?",
                "ひとつ聞いてもいい?",
                "می‌تونم یه چیزی بپرسم؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Ask.",
                "どうぞ。",
                "بپرس.");

            Say(Speaker.Haru, Portrait.Shy,
                "Never mind. I forgot it.",
                "やっぱいいや。忘れちゃった。",
                "بی‌خیال. یادم رفت.");

            Say(Speaker.Yua, Portrait.Neutral,
                "You didn't forget it.",
                "忘れてないでしょ。",
                "یادت نرفت.");

            // The one silent frame of the scene.
            Say(Speaker.Haru, Portrait.Unchanged,
                "…",
                "……",
                "…");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Ask me on Friday.",
                "金曜に聞いて。",
                "جمعه ازم بپرس.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(2.0f);
        }

        // ---------------------------------------------------------------------
        //  Wednesday, 12:25 — the roof again
        //
        //  The four pieces he put in his bag on Monday come back out, which is
        //  the only reason this scene has a lunch in it. No numbers are claimed
        //  that the pictures do not show: the ten-picture sequence belongs to
        //  Monday and is not replayed here, so nothing here counts anything.
        // ---------------------------------------------------------------------

        private void WriteWednesdayRoof()
        {
            Say(Speaker.Yua, Portrait.Neutral,
                "Roof again?",
                "また屋上でいい?",
                "بازم پشت‌بوم؟");

            Say(Speaker.Haru, Portrait.Joyful,
                "Roof again.",
                "うん、屋上。",
                "آره، پشت‌بوم.");

            ClearStage();

            Place(
                Backgrounds.RooftopDay,
                "The roof, Wednesday", "水曜日の屋上", "پشت‌بوم، چهارشنبه");

            Hold(2.0f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Neutral,
                "I brought Monday's back.",
                "月曜の残り、持ってきた。",
                "مالِ دوشنبه رو آوردم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You kept them two days?",
                "二日、取っといたの?",
                "دو روز نگهشون داشتی؟");

            Say(Speaker.Haru, Portrait.Shy,
                "They were in the fridge.",
                "冷蔵庫に入れてた。",
                "تو یخچال بودن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Eat them today.",
                "今日、食べて。",
                "امروز بخورشون.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's why I brought them.",
                "そのために持ってきた。",
                "واسه همین آوردمشون.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "The clouds aren't moving today.",
                "今日、雲が動いてない。",
                "امروز ابرا تکون نمی‌خورن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "They are. Slowly.",
                "動いてるよ。ゆっくり。",
                "می‌خورن. آروم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That one's a rabbit.",
                "あれ、うさぎ。",
                "اون یکی خرگوشه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That one's a cloud.",
                "あれは雲。",
                "اون یکی ابره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It has ears.",
                "耳あるよ。",
                "گوش داره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It has two bits sticking up.",
                "上に出っぱりが二つあるだけ。",
                "دو تا چیز از بالاش زده بیرون.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Ears.",
                "耳。",
                "گوش.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Fine. A rabbit.",
                "はいはい。うさぎ。",
                "باشه. خرگوش.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Thank you.",
                "ありがとう。",
                "ممنون.");

            Narrate(
                "They did that with four more clouds.",
                "そのあと、雲を四つ分やった。",
                "چهار تا ابرِ دیگه هم همین کارو کردن.");

            Hold(1.6f);

            // White button seven.
            DecideIdly(
                "Ask what he did at the weekend", "週末の話を聞く", "بپرس آخر هفته چیکار کرد",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "What did you do at the weekend?",
                        "週末、何してた?",
                        "آخر هفته چیکار کردی؟");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "Nothing much. Stayed in.",
                        "特に何も。家にいた。",
                        "کارِ خاصی نکردم. خونه بودم.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "This Saturday you're not staying in.",
                        "今週の土曜は、家にいないよ。",
                        "این شنبه خونه نمی‌مونی.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "Am I not?",
                        "そうなんだ?",
                        "نمی‌مونم؟");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "No.",
                        "うん。",
                        "نه.");
                },
                "Tell him about Saturday", "土曜の話をする", "درباره‌ی شنبه بهش بگو",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Saturday is a half day.",
                        "土曜は半日だよ。",
                        "شنبه نصفِ روزه.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "It is.",
                        "そうだね。",
                        "آره هست.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "We're going somewhere afterwards.",
                        "そのあと、どっか行こう。",
                        "بعدش می‌ریم یه جایی.");

                    Say(Speaker.Haru, Portrait.Unchanged,
                        "Where?",
                        "どこ?",
                        "کجا؟");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "I'll tell you on Saturday.",
                        "土曜に言う。",
                        "شنبه بهت می‌گم.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "Alright.",
                        "うん。",
                        "باشه.");
                });

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Somebody's watered the plant pots.",
                "植木鉢、水やってある。",
                "یکی به گلدونا آب داده.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The soil's dark all the way down.",
                "土、下まで濡れてる。",
                "خاکش تا ته خیسه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "So they did it properly.",
                "ちゃんとやったんだ。",
                "پس درست‌حسابی آب داده.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "They did it yesterday evening.",
                "昨日の夕方にやってる。",
                "دیروز عصر آب داده.");

            Say(Speaker.Haru, Portrait.Neutral,
                "You keep saying evening.",
                "夕方って、よく言うね。",
                "همه‌ش می‌گی عصر.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Because that's when you do it.",
                "そのときにやるものだから。",
                "چون همون موقع باید آب داد.");

            Hold(2.2f);
        }

        // ---------------------------------------------------------------------
        //  Wednesday, 16:05 — three minutes about a vending machine
        //
        //  ▣ Background PastelStreetVendingDay: pastel street, the pink juice
        //    machine on the left, a bicycle parked, flowerbeds, cherry trees at
        //    the back. The machine was named in full on Monday, in the corridor,
        //    as pink, on this corner, and broken.
        //
        //  A scene with no narrative function whatsoever. It is also where she
        //  is wrong out loud, in front of him, about a thing he can see — and
        //  does not take it back.
        // ---------------------------------------------------------------------

        private void WriteWednesdayMachine()
        {
            ClearStage();

            Place(
                Backgrounds.VendingStreetDay,
                "The corner with the machine", "自販機のある角", "نبشِ دستگاه");

            Hold(2.0f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Neutral,
                "That's the machine.",
                "あれがその自販機。",
                "همون دستگاهه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The pink one that took your hundred yen.",
                "百円飲まれたピンクの。",
                "همون صورتیه که صد ینت رو خورد.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's the one.",
                "それ。",
                "همون.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Have you got another hundred?",
                "もう百円ある?",
                "صد ینِ دیگه داری؟");

            Say(Speaker.Haru, Portrait.Neutral,
                "You told me not to put money in it.",
                "入れるなって言われたけど。",
                "خودت گفتی پول نندازم توش.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Have you got another hundred?",
                "百円、ある?",
                "صد ینِ دیگه داری؟");

            Say(Speaker.Haru, Portrait.Shy,
                "Yes.",
                "ある。",
                "آره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Put it in.",
                "入れて。",
                "بندازش تو.");

            Say(Speaker.Haru, Portrait.Neutral,
                "It's my hundred.",
                "僕の百円だけど。",
                "صد ینِ منه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Put it in the machine, Haru-pi.",
                "自販機に入れて、ハルぴ。",
                "بندازش تو دستگاه، هارو‌پی.");

            Cue(SfxId.VendingThunk, 0.8f);

            Narrate(
                "The machine took the coin. Nothing came out.",
                "自販機は百円を飲みこんで、何も出さなかった。",
                "دستگاه سکه رو خورد. هیچی نداد.");

            // The one silent frame of the scene.
            Say(Speaker.Haru, Portrait.Unchanged,
                "…",
                "……",
                "…");

            Say(Speaker.Yua, Portrait.Unchanged,
                "This machine is fine.",
                "この自販機、大丈夫。",
                "این دستگاه سالمه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It just ate a hundred yen.",
                "今、百円飲んだよ。",
                "همین الان صد ین خورد.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's fine.",
                "大丈夫。",
                "سالمه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Yua-pi. I watched it.",
                "結愛ぴ。見てたんだよ。",
                "یوآ‌پی. من دیدمش.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Hit the panel on the left.",
                "左のパネル、叩いて。",
                "پنلِ سمتِ چپش رو بزن.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Isn't that stealing?",
                "それ、ちょっと泥棒じゃない?",
                "این که دزدی نیست؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You've paid twice.",
                "二回払ってる。",
                "دو بار پول دادی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Here?",
                "ここ?",
                "اینجا؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Lower. By the flap.",
                "もっと下。取り出し口のところ。",
                "پایین‌تر. کنارِ دریچه.");

            Cue(SfxId.CanDrop, 0.75f);

            Hold(0.4f);

            Cue(SfxId.CanDrop, 0.7f);

            Narrate(
                "Two cans came down into the tray.",
                "缶が二本、取り出し口に落ちてきた。",
                "دو تا قوطی افتاد تو دریچه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Two.",
                "二本。",
                "دو تا.");

            Say(Speaker.Yua, Portrait.Joyful,
                "One of them is yours.",
                "一本はそっちの。",
                "یکیش مالِ توئه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Both of them are mine. I paid twice.",
                "両方僕のだよ。二回払ったし。",
                "هردوش مالِ منه. دو بار پول دادم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "One of them is yours.",
                "一本はそっちの。",
                "یکیش مالِ توئه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Alright.",
                "はいはい。",
                "باشه.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "How did you know to hit it there?",
                "なんで、そこを叩けばいいって知ってたの?",
                "از کجا می‌دونستی باید اونجاش رو بزنی؟");

            // The one answer to a question nobody asked that this scene gets.
            Say(Speaker.Yua, Portrait.Unchanged,
                "The cold one's yours.",
                "冷たいほうがそっちの。",
                "سرده مالِ توئه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "They're both cold.",
                "両方冷たいけど。",
                "هردوش سرده.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Then it doesn't matter.",
                "じゃあどっちでもいい。",
                "پس فرقی نمی‌کنه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It doesn't.",
                "しないね。",
                "نمی‌کنه.");

            Narrate(
                "They stayed by the machine for another eleven minutes.",
                "二人はそのあと十一分、自販機の前にいた。",
                "یازده دقیقه‌ی دیگه هم اونجا موندن.");

            Say(Speaker.Haru, Portrait.Neutral,
                "So is it broken or not?",
                "結局、壊れてるの?",
                "بالاخره خرابه یا نه؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Drink your can.",
                "缶、飲んで。",
                "قوطیت رو بخور.");

            Hold(2.4f);
        }

        // =====================================================================
        //  THURSDAY — day four
        //
        //  The quietest of the six and the one carrying the least. Its work is
        //  duration: by the end of it the player has watched these two order the
        //  same drinks in the same café four times, and that is the entire
        //  mechanism by which act five costs anything.
        // =====================================================================

        private void WriteThursday()
        {
            WriteThursdayClassroom();
            WriteThursdayCorridor();
            WriteThursdayCafe();
            WriteThursdayNightStreet();
        }

        // ---------------------------------------------------------------------
        //  Thursday, 9:10 — class 1-A
        // ---------------------------------------------------------------------

        private void WriteThursdayClassroom()
        {
            ClearStage();

            Place(
                Backgrounds.ClassroomDay,
                "Class 1-A, Thursday", "木曜日の一年A組", "کلاسِ یک-الف، پنج‌شنبه");

            Hold(1.8f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Neutral,
                "There's a test on Monday.",
                "月曜、テストだって。",
                "دوشنبه امتحان داریم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "In which subject?",
                "何の?",
                "کدوم درس؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Maths. First period.",
                "数学。一限。",
                "ریاضی. زنگ اول.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That's four days away.",
                "あと四日ある。",
                "چهار روز دیگه‌ست.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You worked that out fast.",
                "計算、速いね。",
                "زود حسابش کردی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Thursday to Monday.",
                "木曜から月曜。",
                "پنج‌شنبه تا دوشنبه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "I'm bad at maths.",
                "数学、苦手なんだ。",
                "من ریاضیم بده.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I know.",
                "知ってる。",
                "می‌دونم.");

            Say(Speaker.Haru, Portrait.Neutral,
                "How do you know that?",
                "なんで知ってるの?",
                "از کجا می‌دونی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You wrote the wrong year on your notebook.",
                "ノートに年、間違えて書いてた。",
                "رو دفترت سالِ اشتباهی نوشتی.");

            Say(Speaker.Haru, Portrait.Shy,
                "That's not maths.",
                "それ数学じゃない。",
                "این که ریاضی نیست.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It's numbers.",
                "数字でしょ。",
                "عدده دیگه.");

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Neutral,
                "We'll do the maths on Sunday.",
                "日曜に数学やろう。",
                "یکشنبه ریاضی رو کار می‌کنیم.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Sunday?",
                "日曜?",
                "یکشنبه؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Sunday afternoon.",
                "日曜の午後。",
                "یکشنبه بعدازظهر.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I haven't said yes.",
                "まだいいって言ってないけど。",
                "من که هنوز نگفتم آره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Say yes.",
                "いいって言って。",
                "بگو آره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Yes.",
                "うん、いいよ。",
                "آره.");

            Hold(1.2f);

            Narrate(
                "The class rep came round collecting the club forms.",
                "委員長が部活の紙を集めに回ってきた。",
                "نماینده‌ی کلاس اومد برگه‌های باشگاه رو جمع کنه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "I didn't fill one in.",
                "書いてないや。",
                "من پُرش نکردم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Neither did I.",
                "あたしも。",
                "منم نکردم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Were you going to?",
                "書くつもりだった?",
                "می‌خواستی پُرش کنی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "No.",
                "ううん。",
                "نه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "What would you have joined?",
                "入るとしたら、何部?",
                "اگه می‌خواستی، کدوم باشگاه؟");

            // He has no answer for himself, so he answers with hers. This is
            // the shape the whole character is built on and it is shown here
            // without a word of comment.
            Say(Speaker.Yua, Portrait.Unchanged,
                "What would you have joined?",
                "そっちは?",
                "تو کدوم رو انتخاب می‌کردی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Whichever one you picked.",
                "結愛ぴが入るとこ。",
                "هر کدوم رو که تو انتخاب می‌کردی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Then it's good we didn't fill them in.",
                "じゃあ書かなくて正解だね。",
                "پس خوب شد پُرشون نکردیم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It is.",
                "そうだね。",
                "خوب شد.");

            Hold(1.8f);
        }

        // ---------------------------------------------------------------------
        //  Thursday, 15:50 — the corridor
        // ---------------------------------------------------------------------

        private void WriteThursdayCorridor()
        {
            ClearStage();

            Place(
                Backgrounds.CorridorSunset,
                "The corridor, Thursday", "木曜日の廊下", "راهرو، پنج‌شنبه");

            Hold(1.8f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Neutral,
                "The lockers have things in them now.",
                "ロッカー、もう物が入ってる。",
                "کمدها الان دیگه توشون چیز هست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Yours has one shoe bag in it.",
                "そっちは上履き袋一つだけ。",
                "تو مالِ تو فقط یه کیسه‌ی کفشه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You looked in my locker.",
                "僕のロッカー見たんだ。",
                "کمدِ منو نگاه کردی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It was open.",
                "開いてた。",
                "باز بود.");

            Say(Speaker.Haru, Portrait.Neutral,
                "It locks by itself.",
                "自動で閉まるはずだけど。",
                "خودش قفل می‌شه که.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Then it was open before it locked.",
                "じゃあ閉まる前に見た。",
                "پس قبل از اینکه قفل شه باز بود.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Alright.",
                "はいはい。",
                "باشه.");

            Hold(1.2f);

            InnerVoice(
                "Fifty-two, forty-one, and the fire door at the end.",
                "五十二番、四十一番、突き当たりに非常口。",
                "پنجاه‌ودو، چهل‌ویک، و درِ اضطراری تهِ راهرو.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Usagi or straight home?",
                "うさぎ寄る? それとも帰る?",
                "بریم اوساگی یا مستقیم بریم خونه؟");

            Say(Speaker.Haru, Portrait.Neutral,
                "Which do you want?",
                "どっちがいい?",
                "تو کدوم رو می‌خوای؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I asked you.",
                "聞いてるのはこっち。",
                "من از تو پرسیدم.");

            Say(Speaker.Haru, Portrait.Shy,
                "Usagi, then.",
                "じゃあ、うさぎ。",
                "پس اوساگی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Why that one?",
                "なんでそっち?",
                "چرا اون یکی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You've been standing facing the gate.",
                "門のほう向いて立ってたから。",
                "چون رو به دروازه وایساده بودی.");

            Hold(1.4f);

            // He reads her the way she reads him. Nobody says so.
            Say(Speaker.Yua, Portrait.Neutral,
                "That's a good answer.",
                "いい答え。",
                "جوابِ خوبی بود.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Was it the right one?",
                "合ってた?",
                "درست بود؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "Come on.",
                "行こ。",
                "بریم.");

            // White button eight.
            DecideIdly(
                "Take the long way there", "遠回りで行く", "از راهِ دور برو",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "We'll go round by the alley.",
                        "路地の方から行こう。",
                        "از راهِ کوچه می‌ریم.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "That's the long way again.",
                        "また遠回りだ。",
                        "بازم راهِ دوره.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "It is.",
                        "そうだね。",
                        "آره هست.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "The café shuts at six.",
                        "カフェ、六時に閉まるよ。",
                        "کافه ساعت شیش می‌بنده.");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "Then walk faster.",
                        "じゃあ早く歩いて。",
                        "پس تندتر راه برو.");
                },
                "Take the short way", "近道で行く", "از راهِ کوتاه برو",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Straight down the main road today.",
                        "今日は大通りをまっすぐ。",
                        "امروز مستقیم از خیابونِ اصلی.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "You never go that way.",
                        "そっち、いつも行かないのに。",
                        "تو که هیچ‌وقت از اون‌ور نمی‌ری.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Today I am.",
                        "今日は行く。",
                        "امروز می‌رم.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "We'll get there in ten minutes.",
                        "十分で着くね。",
                        "ده دقیقه‌ای می‌رسیم.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Nine.",
                        "九分。",
                        "نُه دقیقه.");
                });

            Hold(1.6f);
        }

        // ---------------------------------------------------------------------
        //  Thursday, 16:15 — the Usagi, fourth visit
        //
        //  The running joke about who is paying reaches its third beat here.
        //  Monday she paid while he read the board, Tuesday while he shook out
        //  his umbrella, and today she has stopped explaining when.
        // ---------------------------------------------------------------------

        private void WriteThursdayCafe()
        {
            ClearStage();

            Place(
                Backgrounds.CafeDay,
                "The Usagi", "うさぎ", "کافه‌ی اوساگی");

            Hold(1.8f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Narrate(
                "The woman behind the counter started making them before they spoke.",
                "店員さんは、二人が何か言う前にもう作りはじめていた。",
                "خانومِ پشتِ پیشخون قبل از سفارش شروع کرد به درست کردنشون.");

            Say(Speaker.Haru, Portrait.Joyful,
                "She knows what we have.",
                "覚えられてる。",
                "می‌دونه ما چی می‌خوریم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Bubble tea and matcha, four days running.",
                "タピオカと抹茶、四日連続。",
                "بابل‌تی و ماچا، چهار روزِ پشت‌سرِ هم.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Is that a lot?",
                "多い?",
                "زیاده؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's four days.",
                "四日は四日。",
                "چهار روزه دیگه.");

            // Third beat of the running joke.
            Say(Speaker.Haru, Portrait.Neutral,
                "I'm paying today. I said so yesterday.",
                "今日は僕が払う。昨日そう言った。",
                "امروز من حساب می‌کنم. دیروز گفتم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's paid.",
                "払った。",
                "حساب شد.");

            Say(Speaker.Haru, Portrait.Joyful,
                "When?",
                "いつ?",
                "کِی؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "Sit down.",
                "座って。",
                "بشین.");

            Say(Speaker.Haru, Portrait.Neutral,
                "You didn't answer.",
                "答えてない。",
                "جواب ندادی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Sit down, Haru-pi.",
                "座って、ハルぴ。",
                "بشین، هارو‌پی.");

            Cue(SfxId.ChairScrape, 0.55f);

            Say(Speaker.Haru, Portrait.Joyful,
                "Alright.",
                "うん。",
                "باشه.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "The string lights have a bulb out.",
                "電飾、一個切れてる。",
                "یکی از لامپای ریسه سوخته.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The fourth one from the window.",
                "窓から四つ目。",
                "چهارمی از سمتِ پنجره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You'd already found it.",
                "もう見つけてたんだ。",
                "قبلاً پیداش کرده بودی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It was out on Monday too.",
                "月曜も切れてた。",
                "دوشنبه هم سوخته بود.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Nobody's changed it in four days.",
                "四日、誰も替えてないんだ。",
                "چهار روزه کسی عوضش نکرده.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's one bulb.",
                "一個だけだしね。",
                "یه لامپه فقط.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You noticed it, though.",
                "でも気づいてる。",
                "ولی تو متوجهش شدی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "So did you.",
                "そっちもね。",
                "توام شدی.");

            Hold(1.6f);

            Narrate(
                "The glasses came over. The matcha was steaming again.",
                "グラスが来た。抹茶はまた湯気が立っていた。",
                "لیوونا رو آوردن. ماچا بازم بخار داشت.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Drink your matcha.",
                "抹茶、飲んで。",
                "ماچات رو بخور.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I know.",
                "うん。",
                "می‌دونم.");

            Cel(Portrait.DrinkFull);

            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's good.",
                "おいしい。",
                "خوبه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You said that on Monday.",
                "月曜も言ってた。",
                "دوشنبه هم همینو گفتی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It was good on Monday.",
                "月曜もおいしかった。",
                "دوشنبه هم خوب بود.");

            Cel(Portrait.DrinkReluctant);

            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Finish it.",
                "全部飲んで。",
                "تا آخرش رو بخور.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Cel(Portrait.DrinkFinished);

            Hold(1.8f);
        }

        // ---------------------------------------------------------------------
        //  Thursday, 18:20 — the corner at night
        //
        //  ▣ Background PastelStreetVendingNight: same corner as Wednesday, the
        //    pink machine lit up, a bicycle, streetlamps, a crescent moon.
        // ---------------------------------------------------------------------

        private void WriteThursdayNightStreet()
        {
            ClearStage();

            Place(
                Backgrounds.VendingStreetNight,
                "The corner, after dark", "夜の角", "نبشِ خیابون، شب");

            Hold(2.0f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Neutral,
                "The machine's lit up at night.",
                "夜は自販機、光ってるんだ。",
                "دستگاه شبا روشنه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's the brightest thing on this street.",
                "この通りで一番明るい。",
                "روشن‌ترین چیزِ این خیابونه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Should I put a hundred in?",
                "百円入れてみる?",
                "صد ین بندازم توش؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "No.",
                "だめ。",
                "نه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Yesterday you told me to.",
                "昨日は入れろって言った。",
                "دیروز گفتی بندازم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Yesterday it was four in the afternoon.",
                "昨日は午後四時だった。",
                "دیروز ساعت چهارِ بعدازظهر بود.");

            Say(Speaker.Haru, Portrait.Neutral,
                "What difference does that make?",
                "それ、関係ある?",
                "چه فرقی می‌کنه؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "It's twenty past six.",
                "六時二十分。",
                "ساعت شیش و بیست دقیقه‌ست.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That isn't an answer.",
                "答えになってない。",
                "این که جواب نشد.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Your mother will be waiting.",
                "お母さん、待ってるでしょ。",
                "مامانت منتظرته.");

            Say(Speaker.Haru, Portrait.Neutral,
                "She will.",
                "うん、待ってる。",
                "آره، منتظره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "So walk faster.",
                "じゃあ、早く。",
                "پس تندتر راه برو.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Will yours be waiting?",
                "そっちのお母さんは?",
                "مالِ تو منتظره؟");

            // Not a deflection and not a joke. There is nobody at her house who
            // is going to notice what time she comes in, and she is not going to
            // be the one to say so.
            Say(Speaker.Yua, Portrait.Unchanged,
                "No.",
                "ううん。",
                "نه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "She works late?",
                "遅いの?",
                "دیر میاد؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Something like that.",
                "そんな感じ。",
                "یه همچین چیزی.");

            // The one silent frame of the scene.
            Say(Speaker.Haru, Portrait.Unchanged,
                "…",
                "……",
                "…");

            Say(Speaker.Yua, Portrait.Joyful,
                "Walk faster, Haru-pi.",
                "早く歩いて、ハルぴ。",
                "تندتر راه برو، هارو‌پی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.6f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Tomorrow's Friday.",
                "明日は金曜。",
                "فردا جمعه‌ست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "And Saturday's a half day.",
                "土曜は半日。",
                "و شنبه نصفِ روزه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You still haven't said where we're going.",
                "どこ行くか、まだ聞いてない。",
                "هنوز نگفتی کجا می‌ریم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "No, I haven't.",
                "うん、まだ。",
                "نه، نگفتم.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Do I need to bring anything?",
                "何か持ってったほうがいい?",
                "چیزی باید بیارم؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Your train pass.",
                "定期券。",
                "کارتِ قطارت.");

            Say(Speaker.Haru, Portrait.Joyful,
                "So we're getting a train.",
                "電車乗るんだ。",
                "پس قطار سوار می‌شیم.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Goodnight, Haru-pi.",
                "おやすみ、ハルぴ。",
                "شب بخیر، هارو‌پی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Goodnight.",
                "おやすみ。",
                "شب بخیر.");

            Hold(2.2f);
        }

        // =====================================================================
        //  FRIDAY — day five
        //
        //  The window is repaired, which is the third and last instalment of a
        //  plant laid down on Monday in the most boring language the act has.
        //  And Haru asks, once, the only real question anybody asks in six days:
        //  why him. He is given an answer that is not one, and he takes it
        //  without a second's argument, which is the whole of who he is.
        // =====================================================================

        private void WriteFriday()
        {
            WriteFridayPath();
            WriteFridayClassroom();
            WriteFridayRoof();
            WriteFridayCafe();
        }

        // ---------------------------------------------------------------------
        //  Friday, 8:00 — the school path
        // ---------------------------------------------------------------------

        private void WriteFridayPath()
        {
            ClearStage();

            Place(
                Backgrounds.SchoolAlleyDay,
                "The path to school, Friday", "金曜日の通学路", "راهِ مدرسه، جمعه");

            Hold(1.8f);

            Enter(Speaker.Haru, Portrait.Neutral);
            Enter(Speaker.Yua, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Unchanged,
                "You're at the third tree.",
                "三本目の木のとこだ。",
                "سرِ درختِ سومی‌ای.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Every day this week.",
                "今週ずっと。",
                "کلِ این هفته.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Five days.",
                "五日。",
                "پنج روز.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Is that a lot?",
                "多い?",
                "زیاده؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's a week.",
                "一週間。",
                "یه هفته‌ست.");

            InnerVoice(
                "Twenty-two people at the gate.",
                "門に二十二人。",
                "بیست و دو نفر جلوی دروازه‌ن.");

            InnerVoice(
                "The side door is propped open today.",
                "今日は通用口が開けっぱなし。",
                "امروز درِ کناری رو باز گذاشتن.");

            Say(Speaker.Haru, Portrait.Neutral,
                "The petals have nearly gone.",
                "花びら、ほとんど落ちきったね。",
                "شکوفه‌ها تقریباً تموم شدن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Two weeks and they're finished.",
                "二週間で終わる。",
                "دو هفته‌ست و تموم می‌شن.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's short.",
                "短いね。",
                "کوتاهه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's the same every year.",
                "毎年そう。",
                "هر سال همینه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Do you come and look at them every year?",
                "毎年見に来るの?",
                "هر سال میای نگاهشون کنی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "They're on the way to school.",
                "通学路にあるだけ。",
                "سرِ راهِ مدرسه‌ن.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's not the same as no.",
                "それ、違うって意味じゃないよね。",
                "این که یعنی نه نیست.");

            Say(Speaker.Yua, Portrait.Joyful,
                "No, it isn't.",
                "うん、違う。",
                "نه، نیست.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "The cats from the bakery are on the wall.",
                "パン屋の猫、塀の上にいる。",
                "گربه‌های نونوایی رو دیوارن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "They walk up here in the mornings.",
                "朝はこっちまで来る。",
                "صبح‌ها میان تا این‌ور.");

            Say(Speaker.Haru, Portrait.Joyful,
                "How far is that for a cat?",
                "猫にとって、遠くない?",
                "واسه یه گربه دوره؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Four hundred metres.",
                "四百メートル。",
                "چهارصد متر.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You've measured the cats' walk.",
                "猫の散歩、測ったんだ。",
                "مسیرِ گربه‌ها رو اندازه گرفتی؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "I've measured the street.",
                "通りを測っただけ。",
                "خیابون رو اندازه گرفتم.");

            Cue(SfxId.Petal, 0.4f);

            Hold(1.2f);

            // White button nine.
            DecideIdly(
                "Ask if he slept", "眠れたか聞く", "بپرس خوابیده یا نه",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Neutral,
                        "Did you sleep?",
                        "ちゃんと寝た?",
                        "خوابیدی؟");

                    Say(Speaker.Haru, Portrait.Unchanged,
                        "A bit.",
                        "少しね。",
                        "یه‌کم.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "How many hours?",
                        "何時間?",
                        "چند ساعت؟");

                    Say(Speaker.Haru, Portrait.Shy,
                        "Four, maybe.",
                        "四時間くらいかな。",
                        "شاید چهار ساعت.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Tonight you're sleeping eight.",
                        "今夜は八時間寝て。",
                        "امشب هشت ساعت می‌خوابی.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "I'll try.",
                        "がんばる。",
                        "سعی می‌کنم.");
                },
                "Ask what he ate", "朝ごはんを聞く", "بپرس چی خورده",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Neutral,
                        "Did you have breakfast?",
                        "朝ごはん、食べた?",
                        "صبحونه خوردی؟");

                    Say(Speaker.Haru, Portrait.Unchanged,
                        "I had some tea.",
                        "お茶飲んだ。",
                        "چایی خوردم.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Tea isn't breakfast.",
                        "お茶は朝ごはんじゃない。",
                        "چایی که صبحونه نیست.");

                    Say(Speaker.Haru, Portrait.Shy,
                        "It was hot.",
                        "あったかかったよ。",
                        "داغ بود.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "There's a lunch in my bag for you.",
                        "鞄にお弁当ある。そっちの。",
                        "تو کیفم واست ناهار هست.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "Of course there is.",
                        "だと思った。",
                        "معلومه که هست.");
                });

            Hold(1.6f);
        }

        // ---------------------------------------------------------------------
        //  Friday, 8:30 — class 1-A, and the window
        //
        //  Stage three of the plant. Somebody has fixed the catch, it is
        //  reported in one narrated line, and nobody makes anything of it — the
        //  scene is allowed to be oblique here precisely because Monday and
        //  Tuesday were not.
        // ---------------------------------------------------------------------

        private void WriteFridayClassroom()
        {
            ClearStage();

            Place(
                Backgrounds.ClassroomDay,
                "Class 1-A, Friday", "金曜日の一年A組", "کلاسِ یک-الف، جمعه");

            Hold(1.8f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Narrate(
                "Somebody had fixed the catch on the window.",
                "誰かが窓の留め金を直していた。",
                "یکی گیره‌ی پنجره رو درست کرده بود.");

            Say(Speaker.Haru, Portrait.Neutral,
                "They fixed it.",
                "直ってる。",
                "درستش کردن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Yes.",
                "うん。",
                "آره.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It shuts all the way now.",
                "ちゃんと閉まる。",
                "الان تا آخر بسته می‌شه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It does.",
                "そうだね。",
                "می‌شه.");

            // The one silent frame of the scene.
            Say(Speaker.Haru, Portrait.Unchanged,
                "…",
                "……",
                "…");

            Say(Speaker.Yua, Portrait.Unchanged,
                "What?",
                "何?",
                "چیه؟");

            Say(Speaker.Haru, Portrait.Joyful,
                "Nothing. It's better this way.",
                "別に。このほうがいいよ。",
                "هیچی. این‌طوری بهتره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's better.",
                "いいね。",
                "بهتره.");

            Hold(1.6f);

            InnerVoice(
                "No desk is going to be wet again.",
                "もう机は濡れない。",
                "دیگه هیچ میزی خیس نمی‌شه.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "You said to ask you on Friday.",
                "金曜に聞いてって言ってた。",
                "گفتی جمعه ازت بپرسم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's Friday. Ask.",
                "金曜だよ。どうぞ。",
                "جمعه‌ست. بپرس.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Why did you come over to me?",
                "なんで、僕のところに来たの?",
                "چرا اومدی سراغِ من؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "When?",
                "いつの話?",
                "کِی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Monday. On the path.",
                "月曜。通学路で。",
                "دوشنبه. تو راهِ مدرسه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I was standing under a tree and you walked straight at me.",
                "木の下に立ってたら、まっすぐ来た。",
                "زیرِ یه درخت وایساده بودم و تو مستقیم اومدی سمتِ من.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "You knew which class we were in.",
                "教室も知ってた。",
                "می‌دونستی کدوم کلاسیم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You knew which seats went first.",
                "どの席が先に埋まるかも知ってた。",
                "می‌دونستی کدوم صندلیا زودتر پُر می‌شن.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You had a lunch for two in your bag.",
                "二人分のお弁当も持ってた。",
                "تو کیفت ناهارِ دو نفره داشتی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "What's the question?",
                "質問は?",
                "سؤالت چیه؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Why me?",
                "なんで僕なの?",
                "چرا من؟");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "You were there first.",
                "先に来てたから。",
                "چون زودتر از همه اومده بودی.");

            Say(Speaker.Haru, Portrait.Neutral,
                "That isn't an answer.",
                "答えになってない。",
                "این که جواب نیست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's the one I've got.",
                "それしかない。",
                "جوابیه که دارم.");

            Hold(1.8f);

            // He is not going to push, and it takes him four frames to stop.
            Say(Speaker.Haru, Portrait.Unchanged,
                "Alright.",
                "うん。",
                "باشه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That's it?",
                "それだけ?",
                "همین؟");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's it.",
                "それだけ。",
                "همین.");

            Say(Speaker.Yua, Portrait.Neutral,
                "You waited two days to ask me that.",
                "二日待って、それ聞いたの。",
                "دو روز صبر کردی که اینو بپرسی.");

            Say(Speaker.Haru, Portrait.Shy,
                "I did.",
                "うん。",
                "آره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "And now you're not asking again.",
                "で、もう聞かない。",
                "و حالا دیگه نمی‌پرسی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "No.",
                "うん。",
                "نه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Good.",
                "いいね。",
                "خوبه.");

            Hold(2.2f);
        }

        // ---------------------------------------------------------------------
        //  Friday, 12:30 — the roof, longest scene in the act
        //
        //  Nothing happens in it. Two people lie on a roof and name clouds, and
        //  it is deliberately the last long stretch of the game where that is
        //  true. It also holds the only two firsts the act has been saving: Yua
        //  is shy once, for one frame, in six days — and Haru counts something
        //  out loud, which until now has been the one thing that is hers.
        // ---------------------------------------------------------------------

        private void WriteFridayRoof()
        {
            Say(Speaker.Yua, Portrait.Neutral,
                "Roof.",
                "屋上。",
                "پشت‌بوم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Roof.",
                "屋上ね。",
                "پشت‌بوم.");

            ClearStage();

            Place(
                Backgrounds.RooftopDay,
                "The roof, Friday", "金曜日の屋上", "پشت‌بوم، جمعه");

            Hold(2.2f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Lie down.",
                "寝転んで。",
                "دراز بکش.");

            Say(Speaker.Haru, Portrait.Neutral,
                "The floor's a bit hot.",
                "床、ちょっと熱いけど。",
                "کفش یه‌کم داغه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Lie down.",
                "寝転んで。",
                "دراز بکش.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(2.0f);

            Say(Speaker.Haru, Portrait.Neutral,
                "You can see the water tower from down here.",
                "ここから給水塔が見える。",
                "از این پایین برجِ آب پیداست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's got a crack down the side.",
                "横にひび入ってる。",
                "از پهلوش ترک خورده.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You can't see that from here.",
                "ここからは見えないでしょ。",
                "از اینجا که نمی‌شه دیدش.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I've seen it from the gate.",
                "門から見た。",
                "از دروازه دیدمش.");

            Say(Speaker.Haru, Portrait.Neutral,
                "That's four hundred metres.",
                "四百メートルあるよ。",
                "چهارصد متره.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Three hundred and eighty.",
                "三百八十。",
                "سیصد و هشتاد.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Of course.",
                "だよね。",
                "معلومه.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "That cloud is a bicycle.",
                "あの雲、自転車。",
                "اون ابره دوچرخه‌ست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That is not a bicycle.",
                "自転車じゃない。",
                "اون دوچرخه نیست.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Two wheels and a person on top.",
                "車輪二つと、人が乗ってる。",
                "دو تا چرخ و یه آدم روش.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Two round bits and a long bit.",
                "丸が二つと、細長いのが一つ。",
                "دو تا چیزِ گرد و یه چیزِ دراز.");

            Say(Speaker.Haru, Portrait.Joyful,
                "A bicycle.",
                "自転車。",
                "دوچرخه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Fine. A bicycle.",
                "はいはい。自転車。",
                "باشه. دوچرخه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Thank you.",
                "ありがとう。",
                "ممنون.");

            Narrate(
                "They did that with six more clouds.",
                "そのあと、雲を六つ分やった。",
                "شیش تا ابرِ دیگه هم همین کارو کردن.");

            Hold(2.0f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Stop counting things for a minute.",
                "一分だけ、数えるのやめてみて。",
                "یه دقیقه چیزی نشمر.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Why?",
                "なんで?",
                "چرا؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I want to see if you can.",
                "できるのか見たい。",
                "می‌خوام ببینم می‌تونی یا نه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That's a strange thing to want.",
                "変なこと言うね。",
                "چیزِ عجیبی می‌خوای.");

            Say(Speaker.Haru, Portrait.Joyful,
                "One minute.",
                "一分だけ。",
                "فقط یه دقیقه.");

            Hold(2.4f);

            Narrate(
                "She managed thirty-eight seconds.",
                "三十八秒もった。",
                "سی و هشت ثانیه دووم آورد.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Thirty-eight seconds.",
                "三十八秒。",
                "سی و هشت ثانیه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "You counted them.",
                "数えてたんだ。",
                "تو شمردیشون.");

            // The one thing that is hers, done back to her, kindly.
            Say(Speaker.Haru, Portrait.Neutral,
                "Somebody had to.",
                "誰かが数えないと。",
                "یکی باید می‌شمرد.");

            // Yua is shy exactly once in six days, and this is it.
            Enter(Speaker.Yua, Portrait.Shy);

            Hold(1.8f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Don't do that again.",
                "もうやらないで。",
                "دیگه این کارو نکن.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Enter(Speaker.Yua, Portrait.Neutral);

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "The bell's in six minutes.",
                "あと六分でチャイム。",
                "شیش دقیقه دیگه زنگ می‌خوره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You counted.",
                "数えてる。",
                "شمردی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Somebody had to.",
                "誰かが数えないと。",
                "یکی باید می‌شمرد.");

            Narrate(
                "Neither of them got up for another eleven minutes.",
                "そのあと十一分、二人とも起き上がらなかった。",
                "یازده دقیقه‌ی دیگه هیچ‌کدوم بلند نشدن.");

            Cue(SfxId.SchoolBell, 0.45f);

            Hold(2.4f);
        }

        // ---------------------------------------------------------------------
        //  Friday, 16:30 — the Usagi
        // ---------------------------------------------------------------------

        private void WriteFridayCafe()
        {
            ClearStage();

            Place(
                Backgrounds.CafeDay,
                "The Usagi, Friday", "金曜日のうさぎ", "کافه‌ی اوساگی، جمعه");

            Hold(1.8f);

            Enter(Speaker.Yua, Portrait.Joyful);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Neutral,
                "They've changed the bulb.",
                "電球、替わってる。",
                "لامپ رو عوض کردن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The fourth one from the window.",
                "窓から四つ目。",
                "چهارمی از سمتِ پنجره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You checked as we came in.",
                "入った瞬間見たでしょ。",
                "همون موقع که اومدیم تو نگاه کردی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "So did you.",
                "そっちもね。",
                "توام کردی.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Five days in a row.",
                "五日連続。",
                "پنج روزِ پشتِ هم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Bubble tea and matcha, five times.",
                "タピオカと抹茶、五回。",
                "بابل‌تی و ماچا، پنج بار.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Do you want to do this next week too?",
                "来週もこうする?",
                "هفته‌ی بعدم بیایم؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Yes.",
                "うん。",
                "آره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You didn't think about it.",
                "考えもしなかったね。",
                "اصلاً فکر نکردی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "No.",
                "うん。",
                "نکردم.");

            Hold(1.4f);

            Narrate(
                "The glasses came over.",
                "グラスが来た。",
                "لیوونا رو آوردن.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Drink your matcha.",
                "抹茶、飲んで。",
                "ماچات رو بخور.");

            Cel(Portrait.DrinkFull);

            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Unchanged,
                "Can I ask you something else?",
                "もう一つ聞いてもいい?",
                "می‌تونم یه چیزِ دیگه بپرسم؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Ask.",
                "どうぞ。",
                "بپرس.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "What's your favourite thing on the board?",
                "黒板の中で、何が一番好き?",
                "رو تابلو، از چی بیشتر خوشت میاد؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Bubble tea.",
                "タピオカ。",
                "بابل‌تی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's what you order every day.",
                "毎日それ頼んでる。",
                "همون که هر روز سفارش می‌دی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That's why it's my favourite.",
                "だから一番好き。",
                "واسه همین ازش بیشتر خوشم میاد.");

            Cel(Portrait.DrinkReluctant);

            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Now you. Which is yours?",
                "そっちは? どれが好き?",
                "حالا تو. مالِ تو کدومه؟");

            // The question he cannot answer, and the answer he gives instead.
            Say(Speaker.Haru, Portrait.Unchanged,
                "Matcha.",
                "抹茶。",
                "ماچا.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You don't like matcha.",
                "抹茶、嫌いでしょ。",
                "از ماچا خوشت نمیاد.");

            // The one silent frame of the scene.
            Say(Speaker.Haru, Portrait.Unchanged,
                "…",
                "……",
                "…");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Finish it.",
                "全部飲んで。",
                "تا آخرش رو بخور.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Cel(Portrait.DrinkFinished);

            Hold(1.8f);

            Say(Speaker.Haru, Portrait.Joyful,
                "Tomorrow you're telling me where we're going.",
                "明日、どこ行くか教えてね。",
                "فردا می‌گی کجا می‌ریم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Tomorrow.",
                "明日ね。",
                "فردا.");

            Hold(2.2f);
        }

        // =====================================================================
        //  SATURDAY — day six, and the end of the act
        //
        //  A half day, and then the only afternoon of the six that neither of
        //  them has to be anywhere. It holds the third and last of the moments
        //  her face goes somewhere else, and its trigger is the smallest thing
        //  in the act: he asks her permission to sit down on a swing.
        //
        //  Nothing in the scene explains why that is the one that lands. It is
        //  the first time the arrangement has produced something she did not
        //  ask for, and she has no idea what to do with it except give an order.
        // =====================================================================

        private void WriteSaturday()
        {
            WriteSaturdayCorridor();
            WriteSaturdayCorner();
            WriteSaturdayPark();
            WriteSaturdayPlatform();
            WriteSaturdayNight();
        }

        // ---------------------------------------------------------------------
        //  Saturday, 12:40 — the corridor, end of a half day
        // ---------------------------------------------------------------------

        private void WriteSaturdayCorridor()
        {
            ClearStage();

            Place(
                Backgrounds.CorridorSunset,
                "The corridor, Saturday", "土曜日の廊下", "راهرو، شنبه");

            Hold(1.8f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Narrate(
                "Saturday finished at half past twelve.",
                "土曜は十二時半で終わった。",
                "شنبه ساعت دوازده و نیم تموم شد.");

            Say(Speaker.Haru, Portrait.Neutral,
                "So where are we going?",
                "で、どこ行くの?",
                "خب، کجا می‌ریم؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "There's a park two stops down the line.",
                "二駅先に公園がある。",
                "دو تا ایستگاه اون‌ورتر یه پارک هست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Two stops on the train?",
                "電車で二駅?",
                "دو تا ایستگاه با قطار؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Two stops on the train.",
                "電車で二駅。",
                "دو تا ایستگاه با قطار.");

            Say(Speaker.Haru, Portrait.Joyful,
                "What's in the park?",
                "公園に何があるの?",
                "تو پارک چی هست؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "A stone angel with a fountain under it.",
                "石の天使と、その下に噴水。",
                "یه فرشته‌ی سنگی که زیرش حوض داره.");

            Say(Speaker.Haru, Portrait.Neutral,
                "And?",
                "それと?",
                "و؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Swings. A slide. Flowerbeds.",
                "ブランコ。滑り台。花壇。",
                "تاب. سرسره. باغچه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You've been there.",
                "行ったことあるんだ。",
                "رفتی اونجا.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Come on.",
                "ほら、来て。",
                "بیا دیگه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "That's not a no.",
                "違うって言わないんだ。",
                "این که یعنی نه نیست.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It isn't.",
                "違わない。",
                "نیست.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "We should eat something first.",
                "先に何か食べたほうがいいかな。",
                "اول باید یه چیزی بخوریم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The bakery on the corner is open until two.",
                "角のパン屋、二時までやってる。",
                "نونواییِ سرِ نبش تا ساعت دو بازه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Of course it is.",
                "だと思った。",
                "معلومه که هست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's twelve forty.",
                "十二時四十分。",
                "ساعت دوازده و چهل دقیقه‌ست.");

            Say(Speaker.Haru, Portrait.Neutral,
                "We've got an hour and twenty.",
                "一時間二十分ある。",
                "یه ساعت و بیست دقیقه وقت داریم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "You did that in your head.",
                "暗算したね。",
                "ذهنی حسابش کردی.");

            Say(Speaker.Haru, Portrait.Shy,
                "It was easy.",
                "簡単だった。",
                "آسون بود.");

            Say(Speaker.Yua, Portrait.Joyful,
                "You're bad at maths.",
                "数学、苦手なんでしょ。",
                "تو که ریاضیت بده.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I'm bad at maths on paper.",
                "紙の上だと苦手。",
                "رو کاغذ بدم.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "The corridor's empty already.",
                "廊下、もう誰もいない。",
                "راهرو الان خالیه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Everyone leaves fast on a Saturday.",
                "土曜はみんな帰るの早い。",
                "شنبه‌ها همه زود می‌رن.");

            Say(Speaker.Haru, Portrait.Joyful,
                "We're always the last two.",
                "いつも僕らが最後。",
                "همیشه ما دو تا آخریم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Six days out of six.",
                "六日中六日。",
                "شیش روز از شیش روز.");

            // Apologising for nothing, third of three.
            Say(Speaker.Haru, Portrait.Shy,
                "Sorry. That's my fault.",
                "……ごめん。僕のせいだ。",
                "ببخشید. تقصیرِ منه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "I wait for you.",
                "待ってるのはこっち。",
                "من واسه تو صبر می‌کنم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I know.",
                "うん。",
                "می‌دونم.");

            Hold(1.6f);
        }

        // ---------------------------------------------------------------------
        //  Saturday, 13:00 — the corner
        //
        //  ▣ Background UsagiBakeryStreetDay: cobbles, the bakery window full
        //    of bread and pastries, the bench with the two cats, potted flowers.
        // ---------------------------------------------------------------------

        private void WriteSaturdayCorner()
        {
            ClearStage();

            Place(
                Backgrounds.BakeryStreetDay,
                "The corner, Saturday", "土曜日の角の通り", "سرِ نبش، شنبه");

            Hold(1.8f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Joyful,
                "Both cats are on the bench today.",
                "今日は猫、二匹ともベンチ。",
                "امروز هر دو گربه رو نیمکتن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "They're there every afternoon.",
                "毎日午後いる。",
                "هر روز بعدازظهر اونجان.");

            Say(Speaker.Haru, Portrait.Neutral,
                "The grey one has moved to the other end.",
                "灰色のほう、端に移ってる。",
                "خاکستریه رفته اون سرِ نیمکت.");

            Say(Speaker.Yua, Portrait.Joyful,
                "That's the first thing you've noticed before me.",
                "初めて先に気づいたね。",
                "این اولین چیزیه که زودتر از من دیدی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It isn't.",
                "そんなことないよ。",
                "نیست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Name another one.",
                "じゃあ他に言ってみて。",
                "یکی دیگه بگو.");

            Say(Speaker.Haru, Portrait.Neutral,
                "You look at the door before you sit down anywhere.",
                "座る前に、いつもドアを見る。",
                "قبل از اینکه هرجا بشینی، به در نگاه می‌کنی.");

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Everyone does that.",
                "みんなやってる。",
                "همه این کارو می‌کنن.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Alright.",
                "うん。",
                "باشه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "What's in the window?",
                "ウィンドウ、何がある?",
                "تو ویترین چی هست؟");

            Say(Speaker.Haru, Portrait.Joyful,
                "Melon bread, curry bread, and something with a rabbit on it.",
                "メロンパン、カレーパン、うさぎの顔のやつ。",
                "ملون‌پان، کاری‌پان، و یه چیزی که خرگوش روشه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The rabbit one is custard.",
                "うさぎのはカスタード。",
                "خرگوشیه توش کاستارده.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You've had one.",
                "食べたことあるんだ。",
                "خوردیش قبلاً.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's written on the label.",
                "ラベルに書いてある。",
                "رو برچسبش نوشته.");

            // White button ten, and the last one in the act.
            DecideIdly(
                "Buy him the melon bread", "メロンパンを買う", "واسش ملون‌پان بخر",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "One melon bread. It's yours.",
                        "メロンパン一つ。そっちの。",
                        "یه ملون‌پان. مالِ توئه.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "I didn't say I wanted that one.",
                        "それがいいとは言ってないよ。",
                        "من که نگفتم اونو می‌خوام.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "You looked at it twice.",
                        "二回見てた。",
                        "دو بار نگاش کردی.");

                    Say(Speaker.Haru, Portrait.Shy,
                        "Did I?",
                        "そう?",
                        "جدی؟");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "Twice.",
                        "二回。",
                        "دو بار.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "Then I want that one.",
                        "じゃあ、それがいい。",
                        "پس همونو می‌خوام.");
                },
                "Ask him to pick", "選ばせる", "بذار خودش انتخاب کنه",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Pick one.",
                        "選んで。",
                        "یکی انتخاب کن.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "Which are you having?",
                        "そっちは?",
                        "تو کدومو می‌گیری؟");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "I asked you first.",
                        "先に聞いたのはこっち。",
                        "من اول پرسیدم.");

                    Say(Speaker.Haru, Portrait.Shy,
                        "The melon bread, then.",
                        "じゃあ、メロンパン。",
                        "پس ملون‌پان.");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "That took eleven seconds.",
                        "十一秒かかった。",
                        "یازده ثانیه طول کشید.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "It's a big window.",
                        "ウィンドウ、広いから。",
                        "ویترینش بزرگه.");
                });

            Hold(1.8f);
        }

        // ---------------------------------------------------------------------
        //  Saturday, 14:10 — the park with the stone angel
        //
        //  ▣ Background PastelPlaygroundDay: a stone angel over a fountain,
        //    swings, a slide with children on it, flowerbeds, houses behind.
        //
        //  The swing is named before anybody is told to push anything, the
        //  eleven minutes are narrated because there is no sprite of a girl on a
        //  swing, and the flowerbed gets four frames and no significance — the
        //  red flowers are act three's, and act one has no business pointing at
        //  them.
        //
        //  The third slip is here, and its trigger is one polite question.
        // ---------------------------------------------------------------------

        private void WriteSaturdayPark()
        {
            ClearStage();

            Place(
                Backgrounds.PlaygroundDay,
                "The park", "公園", "پارک");

            Hold(2.0f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            InnerVoice(
                "Six children. Two gates. One way out at the back.",
                "子どもが六人。門が二つ。奥にもう一つ出口。",
                "شیش تا بچه. دو تا دروازه. یه راهِ خروج هم ته پارک.");

            Say(Speaker.Haru, Portrait.Joyful,
                "There's the angel.",
                "天使、あれだ。",
                "فرشته اوناهاش.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The fountain under it works in summer.",
                "下の噴水、夏は動いてる。",
                "حوضِ زیرش تابستون کار می‌کنه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "It's not running now.",
                "今は止まってる。",
                "الان کار نمی‌کنه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's April.",
                "四月だからね。",
                "آوریله.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Six kids on the slide.",
                "滑り台に六人。",
                "شیش تا بچه رو سرسره‌ن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Seven. One's behind the fountain.",
                "七人。一人、噴水の後ろ。",
                "هفت تا. یکیشون پشتِ حوضه.");

            Narrate(
                "A child came out from behind the fountain.",
                "噴水の後ろから、子どもが一人出てきた。",
                "یه بچه از پشتِ حوض اومد بیرون.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Seven.",
                "七人。",
                "هفت تا.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Seven.",
                "七人。",
                "هفت تا.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "There are two swings free.",
                "ブランコ、二つ空いてる。",
                "دو تا تاب خالیه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The chains on the left one are shorter.",
                "左のほうが鎖が短い。",
                "زنجیرِ سمتِ چپیه کوتاه‌تره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You measured the swings.",
                "ブランコまで測ったんだ。",
                "تابا رو هم اندازه گرفتی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I looked at them.",
                "見ただけ。",
                "فقط نگاهشون کردم.");

            // The question, and the whole reason this scene is in the act.
            Say(Speaker.Haru, Portrait.Neutral,
                "Can I sit on the swing?",
                "ブランコ、座ってもいい?",
                "می‌تونم رو تاب بشینم؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "What?",
                "え?",
                "چی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "The swing. Can I sit on it?",
                "ブランコ。座ってもいい?",
                "رو تاب. می‌تونم بشینم؟");

            // Slip three of three.
            FaceSlips();

            Say(Speaker.Yua, Portrait.Unchanged,
                "Why are you asking me?",
                "なんであたしに聞くの?",
                "چرا از من می‌پرسی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I don't know.",
                "わかんない。",
                "نمی‌دونم.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "I'll sit on it. You push.",
                "あたしが座る。押して。",
                "من می‌شینم. تو هلم بده.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Okay.",
                "うん。",
                "باشه.");

            Narrate(
                "She sat on the left swing.",
                "彼女は左のブランコに座った。",
                "نشست رو تابِ سمتِ چپ.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Push me.",
                "押して。",
                "هلم بده.");

            Narrate(
                "He pushed her for eleven minutes.",
                "彼は十一分間、押しつづけた。",
                "یازده دقیقه هلش داد.");

            Hold(2.2f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Stop.",
                "もういい。",
                "بس کن.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Was that long enough?",
                "足りた?",
                "بس بود؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "Your arms will hurt tomorrow.",
                "明日、腕が痛くなるよ。",
                "فردا بازوهات درد می‌گیره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Probably.",
                "たぶんね。",
                "احتمالاً.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Sit down. You've been standing since twelve.",
                "座って。十二時から立ちっぱなし。",
                "بشین. از ساعت دوازده وایسادی.");

            Say(Speaker.Haru, Portrait.Shy,
                "So have you.",
                "そっちもでしょ。",
                "توام همین‌طور.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Sit down, Haru-pi.",
                "座って、ハルぴ。",
                "بشین، هارو‌پی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.6f);

            Say(Speaker.Haru, Portrait.Neutral,
                "What are the flowers in that bed?",
                "あの花壇の花、何?",
                "اون گلای تو باغچه چی‌ان؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I don't know what they're called.",
                "名前は知らない。",
                "نمی‌دونم اسمشون چیه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "They're red.",
                "赤いね。",
                "قرمزن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "They are.",
                "うん。",
                "آره.");

            Hold(1.8f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Yua-pi?",
                "結愛ぴ?",
                "یوآ‌پی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "What.",
                "何。",
                "چیه.");

            // The one silent frame of the scene.
            Say(Speaker.Haru, Portrait.Unchanged,
                "…",
                "……",
                "…");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Say it.",
                "言って。",
                "بگو.");

            Say(Speaker.Haru, Portrait.Joyful,
                "The bed's been watered.",
                "水、やってある。",
                "به باغچه آب دادن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "This morning. It should have been last night.",
                "今朝。昨日の夕方にやるべきだった。",
                "امروز صبح. دیشب باید آب می‌دادن.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Does it matter?",
                "違うの?",
                "فرقی می‌کنه؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It matters to the flowers.",
                "花には違う。",
                "واسه گلا فرق می‌کنه.");

            Hold(2.4f);
        }

        // ---------------------------------------------------------------------
        //  Saturday, 17:30 — the platform
        //
        //  ▣ Background TrainPlatformSunset: platform in low gold light, a pink
        //    drinks machine, coloured waiting chairs, sakura along the tracks.
        //
        //  The fourth beat of the running joke about who is paying. There is
        //  deliberately no line for Haru in it. Three times he has offered and
        //  three times she had already paid; the fourth time he says nothing at
        //  all, and nothing in the scene draws attention to the gap where his
        //  line was.
        // ---------------------------------------------------------------------

        private void WriteSaturdayPlatform()
        {
            ClearStage();

            Place(
                Backgrounds.TrainPlatformSunset,
                "The platform", "ホーム", "سکوی ایستگاه");

            Hold(2.0f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            InnerVoice(
                "Two exits. The stairs and the ramp at the far end.",
                "出口は二つ。階段と、奥のスロープ。",
                "دو تا خروجی. پله‌ها و سطحِ شیب‌دارِ ته سکو.");

            Say(Speaker.Haru, Portrait.Neutral,
                "The next one's at twenty to six.",
                "次は五時四十分。",
                "بعدی ساعت پنج و چهل دقیقه‌ست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You read the board.",
                "時刻表、読んだんだ。",
                "تابلو رو خوندی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Before you did.",
                "先に読んだ。",
                "قبل از تو خوندمش.");

            Say(Speaker.Yua, Portrait.Joyful,
                "That's twice today.",
                "今日、二回目。",
                "امروز دومین بارته.");

            // The gap. She buys, he takes his, and nobody remarks.
            Narrate(
                "She bought two tickets at the machine and turned round with them.",
                "彼女は券売機で切符を二枚買って、振り返った。",
                "دو تا بلیت از دستگاه گرفت و برگشت.");

            Hold(1.8f);

            Narrate(
                "He took his and put it in his pocket.",
                "彼は自分の分を受け取って、ポケットに入れた。",
                "مالِ خودشو گرفت و گذاشت تو جیبش.");

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Platform two.",
                "二番線。",
                "سکوی دو.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "There's a drinks machine here too.",
                "ここにも自販機ある。",
                "اینجا هم یه دستگاهِ نوشیدنی هست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's the same pink as ours.",
                "うちのと同じピンク。",
                "همون صورتیِ مالِ ماست.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Ours.",
                "うちの。",
                "مالِ ما.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The one on the corner.",
                "角のやつ。",
                "همونی که سرِ نبشه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You called it ours.",
                "うちのって言った。",
                "گفتی مالِ ما.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's on the way home.",
                "帰り道にあるから。",
                "سرِ راهِ خونه‌ست.");

            Hold(1.6f);

            Say(Speaker.Haru, Portrait.Neutral,
                "The sakura along the track are further on than ours.",
                "線路沿いの桜、うちのより咲いてる。",
                "شکوفه‌های کنارِ ریل از مالِ ما جلوترن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's warmer two stops down.",
                "二駅先は暖かい。",
                "دو تا ایستگاه اون‌ورتر گرم‌تره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "By how much?",
                "どれくらい?",
                "چقدر؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "Enough for a week of blossom.",
                "花が一週間分。",
                "به اندازه‌ی یه هفته شکوفه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "That's not a temperature.",
                "それ、温度じゃない。",
                "این که دما نیست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's the only measurement that matters today.",
                "今日はそれで足りる。",
                "امروز همین اندازه‌گیری کافیه.");

            Hold(1.8f);

            Say(Speaker.Haru, Portrait.Neutral,
                "It's been a good week.",
                "いい一週間だった。",
                "هفته‌ی خوبی بود.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Six days.",
                "六日。",
                "شیش روز.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Six days.",
                "六日。",
                "شیش روز.");

            // The one silent frame of the scene.
            Say(Speaker.Yua, Portrait.Unchanged,
                "…",
                "……",
                "…");

            Say(Speaker.Haru, Portrait.Neutral,
                "What?",
                "どうしたの?",
                "چیه؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "Nothing. The train's coming.",
                "別に。電車来た。",
                "هیچی. قطار داره میاد.");

            Hold(2.2f);
        }

        // ---------------------------------------------------------------------
        //  Saturday, 19:15 — the corner, and the end of act one
        //
        //  The running argument closes here, and it closes by him agreeing with
        //  her about a fact he watched with his own eyes six days ago. Nothing
        //  says so. The act stops rather than ends.
        // ---------------------------------------------------------------------

        private void WriteSaturdayNight()
        {
            ClearStage();

            Place(
                Backgrounds.VendingStreetNight,
                "The corner, Saturday night", "土曜の夜の角", "نبشِ خیابون، شبِ شنبه");

            Hold(2.2f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Neutral,
                "It's quarter past seven.",
                "七時十五分。",
                "ساعت هفت و ربعه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Your mother will be waiting.",
                "お母さん、待ってる。",
                "مامانت منتظرته.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I told her it would be late.",
                "遅くなるって言ってある。",
                "بهش گفتم دیر می‌شه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "When did you tell her that?",
                "いつ言ったの?",
                "کِی بهش گفتی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Wednesday.",
                "水曜。",
                "چهارشنبه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I only said where we were going today.",
                "行き先は今日言ったのに。",
                "من که تازه امروز گفتم کجا می‌ریم.");

            Say(Speaker.Haru, Portrait.Shy,
                "You said Saturday on Wednesday.",
                "水曜に、土曜って言った。",
                "چهارشنبه گفتی شنبه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "So I did.",
                "言ったね。",
                "آره گفتم.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "The machine's lit up again.",
                "自販機、また光ってる。",
                "دستگاه بازم روشنه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's the brightest thing on the street.",
                "この通りで一番明るい。",
                "روشن‌ترین چیزِ خیابونه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It's a good machine.",
                "いい自販機だよ。",
                "دستگاهِ خوبیه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "It ate two hundred yen of yours.",
                "二百円飲まれてる。",
                "دویست ینت رو خورد.");

            // He has adopted her version of a thing he watched happen.
            Say(Speaker.Haru, Portrait.Unchanged,
                "You said it was fine.",
                "大丈夫って言ってた。",
                "خودت گفتی سالمه.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "I did say that.",
                "言った。",
                "آره، گفتم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "So it's fine.",
                "じゃあ大丈夫。",
                "پس سالمه.");

            Hold(1.8f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Your street's that way.",
                "そっちが家の通り。",
                "کوچه‌ی شما اون‌وره.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It is.",
                "うん。",
                "آره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Go on, then.",
                "じゃあ、行って。",
                "خب، برو دیگه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "You're going the other way on your own.",
                "そっちは一人で反対方向だね。",
                "تو تنهایی از اون‌ور می‌ری.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's eleven minutes.",
                "十一分。",
                "یازده دقیقه‌ست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I could walk you.",
                "送ろうか。",
                "می‌تونم برسونمت.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Then you'd walk back on your own.",
                "そしたら帰りは一人でしょ。",
                "اون‌وقت خودت تنهایی برمی‌گردی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's twenty-two minutes.",
                "二十二分だ。",
                "می‌شه بیست و دو دقیقه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It is.",
                "そうだね。",
                "آره می‌شه.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Monday.",
                "月曜。",
                "دوشنبه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Monday.",
                "月曜。",
                "دوشنبه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The third tree.",
                "三本目の木。",
                "درختِ سومی.");

            // Her line, in his mouth, and the last thing said in act one.
            Say(Speaker.Haru, Portrait.Joyful,
                "I know.",
                "知ってる。",
                "می‌دونم.");

            Hold(2.6f);

            ClearStage();

            InnerVoice(
                "Six days.",
                "六日。",
                "شیش روز.");

            InnerVoice(
                "He said I know.",
                "知ってる、って言った。",
                "گفت می‌دونم.");

            Hold(1.6f);

            InnerVoice(
                "So do I.",
                "あたしも。",
                "منم می‌دونم.");

            Hold(3.0f);
        }
    }
}
