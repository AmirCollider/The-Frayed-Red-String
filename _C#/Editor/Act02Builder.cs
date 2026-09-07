// -----------------------------------------------------------------------------
//  The Frayed Red String
//  Act02Builder.cs  (Editor only)
//
//  Act two — Maboroshi (幻) — written out as code.
//
//  Run it once from The Frayed Red String ▸ Build Act 02 From The Story
//  Document. It writes Assets/Story/Acts/Act02.asset, reusing the asset that is
//  already there so nothing pointing at it breaks, and then hands over to the
//  Story Editor.
//
//  This is a complete rewrite against the dialogue manual, version five
//  (AboutProject/دستورنامه-دیالوگ-نخ-قرمز-پوسیده-v5.md) and the narrative
//  document 3.1.2, which give act two four jobs and no others:
//
//    • Go deeper than act one, and go slower doing it. The manual is exact
//      about what "slower" means and it is not "shorter": a deeper scene has
//      FEWER changes of subject and MORE frames, longer lines, more comfortable
//      silence, and a narrator who works harder because atmosphere is the only
//      thing that makes slowness read as deliberate rather than as a game that
//      has stalled. So act one's twenty short scenes over six days become nine
//      long ones over five, the narration density goes from about one in
//      fifteen to about one in eight, and the café scene alone is longer than
//      any three scenes of act one put together.
//
//    • Haru tells Yua about his friend. It is the only heavy scene in the first
//      four acts and it is written under the manual's seven rules for one: it
//      is entered from the side, in the middle of something ordinary; it is
//      told badly, out of order, with one completely irrelevant detail stuck to
//      it; the method is never said, not once, not obliquely; the horror is in
//      the listener and not in the story; nothing is resolved and nobody is
//      comforted; the scene does not end on it; and it leaks into the next day
//      as an absence nobody explains.
//
//    • Several white choices, and NO blue or green ones. Not one, anywhere in
//      this act. The mechanics table forbids a mechanic before its date, and
//      the date for blue and green is the explanation — which is the last scene
//      of this act. A blue button before Yua has said what a blue button is
//      would spend the whole device for nothing.
//
//    • Yua's room, and the first time anything in this story looks back at the
//      player. It is the act's last scene, it is the top of the escalation
//      ladder, and it is where the buttons are named.
//
//  What act two therefore adds to the game, beyond its own script:
//
//    • The two silences, written out in full for the first time. One where the
//      machine room reaches her through a floor, one where his leg stops him
//      walking. Neither asks the player for anything and both can be clicked
//      straight past. Nothing is offered for sitting with them yet — the
//      endings need a blue or a green press on record before waiting means
//      anything, and there has not been one — so what these two are for, here,
//      is to teach the shape.
//
//    • Yua speaking to the player. Act three answers it with "still there",
//      act four has Haru do the same thing without knowing she got there first.
//
//  The verbs the script below is written in — Say, Narrate, Listen, Hold,
//  Place, Cel, DecideIdly — are in ActScriptWriter, along with everything about
//  writing the asset.
//
//  One note for whoever writes act three. Act four's file header says Yua has
//  overruled blue buttons "since act two". After this rewrite the first act
//  that can contain a refusal is act three, because act two's last scene is
//  where the buttons are introduced. The comment is wrong, the behaviour is
//  not, and it was left alone rather than edited from inside this task.
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

        // =====================================================================
        //  WHAT ACT ONE LEFT ON THE TABLE
        //
        //  Filled in from act one's actual text rather than from memory, which
        //  is the manual's rule for this block and the reason it is at the top
        //  of the file instead of in somebody's head.
        //
        //  Where act one stopped
        //      Saturday night, the corner with the pink juice machine, the
        //      streetlamps on and the bicycle back. She sent him home. The last
        //      thing either of them said was his own joke about a tree, in her
        //      mouth. Then two frames of her thinking: six days, nothing
        //      changed, and nothing is going to.
        //
        //  Said already, and no longer new
        //      They are "-pi" to each other and always were. Morita-sensei and
        //      class 1-A. The classroom window that would not shut, and that
        //      somebody fixed on the Friday. The pink juice machine on the
        //      corner, broken, which ate a hundred yen and was pronounced fine.
        //      Usagi Bakery and melon bread. Volume four of a manga nobody owns
        //      one to three of. The salty lychee that will not sell. Tofu, the
        //      classroom plant, named by a committee of one. The flowerbed at
        //      the playground that goes red in autumn. One classmate, once, who
        //      asked what "-pi" meant and was told to mind her own business.
        //
        //  Jokes the two of them built and now own
        //      "Somebody said."  ·  "It's a good tree."  ·  counting things at
        //      each other  ·  "…which is me."
        //
        //  What each of act one's ten dread moments was excused by
        //      Old friends. A better memory. A boy who apologises for weather.
        //      A girl who likes the air on a rainy morning. Every one of them
        //      had an ordinary sentence available, and six had that sentence
        //      planted in the script before the moment arrived.
        //
        //  Rungs act one burned
        //      One — knowing more than she should — and two — a reaction half a
        //      size wrong. Repeatedly, and nothing above them.
        //
        // ---------------------------------------------------------------------
        //  THE ESCALATION REGISTER
        //
        //  Used so far        1, 2
        //  Not yet used       3, 4, 5, 6
        //  This act goes to   5, by way of 3 and then 4, one rung at a time and
        //                     never two in the same scene.
        //
        //  Rung three, twice, in the first half: she corrects his account of
        //  himself and she is right, and nobody in the scene notices that this
        //  happened. Rung four, three times — twice in the café and once the
        //  day after: the gap opens between what she says out loud and what she is
        //  thinking, and the manual's own warning applies from here — rung four
        //  is not deniable any more, which is why it does not arrive until the
        //  narrative document has asked for the scene it arrives in.
        //
        //  Rung five is the last scene and nothing else in the act.
        //
        //  Rung six — the game doing something it should not, which is the
        //  refused blue button — belongs to act three. It cannot happen here.
        //  There is no button to refuse until twenty frames before this act
        //  ends.
        //
        // ---------------------------------------------------------------------
        //  WHAT THIS ACT PLANTS, AND WHO COLLECTS IT
        //
        //  The friend is never named            → act four, which opens on
        //                                         "you never said his name"
        //  The promise, given on the first ask  → act three keeps it, act four
        //                                         turns it around and points it
        //                                         at him
        //  "dark, and full of loud, sudden      → act six
        //   noises"
        //  Yua's second promise, asked for      → act five, where he says he
        //   lightly: tell nobody else             chose that story on purpose
        //  Haru drops the "-pi", once, mid       → act five, where he drops it
        //   sentence, and nobody reacts            and does not pick it back up
        //  "You are there."                     → act three's "still there"
        //  The rabbit with the worn left ear    → act five
        //  The crack in the mirror              → act five and six
        //  The flowerbed, still only green      → act three's spider lilies
        //
        //  Every one of those is two lines long at most and none of them is
        //  emphasised. A plant with weight on it gets marked by the player and
        //  the payoff is visible from three acts away.
        //
        // ---------------------------------------------------------------------
        //  THE CONTINUITY TABLE
        //
        //  Nothing enters the dialogue below unless it is here.
        //
        //  Season     The rainy season. Act one was April and the blossom; act
        //             three is autumn. This is June, and it rains on Monday, is
        //             hot and clear on Tuesday, and rains again on Wednesday.
        //  Days       1 Monday · 2 Tuesday · 3 Wednesday · 4 Thursday ·
        //             5 Sunday morning. Friday and Saturday are not in the act
        //             and nobody refers to them.
        //  Class      1-A, second floor. Morita-sensei.
        //  Clothes    School uniform, both of them, every day. There is no
        //             other sprite and it is never mentioned.
        //  Carrying   A school bag each. Haru also has an overdue library book
        //             from Monday morning until Thursday morning, when it goes
        //             back without being mentioned. Yua has two umbrellas on
        //             Monday, on purpose.
        //  Haru's leg Tuesday evening, four streets from the corner. He says
        //             the word "leg" and nothing else about it, ever.
        //  The sound  Wednesday, through the classroom floor. She says she has
        //             been frightened of it since she was small. He says he
        //             knows. Nobody asks anything.
        //  The plant  Tofu, on the classroom sill, which has doubled in size in
        //             the wet weather and is a peace lily whatever she says.
        //  The poster A rainy-season safety poster for the corridor. Her name
        //             is on it, his lettering is on it, and somebody draws on
        //             it by Thursday.
        //  The cat    Mochi. The cat with the collar she would not read in act
        //             one. There is a flyer about it on the roof fence on
        //             Tuesday, and the flyer has the name on it.
        //  The machine The same pink juice machine on the corner, still broken,
        //             still fine. The salty lychee row has finally been
        //             restocked with peach, which she takes badly.
        //  The café   Usagi Café, the one with the terrible umbrella stand.
        //             Matcha cake, two forks, boba for her and an iced matcha
        //             for him that he does not like and drinks anyway — and
        //             this time she does not have to tell him to.
        //  The friend Never named, in this act, by anybody. He had a pencil
        //             case with a broken zip. That is the only fact about him
        //             the act contains that is not about Haru.
        //  Third      Nobody. Act one had one classmate for one scene and spent
        //  person     her on the "-pi" suffix. This act has two people in it
        //             and no more, which is part of what makes it slower.
        // =====================================================================

        /// <summary>
        /// Five days, and the act gets quieter across all five.
        /// </summary>
        /// <remarks>
        /// Nine scenes where act one had twenty. Monday and Tuesday are two
        /// scenes each and are almost entirely warmth; Wednesday is two scenes
        /// and is the act; Thursday is two scenes of afterwards; Sunday is one
        /// room, one girl, and the player.
        /// </remarks>
        protected override void Write()
        {
            WriteMondayClassroom();
            WriteMondayCorridor();

            WriteTuesdayRooftop();
            WriteTuesdayCorner();

            WriteWednesdayClassroom();
            WriteWednesdayCafe();

            WriteThursdayClassroom();
            WriteThursdayPlatform();

            WriteSundayRoom();
        }

        // ---------------------------------------------------------------------
        //  Two things this act does often enough to name
        // ---------------------------------------------------------------------

        /// <summary>Characters per second for <see cref="InnerVoice"/> lines.</summary>
        /// <remarks>
        /// The rest of the game types at 45 and the asides at 30. Eighteen is
        /// slower than anything else in it, on purpose: inside her head the
        /// game gets heavy, and the player should feel the machine labouring
        /// before they work out why.
        /// </remarks>
        private const float InnerMonologueTypeSpeed = 18f;

        /// <summary>
        /// Yua thinking, with nobody to hear it.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Never written on its own. Every one of these sits between
        /// <see cref="BeginMonologue"/> and <see cref="EndMonologue"/>, which
        /// are what tell the player whose head they are in — the other person
        /// walks out of frame and the room goes quiet. A thought spoken with
        /// Haru standing right there, on the same name plate, in the same box,
        /// reads as a thing she said to his face, and that is the opposite of
        /// the scene: rung four IS the gap between her mouth and her head, so
        /// the gap has to be visible.
        /// </para>
        /// <para>
        /// An earlier draft marked them with brackets round the text instead.
        /// It was cheap, it looked like a stage direction, and it did not
        /// actually say she was alone. The staging does.
        /// </para>
        /// </remarks>
        private void InnerVoice(string english, string japanese, string persian)
        {
            Say(Speaker.Yua, Portrait.Unchanged, english, japanese, persian);
            Script[Script.Count - 1].TypeSpeed = InnerMonologueTypeSpeed;
        }

        /// <summary>
        /// The other person leaves the frame and the room goes away.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Four things at once, and they are one idea: whoever she is standing
        /// with fades out, the music goes, a heartbeat lands, and her face
        /// changes to the flat one. Everything after this is inside her until
        /// <see cref="EndMonologue"/> puts the room back.
        /// </para>
        /// <para>
        /// The face is an argument, not a setting, which is why it is a
        /// parameter. There are four of these in the act and they are four
        /// different women:
        /// </para>
        /// <para>
        /// The café, after the suicide — <see cref="Portrait.DeadEyes"/>, and
        /// this is the ONLY frame in the whole act that wears it. She has just
        /// found out he owns a grief she had no part in, and what is behind her
        /// eyes is not distress, it is nothing at all. Spending that face once
        /// is the whole reason it lands; a draft that used it for all four
        /// monologues turned the most frightening picture in the project into
        /// wallpaper.
        /// </para>
        /// <para>
        /// The café, when he describes a dark room full of sudden noises —
        /// <see cref="Portrait.Crying"/>. She is not blank here, she is
        /// terrified, and she is alone, so the face is the one nobody is ever
        /// allowed to see. It escalates out of the worried face she wears on
        /// his line, which is the only place in the act that one appears.
        /// </para>
        /// <para>
        /// The café, after she promises — <see cref="Portrait.Joyful"/>. She
        /// smiles all the way through working out what she just signed and then
        /// denies there was anything to sign. Denial with a smile on it is
        /// worse than a flat stare and it is not the same picture twice.
        /// </para>
        /// <para>
        /// Thursday — <see cref="Portrait.Joyful"/> again, and for the opposite
        /// reason. She is enjoying herself. Somebody who has got what she wanted
        /// looks like somebody who has got what she wanted.
        /// </para>
        /// <para>
        /// Stopping the music is the only ambient change this engine can make —
        /// there is no ambient layer, see the self-audit — and it is a fade
        /// rather than the design document's zero-millisecond cut, which is
        /// spent three times in the whole game and not here.
        /// </para>
        /// </remarks>
        private void BeginMonologue(Speaker leaving, Portrait face)
        {
            Exit(leaving);

            Hold(1.4f);

            StopMusic();

            Cue(SfxId.Heartbeat, 0.45f);

            Enter(Speaker.Yua, face);

            Hold(1.0f);
        }

        /// <summary>Gives the room, the music and the other person back.</summary>
        /// <param name="returning">Who walks back into frame.</param>
        /// <param name="yuaFace">The face she is wearing by the time he sees her.</param>
        /// <param name="theirFace">The face he comes back on.</param>
        private void EndMonologue(Speaker returning, Portrait yuaFace, Portrait theirFace)
        {
            Hold(1.2f);

            Enter(Speaker.Yua, yuaFace);

            SetMusic(MusicTrack);

            Enter(returning, theirFace);

            Hold(1.2f);
        }

        // =====================================================================
        //  MONDAY — the rainy season arrives
        //
        //  Dread budget: 1, and it is in the second scene. The first scene of
        //  the act has none at all, for the same reason the first fifty frames
        //  of the game had none: the player has just come back from a title
        //  card and has to be given a reason to want to be here again.
        // =====================================================================

        /// <summary>
        /// Monday morning. Rain, an umbrella that is not one, a plant that has
        /// had a very good weekend, and a poster.
        /// </summary>
        /// <remarks>
        /// The longest purely happy scene in the act and deliberately the first
        /// thing in it. Everything the player is about to be asked to care
        /// about on Wednesday is bought here.
        /// </remarks>
        private void WriteMondayClassroom()
        {
            Place(
                Backgrounds.ClassroomRainy,
                "The classroom", "教室", "کلاس درس");

            Hold(2.4f);

            Narrate(
                "The rainy season came in on Sunday night, and by Monday morning the whole school smelled of wet socks.",
                "梅雨は日曜の夜に来た。月曜の朝には、学校じゅうが濡れた靴下のにおいだった。",
                "فصلِ بارون یکشنبه‌شب اومد، و صبحِ دوشنبه کلِ مدرسه بوی جورابِ خیس می‌داد.");

            Narrate(
                "Three weeks since the first day of term. Long enough for a desk to become your desk, and for a boy by the window to become the boy by the window.",
                "始業式から三週間。机が自分の机になり、窓際の男の子が「あの子」になるには十分な時間だった。",
                "سه هفته از روز اول گذشته بود. اون‌قدر که یه میز بشه میزِ تو، و یه پسرِ کنارِ پنجره بشه اون پسرِ کنارِ پنجره.");

            Enter(Speaker.Haru, Portrait.Neutral);

            Cue(SfxId.ChairScrape, 0.45f);

            Enter(Speaker.Yua, Portrait.Angry);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Haru-pi. That is not an umbrella.",
                "ハルぴ。それ、傘じゃない。",
                "هارو‌پی. اون چتر نیست.");

            Say(Speaker.Haru, Portrait.Neutral,
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

            Say(Speaker.Yua, Portrait.Neutral,
                "It kept your head off. Your entire left side is a different colour to the rest of you.",
                "頭だけね。左半分だけ、色が違うんだけど。",
                "فقط سرتو گرفت. کلِ سمتِ چپت یه رنگِ دیگه‌ست.");

            Say(Speaker.Haru, Portrait.Shy,
                "…That part is true.",
                "……それは、そう。",
                "...این یکی راسته.");

            Hold(1.2f);

            Narrate(
                "She had two umbrellas with her.",
                "彼女は傘を二本持っていた。",
                "دو تا چتر همراهش بود.");

            SayWithSound(Speaker.Yua, Portrait.Joyful, SfxId.GiftBox, 0.40f,
                "Take the yellow one.",
                "黄色いほう、持ってて。",
                "زردی رو وردار.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Why do you have two umbrellas?",
                "なんで傘二本あるの？",
                "چرا دو تا چتر داری؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Because you have a sock.",
                "ハルぴが靴下持ってるから。",
                "چون تو یه جوراب داری.");

            Hold(1.4f);

            // The argument he wins, and he is right about it.
            Narrate(
                "Tofu had doubled in size over the weekend. Nobody had done anything to it.",
                "豆腐は週末のあいだに倍になっていた。誰も何もしていない。",
                "توفو آخرِ هفته دو برابر شده بود. کسی هیچ کاری باهاش نکرده بود.");

            Say(Speaker.Yua, Portrait.Angry,
                "Look at it. Look at what three days of weather has done to it.",
                "見てよ。三日雨が降っただけでこれ。",
                "نگاش کن. ببین سه روز هوا چه بلایی سرش آورده.");

            Say(Speaker.Haru, Portrait.Neutral,
                "It likes the wet.",
                "湿気が好きなんだよ。",
                "خیسی رو دوست داره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It has been on that sill since March doing absolutely nothing, and now, after one weekend, it has decided to become a tree.",
                "三月からずっとあの窓際で何もしてなかったのに、週末ひとつで木になろうとしてる。",
                "از مارس رو اون طاقچه‌ست و هیچ غلطی نکرده، حالا بعدِ یه آخرِ هفته تصمیم گرفته درخت بشه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It is a peace lily.",
                "スパティフィラムだよ。",
                "اون گلِ صلحه.");

            Say(Speaker.Yua, Portrait.Angry,
                "It is a tree.",
                "木だってば。",
                "درخته.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It is in a plastic pot, on a windowsill, indoors. It is a peace lily.",
                "プラスチックの鉢で、窓際で、室内。スパティフィラム。",
                "تو یه گلدونِ پلاستیکیه، رو طاقچه، تو اتاق. گلِ صلحه.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Neutral,
                "It is a tree with low expectations.",
                "……夢の小さい木。",
                "درختیه که توقعِ زیادی نداره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That I will accept.",
                "それは認める。",
                "این یکی رو قبول دارم.");

            Hold(1.6f);

            // Yua, doing something that has nothing to do with Haru.
            Say(Speaker.Yua, Portrait.Neutral,
                "Morita-sensei put my name on the corridor poster. For the rainy season.",
                "森田先生が、廊下のポスターにあたしの名前書いた。梅雨のやつ。",
                "خانمِ موریتا اسمِ منو زد رو پوسترِ راهرو. مالِ فصلِ بارون.");

            Say(Speaker.Haru, Portrait.Neutral,
                "What does it have to say?",
                "何て書くの？",
                "چی باید توش بنویسی؟");

            Say(Speaker.Yua, Portrait.Angry,
                "Do not run on wet floors.",
                "「濡れた床は走らない」。",
                "روی زمینِ خیس ندوید.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That is a good message.",
                "いい標語じゃない。",
                "پیامِ خوبیه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It is a terrible message. It is a poster whose entire purpose is to stop people doing the only enjoyable thing that has ever happened in a corridor.",
                "最悪の標語。廊下で唯一楽しいことを、まるごと禁止するためだけのポスターだよ。",
                "پیامِ افتضاحیه. یه پوستره که کلِ هدفش اینه جلوی تنها کارِ باحالی رو بگیره که تا حالا تو یه راهرو اتفاق افتاده.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You are going to draw someone falling over, aren't you.",
                "転んでる人、描く気でしょ。",
                "می‌خوای یکی رو بکشی که داره می‌افته، نه؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "I am going to draw someone falling over beautifully.",
                "すっごくきれいに転んでる人を描く。",
                "می‌خوام یکی رو بکشم که داره خیلی قشنگ می‌افته.");

            Hold(1.2f);

            // The real decision, lost quietly, in the middle of a joke. Nobody
            // names it and the book stays in his bag until Thursday.
            Say(Speaker.Yua, Portrait.Neutral,
                "You are doing the lettering. At lunch.",
                "字はハルぴが書いて。お昼に。",
                "خط‌نویسیش با توئه. زنگِ ناهار.");

            Say(Speaker.Haru, Portrait.Neutral,
                "I have to take a book back at lunch. It is four days late.",
                "お昼は本返しに行くんだ。四日遅れてる。",
                "زنگِ ناهار باید یه کتاب رو پس بدم. چهار روز دیر کرده.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The lettering takes twenty minutes.",
                "字なんて二十分でしょ。",
                "خط‌نویسیش بیست دقیقه‌ست.");

            Hold(1.0f);

            Say(Speaker.Haru, Portrait.Neutral,
                "…Okay.",
                "……うん。",
                "...باشه.");

            Hold(1.4f);

            DecideIdly(
                "Let her tell you about the poster", "ポスターの話をさせる", "بذار از پوستر بگه",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Joyful,
                        "It is going to say two words, and the two words are going to be mine.",
                        "書くのは二文字。あたしが決めた二文字。",
                        "قراره دو تا کلمه روش باشه، و اون دو تا کلمه مالِ منن.");

                    Say(Speaker.Yua, Portrait.Angry,
                        "Morita-sensei wrote a whole sentence on the form. Wet floors, appropriate footwear, all of it.",
                        "森田先生、用紙に一文まるごと書いてた。濡れた床、適切な履物、ぜんぶ。",
                        "خانمِ موریتا تو فرم یه جمله‌ی کامل نوشته بود. زمینِ خیس، کفشِ مناسب، همه‌شو.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "I am not putting the words appropriate footwear on a wall.",
                        "「適切な履物」なんて壁に貼らないから。",
                        "من عبارتِ «کفشِ مناسب» رو نمی‌چسبونم به دیوار.");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "Nobody in the history of this school has read a poster with the word footwear on it.",
                        "この学校の歴史上、「履物」って書いてあるポスターを読んだ人はいない。",
                        "تو کلِ تاریخِ این مدرسه هیچ‌کس پوستری رو که کلمه‌ی «کفش» توش باشه نخونده.");
                },
                "Let her complain about locker twelve", "下駄箱の文句を言わせる", "بذار از جاکفشیِ دوازده غر بزنه",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Angry,
                        "Locker twelve still has shoes in it.",
                        "十二番、まだ靴入ってる。",
                        "تو جاکفشیِ دوازده هنوز کفش هست.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Since April. Two months. The same pair, facing the same way.",
                        "四月から。二ヶ月。同じ靴が、同じ向きで。",
                        "از آوریل. دو ماه. همون یه جفت، به همون سمت.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "If you owned a pair of shoes you would notice, somewhere inside two months, that you were not wearing them.",
                        "自分の靴なら、二ヶ月のうちどこかで「履いてないな」って気づくはずでしょ。",
                        "اگه یه جفت کفش مالِ تو باشه، یه جایی تو دو ماه متوجه می‌شی که پات نیستن.");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "So they are nobody's. Which means this school is keeping a pair of shoes for nobody.",
                        "つまり誰のものでもない。この学校、誰のものでもない靴を預かってる。",
                        "پس مالِ هیچ‌کسن. یعنی این مدرسه داره واسه‌ی هیچ‌کس یه جفت کفش نگه می‌داره.");
                });

            // Both roads land here, and Haru answers whichever one he got.
            Say(Speaker.Haru, Portrait.Neutral,
                "You have thought about this more than the situation needs.",
                "それ、必要以上に考えてるよね。",
                "بیشتر از اون چیزی که لازمه بهش فکر کردی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That is what a committee is for.",
                "委員ってそういうものだから。",
                "کمیته واسه‌ی همینه دیگه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You are the committee.",
                "委員、結愛ぴ一人でしょ。",
                "کمیته خودتی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I am an excellent committee.",
                "とても優秀な委員です。",
                "من کمیته‌ی خیلی خوبی‌ام.");

            Hold(1.2f);

            SayWithSound(Speaker.Yua, Portrait.Joyful, SfxId.SchoolBell, 0.55f,
                "That is us. Second floor.",
                "はい、行くよ。二階。",
                "خب، ما رفتیم. طبقه‌ی دوم.");

            Narrate(
                "The rain kept doing what rain does. Somewhere below them a door shut too hard and she was quiet for about half a second.",
                "雨は雨のすることを続けていた。下の階でドアが強く閉まり、彼女は半秒だけ黙った。",
                "بارون کارِ همیشگی‌شو می‌کرد. یه جایی طبقه‌ی پایین دری محکم بسته شد و یوآ حدودِ نیم ثانیه ساکت شد.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Bring the yellow one.",
                "黄色いの、持ってきて。",
                "زردی رو بیار.");

            Hold(1.8f);
        }

        /// <summary>
        /// Monday, after school. The rain has stopped and the corridor is full
        /// of umbrellas standing open along the wall.
        /// </summary>
        /// <remarks>
        /// Rung three, first use: she corrects his account of himself and she
        /// is right. It is placed where it is because the manual's version of
        /// this is not a moment of astonishment — nobody asks how she knows,
        /// and nothing is explained. He concedes the fact and then argues about
        /// what it is worth, which is what a person actually does, and it is why
        /// the frame gets past the player.
        /// </remarks>
        private void WriteMondayCorridor()
        {
            ClearStage();

            Place(
                Backgrounds.CorridorSunset,
                "The corridor, after school", "放課後の廊下", "راهرو، بعد از مدرسه");

            Hold(2.2f);

            Narrate(
                "It stopped raining at about two, and by four the corridor had umbrellas standing open all along the wall, drying, and nobody could find theirs.",
                "二時ごろ雨が上がって、四時には廊下じゅうの壁際に傘が開いたまま並んでいた。誰も自分のを見つけられなかった。",
                "حدودِ ساعت دو بارون بند اومد، و ساعتِ چهار کلِ راهرو پر بود از چترای باز که کنارِ دیوار خشک می‌شدن، و هیچ‌کس مالِ خودشو پیدا نمی‌کرد.");

            Enter(Speaker.Yua, Portrait.Angry);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Unchanged,
                "They all look the same.",
                "全部おんなじに見える。",
                "همه‌شون شبیه همن.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Third stand. Ours are the third stand.",
                "三つ目の傘立て。うちのは三つ目。",
                "جاچتریِ سوم. مالِ ما تو سومیه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "You do not know that.",
                "そんなの分かんないでしょ。",
                "تو از کجا می‌دونی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "We came in the side door. The side door is the third stand.",
                "横の入口から入ったから。横の入口は三つ目。",
                "از درِ کناری اومدیم تو. درِ کناری می‌شه جاچتریِ سوم.");

            Hold(1.0f);

            Cue(SfxId.CanDrop, 0.50f);

            Narrate(
                "Both of them were in the third stand.",
                "二本とも三つ目にあった。",
                "هر دوتاشون تو جاچتریِ سوم بودن.");

            Hold(1.4f);

            // Setting Wednesday up, three scenes early, in the middle of an
            // argument about a drink. Nobody treats it as a plan.
            Say(Speaker.Yua, Portrait.Joyful,
                "Wednesday. The café. The one with the terrible umbrella stand.",
                "水曜、喫茶店ね。傘立てがひどいところ。",
                "چهارشنبه. کافه. همون که جاچتریش افتضاحه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "The umbrella stand there is very bad.",
                "あそこの傘立て、本当にひどい。",
                "جاچتریِ اونجا واقعاً بده.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You are having the iced matcha.",
                "ハルぴは冷たい抹茶ね。",
                "تو ماچای سرد می‌خوری.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I have never once liked iced matcha.",
                "冷たい抹茶、一回も好きだったことない。",
                "من حتی یه بارم از ماچای سرد خوشم نیومده.");

            // ◆ Dread moment 1 — rung three.
            Say(Speaker.Yua, Portrait.Neutral,
                "You liked it once.",
                "一回好きだったよ。",
                "یه بار خوشت اومد.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I did not.",
                "好きじゃなかった。",
                "نه نیومد.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "There was far too much sugar in it, and you drank the entire thing, and then you were strange for an hour.",
                "砂糖が入りすぎてて、全部飲んで、そのあと一時間くらい変だった。",
                "خیلی توش شکر بود، و کلِّشو خوردی، و بعدش یه ساعت یه‌جوری بودی.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Shy,
                "…That does not count.",
                "……それは、カウントしないよ。",
                "...اون حساب نیست.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It counts.",
                "カウントする。",
                "حساب می‌شه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It counts against me.",
                "僕に不利にカウントされてる。",
                "به ضررِ من حساب می‌شه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Everything counts against you. That is what counting is.",
                "全部ハルぴに不利だよ。数えるってそういうことでしょ。",
                "همه‌چی به ضررِ توئه. شمردن یعنی همین.");

            Hold(1.6f);

            Narrate(
                "The light in that corridor arrives late and leaves slowly.",
                "この廊下の光は遅れて来て、ゆっくり去る。",
                "نورِ اون راهرو دیر می‌رسه و آروم می‌ره.");

            Say(Speaker.Haru, Portrait.Neutral,
                "I still have that book.",
                "まだ本、持ったままだ。",
                "هنوز اون کتاب دستمه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "The library shuts at five.",
                "図書室、五時までだよ。",
                "کتابخونه پنج می‌بنده.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It shuts at five.",
                "五時まで。",
                "پنج می‌بنده.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Then walk me to the corner first.",
                "じゃあ先に角まで送って。",
                "پس اول تا سرِ نبش منو برسون.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Joyful,
                "…Okay.",
                "……うん。",
                "...باشه.");

            Hold(2.2f);

            Narrate(
                "They went down the side stairs. The book stayed where it was.",
                "二人は横の階段を下りていった。本はそのままだった。",
                "از پله‌های کناری رفتن پایین. کتاب همون‌جا موند.");

            Hold(1.8f);
        }

        // =====================================================================
        //  TUESDAY — the rain breaks and the heat arrives
        //
        //  Dread budget: 2, both of them in the evening scene. The roof is the
        //  last long stretch of the act with nothing wrong in it anywhere, and
        //  it is long on purpose. Wednesday costs more if Tuesday was good.
        // =====================================================================

        /// <summary>
        /// Tuesday lunchtime. The roof, the heat, the shimmer over the flat
        /// roofs of the town, and a flyer tied to the fence.
        /// </summary>
        /// <remarks>
        /// The act is called Maboroshi and this is the only place the word is
        /// visible, in the one form nobody has to name: heat coming off a city
        /// in June, making solid things move. Neither of them calls it anything
        /// and the title card was four scenes ago.
        /// </remarks>
        private void WriteTuesdayRooftop()
        {
            Maybe(MachineRoom, 0.20f);

            ClearStage();

            Place(
                Backgrounds.RooftopDay,
                "The roof", "屋上", "پشت‌بوم");

            Hold(2.4f);

            Narrate(
                "The rain broke overnight and Tuesday came up hot. By lunch the roof was the only place in the building with any air in it.",
                "夜のうちに雨は上がり、火曜は暑くなった。昼には、校舎で空気があるのは屋上だけだった。",
                "شب بارون بند اومد و سه‌شنبه گرم دراومد. تا ظهر پشت‌بوم تنها جایی تو کلِ ساختمون بود که هوا داشت.");

            Narrate(
                "Over the flat roofs of the town the air was moving in a way air does not usually move, and the water tanks on top of them were not quite standing still.",
                "町の平らな屋根の上で、空気がふだんとは違う動き方をしていた。屋根の上の貯水タンクは、じっとしていなかった。",
                "روی پشت‌بومای صافِ شهر هوا یه جوری تکون می‌خورد که معمولاً تکون نمی‌خوره، و منبع‌های آبِ روشون درست سرِ جاشون نبودن.");

            Enter(Speaker.Haru, Portrait.Neutral);

            Narrate(
                "He got to the bench first and put his bag down on the sunny end of it.",
                "彼が先にベンチに着いて、日の当たる側に鞄を置いた。",
                "اول اون به نیمکت رسید و کیفشو گذاشت رو اون سرش که آفتاب داشت.");

            Enter(Speaker.Yua, Portrait.Joyful);

            Say(Speaker.Yua, Portrait.Unchanged,
                "The cicadas are out.",
                "蝉、鳴いてる。",
                "زنجره‌ها دراومدن.");

            Say(Speaker.Haru, Portrait.Neutral,
                "They are early.",
                "早いよね。",
                "زودن.");

            Say(Speaker.Yua, Portrait.Angry,
                "They are three weeks early. They are three weeks early and nobody is doing anything about it, and in three weeks, when they are supposed to arrive, there will be a second lot, and then there will be two lots of them, all summer, forever, and this is how it starts.",
                "三週間も早い。三週間早いのに誰も何もしない。で、本来の時期にもう一群来て、夏じゅうずっと二群になる。永遠に。こうやって始まるんだよ。",
                "سه هفته زودن. سه هفته زودن و هیچ‌کس هیچ کاری نمی‌کنه، و سه هفته‌ی دیگه که قرار بوده بیان، یه دسته‌ی دوم میاد، و بعدش دو دسته می‌شن، کلِ تابستون، تا ابد، و شروعش همینه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "This is how what starts?",
                "何が始まるの？",
                "شروعِ چی؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "I have not decided yet. Something.",
                "まだ決めてない。何かが。",
                "هنوز تصمیم نگرفتم. یه چیزی.");

            Hold(1.4f);

            // The argument he wins.
            Say(Speaker.Haru, Portrait.Neutral,
                "That wobble over the roofs is the vents.",
                "屋根の上のあの揺れ、換気口だよ。",
                "اون لرزشِ روی پشت‌بوما مالِ دریچه‌هاست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It is the heat.",
                "暑いからでしょ。",
                "مالِ گرماست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It is only over the flat roofs. The flat roofs are the ones with vents on them.",
                "平らな屋根の上だけ。換気口があるのは平らな屋根。",
                "فقط رو پشت‌بومای صافه. پشت‌بومای صاف همونان که دریچه دارن.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Neutral,
                "The heat comes out of the vents.",
                "……換気口から熱が出てるんでしょ。",
                "گرما از تو دریچه‌ها میاد بیرون.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You are agreeing with me and calling it disagreeing.",
                "同意しながら反論って言ってる。",
                "داری باهام موافقت می‌کنی و اسمشو می‌ذاری مخالفت.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It is a skill.",
                "才能だよ。",
                "یه مهارته.");

            Hold(1.8f);

            Narrate(
                "Somebody had tied a flyer to the wire fence with string, and the wind had already had most of one corner off it.",
                "誰かが紙をひもで金網に結んでいた。角のひとつは、もう風にほとんど持っていかれていた。",
                "یکی با نخ یه اعلامیه بسته بود به توری، و باد تا حالا یه گوشه‌شو کنده بود.");

            DecideIdly(
                "Let her read the flyer", "貼り紙を読ませる", "بذار اعلامیه رو بخونه",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Neutral,
                        "It is the bakery cat. Somebody has lost the bakery cat.",
                        "パン屋の猫だ。いなくなったって。",
                        "گربه‌ی نونواییه. یکی گربه‌ی نونوایی رو گم کرده.");

                    Say(Speaker.Yua, Portrait.Angry,
                        "And it has a name. Of course it has a name. It is written on a piece of paper, on a fence.",
                        "名前まである。あるに決まってる。紙に書いて、金網に結んである。",
                        "و اسم داره. معلومه که اسم داره. رو یه تیکه کاغذ، بسته به توری، نوشتنش.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Mochi.",
                        "もち。",
                        "موچی.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "It had a collar on in April and I did not read it, and I did not read it on purpose.",
                        "四月に首輪してた。読まなかった。わざと読まなかった。",
                        "آوریل قلاده داشت و من نخوندمش، و عمداً نخوندمش.");

                    Say(Speaker.Yua, Portrait.Neutral,
                        "Because once you read it, it is somebody's.",
                        "読んだ瞬間、誰かのものになるから。",
                        "چون تا بخونیش، مالِ یکی می‌شه.");
                },
                "Let her complain about the heat", "暑さの文句を言わせる", "بذار از گرما غر بزنه",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Angry,
                        "It was raining yesterday. Yesterday I had wet socks. Today I am being cooked on a roof.",
                        "昨日は雨だった。昨日は靴下が濡れてた。今日は屋上で焼かれてる。",
                        "دیروز بارون می‌اومد. دیروز جورابام خیس بود. امروز دارم رو پشت‌بوم پخته می‌شم.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "There is no version of this month that is not damp.",
                        "この月に、湿ってない日なんてない。",
                        "هیچ نسخه‌ای از این ماه نیست که نمور نباشه.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Wet outside, or wet inside. Those are the two options.",
                        "外が濡れてるか、内側が濡れてるか。選択肢は二つ。",
                        "یا بیرون خیسه یا تو. همین دو تا گزینه هست.");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "I am choosing neither. I am filing a complaint.",
                        "どっちも選ばない。苦情を出す。",
                        "هیچ‌کدومو انتخاب نمی‌کنم. شکایت می‌دم.");
                });

            // Both roads land here.
            Say(Speaker.Haru, Portrait.Neutral,
                "Mm.",
                "うん。",
                "هوم.");

            Say(Speaker.Yua, Portrait.Angry,
                "That is not a response.",
                "それ、返事じゃない。",
                "این جواب نیست.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It is my response.",
                "僕の返事だよ。",
                "جوابِ منه.");

            Hold(1.6f);

            Cue(SfxId.Petal, 0.40f);

            Narrate(
                "The wind came across the roof and took the last corner of the flyer with it, and the string held.",
                "風が屋上を渡り、貼り紙の最後の角を持っていった。ひもは残った。",
                "باد از رو پشت‌بوم رد شد و آخرین گوشه‌ی اعلامیه رو هم برد، و نخ سرِ جاش موند.");

            // Anticipation, and nobody remarks on it — including the narrator,
            // who is only allowed to say where the bag is.
            Say(Speaker.Haru, Portrait.Neutral,
                "I will come up again tomorrow. It is better than the classroom.",
                "明日も来ようかな。教室よりいい。",
                "فردا هم میام بالا. از کلاس بهتره.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Tomorrow it rains again. The classroom.",
                "明日はまた雨。教室ね。",
                "فردا دوباره بارون میاد. کلاس.");

            SayWithSound(Speaker.Haru, Portrait.Joyful, SfxId.SchoolBell, 0.45f,
                "…The classroom.",
                "……教室ね。",
                "...کلاس.");

            Hold(2.2f);
        }

        /// <summary>
        /// Tuesday evening, the corner with the pink machine, and the first of
        /// the act's two silences.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The leg. The design document asks for it as something that happens
        /// on the way home now and then, with no explanation offered and none
        /// requested, and that is exactly and only what is written: he says the
        /// word, he says nothing after it, and she waits.
        /// </para>
        /// <para>
        /// Written as an episode rather than an instant — see BeginChildhood —
        /// so that a player who stops anywhere inside it is treated the same as
        /// one who stopped on the first line of it. Nothing is offered for the
        /// waiting yet, because nothing can be: the endings read a count of
        /// blue and green presses, and there have not been any. What this is
        /// for is the shape.
        /// </para>
        /// <para>
        /// Rung three's second use is placed immediately after it, on purpose.
        /// She breaks a silence about his leg by correcting him about his own
        /// body, and neither of them notices that either of those things
        /// happened.
        /// </para>
        /// </remarks>
        private void WriteTuesdayCorner()
        {
            ClearStage();

            Place(
                Backgrounds.VendingStreetDay,
                "The corner", "角の道", "سرِ نبش");

            Hold(2.2f);

            Narrate(
                "The heat stayed on into the evening. Somebody up the road had watered a doorstep and the whole street smelled of wet concrete.",
                "暑さは夕方まで残った。通りの先で誰かが玄関先に水を撒いたらしく、道じゅうが濡れたコンクリートのにおいだった。",
                "گرما تا عصر موند. یکی اون بالای خیابون جلوی درشو آب پاشیده بود و کلِ کوچه بوی بتنِ خیس می‌داد.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Angry,
                "It is gone.",
                "……なくなってる。",
                "نیست.");

            Say(Speaker.Haru, Portrait.Neutral,
                "What is gone?",
                "何が？",
                "چی نیست؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The salty lychee is gone. The whole row is peach.",
                "塩ライチがなくなってる。一列ぜんぶ桃。",
                "لیچیِ نمکی نیست. کلِ ردیف شده هلو.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Somebody bought them.",
                "誰かが買ったんだよ。",
                "یکی خریدشون.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Nobody bought them. Somebody stood exactly here, in front of this machine, with every other flavour in the world available to them, and pressed the salty lychee. Twice. That did not happen.",
                "誰も買ってない。誰かがここに立って、この機械の前で、世界中の味が選べる状態で、塩ライチを押した。二回。そんなことは起きてない。",
                "هیچ‌کس نخریدشون. یکی دقیقاً همین‌جا وایساده، جلوی همین دستگاه، با اینکه هر طعمِ دیگه‌ای تو دنیا جلوش بوده، و لیچیِ نمکی رو زده. دو بار. همچین چیزی اتفاق نیفتاده.");

            Say(Speaker.Haru, Portrait.Neutral,
                "They restocked it.",
                "補充しただけ。",
                "شارژش کردن.");

            Hold(1.0f);

            Say(Speaker.Yua, Portrait.Angry,
                "…They restocked it.",
                "……補充しただけか。",
                "...شارژش کردن.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You are upset that it stopped losing.",
                "負けなくなったのが悔しいんだ。",
                "ناراحتی که دیگه داره نمی‌بازه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "I liked it better when it was losing.",
                "負けてるほうが好きだった。",
                "وقتی می‌باخت بیشتر دوستش داشتم.");

            Cue(SfxId.CanDrop, 0.55f);

            Narrate(
                "She bought a peach one anyway.",
                "それでも彼女は桃を買った。",
                "بازم یه هلو خرید.");

            Hold(1.8f);

            Narrate(
                "They started walking. Four streets on, he stopped.",
                "二人は歩き出した。四つ角を四つ過ぎて、彼は足を止めた。",
                "راه افتادن. چهار تا خیابون بعد، ایستاد.");

            // The wince goes on before the episode opens, not inside it.
            // Inside, Say remaps him to his nine-year-old self, and a pose on
            // that speaker would swap the picture for a stand-in silhouette of
            // a child — see VisualNovelStage.ShowCharacter. Every line between
            // BeginChildhood and EndChildhood is therefore Unchanged.
            Enter(Speaker.Haru, Portrait.Injured);

            // The episode, not the instant. See BeginChildhood.
            BeginChildhood();

            // ◆ Dread moment 2 — the leg.
            SayWithSound(Speaker.Haru, Portrait.Unchanged, SfxId.LegAche, 0.80f,
                "Sorry. Give me a moment. My leg.",
                "ごめん。少しだけ。……脚が。",
                "ببخشید. یه لحظه وایسا. پام.");

            Narrate(
                "He did not say anything after that.",
                "そのあと彼は何も言わなかった。",
                "بعدش هیچی نگفت.");

            ChildVoice(Speaker.HaruChild,
                "It is fine. I can still walk.",
                "平気。まだ歩けるよ。",
                "چیزی نیست. هنوز می‌تونم راه برم.");

            Hold(2.2f);

            Listen(
                "Yua waited. She did not sit down. A car came up the road, went past the two of them, and turned at the top.",
                "結愛は待った。座りもしなかった。車が一台、坂を上ってきて、二人の横を過ぎ、上の角を曲がっていった。",
                "یوآ منتظر موند. ننشست. یه ماشین از خیابون بالا اومد، از کنارِ اون دوتا رد شد و سرِ بالا پیچید.");

            Hold(2.8f);

            EndChildhood();

            Say(Speaker.Haru, Portrait.Neutral,
                "All right. Let us go.",
                "……うん。行こう。",
                "خب. بریم.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Mm.",
                "うん。",
                "هوم.");

            Hold(1.6f);

            Say(Speaker.Haru, Portrait.Shy,
                "Sorry. I am slow.",
                "ごめん。遅くて。",
                "ببخشید. کندم.");

            // ◆ Dread moment 3 — rung three, second use.
            Say(Speaker.Yua, Portrait.Neutral,
                "You are not slow. You have always walked like that.",
                "遅くない。ずっとその歩き方でしょ。",
                "کند نیستی. همیشه همین‌جوری راه می‌ری.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You mean I have always been slow.",
                "つまり、ずっと遅いってことじゃん。",
                "یعنی همیشه کند بودم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You used to run everywhere.",
                "昔はどこでも走ってた。",
                "قبلاً همه‌جا می‌دویدی.");

            Hold(1.6f);

            Say(Speaker.Haru, Portrait.Neutral,
                "…Did I.",
                "……そうだったかな。",
                "...می‌دویدم؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "Constantly. It was exhausting to look at.",
                "ずっと。見てるだけで疲れた。",
                "دائم. نگاه کردنش خسته‌کننده بود.");

            Hold(2.0f);

            Say(Speaker.Haru, Portrait.Neutral,
                "That machine still owes me a hundred yen.",
                "あの機械、まだ百円借りてる。",
                "اون دستگاه هنوز صد ین به من بدهکاره.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It does not owe you anything. It is fine.",
                "何も借りてない。あれは大丈夫。",
                "هیچی به تو بدهکار نیست. اون سالمه.");

            Hold(1.4f);

            Narrate(
                "They walked the rest of it the way they always do. Half a step apart, and matching.",
                "残りの道は、いつもどおりに歩いた。半歩あけて、歩幅は同じで。",
                "بقیه‌ی راهو مثلِ همیشه رفتن. نیم قدم فاصله، و هم‌قدم.");

            Hold(2.2f);
        }

        // =====================================================================
        //  WEDNESDAY — the act
        //
        //  Dread budget: 3. One in the classroom, two in the café, and the
        //  rung goes up exactly once across the whole day.
        // =====================================================================

        /// <summary>
        /// Wednesday morning. The rain is back, and the second of the act's two
        /// silences comes up through the floor.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The design document's other recurring event: the sound of the
        /// machine room reaches Yua, she says out loud and without deciding to
        /// that she has been frightened of it since she was small, and Haru
        /// says he knows. She does not ask how, and that is the entire point of
        /// the moment — the manual's forbidden pattern number one is
        /// astonishment followed by explanation, and the version that frightens
        /// is the one where nobody is astonished at all.
        /// </para>
        /// <para>
        /// The scene also has to say the word "café" out loud before the
        /// background changes, which is the scene-lock rule, and it has to lose
        /// him the library book for the third and last time.
        /// </para>
        /// </remarks>
        private void WriteWednesdayClassroom()
        {
            ClearStage();

            Place(
                Backgrounds.ClassroomRainy,
                "The classroom", "教室", "کلاس درس");

            Hold(2.4f);

            Narrate(
                "It came back in the night, harder than Monday, and by the morning break the windows had gone the colour of an old photograph.",
                "雨は夜のうちに戻ってきた。月曜より強く。中休みには、窓は古い写真の色になっていた。",
                "شب دوباره اومد، سنگین‌تر از دوشنبه، و تا زنگِ تفریح پنجره‌ها رنگِ یه عکسِ قدیمی رو گرفته بودن.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Joyful,
                "The poster is up.",
                "ポスター、貼ったよ。",
                "پوستر رفت بالا.");

            Say(Speaker.Haru, Portrait.Neutral,
                "By the stairs?",
                "階段のとこ？",
                "کنارِ پله‌ها؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "By the stairs. Which is where people run.",
                "階段のとこ。走るとこでしょ。",
                "کنارِ پله‌ها. همون‌جایی که مردم می‌دون.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Your falling-over person is very good.",
                "転んでる人、すごく上手だった。",
                "اون آدمی که داره می‌افته خیلی خوب شده.");

            Say(Speaker.Yua, Portrait.Joyful,
                "She is falling over beautifully. I told you.",
                "きれいに転んでるでしょ。言ったじゃん。",
                "داره قشنگ می‌افته. بهت گفتم.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Your lettering is better than mine.",
                "字も、僕のより結愛ぴのほうが上手い。",
                "خطِ تو از مالِ من بهتره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "My lettering is not on it. That is all yours.",
                "あたし、字書いてないよ。ぜんぶハルぴ。",
                "خطِ من روش نیست. همه‌ش مالِ توئه.");

            Say(Speaker.Haru, Portrait.Shy,
                "…Oh.",
                "……あ。",
                "...آها.");

            Hold(1.6f);

            Narrate(
                "Something turned over under the floor, once, and then kept turning.",
                "床の下で何かが一度回り、そのまま回り続けた。",
                "یه چیزی زیرِ کف یه بار چرخید، و بعد همین‌جوری چرخید.");

            Cue(SfxId.BoilerRoom, 0.55f);

            Hold(2.0f);

            // Her face goes before the episode opens, for the same reason his
            // does on Tuesday: inside it, Say is speaking as a nine-year-old.
            Enter(Speaker.Yua, Portrait.Sad);

            BeginChildhood();

            // ◆ Dread moment 4 — the machine room.
            SayWithSound(Speaker.Yua, Portrait.Unchanged, SfxId.Heartbeat, 0.70f,
                "I hate that sound.",
                "……その音、嫌い。",
                "از این صدا بدم میاد.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I have been frightened of the boiler room since I was small.",
                "小さいころから、ボイラー室の音が怖い。",
                "من از صدای موتورخونه از بچگیم می‌ترسم.");

            Narrate(
                "She said it to the window and not to him.",
                "彼女は窓に向かって言った。彼にではなく。",
                "به پنجره گفتش، نه به هارو.");

            Say(Speaker.Haru, Portrait.Unchanged,
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
                "The sound went on under the floor. Neither of them moved, and the rest of the room carried on being a classroom.",
                "音は床の下で続いた。二人とも動かず、教室の残りは教室のままだった。",
                "صدا زیرِ کف ادامه داشت. هیچ‌کدومشون تکون نخوردن، و بقیه‌ی اتاق به کلاس بودنش ادامه داد.");

            Hold(2.8f);

            EndChildhood();

            Enter(Speaker.Yua, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Anyway. The café. After school.",
                "……とにかく、放課後、喫茶店ね。",
                "به‌هرحال. کافه. بعد از مدرسه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "The café.",
                "喫茶店ね。",
                "کافه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I have to take the book back.",
                "本、返さないと。",
                "باید کتابو پس بدم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Take it back on Friday.",
                "金曜でいいでしょ。",
                "جمعه پسش بده.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "…Okay.",
                "……うん。",
                "...باشه.");

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Do not run in the corridor. There is a poster.",
                "廊下は走らないこと。ポスターあるよ。",
                "تو راهرو ندو. پوستر هست.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I never run in the corridor.",
                "僕、廊下走らないよ。",
                "من هیچ‌وقت تو راهرو نمی‌دوم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I know.",
                "知ってる。",
                "می‌دونم.");

            Hold(2.0f);
        }

        /// <summary>
        /// Wednesday, after school. Usagi Café in the rain, and the story Haru
        /// has been carrying since he was fourteen.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The longest scene in the act by a distance, and the manual's seven
        /// rules for a heavy scene are what the shape of it is:
        /// </para>
        /// <para>
        /// It is entered from the side. Nobody works up to it. She complains
        /// that he asks her four times a day whether she is all right, which is
        /// a joke, and the answer to it is the thing.
        /// </para>
        /// <para>
        /// It is told badly, because people tell these badly: out of order, with
        /// one completely irrelevant detail stuck to it that he apologises for,
        /// stopping in the middle, understating it, and finally not saying the
        /// sentence at all but asking her to guess.
        /// </para>
        /// <para>
        /// The method is never said. Not the place, not the means, not
        /// obliquely, not in narration. This is both the right thing to do and
        /// the better writing — the detail is never the point, and its absence
        /// makes the scene stronger.
        /// </para>
        /// <para>
        /// The horror is in the listener. The story is a sad story. What is
        /// wrong with this scene is a question she asks afterwards that nobody
        /// asks, a pause that breaks about a second late, and two stretches
        /// where what she says and what she is thinking are not the same thing.
        /// </para>
        /// <para>
        /// The second of those had to be rewritten, and the reason is worth
        /// keeping. A first draft had her think "he does not know, he is
        /// guessing" when Haru describes a dark place full of sudden noises —
        /// and that is simply not her position. She was in that room. She knows
        /// he was outside it and she knows what happened to his leg there; the
        /// last scene of this act is her saying he would break his own leg for
        /// her, which is a joke only if you do not know she is describing
        /// something that already happened. A girl who knows all of that does
        /// not wonder whether he knows.
        /// </para>
        /// <para>
        /// What she actually panics about is that he SAID it. Out loud, in a
        /// café, folded into the middle of another sentence. So the frames are
        /// "what did he just say", then "so he remembers", then the offer of an
        /// innocent explanation, then her refusing it: Haru-pi does not say
        /// things on his own. She is three acts early and she is right, and she
        /// drops it — which is the whole of act five loaded here, by her, in
        /// five frames of thinking.
        /// </para>
        /// <para>
        /// It is also better for the player, who knows none of this. "He does
        /// not know" invites them to work out what he does not know. "So he
        /// remembers" gives them nothing to work with at all and leaves them
        /// holding a question about a scene they have not been shown.
        /// </para>
        /// <para>
        /// Nothing is resolved, nobody is comforted, and the scene does not end
        /// on it: the last thing in it is an argument about who is having the
        /// last piece of cake.
        /// </para>
        /// <para>
        /// One note on the monologue itself. It is one speech in the design
        /// document and it stays one speech, but a wall of it in a dialogue box
        /// is a wall the player clicks through. Broken at the places he would
        /// actually stop, it is paced by the person reading it — which is the
        /// only way its last line lands.
        /// </para>
        /// <para>
        /// It is also trimmed against Haru's voice rather than against length.
        /// A first draft ran four rhetorical questions back to back and then
        /// had him say he complained to his friend "so it would not be one
        /// way", and both of those are him analysing himself — which is the one
        /// thing his character sheet says he never does, about anybody,
        /// including himself. A rehearsed grief really does come out smoother
        /// than an unrehearsed one, so it was defensible, and it was still the
        /// only place in the act where he did not sound like himself. Two of
        /// the questions are gone and the strategy is gone; he still complains
        /// to his friend about his day, he just no longer explains why he did
        /// it.
        /// </para>
        /// </remarks>
        private void WriteWednesdayCafe()
        {
            ClearStage();

            Place(
                Backgrounds.CafeRainy,
                "Usagi Café", "うさぎ喫茶", "کافه اوساگی");

            Hold(2.6f);

            Narrate(
                "Rain down the window, and the lights inside turned up too warm to argue with.",
                "窓を雨がつたい、店の灯りは反論できないほど暖かかった。",
                "بارون رو شیشه سُر می‌خورد و چراغای داخل اون‌قدر گرم بودن که نشه باهاشون بحث کرد.");

            Enter(Speaker.Yua, Portrait.Joyful);

            Cue(SfxId.ChairScrape, 0.45f);

            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Unchanged,
                "The umbrella stand has three slots and a hole in the bottom.",
                "ここの傘立て、三本しか入らないうえに底に穴が空いてる。",
                "جاچتریِ اینجا سه تا جا داره و ته‌ش سوراخه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The hole is for the water.",
                "穴は水を出すためでしょ。",
                "سوراخ واسه آبه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "The hole is over the doormat.",
                "その穴、マットの上にある。",
                "سوراخ روی پادریه.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Joyful,
                "…That is a design flaw.",
                "……それは設計ミス。",
                "...این یه ایرادِ طراحیه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That is what I have been saying since April.",
                "四月からずっとそう言ってる。",
                "من از آوریل دارم همینو می‌گم.");

            Hold(1.4f);

            Narrate(
                "She had ordered before he sat down. Matcha cake, two forks, a boba for her and an iced matcha for him.",
                "彼が座る前に、彼女はもう注文していた。抹茶ケーキ、フォーク二本、自分にタピオカ、彼に冷たい抹茶。",
                "قبل از اینکه بشینه سفارش داده بود. کیکِ ماچا، دو تا چنگال، یه بابل‌تی واسه خودش و یه ماچای سرد واسه اون.");

            Say(Speaker.Haru, Portrait.Neutral,
                "You ordered before I sat down.",
                "座る前に頼んでたでしょ。",
                "قبل از اینکه بشینم سفارش دادی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Before you stood up, I think.",
                "立つ前だと思う。",
                "فکر کنم قبل از اینکه بلند شی.");

            DecideIdly(
                "Let her divide the cake", "ケーキの分け方を語らせる", "بذار کیک رو تقسیم کنه",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Joyful,
                        "One cake, two forks. That is not a mistake. That is an arrangement.",
                        "ケーキ一個にフォーク二本。手違いじゃない。取り決め。",
                        "یه کیک، دو تا چنگال. این اشتباه نیست. این یه قراره.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "The arrangement is that I get the corner with the cream on it.",
                        "取り決めは、クリームのついてる角があたしのぶん。",
                        "قرار اینه که اون گوشه‌ای که خامه داره مالِ منه.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "You get the rest. You like the rest. You have said so.",
                        "残りはハルぴ。残りのほうが好きなんでしょ。そう言ってた。",
                        "بقیه‌ش مالِ توئه. تو بقیه‌شو دوست داری. خودت گفتی.");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "You have never said so. But you would, if I asked you.",
                        "……言ってないけど。でも訊いたら言うでしょ。",
                        "هیچ‌وقت نگفتی. ولی اگه ازت بپرسم می‌گی.");
                },
                "Let her complain about the rain", "雨の文句を言わせる", "بذار از بارون غر بزنه",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Angry,
                        "It is going to do this until July.",
                        "七月まではこれだよ。",
                        "تا جولای همینه.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "The whole of June. Every day. My shoes have been damp since Monday and they are staying damp.",
                        "六月まるごと。毎日。靴は月曜からずっと湿ってて、これからも湿ってる。",
                        "کلِ ژوئن. هر روز. کفشام از دوشنبه نمورن و نمور هم می‌مونن.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "People keep saying the second week of July. People are guessing.",
                        "みんな七月の第二週って言う。当てずっぽうなのに。",
                        "همه می‌گن هفته‌ی دومِ جولای. همه دارن حدس می‌زنن.");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "Somebody said it on the weather once and now the whole town says it.",
                        "一回、天気予報で誰かが言った。それで町じゅうが言ってる。",
                        "یه بار یکی تو هواشناسی گفتش و حالا کلِ شهر می‌گنش.");
                });

            // Both roads land here.
            Say(Speaker.Haru, Portrait.Neutral,
                "You have decided a lot of things today.",
                "今日、いろいろ決めてるね。",
                "امروز خیلی چیزا رو تصمیم گرفتی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I decide a lot of things every day. You only notice on Wednesdays.",
                "毎日いろいろ決めてる。ハルぴが気づくのが水曜なだけ。",
                "من هر روز خیلی چیزا رو تصمیم می‌گیرم. تو فقط چهارشنبه‌ها می‌فهمی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That is because on Wednesdays I am paying attention.",
                "水曜はちゃんと見てるからね。",
                "چون چهارشنبه‌ها حواسم هست.");

            Hold(1.6f);

            // Act one's drink sequence, brought back once, with the one thing
            // about it that has changed. She told him to finish it in April.
            Narrate(
                "The drinks came. The matcha still had ice in it and the ice was already going.",
                "飲み物が来た。抹茶にはまだ氷があって、その氷はもう溶けかけていた。",
                "نوشیدنیا اومدن. ماچا هنوز یخ داشت و یخش داشت آب می‌شد.");

            Cel(Portrait.DrinkFull);

            Cel(Portrait.DrinkReluctant);

            Cel(Portrait.DrinkFinished);

            Narrate(
                "She watched him finish it.",
                "彼が飲みきるのを、彼女は見ていた。",
                "نگاه کرد که تا آخرشو خورد.");

            Hold(2.0f);

            Enter(Speaker.Yua, Portrait.Joyful);
            Enter(Speaker.Haru, Portrait.Neutral);

            // --- in from the side ---------------------------------------------

            Say(Speaker.Yua, Portrait.Unchanged,
                "You do it four times a day.",
                "一日四回やってるよ。",
                "روزی چهار بار این کارو می‌کنی.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Do what?",
                "何を？",
                "چه کاری؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Ask me if I am all right.",
                "「大丈夫？」って訊くやつ。",
                "می‌پرسی حالم خوبه یا نه.");

            Say(Speaker.Haru, Portrait.Shy,
                "I do not do it four times.",
                "四回もやってないよ。",
                "چهار بار که نمی‌پرسم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "The shoe lockers. The stairs. Outside the staff room. And once with your face, in maths, which counts.",
                "下駄箱。階段。職員室の前。あと数学の時間に顔で一回。それもカウント。",
                "جاکفشی. پله‌ها. جلوی دفترِ معلما. و یه بار با قیافه‌ت، سرِ ریاضی، که حساب می‌شه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That one does not count.",
                "それはカウントしないでしょ。",
                "اون یکی حساب نیست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That one counts double.",
                "それは二回分。",
                "اون یکی دو برابر حساب می‌شه.");

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Why do you do it?",
                "なんでやるの？",
                "چرا این کارو می‌کنی؟");

            Hold(2.2f);

            Narrate(
                "He turned the fork over. Then again. Then a third time.",
                "彼はフォークを裏返した。もう一度。三度目も裏返した。",
                "چنگالو برگردوند. دوباره. بارِ سوم هم برگردوند.");

            Hold(2.0f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Can I tell you something?",
                "……ひとつ、話してもいい？",
                "می‌تونم یه چیزی بهت بگم؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You can tell me anything.",
                "何でも話していいよ。",
                "هر چی بخوای می‌تونی بهم بگی.");

            Hold(2.4f);

            // --- the monologue ------------------------------------------------

            Say(Speaker.Haru, Portrait.Sad,
                "First of all — sorry. For asking so much. When you say you are not all right, and when I only think you are not.",
                "まず、ごめん。たくさん訊いて。君が「大丈夫じゃない」って言うときも、僕がそう思っただけのときも。",
                "اول از همه ببخشید. که این‌قدر سؤال‌پیچت می‌کنم. وقتایی که می‌گی حالت خوب نیست، و وقتایی که فقط من این‌طوری حس می‌کنم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I want to know. That is all of it.",
                "知りたいんだ。それだけ。",
                "می‌خوام بدونم. کلِّش همینه.");

            Hold(1.6f);

            Say(Speaker.Haru, Portrait.Neutral,
                "I had a friend. Best friend. Same class from the second year of primary school to the second year of middle school.",
                "友達がいた。親友。小二から中二まで、ずっと同じクラスだった。",
                "یه رفیق داشتم. رفیقِ صمیمی. از کلاسِ دومِ دبستان تا کلاسِ هشتم همکلاسی بودیم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Six years. We talked every day.",
                "六年間。毎日しゃべってた。",
                "شیش سال. هر روز کلی حرف می‌زدیم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Except the days he was not doing well. Those days he did not have it in him to talk much.",
                "調子が良くない日を除いて。そういう日は、あんまり話す元気がなかった。",
                "به‌جز روزایی که حالش خوب نبود. اون روزا حال نداشت زیاد حرف بزنه.");

            Hold(1.4f);

            // The completely irrelevant detail, stuck to it, that he apologises
            // for. Nobody tells one of these properly.
            Say(Speaker.Haru, Portrait.Unchanged,
                "He had a pencil case with a broken zip. He never once fixed it. Six years of the same pencil case.",
                "ファスナーの壊れた筆箱を使ってた。一度も直さなかった。六年間おなじ筆箱。",
                "یه جامدادی داشت که زیپش خراب بود. حتی یه بارم درستش نکرد. شیش سال همون جامدادی.");

            Say(Speaker.Haru, Portrait.Shy,
                "Sorry. That has nothing to do with it. I do not know why I said that.",
                "ごめん。関係ないね。なんで言ったんだろう。",
                "ببخشید. این هیچ ربطی نداره. نمی‌دونم چرا گفتمش.");

            Hold(1.8f);

            Say(Speaker.Haru, Portrait.Sad,
                "I knew he was sad. I asked him. I kept asking him.",
                "落ち込んでるのは分かってた。訊いた。何度も訊いた。",
                "می‌دونستم ناراحته. ازش می‌پرسیدم. مدام می‌پرسیدم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "But he was not someone who talked.",
                "でも、あんまり話す人じゃなかった。",
                "اما از اون آدمایی نبود که زیاد حرف بزنه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I said the same things over and over. Trust me. Tell me. I am right here. I am your friend.",
                "同じことばっかり言った。信じてよ。話してよ。ここにいるよ。友達だろ。",
                "همیشه بهش می‌گفتم: بهم اعتماد کن. بهم بگو. من کنارتم. من رفیقتم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "If you do not tell me, who are you going to tell? Do not keep it inside. Whenever you want to talk, I am right here.",
                "僕に言わないなら誰に言うんだ。ひとりで抱えるな。話したくなったら、僕はここにいる。",
                "به من نگی می‌خوای به کی بگی؟ نباید بریزی تو خودت. هر وقت خواستی با کسی حرف بزنی، من همیشه همین‌جام.");

            Hold(2.0f);

            Say(Speaker.Haru, Portrait.Sad,
                "He still did not talk. He never told me what hurt.",
                "それでも話さなかった。何がつらいのか、言ってくれなかった。",
                "اما بازم حرف نمی‌زد. از دردهاش نمی‌گفت.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "And I did not understand why.",
                "理由が分からなかった。",
                "و نمی‌فهمیدم چرا.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Was I not someone he could trust? Was I not his friend? Did he not think of me as one?",
                "僕は信用できなかった？ 友達じゃなかった？ そう思われてなかった？",
                "من قابلِ اعتماد نبودم؟ من دوستش نبودم؟ منو رفیقِ خودش نمی‌دید؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "That if he told me, it would come back to him as pity?",
                "話したら、同情されるだけだと思ってた？",
                "فکر می‌کرد اگه بهم بگه، انگار دارم بهش ترحم می‌کنم؟");

            Hold(1.6f);

            Say(Speaker.Haru, Portrait.Sad,
                "I do not know which one it was. I did everything I could think of so that none of them could reach him.",
                "どれだったのか分からない。そういう考えが彼に届かないように、思いつくことは全部やった。",
                "نمی‌دونم کدومش بود. تمامِ تلاشمو کردم که هیچ‌کدوم از این فکرا سراغش نیاد.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I complained to him too. About my day. About things that had annoyed me. Small things.",
                "僕も愚痴った。今日のこととか、嫌だったこととか。小さいこと。",
                "خودمم بهش غر می‌زدم. از روزم. از چیزایی که حرصم داده بود. چیزای کوچیک.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Nothing. When he was happy he would not stop talking. When he was not, nothing.",
                "だめだった。嬉しいときはずっとしゃべるのに、そうじゃないときは何も。",
                "هیچی. وقتی خوشحال بود ول‌کن نبود. وقتی نبود، هیچی.");

            Hold(2.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "We were not in the same class after the second year of middle school.",
                "中二から、同じクラスじゃなくなった。",
                "از کلاسِ هشتم دیگه همکلاسی نشدیم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Guess why.",
                "理由、当ててみて。",
                "حدس بزن چرا.");

            Hold(3.0f);

            Say(Speaker.Haru, Portrait.Crying,
                "He killed himself.",
                "自殺した。",
                "خودکشی کرد.");

            Hold(3.4f);

            Narrate(
                "Behind the counter a kettle came up to the boil and somebody took it off.",
                "カウンターの奥でやかんが沸いて、誰かが火から下ろした。",
                "پشتِ پیشخون یه کتری جوش اومد و یکی از رو شعله ورداشتش.");

            Hold(3.2f);

            // The listener. A question nobody asks, on a pause that broke about
            // a second late.
            Say(Speaker.Yua, Portrait.Neutral,
                "Do you still have anything of his?",
                "その子のもの、まだ持ってる？",
                "هنوز چیزی ازش داری؟");

            Hold(2.4f);

            Say(Speaker.Haru, Portrait.Sad,
                "…The pencil case.",
                "……筆箱。",
                "...جامدادیه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Keep it.",
                "持っときな。",
                "نگهش دار.");

            Hold(2.8f);

            // ◆ Dread moment 5 — rung four, and the first time in the game that
            // what she says and what she is thinking are two different things.
            Say(Speaker.Yua, Portrait.Sad,
                "I am sorry. That is a terrible thing to have been carrying.",
                "ごめんね。ずっと抱えてたんだね。",
                "متأسفم. چیزِ سنگینی بوده که این همه مدت حمل کردی.");

            // The one frame in the act that wears the flat face.
            BeginMonologue(Speaker.Haru, Portrait.DeadEyes);

            InnerVoice(
                "He has been carrying something… all this time, he has been carrying something.",
                "……何か抱えてた。ずっと、何かを抱えてた。",
                "یه چیزی داشته... این همه مدت یه چیزی داشته.");

            InnerVoice(
                "Something of his own.",
                "自分だけのものを。",
                "یه چیزِ مالِ خودش.");

            Hold(1.8f);

            InnerVoice(
                "And I did not know?",
                "あたし、知らなかった……？",
                "و من خبر نداشتم؟");

            InnerVoice(
                "…I do not like that.",
                "……それ、嫌だな。",
                "...از این خوشم نمیاد.");

            EndMonologue(Speaker.Haru, Portrait.Sad, Portrait.Sad);

            Say(Speaker.Haru, Portrait.Sad,
                "I do not know how to ask you this properly.",
                "うまく頼めないんだけど。",
                "نمی‌دونم چطوری ازت بخوام.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "But if anything ever makes you sad — any time at all —",
                "でも、もし何かで悲しくなったら、いつでも——",
                "اما اگه هر وقت از چیزی ناراحت شدی —");

            Say(Speaker.Haru, Portrait.Unchanged,
                "— or if there is something from before that is still with you —",
                "——昔のことで、まだ残ってるものがあるなら——",
                "— یا اگه چیزی از گذشته هست که ازش ناراحتی و هنوز پیشت مونده —");

            SayWithSound(Speaker.Haru, Portrait.Unchanged, SfxId.BoilerRoom, 0.26f,
                "— even if the reason is that I was weak. Even if none of it was your fault. Even if it was dark, and full of loud, sudden noises —",
                "——たとえその理由が、僕が弱かったことでも。たとえ君のせいじゃなかったとしても。たとえそこが暗くて、大きな音が突然する場所だったとしても——",
                "— حتی اگه دلیلِ ناراحتیت ضعیف بودنِ خودِ من بوده باشه. حتی اگه در اصل تقصیرِ خودت نبوده باشه. حتی اگه تاریک بوده و پر از صداهای بلند و ناگهانی —");

            Cue(SfxId.Heartbeat, 0.75f);

            // The one frame in the act she wears the frightened face, and it is
            // his line and not hers. What is behind her eyes when he is not
            // looking is in the monologue below, and it is flat.
            Enter(Speaker.Yua, Portrait.Sad);

            Hold(2.4f);

            Narrate(
                "Her cup was halfway up to her mouth, and it stayed there.",
                "口元まで運ばれたカップが、そのまま止まっていた。",
                "فنجونش نصفه‌راهِ دهنش بود و همون‌جا موند.");

            // Not blank. Terrified, and alone, which is the only condition
            // under which anybody in this game gets to see it.
            BeginMonologue(Speaker.Haru, Portrait.Crying);

            InnerVoice(
                "What did he… what did he just say?",
                "今……今、なんて言った？",
                "الان... الان چی گفت؟");

            InnerVoice(
                "In the middle of it. Just like that. Like it was—",
                "話の途中で。あんなふうに。まるで——",
                "وسطِ حرفش. همین‌جوری. انگار—");

            Hold(1.6f);

            InnerVoice(
                "Does he remember?",
                "覚えてる？",
                "یادشه؟");

            InnerVoice(
                "Does he remember?",
                "……覚えてるの？",
                "یادشه؟");

            Hold(2.0f);

            InnerVoice(
                "Or he does not, and it just… came out?",
                "ううん、覚えてなくて、ただ……出ただけ？",
                "یا نه، یادش نیست و فقط... همین‌جوری از دهنش دراومد؟");

            Hold(2.2f);

            InnerVoice(
                "…No.",
                "……ちがう。",
                "...نه.");

            InnerVoice(
                "Haru-pi does not just say things.",
                "ハルぴは、ただ言ったりしない。",
                "هارو‌پی همین‌جوری چیزی نمی‌گه.");

            Hold(2.8f);

            EndMonologue(Speaker.Haru, Portrait.Neutral, Portrait.Sad);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Go on.",
                "続けて。",
                "ادامه بده.");

            Hold(2.2f);

            Say(Speaker.Haru, Portrait.Sad,
                "I will get on my knees. I will bow. I will beg you. I want you to talk to me.",
                "膝をつく。頭も下げる。お願いする。僕と話してほしい。",
                "به پات می‌افتم. تعظیم می‌کنم. ازت التماس می‌کنم. ازت می‌خوام با من حرف بزنی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Even if you do not know how. It does not have to be exact. It does not have to be long.",
                "話し方が分からなくてもいい。正確じゃなくても、長くなくてもいい。",
                "حتی اگه نمی‌دونی چطوری حرف بزنی. لازم نیست دقیق باشه. لازم نیست طولانی باشه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Just come to me and say you are sad, and I will let go of the whole world and stay until you are all right.",
                "僕のところに来て、つらいって言ってくれるだけでいい。世界のほうを手放して、君が大丈夫になるまでそばにいる。",
                "فقط کافیه بیای پیشم و بهم بگی ناراحتی، و من از کلِ دنیا جدا می‌شم و پیشت می‌مونم تا حالت خوب بشه.");

            Hold(1.8f);

            // He drops the suffix. Once, here, and nowhere else in the act.
            // Nobody in the scene reacts and the narrator does not mention it.
            Say(Speaker.Haru, Portrait.Sad,
                "Yua.",
                "結愛。",
                "یوآ.");

            Hold(2.2f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "Promise me.",
                "約束して。",
                "بهم قول بده.");

            Hold(3.0f);

            Narrate(
                "The rain had got heavier while he was talking.",
                "話しているあいだに、雨は強くなっていた。",
                "وقتی داشت حرف می‌زد بارون سنگین‌تر شده بود.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Neutral,
                "I promise.",
                "約束する。",
                "قول می‌دم.");

            // Smiling the whole way through, including the lie at the end.
            BeginMonologue(Speaker.Haru, Portrait.Joyful);

            InnerVoice(
                "I said it.",
                "……言っちゃった。",
                "گفتمش.");

            Hold(1.4f);

            InnerVoice(
                "What did I just… what did I agree to?",
                "今、あたし……何に頷いた？",
                "الان چی رو... چی رو قبول کردم؟");

            Hold(1.8f);

            InnerVoice(
                "…No. It is fine.",
                "……ううん。平気。",
                "...نه. چیزی نیست.");

            InnerVoice(
                "There is nothing to tell him. There has never been anything to tell him.",
                "話すことなんてない。最初から、何もない。",
                "چیزی نیست که بهش بگم. از اولم چیزی نبوده که بهش بگم.");

            EndMonologue(Speaker.Haru, Portrait.Neutral, Portrait.Joyful);

            Say(Speaker.Haru, Portrait.Joyful,
                "Thank you, Yua-pi.",
                "ありがとう、結愛ぴ。",
                "ممنون، یوآ‌پی.");

            Hold(2.6f);

            // Back to something small. This is the last thing in the scene and
            // it is on purpose: the return is what lands the blow.
            Say(Speaker.Yua, Portrait.Joyful,
                "You have not eaten any of the cake.",
                "ケーキ、一口も食べてないじゃん。",
                "اصلاً از کیک نخوردی.");

            Say(Speaker.Haru, Portrait.Neutral,
                "I have not.",
                "食べてない。",
                "نخوردم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "There is one piece left.",
                "一切れ残ってる。",
                "یه تیکه مونده.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Then you have it.",
                "じゃあ結愛ぴが食べて。",
                "پس تو بخورش.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I was going to have it anyway. I was being polite.",
                "どうせ食べるつもりだった。礼儀で訊いただけ。",
                "به‌هرحال می‌خواستم بخورمش. داشتم تعارف می‌کردم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You were not being polite.",
                "礼儀じゃなかったでしょ。",
                "تعارف نمی‌کردی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I was being extremely polite.",
                "すっごく礼儀正しかった。",
                "خیلی هم تعارف می‌کردم.");

            Hold(1.8f);

            SayWithSound(Speaker.Haru, Portrait.Joyful, SfxId.PageTurn, 0.35f,
                "Somebody's umbrella has just gone inside out at the crossing.",
                "横断歩道で、誰かの傘がひっくり返った。",
                "چترِ یکی همین الان سرِ چهارراه برگشت.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Where?",
                "どこ？",
                "کجا؟");

            Narrate(
                "They both leaned over and watched it happen through the window, and it took a while.",
                "二人とも身を乗り出して、窓ごしにそれを見ていた。しばらくかかった。",
                "هر دوشون خم شدن و از پشتِ شیشه نگاش کردن، و یه مدتی طول کشید.");

            Hold(2.8f);
        }

        // =====================================================================
        //  THURSDAY — afterwards
        //
        //  Dread budget: 2. The manual's seventh rule for a heavy scene is that
        //  it does not finish when the scene does; it leaks into the days after
        //  as an ABSENCE. So Thursday is an ordinary day with one person
        //  slightly quieter in it, and nobody in the act ever says why.
        // =====================================================================

        /// <summary>
        /// Thursday morning. Clear weather, a poster somebody has drawn on, and
        /// a boy who is a different quiet.
        /// </summary>
        private void WriteThursdayClassroom()
        {
            Maybe(LegAche, 0.25f);

            ClearStage();

            Place(
                Backgrounds.ClassroomDay,
                "The classroom", "教室", "کلاس درس");

            Hold(2.2f);

            Narrate(
                "Thursday came up clear and stayed that way, which nobody had been promised.",
                "木曜は晴れて、そのまま晴れていた。誰もそんな約束はされていなかった。",
                "پنج‌شنبه صاف دراومد و همون‌جوری هم موند، که به کسی قولشو نداده بودن.");

            Narrate(
                "The book went back before class.",
                "本は授業の前に返された。",
                "کتاب قبل از کلاس پس داده شد.");

            Cue(SfxId.ChairScrape, 0.45f);

            Enter(Speaker.Yua, Portrait.Joyful);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Angry,
                "Somebody has drawn on my poster.",
                "誰かがあたしのポスターに落書きした。",
                "یکی رو پوسترِ من خط‌خطی کرده.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Drawn what on it?",
                "何を描いたの？",
                "چی کشیده روش؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "A second person. Falling over. Badly.",
                "二人目。転んでる。下手くそに。",
                "یه نفرِ دوم. که داره می‌افته. بد.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That was there before it went up.",
                "それ、貼る前からあったよ。",
                "اون قبل از اینکه بره بالا اونجا بود.");

            Say(Speaker.Yua, Portrait.Angry,
                "It was not.",
                "なかったもん。",
                "نبود.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It is under the lettering. Whoever drew it drew it before the letters went on.",
                "字の下にあるんだ。描いたのは、字を書く前。",
                "زیرِ خط‌نویسیه. هر کی کشیدتش، قبل از نوشتنِ حروف کشیده.");

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Neutral,
                "…Then it was you.",
                "……じゃあハルぴじゃん。",
                "...پس کارِ خودت بوده.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It was me.",
                "僕です。",
                "کارِ خودم بود.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It is very bad.",
                "すごく下手。",
                "خیلی بده.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It is falling over beautifully.",
                "きれいに転んでる。",
                "داره قشنگ می‌افته.");

            Hold(1.8f);

            Narrate(
                "There were dates on the board that had not been there on Wednesday, and Tofu was on the sill behind them, doing nothing.",
                "水曜にはなかった日付が黒板に書かれていた。その後ろの窓際で、豆腐は何もしていなかった。",
                "رو تخته یه سری تاریخ بود که چهارشنبه نبودن، و توفو پشتشون رو طاقچه بود و هیچ کاری نمی‌کرد.");

            DecideIdly(
                "Let her complain about the dates", "日程の文句を言わせる", "بذار از تاریخا غر بزنه",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Neutral,
                        "The mid-term dates are on the board and they are in chalk.",
                        "中間の日程、黒板に書いてある。チョークで。",
                        "تاریخای میان‌ترم رو تخته‌ن و با گچن.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Small chalk. She did not press. That is not a date, that is a suggestion.",
                        "小さい字。力も入れてない。あれは日程じゃなくて提案。",
                        "گچِ ریز. فشارم نداده. اون تاریخ نیست، پیشنهاده.");

                    Say(Speaker.Yua, Portrait.Angry,
                        "They are going to move. They always move.",
                        "動くよ。いつも動く。",
                        "جابه‌جا می‌شن. همیشه جابه‌جا می‌شن.");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "And when they move, everyone is going to act surprised, and I am not going to.",
                        "動いたとき、みんな驚いた顔する。あたしはしない。",
                        "و وقتی جابه‌جا شن، همه وانمود می‌کنن تعجب کردن، و من نمی‌کنم.");
                },
                "Let her talk about the plant", "豆腐の話をさせる", "بذار از توفو حرف بزنه",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Neutral,
                        "Tofu has stopped.",
                        "豆腐、止まった。",
                        "توفو وایساده.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "All week it grew, and now it has stopped. It is exactly the size it was on Tuesday.",
                        "ずっと伸びてたのに、止まった。火曜とまったく同じ大きさ。",
                        "کلِ هفته رشد کرد و حالا وایساده. دقیقاً همون اندازه‌ی سه‌شنبه‌ست.");

                    Say(Speaker.Yua, Portrait.Angry,
                        "The rain stopped and it stopped. Do not tell me it is indoors. I know it is indoors.",
                        "雨が止んだら止まった。室内だって言わないで。知ってる。",
                        "بارون وایساد و اونم وایساد. بهم نگو تو اتاقه. می‌دونم تو اتاقه.");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "It knows.",
                        "あの子、知ってるの。",
                        "خودش می‌فهمه.");
                });

            // Both roads land here, and he does not argue with either of them,
            // which is the whole of what is wrong with him this morning.
            Say(Speaker.Haru, Portrait.Neutral,
                "Probably.",
                "……そうかもね。",
                "احتمالاً.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Probably?",
                "そうかもね？",
                "احتمالاً؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Probably.",
                "そうかもね。",
                "احتمالاً.");

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Joyful,
                "You are being very quiet today.",
                "今日、やけに静かだね。",
                "امروز خیلی ساکتی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I am always quiet.",
                "いつも静かだよ。",
                "من همیشه ساکتم.");

            Say(Speaker.Yua, Portrait.Neutral,
                "You are being a different quiet.",
                "いつもと違う静かさ。",
                "یه جورِ دیگه ساکتی.");

            Say(Speaker.Haru, Portrait.Shy,
                "…Sorry.",
                "……ごめん。",
                "...ببخشید.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Do not apologise. I like it.",
                "謝らないで。あたしは好きだよ。",
                "عذرخواهی نکن. من خوشم میاد.");

            // ◆ Dread moment 6 — rung four again, and she is enjoying it.
            BeginMonologue(Speaker.Haru, Portrait.Joyful);

            InnerVoice(
                "I do like it.",
                "本当に、好き。",
                "واقعاً خوشم میاد.");

            InnerVoice(
                "When he is like this… when there is less of him…",
                "こういうとき……中身が少ないとき……",
                "وقتی این‌جوریه... وقتی کمتر ازش مونده...");

            InnerVoice(
                "He is softer. He is slower. He is easier to keep hold of…",
                "やわらかくて、ゆっくりで、つかまえておきやすくて……",
                "نرم‌تره. آروم‌تره. راحت‌تر می‌شه نگهش داشت...");

            Hold(1.8f);

            InnerVoice(
                "…Good.",
                "……いいね。",
                "...خوبه.");

            InnerVoice(
                "Keep him like this.",
                "このままにしておこう。",
                "همین‌جوری نگهش دار.");

            EndMonologue(Speaker.Haru, Portrait.Joyful, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Neutral,
                "I might walk back on my own today.",
                "今日はひとりで帰ろうかな。",
                "امروز شاید تنها برگردم.");

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Neutral,
                "The platform. Four twenty.",
                "ホーム。四時二十分。",
                "سکوی ایستگاه. چهار و بیست دقیقه.");

            Hold(1.2f);

            SayWithSound(Speaker.Haru, Portrait.Neutral, SfxId.SchoolBell, 0.50f,
                "…Four twenty.",
                "……四時二十分ね。",
                "...چهار و بیست دقیقه.");

            Hold(2.0f);
        }

        /// <summary>
        /// Thursday evening. The platform, the promise called in a day early,
        /// and the second promise, which is Yua's.
        /// </summary>
        /// <remarks>
        /// The last ordinary scene in the act, and the only place the design
        /// document's fifth act is loaded. Haru says he told her the story and
        /// wishes he had told it somewhere else; Yua asks him, lightly, in the
        /// middle of a joke, never to tell it to anybody else. On a first
        /// playthrough it reads as a girl wanting something to be theirs. After
        /// act five, when he says he chose to tell her that story on purpose,
        /// it is the worst line in the act — and it was two frames long and had
        /// no weight on it at all.
        /// </remarks>
        private void WriteThursdayPlatform()
        {
            ClearStage();

            Place(
                Backgrounds.TrainPlatformSunset,
                "The platform", "駅のホーム", "سکوی ایستگاه");

            Hold(2.4f);

            Narrate(
                "The sakura along the track are all leaves now, and have been for weeks, and nobody photographs them any more.",
                "線路沿いの桜はもう葉ばかりで、何週間もそうで、誰も写真を撮らなくなった。",
                "شکوفه‌های کنارِ ریل حالا همه‌شون برگن، هفته‌هاست همین‌جورن، و دیگه کسی ازشون عکس نمی‌گیره.");

            Enter(Speaker.Haru, Portrait.Neutral);
            Enter(Speaker.Yua, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Shy,
                "About yesterday.",
                "昨日のことなんだけど。",
                "درباره‌ی دیروز.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Mm.",
                "うん。",
                "هوم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I should not have put that on you in a café.",
                "喫茶店であんな話、するべきじゃなかった。",
                "نباید تو یه کافه این چیزو می‌ریختم رو دوشت.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Where should you have put it?",
                "どこでするべきだったの？",
                "کجا باید می‌ریختیش؟");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Joyful,
                "…Somewhere with fewer forks.",
                "……フォークの少ないところ。",
                "...یه جایی که چنگالِ کمتری داشته باشه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "There is no such place. You have looked.",
                "そんな場所ないよ。もう探したでしょ。",
                "همچین جایی نیست. گشتی دیگه.");

            Hold(2.0f);

            Cue(SfxId.Petal, 0.35f);

            Narrate(
                "Somebody further down the platform put money into the machine by the waiting chairs, and something came out of it.",
                "ホームの先で、誰かが待合の椅子のそばの自販機に金を入れると、何かが落ちてきた。",
                "یکی اون‌ورترِ سکو تو دستگاهِ کنارِ صندلیای انتظار پول انداخت، و یه چیزی افتاد بیرون.");

            Cue(SfxId.VendingThunk, 0.40f);

            Say(Speaker.Haru, Portrait.Neutral,
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
                "دیروز قول دادی. تو کافه.");

            Say(Speaker.Haru, Portrait.Shy,
                "So — is there anything?",
                "……それで、何か、ある？",
                "خب... چیزی هست؟");

            Hold(3.4f);

            Say(Speaker.Yua, Portrait.Joyful,
                "No.",
                "ないよ。",
                "نه.");

            Hold(1.8f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Why? Did you want there to be?",
                "どうして？　あってほしかったの？",
                "چطور؟ دلت می‌خواست چیزی باشه؟");

            Say(Speaker.Haru, Portrait.Neutral,
                "No. I am glad there is not.",
                "……ううん。ないならよかった。",
                "نه. خوشحالم که نیست.");

            Narrate(
                "He said it to the track.",
                "彼は線路のほうを見たまま言った。",
                "رو به ریل گفتش.");

            Hold(2.6f);

            // ◆ Dread moment 7 — and act five's whole hinge, in two frames,
            // with a joke on either side of it.
            Say(Speaker.Yua, Portrait.Joyful,
                "Promise me something back, though. It is only fair.",
                "でも、こっちにも約束して。フェアじゃないでしょ。",
                "ولی تو هم به من یه قول بده. انصافه دیگه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That does seem fair.",
                "たしかにフェアだ。",
                "خب آره، انصافه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Do not tell that story to anybody else.",
                "あの話、ほかの誰にもしないで。",
                "اون داستانو به هیچ‌کسِ دیگه نگو.");

            Hold(2.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "…All right.",
                "……うん。",
                "...باشه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Say it properly.",
                "ちゃんと言って。",
                "درست بگو.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I will not tell it to anybody else.",
                "ほかの誰にも話しません。",
                "به هیچ‌کسِ دیگه نمی‌گمش.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Good.",
                "よろしい。",
                "خوبه.");

            Hold(1.8f);

            Say(Speaker.Yua, Portrait.Neutral,
                "It is ours now.",
                "もう、あたしたちのものだから。",
                "دیگه مالِ ماست.");

            Hold(2.6f);

            Narrate(
                "An orange one came through without slowing, the way the orange ones do, and it took the rest of whatever either of them was going to say.",
                "オレンジのが速度を落とさずに通り過ぎた。オレンジのはいつもそうだ。二人が言いかけていたものは、その音に持っていかれた。",
                "یه قطارِ نارنجی بدونِ اینکه آروم کنه رد شد، همون‌جوری که نارنجیا رد می‌شن، و بقیه‌ی حرفی که هر کدومشون می‌خواست بزنه رو با خودش برد.");

            Hold(2.2f);

            Say(Speaker.Yua, Portrait.Joyful,
                "That is yours. Go on.",
                "次のがハルぴの。行って。",
                "اون بعدی مالِ توئه. برو.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Tomorrow.",
                "また明日。",
                "فردا می‌بینمت.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Tomorrow.",
                "また明日。",
                "فردا می‌بینمت.");

            Hold(2.0f);

            Exit(Speaker.Haru);

            Hold(3.0f);

            Narrate(
                "She stayed on the platform for a while after his train had gone, and the light went orange and then went off the ends of things.",
                "彼の電車が行ってからも、彼女はしばらくホームにいた。光はオレンジになり、やがて物の縁から抜けていった。",
                "بعد از اینکه قطارش رفت یه مدتی رو سکو موند، و نور نارنجی شد و بعد از لبه‌ی چیزا رفت.");

            Hold(2.8f);
        }

        // =====================================================================
        //  SUNDAY — the room
        //
        //  The end of the act, and the only place in it that goes above the
        //  daily budget. The manual allows the last moment of an act to be one
        //  rung taller than anything before it, and "taller" here means quieter
        //  and longer rather than louder.
        // =====================================================================

        /// <summary>
        /// Sunday morning, Yua's room, and the first time anything in this
        /// story turns round and looks at the player.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Rung five. The manual's six rules for it are the whole design of the
        /// scene and each one of them is doing a job:
        /// </para>
        /// <para>
        /// She is direct about what she wants and never about why she wants it.
        /// The clinical word for what she is describing does not appear in the
        /// scene in any of the three languages, and it must not: the player
        /// works it out from being given an instruction, which is a thing that
        /// happens to them rather than a thing they are told about. The design
        /// document's own draft of this speech uses the word, and the design
        /// document's section nine says in as many words that its dialogue is
        /// there to show intent and is not to be used in the game.
        /// </para>
        /// <para>
        /// She has to be likeable. The frightening version of this scene is the
        /// one where the player half wants to say yes; a version where she is
        /// visibly a villain just makes an opponent of them and no complicity
        /// forms at all.
        /// </para>
        /// <para>
        /// The player cannot answer. There is no choice anywhere in this scene
        /// and there must not be — the game quietly taking their agency away is
        /// the most frightening thing in it, and it does that without a word.
        /// </para>
        /// <para>
        /// She assumes they are already in it. Not "will you help me" but "you
        /// have been pressing things all week".
        /// </para>
        /// <para>
        /// It is short, and nobody ever mentions it again. Act three opens on
        /// an ordinary morning on the way to school, and being ignored is the
        /// worst part of it.
        /// </para>
        /// </remarks>
        private void WriteSundayRoom()
        {
            ClearStage();

            Place(
                Backgrounds.YuaRoomDay,
                "Yua's room", "結愛の部屋", "اتاق یوآ");

            Hold(3.0f);

            Narrate(
                "Sunday came up bright, the way it does after a week of rain, and it did not suit the room at all.",
                "日曜は晴れた。雨の一週間のあとはたいていそうだ。その明るさは、この部屋にまるで似合わなかった。",
                "یکشنبه روشن دراومد، همون‌جوری که بعد از یه هفته بارون درمیاد، و اصلاً به اون اتاق نمی‌اومد.");

            Narrate(
                "There is a mirror on that wall with a crack across it, and nobody in this house has ever mentioned it.",
                "その壁には、ひびの入った鏡が掛かっている。この家の誰も、一度も口にしたことがない。",
                "رو اون دیوار یه آینه هست با یه ترک، و هیچ‌کس تو این خونه تا حالا حرفشو نزده.");

            Enter(Speaker.Yua, Portrait.Neutral);

            Narrate(
                "She sat on the edge of the bed with the rabbit on her knees and, for a while, did nothing at all.",
                "彼女はベッドの端に腰かけ、うさぎを膝にのせ、しばらく何もしなかった。",
                "لبه‌ی تخت نشست، خرگوشو گذاشت رو زانوهاش و یه مدت هیچ کاری نکرد.");

            Hold(3.4f);

            BeginAside();

            Say(Speaker.Yua, Portrait.Neutral,
                "You are there.",
                "……そこにいるよね。",
                "تو اونجایی.");

            Hold(2.6f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Do not go quiet on me. I have known for a while.",
                "黙らないで。けっこう前から知ってる。",
                "ساکت نشو. یه مدته می‌دونم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Not Haru-pi. I do not think Haru-pi knows you are watching our lives and listening to us. I think it is only me.",
                "ハルぴじゃない。ハルぴは、あなたがあたしたちの生活を見て、話を聞いてることを知らないと思う。知ってるのは、たぶんあたしだけ。",
                "هارو‌پی نه. فکر نکنم هارو‌پی خبر داشته باشه که تو داری زندگی و صحبت‌هامون رو می‌بینی. فکر کنم فقط من می‌دونم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Which makes you mine, a little. That is nice.",
                "つまり、あなたはちょっとだけあたしのもの。いいでしょ。",
                "یعنی یه‌کم مالِ منی. قشنگه.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Neutral,
                "You have been pressing things all week without anybody telling you what they were, so.",
                "誰にも説明されないまま、一週間ずっと押してきたでしょ。だから。",
                "کلِ هفته داشتی چیزا رو فشار می‌دادی بدونِ اینکه کسی بهت بگه اونا چین. خب.");

            SayWithSound(Speaker.Yua, Portrait.Unchanged, SfxId.ChoiceAppear, 0.55f,
                "Sometimes, while we are talking, you are going to get two buttons.",
                "ときどき、あたしたちが話してるあいだに、ボタンが二つ出る。",
                "بعضی وقتا، وقتی داریم حرف می‌زنیم، دو تا دکمه بهت داده می‌شه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "One blue. One green.",
                "青いのと、緑の。",
                "یکی آبی. یکی سبز.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Blue is the kind one. Green is the one I want.",
                "青は優しいほう。緑は、あたしが欲しいほう。",
                "آبی مهربونه‌ست. سبز اونیه که من می‌خوام.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Press the green ones.",
                "緑を押して。",
                "سبزها رو فشار بده.");

            Hold(2.4f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "I want him to be mine. Exactly the way I want him. That is the whole of it and I am not going to dress it up for you.",
                "あたしのものでいてほしい。あたしが望むとおりに。それだけ。きれいに言い換えたりしない。",
                "می‌خوام مالِ من باشه. دقیقاً همون‌جوری که من می‌خوام. کلِ ماجرا همینه و قرار هم نیست واسه تو قشنگش کنم.");

            Say(Speaker.Yua, Portrait.Shy,
                "Do not look at me like that.",
                "そんな目で見ないで。",
                "این‌جوری نگام نکن.");

            Hold(1.8f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Haru-pi will not be hurt. He loves me.",
                "ハルぴは傷つかない。あたしのこと好きだから。",
                "هارو‌پی اذیت نمی‌شه. اون منو دوست داره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "If I asked him, he would say it was fine. I am not going to ask him.",
                "訊いたら、平気だよって言うと思う。訊かないけど。",
                "اگه ازش بپرسم، می‌گه اشکالی نداره. ولی نمی‌پرسم.");

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Joyful,
                "This is love!",
                "これが愛でしょ！",
                "این عشقه!");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Joyful,
                "I think he loves me enough that he would break his own leg for me.",
                "あたしのためなら、自分の脚くらい折ってくれると思う。",
                "فکر می‌کنم اون‌قدری منو دوست داره که حاضره پاشو بخاطرِ من بشکنه.");

            Hold(2.8f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Do you not think so?",
                "そう思わない？",
                "این‌طوری فکر نمی‌کنی؟");

            Hold(3.4f);

            Narrate(
                "Nothing answered her. She waited anyway.",
                "何も答えなかった。それでも彼女は待った。",
                "هیچی جوابشو نداد. بازم منتظر موند.");

            Hold(2.4f);

            Say(Speaker.Yua, Portrait.Joyful,
                "Anyway. Green. Please.",
                "とにかく、緑。……お願い。",
                "به‌هرحال. سبز. لطفاً.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I said please. I do not say please. Write it down somewhere.",
                "「お願い」って言った。あたし、お願いって言わないんだよ。どこかに書いといて。",
                "گفتم لطفاً. من لطفاً نمی‌گم. یه جایی یادداشتش کن.");

            Hold(2.6f);

            StopMusic();

            SayWithSound(Speaker.Yua, Portrait.Neutral, SfxId.StringPull, 0.70f,
                "And do not tell him about this.",
                "それと、このことは言わないで。",
                "و درباره‌ی این به اون چیزی نگو.");

            Hold(3.0f);

            EndAside();

            Narrate(
                "The rabbit's left ear was worn nearly through, in the one place a hand had held it for years.",
                "うさぎの左耳は、同じところを何年も握られて、ほとんど擦り切れていた。",
                "گوشِ چپِ خرگوش تقریباً ساییده شده بود؛ همون‌جایی که سال‌ها یه دست نگهش داشته بود.");

            Hold(3.2f);

            ClearStage();

            Hold(1.4f);
        }

        // =====================================================================
        //  SELF-AUDIT
        //
        //  The manual's section twenty, run against the nine scenes above, in
        //  the order the manual puts them — because the order is the point. The
        //  human questions come first and they outrank everything else in the
        //  document.
        //
        //  ── Human ──────────────────────────────────────────────────────────
        //
        //  If there were no secret, would this be fun to play?
        //      Seven of the nine scenes, yes, on their own terms: a folding
        //      umbrella that is a sock, a peace lily accused of being a tree, a
        //      poster forbidding the only enjoyable thing that happens in a
        //      corridor, cicadas three weeks early and what that is the start
        //      of, an umbrella stand with a hole over the doormat, a lost cat
        //      whose name she did not want to learn, and a drawing of somebody
        //      falling over beautifully that turns out to be his. The eighth is
        //      the café and is not supposed to be fun; the ninth is the ending.
        //
        //  Where is it funny or warm?
        //      "Because you have a sock." · "It is a tree with low
        //      expectations." · "You are agreeing with me and calling it
        //      disagreeing." · "I liked it better when it was losing." · "The
        //      committee is unanimous." · "It is falling over beautifully." ·
        //      "I was being extremely polite."
        //
        //  Is half of it about a third thing?
        //      More than half of every scene except the café, and in the café
        //      the first third and the last twelve frames are. Across the act:
        //      umbrellas, a plant, a poster, shoe locker twelve, cicadas, heat
        //      shimmer, a cat, a vending machine row, a library book, the
        //      weather forecast, mid-term dates written in pencil on a
        //      blackboard, and a piece of cake.
        //
        //  The substitution test — replace all of Haru's answers with "okay":
        //      The act collapses. He wins an argument in seven of the nine
        //      scenes and he is right every time, on facts she does not have:
        //      it is a peace lily, the third umbrella stand is the side door,
        //      the shimmer is over the roofs with vents on them, the machine
        //      was restocked, the hole in the umbrella stand is over the mat,
        //      the drawing is underneath the lettering, and the dates were
        //      written small and unpressed.
        //
        //  Did he win something unimportant and lose something real?
        //      Monday, the library book to twenty minutes of lettering.
        //      Monday evening, the library to a walk to the corner. Tuesday,
        //      the roof tomorrow to the classroom. Wednesday, the book again,
        //      to Friday. Thursday, walking home alone to a platform at twenty
        //      past four. Not one of the five is remarked on by anybody, and
        //      the book is never mentioned again after it goes back.
        //
        //  Does Yua do something that has nothing to do with Haru?
        //      Every scene. The poster and the falling-over person. The
        //      cicadas. Locker twelve. The salty lychee. Not reading a collar
        //      in April on principle. The committee.
        //
        //  Are the lines all the same length?
        //      No. One genuinely long one per scene — the cicadas, the salty
        //      lychee, the corridor poster, locker twelve, the pencil case —
        //      against a lot of one-word answers, and the café monologue is
        //      deliberately built out of short lines so that the reader sets
        //      its pace.
        //
        //  Does any scene sound like the one before it?
        //      Checked in pairs. Monday morning is a joke argument, Monday
        //      evening is a correction. Tuesday lunch is a rant, Tuesday
        //      evening is a silence. Wednesday morning is a silence of the
        //      other kind, Wednesday evening is the act. Thursday morning is
        //      warmth over an absence, Thursday evening is a bargain. No two
        //      consecutive scenes share a shape.
        //
        //  ── Forbidden patterns ─────────────────────────────────────────────
        //
        //  Astonishment and explanation?
        //      None, and this act had two chances to fall into it. Monday
        //      evening, she corrects him about a drink and he does not ask how
        //      she knows — he concedes the fact and argues about what it is
        //      worth. Wednesday, he says "I know" and she does not ask, and the
        //      narration says out loud that she never has, which is the manual's
        //      version: the player is meant to notice that nobody ever asks, and
        //      the characters never notice at all.
        //
        //  Order and obedience with nothing in between?
        //      Checked at every "okay". The lettering has a library book
        //      between the ask and the yes, the corner has a closing time, the
        //      café on Friday has the book again, the platform has a joke about
        //      forks. The one deliberately bare instruction in the act is
        //      "press the green ones", which is not a scene between two
        //      characters at all.
        //
        //  Status announcements?
        //      Every number in the act is load-bearing. Four times a day,
        //      because she is teasing him with a count. Three weeks early,
        //      because it is a joke about cicadas. Locker twelve, four days
        //      late, twenty minutes, four twenty, one piece of cake, one
        //      hundred yen — each of them is somebody's complaint, joke or
        //      arrangement. No clock, no headcount, no exits, no distances.
        //
        //  Observation and confirmation?
        //      Every observation goes into an argument, a joke or a decision.
        //      The umbrella stand becomes a design flaw. The shimmer becomes a
        //      row about vents. The empty lychee row becomes a complaint about
        //      losing.
        //
        //  More than four question-and-answer turns in a row?
        //      Longest chain is four, in the café before the monologue, and it
        //      is broken by him turning a fork over three times.
        //
        //  ── The trap ───────────────────────────────────────────────────────
        //
        //  Seven moments across five days — 1, 2, 3, 2 and then the ending —
        //  and none in the first forty frames of the act:
        //      1  Mon evening   "You liked it once." The sugar. The hour.
        //      2  Tue evening   The leg. He says the word and nothing else.
        //      3  Tue evening   "You used to run everywhere."
        //      4  Wed morning   The machine room, "I know", and nobody asking.
        //      5  Wed café      Two stretches where her mouth and her head are
        //                       not saying the same thing — jealousy of his
        //                       grief, and then five frames of her working out
        //                       that he said that sentence on purpose.
        //      6  Thu morning   "He is easier when there is less of him."
        //      7  Thu platform  "Do not tell that story to anybody else."
        //  and then the room, which is the act's ending and is allowed to be
        //  one rung above all of them.
        //
        //  Is each one deniable?
        //      One, two, three, four and seven each have an ordinary sentence
        //      available and it is planted before the moment: she has a better
        //      memory, his knee hurts and she is being tactful, old friends
        //      know things, and a girl wants a story to be theirs. Five and six
        //      are rung four, which the manual says is no longer deniable — and
        //      they are here because the narrative document puts the café scene
        //      here and nowhere else.
        //
        //  Did I go up more than one rung in a scene? Did I skip one?
        //      No, and no. Three appears twice before four appears at all, four
        //      appears twice before five, and five is on its own in the last
        //      scene. Six — the game refusing a button the player pressed —
        //      cannot happen in this act, because there is no button in this
        //      act to refuse. It belongs to act three.
        //
        //  Is any clinical symptom performed directly? Can the player name it?
        //      No line in the act reports a time, a headcount, an exit or a
        //      distance. Hypervigilance is half a second of quiet after a door
        //      shuts downstairs on Monday, and nobody mentions it. Control is a
        //      scene where she gives no order at all and he does it anyway.
        //      Survivor's guilt is a boy apologising for being slow. The one
        //      place the machinery is visible is her inner voice in the café,
        //      and even there she does not have a word for what she is feeling
        //      — she says she does not like it.
        //
        //  ── Act transition ─────────────────────────────────────────────────
        //
        //  The inheritance block at the top of this file was filled in from act
        //  one's actual text. The rung register is there too, and the planting
        //  list, and every plant in it is two lines long at most.
        //
        //  Shared jokes brought back, once each, with new meaning rather than
        //  as repetition: "somebody said" is now what he says to end a
        //  conversation rather than to start one; "it is fine" is now what she
        //  says about a machine instead of what she said about a hundred yen;
        //  the iced matcha he does not like is drunk without being told to; the
        //  cat's collar she refused to read in April is a name on a flyer. "It
        //  is a good tree" is not repeated anywhere, because act one spent it
        //  on its own last frame and a second use would only be cheaper.
        //
        //  ── Heavy scene ────────────────────────────────────────────────────
        //
        //  Was the method said? No. Not the place, the means, the day or the
        //  time, and not obliquely, and not in narration.
        //
        //  Did the scene end on it? No. It ends on an umbrella going inside out
        //  at a crossing and both of them leaning over to watch.
        //
        //  Did anybody find comfort? No. Nobody says the right thing, nobody is
        //  consoled, and nothing about either of them is better afterwards.
        //
        //  Does it leak? Thursday. He is a different quiet and nobody in the
        //  act ever says why.
        //
        //  ── Addressing the player ──────────────────────────────────────────
        //
        //  Did she explain herself? No. She says what she wants and never once
        //  why she wants it, and the clinical word is absent from all three
        //  languages.
        //
        //  And where she does state it, she states it flat. "Haru-pi will not be
        //  hurt. He loves me." / "If I asked him, he would say it was fine. I
        //  am not going to ask him." / "This is love!" — which is the whole of
        //  her position in four words, said brightly, with no argument attached
        //  and no apology under it. She is not justifying anything. She has
        //  simply told the player what the word means where she lives.
        //
        //  One line in this scene had to go for failing that. It was "if I hold
        //  him a little tighter than people are supposed to, he is fine", and
        //  the tell is "than people are supposed to": she was describing her own
        //  behaviour as outside the normal range and then excusing it, which is
        //  self-diagnosis with the clinical word filed off. It reads as a want
        //  and it is not one. What stands in its place is a claim about him —
        //  "if I asked him, he would say it was fine. I am not going to ask
        //  him" — which says the same thing about the situation, says nothing
        //  at all about her, and is worse to read, because the second sentence
        //  is a decision rather than an excuse.
        //
        //  Did the player get options? None. There is no choice beat anywhere
        //  in that scene.
        //
        //  Was she charming? That is what "I said please. I do not say please.
        //  Write it down somewhere" is for.
        //
        //  ── Who is talking, and to whom ────────────────────────────────────
        //
        //  Twenty-one frames in this act are Yua thinking, and every one of
        //  them used to happen with Haru standing on the same screen, on the
        //  same name plate, in the same box, separated only by a typing speed
        //  the player has no reliable memory of. "I did not know that. I do not
        //  like that.", one frame after he has asked her to promise him
        //  something, read as a girl saying that to his face — the opposite of
        //  the scene, and it undoes rung four, because rung four IS the gap
        //  between her mouth and her head.
        //
        //  A draft before this one put brackets round the text. It was cheap,
        //  it looked like a stage direction, and it never actually said she was
        //  alone. What says it is the staging the act already owns: the platform
        //  scene ends with Haru getting on a train and the player left standing
        //  with Yua, and everybody understood that one without being told. So
        //  BeginMonologue does the same thing on purpose, four times:
        //
        //    · Haru fades out of frame.
        //    · The music goes. It is the only ambient change this engine can
        //      make — there is no ambient layer, see below — and it is a fade,
        //      not the design document's zero-millisecond cut.
        //    · One heartbeat lands in the silence the music left.
        //    · Her face changes to the flat one, and the typing drops to
        //      eighteen characters a second, slower than anything else in the
        //      game, so the machine audibly labours.
        //
        //  EndMonologue gives all four back and names the face she is wearing
        //  by the time he can see her again, which is never the one she was
        //  wearing while he could not.
        //
        //  The faces are the other half of it, and one face for all four was
        //  the wrong answer twice over. A draft before this one wore DeadEyes
        //  through every monologue, which turned the most frightening picture
        //  in the project into wallpaper — by the fourth one the player has
        //  stopped seeing it. So the face is an argument now, passed in, and
        //  the four are four different women:
        //
        //    DeadEyes    the café, after the suicide. ONE frame of the act,
        //                and the only one. She has just found out he owns a
        //                grief she had no part in, and what is behind her eyes
        //                is not distress, it is nothing at all. Spending it
        //                once is the whole reason it lands, and it is the face
        //                act five ends on.
        //
        //    Crying      the café, on the machine room. She is not blank here,
        //                she is terrified — and she is alone, so this is the
        //                face nobody in the story is ever allowed to see. It
        //                escalates out of the worried face she wears on his
        //                line, which is the only place in the act THAT one
        //                appears, because his line is a thing being done to her
        //                rather than a thing she is doing.
        //
        //    Joyful      the café, after she promises. She smiles the whole way
        //                through working out what she just signed, and then
        //                denies there was anything to sign. Denial with a smile
        //                on it is worse than a flat stare.
        //
        //    Joyful      Thursday, and for the opposite reason. She is enjoying
        //                herself. Somebody who has got what she wanted looks
        //                like somebody who has got what she wanted.
        //
        //  The Thursday one also lost its last two frames, and the cut is the
        //  same rule as the room scene's. "That is a strange thing to like, is
        //  it not." / "Fine. It is strange." is her holding her own behaviour
        //  up and judging it abnormal, which is a diagnosis in her own mouth.
        //  She does not think she is strange. She thinks she is right, and she
        //  is getting what she wants, so what replaces it is a plan: "…Good.
        //  Keep him like this." Nobody in this act is allowed to psychoanalyse
        //  themselves, and she least of all.
        //
        //  One thing asked for and not delivered as asked. A pause of about a
        //  third of a second between two lines of thinking, with the box still
        //  on screen, is not something an act can write: Hold is the only delay
        //  a script has and it fades the dialogue box out first, over 0.35s, so
        //  a 0.3s Hold is a blink rather than a pause. What is here instead is
        //  the type speed, which is real and large, and Holds between groups of
        //  thoughts rather than between every line. A true per-line pause is one
        //  float on BeatData and three lines in StoryDirector.WaitForLine, in
        //  files shared with six other acts, and it was not done from inside a
        //  task scoped to act two.
        //
        //  ── Clarity ────────────────────────────────────────────────────────
        //
        //  The narrator was cut back in fifteen places, and every cut was the
        //  same cut: a physical sentence with an explanatory clause bolted on
        //  the end. "She got hers out without saying anything about it" told
        //  the player that nobody acknowledged he was right; "both of them were
        //  in the third stand" shows it and lets her silence be silent. "Yua
        //  waited. She did not ask. She has never asked him about it" was the
        //  worst of them — the narrator standing in front of the silence
        //  explaining what kind of silence it was. It is now a girl not sitting
        //  down and a car going up a road.
        //
        //  What is left for the narrator is weather, light, objects arriving,
        //  bodies moving and time passing. Nothing in the act now tells the
        //  player how a frame felt, confirms that a character was right, or
        //  names what a silence meant.
        //
        //  Every object is named in full before it becomes "it": the yellow
        //  umbrella, the third stand, Tofu, the flyer, the salty lychee row,
        //  the pencil case, the rabbit. Every line reads against the picture it
        //  is spoken over — the roof has no vending machine in it and none is
        //  mentioned, the corridor has no drinks machine and none is mentioned,
        //  and the café is the rainy one because act four says it was raining.
        //  Going somewhere is always discussed before the background changes:
        //  the café is named on Monday evening and again on Wednesday morning,
        //  the platform on Thursday morning. Nothing is said about an action
        //  that has already finished. Every frame is a complete Persian
        //  sentence, and the narration is in the same spoken Persian as the
        //  dialogue — "رو" not "را", "اون" not "آن", "بارون" not "باران" — with
        //  no simile anywhere in the narrator's mouth.
        //
        //  ── Sound ──────────────────────────────────────────────────────────
        //
        //  These are counted, not estimated. An earlier draft of this block
        //  reported "twenty-eight cues across roughly five hundred and twenty
        //  frames", and both halves of that were wrong in a way worth writing
        //  down: the denominator was every beat kind in the act — a hundred and
        //  fourteen of which are silent held pictures — rather than frames, and
        //  the ratio it produced flattered a script that was in fact running
        //  well ABOVE the manual's ceiling. An audit that reports a number
        //  nobody counted is not an audit. Count the beats.
        //
        //  Counting rule used here, so the next person gets the same answer: a
        //  frame is a Line beat, which is Say, SayWithSound, Narrate, Listen,
        //  InnerVoice and ChildVoice. A cue is a Sound beat or a sound carried
        //  on a line, which is Cue and SayWithSound. Choice roads are counted
        //  once, not twice, because one playthrough only walks one of them.
        //
        //      frames on one playthrough   313
        //      cues                         20
        //      overall                      one in 15.7
        //
        //  Which is inside the manual's fifteen-to-twenty-five. Four scenes sit
        //  under fifteen and every cue in them is load-bearing rather than
        //  texture: the leg on Tuesday, the machine room and the heartbeat on
        //  Wednesday, and in the last scene the buttons arriving and the music
        //  being taken off. There is no scene where a cue is there to make a
        //  ratio look right — eight of those were written and then cut once the
        //  denominator was counted properly.
        //
        //  The café runs at one in twenty-four and that is the point of it:
        //  four cues in ninety-five frames means the monologue plays into a
        //  room doing almost nothing, so the two that do land inside it — the
        //  machine room under his sentence about a dark place, and a heartbeat
        //  — are the loudest things in the act.
        //
        //  The music is the act's own track throughout and is taken away
        //  exactly once, on the last line of the last scene. That is a scene
        //  ending and not the design document's zero-millisecond cut, which
        //  belongs to act five and is spent three times in the whole game.
        //
        //  ── The ambient layer, which does not exist ─────────────────────────
        //
        //  Section seventeen of the manual asks for three independent layers,
        //  and one of them cannot be written from this file or from any other
        //  act. There is no ambient bed in this game and no way for a script to
        //  ask for one:
        //
        //    · AudioService plays one-shots and sets loop = false on every
        //      source it owns.
        //    · MusicService loops exactly one named track, chosen per act on
        //      the asset rather than per scene, so it cannot carry a room.
        //    · StoryBeatKind has Sound and Music and nothing between them.
        //    · SceneAudioInstaller, despite the name, only hangs click and
        //      hover sounds on UI controls.
        //
        //  So every scene in this act opens with no bed under it, and that is
        //  an engine gap and not a writing decision. Saying so here is the
        //  manual's own instruction — if you cannot do it, say so, do not write
        //  round it — and the alternative was to keep quiet and let the next
        //  act inherit the same silence with nothing on the record.
        //
        //  What it would take, roughly, in the order it would have to happen:
        //  an Ambience beat kind; a looping synthesised source in AudioService
        //  that survives a scene change and crossfades over about a second and
        //  a half; a bed named on each Place beat so that a change of location
        //  changes the room; and the ten recipes in section seventeen, which
        //  are already written as synth parameters and need no audio files.
        //  Until that exists, every act in this game is a dialogue box in a
        //  vacuum, and this act would use it in all nine scenes.
        //
        //  ── Production ─────────────────────────────────────────────────────
        //
        //  Four white choices — Monday morning, Tuesday lunch, Wednesday café,
        //  Thursday morning — each with two roads of four or five spoken lines,
        //  no stage or picture change inside any road, and both roads rejoining
        //  immediately. Zero blue or green choices, which is the point of the
        //  act.
        //
        //  Every line inside all eight roads is Yua's, and that is a rule now
        //  rather than an accident. The player is Yua. A button that makes Haru
        //  open his mouth is the player deciding what he says, which is not a
        //  thing this game ever lets them do — and five of the eight roads used
        //  to start with him. Worse, it made the choice read as picking a
        //  subject for the pair of them rather than picking what she goes off
        //  about, which is the only thing a white choice in this act is for.
        //
        //  So a road is now a short Yua riff — the poster, locker twelve, a
        //  cat's name she did not want to learn, the heat, how the cake is
        //  being divided, the rain, chalk dates, a plant — and Haru answers on
        //  the far side of the join, where the same three lines work after
        //  either road. He is not silenced by the change; he is moved to where
        //  the player cannot aim him.
        //
        //  Two of the four joins do more than tidy up. The cake road ends on
        //  "you have never said so, but you would, if I asked you", and Haru
        //  walks into "you have decided a lot of things today" without knowing
        //  what he is agreeing with. And Thursday's join is three frames of
        //  "probably", because the point of that morning is that he does not
        //  argue with either road — which lands straight into Yua noticing he
        //  is a different quiet. Every sound is an existing procedural cue and every sprite,
        //  pose and background used above is already in the project. All three
        //  languages carry the same information and leave the power in the same
        //  hands.
        //
        //  ── What this act would like and does not have ─────────────────────
        //
        //  Nothing it needs. Two things would each buy a real frame:
        //
        //    · A rain-wet exterior background. The rainy season is the whole
        //      weather of this act and the only two wet pictures in the project
        //      are indoors, which is why Tuesday and Thursday are dry days and
        //      why the walk home happens in heat rather than in rain.
        //    · A Haru "one beat late" pose — standing, weight off one leg.
        //      Tuesday evening uses the wince, which is the right picture for
        //      the moment itself and the wrong one for the four frames after
        //      it, where the act has to say it in narration instead.
        // =====================================================================
    }
}
