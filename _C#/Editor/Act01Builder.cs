// -----------------------------------------------------------------------------
//  The Frayed Red String
//  Act01Builder.cs  (Editor only)
//
//  Act one — Cherry Blossom Mirage (桜の幻) — written out as code.
//
//  Run it once from The Frayed Red String ▸ Build Act 01 From The Story
//  Document. It writes Assets/Story/Acts/Act01.asset, reusing the asset already
//  at that path if there is one.
//
//  ===========================================================================
//  WHAT THIS REWRITE FIXED, AND WHY IT WAS WORTH REWRITING
//  ===========================================================================
//
//  The draft this replaces was funny. That is not the reason it was replaced,
//  and most of its jokes are still here — the cat, the plant, the dango, the
//  teacher's "maybe", the machine that eats coins. What was wrong with it was
//  underneath the jokes:
//
//   1. IT WAS SET IN APRIL. The narrative document opens the game on the 2nd of
//      September 2024, in the first week of the SECOND term. The old act was
//      written in spring: cherry blossom on every other page, a girl worrying
//      about a brand new uniform, "the first day of high school" in its own
//      scene notes. The game's own locked facts forbid that last phrase
//      outright, because the whole point of a second-term opening is that a
//      Japanese player assumes the two of them made up "-pi" last term.
//
//   2. THE BLOSSOM WAS REAL. The act is called Cherry Blossom Mirage. A mirage
//      is a thing that is not there. If the path to school is actually full of
//      blossom then the title is a description, and the game has told the
//      player the truth on the title card — which is the one thing it must not
//      do. The alley is now full of dead leaves and nobody mentions it.
//
//   3. THEY WERE IN A UNIFORM NOBODY HAS DRAWN. All sixty character sprites
//      have these two in one outfit each, and neither of them is a uniform:
//      she is in a pink and blue frilled dress with a red cord at her wrists,
//      he is in a cream cardigan and grey trousers. The old act's opening
//      joke — the new uniform is a bit loose — pointed at something that is
//      not on screen, which is the scene lock broken in thirty seconds.
//
//      The replacement is better rather than merely correct: a girl worrying
//      about a dress she chose says more about her than a girl worrying about
//      a uniform the school chose. She picked it. Somebody has to approve of it.
//
//   4. THE ONE SCENE THAT HAD A JOB DID NOT DO IT. Friday's classmate exists
//      for exactly one reason: ぴ is real slang and a Japanese player hears the
//      whole of it the first time, where a Persian or English player hears a
//      nickname. So somebody outside the pair has to notice it out loud, once.
//      In the old act, Yua's last "Haru-pi" was six frames before the classmate
//      arrived, and the line immediately before the question was about a manga.
//      The player attached "what did you just call him?" to the manga. The
//      device fired at nothing.
//
//  ===========================================================================
//  THE RULES THIS ACT IS WRITTEN UNDER
//  ===========================================================================
//
//  Dialogue manual version eight, and above every other rule in it:
//
//      If this game had no secret in it at all, and were only a sweet high
//      school romance — would this scene be fun to play?
//
//  Acts one to four have to genuinely BE the cute game, not do an impression of
//  one. The fifth act is devastating only in proportion to how much the player
//  enjoyed the first. Concretely:
//
//    • Half of every scene, at least, is about a third thing. A teacher neither
//      of them has met, a bakery that changed hands, a cat, four dango on a
//      stick, a plant nobody named. Two people who only talk about each other
//      sound like an interrogation.
//    • Every frame carries warmth, a joke, excitement, stubbornness,
//      embarrassment, curiosity, a complaint or an offer. A frame that only
//      moves information is deleted.
//    • Line length is information. Every scene has at least one genuinely long
//      line — somebody excited about something and unwilling to drop it — and
//      several one-word ones.
//    • Haru wins one unimportant argument per scene and quietly loses one real
//      decision. Neither is ever named. If replacing all of his answers with
//      "okay" does not change the scene, he is not in it.
//    • Yua does at least one thing per scene that has nothing to do with Haru.
//    • The dread budget for the whole act is ten moments: one Monday, one
//      Tuesday, one Wednesday, two Thursday, two Friday, three Saturday. Zero
//      in the first fifty frames. Each is deniable with an ordinary sentence,
//      and all ten are listed by name in the self-audit at the foot of the file.
//    • The escalation ladder reaches rung three and stops. Knowing too much,
//      then a reaction half a degree too big, then — once, on the last day —
//      correcting his memory and being right. Rungs four and five belong to
//      act two and are not touched here.
//    • No clinical symptom is performed directly. Hypervigilance is not a line
//      about exits; it is half a second of silence when something hums under a
//      grate, and then she carries on.
//    • Nobody is ever surprised that the other one knows something, and nobody
//      ever explains how they know. Between two old friends, knowing is
//      ordinary. The player is meant to notice that nobody ever asks.
//    • Scene lock. A line may only point at something visible in the current
//      background, visible in the current sprite, or named in full earlier in
//      this same scene. Going somewhere is discussed before the background
//      changes, never after.
//    • Every choice in the act is white and none is counted. Blue and green do
//      not exist until act three and are not hinted at. But each white option
//      has real script behind it, because a choice the game does not answer
//      teaches the player across an hour that pressing things is pointless —
//      which is the one belief act two's refused blue button cannot afford.
//
//  Three languages, three performances, never three translations. What stays
//  constant is the information, the power, the pulse and where the pauses fall.
//  What changes is idiom, particles, register and where the joke lands.
//
//  What the design document is quiet about, and this script keeps quiet: the
//  player is meant to believe these two met this morning. They do not behave
//  like it, they are already "-pi" to one another in the first exchange, and
//  nothing explains why for another five acts.
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Narrative;
using TheFrayedRedString.Presentation;
using UnityEditor;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Act one's script.</summary>
    public sealed class Act01Builder : ActScriptWriter
    {
        protected override int ActNumber => 1;

        protected override string AssetName => "Act01";

        protected override LocalizedLine Title => L("Cherry Blossom Mirage", "桜の幻", "سرابِ شکوفه‌های گیلاس");

        /// <summary>
        /// Monday the 2nd of September 2024, and not the 1st.
        /// </summary>
        /// <remarks>
        /// The narrative document dates the act to the 1st, which is the day the
        /// second term opens. That day is a Sunday. Six school days run Monday
        /// to Saturday from the 2nd, which keeps both the document's month and
        /// the real calendar. See AboutProject/Acts.md, decision one.
        /// </remarks>
        protected override StoryDate StartDate => StoryCalendar.ActOneDayOne;

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

        // =====================================================================
        //  THE CONTINUITY TABLE
        //
        //  Nothing enters the dialogue below unless it is here.
        //
        //  Dates      Mon 2 Sep · Tue 3 · Wed 4 · Thu 5 · Fri 6 · Sat 7,
        //             September 2024. Second term, week one. The phrase "first
        //             day of school" never appears.
        //  Season     Early autumn. Warm in the afternoon, cool by the river in
        //             the evening. The maples at the top of the school path have
        //             turned; the ones at the bottom have not. No blossom
        //             anywhere, ever, and nobody says the word.
        //  Clothes    Her frilled dress and his cardigan, all six days. It is
        //             the only thing either of them has been drawn in. She is
        //             self-conscious about hers on Monday and nowhere else.
        //  Class      1-A, second floor, by the window. Morita-sensei.
        //  Carrying   A school bag each. He also has a paper bag from the bakery
        //             on Monday, and a pencil case whose zip is broken — which
        //             is never mentioned in this act and matters in act four.
        //  The plant  A pot on the classroom windowsill. Nobody has named it.
        //             Planted Monday, repeated Wednesday, paid off Saturday.
        //  The cat    A tabby on the bench outside the bakery, with a collar.
        //             Yua names it Anko on Tuesday and refuses to read the tag.
        //  The machine A pink drinks machine on the corner of the way home. It
        //             takes coins and gives nothing back. On Saturday it gives
        //             two cans at once, and that is the entire punchline.
        //  The test   Morita-sensei said "maybe" on Monday. Haru says her maybe
        //             means yes. He is right on Thursday.
        //  Dango      Four on a stick, from the stall by the station, Friday.
        //  Sounds     The school bell rings twice in six days, on the path,
        //             on Monday and on Friday — the first morning of the act
        //             and the last one. Nothing else in the act keeps time.
        // =====================================================================

        /// <summary>
        /// Six school days, Monday to Saturday.
        /// </summary>
        /// <remarks>
        /// Saturday is a half day and they spend it out of school, which is the
        /// only day of the six where the two of them are somewhere neither of
        /// them has to be. It is also where the act ends, and where three of the
        /// act's ten moments happen.
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
        //  Three things this act does often enough to name
        // ---------------------------------------------------------------------

        /// <summary>Characters per second for <see cref="InnerVoice"/> lines.</summary>
        /// <remarks>The normal rate is 45. Slow enough to read as thinking rather than speaking.</remarks>
        private const float InnerMonologueTypeSpeed = 28f;

        /// <summary>Yua thinking, with nobody to hear it.</summary>
        /// <remarks>
        /// Used for two entirely different jobs and that is deliberate. Most of
        /// them are a girl embarrassed about her own dress or losing an argument
        /// with a vending machine, which is the whole reason the player likes
        /// her; four of them, at the ends of days, are the act's dread budget.
        /// They come in the same voice, at the same speed, in the same box.
        /// </remarks>
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
        /// Twice in the whole act, and the second one is on the last day. Two
        /// tenths of a second, taken from her brightest face rather than her
        /// neutral one, because the recovery is the frightening half. Nothing in
        /// the scene remarks on it and nothing ever will.
        /// </para>
        /// <para>
        /// An earlier draft used it three times, which stops being a technique
        /// and becomes a tic — and worse, a tic the player can name.
        /// </para>
        /// </remarks>
        private void FaceSlips()
        {
            Enter(Speaker.Yua, Portrait.DeadEyes);
            Hold(SlipSeconds);
            Enter(Speaker.Yua, Portrait.Joyful);
        }

        /// <summary>
        /// The leaves on the school path, at the weight the day wants them.
        /// </summary>
        /// <remarks>
        /// Light all week. This is the first week of September and only the tops
        /// of the maples have turned; a real autumn wind is two months away, and
        /// a screen full of leaves in act one would spend in a morning something
        /// act three needs.
        /// </remarks>
        private void AutumnAir(float density = 0.16f)
        {
            Fall(FallKind.MapleLeaf, density, 3f);
        }

        // =====================================================================
        //  MONDAY — day one of six
        //
        //  Dread budget: 1. It is the last two frames of the day and nothing
        //  before it. The first fifty frames of the game have none at all.
        // =====================================================================

        private void WriteMonday()
        {
            WriteMondayAlley();
            WriteMondayClassroom();
            WriteMondayCorner();
        }

        // ---------------------------------------------------------------------
        //  Monday morning — the school path
        //
        //  ▣ Scene state
        //     Background   AutumnSchoolAlleyDay: a paved path, maples turning
        //                  red on the left and yellow on the right, leaves
        //                  across the stones, the school building on the right,
        //                  wooden benches, lanterns set into the ground.
        //     Date         Monday 2 September 2024, morning. Second term.
        //     Clothes      Her dress, his cardigan. Both as drawn.
        //     On stage     The maples, the leaves, the benches, the noticeboard
        //                  by the door.
        //     In hand      A school bag each. Haru also has a paper bag with two
        //                  buttered rolls in it.
        //     From before  Nothing. This is the first scene of the game.
        //
        //  This is the calibration scene and it is the one the manual writes out
        //  as the target. A girl worrying about her own dress, a boy defending
        //  his choice of shade with total confidence, and half the scene about a
        //  teacher neither of them has met and a bakery that has changed hands.
        //  Zero numbers. Zero clock. Zero dread.
        //
        //  He wins two unimportant arguments here — the shade, and whether the
        //  bread counts as breakfast — and loses the only real decision in the
        //  scene, which is where the two of them eat it. Nobody mentions that he
        //  lost it.
        // ---------------------------------------------------------------------

        private void WriteMondayAlley()
        {
            ClearStage();

            Place(
                Backgrounds.SchoolAlleyDay,
                "The path to school", "通学路", "راهِ مدرسه");

            Date(StoryCalendar.ActOneDayOne);

            AutumnAir();

            Hold(1.8f);

            Narrate(
                "The top of the path had gone red over the summer and the bottom had not caught up yet.",
                "坂の上の木は夏のあいだに赤くなって、下のほうはまだ追いついていなかった。",
                "بالای مسیر تابستون قرمز شده بود و پایینش هنوز نرسیده بود.");

            Enter(Speaker.Yua, Portrait.Neutral);

            InnerVoice(
                "Okay. Okay okay okay.",
                "よし。よしよしよし。",
                "خب. خب خب خب.");

            InnerVoice(
                "The hem is fine. The hem is completely fine.",
                "裾は平気。裾はぜんぜん平気。",
                "دامنش خوبه. دامنش کاملاً خوبه.");

            InnerVoice(
                "I ironed it twice.",
                "二回アイロンかけたし。",
                "دو بار اتوش کردم.");

            InnerVoice(
                "...Which is probably why it looks ironed twice.",
                "……だから二回アイロンかけた感じに見えるのかも。",
                "...احتمالاً واسه همینم هست که معلومه دو بار اتو شده.");

            Hold(1.0f);

            InnerVoice(
                "Where is he.",
                "どこにいるの。",
                "کجاست پس.");

            InnerVoice(
                "...Ah.",
                "……あ。",
                "...آها.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Haru-pi!",
                "ハルぴ!",
                "هاروپی!");

            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Neutral,
                "Oh. Morning.",
                "あ。おはよ。",
                "اِ. سلام.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Why are you standing under that one?",
                "なんでそこに立ってるの?",
                "چرا اون زیر وایسادی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "There's shade.",
                "日陰があるから。",
                "سایه داشت.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The whole path has shade.",
                "通学路ぜんぶ日陰でしょ。",
                "کلِ کوچه سایه داره.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "This one's better.",
                "ここのが、いい。",
                "این یکی بهتره.");

            Say(Speaker.Yua, Portrait.Pout,
                "Since when are you a shade expert?",
                "いつから日陰の専門家?",
                "از کِی تا حالا کارشناسِ سایه شدی؟");

            Say(Speaker.Haru, Portrait.Joyful,
                "Since now.",
                "今から。",
                "از الآن.");

            Say(Speaker.Yua, Portrait.Joyful,
                "(laughs)",
                "(笑う)",
                "(می‌خندد)");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Here. One's yours.",
                "はい。ひとつ、結愛ぴの。",
                "بیا. یکیش مالِ توئه.");

            Narrate(
                "There was a paper bag in his other hand with two rolls in it, and a butter stain going through one corner.",
                "もう片方の手には紙袋があって、パンが二つと、角に染みたバターのしみ。",
                "تو اون یکی دستش یه پاکتِ کاغذی بود با دو تا نون توش، و یه گوشه‌ش لکه‌ی کره افتاده بود.");

            Say(Speaker.Yua, Portrait.Surprised,
                "You went to the bakery? Before school?",
                "パン屋行ったの? 学校の前に?",
                "نونوایی رفتی؟ قبلِ مدرسه؟");

            Say(Speaker.Haru, Portrait.Sheepish,
                "It's on the way.",
                "通り道だから。",
                "سرِ راهه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It is not on the way. It's the opposite direction.",
                "通り道じゃないよ。逆方向でしょ。",
                "سرِ راه نیست. جهتش برعکسه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's on a way.",
                "ひとつの通り道ではある。",
                "سرِ یه راهی هست.");

            Hold(0.8f);

            Say(Speaker.Yua, Portrait.Neutral,
                "It's different.",
                "味、変わった。",
                "فرق کرده.");

            Say(Speaker.Haru, Portrait.Surprised,
                "You haven't eaten it.",
                "まだ食べてないでしょ。",
                "که هنوز نخوردیش.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I can see it's different.",
                "見ればわかる。",
                "از رو ظاهرش معلومه.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "The old man sold it in July. His daughter's got it now and she does the dough longer.",
                "七月におじさんが店を譲ったんだ。今は娘さんがやってて、生地を長めに寝かせてる。",
                "تیر ماه پیرمرده مغازه رو واگذار کرد. الآن دخترشه و خمیر رو بیشتر می‌خوابونه.");

            Say(Speaker.Yua, Portrait.Bored,
                "You have opinions about dough.",
                "生地に意見あるんだ。",
                "درباره‌ی خمیر نظر داری.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I have one opinion about dough.",
                "生地への意見はひとつだけ。",
                "درباره‌ی خمیر یه نظر دارم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "And you've been saving it.",
                "それを今まで温めてたと。",
                "و تا حالا نگهش داشته بودی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "For the right moment.",
                "ちょうどいい時のために。",
                "واسه لحظه‌ی مناسبش.");

            Hold(1.4f);

            Narrate(
                "Two girls went past them up the path with their phones out, photographing the red end of it.",
                "スマホを構えた二人組が坂を上っていって、赤いほうを撮っていた。",
                "دو تا دختر با گوشی از کنارشون رد شدن و رفتن بالا، از سمتِ قرمزش عکس می‌گرفتن.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Morita-sensei's ours this term.",
                "今学期、森田先生だって。",
                "این ترم خانمِ موریتا مالِ ماست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I heard.",
                "聞いた。",
                "شنیدم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "They say she's strict.",
                "厳しいらしいよ。",
                "می‌گن سختگیره.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Who says?",
                "誰が言ってるの?",
                "کی می‌گه؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "...I don't know. Someone was saying.",
                "……知らない。誰かが言ってた。",
                "...نمی‌دونم. یکی می‌گفت.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "Then don't say \"they say\". Say \"someone was saying\".",
                "じゃあ「らしい」じゃなくて「誰かが言ってた」でしょ。",
                "پس نگو «می‌گن». بگو «یکی می‌گفت».");

            Say(Speaker.Yua, Portrait.Pout,
                "Does it matter?",
                "違うの?",
                "فرقی داره؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Enormously.",
                "だいぶ。",
                "خیلی.");

            Say(Speaker.Yua, Portrait.Bored,
                "Fine. \"Someone was saying she's strict.\"",
                "はいはい。「誰かが、厳しいって言ってた」。",
                "باشه. «یکی می‌گفت سختگیره.»");

            Say(Speaker.Haru, Portrait.Joyful,
                "Better.",
                "うん。",
                "حالا شد.");

            Hold(1.2f);

            DecideIdly(
                "Eat it now", "今食べる", "همین الآن بخورش",
                "Keep it for the roof", "屋上まで取っておく", "واسه پشت‌بوم نگهش دار");

            Say(Speaker.Yua, Portrait.Neutral,
                "We're eating these at lunch. On the roof.",
                "これ、お昼に食べよ。屋上で。",
                "اینارو ناهار می‌خوریم. رو پشت‌بوم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's four floors.",
                "四階分あるけど。",
                "چهار طبقه‌ست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It is.",
                "そうだね。",
                "آره هست.");

            Hold(0.9f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Okay.",
                "わかった。",
                "باشه.");

            SayWithSound(Speaker.Yua, Portrait.Joyful, SfxId.SchoolBell, 0.7f,
                "That's the first one.",
                "一限のやつだ。",
                "اینم زنگِ اول.");

            Narrate(
                "They went up the red end of the path with everybody else.",
                "二人も、みんなと一緒に赤いほうへ坂を上がっていった。",
                "با بقیه از سمتِ قرمزِ مسیر رفتن بالا.");
        }

        // ---------------------------------------------------------------------
        //  Monday, second period — 1-A
        //
        //  ▣ Scene state
        //     Background   SunnyClassroomDay: desks in rows, a chalkboard, pink
        //                  curtains, an open window, a pot on the sill.
        //     Date         Monday 2 September, mid-morning.
        //     On stage     Desks, the chalkboard, the window, the plant.
        //     In hand      Nothing. The bags are under the desks.
        //     From before  The rolls, saved for the roof. Morita-sensei, whom
        //                  neither of them has met yet.
        //
        //  The plant is planted here and the manual is specific about how: stage
        //  one of a motif is completely obvious, even boring. Somebody says
        //  there is a plant. Somebody says nobody has named it. That is all it
        //  does today.
        //
        //  He wins the argument about whether plants get names. He loses where
        //  they are sitting, which was decided before he arrived and which
        //  nobody mentions.
        // ---------------------------------------------------------------------

        private void WriteMondayClassroom()
        {
            ClearStage();

            Place(
                Backgrounds.ClassroomDay,
                "1-A", "一年A組", "اول-الف");

            NoFall(1.5f);

            Hold(1.4f);

            Narrate(
                "The window at the back was open and the curtain kept going out and coming back.",
                "後ろの窓が開いていて、カーテンが出ては戻ってきていた。",
                "پنجره‌ی ته کلاس باز بود و پرده مدام می‌رفت بیرون و برمی‌گشت.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Narrate(
                "Haru put his bag down at the desk by the window and sat at the one behind it.",
                "ハルは窓際の席に鞄を置いて、その後ろの席に座った。",
                "هارو کیفشو گذاشت رو میزِ کنارِ پنجره و خودش پشتیش نشست.");

            Say(Speaker.Yua, Portrait.Smug,
                "Good.",
                "よし。",
                "خوبه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "What is?",
                "なにが?",
                "چی خوبه؟");

            Say(Speaker.Yua, Portrait.Neutral,
                "Nothing. The curtain.",
                "べつに。カーテン。",
                "هیچی. پرده.");

            Hold(1.0f);

            Say(Speaker.Yua, Portrait.Surprised,
                "There's a plant.",
                "植木鉢ある。",
                "یه گلدون هست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "There is.",
                "あるね。",
                "هست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Whose is it?",
                "誰の?",
                "مالِ کیه؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "The room's, I think.",
                "教室の、だと思う。",
                "فکر کنم مالِ کلاسه.");

            Say(Speaker.Yua, Portrait.Thinking,
                "It doesn't have a name.",
                "名前ない。",
                "اسم نداره.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "Plants don't get names.",
                "植物に名前はつけないよ。",
                "به گیاه که اسم نمی‌ذارن.");

            Say(Speaker.Yua, Portrait.Pout,
                "Says who.",
                "誰が決めたの。",
                "کی گفته.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Everyone. Everywhere. Always.",
                "みんな。どこでも。ずっと。",
                "همه. همه‌جا. همیشه.");

            Say(Speaker.Yua, Portrait.Bored,
                "That's three sources and you've checked none of them.",
                "三つ挙げて、ひとつも確かめてないでしょ。",
                "سه تا منبع گفتی و هیچ‌کدومشو چک نکردی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I've checked a lot of them.",
                "けっこう確かめたよ。",
                "خیلی‌هاشو چک کردم.");

            Hold(1.2f);

            SayWithSound(Speaker.Haru, Portrait.Neutral, SfxId.ChairScrape, 0.6f,
                "She's coming.",
                "来た。",
                "داره میاد.");

            Narrate(
                "Morita-sensei came in, said her name once, and said it again because nobody had written it down.",
                "森田先生が入ってきて、一度名前を言って、誰も書いていなかったのでもう一度言った。",
                "خانمِ موریتا اومد تو، یه بار اسمشو گفت، و چون هیشکی ننوشته بودش دوباره گفت.");

            Hold(1.6f);

            Narrate(
                "She wrote three things on the board, and rubbed out the third one, and left the first two.",
                "黒板に三つ書いて、三つめを消して、二つ残した。",
                "سه چیز رو تخته نوشت، سومی رو پاک کرد، و دوتای اول رو گذاشت بمونه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "What was the third one.",
                "三つめ、なんだった。",
                "سومیه چی بود.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "A date.",
                "日付。",
                "یه تاریخ.");

            Say(Speaker.Yua, Portrait.Surprised,
                "For what?",
                "なんの?",
                "واسه چی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "She said maybe a test. Then she rubbed it out.",
                "たぶんテスト、って言って、消した。",
                "گفت شاید امتحان. بعد پاکش کرد.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Then there's no test.",
                "じゃあテストないじゃん。",
                "پس امتحانی در کار نیست.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "Her maybe means yes.",
                "あの人の「たぶん」は、ある。",
                "«شاید»ِ اون یعنی حتماً.");

            Say(Speaker.Yua, Portrait.Pout,
                "You've known her for eleven minutes.",
                "知り合って十一分でしょ。",
                "یازده دقیقه‌ست می‌شناسیش.");

            Say(Speaker.Haru, Portrait.Joyful,
                "And in eleven minutes she rubbed out a date instead of not writing it.",
                "その十一分で、書かないんじゃなくて、書いてから消した。",
                "و توی همین یازده دقیقه، به جای اینکه ننویستش، نوشت و بعد پاکش کرد.");

            Hold(1.0f);

            Say(Speaker.Yua, Portrait.Bored,
                "...I hate when you do that.",
                "……そういうとこ、ほんと嫌い。",
                "...از این کارت متنفرم.");

            Say(Speaker.Haru, Portrait.Sheepish,
                "Sorry.",
                "ごめん。",
                "ببخشید.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Don't apologise, it's worse.",
                "謝るともっと嫌。",
                "عذرخواهی نکن، بدتره.");

            DecideIdly(
                "Write the date down", "日付を書いておく", "تاریخ رو یادداشت کن",
                "Don't write it down", "書かない", "یادداشت نکن");

            Hold(1.4f);

            Narrate(
                "The curtain went out and came back. The plant did nothing at all.",
                "カーテンが出て、戻ってきた。植木鉢は、なにもしなかった。",
                "پرده رفت بیرون و برگشت. گلدون هیچ کاری نکرد.");
        }

        // ---------------------------------------------------------------------
        //  Monday, going home — the corner with the machine
        //
        //  ▣ Scene state
        //     Background   PastelStreetVendingAutumnDay: a residential street,
        //                  a pink drinks machine on the left, a bicycle against
        //                  the wall, flowerbeds still in flower. The spring
        //                  version of this corner is a separate file and is not
        //                  used between September and March.
        //     Date         Monday 2 September, late afternoon.
        //     On stage     The machine, the bicycle, the flowerbeds.
        //     In hand      Bags. The rolls are gone.
        //     From before  Morita-sensei's maybe. The plant with no name.
        //
        //  The machine is established here and it has to be established properly,
        //  because on Saturday it is the last joke in the act. It takes his coin
        //  and gives him nothing, he refuses to hit it, she hits it, nothing
        //  happens, and they walk away. That is the whole scene.
        //
        //  ◆ DREAD 1 — and it is the last two frames of the day. The manual
        //  writes this exact pair out as its worked example, and it earns it:
        //  first time through it is two nice frames about a nice day, and the
        //  player does not get frightened at all. They only remember it later.
        // ---------------------------------------------------------------------

        private void WriteMondayCorner()
        {
            ClearStage();

            Place(
                Backgrounds.VendingStreetDay,
                "The corner", "曲がり角", "سرِ نبش");

            AutumnAir(0.10f);

            Hold(1.6f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Unchanged,
                "There was a test date.",
                "やっぱり日付、あった。",
                "تاریخِ امتحان بود.");

            Say(Speaker.Yua, Portrait.Pout,
                "There was a rubbed-out test date.",
                "消された日付ね。",
                "تاریخِ امتحانِ پاک‌شده بود.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Those are the ones that happen.",
                "消したやつが、いちばん来る。",
                "همون‌هان که اتفاق می‌افتن.");

            Hold(1.0f);

            Narrate(
                "The machine on the corner was pink and had a bicycle leaning on the wall beside it.",
                "角の自販機はピンクで、その横の壁に自転車が立てかけてあった。",
                "دستگاهِ سرِ نبش صورتی بود و بغلش یه دوچرخه به دیوار تکیه داده بود.");

            Say(Speaker.Yua, Portrait.Surprised,
                "I've got a hundred yen.",
                "百円ある。",
                "صد ین دارم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "So have I.",
                "僕もある。",
                "منم دارم.");

            Say(Speaker.Yua, Portrait.Smug,
                "Then you go first.",
                "じゃあハルぴから。",
                "پس تو اول برو.");

            Say(Speaker.Haru, Portrait.Surprised,
                "Why me first?",
                "なんで僕から?",
                "چرا من اول؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "Research.",
                "実験。",
                "تحقیق.");

            Narrate(
                "He put the coin in the machine.",
                "ハルは自販機に百円を入れた。",
                "سکه رو انداخت تو دستگاه.");

            Cue(SfxId.VendingThunk, 0.8f);

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "...Hm.",
                "……ん。",
                "...هوم.");

            Say(Speaker.Yua, Portrait.Surprised,
                "Where did it go?",
                "どこ行ったの?",
                "کجا رفت؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Inside.",
                "中。",
                "تو.");

            Say(Speaker.Yua, Portrait.Bored,
                "Yes. Thank you.",
                "そうですね。ありがとう。",
                "بله. ممنون.");

            Say(Speaker.Haru, Portrait.Sheepish,
                "It's an old one. The coin slot's tired.",
                "古い機械だから。投入口がくたびれてる。",
                "دستگاهِ قدیمیه. شیارِ سکه‌ش خسته‌ست.");

            Say(Speaker.Yua, Portrait.Pout,
                "Hit it.",
                "叩いて。",
                "بزنش.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "I'm not hitting a machine in the middle of the street.",
                "道の真ん中で機械は叩かない。",
                "من وسطِ خیابون به دستگاه نمی‌زنم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Nobody's watching.",
                "誰も見てないよ。",
                "کسی نگاه نمی‌کنه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I'm watching.",
                "僕が見てる。",
                "من نگاه می‌کنم.");

            Hold(0.9f);

            Narrate(
                "Yua hit the machine.",
                "結愛が自販機を叩いた。",
                "یوآ زد به دستگاه.");

            Cue(SfxId.DeskKnock, 0.55f);

            Hold(1.3f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Nothing.",
                "なにも出ない。",
                "هیچی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Now there are two of us watching you do that.",
                "今ので、見てた人が二人になった。",
                "حالا دو نفر دیدن که این کارو کردی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Worth it.",
                "後悔はない。",
                "می‌ارزید.");

            Hold(1.0f);

            Say(Speaker.Haru, Portrait.Neutral,
                "There's a slot on the side for a refund.",
                "横に返却のとこあるよ。",
                "بغلش یه شیار واسه پس گرفتن داره.");

            Say(Speaker.Yua, Portrait.Surprised,
                "There's a what?",
                "なにがあるって?",
                "چی داره؟");

            Say(Speaker.Haru, Portrait.Sheepish,
                "A refund slot. It doesn't work either.",
                "返却口。あれも壊れてる。",
                "شیارِ پس گرفتن. اونم کار نمی‌کنه.");

            Say(Speaker.Yua, Portrait.Bored,
                "Then why did you tell me about it.",
                "じゃあなんで言ったの。",
                "پس چرا اصلاً بهم گفتی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Completeness.",
                "いちおう。",
                "محضِ کامل بودن.");

            DecideIdly(
                "Try your own hundred yen", "自分の百円も入れてみる", "صدینِ خودتم امتحان کن",
                "Keep your hundred yen", "百円は取っておく", "صدینِ خودتو نگه دار");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Tomorrow, then. The bakery street, not this one.",
                "じゃあ明日。こっちじゃなくて、パン屋のほうの道。",
                "پس فردا. خیابونِ نونوایی، نه این یکی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The bakery street.",
                "パン屋のほうね。",
                "خیابونِ نونوایی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's longer.",
                "遠回りだけど。",
                "درازتره.");

            Say(Speaker.Yua, Portrait.Smug,
                "It is.",
                "そうだね。",
                "آره هست.");

            Hold(1.0f);

            Say(Speaker.Haru, Portrait.Neutral,
                "See you tomorrow, Yua-pi.",
                "また明日、結愛ぴ。",
                "فردا می‌بینمت، یوآپی.");

            Exit(Speaker.Haru);

            Narrate(
                "He went down the hill and she stayed on the corner for a moment, next to the machine that had her friend's money in it.",
                "ハルは坂を下りていって、結愛は少しだけ角に残った。友だちの百円が入ったままの自販機の横に。",
                "اون از سرازیری رفت پایین و یوآ یه لحظه سرِ نبش موند، کنارِ دستگاهی که پولِ دوستش توش بود.");

            Hold(2.0f);

            // ◆ DREAD 1. Two frames, and the entire day's budget. First reading:
            // she is fond of him. Second reading: she has been checking.
            InnerVoice(
                "Good.",
                "よかった。",
                "خوبه.");

            InnerVoice(
                "Still the same.",
                "まだ、おんなじ。",
                "هنوز همون‌جوریه.");
        }

        // =====================================================================
        //  TUESDAY — day two of six
        //
        //  Dread budget: 1, in the bakery scene, and it is one line long.
        // =====================================================================

        private void WriteTuesday()
        {
            WriteTuesdayClassroom();
            WriteTuesdayBakery();
        }

        // ---------------------------------------------------------------------
        //  Tuesday, morning — 1-A, raining
        //
        //  ▣ Scene state
        //     Background   OvercastClassroomRainy: the same room under grey
        //                  light, rain on the glass, the curtain moving, a
        //                  bookshelf at the back.
        //     Date         Tuesday 3 September, morning.
        //     On stage     Desks, the chalkboard, the window, the plant, the
        //                  bookshelf.
        //     From before  The rubbed-out date. The nameless plant. The machine
        //                  that has his hundred yen.
        //
        //  A short, light scene whose only jobs are to keep the test argument
        //  alive and to get them to the bakery street, which is where the day
        //  actually happens. He wins the argument about the window. She decides
        //  they are going to the bakery, and does it by having already decided.
        // ---------------------------------------------------------------------

        private void WriteTuesdayClassroom()
        {
            ClearStage();

            Place(
                Backgrounds.ClassroomRainy,
                "1-A", "一年A組", "اول-الف");

            Date(2024, 9, 3);

            NoFall(0f);

            Hold(1.6f);

            Narrate(
                "It had started in the night and had not got bored of it yet.",
                "夜のうちに降りはじめて、まだ飽きていなかった。",
                "شب شروع شده بود و هنوز ازش خسته نشده بود.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Pout,
                "Shut the window.",
                "窓、閉めて。",
                "پنجره رو ببند.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It doesn't shut.",
                "閉まらないんだ。",
                "بسته نمی‌شه.");

            Say(Speaker.Yua, Portrait.Surprised,
                "What do you mean it doesn't shut.",
                "閉まらないってどういうこと。",
                "یعنی چی بسته نمی‌شه.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "It goes about that far and then it stops. Since last year, apparently.",
                "そこまでで止まる。去年からずっとらしいよ。",
                "تا اینجا می‌ره و وایمیسته. ظاهراً از پارساله.");

            Say(Speaker.Yua, Portrait.Bored,
                "Apparently.",
                "らしい、ね。",
                "ظاهراً.");

            Say(Speaker.Haru, Portrait.Sheepish,
                "Someone was saying.",
                "誰かが言ってた。",
                "یکی می‌گفت.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Look at you.",
                "えらいえらい。",
                "به به.");

            Hold(1.2f);

            Narrate(
                "The rain came in through the gap the window would not close and made a dark line along the sill.",
                "閉まりきらない隙間から雨が入って、窓枠に黒い線をつくっていた。",
                "بارون از همون درزی که پنجره نمی‌بستش می‌اومد تو و رو لبه‌ی پنجره یه خطِ تیره درست کرده بود.");

            Say(Speaker.Yua, Portrait.Surprised,
                "It's getting wet.",
                "濡れてる。",
                "داره خیس می‌شه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It is.",
                "濡れてるね。",
                "آره داره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Not the sill. The plant.",
                "窓枠じゃなくて。植木鉢。",
                "نه لبه‌ی پنجره. گلدون.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Plants like that.",
                "植物はそれが好きでしょ。",
                "گیاه‌ها که از این خوششون میاد.");

            Say(Speaker.Yua, Portrait.Pout,
                "Not from the side.",
                "横からは好きじゃない。",
                "از پهلو نه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It has no opinion about the angle.",
                "角度に意見はないと思うけど。",
                "درباره‌ی زاویه‌ش نظری نداره.");

            Say(Speaker.Yua, Portrait.Thinking,
                "It would if it had a name.",
                "名前があったら、あるよ。",
                "اگه اسم داشت، داشت.");

            Say(Speaker.Haru, Portrait.Bored,
                "We're not doing this again.",
                "その話、また?",
                "بازم این بحث؟");

            Say(Speaker.Yua, Portrait.Smug,
                "We're doing it for six years.",
                "六年やるよ。",
                "شش سال ادامه‌ش می‌دیم.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "It'll have stopped by four.",
                "四時には止むよ。",
                "تا چهار بند میاد.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "And if it hasn't?",
                "止まなかったら?",
                "و اگه نیومد؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Then we go the short way.",
                "じゃあ近いほうで帰る。",
                "اون‌وقت از راهِ کوتاهه می‌ریم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "We're going past the bakery.",
                "パン屋の前、通るよ。",
                "از جلوی نونوایی رد می‌شیم.");

            Hold(0.8f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Okay.",
                "うん。",
                "باشه.");
        }

        // ---------------------------------------------------------------------
        //  Tuesday, going home — the bakery street
        //
        //  ▣ Scene state
        //     Background   UsagiBakeryStreetDay: cobbles, the Usagi Bakery
        //                  window full of bread, a wooden bench with cats on it,
        //                  pots of flowers.
        //     Date         Tuesday 3 September, late afternoon. The rain stopped.
        //     On stage     The window, the bench, the cats, the flowers.
        //     From before  The bakery changed hands in July. The daughter does
        //                  the dough longer.
        //
        //  Yua's scene. She names a cat, and she will not read its collar, and
        //  she talks for longer about why than she has talked about anything
        //  else in two days. This is the act's "she does one thing that has
        //  nothing to do with him", and it is also the reason the player likes
        //  her — which is the thing the fifth act spends.
        //
        //  ◆ DREAD 2, one line, in the middle of it. She knows what is written
        //  on a collar she has refused to look at. Deniable: she looked
        //  yesterday, or last week, or any day at all. Nobody asks.
        // ---------------------------------------------------------------------

        private void WriteTuesdayBakery()
        {
            ClearStage();

            Place(
                Backgrounds.BakeryStreetDay,
                "Outside Usagi Bakery", "うさぎベーカリーの前", "جلوی نونواییِ اوساگی");

            AutumnAir(0.08f);

            Hold(1.6f);

            Narrate(
                "It had stopped at about ten past four and the cobbles were still dark.",
                "四時十分ごろに止んで、石畳はまだ黒かった。",
                "حدودِ چهار و ده دقیقه بند اومده بود و سنگ‌فرش هنوز تیره بود.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.CalmSerious,
                "Ten past four.",
                "四時十分。",
                "چهار و ده دقیقه.");

            Say(Speaker.Yua, Portrait.Bored,
                "Don't.",
                "やめて。",
                "نگو.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I said four.",
                "四時って言った。",
                "من گفتم چهار.");

            Say(Speaker.Yua, Portrait.Pout,
                "You said four. It was ten past. You were wrong for ten minutes and I was right for ten minutes.",
                "四時って言った。実際は十分過ぎ。ハルぴは十分まちがってて、あたしは十分正しかった。",
                "گفتی چهار. ده دقیقه بعدش بود. تو ده دقیقه اشتباه می‌کردی و من ده دقیقه درست می‌گفتم.");

            Say(Speaker.Haru, Portrait.Sheepish,
                "That's a way of putting it.",
                "そういう言い方もある。",
                "می‌شه این‌جوری هم گفتش.");

            Hold(1.2f);

            Narrate(
                "There were two cats on the bench outside the window. One of them was asleep and one of them was not.",
                "ガラスの前のベンチに猫が二匹。一匹は寝ていて、一匹は寝ていなかった。",
                "رو نیمکتِ جلوی ویترین دو تا گربه بودن. یکیشون خواب بود و یکیشون نه.");

            Say(Speaker.Yua, Portrait.Surprised,
                "Oh.",
                "あ。",
                "اوه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Oh, oh, look at it.",
                "うわ、見て、見てよ。",
                "وای، وای، نگاش کن.");

            Say(Speaker.Haru, Portrait.Neutral,
                "I'm looking at it.",
                "見てるよ。",
                "دارم نگاش می‌کنم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Its name is Anko.",
                "この子の名前、あんこ。",
                "اسمش آنکوئه.");

            Say(Speaker.Haru, Portrait.Surprised,
                "It has a name. It's on the collar.",
                "名前あるよ。首輪に書いてある。",
                "اسم داره. رو قلاده‌ش نوشته.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I'm not reading that.",
                "読まない。",
                "من اونو نمی‌خونم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Why not?",
                "なんで?",
                "چرا نه؟");

            // The act's long line. Somebody excited about something who will not
            // let go of it, exactly where the manual asks for one.
            Say(Speaker.Yua, Portrait.Joyful,
                "Because if I read it I'll know it, and then it stops being Anko, and I have to carry that for the rest of my life. There are things it's better not to know. Cats are one. Honestly there are about four things in the world I want to know less than that cat's real name, and two of them are what's in the third-year toilets.",
                "だって読んだら知っちゃうでしょ。そしたら、あんこじゃなくなる。それを一生かかえて生きるの。知らないほうがいいことってあるんだよ。猫はそのひとつ。ていうか、あの猫の本名より知りたくないことなんて世界に四つくらいしかなくて、そのうち二つは三年のトイレの話。",
                "چون اگه بخونمش می‌دونمش، بعدش دیگه آنکو نیست، و باید تا آخرِ عمرم اینو با خودم حمل کنم. یه چیزایی هست که ندونستنش بهتره. گربه یکیشونه. اصلاً توی کلِ دنیا چهار تا چیز بیشتر نیست که کمتر از اسمِ واقعیِ این گربه بخوام بدونمشون، و دوتاش مربوط به دستشوییِ سومی‌هاست.");

            Hold(1.0f);

            Say(Speaker.Haru, Portrait.Bored,
                "...Four.",
                "……四つ。",
                "...چهار تا.");

            Say(Speaker.Yua, Portrait.Smug,
                "About four.",
                "だいたい四つ。",
                "حدودِ چهار تا.");

            Hold(1.2f);

            Narrate(
                "The cat that was not asleep looked at both of them and decided against it.",
                "寝ていないほうの猫が二人を見て、やめておくことにした。",
                "گربه‌ای که خواب نبود به هردوشون نگاه کرد و منصرف شد.");

            // ◆ DREAD 2. One line. She has not looked at the collar in this
            // scene, and says what is on it. Deniable a dozen ways, and nobody
            // in the scene reaches for any of them.
            Say(Speaker.Yua, Portrait.Neutral,
                "It's four characters anyway. Anko's two. I did it a favour.",
                "どうせ四文字だし。あんこは二文字。得させてあげた。",
                "تازه چهار حرفیه. آنکو دو حرفیه. بهش لطف کردم.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "The bread's half price after five.",
                "五時過ぎたら、パン半額。",
                "بعد از پنج نون نصفِ قیمته.");

            Say(Speaker.Yua, Portrait.Surprised,
                "Is it?",
                "そうなの?",
                "جدی؟");

            Say(Speaker.Haru, Portrait.Sheepish,
                "The sign says so. It's been saying so since July.",
                "貼り紙にそう書いてある。七月からずっと。",
                "رو تابلوش نوشته. از تیر تا حالا همین‌جور نوشته.");

            Say(Speaker.Yua, Portrait.Pout,
                "You've been buying it in the morning at full price.",
                "で、朝に定価で買ってるんだ。",
                "و تو صبح‌ها به قیمتِ کامل می‌خریش.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "In the morning it's warm.",
                "朝はあったかいから。",
                "صبح‌ها گرمه.");

            Hold(0.9f);

            Say(Speaker.Yua, Portrait.Joyful,
                "...Okay, that's a good reason.",
                "……それは、いい理由。",
                "...باشه، دلیلِ خوبیه.");

            DecideIdly(
                "Wait until five", "五時まで待つ", "تا پنج صبر کن",
                "Go home now", "もう帰る", "همین الآن برو خونه");

            Hold(1.2f);

            Narrate(
                "The cat went under the bench. The other one stayed where it was and carried on not caring.",
                "猫はベンチの下に入った。もう一匹はそのままで、あいかわらず気にしていなかった。",
                "گربه رفت زیرِ نیمکت. اون یکی همون‌جا موند و همچنان اهمیتی نداد.");
        }

        // =====================================================================
        //  WEDNESDAY — day three of six
        //
        //  Dread budget: 1, and for the first time it is his rather than hers.
        // =====================================================================

        private void WriteWednesday()
        {
            WriteWednesdayRoof();
            WriteWednesdayCorridor();
        }

        // ---------------------------------------------------------------------
        //  Wednesday, lunch — the roof
        //
        //  ▣ Scene state
        //     Background   SchoolRooftopSunnyDay: wire mesh fence, potted
        //                  plants, a wooden bench with a cushion on it, the city
        //                  a long way down, big clouds.
        //     Date         Wednesday 4 September, lunch.
        //     On stage     The fence, the bench, the cushion, the pots.
        //     In hand      A wooden bento box, hers. He has nothing until she
        //                  opens it, which is why his pictures run one behind.
        //     From before  The roof was promised on Monday and this is it.
        //
        //  The act's only wordless sequence: ten pictures of a lunch, and the
        //  numbers in them are real. Six sushi and four sausages go in; four and
        //  three come out onto his lid. She says the numbers once and the frames
        //  do the rest.
        //
        //  ◆ DREAD 3 — and it is HIS. He puts his bag on the left side of the
        //  bench before she has said anything, and she sits down on the right
        //  without looking. Nobody comments. First reading: they are used to
        //  each other. Second reading: he has been trained, and she never had to
        //  ask twice about anything.
        // ---------------------------------------------------------------------

        private void WriteWednesdayRoof()
        {
            ClearStage();

            Place(
                Backgrounds.RooftopDay,
                "The roof", "屋上", "پشت‌بام");

            Date(2024, 9, 4);

            AutumnAir(0.06f);

            Hold(1.8f);

            Narrate(
                "Four floors up, the city was small enough to hold and the clouds were not moving anywhere.",
                "四階分上がると、街は手のひらくらいで、雲はどこにも行かなかった。",
                "چهار طبقه بالاتر، شهر اون‌قدر کوچیک بود که تو دست جا می‌شد و ابرها هیچ‌جا نمی‌رفتن.");

            Enter(Speaker.Haru, Portrait.Neutral);

            // ◆ DREAD 3. The whole of it is this and the line after it.
            Narrate(
                "Haru put his bag on the left end of the bench and sat down next to it.",
                "ハルはベンチの左端に鞄を置いて、その横に座った。",
                "هارو کیفشو گذاشت سمتِ چپِ نیمکت و بغلش نشست.");

            Enter(Speaker.Yua, Portrait.Neutral);

            Narrate(
                "Yua sat down on the right.",
                "結愛は右に座った。",
                "یوآ سمتِ راست نشست.");

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Joyful,
                "Four floors.",
                "四階分。",
                "چهار طبقه.");

            Say(Speaker.Haru, Portrait.StrainedSmile,
                "Four floors.",
                "四階分。",
                "چهار طبقه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You're out of breath.",
                "息あがってる。",
                "نفست بند اومده.");

            Say(Speaker.Haru, Portrait.Sheepish,
                "I'm fine.",
                "平気。",
                "خوبم.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Smug,
                "I made too much.",
                "作りすぎた。",
                "زیادی درست کردم.");

            Say(Speaker.Haru, Portrait.Surprised,
                "You made lunch?",
                "お弁当、作ったの?",
                "ناهار درست کردی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I made too much lunch. It's different.",
                "お弁当を、作りすぎた。ちょっと違う。",
                "زیادی ناهار درست کردم. فرق داره.");

            Cel(Portrait.LunchOpen);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Six and four.",
                "六つと、四つ。",
                "شیش تا و چهار تا.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Six and four what?",
                "六つと四つ、なに?",
                "شیش تا و چهار تا چی؟");

            Cel(Portrait.LunchOffer);

            Say(Speaker.Haru, Portrait.Joyful,
                "...Oh.",
                "……あ。",
                "...اوه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Hold the lid out.",
                "ふた、出して。",
                "درشو بگیر جلو.");

            Cel(Portrait.LunchShared);

            Say(Speaker.Haru, Portrait.Surprised,
                "That's most of it.",
                "ほとんどじゃん。",
                "این که بیشترشه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I said too much.",
                "作りすぎたって言ったでしょ。",
                "گفتم که زیادی درست کردم.");

            Say(Speaker.Haru, Portrait.Sheepish,
                "You said too much. You didn't say most of it.",
                "作りすぎたとは言った。ほとんどとは言ってない。",
                "گفتی زیادی. نگفتی بیشترش.");

            Say(Speaker.Yua, Portrait.Pout,
                "Eat it before I take it back.",
                "返してもらう前に食べて。",
                "قبل از اینکه پسش بگیرم بخورش.");

            Cel(Portrait.LunchFirstLift);
            Cel(Portrait.LunchFirstBite);

            Say(Speaker.Haru, Portrait.Unchanged,
                "...The sausage is shaped like something.",
                "……ソーセージ、なんかの形してる。",
                "...سوسیسه شکلِ یه چیزیه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "It's an octopus.",
                "たこ。",
                "اختاپوسه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It has six legs.",
                "足、六本ある。",
                "شیش تا پا داره.");

            Say(Speaker.Yua, Portrait.Bored,
                "It has as many legs as the knife wanted.",
                "包丁が出したぶんだけ足がある。",
                "به اندازه‌ای پا داره که چاقو خواسته.");

            Cel(Portrait.LunchSecondLift);
            Cel(Portrait.LunchSecondBite);

            Say(Speaker.Haru, Portrait.Joyful,
                "It's good.",
                "おいしい。",
                "خوبه.");

            Say(Speaker.Yua, Portrait.Smug,
                "Obviously.",
                "でしょ。",
                "معلومه.");

            Hold(1.0f);

            Say(Speaker.Haru, Portrait.Neutral,
                "There's a sports day thing in October. They're already asking for volunteers.",
                "十月に体育祭あって、もう係を募集してる。",
                "اکتبر جشنِ ورزشی دارن و از الآن داوطلب می‌خوان.");

            Say(Speaker.Yua, Portrait.Surprised,
                "In October?",
                "十月に?",
                "اکتبر؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's on the board by the stairs.",
                "階段のとこの掲示板に出てる。",
                "رو تابلوی کنارِ پله‌هاست.");

            Say(Speaker.Yua, Portrait.Pout,
                "We never read that board.",
                "あの掲示板、いつも読まないのに。",
                "ما که هیچ‌وقت اون تخته رو نمی‌خونیم.");

            Say(Speaker.Haru, Portrait.Sheepish,
                "I read it.",
                "僕は読んでる。",
                "من می‌خونمش.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You read the bakery sign and the stairs board. What else are you reading.",
                "パン屋の貼り紙と、階段の掲示板。ほかになに読んでるの。",
                "تابلوی نونوایی و تخته‌ی پله‌ها رو می‌خونی. دیگه چی می‌خونی.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "Everything, mostly.",
                "だいたい全部。",
                "تقریباً همه‌چی.");

            Cel(Portrait.LunchThirdLift);
            Cel(Portrait.LunchThirdBite);

            DecideIdly(
                "Volunteer for the sports day", "体育祭の係に立候補する", "واسه جشنِ ورزشی داوطلب شو",
                "Absolutely not", "ぜったい嫌", "به هیچ وجه");

            Cel(Portrait.LunchFinished);

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Don't take the stairs fast on the way down.",
                "下りは、急がないで。",
                "موقعِ پایین رفتن از پله‌ها تند نرو.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I won't.",
                "うん。",
                "نمی‌رم.");

            Hold(1.2f);

            Narrate(
                "The clouds had still not gone anywhere. Neither had the bag on the left end of the bench.",
                "雲はまだどこにも行っていなかった。ベンチの左端の鞄も。",
                "ابرها هنوز هیچ‌جا نرفته بودن. کیفِ سمتِ چپِ نیمکت هم همین‌طور.");
        }

        // ---------------------------------------------------------------------
        //  Wednesday, after school — the corridor
        //
        //  ▣ Scene state
        //     Background   SchoolCorridorAutumnSunset: an empty corridor in
        //                  gold light, long shadows, floor-to-ceiling windows
        //                  with turned maples behind them, lockers, and a
        //                  grille in the floor at the far end that this act
        //                  never looks at.
        //     Date         Wednesday 4 September, going home.
        //     On stage     The windows, the lockers, the light.
        //     From before  The plant. The window that does not shut.
        //
        //  Short and warm and with nothing in it. The plant motif takes its
        //  second step here, which per the manual is a plain repeat and nothing
        //  cleverer — the ambiguity is only allowed in the third one.
        // ---------------------------------------------------------------------

        private void WriteWednesdayCorridor()
        {
            ClearStage();

            Place(
                Backgrounds.CorridorSunset,
                "The second-floor corridor", "二階の廊下", "راهروی طبقه‌ی دوم");

            NoFall(0f);

            Hold(1.6f);

            Narrate(
                "The corridor took the last of the light all the way down its length and put it on the lockers.",
                "廊下は最後の光を端まで運んで、ロッカーの上に置いた。",
                "راهرو آخرین نورِ روز رو تا ته با خودش برد و گذاشت رو کمدها.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Neutral,
                "I watered it.",
                "水やった。",
                "آبش دادم.");

            Say(Speaker.Haru, Portrait.Surprised,
                "Watered what?",
                "なにに?",
                "به چی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The one on the sill.",
                "窓際の。",
                "همون که لبِ پنجره‌ست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's been raining into it for two days.",
                "二日間ずっと雨入ってたけど。",
                "دو روزه که بارون توش می‌ره.");

            Say(Speaker.Yua, Portrait.Pout,
                "From the side.",
                "横からね。",
                "از پهلو.");

            Say(Speaker.Haru, Portrait.Bored,
                "From the side.",
                "横から、ね。",
                "از پهلو.");

            Say(Speaker.Yua, Portrait.Smug,
                "I'm glad we agree.",
                "意見が合ってよかった。",
                "خوشحالم که هم‌نظریم.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Still no name, though.",
                "でも名前はまだない。",
                "ولی هنوز اسم نداره.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I'm working on it.",
                "考え中。",
                "روش کار می‌کنم.");

            Hold(1.0f);

            Narrate(
                "Somewhere below them a door went, and the sound came up the stairwell and got tired halfway.",
                "下のほうでドアが閉まって、その音は階段を上ってきて、途中で疲れた。",
                "یه جایی پایین دری بسته شد و صداش از راه‌پله اومد بالا و نصفه‌راه خسته شد.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Café tomorrow?",
                "明日、喫茶店行く?",
                "فردا کافه بریم؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Tomorrow the café.",
                "明日は喫茶店。",
                "فردا کافه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I said that.",
                "僕が言った。",
                "من گفتمش.");

            Say(Speaker.Yua, Portrait.Smug,
                "You did.",
                "うん、言った。",
                "آره گفتی.");
        }

        // =====================================================================
        //  THURSDAY — day four of six
        //
        //  Dread budget: 2. One in the café, one on the way home, and they are
        //  the first two that are not a monologue: they happen in front of him.
        // =====================================================================

        private void WriteThursday()
        {
            WriteThursdayClassroom();
            WriteThursdayCafe();
            WriteThursdayAlley();
        }

        // ---------------------------------------------------------------------
        //  Thursday, morning — 1-A
        //
        //  ▣ Scene state
        //     Background   SunnyClassroomDay.
        //     Date         Thursday 5 September, morning.
        //     On stage     Desks, the chalkboard with a date on it, the window,
        //                  the plant.
        //     From before  Morita-sensei's rubbed-out maybe, three days old.
        //
        //  The test argument pays off: he was right, and the way he is right
        //  about it is the joke. She loses and does not concede, which is the
        //  correct shape — she never concedes, she changes the subject to
        //  something she has already decided.
        // ---------------------------------------------------------------------

        private void WriteThursdayClassroom()
        {
            ClearStage();

            Place(
                Backgrounds.ClassroomDay,
                "1-A", "一年A組", "اول-الف");

            Date(2024, 9, 5);

            NoFall(0f);

            Hold(1.6f);

            Narrate(
                "There was a date on the board that had not been there on Wednesday.",
                "水曜になかった日付が、黒板に書かれていた。",
                "رو تخته یه تاریخی بود که چهارشنبه نبود.");

            Enter(Speaker.Yua, Portrait.Surprised);
            Enter(Speaker.Haru, Portrait.Neutral);

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.CalmSerious,
                "Mm.",
                "ふむ。",
                "هوم.");

            Say(Speaker.Yua, Portrait.Pout,
                "Don't you dare make a sound about this.",
                "その音、出さないで。",
                "جرئت نداری در این باره صدایی دربیاری.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I made one sound.",
                "一回しか出してない。",
                "فقط یه صدا درآوردم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It was a whole paragraph.",
                "段落ぶんあった。",
                "یه پاراگراف کامل بود.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It was \"mm\".",
                "「ふむ」だけ。",
                "فقط «هوم» بود.");

            Hold(1.0f);

            Say(Speaker.Yua, Portrait.Bored,
                "Her maybe means yes.",
                "あの人の「たぶん」は、ある。",
                "«شاید»ِ اون یعنی حتماً.");

            Say(Speaker.Haru, Portrait.Sheepish,
                "I didn't say it.",
                "言ってないよ。",
                "من که نگفتم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You've been not-saying it since Monday. It's louder than saying it.",
                "月曜からずっと言わないでいるでしょ。言うよりうるさい。",
                "از دوشنبه داری نمی‌گیش. از گفتنش بلندتره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's the best thing anyone's said to me this week.",
                "今週いちばん嬉しい。",
                "بهترین چیزیه که این هفته کسی بهم گفته.");

            Hold(1.4f);

            Narrate(
                "Morita-sensei wrote a second thing under the date and did not explain it.",
                "森田先生は日付の下にもうひとつ書いて、説明はしなかった。",
                "خانمِ موریتا زیرِ تاریخ یه چیزِ دوم نوشت و توضیحش نداد.");

            Say(Speaker.Yua, Portrait.Neutral,
                "There's a stall by the station on Fridays.",
                "金曜、駅んとこに屋台出る。",
                "جمعه‌ها کنارِ ایستگاه یه دکه هست.");

            Say(Speaker.Haru, Portrait.Surprised,
                "We're in the middle of a test announcement.",
                "今、テストの話の真ん中だけど。",
                "وسطِ اعلامِ امتحانیم.");

            Say(Speaker.Yua, Portrait.Smug,
                "We were. Now we're in the middle of a stall by the station.",
                "だった。今は屋台の話の真ん中。",
                "بودیم. الآن وسطِ دکه‌ی کنارِ ایستگاهیم.");

            Say(Speaker.Haru, Portrait.Bored,
                "...Dango?",
                "……団子?",
                "...دانگو؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "Dango.",
                "団子。",
                "دانگو.");

            DecideIdly(
                "Ask what the second thing was", "二つめ、なにか聞く", "بپرس چیزِ دومی چی بود",
                "Write down the date instead", "日付だけ書いておく", "به جاش تاریخ رو بنویس");

            Hold(1.2f);
        }

        // ---------------------------------------------------------------------
        //  Thursday, after school — the café
        //
        //  ▣ Scene state
        //     Background   CozyCafeDay: pink interior, fairy lights, parfaits
        //                  and tea sets on wooden tables.
        //     Date         Thursday 5 September, late afternoon.
        //     On stage     The counter, the lights, the tables. A table near the
        //                  door, which is where they sit and which nobody
        //                  mentions.
        //     In hand      Nothing until the drinks arrive.
        //     From before  The café was agreed on Wednesday, by him, after she
        //                  had already said it.
        //
        //  The act's only branching choice, and it is the manual's worked
        //  example, because that example is doing something no other scene in
        //  act one does: it lets the player press a button that changes six
        //  frames and changes nothing. The player has to believe for an hour
        //  that pressing things matters, or act three's refused blue button has
        //  no floor to fall through.
        //
        //  ◆ DREAD 4. She orders a drink he has never said he likes, and he
        //  drinks it. Deniable: she is being nice, and he is being polite. The
        //  three matcha pictures do the rest without a line of dialogue.
        // ---------------------------------------------------------------------

        private void WriteThursdayCafe()
        {
            ClearStage();

            Place(
                Backgrounds.CafeDay,
                "The café", "喫茶店", "کافه");

            // Indoors. The fall layer covers the whole picture, not the window
            // in it, so anything but NoFall here is leaves coming down between
            // the tables.
            NoFall(0f);

            Hold(1.8f);

            Narrate(
                "The lights over the counter were on in the afternoon, which is the whole business model.",
                "昼間から電飾がついていて、それがこの店の商売なのだった。",
                "چراغ‌های بالای پیشخون بعدازظهر هم روشن بودن، و کلِ مدلِ کاریِ اینجا همینه.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Narrate(
                "Yua took the table nearest the door and sat with her back to the wall.",
                "結愛は入口にいちばん近い席をとって、壁を背にして座った。",
                "یوآ نزدیک‌ترین میز به در رو گرفت و پشتش رو به دیوار نشست.");

            Say(Speaker.Haru, Portrait.Neutral,
                "It's warmer at the back.",
                "奥のほうがあったかいよ。",
                "ته‌ش گرم‌تره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I like it here.",
                "ここがいい。",
                "من اینجا رو دوست دارم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Neutral,
                "They've got bubble tea and they've got matcha.",
                "タピオカと、抹茶がある。",
                "بابل‌تی دارن و ماچا دارن.");

            DecideIdly(
                "Let Haru choose", "ハルぴに選ばせる", "بذار هارو انتخاب کنه",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Neutral,
                        "Which one do you want?",
                        "どっちがいい?",
                        "کدوم رو می‌خوای؟");

                    Say(Speaker.Haru, Portrait.Unchanged,
                        "What are you having?",
                        "結愛ぴは?",
                        "تو چی می‌گیری؟");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Bubble tea.",
                        "タピオカ。",
                        "بابل‌تی.");

                    Say(Speaker.Haru, Portrait.Unchanged,
                        "Then bubble tea.",
                        "じゃあタピオカ。",
                        "پس منم بابل‌تی.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "No. You're having the matcha.",
                        "だめ。ハルぴは抹茶。",
                        "نه. تو ماچا.");

                    Say(Speaker.Haru, Portrait.Bored,
                        "...Then why did you ask?",
                        "……じゃあなんで聞いたの?",
                        "...پس چرا پرسیدی؟");

                    Say(Speaker.Yua, Portrait.Smug,
                        "I wanted to hear it.",
                        "聞きたかったから。",
                        "دوست داشتم بشنوم.");
                },
                "Order for him", "自分で頼む", "خودت سفارش بده",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "One bubble tea, one matcha.",
                        "タピオカひとつ、抹茶ひとつ。",
                        "یه بابل‌تی، یه ماچا.");

                    Say(Speaker.Haru, Portrait.Surprised,
                        "You didn't ask me.",
                        "僕、聞かれてない。",
                        "از من که نپرسیدی.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "You'd have said whatever I said.",
                        "あたしが言ったのと同じこと言ったでしょ。",
                        "هرچی من می‌گفتم رو می‌گفتی.");

                    Say(Speaker.Haru, Portrait.Sheepish,
                        "...Probably.",
                        "……たぶん。",
                        "...احتمالاً.");
                });

            Say(Speaker.Haru, Portrait.Neutral,
                "I'm paying.",
                "僕が払う。",
                "من حساب می‌کنم.");

            Hold(1.2f);

            Narrate(
                "The drinks came. The matcha still had steam coming off it.",
                "飲みものが来た。抹茶はまだ湯気が立っていた。",
                "نوشیدنی‌ها رو آوردن. ماچا هنوز بخار داشت.");

            Cel(Portrait.DrinkFull);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Drink it while it's hot.",
                "あったかいうちに飲んで。",
                "تا داغه بخورش.");

            Cel(Portrait.DrinkReluctant);

            Hold(1.0f);

            Say(Speaker.Yua, Portrait.Neutral,
                "All of it.",
                "ぜんぶ。",
                "تا آخرش.");

            Say(Speaker.Haru, Portrait.StrainedSmile,
                "All of it.",
                "ぜんぶ、ね。",
                "تا آخرش.");

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Joyful,
                "I named it, by the way.",
                "そういえば、名前つけた。",
                "راستی اسم گذاشتم روش.");

            Say(Speaker.Haru, Portrait.Surprised,
                "Named what?",
                "なにに?",
                "رو چی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Nothing. I'll tell you Saturday.",
                "べつに。土曜に言う。",
                "هیچی. شنبه بهت می‌گم.");

            Say(Speaker.Haru, Portrait.Bored,
                "That's two days of this.",
                "二日間これやるんだ。",
                "یعنی دو روز اینو تحمل کنم.");

            Say(Speaker.Yua, Portrait.Smug,
                "That's the shortest I could make it.",
                "これでも短くしたほう。",
                "کوتاه‌ترین حالتیه که تونستم.");

            Cel(Portrait.DrinkFinished);

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Joyful,
                "...That wasn't bad.",
                "……わるくなかった。",
                "...بد نبود.");

            Say(Speaker.Yua, Portrait.Smug,
                "I know.",
                "知ってる。",
                "می‌دونم.");
        }

        // ---------------------------------------------------------------------
        //  Thursday, evening — the old alley
        //
        //  ▣ Scene state
        //     Background   TraditionalAlleywayNight: paved alley, a crescent
        //                  moon, a paper lantern lit by a doorway, lit windows,
        //                  pots of hydrangea along the wall.
        //     Date         Thursday 5 September, after dark.
        //     On stage     The lantern, the windows, the hydrangeas, a grate in
        //                  the road.
        //     From before  The matcha. The name she will not give until Saturday.
        //
        //  ◆ DREAD 5, and it is the first one that is a symptom rather than a
        //  fact. Something under the road hums. She stops talking for less than
        //  a second and then carries on with the same sentence. Deniable: she
        //  lost her thread. Nothing is explained and nothing ever will be in
        //  this act — the machine room is five acts away.
        // ---------------------------------------------------------------------

        private void WriteThursdayAlley()
        {
            ClearStage();

            Place(
                Backgrounds.AlleywayNight,
                "The old alley", "古い路地", "کوچه‌ی قدیمی");

            NoFall(1.5f);

            Hold(1.8f);

            Narrate(
                "The lantern by the doorway was the only warm thing in the street and it was not trying very hard.",
                "戸口の提灯だけがあたたかくて、それもあまり頑張ってはいなかった。",
                "فانوسِ کنارِ در تنها چیزِ گرمِ کوچه بود و خیلی هم تلاش نمی‌کرد.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Neutral,
                "The hydrangeas are over.",
                "紫陽花、終わってる。",
                "ادریسی‌ها تموم شدن.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "They went over in July.",
                "七月に終わったよ。",
                "تیر تموم شدن.");

            Say(Speaker.Yua, Portrait.Pout,
                "Then why are they still out here.",
                "じゃあなんでまだ外にあるの。",
                "پس چرا هنوز اینجان.");

            Say(Speaker.Haru, Portrait.Sheepish,
                "Somebody likes them anyway.",
                "それでも好きな人がいるんでしょ。",
                "یکی بازم دوستشون داره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That's worse.",
                "そっちのほうが重い。",
                "این بدتره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's much worse, yes.",
                "うん、だいぶ重い。",
                "آره، خیلی بدتره.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Neutral,
                "So the stall does four on a stick, and there's three of the ones I like and one of the ones I don't, which means the maths only works if—",
                "屋台のやつ、一本に四つで、好きなのが三つと好きじゃないのが一つ入ってるから、計算が合うのは——",
                "دکه‌ی جمعه چهار تا رو یه سیخ می‌ذاره، سه تاش اونیه که دوست دارم و یکیش اونی که ندارم، یعنی حساب‌کتابش فقط وقتی جور درمیاد که—");

            // ◆ DREAD 5. Something under the road. Less than a second.
            Cue(SfxId.BoilerRoom, 0.30f);

            Hold(0.9f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "—only works if you take the fourth one.",
                "——ハルぴが四つめを食べるときだけ。",
                "—فقط وقتی جور درمیاد که تو چهارمیه رو برداری.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "I'll take the fourth one.",
                "四つめ、食べるよ。",
                "من چهارمیه رو برمی‌دارم.");

            Say(Speaker.Yua, Portrait.Neutral,
                "You don't know which one it is yet.",
                "まだどれか知らないでしょ。",
                "هنوز نمی‌دونی کدومه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I'll take it anyway.",
                "それでも食べる。",
                "بازم برمی‌دارم.");

            Hold(1.6f);

            Narrate(
                "A light went on in one of the windows above them and then went off again.",
                "上の窓のひとつに明かりがついて、また消えた。",
                "یکی از پنجره‌های بالا روشن شد و دوباره خاموش.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Tomorrow the station.",
                "明日、駅ね。",
                "فردا ایستگاه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Tomorrow the station.",
                "明日、駅。",
                "فردا ایستگاه.");
        }

        // =====================================================================
        //  FRIDAY — day five of six
        //
        //  Dread budget: 2, both on the platform, and the second is the act's
        //  most important thirty seconds.
        // =====================================================================

        private void WriteFriday()
        {
            WriteFridayPath();
            WriteFridayPlatform();
        }

        // ---------------------------------------------------------------------
        //  Friday, morning — the school path again
        //
        //  ▣ Scene state
        //     Background   AutumnSchoolAlleyDay, four days on. More of the
        //                  bottom has turned and there is more on the ground.
        //     Date         Friday 6 September, morning.
        //     On stage     The maples, the leaves, the benches, the lanterns.
        //     From before  Dango after school. The fourth one is his.
        //
        //  The scene that bookends Monday, and it is the same path with more on
        //  the floor and no remark about it from anybody. He wins the argument
        //  about which side goes first. She decides the afternoon, again, by
        //  having decided it on Thursday.
        // ---------------------------------------------------------------------

        private void WriteFridayPath()
        {
            ClearStage();

            Place(
                Backgrounds.SchoolAlleyDay,
                "The path to school", "通学路", "راهِ مدرسه");

            Date(2024, 9, 6);

            AutumnAir(0.24f);

            Hold(1.8f);

            Narrate(
                "There was a line of red along the gutter that had not been there on Monday.",
                "月曜にはなかった赤い筋が、側溝に沿ってできていた。",
                "کنارِ جوب یه خطِ قرمز افتاده بود که دوشنبه نبود.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Neutral,
                "This side's going faster.",
                "こっち側のほうが早い。",
                "این‌ور داره تندتر می‌ریزه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "They're the same trees.",
                "同じ木でしょ。",
                "درخت‌هاش که یکین.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "They're the same trees on the side that gets the wind.",
                "風が当たる側の、同じ木。",
                "همون درخت‌هان، منتها این‌ور باد می‌خوره.");

            Say(Speaker.Yua, Portrait.Pout,
                "Give them a week.",
                "一週間待って。",
                "یه هفته بهشون وقت بده.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Three days.",
                "三日。",
                "سه روز.");

            Say(Speaker.Yua, Portrait.Surprised,
                "Three?",
                "三日?",
                "سه روز؟");

            Say(Speaker.Haru, Portrait.Joyful,
                "For this side. Your side has a week.",
                "こっち側は三日。結愛ぴの側は一週間。",
                "این‌ور سه روز. سمتِ تو یه هفته.");

            Say(Speaker.Yua, Portrait.Bored,
                "I don't have a side.",
                "あたしの側とかない。",
                "من سمتی ندارم.");

            Say(Speaker.Haru, Portrait.Smug,
                "You've walked on it every day this week.",
                "今週ずっとそっち歩いてる。",
                "این هفته هر روز از همون‌ور راه رفتی.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "...That's the flat side.",
                "……そっちのほうが平ら。",
                "...اون‌ور صافه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It is the flat side.",
                "うん、平ら。",
                "آره صافه.");

            Hold(1.4f);

            Narrate(
                "Two boys from another class went past kicking the same leaf between them until they lost it.",
                "別のクラスの男子二人が、同じ落ち葉を蹴り合いながら通りすぎて、途中で見失った。",
                "دو تا پسر از یه کلاسِ دیگه رد شدن و یه برگ رو بین خودشون شوت می‌کردن تا گمش کردن.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Four on a stick.",
                "一本に四つ。",
                "چهار تا رو یه سیخ.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I remember.",
                "覚えてる。",
                "یادمه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You get the fourth one.",
                "四つめはハルぴ。",
                "چهارمیه مالِ توئه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I remember that too.",
                "それも覚えてる。",
                "اونم یادمه.");

            DecideIdly(
                "Tell him which one the fourth is", "四つめがどれか教える", "بهش بگو چهارمی کدومه",
                "Let him find out", "自分で知ればいい", "بذار خودش بفهمه");

            SayWithSound(Speaker.Yua, Portrait.Neutral, SfxId.SchoolBell, 0.7f,
                "That's us.",
                "行こ。",
                "نوبتِ ماست.");
        }

        // ---------------------------------------------------------------------
        //  Friday, after school — the platform
        //
        //  ▣ Scene state
        //     Background   TrainPlatformAutumnSunset: a platform in gold and
        //                  pink light, a pink drinks machine, coloured seats,
        //                  turned trees along the track, a stall at the far
        //                  end. December's version of this platform has the
        //                  same seats and nothing on the trees.
        //     Date         Friday 6 September, evening.
        //     On stage     The seats, the machine, the stall, the track.
        //     In hand      One stick of dango with four on it.
        //     From before  The fourth one is his, agreed twice.
        //
        //  ===================================================================
        //  THE SCENE THE ACT EXISTS TO SET UP
        //  ===================================================================
        //
        //  ぴ is real Japanese slang and a Japanese player gets the whole of it
        //  the first time Yua says it in the opening scene. A Persian or English
        //  player hears a nickname and nothing else. So once, and only once,
        //  somebody outside the pair has to notice it out loud and be told to
        //  mind their own business — and the suffix acquires in two languages
        //  the weight it already had in the third.
        //
        //  The previous draft put six frames and a manga between Yua's last
        //  "Haru-pi" and the classmate's question, so the question landed on the
        //  manga instead. Here the suffix is the line immediately before, it is
        //  said at normal volume in front of somebody who is standing right
        //  there, and the classmate interrupts on it.
        //
        //  ◆ DREAD 6 — her face, for two tenths of a second, while a third
        //  person stands close to him. Nobody in the scene sees it.
        //  ◆ DREAD 7 — "It's ours" arrives a beat too fast and a degree too
        //  flat, and then she is completely normal again. Deniable: she was
        //  embarrassed. Which is also true.
        // ---------------------------------------------------------------------

        private void WriteFridayPlatform()
        {
            ClearStage();

            Place(
                Backgrounds.TrainPlatformSunset,
                "The platform", "ホーム", "سکوی ایستگاه");

            AutumnAir(0.12f);

            Hold(1.8f);

            Narrate(
                "The light on the platform was the colour that makes everybody look like they are about to say something.",
                "ホームの光は、みんながこれから何か言いそうに見える色をしていた。",
                "نورِ سکو همون رنگی بود که همه رو شبیهِ کسی می‌کنه که می‌خواد چیزی بگه.");

            Enter(Speaker.Yua, Portrait.Joyful);
            Enter(Speaker.Haru, Portrait.Neutral);

            Narrate(
                "There were four on the stick. Three of one kind and one of another.",
                "串には四つ。三つが同じで、ひとつだけ別のもの。",
                "روی سیخ چهار تا بود. سه تا از یه جور و یکی از یه جورِ دیگه.");

            Say(Speaker.Haru, Portrait.Surprised,
                "The fourth one is the one at the bottom.",
                "四つめって、いちばん下のやつか。",
                "چهارمیه اونیه که تهِ سیخه.");

            Say(Speaker.Yua, Portrait.Smug,
                "The fourth one is the one at the bottom.",
                "四つめは、いちばん下。",
                "چهارمیه اونیه که تهِ سیخه.");

            Say(Speaker.Haru, Portrait.Bored,
                "You've had this planned since Thursday.",
                "木曜から決めてたでしょ。",
                "از پنج‌شنبه نقشه‌شو کشیده بودی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Since Wednesday.",
                "水曜から。",
                "از چهارشنبه.");

            Say(Speaker.Haru, Portrait.Sheepish,
                "The stall isn't there on Wednesday.",
                "水曜は屋台出てないよ。",
                "چهارشنبه که دکه نیست.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I know. That's how long I've been planning it.",
                "知ってる。それくらい前から考えてた。",
                "می‌دونم. انقدر وقته دارم نقشه‌شو می‌کشم.");

            Hold(1.2f);

            Narrate(
                "A train came through the far platform without stopping and took all the sound with it.",
                "向かいのホームを電車が止まらずに通って、音をぜんぶ持っていった。",
                "یه قطار از سکوی روبه‌رو بدونِ توقف رد شد و کلِ صدا رو با خودش برد.");

            Say(Speaker.Haru, Portrait.Neutral,
                "That one doesn't stop here.",
                "あれ、ここ止まらない。",
                "اون اینجا واینمی‌سته.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Everything stops here.",
                "ぜんぶ止まるでしょ。",
                "همه‌شون اینجا وایمیسن.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "Not the orange ones.",
                "オレンジのは止まらない。",
                "نارنجی‌ها نه.");

            Say(Speaker.Yua, Portrait.Surprised,
                "...That one was orange.",
                "……あれオレンジだった。",
                "...اون نارنجی بود.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It was orange.",
                "オレンジ。",
                "نارنجی بود.");

            Say(Speaker.Yua, Portrait.Pout,
                "I hate this about you.",
                "そういうとこ嫌い。",
                "از این خصلتت بدم میاد.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You've said that three times this week.",
                "今週三回目。",
                "این هفته سه بار اینو گفتی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Because it keeps being true.",
                "毎回ほんとだから。",
                "چون هر بار درسته.");

            Hold(1.4f);

            Narrate(
                "A girl from their class came along the platform and stopped a little too close to Haru, the way people do when there is nowhere else to stand.",
                "同じクラスの子がホームを歩いてきて、立つ場所がないときの距離でハルの横に止まった。",
                "یه دختر از کلاسشون از سکو اومد و یه‌کم زیادی نزدیکِ هارو وایساد، همون‌جوری که آدم‌ها وقتی جای دیگه‌ای واسه وایسادن نیست وایمیسن.");

            // ◆ DREAD 6. Two tenths of a second, from her brightest face.
            FaceSlips();

            Say(Speaker.Classmate, Portrait.Unchanged,
                "Oh — you two are in 1-A.",
                "あ、一年A組の二人だ。",
                "اِ — شما دوتا اول-الفین.");

            Say(Speaker.Haru, Portrait.Neutral,
                "We are.",
                "そうです。",
                "آره.");

            Say(Speaker.Classmate, Portrait.Unchanged,
                "I sit two rows behind you. By the bookshelf.",
                "二列後ろに座ってる。本棚のとこ。",
                "من دو ردیف عقب‌ترتون می‌شینم. بغلِ قفسه‌ی کتاب.");

            Say(Speaker.Yua, Portrait.Neutral,
                "By the window that doesn't shut.",
                "閉まらない窓のとこね。",
                "بغلِ اون پنجره‌ای که بسته نمی‌شه.");

            Say(Speaker.Classmate, Portrait.Unchanged,
                "That's the one.",
                "それそれ。",
                "آره همون.");

            Hold(0.8f);

            // The suffix, at normal volume, in front of the third person, in the
            // frame immediately before the question. This is the whole scene.
            Say(Speaker.Yua, Portrait.Joyful,
                "Haru-pi, give me the stick, you're holding it at an angle.",
                "ハルぴ、串かして。傾いてる。",
                "هاروپی، سیخ رو بده من، کجش گرفتی.");

            Say(Speaker.Haru, Portrait.Sheepish,
                "I'm holding it at a normal angle.",
                "ふつうの角度だけど。",
                "عادی گرفتمش.");

            Say(Speaker.Classmate, Portrait.Unchanged,
                "...Sorry, what did you just call him?",
                "……ごめん、今なんて呼んだ?",
                "...ببخشید، الآن چی صداش کردی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Nothing.",
                "べつに。",
                "هیچی.");

            Say(Speaker.Classmate, Portrait.Unchanged,
                "No, really — what was that?",
                "ううん、ほんとに。なんて?",
                "نه جدی — اون چی بود؟");

            Hold(0.6f);

            // ◆ DREAD 7. A beat too fast, a degree too flat, and then gone.
            Say(Speaker.Yua, Portrait.Neutral,
                "It's ours.",
                "うちらのだから。",
                "مالِ خودمونه.");

            Hold(1.6f);

            Narrate(
                "The girl said something friendly about the dango and went down the steps.",
                "その子は団子のことを何か感じよく言って、階段を降りていった。",
                "دختره یه چیزِ خوب درباره‌ی دانگو گفت و از پله‌ها رفت پایین.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "She sits by the bookshelf.",
                "本棚のとこに座ってるんだ。",
                "بغلِ قفسه می‌شینه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "She does.",
                "うん、座ってる。",
                "آره می‌شینه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "We still never read that board.",
                "結局、掲示板は読んでないね。",
                "بازم اون تخته رو نخوندیم.");

            Say(Speaker.Yua, Portrait.Smug,
                "We're not going to.",
                "読まないよ。",
                "نمی‌خونیمش.");

            Hold(1.0f);

            Narrate(
                "The fourth one was still on the stick. He ate it without being asked twice.",
                "四つめはまだ串に残っていた。二度言われる前に、ハルが食べた。",
                "چهارمیه هنوز رو سیخ بود. بدونِ اینکه دوبار بهش گفته بشه، خوردش.");
        }

        // =====================================================================
        //  SATURDAY — day six of six
        //
        //  Dread budget: 3, which is more than any other day and all of it is
        //  in the second half. Saturday is a half day and they spend it
        //  somewhere neither of them has to be, which is the first time in the
        //  act that is true.
        //
        //  This is also where the ladder takes its only step to rung three. She
        //  corrects his memory of his own childhood and she is right about it,
        //  and the scene does not stop to notice.
        // =====================================================================

        private void WriteSaturday()
        {
            WriteSaturdayPlayground();
            WriteSaturdayCorner();
        }

        // ---------------------------------------------------------------------
        //  Saturday, early afternoon — the playground
        //
        //  ▣ Scene state
        //     Background   PastelPlaygroundDay: a stone angel fountain in the
        //                  middle, a swing set and a slide with children on it,
        //                  flowerbeds, houses round the edge.
        //     Date         Saturday 7 September, early afternoon.
        //     On stage     The fountain, the swings, the slide, the children.
        //     From before  A name she has been sitting on since Thursday.
        //
        //  ◆ DREAD 8 — she counts to five for two children who are arguing, and
        //  she is completely lovely about it, and on "five" the hand comes down
        //  and the face goes ordinary, one beat before the count has finished
        //  being useful. Deniable: she was done. Nobody sees it except the
        //  player, and the player is not sure they saw it either.
        // ---------------------------------------------------------------------

        private void WriteSaturdayPlayground()
        {
            ClearStage();

            Place(
                Backgrounds.PlaygroundDay,
                "The playground", "公園", "زمینِ بازی");

            Date(2024, 9, 7);

            AutumnAir(0.10f);

            Hold(1.8f);

            Narrate(
                "The fountain was on and the angel in the middle of it had a leaf on its head.",
                "噴水は出ていて、真ん中の天使の頭には葉っぱが一枚のっていた。",
                "فواره روشن بود و رو سرِ فرشته‌ی وسطش یه برگ نشسته بود.");

            Enter(Speaker.Yua, Portrait.Joyful);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's got a hat.",
                "帽子かぶってる。",
                "کلاه گذاشته سرش.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It's got a leaf.",
                "葉っぱだよ。",
                "برگه.");

            Say(Speaker.Yua, Portrait.Pout,
                "It's got a hat.",
                "帽子。",
                "کلاهه.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "It's a leaf that has landed in the shape of a hat, which is a different thing and a better one.",
                "帽子のかたちに落ちた葉っぱ。別のもので、そっちのほうがいい。",
                "برگیه که شکلِ کلاه افتاده، که چیزِ دیگه‌ایه و بهترم هست.");

            Say(Speaker.Yua, Portrait.Bored,
                "...Fine.",
                "……はい。",
                "...باشه.");

            Hold(1.2f);

            Narrate(
                "At the slide, two small children had arrived at the top at the same moment and had strong opinions about it.",
                "すべり台の上で、小さい子が二人同時に着いてしまって、それぞれに強い意見があった。",
                "بالای سرسره، دو تا بچه‌ی کوچیک همزمان رسیده بودن و هرکدوم نظرِ محکمی داشتن.");

            Say(Speaker.Yua, Portrait.Surprised,
                "Oh no.",
                "うわ。",
                "وای نه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Leave them, they'll sort it.",
                "ほっとけば解決するよ。",
                "ولشون کن، خودشون حلش می‌کنن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "They will not sort it.",
                "しないよ。",
                "حلش نمی‌کنن.");

            Narrate(
                "She went over.",
                "結愛が近づいていった。",
                "رفت سمتشون.");

            Hold(1.4f);

            // The hand goes up here with all five fingers open, and it stays
            // up through the whole count. Nothing points at the number.
            Say(Speaker.Yua, Portrait.Counting,
                "Right. The one at the top goes first, because that is how a slide works. And the one at the bottom gets to count out loud. All right? Count for me.",
                "はい。上にいる子が先。すべり台ってそういうものだから。下の子は、声に出して数える係。いい? 数えて。",
                "خب. اونی که بالاست اول می‌ره، چون سرسره همین‌جوریه. اونی که پایینه هم بلندبلند می‌شمره. باشه؟ بشمر واسم.");

            Hold(1.0f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "One.",
                "いち。",
                "یک.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Two. Three.",
                "に。さん。",
                "دو. سه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Four.",
                "し。",
                "چهار.");

            // ◆ DREAD 8. The hand comes down on this one and not on the one
            // after it, and her face is ordinary, and then she is straight back
            // to the children. Deniable: she had finished.
            Say(Speaker.Yua, Portrait.Neutral,
                "Five.",
                "ご。",
                "پنج.");

            Hold(0.9f);

            Say(Speaker.Yua, Portrait.Joyful,
                "There. Nobody shouted and everybody had a job.",
                "ほら。誰も怒鳴らないで、みんな係があった。",
                "بفرما. هیچ‌کس داد نزد و همه یه کاری داشتن.");

            Narrate(
                "Both children went down the slide and immediately climbed back up to do it again in the wrong order.",
                "二人ともすべって、すぐにまた上って、今度は順番を守らずにやり直した。",
                "هردو بچه از سرسره اومدن پایین و بلافاصله رفتن بالا که دوباره، این بار به ترتیبِ غلط، انجامش بدن.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You were good at that.",
                "うまかったね。",
                "خوب بلد بودی.");

            Say(Speaker.Yua, Portrait.Smug,
                "I'm good at telling people what to do.",
                "人に指図するの得意なの。",
                "من تو دستور دادن به بقیه خوبم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You are.",
                "うん。",
                "آره هستی.");

            Hold(1.2f);

            DecideIdly(
                "Have a go on the swings", "ブランコ乗る", "برو تاب سوار شو",
                "Sit by the fountain", "噴水のとこに座る", "کنارِ فواره بشین");

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Thinking,
                "Tomo.",
                "トモ。",
                "تومو.");

            Say(Speaker.Haru, Portrait.Surprised,
                "...What?",
                "……なに?",
                "...چی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The plant. It's called Tomo.",
                "植木鉢。トモっていう名前。",
                "گلدونه. اسمش توموئه.");

            Say(Speaker.Haru, Portrait.Bored,
                "Two days. For Tomo.",
                "二日かけて、トモ。",
                "دو روز. واسه تومو.");

            Say(Speaker.Yua, Portrait.Pout,
                "It took two days because it had to be right.",
                "ちゃんとしたのにしたかったから二日かかったの。",
                "دو روز طول کشید چون باید درست می‌بود.");

            Say(Speaker.Haru, Portrait.CalmSerious,
                "Plants don't get names.",
                "植物に名前はつけない。",
                "به گیاه که اسم نمی‌ذارن.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Tomo does.",
                "トモにはつける。",
                "به تومو می‌ذارن.");

            Hold(1.0f);

            Say(Speaker.Haru, Portrait.Joyful,
                "...Fine. Tomo.",
                "……わかった。トモ。",
                "...باشه. تومو.");
        }

        // ---------------------------------------------------------------------
        //  Saturday, evening — the corner with the machine
        //
        //  ▣ Scene state
        //     Background   PastelStreetVendingNight: the same street after dark,
        //                  the machine lit from inside, streetlamps, a crescent
        //                  moon, the flowerbeds.
        //     Date         Saturday 7 September, after dark.
        //     On stage     The machine, the bicycle, the lamps.
        //     From before  His hundred yen, since Monday. Tomo, since an hour
        //                  ago. Six days of it.
        //
        //  The machine pays out twice, which is the last joke in the act and the
        //  reason it was established properly on day one.
        //
        //  ◆ DREAD 9 — the ladder's only step to rung three. She corrects his
        //  memory of something from before they supposedly met, and she is
        //  right, and neither of them stops. Deniable: he told her at some point
        //  and forgot. He does not say that. Nobody says anything.
        //  ◆ DREAD 10 — two frames at the very end, and they are the act's last
        //  two frames. The manual allows the close of an act to sit one rung
        //  above the day's budget as long as it stays deniable and does not
        //  explain itself. These do both.
        // ---------------------------------------------------------------------

        private void WriteSaturdayCorner()
        {
            ClearStage();

            Place(
                Backgrounds.VendingStreetNight,
                "The corner", "曲がり角", "سرِ نبش");

            NoFall(2f);

            Hold(1.8f);

            Narrate(
                "The machine was the brightest thing on the street and it was lit from the inside like a small shop nobody works in.",
                "自販機は通りでいちばん明るくて、誰も働いていない小さな店みたいに内側から光っていた。",
                "دستگاه روشن‌ترین چیزِ خیابون بود و از تو روشن بود، مثلِ یه مغازه‌ی کوچیک که هیشکی توش کار نمی‌کنه.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Smug,
                "It still has your hundred yen.",
                "まだハルぴの百円入ってる。",
                "هنوز صدینِ تو رو داره.");

            Say(Speaker.Haru, Portrait.Sheepish,
                "It does.",
                "入ってるね。",
                "آره داره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Six days.",
                "六日。",
                "شیش روز.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's not interest-bearing.",
                "利子はつかないよ。",
                "بهره که نمی‌ده.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Try again.",
                "もう一回やって。",
                "دوباره امتحان کن.");

            Say(Speaker.Haru, Portrait.Bored,
                "It's going to take this one too.",
                "これも持っていかれる。",
                "اینم می‌خوره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Then it owes you two hundred and that's a much better story.",
                "じゃあ二百円の貸しになって、話としてはそっちのがいい。",
                "اون‌وقت دویست ین بهت بدهکاره و این داستانِ خیلی بهتریه.");

            Narrate(
                "He put the coin in.",
                "ハルは百円を入れた。",
                "سکه رو انداخت.");

            Cue(SfxId.VendingThunk, 0.8f);

            Hold(1.4f);

            Cue(SfxId.CanDrop, 0.85f);

            Hold(0.7f);

            Cue(SfxId.CanDrop, 0.85f);

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Surprised,
                "...Two.",
                "……二本。",
                "...دو تا.");

            Say(Speaker.Yua, Portrait.Surprised,
                "Two!",
                "二本!",
                "دو تا!");

            Say(Speaker.Haru, Portrait.Joyful,
                "It was saving up.",
                "貯めてたんだ。",
                "داشته جمع می‌کرده.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It was saving up! For six days! It had a plan the whole time and none of us knew!",
                "貯めてたんだよ! 六日間! ずっと計画があって、誰も知らなかった!",
                "داشته جمع می‌کرده! شیش روز! تمامِ این مدت نقشه داشته و هیچ‌کدوممون خبر نداشتیم!");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "One's yours.",
                "ひとつは結愛ぴの。",
                "یکیش مالِ توئه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Obviously one's mine.",
                "そりゃそうでしょ。",
                "معلومه که یکیش مالِ منه.");

            Hold(1.6f);

            // SCENE LOCK. Not "the low wall" — the September picture of this
            // corner has flowerbeds and no wall to sit on. The wall arrives in
            // November, in the winter picture, and act two sits on it there.
            Narrate(
                "They sat down on the kerb by the flowerbeds and opened them at the same time, which was not planned and happened anyway.",
                "花壇のふちの縁石に腰を下ろして、二人同時に開けた。示し合わせていないのに、そうなった。",
                "رو جدولِ کنارِ باغچه نشستن و همزمان بازشون کردن، که قرار نبود و بازم شد.");

            // A can each, in the same frame. The machine has been taking his
            // hundred yen since Monday and this is what six days of that bought
            // — the one picture the whole running joke was for. It stays up
            // through the four lines after it, because none of them needs a
            // face and all of them are better with two hands round a can.
            Cel(Portrait.HoldCan, 2.0f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "Six days.",
                "六日間。",
                "شیش روز.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Six days of what?",
                "六日間、なにが?",
                "شیش روزِ چی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Term. It's gone fast.",
                "学期。早かった。",
                "ترم. زود گذشت.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It has.",
                "うん。",
                "آره.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.StrainedSmile,
                "I used to be able to do that slide in one go. Both feet, no hands, all the way down.",
                "昔はあのすべり台、一回で下まで行けたんだ。両足で、手はつかないで。",
                "قبلاً می‌تونستم اون سرسره رو یه‌ضرب برم. با دو تا پا، بدونِ دست، تا ته.");

            Say(Speaker.Yua, Portrait.Neutral,
                "You never could.",
                "できたことないよ。",
                "هیچ‌وقت نمی‌تونستی.");

            // ◆ DREAD 9. Rung three, once, and the act's highest. She corrects
            // his memory and she is right, and the scene does not stop.
            Say(Speaker.Yua, Portrait.Unchanged,
                "You always put a hand down at the end. Every time.",
                "最後にいつも手、つけてた。毎回。",
                "همیشه آخرش یه دست می‌ذاشتی زمین. هر بار.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Joyful,
                "...Yeah.",
                "……そうだっけ。",
                "...آره.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Yeah, I did.",
                "そうだったかも。",
                "آره، می‌ذاشتم.");

            Hold(1.8f);

            Narrate(
                "A car came up the street, went past the two of them, and turned at the top.",
                "車が一台上ってきて、二人の横を通って、上で曲がった。",
                "یه ماشین از خیابون اومد بالا، از کنارِ اون دوتا رد شد و سرِ بالا پیچید.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Monday, then.",
                "じゃあ月曜。",
                "پس دوشنبه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Monday.",
                "月曜。",
                "دوشنبه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Night, Yua-pi.",
                "おやすみ、結愛ぴ。",
                "شب بخیر، یوآپی.");

            Exit(Speaker.Haru);

            Hold(2.2f);

            Narrate(
                "He went down the hill. The machine carried on being the brightest thing in the street.",
                "ハルは坂を下りていった。自販機は、あいかわらず通りでいちばん明るかった。",
                "از سرازیری رفت پایین. دستگاه همچنان روشن‌ترین چیزِ خیابون بود.");

            Hold(2.0f);

            // ◆ DREAD 10. The last two frames of the act.
            InnerVoice(
                "Six days.",
                "六日間。",
                "شیش روز.");

            InnerVoice(
                "Nothing's changed at all.",
                "なんにも、変わってない。",
                "هیچی عوض نشده.");
        }

        // =====================================================================
        //  SELF-AUDIT
        //
        //  The manual's rule about counting: an audit that reports a number it
        //  did not count is not an audit. These were counted.
        // =====================================================================
        //
        //  SHAPE
        //    6 days · 14 scenes · 385 written frames that show text · 9 white
        //    choices, one of which branches and changes nothing. A single
        //    playthrough reads 378 or 381 of the 385: the branch is seven
        //    frames down one road and four down the other, and both roads are
        //    counted above because both are written.
        //    No blue or green anywhere. No player address. No aside. Nothing
        //    above ladder rung three.
        //
        //  THE TEN DREAD MOMENTS, AND THE ORDINARY SENTENCE THAT EXPLAINS EACH
        //    1  Mon, the corner, last two frames. "Good. / Still the same."
        //         → she is fond of him.            [rung 1]
        //    2  Tue, the bakery. She says what is on a collar she refused to
        //       read. → she looked at it another day.                 [rung 1]
        //    3  Wed, the roof. HIS. He puts his bag on the left end before she
        //       says anything; she sits on the right without looking.
        //         → they are used to each other.                      [rung 1]
        //    4  Thu, the café. She orders him a drink he has never said he
        //       likes and tells him to finish it. → she is being nice.[rung 2]
        //    5  Thu, the alley. Something hums under the road and she stops
        //       mid-sentence for under a second. → she lost her thread.
        //                                                             [rung 2]
        //    6  Fri, the platform. Her face for two tenths of a second while a
        //       third person stands close to him. → nobody saw it.    [rung 2]
        //    7  Fri, the platform. "It's ours", a beat early and a degree flat.
        //         → she was embarrassed. Which is also true.          [rung 2]
        //    8  Sat, the playground. One flat syllable on "five" in the middle
        //       of being completely lovely. → she was concentrating.  [rung 2]
        //    9  Sat, the corner. She corrects his memory of his own childhood
        //       and is right. → he told her once and forgot.          [rung 3]
        //   10  Sat, the corner, last two frames. "Six days. / Nothing's
        //       changed at all." → she had a nice week.               [rung 3]
        //
        //    Zero in the first fifty frames: the first is frame 112.
        //    Never two rungs in one scene. No rung skipped: 1 before 2, 2
        //    before 3. Rungs 4 and 5 are untouched and belong to act two.
        //
        //    One thing that is NOT on the list and was checked: on Monday, in
        //    the classroom, she says "Good." about a seat he took without being
        //    asked, and then says she meant the curtain. That is not a dread
        //    moment — a girl saying "good" about where her friend sat is not
        //    strange in any way, and nothing in the frame is wrong. It is there
        //    so that the same word, alone, at the end of the same day, has been
        //    heard before. The strangeness is created backwards, eleven hours
        //    later, and only on a second playthrough.
        //
        //  HARU WINS ONE UNIMPORTANT ARGUMENT PER SCENE
        //    the shade · plants do not get names · the window has been broken
        //    since last year · the sausage has six legs · "mm" was one sound ·
        //    the hydrangeas · which side of the path goes faster · the orange
        //    trains · it is a leaf in the shape of a hat.
        //
        //  AND LOSES ONE REAL DECISION PER SCENE, UNREMARKED
        //    where they eat the rolls · where they sit in class · which way
        //    they walk home · that they go past the bakery · which table in the
        //    café · what he drinks · that he finishes it · which dango is his ·
        //    that the plant has a name.
        //
        //  YUA DOES SOMETHING WITH NOTHING TO DO WITH HIM, EVERY SCENE
        //    the hem · the plant · hitting the machine · the angle of the rain
        //    · naming the cat and the whole speech about not reading the collar
        //    · the leaf on the angel · the two children on the slide · Tomo.
        //
        //  THE SUBSTITUTION TEST
        //    Replace every one of Haru's answers with "okay" and the act loses:
        //    both bakeries, the dough, Morita-sensei's maybe and its payoff,
        //    the broken window, the orange trains, six-legged octopuses, the
        //    hat, and the slide. He is in it.
        //
        //  MOTIFS
        //    The plant     Mon plain · Wed repeat · Sat paid off as Tomo.
        //    The machine   Mon takes a coin · Sat gives two cans.
        //    The test      Mon "maybe" · Tue argued · Thu he was right.
        //    The left      His bag Wed · the flat side of the path Fri · the
        //                  hand he put down at the end of the slide Sat.
        //                  Never once remarked on by anybody.
        //
        //  CONTINUITY
        //    Dates on screen: Mon 2, Tue 3, Wed 4, Thu 5, Fri 6, Sat 7
        //    September 2024, through StoryCalendar, weekday computed.
        //    Weather: maple leaves, light, outdoors only, never in a room and
        //    never once mentioned by anybody. No blossom anywhere in the act.
        //    Clothes: her dress and his cardigan, which is what is drawn. No
        //    uniform is referred to at any point.
        //    Bento numbers: six and four said once, ten pictures, four and
        //    three onto his lid — the frames and the line agree.
        //    Sounds: the school bell twice, Mon and Fri, both on the path.
        //    The machine Mon (takes) and Sat (pays out twice). A chair Mon, a
        //    knock on the machine Mon, the thing under the road Thu. Counted,
        //    not remembered: nine cues in the act.
        //    Weather by scene: leaves outdoors in daylight only. Never in a
        //    room — not the classroom, not the corridor, not the café — and
        //    never at night, which is both of the night scenes.
        //
        //  WHAT THIS ACT PLANTS FOR LATER, AND HOW MUCH IT WEIGHS
        //    · "-pi", from the first exchange, never explained.        (act 6)
        //    · Nobody ever asks how the other one knows anything.      (act 6)
        //    · His leg: four floors, out of breath, "don't take the stairs fast
        //      on the way down", the hand at the end of the slide.  (acts 2, 6)
        //    · The thing under the road, once, for less than a second.  (act 6)
        //    · The red cord at her wrists, in every sprite, never named. (act 5)
        //    · Five: counted out loud for two children who are not listening.
        //                                                            (the end)
        //    All of them two lines or fewer, mid-sentence, with no pause after.
        //
        //  WHAT IS DELIBERATELY NOT HERE
        //    No blue or green button and no hint of one — act three.
        //    No line to the player — act two's last scene.
        //    Nobody is ever surprised that the other one knows something, and
        //    nobody explains how they know. The player is meant to notice that
        //    nobody asks. The characters never notice at all.
        // =====================================================================
    }
}
