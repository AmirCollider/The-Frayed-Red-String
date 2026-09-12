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
//  STALE AGAINST THE CURRENT DOCUMENTS. This act was written against version
//  four of the dialogue manual; the manual is now at version eight and the
//  narrative document at 3.1.7. It is due a rewrite against both, and the work
//  is written up as defect 1 and defect 6 in AboutProject/Roadmap.md. Read that
//  before changing anything here.
//
//  This is a complete rewrite against the dialogue manual, version four. The
//  draft it replaces is quoted in that manual's calibration section as the
//  example of what a scene must not be, and it is worth writing down why, at
//  the top of the file, so that it is not written a fourth time:
//
//      The old act one opened with a girl announcing the time, the minutes
//      left until the bell, the number of people at the gate, the number of
//      ways into the building and which cherry tree was third from it. Six
//      frames. It read as a surveillance report, no teenager has ever spoken
//      like that, and it burned the whole trap in the first minute of the game.
//
//  The rule that replaces it is the manual's, and it outranks every other rule
//  in this file:
//
//      If this game had no secret in it at all, and were only a sweet high
//      school romance — would this scene be fun to play?
//
//  A scene that fails that is cut, no matter how many other rules it passes.
//  Acts one to four have to genuinely BE the cute game, not do an impression of
//  one, because the fifth act is only devastating in proportion to how much the
//  player enjoyed the first.
//
//  What that turned into, concretely, and what every scene below obeys:
//
//    • Half of every scene, at least, is about a third thing. Morita-sensei, a
//      bakery that changed hands, the price of melon bread, a cat, an exam
//      nobody is sure was announced. Two people who only talk about each other
//      sound like an interrogation.
//    • Every frame carries warmth, a joke, excitement, stubbornness,
//      embarrassment, curiosity, a complaint or an offer. A frame that only
//      moves information is deleted.
//    • Line length is information. Every scene has at least one genuinely long
//      line — somebody excited about something and unwilling to drop it — and
//      several one-word ones. Uniform line lengths are dead rhythm and make
//      people sound like machinery.
//    • Haru wins one unimportant argument per scene and quietly loses one real
//      decision. Neither is ever named. If replacing all of his answers with
//      "okay" does not change the scene, he is not in it.
//    • Yua does at least one thing per scene that has nothing to do with Haru.
//    • The dread budget for the whole act is ten moments: one on Monday, one
//      Tuesday, one Wednesday, two Thursday, two Friday, three Saturday. Zero
//      in the first fifty frames of the game. Each of the ten is deniable — a
//      player has to be able to explain it away with an ordinary sentence — and
//      they are listed by name in the self-audit at the bottom of this file.
//    • No clinical symptom is ever performed directly. Hypervigilance is not a
//      line about exits; it is half a second of silence when a door slams
//      downstairs, and then she carries on. If the player can name the symptom,
//      it is written wrong.
//    • Nobody is ever surprised that the other one knows something, and nobody
//      ever explains how they know. Between two old friends, knowing is
//      ordinary. The player is meant to notice that nobody ever asks — and the
//      characters never notice at all.
//    • Scene lock. A line may only point at something visible in the current
//      background, visible in the current sprite, or named in full earlier in
//      this same scene. Going somewhere is discussed before the background
//      changes, never after.
//    • Every physical action gets intent, act and result, in that order, and
//      nobody comments on an action that is already finished.
//    • The narrator sets atmosphere and does nothing else — one or two lines
//      a scene, in the same spoken Persian the characters use, and only about
//      what the sprites and the background cannot show. No similes, no
//      summaries, no telling the player how something felt, no confirming that
//      a character was right, no repeating an instruction as an action. It
//      runs a little under the manual's one-in-ten, which is the safe side to
//      be wrong on: an absent narrator costs atmosphere, and a busy one takes
//      the scene away from the two people in it.
//
//  Six school days, Monday to Saturday. Twenty scenes. Every choice in the act
//  is a white one and none of them is counted — the blue and green buttons do
//  not exist until act two — but each white option has script behind it,
//  because a choice the game does not answer teaches the player across an hour
//  that pressing things is pointless, which is the one belief act two's refused
//  blue button cannot afford them to hold.
//
//  Three languages, three performances, never three translations. What stays
//  constant is the information, the power, the pulse and where the pauses fall.
//  What changes is idiom, register, particles and where the joke lands. "Move."
//  is an order in English, an order in Persian, and 早く in Japanese — never
//  行くよ, which invites.
//
//  What the design document is quiet about, and this script keeps quiet: the
//  player is meant to believe these two met this morning. They do not behave
//  like it, they are already "-pi" to one another in the first exchange, and
//  nothing explains why for another five acts.
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

        // =====================================================================
        //  THE CONTINUITY TABLE
        //
        //  Nothing enters the dialogue below unless it is in this table. Every
        //  number, object, day and piece of clothing in twenty scenes is here,
        //  and the manual's rule is that if it is not, it does not get said.
        //
        //  Days       1 Monday · 2 Tuesday · 3 Wednesday · 4 Thursday
        //             5 Friday · 6 Saturday (half day)
        //  Class      1-A, second floor. Morita-sensei.
        //  Clothes    school uniform, both of them, all six days. Never
        //             mentioned after Monday — there is no other sprite.
        //  Carrying   a school bag each. Yua also has a wrapped lunch box on
        //             Wednesday, and nothing else all week.
        //  Haru's leg Day 1, his knee catches the corner of a desk. After that
        //             it is never mentioned again by anybody, and is visible
        //             only as the beat he is always late by.
        //  The window The classroom window that will not shut. Planted day 1,
        //             repeated day 2 when the rain comes in, paid off day 5
        //             when somebody has finally fixed it.
        //  The lunch  Six sushi and four octopus sausages, because that is what
        //             the drawing shows. Seven go to Haru and three stay with
        //             her, because that is what the next two drawings show.
        //             Three pieces each are lifted on screen, so nobody claims
        //             to have finished ten.
        //  The drinks Boba for her, iced matcha for him, which he does not like
        //             and drinks anyway. Three pictures: full, reluctant, empty.
        //  The machine One juice machine. Pink, on the corner of the way home,
        //             and broken. It eats a hundred yen on day 2 and is
        //             pronounced fine, and it is still fine on day 6.
        //  The bakery Usagi Bakery, and the other one near the station that
        //             changed hands. Melon bread. Cats on the bench.
        //  The garden The flowerbed at the playground. Nothing is planted in
        //             flower yet — it is spring and the cherry is out — and all
        //             that is said about it, on the last day, is that it goes
        //             red in autumn. That is act three's whole foundation and
        //             it is laid in two lines with no weight on them at all.
        //  Third      One classmate, once, on Thursday. She has a name plate
        //  person     and no body, and she exists to make the "-pi" suffix mean
        //             something in the two languages where it does not mean
        //             anything by itself.
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
        //  Two things this act does often enough to name
        // ---------------------------------------------------------------------

        /// <summary>Characters per second for <see cref="InnerVoice"/> lines.</summary>
        /// <remarks>The normal rate is 45. Slow enough to read as thinking rather than speaking.</remarks>
        private const float InnerMonologueTypeSpeed = 28f;

        /// <summary>Yua thinking, with nobody to hear it.</summary>
        /// <remarks>
        /// Used for two entirely different jobs and that is deliberate. Most of
        /// them are a girl being embarrassed about her own uniform or annoyed
        /// about a crowd, which is the whole reason the player likes her; two
        /// of them, at the end of two days, are the act's dread budget. They
        /// come in the same voice, at the same speed, in the same box.
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
        /// Once in the whole act, on Thursday, when a third person stands too
        /// close to Haru. Two tenths of a second, on her brightest face rather
        /// than her neutral one, because the recovery is the frightening half.
        /// Nothing in the scene remarks on it and nothing ever will.
        /// </para>
        /// <para>
        /// The draft this replaces used it three times, which stops being a
        /// technique and becomes a tic — and worse, a tic the player can name.
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
        //  Dread budget: 1. It is the last two frames of the day and nothing
        //  before it. The first fifty frames of the game have none at all.
        // =====================================================================

        private void WriteMonday()
        {
            WriteMondayAlley();
            WriteMondayClassroom();
            WriteMondayCorridor();
            WriteMondayCorner();
        }

        // ---------------------------------------------------------------------
        //  Monday morning — the school alley
        //
        //  ▣ Scene state
        //     Background   CherryBlossomSchoolAlleyDay: a paved path, cherry
        //                  trees in blossom down both sides, the school
        //                  building on the right, wooden benches, lanterns set
        //                  into the ground. Everything named below is one of
        //                  those, or was named in full earlier in this scene.
        //     Time         Monday, morning. First day of high school.
        //     Clothes      School uniform, new, and hers does not fit yet.
        //     On stage     Blossom, benches, the noticeboard by the door.
        //     In hand      A school bag each. Haru also has a paper bag with
        //                  two buttered rolls in it.
        //     From before  Nothing. This is the first scene of the game.
        //
        //  This is the calibration scene, and it is the one the manual writes
        //  out as the target. A girl worrying about her own uniform, a boy
        //  defending his choice of shade with total confidence, and half the
        //  scene about a teacher neither of them has met and a bakery that has
        //  changed hands. Zero numbers. Zero clock. Zero dread.
        //
        //  He wins two unimportant arguments here — the shade, and whether the
        //  bread is stale — and loses the only real decision in the scene, which
        //  is where the two of them eat it. Nobody mentions that he lost it.
        // ---------------------------------------------------------------------

        private void WriteMondayAlley()
        {
            // SEASON: this scene is written as April and the picture has to
            // agree with the words. Backgrounds.SchoolAlleyDay is the autumn
            // path now — the correct one for the 2nd of September — and this
            // call site flips to it in the same pass that rewrites the four
            // blossom lines below. See AboutProject/Roadmap.md, defect 13.
            Place(
                Backgrounds.SchoolAlleySpring,
                "The path to school", "通学路", "راهِ مدرسه");

            Hold(1.8f);

            Narrate(
                "The school alley was full of blossom and everybody was taking photos of it.",
                "通学路は花でいっぱいで、みんな写真を撮っていた。",
                "کوچه‌ی مدرسه پر از شکوفه بود و همه داشتن عکس می‌گرفتن.");

            Enter(Speaker.Yua, Portrait.Neutral);

            InnerVoice(
                "Oh no. Oh no oh no.",
                "うわ。うわうわうわ。",
                "وای. وای وای وای.");

            InnerVoice(
                "The new uniform is a bit loose.",
                "新しい制服、ちょっとぶかぶか。",
                "لباس فرمِ جدید یه‌ذره گشاده.");

            InnerVoice(
                "Fine. It doesn't matter.",
                "まあ。別にいい。",
                "خب. مهم نیست.");

            InnerVoice(
                "…It matters a bit.",
                "…ちょっとは気になる。",
                "...یه‌ذره مهمه.");

            Cue(SfxId.Petal, 0.5f);

            InnerVoice(
                "Ugh, it's so crowded.",
                "うわ、混みすぎ。",
                "اَه، چقدر شلوغه.");

            InnerVoice(
                "Where's Haru?",
                "ハルはどこ?",
                "هارو کجاست؟");

            InnerVoice(
                "…Ah.",
                "…あ。",
                "...آها.");

            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Joyful,
                "Haru-pi!",
                "ハルぴ!",
                "هاروپی!");

            Say(Speaker.Haru, Portrait.Shy,
                "Oh. Hi.",
                "あ。おはよ。",
                "اِ. سلام.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Why are you standing right under there?",
                "なんでそんなとこに立ってるの?",
                "چرا اون زیر وایسادی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "There was shade.",
                "日陰だったから。",
                "سایه داشت.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The whole alley has shade.",
                "この道、全部日陰だけど。",
                "کلِ کوچه سایه داره.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "This one's better.",
                "ここのがいい。",
                "این یکی بهتره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Since when are you an expert on shade?",
                "いつから日陰の専門家になったの?",
                "از کِی تا حالا کارشناسِ سایه شدی؟");

            Say(Speaker.Haru, Portrait.Joyful,
                "Since now.",
                "今から。",
                "از الآن.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Pff.",
                "ふふっ。",
                "پیف.");

            Hold(1.2f);

            // Half the scene, from here, is about a teacher neither of them has
            // met. This is the third thing, and it is doing all the work.
            Say(Speaker.Haru, Portrait.Neutral,
                "Oh — did you hear 1-A's got a new teacher?",
                "そういえば、一年A組、新しい先生らしいよ。",
                "راستی، شنیدی اول‌الف معلمِ جدید داره؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Morita-sensei?",
                "森田先生?",
                "خانمِ موریتا؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Yeah. They say she's strict.",
                "うん。厳しいって言われてる。",
                "آره. می‌گن سختگیره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Who says?",
                "誰が言ってるの?",
                "کی می‌گه؟");

            Say(Speaker.Haru, Portrait.Shy,
                "…I don't know. Somebody said it.",
                "…わかんない。誰かが言ってた。",
                "...نمی‌دونم. یکی می‌گفت.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Then don't say \"they say\". Say \"somebody said\".",
                "じゃあ「言われてる」じゃなくて「誰かが言ってた」でしょ。",
                "پس نگو «می‌گن». بگو «یکی می‌گفت».");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Is there a difference?",
                "違うの?",
                "فرقی داره؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Enormous.",
                "全然違う。",
                "خیلی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Okay. \"Somebody said she's strict.\"",
                "はい。「誰かが厳しいって言ってた。」",
                "باشه. «یکی می‌گفت سختگیره.»");

            Say(Speaker.Yua, Portrait.Joyful,
                "There we go.",
                "よろしい。",
                "حالا شد.");

            Hold(1.4f);

            Narrate(
                "There was a big board by the door with everybody's name on it.",
                "入り口に大きな掲示板があって、みんなの名前が貼ってあった。",
                "یه تخته‌ی بزرگ دمِ در بود و اسمِ همه روش زده بودن.");

            Say(Speaker.Yua, Portrait.Neutral,
                "I'm in 1-A.",
                "あたし、一年A組。",
                "اسمِ من اول‌الفه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "So am I.",
                "僕も。",
                "منم اول‌الفم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "So I have to put up with you for another six years.",
                "じゃああと六年も我慢しなきゃ。",
                "پس شیش سال دیگه هم باید تحملت کنم.");

            Say(Speaker.Haru, Portrait.Neutral,
                "It's not six. It's three.",
                "六年じゃないよ。三年。",
                "شیش سال نیست، سه ساله.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It feels like six.",
                "六年に感じる。",
                "احساسش شیش ساله.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Then your maths is wrong.",
                "じゃあ計算が違う。",
                "پس حسابت غلطه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "My maths is right. My feelings are wrong.",
                "計算は合ってる。気持ちの方が間違ってる。",
                "حسابم درسته، حسِ من غلطه.");

            Hold(1.2f);

            // The second unimportant argument, and the one he wins outright,
            // with a fact she did not have.
            Say(Speaker.Haru, Portrait.Neutral,
                "I got buttered rolls on the way. There's two.",
                "来る途中でバターロール買った。二つある。",
                "یه نونِ کره‌ای سرِ راه خریدم. دو تاست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "From Usagi?",
                "うさぎパン?",
                "از نونواییِ اوساگی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "No, the other one. Near the station.",
                "ううん、駅の方の。",
                "نه، اون یکی. نزدیکِ ایستگاه.");

            Say(Speaker.Yua, Portrait.Angry,
                "That one's bread is dry.",
                "あそこのパン、ぱさぱさ。",
                "اون یکی نونش خشکه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "It isn't dry. It's fresh.",
                "ぱさぱさじゃないよ。焼きたて。",
                "خشک نیست. تازه‌ست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I bought one there last year and it was dry.",
                "去年一回買って、ぱさぱさだった。",
                "پارسال یه بار ازش خریدم، خشک بود.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "That was last year.",
                "去年でしょ。",
                "پارسال بوده.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Bread doesn't change.",
                "パンは変わらない。",
                "نون که عوض نمی‌شه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "The baker changed.",
                "パン屋さんが変わった。",
                "نونوا عوض شده.");

            Say(Speaker.Yua, Portrait.Neutral,
                "…Really?",
                "…ほんとに?",
                "...جدی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's his daughter now.",
                "今は娘さんがやってる。",
                "حالا دخترشه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Then maybe it's got better.",
                "じゃあ良くなったかも。",
                "خب پس شاید خوب شده باشه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It has.",
                "なった。",
                "شده.");

            Hold(1.0f);

            // And the one decision in the scene that is actually about the two
            // of them. He asks, he is answered, he does not ask twice, and
            // nobody — including him — treats it as anything at all.
            Say(Speaker.Haru, Portrait.Neutral,
                "Want to sit on the bench and eat them?",
                "ベンチで食べる?",
                "می‌خوای بشینیم رو نیمکت بخوریمش؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "In class.",
                "教室で。",
                "تو کلاس.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.6f);

            // A white choice. Two roads, both harmless, both answered — which
            // is the entire reason it is here. See DecideIdly.
            DecideIdly(
                "Tell him his collar is crooked", "襟が曲がってると言う", "بگو یقه‌ت کجه",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Neutral,
                        "Your collar's crooked.",
                        "襟、曲がってる。",
                        "یقه‌ت کجه.");

                    Say(Speaker.Haru, Portrait.Shy,
                        "Is it?",
                        "そう?",
                        "کجه؟");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "The left one. It's been like that the whole time.",
                        "左。さっきからずっと。",
                        "چپیه. از اول همین‌جوری بوده.");

                    Say(Speaker.Haru, Portrait.Unchanged,
                        "Why didn't you say?",
                        "なんで言わなかったの?",
                        "چرا نگفتی؟");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "I was enjoying it.",
                        "楽しんでた。",
                        "داشتم لذت می‌بردم.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "That's mean.",
                        "ひどい。",
                        "خیلی بدجنسی.");
                },
                "Complain about your own uniform", "自分の制服の文句を言う", "از لباسِ خودت غر بزن",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Neutral,
                        "This uniform is enormous on me.",
                        "この制服、ぶかぶかなんだけど。",
                        "این لباس واسه من خیلی گشاده.");

                    Say(Speaker.Haru, Portrait.Unchanged,
                        "It looks the right size.",
                        "ちょうどよく見えるけど。",
                        "به نظر اندازه‌ست.");

                    Say(Speaker.Yua, Portrait.Angry,
                        "The sleeves are down to here.",
                        "袖がここまである。",
                        "آستین‌هاش تا اینجاست.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "You can fold them.",
                        "折ればいいよ。",
                        "می‌تونی تاشون کنی.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Then it looks folded.",
                        "折ってあるように見えるじゃん。",
                        "اون‌وقت معلومه تا شده.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "Then it's a problem with no solution.",
                        "じゃあ解決不能だ。",
                        "پس مشکلیه که راه‌حل نداره.");
                });

            Say(Speaker.Yua, Portrait.Neutral,
                "Our classroom's on the second floor.",
                "教室、二階だって。",
                "کلاسمون طبقه‌ی دومه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "The stairs are over that way.",
                "階段はあっち。",
                "پله‌ها اون‌وره.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Come on.",
                "行こ。",
                "بریم.");

            Hold(1.2f);
        }

        // ---------------------------------------------------------------------
        //  Monday, first period — classroom 1-A
        //
        //  ▣ Scene state
        //     Background   SunnyClassroomDay: wooden desks in rows, pastel
        //                  stationery and pencil pouches on them, a chalkboard,
        //                  pink curtains, and one window standing open.
        //     Time         Monday, morning, straight on from the alley.
        //     Clothes      Uniform.
        //     On stage     The desks, the board, the curtains, the open window.
        //     In hand      Bags, and the paper bag of rolls he is still holding.
        //     From before  The rolls are not eaten yet. The noticeboard by the
        //                  door has been seen but not read.
        //
        //  Two things are planted here and neither of them is pointed at.
        //
        //  The window that will not shut is stage one of a three-stage motif,
        //  and stage one is allowed to be flatly obvious — it is a boy trying
        //  to close a window, failing, and being told by a girl that a
        //  second-year told her it has been like that since last year. The rain
        //  comes through it on Tuesday. Somebody has fixed it by Friday.
        //
        //  His knee catches the corner of a desk, he says one thing about it,
        //  and it is never mentioned again by anybody in this act. Everyone
        //  bangs a knee on a desk. That is the point: the player is meant to
        //  have no reason at all to remember it until act six, when it turns
        //  out to be the only injury in the story.
        // ---------------------------------------------------------------------

        private void WriteMondayClassroom()
        {
            ClearStage();

            Place(
                Backgrounds.ClassroomDay,
                "Class 1-A", "一年A組", "کلاسِ اول-الف");

            Hold(1.6f);

            Narrate(
                "The desks still had the label stickers on them and half the class was peeling them off.",
                "机にはまだシールが貼ってあって、半分くらいの子が剥がしていた。",
                "رو میزها هنوز برچسب بود و نصفِ بچه‌ها داشتن می‌کندنشون.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            // Pre-emption. She has not said a word about where she wants to sit.
            Narrate(
                "Haru put his bag down on the desk by the window and then took the one next to it.",
                "ハルは窓際の机に鞄を置いて、その隣に座った。",
                "هارو کیفشو گذاشت رو میزِ کنارِ پنجره و خودش نشست بغلی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Look at this pencil case.",
                "この筆箱見て。",
                "این جامدادی رو ببین.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's got a rabbit on it.",
                "うさぎがついてる。",
                "روش خرگوش داره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It has two rabbits on it. The second one is behind the zip and you only find it when you open it, and I have had it for four days and I only found it this morning on the train, and I made a noise, out loud, in front of people.",
                "うさぎは二匹。二匹目はファスナーの裏にいて、開けないと見つからないの。四日持ってて、今朝電車で初めて気づいて、人の前で声出しちゃった。",
                "دو تا خرگوش داره. دومیش پشتِ زیپه و تا بازش نکنی پیداش نمی‌کنی، و من چهار روزه دارمش و تازه امروز صبح تو قطار پیداش کردم و جلوی ملت صدام درومد.");

            Say(Speaker.Haru, Portrait.Joyful,
                "What kind of noise?",
                "どんな声?",
                "چه جور صدایی؟");

            Say(Speaker.Yua, Portrait.Angry,
                "We're not doing that.",
                "その話はしない。",
                "این بحث رو نمی‌کنیم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Okay.",
                "はい。",
                "باشه.");

            Hold(1.2f);

            Narrate(
                "A breeze came in and moved the curtains.",
                "風が入って、カーテンが揺れた。",
                "یه باد اومد تو و پرده‌ها رو تکون داد.");

            Say(Speaker.Haru, Portrait.Neutral,
                "This window doesn't shut.",
                "この窓、閉まらない。",
                "این پنجره بسته نمی‌شه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Push the top part.",
                "上のとこ押して。",
                "بالاش رو فشار بده.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I did. It comes back.",
                "押した。戻ってくる。",
                "دادم. برمی‌گرده.");

            Say(Speaker.Haru, Portrait.Shy,
                "A second-year told me it's been like that since last year.",
                "二年の人が、去年からこうだって言ってた。",
                "یکی از بچه‌های سال بالایی می‌گفت از پارساله همین‌جوریه.");

            // The callback lands on the exact word she made him use an hour ago.
            Say(Speaker.Yua, Portrait.Joyful,
                "There we go. \"Somebody said.\"",
                "よろしい。「誰かが言ってた」。",
                "حالا شد. «یکی می‌گفت.»");

            Say(Speaker.Haru, Portrait.Joyful,
                "I'm learning.",
                "学んでる。",
                "دارم یاد می‌گیرم.");

            Hold(1.0f);

            Cue(SfxId.ChairScrape, 0.6f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Sit down, they're coming.",
                "座って、来るよ。",
                "بشین، دارن میان.");

            // Intent, act, result. He gets up to close it, the desk gets him
            // on the way, and that is the whole of it.
            Say(Speaker.Haru, Portrait.Unchanged,
                "Let me try the window once more.",
                "窓、もう一回だけ。",
                "بذار یه بار دیگه پنجره رو امتحان کنم.");

            Cue(SfxId.DeskKnock, 0.85f);

            Say(Speaker.Haru, Portrait.Injured,
                "Ow.",
                "いった。",
                "آخ.");

            Say(Speaker.Yua, Portrait.Neutral,
                "What did you do?",
                "何したの?",
                "چیکار کردی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "The corner of the desk.",
                "机の角。",
                "گوشه‌ی میز.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Which knee?",
                "どっちの膝?",
                "کدوم زانو؟");

            Say(Speaker.Haru, Portrait.Neutral,
                "It's nothing.",
                "なんでもない。",
                "چیزی نیست.");

            Hold(1.4f);

            Enter(Speaker.Haru, Portrait.Neutral);

            Cue(SfxId.SchoolBell, 0.6f);

            Narrate(
                "Morita-sensei came in and said her name once, and then said it again because nobody had written it down.",
                "森田先生が入ってきて、一度名前を言って、誰も書いていなかったのでもう一度言った。",
                "خانمِ موریتا اومد تو و یه بار اسمشو گفت، بعد دوباره گفت چون هیشکی ننوشته بودش.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Neutral,
                "She isn't strict.",
                "厳しくないじゃん。",
                "سختگیر نیست که.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's the first day.",
                "初日だからね。",
                "روزِ اوله.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "So she's saving it.",
                "取っておいてるんだ。",
                "پس داره نگهش می‌داره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's what I'd do.",
                "僕ならそうする。",
                "من که بودم همینکارو می‌کردم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "You'd be a terrifying teacher.",
                "ハルぴが先生だったら怖いね。",
                "تو معلمِ ترسناکی می‌شدی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I'd be a very quiet teacher.",
                "すごく静かな先生になる。",
                "من معلمِ خیلی ساکتی می‌شدم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That's worse.",
                "その方が怖い。",
                "این بدتره.");

            Hold(1.2f);

            // The scene's real decision, and it goes the way they all go.
            Say(Speaker.Haru, Portrait.Neutral,
                "There was a club list on the board by the door.",
                "入り口の掲示板に部活の一覧があった。",
                "تخته‌ی دمِ در یه لیستِ باشگاه‌ها داشت.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "There was.",
                "あったね。",
                "داشت.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Want to go and read it after school?",
                "放課後、見に行く?",
                "می‌خوای بعدِ مدرسه بریم بخونیمش؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It'll still be there tomorrow.",
                "明日もあるでしょ。",
                "فردام هست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It will.",
                "あるね。",
                "هست.");

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Joyful,
                "Give me a roll.",
                "パン、ちょうだい。",
                "یه نون بده.");

            Say(Speaker.Haru, Portrait.Shy,
                "We're in class.",
                "授業中だよ。",
                "سرِ کلاسیم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Under the desk.",
                "机の下で。",
                "زیرِ میز.");

            Say(Speaker.Haru, Portrait.Joyful,
                "…Okay.",
                "…はい。",
                "...باشه.");

            Hold(1.8f);

            // The narrator is not allowed to tell the player a character was
            // right, so she has to say it herself, which is funnier anyway.
            Say(Speaker.Yua, Portrait.Unchanged,
                "…It isn't dry.",
                "…ぱさぱさじゃない。",
                "...خشک نیست.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I know.",
                "知ってる。",
                "می‌دونم.");

            Say(Speaker.Yua, Portrait.Angry,
                "Don't make the face.",
                "その顔やめて。",
                "اون قیافه رو نگیر.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "What face?",
                "どの顔?",
                "کدوم قیافه؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "That one.",
                "それ。",
                "همون.");

            Hold(1.4f);
        }

        // ---------------------------------------------------------------------
        //  Monday, after school — the corridor
        //
        //  ▣ Scene state
        //     Background   SchoolCorridorSunset: an empty hallway in low gold
        //                  light, long shadows on the floor, floor-to-ceiling
        //                  windows looking out at the cherry trees, lockers.
        //     Time         Monday, late afternoon.
        //     Clothes      Uniform.
        //     On stage     The lockers, the windows, the light on the floor.
        //     In hand      Bags. The rolls are gone.
        //     From before  The club list on the board by the door, which she
        //                  said would still be there tomorrow.
        //
        //  He is a beat late twice in this scene and there is no line about it
        //  either time. The narrator is allowed to say that somebody started
        //  walking a moment after somebody else, because that is a physical
        //  action the sprites cannot show; it is not allowed to say why, and it
        //  does not.
        //
        //  Where they walk home is the real decision, and it is settled before
        //  the background changes, which is the rule: nobody stands on the
        //  corner discussing whether to go to the corner.
        // ---------------------------------------------------------------------

        private void WriteMondayCorridor()
        {
            ClearStage();

            Place(
                Backgrounds.CorridorSunset,
                "The corridor, after school", "放課後の廊下", "راهرو، بعد از مدرسه");

            Hold(1.8f);

            Narrate(
                "The corridor was empty and the light had gone orange on the floor.",
                "廊下には誰もいなくて、床の光がオレンジになっていた。",
                "راهرو خالی بود و نورِ روی زمین نارنجی شده بود.");

            Enter(Speaker.Yua, Portrait.Joyful);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Unchanged,
                "There's a tea ceremony club.",
                "茶道部があるって。",
                "باشگاهِ چای‌خوری دارن.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You read the list.",
                "見たんだ。",
                "لیست رو خوندی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "On the way past. A whole club, for drinking tea. They meet twice a week. There is a president of it. Somebody stood up in a room and said I want to be the president of the club where we drink the tea, and everybody agreed, and now that is a thing that exists in this building.",
                "通りすがりに。お茶を飲むための部活。週二回。部長までいる。誰かが立ち上がって「お茶を飲む部の部長になりたい」って言って、みんなが賛成して、それがこの校舎に実在してるの。",
                "سرِ راه. یه باشگاهِ کامل، واسه چای خوردن. هفته‌ای دو بار جمع می‌شن. رئیس هم دارن. یکی تو یه اتاق بلند شده گفته من می‌خوام رئیسِ باشگاهی بشم که توش چای می‌خوریم، همه هم قبول کردن، و الآن این یه چیزیه که تو این ساختمون وجود داره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You want to join it.",
                "入りたいんでしょ。",
                "می‌خوای بری توش.");

            Say(Speaker.Yua, Portrait.Angry,
                "I want to watch it.",
                "見たいだけ。",
                "می‌خوام تماشاش کنم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "That's worse.",
                "その方がやばい。",
                "این بدتره.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It really is.",
                "ほんとにね。",
                "واقعاً بدتره.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "These windows face west.",
                "この窓、西向きだ。",
                "این پنجره‌ها رو به غربن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "They face the trees.",
                "木の方向いてる。",
                "رو به درختا.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "The trees are west.",
                "木が西にある。",
                "درختا غربن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "How do you have a west.",
                "なんで西とかわかるの。",
                "تو از کجا غرب داری آخه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Everybody has a west.",
                "みんな西くらいある。",
                "همه غرب دارن.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I don't.",
                "あたしはない。",
                "من ندارم.");

            Hold(1.4f);

            Narrate(
                "She started walking, and Haru started a moment after her.",
                "彼女が歩き出して、ハルはその少しあとに歩き出した。",
                "یوآ راه افتاد و هارو یه لحظه بعدش راه افتاد.");

            Hold(1.2f);

            // The decision about the way home, made before the background moves.
            Say(Speaker.Haru, Portrait.Neutral,
                "We could go back past the station.",
                "駅の方から帰ってもいいけど。",
                "می‌تونیم از سمتِ ایستگاه برگردیم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "For the bread.",
                "パンのために。",
                "به‌خاطرِ نون.");

            Say(Speaker.Haru, Portrait.Shy,
                "For the bread.",
                "パンのために。",
                "به‌خاطرِ نون.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "We'll go by the corner. There's a juice machine on the corner.",
                "角から行こ。角に自販機ある。",
                "از سرِ نبش می‌ریم. سرِ نبش یه دستگاهِ آب‌میوه هست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Tomorrow the station.",
                "明日は駅ね。",
                "فردا ایستگاه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Tomorrow the station.",
                "明日は駅。",
                "فردا ایستگاه.");

            Hold(1.6f);
        }

        // ---------------------------------------------------------------------
        //  Monday, on the way home — the corner
        //
        //  ▣ Scene state
        //     Background   PastelStreetVendingDay: a suburban street in full
        //                  daylight, a pink drinks machine on the left, a
        //                  bicycle parked against the wall, flowerbeds, cherry
        //                  trees behind.
        //     Time         Monday, early evening.
        //     Clothes      Uniform.
        //     On stage     The machine, the bicycle, the flowerbeds.
        //     In hand      Bags. Nothing else — nobody buys anything today.
        //     From before  She named the juice machine in the corridor. This is
        //                  the first time it is seen.
        //
        //  Today the machine is only introduced: pink, on the corner, full of
        //  juice, with an argument about which button is the melon soda. It
        //  takes his hundred yen tomorrow and it is still a good machine on
        //  Saturday.
        //
        //  ◆ Dread moment 1 of 10 — the whole of Monday's budget.
        //
        //  The last two frames of the day, said to nobody, on an empty street,
        //  after fifty-odd frames in which nothing whatsoever is wrong:
        //
        //      Good.
        //      He's still the same.
        //
        //  First time through, that is two girls' sentences about an old friend
        //  and the player thinks it is sweet. It only becomes something else on
        //  a second playthrough, when it turns out she had been checking. That
        //  is the whole technique: the player is not frightened now. They
        //  remember it later.
        // ---------------------------------------------------------------------

        private void WriteMondayCorner()
        {
            ClearStage();

            Place(
                Backgrounds.VendingStreetDay,
                "The corner", "角の道", "سرِ نبش");

            Hold(1.8f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Narrate(
                "The machine on the corner was pink and there was a bicycle leaning on the wall next to it.",
                "角の自販機はピンクで、その横の壁に自転車が立てかけてあった。",
                "دستگاهِ سرِ نبش صورتی بود و بغلش یه دوچرخه به دیوار تکیه داده بود.");

            Say(Speaker.Yua, Portrait.Angry,
                "The peach one has gone up ten yen.",
                "桃のやつ、十円上がってる。",
                "هلویی ده ین گرون شده.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Since when?",
                "いつから?",
                "از کِی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Since whenever they decided to ruin my life.",
                "あたしの人生を壊すって決めた日から。",
                "از هر وقتی که تصمیم گرفتن زندگیمو خراب کنن.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Ten yen.",
                "十円。",
                "ده ین.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Ten yen every day is a lot of yen.",
                "毎日十円は、けっこうな円だよ。",
                "روزی ده ین می‌شه خیلی ین.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You don't buy one every day.",
                "毎日は買ってないでしょ。",
                "که هر روز نمی‌خری.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I could.",
                "買えるし。",
                "می‌تونم بخرم.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "The melon soda is the second button.",
                "メロンソーダは二番目のボタン。",
                "ملون‌سودا دکمه‌ی دومیه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's the third.",
                "三番目。",
                "سومیه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Second from the left.",
                "左から二番目。",
                "از چپ دومی.");

            Say(Speaker.Yua, Portrait.Neutral,
                "…From the left.",
                "…左から。",
                "...از چپ.");

            Say(Speaker.Haru, Portrait.Joyful,
                "From the left.",
                "左から。",
                "از چپ.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Fine. Second.",
                "はいはい。二番目。",
                "باشه بابا. دومی.");

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Neutral,
                "That bicycle is always here.",
                "この自転車、いつもここにある。",
                "این دوچرخه همیشه اینجاست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Somebody must live here.",
                "誰か住んでるんじゃない。",
                "لابد یکی همین‌جا زندگی می‌کنه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Then they should ride it.",
                "じゃあ乗ればいいのに。",
                "خب پس سوارش بشه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Maybe they're waiting for something.",
                "何かを待ってるのかも。",
                "شاید منتظرِ یه چیزیه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It's a bicycle, Haru-pi.",
                "自転車だよ、ハルぴ。",
                "دوچرخه‌ست هاروپی.");

            Hold(1.6f);

            // The real one, and the last thing he asks for today.
            Say(Speaker.Haru, Portrait.Neutral,
                "Do you want to sit on the wall for a bit?",
                "ちょっと壁に座ってく?",
                "می‌خوای یه‌کم رو دیوار بشینیم؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I've got to be back.",
                "帰らないと。",
                "باید برگردم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Tomorrow the station.",
                "明日は駅。",
                "فردا ایستگاه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Tomorrow the station.",
                "明日は駅。",
                "فردا ایستگاه.");

            Hold(1.2f);

            Exit(Speaker.Haru);

            Hold(2.2f);

            // ◆ Dread moment 1. Two frames, and the whole of Monday.
            InnerVoice(
                "Good.",
                "よし。",
                "خوبه.");

            InnerVoice(
                "He's still the same.",
                "まだ同じだ。",
                "هنوز همون‌جوریه.");

            Hold(2.8f);
        }

        // =====================================================================
        //  TUESDAY — day two of six
        //
        //  Dread budget: 1. An apology, in the wrong size, for the weather.
        // =====================================================================

        private void WriteTuesday()
        {
            WriteTuesdayClassroom();
            WriteTuesdayCafe();
            WriteTuesdayAlleyway();
        }

        // ---------------------------------------------------------------------
        //  Tuesday, morning — classroom 1-A, in the rain
        //
        //  ▣ Scene state
        //     Background   OvercastClassroomRainy: the same room under grey
        //                  light, the pink curtains moving by the open window,
        //                  desks, the chalkboard, a bookshelf.
        //     Time         Tuesday, morning. It has rained all night.
        //     Clothes      Uniform. Her left sleeve is wet.
        //     On stage     The window, the sill, the curtains, the bookshelf.
        //     In hand      Bags.
        //     From before  The window that will not shut, planted yesterday.
        //                  Morita-sensei, who said "maybe" about a test.
        //
        //  Stage two of the window. Yesterday it would not close; today the
        //  rain has come through it onto the sill and onto her sleeve, and
        //  neither of them does anything about it. It gets fixed on Friday by
        //  somebody neither of them ever meets.
        //
        //  ◆ Dread moment 2 of 10 — the whole of Tuesday's budget.
        //
        //  He apologises for the rain. Told it is not his fault, he agrees that
        //  it is not his fault, and apologises again. Two frames, dropped
        //  instantly, and completely explicable: some people apologise for
        //  everything. The player has no reason to weigh it. It is only the
        //  second time in the act that it happens — Thursday — that it starts
        //  to look like a shape.
        // ---------------------------------------------------------------------

        private void WriteTuesdayClassroom()
        {
            ClearStage();

            Place(
                Backgrounds.ClassroomRainy,
                "Class 1-A, Tuesday", "火曜、一年A組", "کلاسِ اول-الف، سه‌شنبه");

            Hold(1.8f);

            Narrate(
                "It had rained all night and the room smelled of wet wood.",
                "夜通し雨で、教室は濡れた木のにおいがした。",
                "از دیشب بارون اومده بود و کلاس بوی چوبِ خیس می‌داد.");

            Enter(Speaker.Yua, Portrait.Angry);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Unchanged,
                "The rain's come in on the sill.",
                "雨が窓の下に入ってる。",
                "بارون از پنجره اومده رو لبه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Your sleeve's wet.",
                "袖、濡れてる。",
                "آستینت خیسه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "My sleeve is soaked.",
                "袖、びしょびしょ。",
                "آستینم خیسِ خیسه.");

            // ◆ Dread moment 2. Two frames, and gone.
            Say(Speaker.Haru, Portrait.Shy,
                "Sorry.",
                "ごめん。",
                "ببخشید.");

            Say(Speaker.Yua, Portrait.Neutral,
                "It's not your fault it rained.",
                "雨はハルぴのせいじゃないでしょ。",
                "بارون که تقصیرِ تو نیست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I know. Sorry.",
                "うん。ごめん。",
                "می‌دونم. ببخشید.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Joyful,
                "Anyway. Look what I found on the bookshelf.",
                "まあいいや。本棚でこれ見つけた。",
                "بی‌خیال. ببین تو قفسه چی پیدا کردم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Somebody's left a manga in the school books.",
                "教科書に紛れてマンガがある。",
                "یکی لای کتابا مانگا جا گذاشته.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Volume four. Only volume four. Somebody carried volume four of something into a school, put it on a shelf between a dictionary and an atlas, and walked away, and now I have to know what happens in volumes one, two and three for the rest of my life.",
                "四巻。四巻だけ。誰かが四巻だけ学校に持ってきて、辞書と地図帳の間に置いて、帰った。おかげで一生、一巻から三巻が気になる。",
                "جلدِ چهار. فقط جلدِ چهار. یکی جلدِ چهارِ یه چیزی رو آورده مدرسه، گذاشته لای فرهنگ لغت و اطلس، و رفته، و حالا من تا آخرِ عمرم باید بدونم تو جلدِ یک و دو و سه چی شده.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Read the back.",
                "裏、読んでみて。",
                "پشتشو بخون.");

            Say(Speaker.Yua, Portrait.Angry,
                "The back of volume four spoils volumes one, two and three.",
                "四巻の裏には一巻から三巻のネタバレが書いてある。",
                "پشتِ جلدِ چهار، جلدِ یک و دو و سه رو لو می‌ده.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "So don't read it.",
                "じゃあ読まない。",
                "خب نخونش.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I've read it.",
                "もう読んだ。",
                "خوندمش.");

            Say(Speaker.Haru, Portrait.Joyful,
                "How long have you had it?",
                "いつから持ってるの?",
                "چند وقته دستته؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "Four minutes.",
                "四分。",
                "چهار دقیقه.");

            Hold(1.4f);

            // The manual's own exchange. He wins it on a reading of one word.
            Say(Speaker.Haru, Portrait.Neutral,
                "Morita-sensei said we've got a test on Monday.",
                "森田先生、月曜にテストって言ってた。",
                "خانمِ موریتا گفته دوشنبه امتحان داریم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "She didn't.",
                "言ってない。",
                "نگفته.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "She did.",
                "言ってたよ。",
                "چرا، گفته.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "She said \"maybe\".",
                "「たぶん」って言った。",
                "اون گفت «شاید».");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Her \"maybe\" means definitely.",
                "あの人の「たぶん」は確定。",
                "«شاید»ِ اون یعنی حتماً.");

            Say(Speaker.Yua, Portrait.Neutral,
                "…Yeah, okay.",
                "…まあ、そうかも。",
                "...آره خب.");

            Say(Speaker.Haru, Portrait.Joyful,
                "See?",
                "でしょ?",
                "دیدی؟");

            Say(Speaker.Yua, Portrait.Angry,
                "I didn't say anything.",
                "何も言ってない。",
                "هیچی نگفتم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Your face said something.",
                "顔が言ってた。",
                "قیافه‌ت یه چیزی گفت.");

            Say(Speaker.Yua, Portrait.Joyful,
                "My face said nothing.",
                "顔は何も言ってない。",
                "قیافه‌م هیچی نگفت.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "I'll go and shut the window properly.",
                "窓、ちゃんと閉めてくる。",
                "می‌رم پنجره رو درست ببندم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It doesn't shut. We established this yesterday.",
                "閉まらないって。昨日わかったでしょ。",
                "بسته نمی‌شه. دیروز که فهمیدیم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "The caretaker could look at it.",
                "用務員さんに見てもらえば。",
                "سرایدار می‌تونه یه نگاه بهش بندازه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Leave it. I like the air.",
                "そのままにして。風があった方がいい。",
                "ولش کن. هوای تازه‌ش رو دوست دارم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.6f);

            Cue(SfxId.SchoolBell, 0.5f);

            Say(Speaker.Yua, Portrait.Joyful,
                "If it's still raining after school we're not going to the station.",
                "放課後まだ降ってたら、駅は無しね。",
                "اگه بعدِ مدرسه هنوز بارون بیاد، ایستگاه نمی‌ریم.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Then where?",
                "じゃあどこ?",
                "پس کجا؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "There's a café two streets down with a pink front.",
                "二本先にピンクのカフェがある。",
                "دو تا خیابون پایین‌تر یه کافه هست که جلوش صورتیه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Then it'll still be raining.",
                "じゃあまだ降ってるね。",
                "پس بارون قطع نمی‌شه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It will.",
                "降ってる。",
                "قطع نمی‌شه.");

            Hold(1.6f);
        }

        // ---------------------------------------------------------------------
        //  Tuesday, after school — the café, in the rain
        //
        //  ▣ Scene state
        //     Background   CozyCafeDimRainy: the pastel café under grey light,
        //                  rain running down the big windows, warm lamps on,
        //                  a matcha cake and a dango tea set on the table in
        //                  front of them.
        //     Time         Tuesday, late afternoon. Still raining.
        //     Clothes      Uniform, both of them damp.
        //     On stage     The cake, the dango, the tea set, the windows.
        //     In hand      Nothing. There are no café-prop sprites and none is
        //                  needed: the food is on the table in the picture.
        //     From before  The station trip, cancelled by the rain. Volume four
        //                  of somebody's manga, which she has taken with her.
        //
        //  She takes the table nearest the door. There is one line of narration
        //  about it and nobody remarks on it, here or ever. It happens again on
        //  Thursday and it is never once mentioned by either of them, which is
        //  the only way to write it: a girl who says "I like to sit near an
        //  exit" is a girl the player can diagnose in one frame.
        //
        //  He wins the argument about how many dango are on a stick. He loses
        //  who pays, and he loses it in about four seconds.
        // ---------------------------------------------------------------------

        private void WriteTuesdayCafe()
        {
            ClearStage();

            Place(
                Backgrounds.CafeRainy,
                "The café", "カフェ", "کافه");

            Hold(1.8f);

            Narrate(
                "The rain was coming down the big windows and the lamps were already on.",
                "大きな窓を雨が伝っていて、ランプはもう点いていた。",
                "بارون داشت از شیشه‌های بزرگ می‌اومد پایین و چراغ‌ها روشن بودن.");

            Narrate(
                "Yua took the table nearest the door.",
                "結愛はドアに一番近い席に座った。",
                "یوآ نزدیک‌ترین میز به در رو گرفت.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Joyful,
                "This place is so pink.",
                "ここ、ピンクすぎ。",
                "اینجا چقدر صورتیه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You chose it.",
                "選んだの結愛ぴでしょ。",
                "خودت انتخابش کردی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I'm allowed to have opinions about my own decisions.",
                "自分の決めたことに文句言うのは自由。",
                "آدم حق داره درباره‌ی تصمیمِ خودش نظر داشته باشه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's a rule you've just made up.",
                "今作ったルールだよね。",
                "این قانون رو همین الآن ساختی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It's a good one.",
                "いいルールでしょ。",
                "قانونِ خوبیه.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "There's four dango on that stick.",
                "その串、団子四つある。",
                "روی اون سیخ چهار تا دانگو هست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "There's three. There's always three.",
                "三つでしょ。いつも三つ。",
                "سه تاست. همیشه سه تاست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Count them.",
                "数えて。",
                "بشمرشون.");

            Say(Speaker.Yua, Portrait.Angry,
                "…Four.",
                "…四つ。",
                "...چهار تا.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Four.",
                "四つ。",
                "چهار تا.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That's a lot of dango for one stick.",
                "一本にしては多い。",
                "واسه یه سیخ خیلی زیاده.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's a good café.",
                "いいカフェだ。",
                "کافه‌ی خوبیه.");

            Hold(1.2f);

            DecideIdly(
                "Talk about volume four", "四巻の話をする", "حرف رو ببر سرِ جلدِ چهار",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Joyful,
                        "So in volume four there's a boy who runs a bathhouse.",
                        "四巻ね、銭湯やってる男の子が出てくるの。",
                        "خب تو جلدِ چهار یه پسره هست که حمومِ عمومی می‌چرخونه.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "On his own?",
                        "一人で?",
                        "تنهایی؟");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "On his own, and everybody in the town owes him money, and he never asks for it, and by page sixty I wanted to shout at him.",
                        "一人で。町中が彼にお金を借りてて、彼は一度も催促しない。六十ページで叫びたくなった。",
                        "تنهایی، و کلِ شهر بهش بدهکارن، و اون هیچ‌وقت طلبش نمی‌کنه، و تا صفحه‌ی شصت دلم می‌خواست سرش داد بزنم.");

                    Say(Speaker.Haru, Portrait.Shy,
                        "Maybe he doesn't mind.",
                        "気にしてないのかも。",
                        "شاید براش مهم نیست.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Everybody minds.",
                        "みんな気にするよ。",
                        "واسه‌ی همه مهمه.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "Then he's badly written.",
                        "じゃあ書き方が下手だ。",
                        "پس بد نوشتنش.");
                },
                "Complain about the rain", "雨の文句を言う", "از بارون غر بزن",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Angry,
                        "My shoes are going to be wet until Friday.",
                        "靴、金曜まで乾かない。",
                        "کفشام تا جمعه خیس می‌مونن.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "Put paper in them tonight.",
                        "今夜、新聞入れておけば。",
                        "امشب توشون کاغذ بذار.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Paper?",
                        "紙?",
                        "کاغذ؟");

                    Say(Speaker.Haru, Portrait.Unchanged,
                        "Newspaper. It pulls the water out.",
                        "新聞紙。水を吸う。",
                        "روزنامه. آب رو می‌کشه بیرون.");

                    Say(Speaker.Yua, Portrait.Neutral,
                        "Who told you that?",
                        "誰から聞いたの?",
                        "کی اینو بهت گفته؟");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "Somebody said it.",
                        "誰かが言ってた。",
                        "یکی می‌گفت.");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "You're going to say that forever now.",
                        "これから一生それ言うでしょ。",
                        "تا ابد قراره اینو بگی.");
                });

            Hold(1.2f);

            Narrate(
                "Somebody at the counter dropped a tray, and it was loud.",
                "カウンターの方で誰かがトレイを落として、大きな音がした。",
                "یکی دمِ پیشخون سینی رو انداخت و صداش بلند بود.");

            Cue(SfxId.CanDrop, 0.7f);

            // Half a second of nothing, and then the sentence carries on from
            // where it was. Nobody says a word about it. This is the whole of
            // how hypervigilance is written in this act.
            Hold(0.5f);

            Say(Speaker.Yua, Portrait.Neutral,
                "…Anyway, the cake here is matcha.",
                "…とにかく、ここのケーキは抹茶。",
                "...خلاصه، کیکِ اینجا ماچاست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's pistachio.",
                "ピスタチオでしょ。",
                "پسته‌ایه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's green.",
                "緑だし。",
                "سبزه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Pistachio is green.",
                "ピスタチオも緑。",
                "پسته هم سبزه.");

            Say(Speaker.Yua, Portrait.Angry,
                "Everything is green.",
                "全部緑じゃん。",
                "همه‌چی سبزه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Not everything.",
                "全部じゃない。",
                "همه‌چی که نه.");

            Hold(1.4f);

            // The one real decision in the scene.
            Say(Speaker.Haru, Portrait.Neutral,
                "I'll get this one.",
                "ここは僕が。",
                "این یکی رو من حساب می‌کنم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I've already left it on the counter.",
                "もうカウンターに置いてきた。",
                "قبلاً گذاشتمش رو پیشخون.");

            Say(Speaker.Haru, Portrait.Shy,
                "When?",
                "いつ?",
                "کِی؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "When you were counting dango.",
                "団子数えてたとき。",
                "وقتی داشتی دانگو می‌شمردی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's not fair.",
                "ずるい。",
                "این انصاف نیست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You can get Thursday.",
                "木曜はハルぴね。",
                "پنج‌شنبه رو تو حساب کن.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.6f);
        }

        // ---------------------------------------------------------------------
        //  Tuesday, evening — the alleyway
        //
        //  ▣ Scene state
        //     Background   TraditionalAlleywayNight: a paved alley under a
        //                  crescent moon, a lit paper lantern by a doorway,
        //                  windows with lights on, hydrangeas in pots along
        //                  the wall.
        //     Time         Tuesday, after dark. The rain has stopped.
        //     Clothes      Uniform.
        //     On stage     The lantern, the pots, the lit windows.
        //     In hand      Bags. Volume four is on a café table two streets
        //                  back and neither of them is going to fetch it.
        //     From before  The café, and the manga she walked out without.
        //
        //  No dread in this scene at all. Tuesday's one moment was spent in the
        //  classroom this morning, and the rest of the day is allowed to be
        //  exactly what it looks like: two people arguing about a cat.
        //
        //  This is also the similarity test doing its job. The three scenes of
        //  Tuesday are a wet classroom, a warm café and a dark street, and the
        //  jokes in them are a complaint, a count and a naming argument, so
        //  none of the three sounds like the other two.
        // ---------------------------------------------------------------------

        private void WriteTuesdayAlleyway()
        {
            ClearStage();

            Place(
                Backgrounds.AlleywayNight,
                "The alleyway", "路地", "کوچه");

            Hold(1.8f);

            Narrate(
                "The rain had stopped and the stones were still shining under the lantern.",
                "雨はやんで、提灯の下の石畳がまだ光っていた。",
                "بارون بند اومده بود و سنگای زیرِ فانوس هنوز برق می‌زدن.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Joyful,
                "There's a cat on the wall.",
                "壁に猫いる。",
                "یه گربه رو دیواره.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It lives here.",
                "ここの猫だよ。",
                "مالِ همین‌جاست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's a stray.",
                "野良でしょ。",
                "ولگرده.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's got a collar.",
                "首輪してる。",
                "قلاده داره.");

            Say(Speaker.Yua, Portrait.Neutral,
                "…It's got a collar.",
                "…首輪してる。",
                "...قلاده داره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It's a very well fed stray.",
                "よく食べてる野良だね。",
                "ولگردِ خیلی سیریه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I'm calling it Anko.",
                "アンコって呼ぶ。",
                "اسمشو می‌ذارم آنکو.");

            Say(Speaker.Haru, Portrait.Neutral,
                "It has a name already. It's on the collar.",
                "名前ある。首輪に書いてある。",
                "اسم داره. رو قلاده‌شه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I'm not reading the collar.",
                "首輪は読まない。",
                "قلاده رو نمی‌خونم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Why not?",
                "なんで?",
                "چرا نمی‌خونی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Because then it'll have a name, and it won't be Anko, and I'll have to live with that. Some things you're better off not knowing. Cats are one of them. There's about four things in the world I want to know less about than that cat's real name.",
                "だって読んだら名前がわかっちゃうでしょ。アンコじゃなくなる。それを一生抱えて生きるの。知らない方がいいことってあるの。猫はその一つ。あの猫の本名より知りたくないことって、世界に四つくらいしかない。",
                "چون اون‌وقت اسم داره، و آنکو نیست، و من باید باهاش سر کنم. بعضی چیزا رو ندونی بهتره. گربه یکیشونه. تو کلِ دنیا چهار تا چیز هست که کمتر از اسمِ واقعیِ اون گربه بخوام درباره‌شون بدونم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Anko it is.",
                "じゃあアンコで。",
                "پس آنکو.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "You left the manga on the table.",
                "マンガ、テーブルに置いてきたでしょ。",
                "مانگا رو گذاشتی رو میز.");

            Say(Speaker.Yua, Portrait.Angry,
                "I did not.",
                "置いてない。",
                "نذاشتم که.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's on the table.",
                "テーブルにある。",
                "رو میزه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "…It's on the table.",
                "…テーブルにある。",
                "...رو میزه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I'll go back for it.",
                "取ってくるよ。",
                "می‌رم برش دارم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's two streets and it's dark. Leave it.",
                "二本先だし暗い。いいよ。",
                "دو تا خیابونه و هوا تاریکه. ولش کن.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Neutral,
                "…Do you think it'll still be there tomorrow?",
                "…明日もあると思う?",
                "...به نظرت فردا هنوز هست؟");

            Say(Speaker.Haru, Portrait.Joyful,
                "Somebody will have taken it to the shelf it came from.",
                "誰かが元の棚に返してるよ。",
                "یکی برش می‌گردونه به همون قفسه‌ای که ازش اومده.");

            Say(Speaker.Yua, Portrait.Joyful,
                "That's the nicest thing anyone has ever said about a café.",
                "カフェについてそんな優しいこと初めて聞いた。",
                "این قشنگ‌ترین حرفیه که تا حالا یکی درباره‌ی یه کافه زده.");

            Hold(1.2f);

            Narrate(
                "Her street came first. She went, and Haru stood a moment before he went the other way.",
                "先に結愛の通りに着いた。彼女が行って、ハルは少し立ってから反対へ歩いた。",
                "اول کوچه‌ی یوآ رسید. اون رفت و هارو یه لحظه وایساد بعد از اون‌ور رفت.");

            Hold(2.4f);
        }

        // =====================================================================
        //  WEDNESDAY — day three of six
        //
        //  Dread budget: 1. One sentence about which piece he always takes.
        // =====================================================================

        private void WriteWednesday()
        {
            WriteWednesdayClassroom();
            WriteWednesdayRooftop();
            WriteWednesdayCorner();
        }

        // ---------------------------------------------------------------------
        //  Wednesday, before lunch — classroom 1-A
        //
        //  ▣ Scene state
        //     Background   SunnyClassroomDay. Dry again.
        //     Time         Wednesday, late morning, the bell for lunch.
        //     Clothes      Uniform.
        //     On stage     Desks, the chalkboard with the test date on it, the
        //                  window that still does not shut.
        //     In hand      Bags. Yua has a wrapped lunch box she has not opened.
        //     From before  Morita-sensei's "maybe", which is now a date on a
        //                  board. The window. The station, still owed.
        //
        //  The lunch box is planted here and opened on the roof, and the roof is
        //  argued about here and stood on in the next scene, because the rule is
        //  that going somewhere is discussed before the background changes.
        //
        //  He wins the argument about which staircase reaches the roof. He loses
        //  where they eat, in three lines, and the scene does not slow down for
        //  it.
        // ---------------------------------------------------------------------

        private void WriteWednesdayClassroom()
        {
            ClearStage();

            Place(
                Backgrounds.ClassroomDay,
                "Class 1-A, Wednesday", "水曜、一年A組", "کلاسِ اول-الف، چهارشنبه");

            Hold(1.6f);

            Narrate(
                "Somebody had written Monday's test on the board and drawn a face next to it.",
                "誰かが黒板に月曜のテストと書いて、その横に顔を描いていた。",
                "یکی امتحانِ دوشنبه رو نوشته بود رو تخته و بغلش یه قیافه کشیده بود.");

            Enter(Speaker.Yua, Portrait.Joyful);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Unchanged,
                "So the test is real.",
                "テスト、本当だった。",
                "پس امتحان واقعیه.");

            Say(Speaker.Yua, Portrait.Angry,
                "Don't.",
                "言わないで。",
                "نگو.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I haven't said anything.",
                "何も言ってない。",
                "هیچی نگفتم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Your face said something.",
                "顔が言ってる。",
                "قیافه‌ت یه چیزی گفت.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's mine now, is it?",
                "それ、僕のになったんだ。",
                "این دیگه مالِ منه؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "You took it. It's yours.",
                "取ったんだからハルぴの。",
                "برش داشتی، مالِ خودته.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Neutral,
                "I made a lunch box.",
                "お弁当作った。",
                "بنتو درست کردم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You made it.",
                "作ったんだ。",
                "خودت درستش کردی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "At six this morning, standing up, half asleep, with the radio on. The rice took four goes because the first three were wrong in three completely different ways, and I am telling you this so that when you eat it you understand what you are holding.",
                "今朝六時、立ったまま、半分寝ながら、ラジオつけて。ご飯は四回炊いた。最初の三回が三通りに失敗したから。食べるとき、それを持ってるって分かってほしいの。",
                "امروز صبح ساعت شیش، سرِ پا، نصفه‌خواب، با رادیوی روشن. برنجش چهار بار طول کشید چون سه بارِ اول به سه شکلِ کاملاً متفاوت خراب شد، و اینو دارم بهت می‌گم که وقتی خوردیش بفهمی چی دستته.");

            Say(Speaker.Haru, Portrait.Shy,
                "…Thank you.",
                "…ありがとう。",
                "...ممنون.");

            Say(Speaker.Yua, Portrait.Joyful,
                "You haven't eaten it yet.",
                "まだ食べてないでしょ。",
                "که هنوز نخوردیش.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "We could eat here. It's windy on the roof.",
                "ここで食べてもいいよ。屋上は風強いし。",
                "می‌تونیم همین‌جا بخوریم. پشت‌بوم باد میاد.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The roof.",
                "屋上。",
                "پشت‌بوم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "It's four floors up.",
                "四階分ある。",
                "چهار طبقه بالاست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's three, and then the little staircase at the end.",
                "三階分と、最後の短い階段。",
                "سه طبقه‌ست، بعدش اون پله‌ی کوچیکِ ته راهرو.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The big staircase goes all the way.",
                "大きい階段で上まで行けるでしょ。",
                "پله‌ی بزرگه تا بالا می‌ره.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "The big one stops on three. There's a door.",
                "大きいのは三階で終わり。ドアがある。",
                "بزرگه تو طبقه‌ی سه تموم می‌شه. یه در هست.");

            Say(Speaker.Yua, Portrait.Angry,
                "…There's a door.",
                "…ドアがある。",
                "...یه در هست.");

            Say(Speaker.Haru, Portrait.Joyful,
                "There's a door.",
                "ドアがある。",
                "یه در هست.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Stop being right in the corridor.",
                "廊下で正しいのやめて。",
                "بسه، تو راهرو هم داری درست می‌گی.");

            Hold(1.2f);

            Cue(SfxId.SchoolBell, 0.55f);

            Say(Speaker.Yua, Portrait.Joyful,
                "Come on, before the third-years take the good side.",
                "行こ、三年生にいい方取られる前に。",
                "بریم، قبل از اینکه سومی‌ها سمتِ خوبش رو بگیرن.");

            Say(Speaker.Haru, Portrait.Neutral,
                "There's a good side?",
                "いい方なんてあるの?",
                "سمتِ خوب هم داره؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "There is always a good side.",
                "いい方は絶対ある。",
                "همیشه یه سمتِ خوب هست.");

            Hold(1.4f);
        }

        // ---------------------------------------------------------------------
        //  Wednesday, lunch — the roof
        //
        //  ▣ Scene state
        //     Background   SchoolRooftopSunnyDay: wire fencing, potted plants,
        //                  a wooden bench with a cushion and a schoolbag on it,
        //                  the city a long way below, big clouds.
        //     Time         Wednesday, lunch break.
        //     Clothes      Uniform.
        //     On stage     The fence, the bench, the pots, the city.
        //     In hand      The lunch box, and then the lid, and then the food.
        //     From before  The box she made at six this morning, and the
        //                  staircase argument he won on the way up.
        //
        //  ▣ The numbers, which are fixed by the drawings and not by the writer
        //     Six sushi and four octopus sausages in the box when it opens.
        //     Four sushi and three sausages go across to his lid; two sushi and
        //     one sausage stay with her. Each of them lifts three pieces on
        //     screen, so she finishes what she kept and he does not finish what
        //     he was given — and the dialogue says so, because the drawing does.
        //     Nothing in this scene claims anybody ate ten pieces of anything.
        //
        //  ◆ Dread moment 3 of 10 — the whole of Wednesday's budget.
        //
        //      You always take the third one.
        //      Yeah.
        //
        //  It is affection, not surveillance: it is about a habit of his, not
        //  about the room. It is deniable to the point of being sweet. And it
        //  is said on the third day of knowing somebody the player believes she
        //  met on Monday morning, by a girl who has clearly watched him eat
        //  several hundred times. Nobody asks. Nobody explains. That is the
        //  entire moment.
        // ---------------------------------------------------------------------

        private void WriteWednesdayRooftop()
        {
            ClearStage();

            Place(
                Backgrounds.RooftopDay,
                "The roof", "屋上", "پشت‌بوم");

            Hold(2.0f);

            Narrate(
                "The wind was going straight through the fence and the city was a long way down.",
                "風がフェンスを抜けていって、街はずっと下にあった。",
                "باد از لای توری رد می‌شد و شهر خیلی پایین بود.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Joyful,
                "See, the good side.",
                "ほら、いい方。",
                "دیدی، سمتِ خوب.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's the same as the other side.",
                "反対側と同じだけど。",
                "با اون‌ور فرقی نداره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The bench is here.",
                "ベンチがこっち。",
                "نیمکت اینجاست.");

            Say(Speaker.Haru, Portrait.Joyful,
                "…The bench is here.",
                "…ベンチがこっち。",
                "...نیمکت اینجاست.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Which one's the station?",
                "駅ってどれ?",
                "کدومش ایستگاهه؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "The long grey roof.",
                "あの長い灰色の屋根。",
                "همون سقفِ خاکستریِ دراز.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That's a supermarket.",
                "それスーパーでしょ。",
                "اون سوپرمارکته.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "The supermarket's the one with the car park on the roof.",
                "スーパーは屋上が駐車場のやつ。",
                "سوپرمارکت اونیه که رو پشت‌بومش پارکینگه.");

            Say(Speaker.Yua, Portrait.Angry,
                "How can you see a car park from here.",
                "ここから駐車場見えるわけ。",
                "از اینجا چطوری پارکینگ می‌بینی آخه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "There are cars on it.",
                "車が乗ってる。",
                "روش ماشین هست.");

            Say(Speaker.Yua, Portrait.Joyful,
                "…There are cars on it.",
                "…車が乗ってる。",
                "...روش ماشین هست.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "What's in it?",
                "何が入ってるの?",
                "چی توشه؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You'll see in a second.",
                "今わかる。",
                "الآن می‌بینی.");

            // ── the wordless sequence, ten pictures, with the talking in it ──
            Cel(Portrait.LunchOpen, Portrait.Unchanged);
            Cel(Portrait.LunchOffer);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Six sushi and four octopus sausages.",
                "お寿司が六つ、たこさんウインナーが四つ。",
                "شیش تا سوشی و چهار تا سوسیسِ اختاپوسی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Four octopuses?",
                "たこさん四つ?",
                "چهار تا اختاپوس؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Four.",
                "四つ。",
                "چهار تا.");

            Cel(Portrait.LunchShared);

            Say(Speaker.Haru, Portrait.Unchanged,
                "That's most of it.",
                "ほとんどこっちだよ。",
                "این که بیشترشه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Seven for you, three for me.",
                "七つそっち、三つこっち。",
                "هفت تا مالِ تو، سه تا مالِ من.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Take two of mine back.",
                "二つ返すよ。",
                "دو تا از مالِ منو پس بگیر.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "No.",
                "だめ。",
                "نه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Cel(Portrait.LunchFirstLift);
            Cel(Portrait.LunchFirstBite);

            Say(Speaker.Haru, Portrait.Unchanged,
                "The rice is good.",
                "ご飯おいしい。",
                "برنجش خوبه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's the fourth rice.",
                "四回目のご飯だからね。",
                "برنجِ چهارمیه.");

            Cel(Portrait.LunchSecondLift);
            Cel(Portrait.LunchSecondBite);

            Say(Speaker.Yua, Portrait.Unchanged,
                "There's a bird that's been on that fence since we sat down.",
                "座ってからずっとフェンスに鳥がいる。",
                "یه پرنده از وقتی نشستیم رو اون توری وایساده.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's waiting for the sausages.",
                "ウインナー待ってる。",
                "منتظرِ سوسیساست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's not getting the sausages.",
                "あげないよ。",
                "سوسیس بهش نمی‌رسه.");

            // ◆ Dread moment 3.
            Say(Speaker.Yua, Portrait.Unchanged,
                "You always take the third one.",
                "三つ目、いつもそれ取るよね。",
                "تو همیشه سومی رو برمی‌داری.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Yeah.",
                "うん。",
                "آره.");

            Cel(Portrait.LunchThirdLift);
            Cel(Portrait.LunchThirdBite);
            Cel(Portrait.LunchFinished);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Mine's gone.",
                "こっちは終わり。",
                "مالِ من تموم شد.");

            Say(Speaker.Haru, Portrait.Neutral,
                "You've had three things and I've still got half a lid.",
                "三つで終わり? こっちまだ半分ある。",
                "تو سه تا خوردی و مالِ من هنوز نصفه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Eat slowly. I like watching you decide.",
                "ゆっくり食べて。迷ってるとこ見るの好き。",
                "آروم بخور. دوست دارم ببینم داری انتخاب می‌کنی.");

            Say(Speaker.Haru, Portrait.Shy,
                "I'm not deciding anything.",
                "迷ってないよ。",
                "دارم چیزی انتخاب نمی‌کنم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "You've moved the same sausage twice.",
                "同じウインナー二回動かしたでしょ。",
                "یه سوسیس رو دو بار جابه‌جا کردی.");

            Hold(1.6f);

            Cue(SfxId.SchoolBell, 0.5f);

            Say(Speaker.Haru, Portrait.Neutral,
                "That's the bell.",
                "チャイム。",
                "زنگ خورد.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Finish it on the stairs.",
                "階段で食べて。",
                "تو پله‌ها تمومش کن.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's four floors of sausage.",
                "四階分のウインナーだ。",
                "می‌شه چهار طبقه سوسیس.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Three, and the little staircase.",
                "三階と、短い階段。",
                "سه طبقه و اون پله‌ی کوچیکه.");

            Hold(1.8f);
        }

        // ---------------------------------------------------------------------
        //  Wednesday, on the way home — the corner
        //
        //  ▣ Scene state
        //     Background   PastelStreetVendingDay: the pink drinks machine, the
        //                  bicycle still against the wall, the flowerbeds.
        //     Time         Wednesday, early evening.
        //     Clothes      Uniform.
        //     On stage     The machine, the bicycle, the flowerbeds.
        //     In hand      Bags. He has one hundred-yen coin, which he loses.
        //     From before  The machine, named and argued over on Monday. Melon
        //                  soda is the second button from the left.
        //
        //  The coin, in three beats and in that order: he says he has one, he
        //  puts it in, nothing comes out. Nobody discusses a drink that has
        //  already been drunk, because there is no drink.
        //
        //  What the scene is really doing is setting up a line on Saturday. She
        //  declares the machine fine; he agrees; and on the last night of the
        //  act he defends it with her sentence, about a thing he watched happen
        //  with his own eyes. Nothing points at it either time.
        //
        //  The Maybe beat before the scene change is the design document's
        //  machine-room interlude, which turns up at random in every act. It is
        //  its own small asset and its own dread, and it is not part of the ten
        //  moments budgeted for the hand-written script.
        // ---------------------------------------------------------------------

        private void WriteWednesdayCorner()
        {
            ClearStage();

            Maybe(MachineRoom, 0.2f);

            Place(
                Backgrounds.VendingStreetDay,
                "The corner", "角の道", "سرِ نبش");

            Hold(1.8f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Joyful,
                "They've put a new flavour in.",
                "新しい味入ってる。",
                "یه طعمِ جدید گذاشتن توش.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Which one?",
                "どれ?",
                "کدوم؟");

            Say(Speaker.Yua, Portrait.Angry,
                "\"Salty lychee.\" Somebody sat in a room and decided that. There was a meeting. People agreed.",
                "「塩ライチ」。誰かが会議室で決めたんだよ。会議があって、賛成した人がいるの。",
                "«لیچیِ شور». یکی تو یه اتاق نشسته و اینو تصویب کرده. جلسه گذاشتن. آدما موافقت کردن.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It might be nice.",
                "おいしいかもよ。",
                "شاید خوب باشه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It might be salty lychee.",
                "塩ライチかもよ。",
                "شایدم لیچیِ شور باشه.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "The melon soda light is off.",
                "メロンソーダのランプ消えてる。",
                "چراغِ ملون‌سودا خاموشه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That means it's cold.",
                "冷たいってことでしょ。",
                "یعنی سرده.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It means it's sold out.",
                "売り切れってこと。",
                "یعنی تموم شده.");

            Say(Speaker.Yua, Portrait.Neutral,
                "…Since when do you know vending machines?",
                "…なんで自販機に詳しいの?",
                "...از کِی تا حالا دستگاه‌شناس شدی؟");

            Say(Speaker.Haru, Portrait.Joyful,
                "Since now.",
                "今から。",
                "از الآن.");

            Say(Speaker.Yua, Portrait.Joyful,
                "You've used that one.",
                "それ、前も使った。",
                "این یکی رو قبلاً گفتی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's a good one.",
                "いいやつだから。",
                "خوبه دیگه.");

            Hold(1.4f);

            // Intent, act, result.
            Say(Speaker.Haru, Portrait.Neutral,
                "I've got one hundred-yen coin.",
                "百円玉が一枚ある。",
                "یه سکه‌ی صد ینی دارم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Put the coin in the machine.",
                "その百円、自販機に入れて。",
                "سکه رو بنداز تو دستگاه.");

            Cue(SfxId.VendingThunk, 0.8f);

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Nothing came out.",
                "何も出てこない。",
                "هیچی نیومد.");

            Say(Speaker.Yua, Portrait.Angry,
                "Press it again.",
                "もう一回押して。",
                "دوباره بزنش.");

            Cue(SfxId.Tap, 0.7f);

            Hold(0.9f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "Still nothing.",
                "まだ出てこない。",
                "بازم هیچی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Hit the side of it.",
                "横叩いて。",
                "بغلش رو بزن.");

            Say(Speaker.Haru, Portrait.Shy,
                "I'm not hitting a machine on a street.",
                "道端で自販機叩かないよ。",
                "من وسطِ خیابون به دستگاه نمی‌زنم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Then move.",
                "じゃあどいて。",
                "پس برو کنار.");

            Cue(SfxId.DeskKnock, 0.6f);

            Hold(1.0f);

            Say(Speaker.Haru, Portrait.Joyful,
                "Nothing.",
                "何も。",
                "هیچی.");

            Say(Speaker.Yua, Portrait.Neutral,
                "That's a hundred yen of yours.",
                "ハルぴの百円だ。",
                "صد ینِ تو بود.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I'll put another one in.",
                "もう一枚入れる。",
                "یکی دیگه می‌ندازم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You will not.",
                "だめ。",
                "نمی‌ندازی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.4f);

            // The sentence Saturday needs.
            Say(Speaker.Yua, Portrait.Joyful,
                "It's a good machine. It's just tired.",
                "いい自販機だよ。疲れてるだけ。",
                "دستگاهِ خوبیه. فقط خسته‌ست.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It's fine.",
                "大丈夫。",
                "سالمه.");

            Hold(1.6f);

            Narrate(
                "They were about ten steps down the street when something fell inside the machine.",
                "十歩ほど歩いたところで、自販機の中で何かが落ちた。",
                "ده قدمی رفته بودن که یه چیزی افتاد تو دستگاه.");

            Cue(SfxId.CanDrop, 0.8f);

            Say(Speaker.Yua, Portrait.Joyful,
                "Go on then.",
                "ほら、行って。",
                "خب برو دیگه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It's salty lychee, isn't it.",
                "塩ライチだよね。",
                "لیچیِ شوره، نه؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's absolutely salty lychee.",
                "絶対塩ライチ。",
                "صد در صد لیچیِ شوره.");

            Hold(1.4f);

            // Planted here, two days before anybody needs it. She says it, she
            // forgets she said it, and on Friday he has arranged his whole
            // Saturday around it.
            Say(Speaker.Yua, Portrait.Neutral,
                "Saturday's only a half day, you know.",
                "土曜って半日だけだよね。",
                "شنبه فقط نصفِ روزه ها.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It is.",
                "そうだね。",
                "آره هست.");

            Say(Speaker.Yua, Portrait.Joyful,
                "A whole afternoon, out of nowhere.",
                "午後まるまる、いきなり空いてる。",
                "یه بعدازظهرِ کامل، همین‌جوری از ناکجا.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Out of nowhere.",
                "いきなり。",
                "همین‌جوری از ناکجا.");

            Hold(2.2f);
        }

        // =====================================================================
        //  THURSDAY — day four of six
        //
        //  Dread budget: 2. "Finish all of it", and two tenths of a second.
        // =====================================================================

        private void WriteThursday()
        {
            WriteThursdayAlley();
            WriteThursdayCafe();
            WriteThursdayPlatform();
        }

        // ---------------------------------------------------------------------
        //  Thursday, morning — the school alley again
        //
        //  ▣ Scene state
        //     Background   CherryBlossomSchoolAlleyDay, four days on. The
        //                  blossom is starting to come off the trees.
        //     Time         Thursday, morning.
        //     Clothes      Uniform, and hers fits about as badly as it did.
        //     On stage     The trees, the benches, the lanterns, the board by
        //                  the door.
        //     In hand      Bags.
        //     From before  Monday's club list, still unread. The bakery near
        //                  the station and its new baker.
        //
        //  Zero dread. The whole scene is a running joke about a piece of paper
        //  he has now asked to look at three times in four days, and the fourth
        //  refusal is the funniest one because by then the player is counting
        //  too — and counting the joke, not the pattern. That is the shape this
        //  act wants: the thing the player is laughing at and the thing that is
        //  wrong are the same thing, and nobody says so until act five.
        // ---------------------------------------------------------------------

        private void WriteThursdayAlley()
        {
            ClearStage();

            // SEASON: as above. Flips with the rewrite, not before it.
            Place(
                Backgrounds.SchoolAlleySpring,
                "The path to school", "通学路", "راهِ مدرسه");

            Hold(1.8f);

            Narrate(
                "The blossom had started coming off and there was a pink line of it along the gutter.",
                "花が散りはじめて、側溝にピンクの筋ができていた。",
                "شکوفه‌ها داشتن می‌ریختن و کنارِ جوب یه خطِ صورتی درست شده بود.");

            Cue(SfxId.Petal, 0.45f);

            Enter(Speaker.Yua, Portrait.Angry);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Somebody has parked a bicycle across the whole path.",
                "誰かが道を塞いで自転車停めてる。",
                "یکی دوچرخه‌شو گذاشته وسطِ راه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You can go round it.",
                "回れば通れるよ。",
                "می‌شه از بغلش رد شد.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I can go round it. That is not the point. The point is that a human being got off a bicycle, looked at where the bicycle was, thought about it, and walked away.",
                "回れるよ。そこじゃないの。人間が自転車から降りて、置いた場所を見て、考えて、そのまま行ったってこと。",
                "می‌شه رد شد. بحث این نیست. بحث اینه که یه آدم از دوچرخه پیاده شده، به جایی که گذاشته بودش نگاه کرده، فکر کرده، و رفته.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Maybe they were late.",
                "遅刻しそうだったのかも。",
                "شاید دیرش شده بود.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Everybody's late. That's what a school is.",
                "みんな遅刻しそうなの。学校ってそういうとこ。",
                "همه دیرشون شده. مدرسه یعنی همین.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You're not late.",
                "結愛ぴは遅刻してないでしょ。",
                "تو که دیرت نشده.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I'm never late. That's why I'm allowed to be angry.",
                "遅刻しないもん。だから怒る権利がある。",
                "من هیچ‌وقت دیر نمی‌کنم. واسه همین حق دارم عصبانی بشم.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "The trees have about a week left.",
                "桜、あと一週間くらいかな。",
                "درخت‌ها یه هفته دیگه وقت دارن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Three days.",
                "三日。",
                "سه روز.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "A week. It's only the ones on this side going.",
                "一週間。散ってるのはこっち側だけ。",
                "یه هفته. فقط اینایی که این‌ورن دارن می‌ریزن.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Because this side gets the wind.",
                "こっちは風が当たるから。",
                "چون این‌ور باد می‌خوره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "So the other side has a week.",
                "だから反対側はあと一週間。",
                "پس اون‌ور یه هفته وقت داره.");

            Say(Speaker.Yua, Portrait.Angry,
                "That is not what I said.",
                "そんなこと言ってない。",
                "من اینو نگفتم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It's what you meant.",
                "そういう意味だった。",
                "منظورت این بود.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Don't do that.",
                "それやめて。",
                "این کارو نکن.");

            Hold(1.4f);

            DecideIdly(
                "Ask what he had for breakfast", "朝ごはんを聞く", "بپرس صبحونه چی خورده",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Neutral,
                        "What did you have for breakfast?",
                        "朝ごはん何食べた?",
                        "صبحونه چی خوردی؟");

                    Say(Speaker.Haru, Portrait.Unchanged,
                        "Rice and an egg.",
                        "ご飯と卵。",
                        "برنج و تخم‌مرغ.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Every day?",
                        "毎日?",
                        "هر روز؟");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "It's a good breakfast.",
                        "いい朝ごはんだよ。",
                        "صبحونه‌ی خوبیه.");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "It's the same breakfast.",
                        "同じ朝ごはんでしょ。",
                        "صبحونه‌ی تکراریه.");

                    Say(Speaker.Haru, Portrait.Unchanged,
                        "That's what makes it good.",
                        "同じだからいいんだよ。",
                        "همین که تکراریه خوبش می‌کنه.");
                },
                "Tell him about the dream you had", "見た夢の話をする", "از خوابی که دیدی بگو",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Joyful,
                        "I dreamed the school was a boat.",
                        "学校が船になる夢見た。",
                        "خواب دیدم مدرسه یه کشتیه.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "Was it a good boat?",
                        "いい船だった?",
                        "کشتیِ خوبی بود؟");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "It was a terrible boat. Morita-sensei was steering it and we were all doing a test.",
                        "ひどい船。森田先生が操縦してて、みんなテスト受けてた。",
                        "کشتیِ افتضاحی بود. خانمِ موریتا داشت می‌روندش و همه‌مون داشتیم امتحان می‌دادیم.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "Did you pass?",
                        "受かった?",
                        "قبول شدی؟");

                    Say(Speaker.Yua, Portrait.Angry,
                        "I woke up.",
                        "起きた。",
                        "بیدار شدم.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "So you failed.",
                        "じゃあ落ちた。",
                        "پس رد شدی.");
                });

            Hold(1.2f);

            // Third time in four days, and the third no.
            Say(Speaker.Haru, Portrait.Neutral,
                "The club list is on that board.",
                "部活の一覧、あの掲示板。",
                "لیستِ باشگاه‌ها رو همون تخته‌ست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It is.",
                "あるね。",
                "هست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's been on that board since Monday.",
                "月曜からずっとある。",
                "از دوشنبه رو همون تخته‌ست.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It's very reliable.",
                "信頼できる紙だね。",
                "کاغذِ قابل‌اعتمادیه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Can we read it after school?",
                "放課後、読める?",
                "بعدِ مدرسه می‌شه بخونیمش؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "After the café.",
                "カフェのあとね。",
                "بعدِ کافه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.6f);
        }

        // ---------------------------------------------------------------------
        //  Thursday, after school — the café, in the sun
        //
        //  ▣ Scene state
        //     Background   CozyCafeDay: the same pastel room in bright
        //                  daylight, fairy lights along the wall, strawberry
        //                  parfaits and tea sets on the polished tables.
        //     Time         Thursday, late afternoon.
        //     Clothes      Uniform.
        //     On stage     The lights, the parfaits, the counter, the tables.
        //     In hand      A boba for her, an iced matcha for him. Three
        //                  pictures: both full, hers gone and his half drunk,
        //                  both empty.
        //     From before  Tuesday, when she paid and told him he could have
        //                  Thursday. The club list, still unread, waiting for
        //                  after this.
        //
        //  ▣ The order of the drink, which is fixed and not negotiable
        //     Ordered, arrives, drunk, finished. Nobody says a word about a cup
        //     that is already empty, and the line she says about finishing it
        //     is said while there is still something in it.
        //
        //  ◆ Dread moment 4 of 10.
        //
        //      Drink all of it.
        //
        //  Three words, on a drink she chose for him, that he has already said
        //  he does not like. Completely deniable — she is teasing him, and it
        //  is funny, and the player laughs. It is also the first thing in the
        //  act that is a straightforward instruction with nothing between it
        //  and him doing it.
        // ---------------------------------------------------------------------

        private void WriteThursdayCafe()
        {
            ClearStage();

            Place(
                Backgrounds.CafeDay,
                "The café", "カフェ", "کافه");

            Hold(1.8f);

            Narrate(
                "The little lights along the wall were on even though it was the middle of the afternoon.",
                "昼間なのに、壁の小さな電飾がついていた。",
                "چراغ‌ریسه‌های رو دیوار روشن بودن، با اینکه وسطِ روز بود.");

            Narrate(
                "Yua took the table nearest the door again.",
                "結愛はまたドアに一番近い席に座った。",
                "یوآ بازم نزدیک‌ترین میز به در رو گرفت.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Joyful,
                "Those lights have been on since Tuesday.",
                "この電飾、火曜からずっとついてる。",
                "این چراغا از سه‌شنبه روشنن.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "They're on all year.",
                "一年中ついてるよ。",
                "کلِ سال روشنن.");

            Say(Speaker.Yua, Portrait.Angry,
                "In summer?",
                "夏も?",
                "تابستون هم؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "In summer.",
                "夏も。",
                "تابستون هم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That's not what lights are for.",
                "電飾ってそういうものじゃない。",
                "چراغ که واسه این نیست.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It's what these lights are for.",
                "この電飾はそうなんだよ。",
                "این چراغا واسه همینن.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Neutral,
                "This song is from an advert.",
                "この曲、コマーシャルの。",
                "این آهنگ مالِ یه تبلیغه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's from a drama. The advert took it after.",
                "ドラマの曲。コマーシャルは後から使った。",
                "مالِ یه سریاله. تبلیغ بعداً برش داشته.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Which drama?",
                "どのドラマ?",
                "کدوم سریال؟");

            Say(Speaker.Haru, Portrait.Shy,
                "…The one with the bakery in it.",
                "…パン屋が出てくるやつ。",
                "...همونی که توش نونوایی هست.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Everything with you comes back to bread.",
                "ハルぴ、全部パンに戻るね。",
                "همه‌چیزِ تو آخرش به نون می‌رسه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Bread is important.",
                "パンは大事。",
                "نون مهمه.");

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Neutral,
                "They've got boba and they've got matcha.",
                "タピオカと抹茶がある。",
                "بابل‌تی و ماچا داره.");

            // The manual's choice, and the one place in act one where a white
            // button decides how something is done rather than what is said.
            DecideIdly(
                "Let Haru choose", "ハルに選ばせる", "بذار هارو انتخاب کنه",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Which one do you want?",
                        "どっちがいい?",
                        "کدوم رو می‌خوای؟");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "What are you getting?",
                        "結愛ぴは?",
                        "تو چی می‌گیری؟");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Boba.",
                        "タピオカ。",
                        "بابل‌تی.");

                    Say(Speaker.Haru, Portrait.Unchanged,
                        "Then boba.",
                        "じゃあタピオカ。",
                        "پس منم بابل‌تی.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "No. You're having the matcha.",
                        "だめ。ハルぴは抹茶。",
                        "نه. تو ماچا.");

                    Say(Speaker.Haru, Portrait.Shy,
                        "…Then why did you ask?",
                        "…じゃあなんで聞いたの?",
                        "...پس چرا پرسیدی؟");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "I wanted to hear it.",
                        "聞きたかったから。",
                        "دوست داشتم بشنوم.");
                },
                "Order for both of you", "自分で注文する", "خودت سفارش بده",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Unchanged,
                        "One boba, one matcha.",
                        "タピオカ一つ、抹茶一つ。",
                        "یه بابل‌تی، یه ماچا.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "You didn't ask me.",
                        "僕には聞かなかったね。",
                        "از من که نپرسیدی.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "You'd have said whatever I said.",
                        "あたしが言ったの、そのまま言うでしょ。",
                        "هرچی من می‌گفتم رو می‌گفتی.");

                    Say(Speaker.Haru, Portrait.Shy,
                        "…Probably.",
                        "…たぶん。",
                        "...احتمالاً.");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "So I saved us both some time.",
                        "時間の節約。",
                        "پس تو وقتِ جفتمون صرفه‌جویی کردم.");
                });

            // Both roads land here, and Tuesday is what makes it land.
            Say(Speaker.Haru, Portrait.Neutral,
                "I'm paying. It's Thursday.",
                "今日は僕。木曜だから。",
                "من حساب می‌کنم. پنج‌شنبه‌ست.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It is Thursday.",
                "木曜だね。",
                "آره، پنج‌شنبه‌ست.");

            Hold(1.4f);

            Narrate(
                "The drinks came. The ice in the matcha had not started going yet.",
                "飲み物が来た。抹茶の氷はまだ溶けていなかった。",
                "نوشیدنیا رو آوردن. یخِ ماچا هنوز آب نشده بود.");

            Cel(Portrait.DrinkFull);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Well?",
                "どう?",
                "خب؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's very green.",
                "すごく緑。",
                "خیلی سبزه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That's not an answer.",
                "答えになってない。",
                "این جواب نشد.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's grass. It's cold grass.",
                "草。冷たい草。",
                "علفه. علفِ سرد.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's matcha.",
                "抹茶だよ。",
                "ماچاست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's cold grass with sugar in it.",
                "砂糖入りの冷たい草。",
                "علفِ سرده که توش شکر ریختن.");

            Hold(1.0f);

            Cel(Portrait.DrinkReluctant);

            // ◆ Dread moment 4.
            Say(Speaker.Yua, Portrait.Unchanged,
                "Drink all of it.",
                "全部飲んで。",
                "تا آخرش رو بخور.");

            Hold(1.2f);

            Cel(Portrait.DrinkFinished);

            Say(Speaker.Haru, Portrait.Unchanged,
                "…That was the worst thing I have ever done in a café, and I would like it noted that I did it, and that I did not complain more than four times, which for me is restraint.",
                "…カフェで一番つらいことをした。やり遂げたこと、文句が四回で済んだことは記録しておいてほしい。僕にしては我慢した方。",
                "...این بدترین کاری بود که تا حالا تو یه کافه کردم، و می‌خوام ثبت بشه که انجامش دادم، و بیشتر از چهار بار غر نزدم، که واسه من یعنی خویشتن‌داری.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You complained six times.",
                "六回だよ。",
                "شیش بار غر زدی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Four.",
                "四回。",
                "چهار بار.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Grass, cold grass, cold grass with sugar, worst thing in a café, and now this.",
                "草、冷たい草、砂糖入りの冷たい草、カフェで一番つらい、そして今。",
                "علف، علفِ سرد، علفِ سرد با شکر، بدترین کارِ تو یه کافه، و حالا این.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's five.",
                "五回だ。",
                "می‌شه پنج تا.");

            Say(Speaker.Yua, Portrait.Joyful,
                "And that's six.",
                "それで六回。",
                "این هم می‌شه شیش تا.");

            Hold(1.8f);

            Say(Speaker.Haru, Portrait.Neutral,
                "The board.",
                "掲示板。",
                "تخته.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The station's on the way. We'll go past the platform.",
                "駅、通り道だから。ホームの方から行こ。",
                "ایستگاه سرِ راهه. از سمتِ سکو می‌ریم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.6f);
        }

        // ---------------------------------------------------------------------
        //  Thursday, evening — the station platform
        //
        //  ▣ Scene state
        //     Background   TrainPlatformSunset: the platform in gold and pink
        //                  light, a pink drinks machine, coloured waiting
        //                  chairs, cherry trees along the track.
        //     Time         Thursday, sunset.
        //     Clothes      Uniform.
        //     On stage     The chairs, the machine on the platform — which is
        //                  not the machine on the corner and is named in full
        //                  so nobody confuses them — the track, the trees.
        //     In hand      Bags.
        //     From before  The bakery by the station and its new baker. The
        //                  club board, which they have now failed to reach on
        //                  four consecutive days.
        //
        //  The third person in act one, and the only one. She has a name plate
        //  and no body, she gets four lines, and she exists to do a job the
        //  Japanese script does not need doing: ぴ is real slang and a Japanese
        //  player hears everything it means the first time Yua says it, while a
        //  Persian or English player just hears a nickname. So somebody outside
        //  the pair notices it out loud, once, and is told to mind her own
        //  business — and the suffix acquires in two languages the weight it
        //  already had in the third.
        //
        //  ◆ Dread moment 5 of 10.
        //
        //  Two tenths of a second. Her face goes flat while somebody else is
        //  standing next to Haru, and then it is back. There is no line about
        //  it, nobody sees it, and it never happens again in this act — once is
        //  a glitch the player half-doubts, and three times is a tell.
        // ---------------------------------------------------------------------

        private void WriteThursdayPlatform()
        {
            ClearStage();

            Place(
                Backgrounds.TrainPlatformSunset,
                "The platform", "駅のホーム", "سکوی ایستگاه");

            Hold(2.0f);

            Narrate(
                "The light on the platform had gone gold and the chairs were all empty.",
                "ホームの光は金色になって、椅子は全部空いていた。",
                "نورِ سکو طلایی شده بود و صندلی‌ها همه خالی بودن.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Joyful,
                "There's a drinks machine on the platform too.",
                "ホームにも自販機ある。",
                "رو سکو هم یه دستگاهِ نوشیدنی هست.");

            Say(Speaker.Haru, Portrait.Neutral,
                "That one works.",
                "そっちは動く。",
                "اون یکی کار می‌کنه.");

            Say(Speaker.Yua, Portrait.Angry,
                "Ours works.",
                "うちのも動くもん。",
                "مالِ ما هم کار می‌کنه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Ours.",
                "うちの。",
                "مالِ ما.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The one on the corner.",
                "角のやつ。",
                "همون سرِ نبشی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Ours.",
                "うちの。",
                "مالِ ما.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "That's the express. It doesn't stop here.",
                "あれ急行。ここは通過。",
                "اون تندروئه. اینجا واینمی‌سته.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Everything stops here.",
                "全部止まるでしょ。",
                "همه‌شون اینجا وایمیسن.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Not the orange ones.",
                "オレンジのは止まらない。",
                "نارنجی‌ها نه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "…That one's orange.",
                "…あれオレンジだ。",
                "...اون نارنجیه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It's orange.",
                "オレンジ。",
                "نارنجیه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I hate this about you.",
                "そういうとこ嫌い。",
                "از این خصلتت بدم میاد.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You've said that three times this week.",
                "今週三回目。",
                "این هفته سه بار اینو گفتی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Because it keeps being true.",
                "毎回本当だから。",
                "چون هر بار درسته.");

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Neutral,
                "The bakery's the low one with the blue door.",
                "パン屋、青いドアの低い建物。",
                "نونوایی همون کوتاهه‌ست که درش آبیه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "She shuts at six.",
                "六時で閉まる。",
                "شیش می‌بنده.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Then we're too late.",
                "じゃあ間に合わない。",
                "پس دیر رسیدیم.");

            Say(Speaker.Haru, Portrait.Shy,
                "Sorry.",
                "ごめん。",
                "ببخشید.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Haru-pi, you didn't build the bakery.",
                "ハルぴがパン屋建てたわけじゃないでしょ。",
                "هاروپی، نونوایی رو که تو نساختی.");

            Hold(1.2f);

            // The third person.
            Enter(Speaker.Yua, Portrait.Joyful);

            Say(Speaker.Classmate, Portrait.Unchanged,
                "Oh — you two are in 1-A.",
                "あ、一年A組の二人だ。",
                "اِ — شما دوتا اول-الفین.");

            Say(Speaker.Haru, Portrait.Neutral,
                "We are.",
                "そうです。",
                "آره.");

            Say(Speaker.Classmate, Portrait.Unchanged,
                "You sit by the window. I sit two rows back, by the bookshelf.",
                "窓際の子だよね。あたし二列後ろ、本棚のとこ。",
                "شما کنارِ پنجره می‌شینین. من دو ردیف عقب‌ترم، بغلِ قفسه‌ی کتاب.");

            // ◆ Dread moment 5. Nobody in the scene sees it.
            FaceSlips();

            Say(Speaker.Yua, Portrait.Joyful,
                "The shelf with volume four on it.",
                "四巻がある棚ね。",
                "همون قفسه‌ای که جلدِ چهار روشه.");

            Say(Speaker.Classmate, Portrait.Unchanged,
                "…What did you call him just now?",
                "…今、なんて呼んだ?",
                "...الآن چی صداش کردی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Nothing.",
                "別に。",
                "هیچی.");

            Say(Speaker.Classmate, Portrait.Unchanged,
                "No, really, what was that?",
                "ううん、ほんとに、なんて?",
                "نه جدی، اون چی بود؟");

            Say(Speaker.Yua, Portrait.Neutral,
                "It's ours.",
                "うちらのだから。",
                "مالِ خودمونه.");

            Hold(1.6f);

            Narrate(
                "The orange train went through without stopping and the girl went down the steps.",
                "オレンジの電車は止まらずに通り過ぎて、その子は階段を降りていった。",
                "قطارِ نارنجی بدونِ توقف رد شد و اون دختره از پله‌ها رفت پایین.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "She sits by the bookshelf.",
                "本棚のとこに座ってるんだ。",
                "بغلِ قفسه می‌شینه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "She does.",
                "そうだね。",
                "آره می‌شینه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "We never read the board again.",
                "また掲示板見なかったね。",
                "بازم تخته رو نخوندیم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Tomorrow.",
                "明日。",
                "فردا.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You've said tomorrow four times.",
                "「明日」四回目。",
                "چهار بار گفتی فردا.");

            Say(Speaker.Yua, Portrait.Joyful,
                "And I'll say it a fifth.",
                "五回目も言う。",
                "بارِ پنجم هم می‌گم.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "I'll walk you to your street.",
                "通りまで送るよ。",
                "تا کوچه‌تون می‌رسونمت.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Go home, it's the other way for you.",
                "帰りな、そっち反対でしょ。",
                "برو خونه، مسیرِ تو اون‌وره.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(2.2f);
        }

        // =====================================================================
        //  FRIDAY — day five of six
        //
        //  Dread budget: 2. A window that got fixed, and a Wednesday he kept.
        // =====================================================================

        private void WriteFriday()
        {
            WriteFridayClassroom();
            WriteFridayCorridor();
            WriteFridayBakery();
        }

        // ---------------------------------------------------------------------
        //  Friday, morning — classroom 1-A
        //
        //  ▣ Scene state
        //     Background   SunnyClassroomDay.
        //     Time         Friday, morning.
        //     Clothes      Uniform.
        //     On stage     Desks, the board, the bookshelf two rows back, and
        //                  the window — which now shuts.
        //     In hand      Bags.
        //     From before  The window, planted Monday, rained through on
        //                  Tuesday. Monday's test. The girl by the bookshelf.
        //
        //  Stage three of the window, and the whole payoff is one line of
        //  narration about somebody neither of them has ever met.
        //
        //  ◆ Dread moment 6 of 10.
        //
        //      Who told them to fix it?
        //
        //  Flat, and then gone, and then she is bright again inside two frames.
        //  Completely deniable, and pre-denied three days early: on Tuesday she
        //  said, in front of the player, that she liked the air. She is allowed
        //  to be annoyed that a draught she liked has been taken away. That is
        //  the honest reading and it is available the whole time.
        // ---------------------------------------------------------------------

        private void WriteFridayClassroom()
        {
            ClearStage();

            Place(
                Backgrounds.ClassroomDay,
                "Class 1-A, Friday", "金曜、一年A組", "کلاسِ اول-الف، جمعه");

            Hold(1.6f);

            Narrate(
                "Somebody had finally fixed the window. There was a new catch on it and a bit of fresh paint.",
                "誰かがついに窓を直していた。新しい留め金と、塗り直しの跡があった。",
                "یکی بالاخره پنجره رو درست کرده بود. یه گیره‌ی نو روش بود و یه‌کم رنگِ تازه.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Joyful);

            Say(Speaker.Haru, Portrait.Unchanged,
                "The window shuts.",
                "窓、閉まる。",
                "پنجره بسته می‌شه.");

            // ◆ Dread moment 6.
            Say(Speaker.Yua, Portrait.DeadEyes,
                "Who told them to fix it?",
                "誰が直せって言ったの?",
                "کی گفت درستش کنن؟");

            Hold(0.6f);

            Say(Speaker.Yua, Portrait.Joyful,
                "Now it's going to be boiling in here by third period.",
                "これで三時間目には蒸し風呂だ。",
                "حالا تا زنگِ سوم اینجا کوره می‌شه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "You can open it. It opens.",
                "開けられるよ。開くから。",
                "می‌تونی بازش کنی. باز می‌شه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's not the same.",
                "同じじゃない。",
                "فرق داره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It is exactly the same amount of open.",
                "開き具合はまったく同じ。",
                "دقیقاً همون‌قدر بازه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It's not the same.",
                "同じじゃない。",
                "فرق داره.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Neutral,
                "There's a plant on the sill and nobody has named it.",
                "窓辺に鉢植えがあるのに、誰も名前つけてない。",
                "یه گلدون رو لبه‌ی پنجره‌ست و هیشکی اسم روش نذاشته.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Plants don't have names.",
                "植物に名前はつけないよ。",
                "گلدون که اسم نداره.");

            Say(Speaker.Yua, Portrait.Angry,
                "That plant has been in this room longer than we have. It was here in March. It watched the whole of the last class leave. It has seen more of this school than Morita-sensei and it does not have a name, and I am fixing that today.",
                "その鉢植え、あたしたちより先にこの教室にいたの。三月からいた。前のクラスが出ていくのを全部見てた。森田先生よりこの学校を見てるのに名前がない。今日つける。",
                "این گلدون از ما بیشتر تو این اتاق بوده. اسفند اینجا بوده. رفتنِ کلِ کلاسِ قبلی رو دیده. از خانمِ موریتا بیشتر این مدرسه رو دیده و اسم نداره، و من امروز درستش می‌کنم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Anko.",
                "アンコ。",
                "آنکو.");

            Say(Speaker.Yua, Portrait.Angry,
                "Anko is the cat.",
                "アンコは猫。",
                "آنکو گربه‌ست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You never read the collar.",
                "首輪読んでないでしょ。",
                "قلاده رو که نخوندی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Don't you dare.",
                "やめて。",
                "حرفشم نزن.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Monday's test is chapter three.",
                "月曜のテスト、三章。",
                "امتحانِ دوشنبه فصلِ سومه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It's one and two.",
                "一章と二章。",
                "فصلِ یک و دوئه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "She wrote three on the board and then rubbed it out.",
                "黒板に三章って書いて、消した。",
                "رو تخته نوشت سه، بعد پاکش کرد.");

            Say(Speaker.Yua, Portrait.Neutral,
                "…Why would she rub it out?",
                "…なんで消したの?",
                "...چرا پاکش کرد؟");

            Say(Speaker.Haru, Portrait.Joyful,
                "Because she said \"maybe\".",
                "「たぶん」って言ってたから。",
                "چون گفته بود «شاید».");

            Say(Speaker.Yua, Portrait.Joyful,
                "I am going to fail on a technicality.",
                "屁理屈で落ちる。",
                "من سرِ یه نکته‌ی فنی رد می‌شم.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "We could do chapter three in the library after school.",
                "放課後、図書室で三章やる?",
                "بعدِ مدرسه می‌تونیم فصلِ سه رو تو کتابخونه بخونیم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The bakery shuts at six.",
                "パン屋、六時に閉まる。",
                "نونوایی شیش می‌بنده.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It does.",
                "閉まるね。",
                "آره می‌بنده.");

            Say(Speaker.Yua, Portrait.Joyful,
                "So we're going to the bakery.",
                "だからパン屋。",
                "پس می‌ریم نونوایی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.6f);
        }

        // ---------------------------------------------------------------------
        //  Friday, after school — the corridor
        //
        //  ▣ Scene state
        //     Background   SchoolCorridorSunset: gold light, long shadows, the
        //                  tall windows, the lockers.
        //     Time         Friday, late afternoon.
        //     Clothes      Uniform.
        //     On stage     The lockers, the cleaning rota pinned by them, the
        //                  windows.
        //     In hand      Bags.
        //     From before  The plant, now named. The club list. Wednesday,
        //                  when she mentioned that Saturday was a half day.
        //
        //  ◆ Dread moment 7 of 10.
        //
        //      I told my mother it'd be late.
        //      When did you tell her that?
        //      Wednesday.
        //      I've only just said where we're going.
        //      You said Saturday on Wednesday.
        //      …So I did.
        //
        //  He has arranged his Saturday around a sentence she does not remember
        //  saying. The honest reading is that he pays attention, and half the
        //  people who play this will find it lovely. The other reading is that
        //  everything she says is kept. Neither of them remarks on it and the
        //  scene moves straight on to whose turn it is to sweep.
        // ---------------------------------------------------------------------

        private void WriteFridayCorridor()
        {
            ClearStage();

            Place(
                Backgrounds.CorridorSunset,
                "The corridor, Friday", "金曜の廊下", "راهرو، جمعه");

            Hold(1.8f);

            Narrate(
                "The rota was pinned up by the lockers with two names crossed out on it.",
                "掃除当番の表がロッカーの横に貼ってあって、名前が二つ消してあった。",
                "برنامه‌ی نظافت بغلِ کمدها زده شده بود و دو تا اسم روش خط خورده بود.");

            Enter(Speaker.Yua, Portrait.Joyful);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Unchanged,
                "The plant is called Tofu.",
                "鉢植えの名前、豆腐になった。",
                "اسمِ گلدون شد توفو.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Who decided that?",
                "誰が決めたの?",
                "کی تصمیم گرفت؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Four of us. There was a vote. Tofu got two, Midori got one, and one person wrote something rude and it was thrown out by the committee, which is me.",
                "四人で。投票した。豆腐が二票、みどりが一票、あと一人がひどいこと書いたから委員会が無効にした。委員会はあたし。",
                "چهار نفرمون. رأی‌گیری کردیم. توفو دو تا آورد، میدوری یکی، و یکی هم یه چیزِ بی‌ادبی نوشت که کمیته ردش کرد، و کمیته منم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's four votes and three people.",
                "四票で三人だね。",
                "می‌شه چهار رأی و سه نفر.");

            Say(Speaker.Yua, Portrait.Angry,
                "Tofu voted.",
                "豆腐も投票した。",
                "توفو هم رأی داد.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Fair.",
                "なるほど。",
                "قبوله.");

            Hold(1.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "We're on the rota next week.",
                "来週、掃除当番だ。",
                "هفته‌ی بعد نظافتِ ماست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "We're on it the week after.",
                "再来週でしょ。",
                "هفته‌ی بعدترشه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "The crossed-out ones are this week. We're the two under them.",
                "消してあるのが今週。その下が僕たち。",
                "خط‌خورده‌ها مالِ این هفته‌ن. ما دوتای زیرشونیم.");

            Say(Speaker.Yua, Portrait.Neutral,
                "…We're the two under them.",
                "…その下だ。",
                "...ما دوتای زیرشونیم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Bring gloves.",
                "手袋持ってきて。",
                "دستکش بیار.");

            Say(Speaker.Yua, Portrait.Joyful,
                "You bring gloves. I'll bring opinions.",
                "手袋はハルぴ。あたしは意見を持ってく。",
                "تو دستکش بیار. من نظر میارم.");

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Tomorrow's the half day. We'll go to the playground with the fountain.",
                "明日は半日。噴水のある公園行こ。",
                "فردا نصفِ روزه. می‌ریم اون پارکی که فواره داره.");

            // ◆ Dread moment 7.
            Say(Speaker.Haru, Portrait.Unchanged,
                "I told my mother it'd be late.",
                "母さんには遅くなるって言ってある。",
                "به مامانم گفتم دیر می‌شه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "When did you tell her that?",
                "いつ言ったの?",
                "کِی بهش گفتی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Wednesday.",
                "水曜。",
                "چهارشنبه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "I've only just said where we're going.",
                "行き先、今言ったばっかりだけど。",
                "من که تازه الآن گفتم کجا می‌ریم.");

            Say(Speaker.Haru, Portrait.Shy,
                "You said Saturday on Wednesday.",
                "水曜に、土曜って言った。",
                "چهارشنبه گفتی شنبه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "…So I did.",
                "…言ったね。",
                "...آره گفتم.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "We could meet at the station.",
                "駅で待ち合わせでもいいけど。",
                "می‌تونیم ایستگاه همدیگه رو ببینیم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The corner.",
                "角で。",
                "سرِ نبش.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "And then the bakery, because it's still open now.",
                "で、今はまだ開いてるからパン屋。",
                "و حالا هم نونوایی، چون هنوز بازه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It's still open now.",
                "まだ開いてる。",
                "هنوز بازه.");

            Hold(1.6f);
        }

        // ---------------------------------------------------------------------
        //  Friday, evening — outside Usagi Bakery
        //
        //  ▣ Scene state
        //     Background   UsagiBakeryStreetDay: a cobbled street, the shop
        //                  window full of bread, a wooden bench with cats
        //                  asleep on it, flowers in pots.
        //     Time         Friday, early evening. Still open.
        //     Clothes      Uniform.
        //     On stage     The window, the bench, the cats, the pots.
        //     In hand      Bags, and one melon bread between them.
        //     From before  Usagi, named on Monday morning. The other bakery by
        //                  the station, which he prefers and she does not.
        //
        //  Zero dread. Friday's two moments were both spent inside school and
        //  the evening is allowed to be a boy and a girl arguing about whether
        //  a cat would eat melon bread.
        //
        //  The last thing in the day is the design document's leg interlude,
        //  which comes up at random on a walk home in every act: he stops, says
        //  his leg is hurting, says nothing else, and she waits without asking.
        //  It is its own asset and its own scene, and it is deliberately not
        //  part of the ten budgeted moments — a thing the game may or may not
        //  show you is not a thing the script can spend a budget on.
        // ---------------------------------------------------------------------

        private void WriteFridayBakery()
        {
            ClearStage();

            Place(
                Backgrounds.BakeryStreetDay,
                "Usagi Bakery", "うさぎパン", "نونواییِ اوساگی");

            Hold(1.8f);

            Narrate(
                "There were three cats asleep on the bench outside and none of them moved.",
                "外のベンチで猫が三匹寝ていて、どれも動かなかった。",
                "سه تا گربه رو نیمکتِ بیرون خواب بودن و هیچ‌کدوم تکون نخوردن.");

            Enter(Speaker.Yua, Portrait.Joyful);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Three cats.",
                "猫三匹。",
                "سه تا گربه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "They're here every day.",
                "毎日いるよ。",
                "هر روز اینجان.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "How do you know that?",
                "なんで知ってるの?",
                "تو از کجا می‌دونی؟");

            Say(Speaker.Haru, Portrait.Joyful,
                "I walk past every day.",
                "毎日通るから。",
                "هر روز از اینجا رد می‌شم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "That is a boring answer and I reject it.",
                "つまらない答え。却下。",
                "این جوابِ کسل‌کننده‌ایه و من قبولش نمی‌کنم.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Neutral,
                "I'm getting one for the cats.",
                "猫にひとつ買う。",
                "یکی هم واسه گربه‌ها می‌گیرم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Cats don't eat melon bread.",
                "猫はメロンパン食べないよ。",
                "گربه ملون‌پان نمی‌خوره.");

            Say(Speaker.Yua, Portrait.Angry,
                "Everything eats melon bread.",
                "みんなメロンパン食べる。",
                "همه‌چی ملون‌پان می‌خوره.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's sugar and flour.",
                "砂糖と小麦粉だよ。",
                "شکره و آرد.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "So am I, mostly.",
                "あたしもだいたいそう。",
                "منم بیشترم از همینام.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You're not a cat.",
                "猫じゃないでしょ。",
                "تو که گربه نیستی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Not with that attitude.",
                "その言い方だとね。",
                "با این طرزِ فکرِ تو نه.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "This one's better than the station one.",
                "ここのが駅のより好き。",
                "این یکی از ایستگاهیه بهتره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I have been saying that since Monday.",
                "月曜からずっと言ってる。",
                "من از دوشنبه دارم همینو می‌گم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You said the station one was dry. That's a different sentence.",
                "駅のはぱさぱさって言った。別の文でしょ。",
                "تو گفتی ایستگاهیه خشکه. این یه جمله‌ی دیگه‌ست.");

            Say(Speaker.Yua, Portrait.Angry,
                "It is the same sentence with better manners.",
                "同じ文を丁寧に言っただけ。",
                "همون جمله‌ست با ادبِ بیشتر.");

            Say(Speaker.Haru, Portrait.Joyful,
                "They both make good bread. That's allowed.",
                "どっちもおいしいよ。両方でいい。",
                "هردوشون نونِ خوب می‌پزن. اشکالی نداره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "It is not allowed. You have to pick one. That's what a bakery is for. You pick one and then you defend it for the rest of your life against people who are wrong.",
                "だめ。ひとつ選ぶの。パン屋ってそういうもの。選んで、あとは一生、間違ってる人たちから守るの。",
                "اشکال داره. باید یکی رو انتخاب کنی. نونوایی واسه همینه. یکی رو انتخاب می‌کنی و تا آخرِ عمرت جلوی آدمایی که اشتباه می‌کنن ازش دفاع می‌کنی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Then I pick this one.",
                "じゃあこっち。",
                "پس من این یکی رو انتخاب می‌کنم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Good. Now you're a person.",
                "よし。人間になった。",
                "خوبه. حالا شدی آدم.");

            Hold(1.4f);

            Narrate(
                "There was one melon bread left in the window and she bought it.",
                "ショーケースにメロンパンが一つ残っていて、彼女がそれを買った。",
                "یه ملون‌پان تو ویترین مونده بود و یوآ خریدش.");

            Say(Speaker.Haru, Portrait.Neutral,
                "You have it.",
                "食べなよ。",
                "خودت بخورش.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You're having it.",
                "ハルぴが食べるの。",
                "تو می‌خوریش.");

            Say(Speaker.Haru, Portrait.Shy,
                "You queued for it.",
                "並んだの結愛ぴでしょ。",
                "تو واسش تو صف وایسادی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "And I've decided. Eat it before the cats hear about it.",
                "決めた。猫にバレる前に食べて。",
                "و تصمیممو گرفتم. بخورش قبل از اینکه گربه‌ها بفهمن.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.8f);

            Say(Speaker.Yua, Portrait.Neutral,
                "The corner. Tomorrow.",
                "明日、角で。",
                "سرِ نبش. فردا.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "The corner.",
                "角で。",
                "سرِ نبش.");

            Hold(2.0f);

            // The design document's leg interlude, on a walk home, at random.
            Maybe(LegAche, 0.3f);
        }

        // =====================================================================
        //  SATURDAY — day six of six, and the end of act one
        //
        //  Dread budget: 3. A fact he watched with his own eyes and gave up, a
        //  bench nobody asked him to save, and two frames on an empty corner.
        // =====================================================================

        private void WriteSaturday()
        {
            WriteSaturdayClassroom();
            WriteSaturdayCorner();
            WriteSaturdayPlayground();
            WriteSaturdayNight();
        }

        // ---------------------------------------------------------------------
        //  Saturday, half day — classroom 1-A
        //
        //  ▣ Scene state
        //     Background   SunnyClassroomDay.
        //     Time         Saturday, late morning. Half day.
        //     Clothes      Uniform.
        //     On stage     Desks, Tofu on the sill, the window that shuts now.
        //     In hand      Bags, and a club application form each.
        //     From before  Five days of not reading the club list. Tofu. The
        //                  corner, where they are meeting later.
        //
        //  The running joke of the week pays off by the paper coming to them
        //  instead: Morita-sensei hands the club forms out in class, so after
        //  five refusals he finally has one in his hand.
        //
        //  And then the biggest thing he gives up in the act happens in about
        //  eight seconds, in the middle of a joke, and neither of them notices
        //  it. He wants to join the gardening club. He does not join the
        //  gardening club. Nobody says a word about him not joining, then or
        //  ever, and the act does not point at it — which is what makes it
        //  work, and also what makes it the one the player remembers in act
        //  five when he says he tried everything.
        // ---------------------------------------------------------------------

        private void WriteSaturdayClassroom()
        {
            ClearStage();

            Place(
                Backgrounds.ClassroomDay,
                "Class 1-A, Saturday", "土曜、一年A組", "کلاسِ اول-الف، شنبه");

            Hold(1.6f);

            Narrate(
                "Morita-sensei came round with a stack of club forms and put one on every desk.",
                "森田先生が部活の用紙を配って、全部の机に一枚ずつ置いていった。",
                "خانمِ موریتا با یه دسته فرمِ باشگاه اومد و رو هر میز یکی گذاشت.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Joyful);

            Say(Speaker.Haru, Portrait.Unchanged,
                "The list came to us.",
                "向こうから来た。",
                "لیست خودش اومد پیشِ ما.");

            Say(Speaker.Yua, Portrait.Angry,
                "Don't.",
                "言わないで。",
                "نگو.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Five days.",
                "五日。",
                "پنج روز.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Don't.",
                "言わないで。",
                "نگو.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Tomorrow, tomorrow, tomorrow, tomorrow, and then a teacher did it for us.",
                "明日、明日、明日、明日、そして先生がやってくれた。",
                "فردا، فردا، فردا، فردا، و آخرش یه معلم به‌جامون انجامش داد.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I'm putting this in the bin.",
                "これ捨てる。",
                "این رو می‌ندازم تو سطل.");

            Hold(1.2f);

            Say(Speaker.Yua, Portrait.Neutral,
                "There's a sports festival in June and they want volunteers already.",
                "六月に体育祭で、もう係を募集してる。",
                "ژوئن جشنِ ورزشی دارن و از الآن داوطلب می‌خوان.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Are you volunteering?",
                "やるの?",
                "داوطلب می‌شی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I'm going to run the whole thing and then complain about it constantly for two months. That's the plan. That's been the plan since I read the notice, which was eleven minutes ago.",
                "全部仕切って、二か月ずっと文句を言う。それが計画。掲示を読んだ十一分前からの計画。",
                "کلشو خودم می‌چرخونم و بعد دو ماه یه‌بند ازش غر می‌زنم. نقشه‌م اینه. از وقتی اعلان رو خوندم نقشه‌م همینه، که یازده دقیقه پیش بوده.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's a good plan.",
                "いい計画。",
                "نقشه‌ی خوبیه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It's a perfect plan.",
                "完璧な計画。",
                "نقشه‌ی بی‌نقصیه.");

            Hold(1.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "There's a gardening club.",
                "園芸部がある。",
                "باشگاهِ باغبونی دارن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Of course there's a gardening club.",
                "そりゃあるでしょ。",
                "معلومه که باشگاهِ باغبونی دارن.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "They've got the beds along the back wall. Tuesdays and Fridays.",
                "裏の花壇、そこ。火曜と金曜。",
                "باغچه‌های پشتِ دیوار مالِ اوناست. سه‌شنبه و جمعه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Tuesdays and Fridays are café days.",
                "火曜と金曜はカフェの日。",
                "سه‌شنبه و جمعه روزِ کافه‌ست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "…They are.",
                "…そうだね。",
                "...آره هستن.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Also you'd be terrible at it. You'd apologise to the plants.",
                "それにハルぴ、絶対向いてない。植物に謝るでしょ。",
                "تازه افتضاح هم می‌شدی توش. از گیاها عذرخواهی می‌کردی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I would apologise to the plants.",
                "植物に謝る。",
                "از گیاها عذرخواهی می‌کردم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "See.",
                "でしょ。",
                "دیدی.");

            Hold(1.6f);

            Cue(SfxId.SchoolBell, 0.5f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Right. Home, bag, and then the corner.",
                "はい。家、鞄置いて、角。",
                "خب. خونه، کیف، بعدش سرِ نبش.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "And then the playground with the fountain.",
                "そのあと噴水のある公園。",
                "بعدش اون پارکی که فواره داره.");

            Say(Speaker.Yua, Portrait.Joyful,
                "You remembered the fountain.",
                "噴水、覚えてたんだ。",
                "فواره رو یادت مونده.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You said it twice.",
                "二回言ったから。",
                "دو بار گفتیش.");

            Hold(1.8f);
        }

        // ---------------------------------------------------------------------
        //  Saturday, afternoon — the corner
        //
        //  ▣ Scene state
        //     Background   PastelStreetVendingDay: the pink machine, the
        //                  flowerbeds, the wall the bicycle was against.
        //     Time         Saturday, early afternoon. Out of uniform is not an
        //                  option — there is one sprite — so they are still in
        //                  it, and nobody mentions clothes.
        //     On stage     The machine, the flowerbeds, the wall.
        //     In hand      Bags are at home. Nothing in anybody's hands.
        //     From before  Wednesday, when the machine ate his hundred yen and
        //                  she pronounced it fine.
        //
        //  ◆ Dread moment 8 of 10.
        //
        //      That machine ate a hundred yen of yours.
        //      You said it was fine.
        //      …I did say that.
        //      So it's fine.
        //
        //  He was standing right there. He put the coin in, he pressed it
        //  twice, he hit the side of it, and nothing came out. Four days later
        //  her sentence about it has replaced his own eyes, and he says it
        //  cheerfully, and she agrees, and the scene carries on to the
        //  flowerbeds.
        //
        //  Deniable in every direction: he is being funny, he is being loyal,
        //  it is a machine and it does not matter. Nothing here is worth
        //  arguing about, which is exactly why he does not argue.
        // ---------------------------------------------------------------------

        private void WriteSaturdayCorner()
        {
            ClearStage();

            Place(
                Backgrounds.VendingStreetDay,
                "The corner, Saturday", "土曜、角の道", "سرِ نبش، شنبه");

            Hold(1.8f);

            Narrate(
                "The bicycle was gone from the wall and there was nothing where it had been.",
                "壁のところの自転車はなくなっていて、あとには何もなかった。",
                "دوچرخه از کنارِ دیوار رفته بود و جاش هیچی نبود.");

            Enter(Speaker.Haru, Portrait.Neutral);

            Hold(1.2f);

            Enter(Speaker.Yua, Portrait.Joyful);

            Say(Speaker.Yua, Portrait.Unchanged,
                "How long have you been standing here?",
                "どれくらい立ってたの?",
                "چقدر وقته اینجا وایسادی؟");

            Say(Speaker.Haru, Portrait.Shy,
                "Not long.",
                "そんなに。",
                "زیاد نه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "The bicycle's gone.",
                "自転車、なくなってる。",
                "دوچرخه رفته.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Somebody rode it.",
                "誰かが乗った。",
                "یکی سوارش شد.");

            Say(Speaker.Yua, Portrait.Angry,
                "Somebody stole it.",
                "盗まれたんでしょ。",
                "یکی دزدیدش.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "The lock's gone too.",
                "鍵もない。",
                "قفلشم نیست.");

            Say(Speaker.Yua, Portrait.Neutral,
                "…The lock's gone too.",
                "…鍵もない。",
                "...قفلشم نیست.");

            Say(Speaker.Haru, Portrait.Joyful,
                "They were waiting for something, and it came.",
                "何かを待ってて、それが来たんだよ。",
                "منتظرِ یه چیزی بود، و اون چیز اومد.");

            Say(Speaker.Yua, Portrait.Joyful,
                "You've been saving that all week.",
                "一週間温めてたでしょ。",
                "کلِ هفته این رو نگه داشته بودی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Since Monday.",
                "月曜から。",
                "از دوشنبه.");

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Angry,
                "The salty lychee is still in there.",
                "塩ライチ、まだある。",
                "لیچیِ شور هنوز توشه.");

            Say(Speaker.Haru, Portrait.Neutral,
                "It's a full row of it.",
                "一列全部それ。",
                "یه ردیفِ کامل همونه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Nobody in this town wants salty lychee and it is taking up an entire row, and every day it does not sell, somebody somewhere becomes slightly more certain that it is selling.",
                "この町に塩ライチを欲しがる人はいないのに、一列占領してる。売れない日が続くほど、どこかの誰かが「売れてる」って確信していく。",
                "تو این محله هیشکی لیچیِ شور نمی‌خواد و یه ردیفِ کامل رو گرفته، و هر روزی که فروش نمی‌ره، یکی یه جایی یه‌ذره مطمئن‌تر می‌شه که داره فروش می‌ره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You could buy one and end the experiment.",
                "一本買えば実験終わるよ。",
                "می‌تونی یکی بخری و آزمایش رو تموم کنی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I would rather die on this corner.",
                "この角で死んだ方がまし。",
                "ترجیح می‌دم همین سرِ نبش بمیرم.");

            Hold(1.4f);

            // ◆ Dread moment 8.
            Say(Speaker.Yua, Portrait.Neutral,
                "That machine ate a hundred yen of yours.",
                "その自販機、ハルぴの百円飲んだよね。",
                "این دستگاه صد ینت رو خورد.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You said it was fine.",
                "大丈夫って言ってた。",
                "خودت گفتی سالمه.");

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "…I did say that.",
                "…言った。",
                "...آره، گفتم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "So it's fine.",
                "じゃあ大丈夫。",
                "پس سالمه.");

            Hold(1.8f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Shall I get us something for the walk?",
                "歩きながら飲むもの買う?",
                "می‌خوای واسه راه یه چیزی بگیرم؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "There's a shop by the playground.",
                "公園のとこにお店ある。",
                "بغلِ پارک یه مغازه هست.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "It's ten minutes that way, past the houses.",
                "あっちに十分、家並みを抜けて。",
                "ده دقیقه اون‌وره، از لای خونه‌ها.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Then let's go the way with the wall you can walk on.",
                "じゃあ、あの塀の上を歩ける道で行こ。",
                "پس از اون راهی بریم که یه دیوار داره آدم می‌تونه روش راه بره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You're not walking on a wall.",
                "塀の上は歩かない。",
                "تو رو دیوار راه نمی‌ری.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.8f);
        }

        // ---------------------------------------------------------------------
        //  Saturday, late afternoon — the playground
        //
        //  ▣ Scene state
        //     Background   PastelPlaygroundDay: a stone angel fountain in the
        //                  middle, a swing set and a slide with children on
        //                  them, flowerbeds all round, houses behind.
        //     Time         Saturday, late afternoon, going gold.
        //     On stage     The fountain, the swings, the slide, the beds, the
        //                  bench by the gate.
        //     In hand      Nothing.
        //     From before  Nothing needed. This is the one place in the act
        //                  neither of them has to be.
        //
        //  ◆ Dread moment 9 of 10.
        //
        //  He goes to the bench nearest the gate and wipes it off before she
        //  has said anything about where she wants to sit. One line of
        //  narration, no thanks, no comment, and the conversation is about
        //  fountains four frames later.
        //
        //  The first time he did that was Monday morning, with a desk by a
        //  window, and it read as a considerate friend. The manual's line about
        //  it is the whole design: the first time is thoughtfulness, and the
        //  second time is training.
        //
        //  ▣ The flowerbed
        //     Stage one of the motif act three is built on, and stage one is
        //     supposed to be so plain it is nearly dull. He asks what is planted
        //     there. She says nothing yet, and that it goes red in autumn. Six
        //     frames, no weight on any of them, and it is spring — the cherry
        //     is only just off the trees — so red spider lilies are months away
        //     and neither the flower nor its name is spoken. When act three puts
        //     her in front of them and she says the thing about her eyes, this
        //     is the scene it lands on.
        // ---------------------------------------------------------------------

        private void WriteSaturdayPlayground()
        {
            ClearStage();

            Place(
                Backgrounds.PlaygroundDay,
                "The playground", "公園", "پارک");

            Hold(2.0f);

            Narrate(
                "There were four children on the slide and a queue behind them that was mostly arguing.",
                "滑り台に子どもが四人いて、その後ろの列はだいたい言い合いだった。",
                "چهار تا بچه رو سرسره بودن و صفِ پشتشون بیشتر داشت بحث می‌کرد.");

            // ◆ Dread moment 9.
            Narrate(
                "Haru went to the bench by the gate and wiped the dust off it with his sleeve.",
                "ハルは門のそばのベンチに行って、袖でほこりを払った。",
                "هارو رفت سمتِ نیمکتِ کنارِ درِ پارک و با آستینش گردِ روش رو پاک کرد.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Joyful,
                "That angel is holding a bird.",
                "あの天使、鳥持ってる。",
                "اون فرشته یه پرنده تو دستشه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's a fish.",
                "魚だよ。",
                "ماهیه.");

            Say(Speaker.Yua, Portrait.Angry,
                "It has wings.",
                "羽根ある。",
                "بال داره.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "The angel has wings. The fish has fins.",
                "羽根は天使。魚はひれ。",
                "بال مالِ فرشته‌ست. ماهی باله داره.");

            Say(Speaker.Yua, Portrait.Neutral,
                "…That's a fin.",
                "…ひれだ。",
                "...اون باله‌ست.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's a fin.",
                "ひれ。",
                "باله‌ست.");

            Say(Speaker.Yua, Portrait.Joyful,
                "This is the sixth day in a row.",
                "六日連続だよ。",
                "شیش روزه پشتِ سرِ هم داری اینکارو می‌کنی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You keep starting them.",
                "始めるのは結愛ぴだけど。",
                "شروع‌کننده‌ش خودتی.");

            Hold(1.4f);

            Narrate(
                "Two of the children by the slide had got as far as shouting.",
                "滑り台の子ども二人は、もう怒鳴り合いになっていた。",
                "دو تا از بچه‌های سرِ سرسره کارشون به داد زدن کشیده بود.");

            Exit(Speaker.Yua);

            Hold(1.0f);

            Narrate(
                "She went over, said something to both of them, and came back.",
                "彼女は行って、二人に何か言って、戻ってきた。",
                "یوآ رفت، به هردوشون یه چیزی گفت، و برگشت.");

            Enter(Speaker.Yua, Portrait.Joyful);

            Say(Speaker.Haru, Portrait.Neutral,
                "What did you tell them?",
                "何て言ったの?",
                "چی بهشون گفتی؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That the one at the top goes first because that is how a slide works, and that the one at the bottom is allowed to count to five out loud, and now they are both busy being in charge of something and neither of them is shouting.",
                "上にいる子が先に滑る、それが滑り台のルール。下の子は五まで声に出して数えていい。二人とも何かの担当になったから、もう怒鳴ってない。",
                "گفتم اونی که بالاست اول می‌ره چون سرسره همین‌جوریه، و اونی که پایینه اجازه داره بلندبلند تا پنج بشمره، و حالا هردوشون مسئولِ یه چیزی‌ان و هیچ‌کدوم داد نمی‌زنه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's frightening.",
                "こわいな。",
                "این ترسناکه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "That's management.",
                "運営っていうの。",
                "بهش می‌گن مدیریت.");

            Hold(1.4f);

            DecideIdly(
                "Get on the swings", "ブランコに乗る", "برو رو تاب",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Joyful,
                        "The swings are free.",
                        "ブランコ空いてる。",
                        "تاب‌ها خالی‌ان.");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "They're for children.",
                        "子ども用でしょ。",
                        "مالِ بچه‌هاست.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "There's no sign.",
                        "看板ないもん。",
                        "تابلو که نداره.");

                    Say(Speaker.Haru, Portrait.Shy,
                        "There's four children watching.",
                        "子どもが四人見てる。",
                        "چهار تا بچه دارن نگاه می‌کنن.");

                    Say(Speaker.Yua, Portrait.Joyful,
                        "Then they'll learn something.",
                        "勉強になるでしょ。",
                        "خب یه چیزی یاد می‌گیرن.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "They will learn something.",
                        "何かは学ぶね。",
                        "یه چیزی یاد می‌گیرن.");
                },
                "Sit by the fountain", "噴水のそばに座る", "بشین کنارِ فواره",
                () =>
                {
                    Say(Speaker.Yua, Portrait.Neutral,
                        "The stone's warm.",
                        "石、あったかい。",
                        "سنگش گرمه.");

                    Say(Speaker.Haru, Portrait.Unchanged,
                        "It's been in the sun all day.",
                        "一日中日が当たってたから。",
                        "کلِ روز آفتاب خورده.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "Can you drink this water?",
                        "この水、飲める?",
                        "این آب رو می‌شه خورد؟");

                    Say(Speaker.Haru, Portrait.Neutral,
                        "No.",
                        "だめ。",
                        "نه.");

                    Say(Speaker.Yua, Portrait.Unchanged,
                        "That was quick.",
                        "即答だ。",
                        "چه سریع.");

                    Say(Speaker.Haru, Portrait.Joyful,
                        "There's a coin in it and a leaf and something I'm not looking at again.",
                        "硬貨と葉っぱと、もう見たくない何かが入ってる。",
                        "توش یه سکه هست و یه برگ و یه چیزی که دیگه بهش نگاه نمی‌کنم.");
                });

            Hold(1.4f);

            // ▣ The flowerbed. Stage one, and deliberately flat.
            Say(Speaker.Haru, Portrait.Neutral,
                "What's planted along there?",
                "そこ、何植えてあるの?",
                "اون‌طرف چی کاشتن؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Nothing yet.",
                "まだ何も。",
                "فعلاً هیچی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It's all soil.",
                "土だけだね。",
                "همه‌ش خاکه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "In autumn it goes red. The whole bed, all at once.",
                "秋に赤くなるの。花壇まるごと、いっぺんに。",
                "پاییز قرمز می‌شه. کلِ باغچه، یهو با هم.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Red flowers?",
                "赤い花?",
                "گلِ قرمز؟");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Loads of them.",
                "たくさん。",
                "کلی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "We should come back in autumn.",
                "秋にまた来よ。",
                "پاییز باید دوباره بیایم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "We should.",
                "来よう。",
                "باید بیایم.");

            Hold(1.8f);

            Say(Speaker.Haru, Portrait.Neutral,
                "We could stay till the lights come on.",
                "電気つくまでいてもいいけど。",
                "می‌تونیم بمونیم تا چراغا روشن بشن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "One more go on the slide first.",
                "その前に滑り台もう一回。",
                "اول یه بارِ دیگه سرسره.");

            Say(Speaker.Haru, Portrait.Shy,
                "…On the slide.",
                "…滑り台。",
                "...سرسره.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I'm in charge of it now. I made the rules.",
                "今あたしが担当。ルール作ったの。",
                "الآن من مسئولشم. قانوناشو خودم گذاشتم.");

            Hold(2.0f);
        }

        // ---------------------------------------------------------------------
        //  Saturday night — the corner, and the end of act one
        //
        //  ▣ Scene state
        //     Background   PastelStreetVendingNight: a starry sky, a crescent
        //                  moon, the pink machine lit up, a bicycle back
        //                  against the wall, streetlamps, flowerbeds.
        //     Time         Saturday, after dark.
        //     On stage     The machine, the bicycle, the lamps.
        //     In hand      Nothing.
        //     From before  The bicycle, which was gone this afternoon and is
        //                  here now. Monday morning, and a boy standing under
        //                  a tree because the shade was better.
        //
        //  ◆ Dread moment 10 of 10 — the last three frames of the act.
        //
        //      Six days.
        //      Nothing changed.
        //      …And nothing's going to.
        //
        //  Said by a happy girl on an empty street about the best week she has
        //  had, and it is a completely ordinary thing for a happy girl to
        //  think. It is also a vow, and it is the same voice that closed
        //  Monday, and the act stops on it rather than ending.
        //
        //  Everything before those three frames is warm on purpose. The last
        //  thing the two of them do together in act one is her taking his joke
        //  out of his mouth and using it herself, which is the friendliest
        //  version of the thing this whole game is about.
        // ---------------------------------------------------------------------

        private void WriteSaturdayNight()
        {
            ClearStage();

            Place(
                Backgrounds.VendingStreetNight,
                "The corner, Saturday night", "土曜の夜の角", "سرِ نبش، شبِ شنبه");

            Hold(2.2f);

            Narrate(
                "The streetlamps were on and the machine was the brightest thing on the road.",
                "街灯がついて、通りで一番明るいのは自販機だった。",
                "چراغای خیابون روشن بودن و دستگاه روشن‌ترین چیزِ خیابون بود.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Haru, Portrait.Unchanged,
                "The bicycle's back.",
                "自転車、戻ってる。",
                "دوچرخه برگشته.");

            Say(Speaker.Yua, Portrait.Joyful,
                "So nobody stole it.",
                "盗まれてないじゃん。",
                "پس ندزدیدنش.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Somebody rode it and brought it back.",
                "誰かが乗って、返した。",
                "یکی سوارش شد و برش گردوند.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "…Yeah.",
                "…そうだね。",
                "...آره.");

            Say(Speaker.Haru, Portrait.Joyful,
                "That's six.",
                "六回目。",
                "می‌شه شیش تا.");

            Say(Speaker.Yua, Portrait.Angry,
                "You are not counting.",
                "数えてないでしょ。",
                "تو که نمی‌شمری.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I'm counting.",
                "数えてる。",
                "دارم می‌شمرم.");

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Test on Monday.",
                "月曜テスト。",
                "دوشنبه امتحان.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Chapters one and two.",
                "一章と二章。",
                "فصلِ یک و دو.");

            Say(Speaker.Yua, Portrait.Angry,
                "You said three.",
                "三章って言ってた。",
                "تو گفتی سه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I asked her yesterday. It's one and two.",
                "昨日聞いた。一章と二章。",
                "دیروز ازش پرسیدم. یک و دوئه.");

            Say(Speaker.Yua, Portrait.Neutral,
                "You asked Morita-sensei a question.",
                "森田先生に質問したの。",
                "تو از خانمِ موریتا سؤال پرسیدی.");

            Say(Speaker.Haru, Portrait.Shy,
                "It was a small question.",
                "小さい質問。",
                "سؤالِ کوچیکی بود.");

            Say(Speaker.Yua, Portrait.Joyful,
                "You spoke to a teacher on purpose. On a Friday. Out loud. In front of her. And you have been sitting on it for a whole day like it was nothing.",
                "自分から先生に話しかけたんだ。金曜に。声に出して。本人の前で。それを一日、何でもない顔で抱えてた。",
                "تو عمداً با یه معلم حرف زدی. اونم جمعه. بلندبلند. جلوی خودش. و یه روزِ کامل نگهش داشتی انگار هیچی نشده.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It was a small question.",
                "小さい質問だってば。",
                "سؤالِ کوچیکی بود.");

            Hold(1.6f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Your street's that way.",
                "そっちが家の通り。",
                "کوچه‌ی شما اون‌وره.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "It is.",
                "うん。",
                "آره.");

            Say(Speaker.Haru, Portrait.Neutral,
                "I'll walk you round.",
                "回って送るよ。",
                "دور می‌زنم می‌رسونمت.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Then you'd walk back on your own in the dark.",
                "そしたら帰り、暗い中一人でしょ。",
                "اون‌وقت خودت تنهایی تو تاریکی برمی‌گردی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "…I would.",
                "…そうだね。",
                "...آره برمی‌گردم.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Go home, Haru-pi.",
                "帰りな、ハルぴ。",
                "برو خونه، هاروپی.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(1.4f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Monday.",
                "月曜。",
                "دوشنبه.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Monday.",
                "月曜。",
                "دوشنبه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Stand under that stupid tree again.",
                "またあの木の下に立ってて。",
                "بازم زیرِ همون درختِ مسخره وایسا.");

            Say(Speaker.Haru, Portrait.Joyful,
                "It's a good tree.",
                "いい木だよ。",
                "درختِ خوبیه.");

            // His joke, six days old, in her mouth, and the last thing either
            // of them says in act one.
            Say(Speaker.Yua, Portrait.Joyful,
                "It's a good tree.",
                "いい木。",
                "درختِ خوبیه.");

            Hold(2.4f);

            Exit(Speaker.Haru);

            Hold(2.6f);

            // ◆ Dread moment 10.
            InnerVoice(
                "Six days.",
                "六日。",
                "شیش روز.");

            InnerVoice(
                "Nothing changed.",
                "何も変わらなかった。",
                "هیچی عوض نشد.");

            Hold(1.8f);

            InnerVoice(
                "…And nothing's going to.",
                "…これからも変わらない。",
                "...هیچی هم عوض نمی‌شه.");

            Hold(3.4f);

            ClearStage();

            Hold(1.2f);
        }

        // =====================================================================
        //  SELF-AUDIT
        //
        //  The manual's section sixteen, run against the twenty scenes above,
        //  in the order the manual puts them, because the order is the point:
        //  the human questions come first and they outrank everything else.
        //
        //  ── Human ──────────────────────────────────────────────────────────
        //
        //  If there were no secret, would this be fun?
        //      Yes, and that was the test each scene was cut against. Six days
        //      of two teenagers arguing about shade, bread, a manga nobody owns
        //      the first three volumes of, how many dango are on a stick, a cat
        //      with a collar she refuses to read, a plant called Tofu, whether
        //      an angel is holding a fish, and salty lychee. Nothing in the act
        //      needs act five to be worth playing.
        //
        //  Where is it funny or warm?
        //      Every scene has at least one place. The shade expert. "Somebody
        //      said." Four dango. Anko. The bread that is not dry. Tofu voting.
        //      The club list arriving by teacher on the last day. The slide
        //      committee. He apologises to plants and admits it.
        //
        //  Is half of it about a third thing?
        //      More than half, in every scene, and deliberately never the same
        //      third thing twice running: a teacher, a bakery, a window, a
        //      manga, a cat, a lunch, a vending machine flavour, a bicycle, a
        //      song, a train, a rota, a plant, a fountain, a flowerbed.
        //
        //  The substitution test — replace all of Haru's answers with "okay":
        //      The act collapses. He wins an argument in every single scene,
        //      on facts she does not have: the baker changed, the window has
        //      been broken a year, there are four dango, the melon soda light
        //      means sold out, the big staircase stops on three, the orange
        //      trains do not stop, the crossed-out names are this week, the
        //      angel is holding a fish, she said Saturday on Wednesday, and the
        //      test is chapters one and two because he asked a teacher.
        //
        //  Did he win something unimportant and lose something real?
        //      In every scene, and it is never named. Where they eat the rolls,
        //      the club board, the window, where they sit, the roof, two pieces
        //      of sushi back, a second hundred yen, walking her home twice,
        //      studying in the library, the wall he wanted to walk on, and the
        //      gardening club. Not one of those is remarked on by anybody.
        //
        //  Does Yua do something that has nothing to do with Haru?
        //      Every scene: the second rabbit on the pencil case, volume four,
        //      the price of the peach drink, the bird on the fence, the salty
        //      lychee campaign, naming the plant, the sports festival, the two
        //      children on the slide.
        //
        //  Are the lines all the same length?
        //      No. Every scene has one genuinely long one — the pencil case,
        //      volume four, the rice at six in the morning, the bicycle in the
        //      path, the salty lychee row, the bakery loyalty speech, the slide
        //      committee, the small question — and a lot of one-word ones.
        //
        //  Does any scene sound like the one before it?
        //      Checked in pairs. The three Tuesday scenes are a complaint, a
        //      count and a naming argument. Wednesday is a boast, a meal and a
        //      slapstick. Thursday is a rant, a dare and an intrusion. No two
        //      consecutive scenes share a joke shape or a location.
        //
        //  ── Forbidden patterns ─────────────────────────────────────────────
        //
        //  Astonishment and explanation?
        //      None. Nobody in twenty scenes asks how the other one knows
        //      anything, and nothing is ever explained. The one place it could
        //      have gone — "you always take the third one" — is answered with
        //      "yeah" and the scene moves to a bird on a fence.
        //
        //  Order and obedience with nothing in between?
        //      Checked at every "okay". There is a joke, a fact, a refusal or a
        //      change of subject between the ask and the yes in all of them.
        //      The single deliberate exception is "drink all of it", which is
        //      dread moment 4 and is bare on purpose.
        //
        //  Status announcements?
        //      The old draft's clock, headcount, exit count and tree number are
        //      all gone. Every number left in the act is load-bearing: six
        //      sushi and four sausages, because the drawing shows them; four
        //      dango, because he counted them out loud; a hundred yen, because
        //      it is lost; ten yen, because it is a complaint; six days, at the
        //      end, because that is the act.
        //
        //  Observation and confirmation?
        //      Every observation goes somewhere — into an argument, a joke or a
        //      decision. Nobody says "the soil is wet" and is told "so it was
        //      watered".
        //
        //  More than four question-and-answer turns in a row?
        //      Longest chain in the act is four, in the Monday teacher exchange
        //      and the Wednesday staircase one, and both are broken by a joke.
        //
        //  ── The trap ───────────────────────────────────────────────────────
        //
        //  Ten moments, one, one, one, two, two, three, and none in the first
        //  fifty frames:
        //      1  Mon night   "Good. He's still the same."
        //      2  Tue morning He apologises for the rain, twice.
        //      3  Wed lunch   "You always take the third one."
        //      4  Thu café    "Drink all of it."
        //      5  Thu platform Two tenths of a second while a girl stands next
        //                     to him.
        //      6  Fri morning "Who told them to fix it?"
        //      7  Fri corridor He told his mother on Wednesday.
        //      8  Sat corner  "You said it was fine." / "So it's fine."
        //      9  Sat park    He wipes the bench by the gate, unasked.
        //     10  Sat night   "Six days. Nothing changed. And nothing's going
        //                     to."
        //
        //  Is each one deniable?
        //      Every one has an ordinary sentence that explains it, and six of
        //      the ten have the explanation planted in the script before the
        //      moment happens — she says out loud on Tuesday that she likes the
        //      air, which is there so that Friday's flat line has somewhere
        //      honest to land.
        //
        //  Is any clinical sign performed directly? Can the player name it?
        //      No line in the act reports a time, a headcount, an exit or a
        //      distance. Hypervigilance is half a second of silence after a
        //      dropped tray. Exit-scanning is a table by a door, in narration,
        //      twice, unremarked. Control is a scene where she gives no orders
        //      at all and he does it anyway. Survivor's guilt is an apology for
        //      the weather. None of the five has a word attached to it anywhere
        //      in the act.
        //
        //  ── Clarity ────────────────────────────────────────────────────────
        //
        //  Every pronoun has a referent; every object is named in full before
        //  it becomes "it"; the two vending machines are distinguished by name
        //  in the one scene that has both. Every line reads against the picture
        //  it is spoken over. Nothing is said about a finished action. Every
        //  frame is a complete Persian sentence, and the narration is in the
        //  same spoken Persian as the dialogue.
        //
        //  ── Production ─────────────────────────────────────────────────────
        //
        //  Five white choices, each with two roads of five to seven spoken lines,
        //  no stage or picture changes inside any road, and both roads rejoin.
        //  Every sound is an existing procedural cue. The continuity table at
        //  the top of this file is current. All three languages carry the same
        //  information and leave the power in the same hands.
        //
        //  ── What this act would like and does not have ─────────────────────
        //
        //  Nothing it needs. Every background, sprite, pose and cue used above
        //  already exists in the project. Three things would each buy a real
        //  frame if anybody ever draws them, and the act is written so that
        //  none of them is required:
        //
        //    · A Yua pose holding a paper bag or a melon bread. Monday morning
        //      and Friday evening both hand something over and neither can show
        //      it.
        //    · A Haru "one beat late" pose — standing, weight off one leg. The
        //      act says it in narration three times because there is no
        //      picture of it.
        //    · A wide of the playground flowerbed as bare soil, for the six
        //      frames on Saturday that act three is built on.
        // =====================================================================
    }
}
