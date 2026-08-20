// -----------------------------------------------------------------------------
//  The Frayed Red String
//  Act06Builder.cs  (Editor only)
//
//  Act six — Rotten Roots (腐った根) — written out as code.
//
//  Run it once from The Frayed Red String ▸ Build Act 06 From The Story
//  Document. It writes Assets/Story/Acts/Act06.asset, reusing the asset already
//  at that path if there is one, and then hands over to the Story Editor.
//
//  The flashback, and the only act the player does not hold. The design
//  document is exact: the screen goes black, they are thrown into the past, and
//  they have no control over anything — they are watching a film. So the first
//  beat here is BeginFilm and there is no matching EndFilm, because the act does
//  not give the story back. Every line holds for as long as it takes to read and
//  then goes by itself, clicking does nothing, and there is no continue arrow to
//  click at. Pausing still works, deliberately: a sequence this long that could
//  not be paused would be a hang, not a film.
//
//  Everything in this act is the eight-year-old cause of something the player
//  has already watched happen. Nothing in it is foreshadowing, because there is
//  nothing left to foreshadow — the point is recognition, not suspense, and the
//  writing should let the player get there a line before the scene does.
//
//  Three notes.
//
//  THE MACHINE ROOM. The document says no scene is shown directly and that is
//  the whole of how it is written here. The camera is outside the door with a
//  nine-year-old boy who cannot stand up, for as long as it takes, and it never
//  goes in. What happened is established afterwards, in one flat line Yua says
//  about herself, and never described. This is not squeamishness — it is that
//  the act's subject is the two children the room produced, and a game that
//  staged it would be about the room.
//
//  THE ART. None of the six backgrounds this act needs exist yet, and neither
//  do the child sprites. Both fall back to generated stand-ins, so the act plays
//  end to end today and its pacing can be judged; the names are already the ones
//  the finished files should have. See Backgrounds and CharacterArt.
//
//  THE ENDING. Special scene four in the document: the game cuts back and forth
//  between the day Yua fell over and the sound at the end of act five, and then
//  the song starts. It is written literally, as a real alternation of pictures
//  and cues with the gaps getting shorter, and it is the last thing that happens
//  before act seven.
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Narrative;
using UnityEditor;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Act six's script.</summary>
    public sealed class Act06Builder : ActScriptWriter
    {
        protected override int ActNumber => 6;

        protected override string AssetName => "Act06";

        protected override LocalizedLine Title =>
            L("Rotten Roots", "腐った根", "ریشه‌های پوسیده");

        [MenuItem("The Frayed Red String/Build Act 06 From The Story Document")]
        public static void Build()
        {
            new Act06Builder().BuildAsset();
        }

        /// <summary>Builds the act under a given policy, for the one-press setup.</summary>
        public static void Build(ActScriptWriter.RebuildPolicy policy)
        {
            new Act06Builder().BuildAsset(policy);
        }

        /// <summary>The act, in order.</summary>
        protected override void Write()
        {
            WriteFallingBack();
            WriteThePiName();
            WriteOrdinaryDays();
            WriteTheGarden();
            WriteTheCatch();
            WriteTheBackLane();
            WriteTheDoor();
            WriteWhatSheDecided();
            WriteTheSwitching();
        }

        // ---------------------------------------------------------------------
        //  1 · Black, and eight years
        // ---------------------------------------------------------------------

        /// <summary>
        /// The screen goes black and the player stops being able to do anything.
        /// </summary>
        /// <remarks>
        /// The mode is set before the first picture, so there is never a frame in
        /// which a click would have worked. A player who presses once out of
        /// habit and gets nothing has learnt the rule before the act has said a
        /// word.
        /// </remarks>
        private void WriteFallingBack()
        {
            BeginFilm();

            CutMusic();

            Hold(3.0f);

            Narrate(
                "Eight years earlier.",
                "八年前。",
                "هشت سال قبل.");

            Hold(3.0f);

            Place(
                Backgrounds.ElementaryYardDay,
                "The school yard", "校庭", "حیاط مدرسه");

            Hold(2.6f);

            Narrate(
                "There is a school with a low yard wall and a chain-link fence at the far end, and behind the fence a hill with houses on it.",
                "低い塀と、奥に金網のある校庭。その向こうには、家の建った丘がある。",
                "مدرسه‌ای هست با دیوارِ کوتاهِ حیاط و یک نرده‌ی سیمیِ آن‌طرف، و پشتِ نرده تپه‌ای با خانه‌هایی رویش.");

            Narrate(
                "Two children in this yard are nine years old, and one of them is going to be dead in eight years, and neither of them knows anything at all.",
                "この校庭にいる二人の子どもは九歳で、そのうち一人は八年後にはもういない。二人とも、何ひとつ知らない。",
                "دو بچه در این حیاط نُه ساله‌اند، و یکیشان هشت سالِ دیگر مرده است، و هیچ‌کدامشان هیچ چیزی نمی‌دانند.");

            Hold(3.4f);

            Enter(Speaker.YuaChild, Portrait.Joyful);
            Enter(Speaker.HaruChild, Portrait.Joyful);

            Hold(2.4f);
        }

        // ---------------------------------------------------------------------
        //  2 · Where the name comes from
        // ---------------------------------------------------------------------

        /// <summary>
        /// The first special scene: Yua invents the suffix.
        /// </summary>
        /// <remarks>
        /// The player has heard "Haru-pi" roughly two hundred times by now,
        /// starting on the first morning of act one, where it was the earliest
        /// sign that these two had not just met. This is where it comes from, and
        /// the reason it lands is that she explains it completely honestly — it
        /// is a nine-year-old's word for "you are the only person who has ever
        /// let me stay".
        /// </remarks>
        private void WriteThePiName()
        {
            Say(Speaker.YuaChild, Portrait.Neutral,
                "Haru-chan.",
                "ハルちゃん。",
                "هارو‌چان.");

            Say(Speaker.HaruChild, Portrait.Joyful,
                "Mm?",
                "なあに？",
                "هوم؟");

            Say(Speaker.YuaChild, Portrait.Unchanged,
                "Can I call you something else instead? From now on. Not chan. Pi.",
                "これから、別の呼び方してもいい？　ちゃんじゃなくて、ぴ。",
                "می‌شه از این به بعد یه چیزِ دیگه صدات کنم؟ چان نه. پی.");

            Hold(1.8f);

            Say(Speaker.HaruChild, Portrait.Neutral,
                "Pi? What does pi mean?",
                "ぴ？　ぴって、なに？",
                "پی؟ پی یعنی چی؟");

            Say(Speaker.YuaChild, Portrait.Shy,
                "I do not know. I made it up.",
                "……知らない。今つくった。",
                "نمی‌دونم. خودم درستش کردم.");

            Hold(2.0f);

            Say(Speaker.YuaChild, Portrait.Neutral,
                "When we talk to other people we say san, or chan. Everybody gets one of those.",
                "ほかの人と話すときは、さんとか、ちゃんって言うでしょ。みんなそれ。",
                "وقتی با بقیه صحبت می‌کنیم بهشون سان و چان می‌گیم. همه یکی از این‌ها رو دارن.");

            Say(Speaker.YuaChild, Portrait.Unchanged,
                "But you are not other people.",
                "でもハルちゃんは、ほかの人じゃない。",
                "اما تو واسه‌ی من بقیه نیستی.");

            Hold(2.2f);

            Say(Speaker.YuaChild, Portrait.Sad,
                "You are the only friend I have who is fine with me. So you should have a word nobody else has.",
                "私のこと、そのままでいいって言ってくれる友だちは、ハルちゃんだけ。だから、誰も持ってない呼び方がいい。",
                "تو تنها دوستی هستی که من رو قبولم داری. پس باید یه کلمه داشته باشی که هیچ‌کس دیگه نداره.");

            Say(Speaker.YuaChild, Portrait.Joyful,
                "I want to be special to you too. So — you are my pi.",
                "私もハルちゃんの特別になりたいの。だから、ハルぴ。私のぴ。",
                "می‌خوام واست خاص باشم. پس — تو پیِ من هستی!");

            Hold(2.6f);

            Say(Speaker.HaruChild, Portrait.Shy,
                "Then you are Yua-pi.",
                "じゃあ、結愛ぴだ。",
                "پس توام یوآ‌پی هستی.");

            Say(Speaker.YuaChild, Portrait.Joyful,
                "Obviously I am Yua-pi. That is how it works.",
                "当たり前でしょ。そういうものなの。",
                "معلومه که یوآ‌پی‌ام. قاعده‌ش همینه.");

            Hold(3.0f);

            Narrate(
                "They will still be doing this at seventeen, in a corridor, in front of people, and neither of them will ever have explained it to anybody.",
                "十七になっても、廊下で、人前で、二人はまだこう呼び合っている。誰にも説明したことはない。",
                "توی هفده‌سالگی هم هنوز همین کار را می‌کنند، توی راهرو، جلوی بقیه، و هیچ‌کدامشان هیچ‌وقت برای کسی توضیحش نداده.");

            Hold(3.0f);
        }

        // ---------------------------------------------------------------------
        //  3 · Several minutes of nothing happening
        // ---------------------------------------------------------------------

        /// <summary>
        /// The childhood the document asks for, and the reason the act works.
        /// </summary>
        /// <remarks>
        /// <para>
        /// "Several minutes of childish romance" is a genuine instruction and
        /// not padding. The last act the player watched ended with one of these
        /// two killing the other, and the flashback only means anything if they
        /// are made to sit in the ordinary version for long enough to forget
        /// they are waiting for something.
        /// </para>
        /// <para>
        /// So none of these scenes is about anything. That is the whole
        /// technique. The only things planted are the ones the player already
        /// knows the answer to.
        /// </para>
        /// </remarks>
        private void WriteOrdinaryDays()
        {
            ClearStage();

            Place(
                Backgrounds.ElementaryClassroomDay,
                "Class 3-2", "三年二組", "کلاس ۲-۳");

            Hold(2.2f);

            Narrate(
                "They were made to sit together in the third year because of the alphabet, and neither of them ever mentioned that this was luck.",
                "三年生のとき、名簿順で隣の席になった。それが運だったなんて、二人とも口にしたことはない。",
                "سالِ سوم به‌خاطرِ ترتیبِ حروف کنارِ هم نشاندنشان، و هیچ‌کدامشان هیچ‌وقت نگفت که این شانس بوده.");

            Enter(Speaker.YuaChild, Portrait.Neutral);
            Enter(Speaker.HaruChild, Portrait.Neutral);

            Say(Speaker.YuaChild, Portrait.Joyful,
                "You wrote the date wrong.",
                "日付、まちがってる。",
                "تاریخ رو اشتباه نوشتی.");

            Say(Speaker.HaruChild, Portrait.Shy,
                "I did not.",
                "まちがってないよ。",
                "اشتباه ننوشتم.");

            Say(Speaker.YuaChild, Portrait.Unchanged,
                "It is the fourteenth. You wrote the fourth. I am fixing it.",
                "十四日。ハルぴ、四日って書いてる。直してあげる。",
                "چهاردهمه. تو نوشتی چهارم. دارم درستش می‌کنم.");

            Say(Speaker.HaruChild, Portrait.Joyful,
                "You always fix it.",
                "結愛ぴ、いつも直すね。",
                "تو همیشه درستش می‌کنی.");

            Say(Speaker.YuaChild, Portrait.Joyful,
                "Because you always get it wrong.",
                "ハルぴがいつもまちがえるから。",
                "چون تو همیشه اشتباه می‌نویسی.");

            Hold(3.0f);

            ClearStage();

            Place(
                Backgrounds.ElementaryYardDay,
                "The yard, lunchtime", "校庭・昼休み", "حیاط، وقتِ ناهار");

            Hold(2.0f);

            Enter(Speaker.HaruChild, Portrait.Joyful);
            Enter(Speaker.YuaChild, Portrait.Joyful);

            Say(Speaker.HaruChild, Portrait.Unchanged,
                "If you could be any animal.",
                "もし、なんの動物にでもなれるとしたら。",
                "اگه می‌تونستی هر حیوونی باشی.");

            Say(Speaker.YuaChild, Portrait.Neutral,
                "A rabbit.",
                "うさぎ。",
                "خرگوش.");

            Say(Speaker.HaruChild, Portrait.Joyful,
                "You did not think about it.",
                "考えてないでしょ。",
                "بهش فکر نکردی.");

            Say(Speaker.YuaChild, Portrait.Unchanged,
                "I did not have to think about it. It is a rabbit. It has always been a rabbit.",
                "考えなくていいの。うさぎだから。ずっとうさぎだったから。",
                "لازم نبود فکر کنم. خرگوشه. همیشه خرگوش بوده.");

            Hold(2.0f);

            Say(Speaker.HaruChild, Portrait.Neutral,
                "Why a rabbit?",
                "どうしてうさぎ？",
                "چرا خرگوش؟");

            Say(Speaker.YuaChild, Portrait.Shy,
                "They are quiet, and people keep them.",
                "静かだし、……飼ってもらえるから。",
                "ساکتن، و آدم‌ها نگهشون می‌دارن.");

            Hold(3.2f);

            Narrate(
                "Somebody bought her one that winter. Its left ear is worn almost through now, in the one place a hand has held it for eight years.",
                "その冬、誰かが彼女にうさぎを買った。八年間おなじ場所を握られて、左耳はもうほとんど擦り切れている。",
                "همان زمستان یکی برایش خرید. حالا گوشِ چپش تقریباً ساییده شده؛ همان‌جایی که هشت سال یک دست نگهش داشته.");

            Hold(3.0f);

            ClearStage();

            Place(
                Backgrounds.RiverbankChildhoodDusk,
                "The riverbank", "川べり", "کنارِ رودخانه");

            Hold(2.2f);

            Enter(Speaker.YuaChild, Portrait.Neutral);
            Enter(Speaker.HaruChild, Portrait.Neutral);

            Narrate(
                "They took the long way home most days. It added twenty minutes and neither of them ever suggested the short one.",
                "たいてい遠回りで帰った。二十分よけいにかかる。短いほうを、どちらも一度も提案しなかった。",
                "بیشترِ روزها از راه دور برمی‌گشتند. بیست دقیقه بیشتر می‌شد و هیچ‌کدامشان هیچ‌وقت راهِ کوتاه را پیشنهاد نداد.");

            Say(Speaker.HaruChild, Portrait.Joyful,
                "When we are grown up we should live in the same house.",
                "大人になったら、おなじ家に住もうよ。",
                "وقتی بزرگ شدیم باید توی یه خونه زندگی کنیم.");

            Say(Speaker.YuaChild, Portrait.Unchanged,
                "Obviously.",
                "当たり前。",
                "معلومه.");

            Say(Speaker.HaruChild, Portrait.Unchanged,
                "You did not even think about it.",
                "これも考えてない。",
                "به این یکی هم فکر نکردی.");

            Say(Speaker.YuaChild, Portrait.Joyful,
                "Some things you do not have to think about, Haru-pi.",
                "考えなくていいこともあるの、ハルぴ。",
                "بعضی چیزها فکر کردن نمی‌خوان، هارو‌پی.");

            Hold(3.4f);
        }

        // ---------------------------------------------------------------------
        //  4 · The garden
        // ---------------------------------------------------------------------

        /// <summary>
        /// The second special scene, and the answer to a question act three
        /// asked and dropped.
        /// </summary>
        /// <remarks>
        /// In act three Yua tells Haru that seeing red spider lilies makes her
        /// eyes wet without her deciding to, and he says he knows, and neither of
        /// them says anything else. This is why. The whole of the promise the
        /// rest of the game is made of gets made here, by two nine-year-olds, in
        /// a flowerbed, completely sincerely.
        /// </remarks>
        private void WriteTheGarden()
        {
            ClearStage();

            Place(
                Backgrounds.SpiderLilyGardenDusk,
                "The garden", "あの庭", "باغچه");

            Hold(3.0f);

            Narrate(
                "There is a strip of ground behind the houses on the hill where somebody, years ago, planted red spider lilies and then stopped coming.",
                "丘の家々の裏に、細長い土地がある。何年も前に誰かが彼岸花を植えて、それきり来なくなった場所だ。",
                "پشتِ خانه‌های روی تپه نواری از زمین هست که سال‌ها پیش یکی در آن سوسنِ عنکبوتیِ قرمز کاشته و بعد دیگر نیامده.");

            Hold(2.4f);

            Enter(Speaker.YuaChild, Portrait.Crying);

            Narrate(
                "He saw her from the path. She was sitting down in the middle of them with her back to the road, and she was talking.",
                "道から見えた。彼女は花のまんなかに、道に背を向けて座って、何かを話していた。",
                "از روی مسیر دیدش. وسطِ گل‌ها نشسته بود، پشتش به جاده، و داشت حرف می‌زد.");

            Hold(3.0f);

            Narrate(
                "He did not go over. He went home and did not sleep, and the next morning she came to school exactly as she always did.",
                "彼は近づかなかった。家に帰って、眠れなかった。翌朝、彼女はいつもどおりに登校してきた。",
                "نزدیک نشد. رفت خانه و نخوابید، و صبح روز بعد یوآ دقیقاً مثل همیشه به مدرسه آمد.");

            Hold(2.6f);

            ClearStage();

            Place(
                Backgrounds.ElementaryHallwayDay,
                "The corridor", "廊下", "راهرو");

            Hold(2.0f);

            Enter(Speaker.YuaChild, Portrait.Joyful);
            Enter(Speaker.HaruChild, Portrait.Neutral);

            Say(Speaker.YuaChild, Portrait.Unchanged,
                "Morning. You look terrible. Did you stay up?",
                "おはよ。ひどい顔。夜ふかしした？",
                "صبح بخیر. قیافه‌ت داغونه. بیدار موندی؟");

            Hold(2.0f);

            Say(Speaker.HaruChild, Portrait.Sad,
                "Yua-pi. I saw you in the garden yesterday. You were crying.",
                "結愛ぴ。昨日、あの庭にいたよね。泣いてた。",
                "یوآ‌پی. دیروز توی باغچه دیدمت. داشتی گریه می‌کردی.");

            Hold(3.0f);

            Say(Speaker.YuaChild, Portrait.Joyful,
                "You are seeing things.",
                "見まちがいでしょ。",
                "اشتباه دیدی.");

            Say(Speaker.HaruChild, Portrait.Unchanged,
                "Tell me what happened.",
                "何があったのか、教えて。",
                "بهم بگو که چی شده.");

            Say(Speaker.YuaChild, Portrait.Neutral,
                "Nothing happened.",
                "何もないよ。",
                "هیچی نشده.");

            Hold(2.4f);

            Say(Speaker.HaruChild, Portrait.Sad,
                "It is fine to be strong in front of everybody else. You are allowed to do that.",
                "ほかのみんなの前では、強くていいよ。それはいいの。",
                "اشکال نداره جلوی بقیه قوی باشی. حقته که باشی.");

            Say(Speaker.HaruChild, Portrait.Unchanged,
                "But am I not your pi?",
                "でも、僕は結愛ぴのぴじゃないの？",
                "اما مگه من پیِ تو نیستم؟");

            Hold(3.0f);

            Say(Speaker.HaruChild, Portrait.Sad,
                "Talk to me.",
                "話してよ。",
                "باهام صحبت کن.");

            Hold(4.0f);

            Say(Speaker.YuaChild, Portrait.Crying,
                "…",
                "……",
                "…");

            Hold(2.6f);

            Say(Speaker.YuaChild, Portrait.Unchanged,
                "My house is not — it is not a good house. My mother is not well and my father is not there and when they are both in it I go out.",
                "うちは、……いい家じゃないの。お母さんは調子が悪くて、お父さんはいなくて、二人とも家にいる日は、外に出てる。",
                "خونه‌مون — خونه‌ی خوبی نیست. مادرم حالش خوب نیست و پدرم نیست و روزهایی که هر دوشون خونه‌ن، من می‌رم بیرون.");

            Say(Speaker.YuaChild, Portrait.Crying,
                "There is nowhere to go. So I go to the garden.",
                "行くところがないの。だから、あの庭に行く。",
                "جایی برای رفتن نیست. واسه همین می‌رم توی باغچه.");

            Hold(2.4f);

            Say(Speaker.YuaChild, Portrait.Unchanged,
                "I talk to it. I know that is stupid. It is the only thing that stays.",
                "話しかけてる。……ばかみたいだって分かってる。でも、あそこだけは、いなくならない。",
                "باهاش حرف می‌زنم. می‌دونم احمقانه‌ست. تنها چیزیه که می‌مونه.");

            Say(Speaker.YuaChild, Portrait.Crying,
                "And I cry on them. So really I am watering them. So it is not a waste.",
                "そこで泣くから、水をあげてることになる。だから、むだじゃない。",
                "و روشون گریه می‌کنم. پس در واقع دارم بهشون آب می‌دم. پس هدر نمی‌ره.");

            Hold(4.0f);

            Say(Speaker.HaruChild, Portrait.Crying,
                "Yua-pi.",
                "結愛ぴ。",
                "یوآ‌پی.");

            Say(Speaker.HaruChild, Portrait.Sad,
                "I am going to be beside you. Until the end of the world. Whatever happens.",
                "僕、ずっと結愛ぴのそばにいる。世界が終わるまで。何があっても。",
                "من کنارت می‌مونم. تا آخرِ دنیا. هر چی هم که بشه.");

            Say(Speaker.YuaChild, Portrait.Unchanged,
                "You cannot promise that. People say that.",
                "そんなの約束できないよ。みんな言うだけ。",
                "نمی‌تونی همچین قولی بدی. همه این رو می‌گن.");

            Say(Speaker.HaruChild, Portrait.Unchanged,
                "Then promise it back and we will both be lying, and then it will be even.",
                "じゃあ結愛ぴも約束して。二人とも嘘つきなら、おあいこ。",
                "پس تو هم بده، اون‌وقت هر دومون دروغ گفتیم و بی‌حساب می‌شیم.");

            Hold(2.4f);

            Say(Speaker.YuaChild, Portrait.Joyful,
                "…Fine.",
                "……いいよ。",
                "…باشه.");

            Hold(2.0f);

            Say(Speaker.YuaChild, Portrait.Neutral,
                "Until the end of the world. And afterwards we live in the same house.",
                "世界が終わるまで。そのあとは、おなじ家に住む。",
                "تا آخرِ دنیا. و بعدش توی یه خونه زندگی می‌کنیم.");

            Say(Speaker.HaruChild, Portrait.Joyful,
                "And we will be good ones. Good parents. Not like — just good ones.",
                "それで、ちゃんとした親になる。……ちゃんとした親に。",
                "و آدم‌های خوبی می‌شیم. پدر و مادرِ خوبی. نه مثلِ — فقط خوب.");

            Say(Speaker.YuaChild, Portrait.Crying,
                "Good ones.",
                "……ちゃんとした親に。",
                "خوب.");

            Hold(3.0f);

            Narrate(
                "And that whoever else came and went, the two of them would be the place the other one could go. They were nine. They meant every word of it.",
                "ほかの誰がいなくなっても、二人はおたがいの行き場所でいる。九歳だった。二人とも、本気だった。",
                "و هر کسِ دیگری که آمد و رفت، آن دو جایی باشند که آن یکی می‌تواند برود. نُه ساله بودند. تک‌تکِ کلماتش را جدی می‌گفتند.");

            Hold(4.0f);
        }

        // ---------------------------------------------------------------------
        //  5 · The day she fell over
        // ---------------------------------------------------------------------

        /// <summary>
        /// The third special scene, and the shape act seven ends on.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Deliberately trivial. She trips, he throws himself under her, and the
        /// document cuts before the landing. It is the smallest event in the act
        /// and it is the one the game closes with — act seven is Haru falling
        /// into her arms and it is meant to read as this picture, mirrored, eight
        /// years late.
        /// </para>
        /// <para>
        /// It happens before the machine room on purpose. What the player has to
        /// see is that catching her was already what he did, on an ordinary
        /// Tuesday, for no reason and at no cost — long before there was anything
        /// to be guilty about.
        /// </para>
        /// </remarks>
        private void WriteTheCatch()
        {
            ClearStage();

            Place(
                Backgrounds.ElementaryYardDay,
                "An ordinary day", "なんでもない日", "یک روزِ عادی");

            Hold(2.2f);

            Enter(Speaker.YuaChild, Portrait.Joyful);
            Enter(Speaker.HaruChild, Portrait.Joyful);

            Narrate(
                "Nothing about this afternoon is worth writing down. That is the point of it.",
                "この午後には、書き留めるようなことは何もない。それがこの午後だ。",
                "هیچ چیزِ این بعدازظهر ارزشِ نوشتن ندارد. نکته‌اش هم همین است.");

            Say(Speaker.YuaChild, Portrait.Joyful,
                "You cannot catch me.",
                "つかまえられないよ。",
                "نمی‌تونی بگیریم.");

            Say(Speaker.HaruChild, Portrait.Joyful,
                "I can always catch you.",
                "いつだってつかまえられる。",
                "همیشه می‌تونم بگیرمت.");

            Hold(1.6f);

            Narrate(
                "She went over the raised edge of the path at a run, the way children do, all at once and with her hands nowhere near ready.",
                "彼女は走ったまま段差につまずいた。子どもらしく、いっぺんに、手はまるで間に合わないかたちで。",
                "با دو از لبه‌ی بلندِ مسیر رد شد، همان‌طور که بچه‌ها می‌شوند؛ یک‌دفعه، و دست‌هایش اصلاً آماده نبود.");

            Hold(1.4f);

            SayWithSound(Speaker.HaruChild, Portrait.Sad, SfxId.Heartbeat, 0.65f,
                "Yua-pi — ",
                "結愛ぴ——",
                "یوآ‌پی —");

            Narrate(
                "He did not think about it either. He got there and put his arms out and put himself underneath her.",
                "彼も考えなかった。走って、腕を広げて、自分を下に入れた。",
                "او هم فکر نکرد. رسید و دست‌هایش را باز کرد و خودش را انداخت زیرش.");

            // The document cuts before the landing, and so does this.
            Hold(3.4f);

            ClearStage();

            Hold(2.6f);

            Narrate(
                "She had a grazed elbow. He had a bruise across his back for a fortnight and told her it did not hurt, which was the first lie he ever told her.",
                "彼女はひじを擦りむいた。彼は背中に二週間分のあざができて、痛くないと言った。それが、彼が彼女についた最初の嘘だった。",
                "آرنجش خراش برداشت. کمرِ او دو هفته کبود ماند و به یوآ گفت درد نمی‌کند، و این اولین دروغی بود که در عمرش به او گفت.");

            Hold(3.6f);
        }

        // ---------------------------------------------------------------------
        //  6 · The road they took her down
        // ---------------------------------------------------------------------

        /// <summary>
        /// The last few minutes before it, written as ordinary.
        /// </summary>
        private void WriteTheBackLane()
        {
            Place(
                Backgrounds.BackLaneDusk,
                "The way home", "帰り道", "راه خانه");

            Hold(2.6f);

            Narrate(
                "The long way, on a Thursday in the autumn, the same as every other day that year.",
                "秋の木曜日、いつもの遠回り。その年のほかの日と、何も変わらない。",
                "راه دور، یک پنجشنبه‌ی پاییزی، مثلِ هر روزِ دیگرِ آن سال.");

            Enter(Speaker.YuaChild, Portrait.Neutral);

            Narrate(
                "She was ahead of him because she was always ahead of him, by about the distance of an argument about who was ahead.",
                "彼女は先を歩いていた。いつもそうだ。どっちが先かで言い合いになるくらいの、その距離だけ。",
                "جلوترش بود چون همیشه جلوترش بود؛ به‌اندازه‌ی همان بحثی که سرِ جلوتر بودن می‌کردند.");

            Hold(2.4f);

            Narrate(
                "Three older boys came out of the lane by the allotments. It took less than a minute and nobody shouted.",
                "畑ぞいの路地から、年上の男子が三人出てきた。一分もかからなかったし、誰も叫ばなかった。",
                "سه پسرِ بزرگ‌تر از کوچه‌ی کنارِ زمین‌ها بیرون آمدند. کمتر از یک دقیقه طول کشید و هیچ‌کس داد نزد.");

            Hold(3.0f);

            Exit(Speaker.YuaChild);

            Hold(2.4f);

            Enter(Speaker.HaruChild, Portrait.Sad);

            Narrate(
                "He was forty metres back. He saw which way they went, and he went that way.",
                "彼は四十メートル後ろにいた。どっちへ行ったかを見て、そっちへ走った。",
                "چهل متر عقب‌تر بود. دید از کدام طرف رفتند، و از همان طرف رفت.");

            Hold(3.0f);
        }

        // ---------------------------------------------------------------------
        //  7 · Outside the door
        // ---------------------------------------------------------------------

        /// <summary>
        /// The machine room, from outside it, for as long as it takes.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The camera does not go in and the writing does not describe anything
        /// that happens in there. What is staged is a boy on the ground on the
        /// wrong side of a door, and what the player is given is the sound the
        /// whole game has been flinching at since act one, in the place it comes
        /// from.
        /// </para>
        /// <para>
        /// The long holds are the scene. Nothing else in this act is paced like
        /// this, and in a mode where the player cannot press anything to make it
        /// go faster, four seconds is a very long time — which is the only honest
        /// way to put them where he was.
        /// </para>
        /// </remarks>
        private void WriteTheDoor()
        {
            ClearStage();

            Place(
                Backgrounds.MachineRoomDoorDusk,
                "The machine room", "機械室", "موتورخانه");

            Hold(2.6f);

            Narrate(
                "A low brick building behind the allotments that had not been used for anything in years. A steel door, and a sound underneath it.",
                "畑の裏の、何年も使われていない低いレンガの建物。鉄の扉と、その下から響く音。",
                "ساختمانِ آجریِ کوتاهی پشتِ زمین‌ها که سال‌ها بود به هیچ کاری نمی‌آمد. دری فلزی، و صدایی از زیرش.");

            Cue(SfxId.BoilerRoom, 0.70f);

            Hold(4.0f);

            Enter(Speaker.HaruChild, Portrait.Sad);

            Narrate(
                "He got to the door. One of them turned round and there were three of them and he was nine.",
                "扉までは行けた。ひとりが振り返って、向こうは三人で、彼は九歳だった。",
                "به در رسید. یکیشان برگشت و آن‌ها سه نفر بودند و او نُه ساله بود.");

            Hold(2.6f);

            SayWithSound(Speaker.HaruChild, Portrait.Crying, SfxId.LegAche, 1.0f,
                "…",
                "……",
                "…");

            Hold(3.0f);

            Narrate(
                "They broke his leg below the knee, and then they went inside and shut the door, and he could not stand up.",
                "膝の下で脚を折られた。そして三人は中に入り、扉を閉めた。彼は立ち上がれなかった。",
                "پایش را زیرِ زانو شکستند، و بعد رفتند تو و در را بستند، و او نتوانست بلند شود.");

            Hold(4.0f);

            Narrate(
                "He got as far as the door.",
                "扉のところまでは、行けた。",
                "تا خودِ در رسید.");

            Hold(4.0f);

            Narrate(
                "That is where he stayed.",
                "そこから、動けなかった。",
                "و همان‌جا ماند.");

            Hold(4.6f);

            Cue(SfxId.BoilerRoom, 0.55f);

            Hold(4.6f);

            Narrate(
                "Nothing about what happened on the other side of that door is shown here, and nothing about it is described. It is not the subject of this act and it is not going to be made into one.",
                "その扉の向こうで起きたことは、ここでは何も映さないし、何も書かない。この幕の主題ではないし、主題にするつもりもない。",
                "هیچ چیزی از آنچه آن‌طرفِ آن در گذشت اینجا نشان داده نمی‌شود و هیچ چیزی از آن توصیف نمی‌شود. موضوعِ این پرده نیست و قرار هم نیست بشود.");

            Hold(4.0f);

            Narrate(
                "What matters here is the two children it produced, and they are both still outside in about four minutes.",
                "ここで大事なのは、それがつくった二人の子どもだ。四分ほどして、二人とも外にいる。",
                "چیزی که اینجا اهمیت دارد آن دو بچه‌ای است که این اتفاق ساخت، و هر دو حدودِ چهار دقیقه‌ی دیگر بیرونند.");

            Hold(4.0f);

            Narrate(
                "The three of them came out and walked off up the lane, and one of them was laughing about something else entirely.",
                "三人は出てきて、路地を歩いていった。そのうちひとりは、まるで別のことで笑っていた。",
                "آن سه بیرون آمدند و از کوچه بالا رفتند، و یکیشان داشت به چیزِ کاملاً دیگری می‌خندید.");

            Hold(4.0f);
        }

        // ---------------------------------------------------------------------
        //  8 · What she decided, and what he promised
        // ---------------------------------------------------------------------

        /// <summary>
        /// Two sentences that the previous five acts were the consequence of.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Hers is the design document's, almost word for word, and the way to
        /// play it is flat — not crying, not angry. She has worked something out
        /// and is reporting it. That flatness is the same face she wears in act
        /// two when she takes a blue button away from the player, eight years
        /// later, and it should be recognisable as the same face.
        /// </para>
        /// <para>
        /// His is the other half of the game: if you become a monster I will stop
        /// you myself. He kept it. That is what act five was.
        /// </para>
        /// </remarks>
        private void WriteWhatSheDecided()
        {
            Enter(Speaker.YuaChild, Portrait.DeadEyes);

            Hold(3.4f);

            Narrate(
                "She came out last. She sat down on the step next to him, because he could not get up, and she straightened her skirt first.",
                "彼女は最後に出てきた。彼が立てないので、隣の段に座った。座る前に、スカートを直した。",
                "آخر از همه بیرون آمد. کنارِ او روی پله نشست، چون او نمی‌توانست بلند شود، و اول دامنش را صاف کرد.");

            Hold(3.6f);

            Say(Speaker.YuaChild, Portrait.Unchanged,
                "This is what being weak gets you.",
                "弱いって、こういうことなんだね。",
                "نتیجه‌ی ضعیف بودن این هست.");

            Hold(3.0f);

            Say(Speaker.YuaChild, Portrait.Unchanged,
                "I am tired. My house. And now this.",
                "疲れた。うちのことも。それで、これも。",
                "خسته شدم. اون از خانواده‌م. این هم از اینجا.");

            Hold(2.6f);

            Say(Speaker.YuaChild, Portrait.Unchanged,
                "I am going to get strong enough that nobody can do anything to me. Not anybody. Ever.",
                "誰にも何もできないくらい、強くなる。誰にも。二度と。",
                "اون‌قدری قوی می‌شم که هیچ‌کس نتونه به من آسیب برسونه. هیچ‌کس. هیچ‌وقت.");

            Say(Speaker.YuaChild, Portrait.DeadEyes,
                "And I am going to pay it back.",
                "それで、返す。",
                "و تلافی می‌کنم.");

            Hold(4.0f);

            Say(Speaker.HaruChild, Portrait.Crying,
                "Yua-pi.",
                "結愛ぴ。",
                "یوآ‌پی.");

            Hold(2.4f);

            Say(Speaker.HaruChild, Portrait.Sad,
                "If you turn into a monster, I will stop you myself.",
                "もし結愛ぴが化けものになったら、僕が止める。自分で。",
                "اگه هیولا بشی، خودم جلوت رو می‌گیرم.");

            Hold(3.6f);

            Say(Speaker.YuaChild, Portrait.Neutral,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(3.0f);

            Say(Speaker.YuaChild, Portrait.Unchanged,
                "You promise.",
                "約束ね。",
                "قول دادی.");

            Say(Speaker.HaruChild, Portrait.Unchanged,
                "I promise.",
                "約束する。",
                "قول می‌دم.");

            Hold(4.0f);

            Narrate(
                "Two promises got made at that door. He kept both of them, eight years apart, and the second one is why there is nobody left to tell.",
                "あの扉の前で、約束は二つ結ばれた。彼は八年をはさんで、どちらも守った。二つめのせいで、もう伝える相手がいない。",
                "جلوی آن در دو قول داده شد. او هر دو را نگه داشت، به فاصله‌ی هشت سال، و دومی همان دلیلی است که دیگر کسی نمانده که به او بگویند.");

            Hold(4.6f);
        }

        // ---------------------------------------------------------------------
        //  9 · Special scene four
        // ---------------------------------------------------------------------

        /// <summary>
        /// The game cuts between the day she fell over and the sound at the end
        /// of act five, faster and faster, and then the song starts.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Written as a literal alternation, exactly as the document has it. Two
        /// pictures, back and forth, with the holds getting shorter each time —
        /// which is a thing that only works because the player cannot click:
        /// every cut lands on the frame it was written to land on, for everybody,
        /// every time.
        /// </para>
        /// <para>
        /// The two images are the same image. A boy putting his arms out and
        /// throwing himself underneath somebody, and a boy opening one hand. The
        /// act does not point at that and must not.
        /// </para>
        /// </remarks>
        private void WriteTheSwitching()
        {
            ClearStage();

            CutMusic();

            Hold(3.0f);

            // Four passes, tightening.
            Place(
                Backgrounds.ElementaryYardDay,
                "An ordinary day", "なんでもない日", "یک روزِ عادی");

            Hold(2.8f);

            CutTo(Backgrounds.AlleywayNight);

            Cue(SfxId.Gunshot, 0.55f);

            Hold(2.2f);

            CutTo(Backgrounds.ElementaryYardDay);

            Hold(1.8f);

            CutTo(Backgrounds.AlleywayNight);

            Cue(SfxId.Gunshot, 0.70f);

            Hold(1.4f);

            CutTo(Backgrounds.ElementaryYardDay);

            Hold(1.1f);

            CutTo(Backgrounds.AlleywayNight);

            Cue(SfxId.Gunshot, 0.85f);

            Hold(0.9f);

            CutTo(Backgrounds.ElementaryYardDay);

            Hold(0.8f);

            Narrate(
                "He caught her. He caught her every single time, for eight years, and then once he could not.",
                "彼は受け止めた。八年間、一度残らず受け止めて、そして一度だけ、受け止められなかった。",
                "گرفتش. هشت سال، هر بار گرفتش، و بعد یک بار نتوانست.");

            Hold(3.6f);

            ClearStage();

            Hold(2.4f);

            // And the song, which is act seven.
            SetMusic(MusicTracks.ForAct(7));

            Hold(4.0f);
        }
    }
}
