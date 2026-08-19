// -----------------------------------------------------------------------------
//  The Frayed Red String
//  Act01Strings.cs
//
//  Act one: "Cherry Blossom Mirage".
//
//  On the writing:
//  The design document sketches this act in a paragraph — the two of them meet
//  on the first day, become friends, go to a café, walk home, and Yua behaves
//  oddly in ways that are hard to catch. The lines below are that paragraph
//  performed rather than summarised, which means three rules held throughout:
//
//    1. Nobody explains the "-pi" suffix. They use it from the first exchange
//       and the narration declines to comment on it more than once. The player
//       is supposed to feel the wrongness and be unable to point at it.
//
//    2. Yua's cruelty is never in what she says, only in what she notices, how
//       long she looks, and how fast she gets what she wants. Every one of her
//       lines here would be affectionate coming from anyone else. That is the
//       whole trick of the act, and it is why she is given the prettiest voice
//       in the game to say them in.
//
//    3. Haru is written as warm, funny and slightly too prepared. His kindness
//       has to be likeable rather than pitiable, or the ending is not a tragedy,
//       just an unpleasant story about an unpleasant person.
//
//  Persian is written without zero-width non-joiners; see the note in
//  LocalizationDatabase for why.
// -----------------------------------------------------------------------------

using System.Collections.Generic;

namespace TheFrayedRedString.Localization
{
    /// <summary>Keys and text for act one.</summary>
    public static class Act01Strings
    {
        // --- Place captions ---------------------------------------------------

        public const string PlaceAlley = "act01.place.alley";
        public const string PlaceClassroom = "act01.place.classroom";
        public const string PlaceRooftop = "act01.place.rooftop";
        public const string PlaceCorridor = "act01.place.corridor";
        public const string PlaceCafe = "act01.place.cafe";
        public const string PlaceStreet = "act01.place.street";
        public const string PlacePlatform = "act01.place.platform";
        public const string PlaceClassroomRain = "act01.place.classroomRain";
        public const string PlaceCafeRain = "act01.place.cafeRain";
        public const string PlaceStreetNight = "act01.place.streetNight";

        // --- Scene 1: the path to the gate ------------------------------------

        public const string Alley01 = "act01.alley.01";
        public const string Alley02 = "act01.alley.02";
        public const string Alley03 = "act01.alley.03";
        public const string Alley04 = "act01.alley.04";
        public const string Alley05 = "act01.alley.05";
        public const string Alley06 = "act01.alley.06";
        public const string Alley07 = "act01.alley.07";
        public const string Alley08 = "act01.alley.08";
        public const string Alley09 = "act01.alley.09";
        public const string Alley10 = "act01.alley.10";
        public const string Alley11 = "act01.alley.11";
        public const string Alley12 = "act01.alley.12";

        // --- Scene 2: homeroom -------------------------------------------------

        public const string Class01 = "act01.class.01";
        public const string Class02 = "act01.class.02";
        public const string Class03 = "act01.class.03";
        public const string Class04 = "act01.class.04";
        public const string Class05 = "act01.class.05";
        public const string Class06 = "act01.class.06";
        public const string Class07 = "act01.class.07";
        public const string Class08 = "act01.class.08";
        public const string Class09 = "act01.class.09";
        public const string Class10 = "act01.class.10";
        public const string Class11 = "act01.class.11";
        public const string Class12 = "act01.class.12";
        public const string Class13 = "act01.class.13";
        public const string Class14 = "act01.class.14";
        public const string Class15 = "act01.class.15";

        // --- Scene 3: the roof --------------------------------------------------

        public const string Roof01 = "act01.roof.01";
        public const string Roof02 = "act01.roof.02";
        public const string Roof03 = "act01.roof.03";
        public const string Roof04 = "act01.roof.04";
        public const string Roof05 = "act01.roof.05";
        public const string Roof06 = "act01.roof.06";
        public const string Roof07 = "act01.roof.07";
        public const string Roof08 = "act01.roof.08";
        public const string Roof09 = "act01.roof.09";
        public const string Roof10 = "act01.roof.10";
        public const string Roof11 = "act01.roof.11";
        public const string Roof12 = "act01.roof.12";
        public const string Roof13 = "act01.roof.13";
        public const string Roof14 = "act01.roof.14";
        public const string Roof15 = "act01.roof.15";
        public const string Roof16 = "act01.roof.16";
        public const string Roof17 = "act01.roof.17";

        // --- Scene 4: the west corridor -----------------------------------------

        public const string Hall01 = "act01.hall.01";
        public const string Hall02 = "act01.hall.02";
        public const string Hall03 = "act01.hall.03";
        public const string Hall04 = "act01.hall.04";

        // --- Interlude: the machine room ----------------------------------------

        public const string Boiler01 = "act01.boiler.01";
        public const string Boiler02 = "act01.boiler.02";
        public const string Boiler03 = "act01.boiler.03";
        public const string Boiler04 = "act01.boiler.04";
        public const string Boiler05 = "act01.boiler.05";
        public const string Boiler06 = "act01.boiler.06";
        public const string Boiler07 = "act01.boiler.07";
        public const string Boiler08 = "act01.boiler.08";

        // --- Interlude: his leg --------------------------------------------------

        public const string Leg01 = "act01.leg.01";
        public const string Leg02 = "act01.leg.02";
        public const string Leg03 = "act01.leg.03";
        public const string Leg04 = "act01.leg.04";
        public const string Leg05 = "act01.leg.05";

        // --- Scene 5: the café ----------------------------------------------------

        public const string Cafe01 = "act01.cafe.01";
        public const string Cafe02 = "act01.cafe.02";
        public const string Cafe03 = "act01.cafe.03";
        public const string Cafe04 = "act01.cafe.04";
        public const string Cafe05 = "act01.cafe.05";
        public const string Cafe06 = "act01.cafe.06";
        public const string Cafe07 = "act01.cafe.07";
        public const string Cafe08 = "act01.cafe.08";
        public const string Cafe09 = "act01.cafe.09";
        public const string Cafe10 = "act01.cafe.10";
        public const string Cafe11 = "act01.cafe.11";
        public const string Cafe12 = "act01.cafe.12";
        public const string Cafe13 = "act01.cafe.13";
        public const string Cafe14 = "act01.cafe.14";
        public const string Cafe15 = "act01.cafe.15";
        public const string Cafe16 = "act01.cafe.16";
        public const string Cafe17 = "act01.cafe.17";
        public const string Cafe18 = "act01.cafe.18";
        public const string Cafe19 = "act01.cafe.19";

        // --- Scene 6: the road home and the platform --------------------------------

        public const string Street01 = "act01.street.01";
        public const string Street02 = "act01.street.02";
        public const string Street03 = "act01.street.03";
        public const string Street04 = "act01.street.04";
        public const string Street05 = "act01.street.05";

        public const string Platform01 = "act01.platform.01";
        public const string Platform02 = "act01.platform.02";
        public const string Platform03 = "act01.platform.03";
        public const string Platform04 = "act01.platform.04";
        public const string Platform05 = "act01.platform.05";
        public const string Platform06 = "act01.platform.06";

        // --- Scene 7: the rain ---------------------------------------------------------

        public const string Rain01 = "act01.rain.01";
        public const string Rain02 = "act01.rain.02";
        public const string Rain03 = "act01.rain.03";
        public const string Rain04 = "act01.rain.04";
        public const string Rain05 = "act01.rain.05";
        public const string Rain06 = "act01.rain.06";
        public const string Rain07 = "act01.rain.07";
        public const string Rain08 = "act01.rain.08";

        // --- Scene 8: the café, wet ------------------------------------------------------

        public const string Wet01 = "act01.wet.01";
        public const string Wet02 = "act01.wet.02";
        public const string Wet03 = "act01.wet.03";
        public const string Wet04 = "act01.wet.04";
        public const string Wet05 = "act01.wet.05";
        public const string Wet06 = "act01.wet.06";
        public const string Wet07 = "act01.wet.07";
        public const string Wet08 = "act01.wet.08";

        // --- Scene 9: the last lamp -----------------------------------------------------------

        public const string Night01 = "act01.night.01";
        public const string Night02 = "act01.night.02";
        public const string Night03 = "act01.night.03";
        public const string Night04 = "act01.night.04";
        public const string Night05 = "act01.night.05";
        public const string Night06 = "act01.night.06";
        public const string Night07 = "act01.night.07";
        public const string Night08 = "act01.night.08";
        public const string Night09 = "act01.night.09";
        public const string Night10 = "act01.night.10";
        public const string Night11 = "act01.night.11";
        public const string Night12 = "act01.night.12";
        public const string Night13 = "act01.night.13";
        public const string Night14 = "act01.night.14";
        public const string Night15 = "act01.night.15";

        /// <summary>Adds every act-one string to the table.</summary>
        public static void Register(Dictionary<string, LocEntry> table)
        {
            void Add(string key, string english, string japanese, string persian)
            {
                table[key] = new LocEntry(english, japanese, persian);
            }

            // -----------------------------------------------------------------
            //  Place captions
            // -----------------------------------------------------------------

            Add(PlaceAlley,
                "The Cherry Blossom Path",
                "桜並木の道",
                "کوچه شکوفه های گیلاس");

            Add(PlaceClassroom,
                "Class 1-A",
                "一年A組",
                "کلاس اول الف");

            Add(PlaceRooftop,
                "The Rooftop",
                "屋上",
                "پشت بام مدرسه");

            Add(PlaceCorridor,
                "West Corridor",
                "西廊下",
                "راهروی غربی");

            Add(PlaceCafe,
                "Café Kotori",
                "喫茶コトリ",
                "کافه کوتوری");

            Add(PlaceStreet,
                "Sakuradai Street",
                "桜台の通り",
                "خیابان ساکورادای");

            Add(PlacePlatform,
                "Sakuradai Station",
                "桜台駅",
                "ایستگاه ساکورادای");

            Add(PlaceClassroomRain,
                "Class 1-A, Thursday",
                "一年A組・木曜日",
                "کلاس اول الف، پنجشنبه");

            Add(PlaceCafeRain,
                "Café Kotori, Raining",
                "喫茶コトリ・雨",
                "کافه کوتوری، زیر باران");

            Add(PlaceStreetNight,
                "Sakuradai Street, After Dark",
                "桜台の通り・夜",
                "خیابان ساکورادای، بعد از تاریکی");

            // -----------------------------------------------------------------
            //  Scene 1 — the path to the gate
            // -----------------------------------------------------------------

            Add(Alley01,
                "The first morning of high school. The path up to the gate is nothing but cherry trees, and the wind is taking them apart one petal at a time.",
                "高校生活、最初の朝。校門までの道は桜ばかりで、風が花びらを一枚ずつほどいていく。",
                "اولین صبح دبیرستان. مسیر تا در مدرسه چیزی نیست جز درختان گیلاس، و باد دارد آنها را گلبرگ به گلبرگ از هم باز میکند.");

            Add(Alley02,
                "A new school. New faces. A clean page.",
                "新しい学校。新しい顔ぶれ。まっさらなページ。",
                "یک مدرسه تازه. آدمهای تازه. یک صفحه سفید.");

            Add(Alley03,
                "Though a page is only ever as clean as whoever gets to write on it.",
                "もっとも、ページがどれだけ綺麗かは、そこに書く人しだいだけど。",
                "هرچند یک صفحه فقط به اندازه کسی سفید میماند که اجازه دارد رویش بنویسد.");

            Add(Alley04,
                "There is a boy under the third tree, looking up into the branches like he is waiting for something specific to fall out of them.",
                "三本目の桜の下に男の子がいる。何か決まったものが落ちてくるのを待っているみたいに、枝を見上げている。",
                "پسری زیر سومین درخت ایستاده و طوری به شاخه ها نگاه میکند انگار منتظر است چیز مشخصی از بینشان بیفتد.");

            Add(Alley05,
                "Yua-pi. You're early.",
                "結愛ぴ。早いね。",
                "یوآ پی. زود اومدی.");

            Add(Alley06,
                "I'm not early. You're just always here first, Haru-pi.",
                "早くないよ。陽翔ぴがいつも先にいるだけでしょ。",
                "زود نیومدم. تو همیشه زودتر از منی، هارو پی.");

            Add(Alley07,
                "Neither of them stumbles over the other's name. It is the first day of school, and they have never met.",
                "二人とも、その呼び方に少しも引っかからない。今日は入学初日で、二人は初対面のはずだ。",
                "هیچکدامشان روی اسم آن یکی مکث نمیکند. امروز اولین روز مدرسه است و آنها تا حالا همدیگر را ندیده اند.");

            Add(Alley08,
                "Then we're both early. Come on — if we go now we can get the window seats.",
                "じゃあ二人とも早いってことで。ほら、今行けば窓際の席とれるよ。",
                "پس هردومون زود اومدیم. بیا، اگه الان بریم صندلی های کنار پنجره رو میگیریم.");

            Add(Alley09,
                "You say that like the window seats are a prize.",
                "窓際の席が賞品みたいな言い方するんだ。",
                "طوری میگی انگار صندلی کنار پنجره جایزه است.");

            Add(Alley10,
                "They are. Sakura on one side, nobody sitting behind you, and the teacher can't see your hands.",
                "賞品だよ。片側は桜、後ろには誰もいない、しかも先生から手元が見えない。",
                "خب هست دیگه. یه طرفش شکوفه است، پشت سرت کسی نیست، و معلم هم دستهات رو نمیبینه.");

            Add(Alley11,
                "...All right. That is a good seat.",
                "……うん。それはいい席だ。",
                "باشه. قبول. صندلی خوبیه.");

            Add(Alley12,
                "He has already worked out where I would want to sit. We have been talking for ninety seconds.",
                "この人はもう、わたしがどこに座りたいかを考え終えている。話し始めて九十秒しか経っていないのに。",
                "او از قبل حساب کرده که من دوست دارم کجا بنشینم. ما نود ثانیه است که داریم حرف میزنیم.");

            // -----------------------------------------------------------------
            //  Scene 2 — homeroom
            // -----------------------------------------------------------------

            Add(Class01,
                "Class 1-A smells like new tatami and floor polish. Somebody has already written their name on the board and spelled it wrong.",
                "一年A組は新しい畳とワックスの匂いがする。誰かがもう黒板に自分の名前を書いて、しかも間違えている。",
                "کلاس اول الف بوی حصیر نو و واکس کف میدهد. یک نفر از قبل اسمش را روی تخته نوشته، و غلط نوشته.");

            Add(Class02,
                "1-A. Both of us.",
                "一年A組。二人とも。",
                "اول الف. جفتمون.");

            Add(Class03,
                "Lucky.",
                "運がいいね。",
                "خوش شانسیم.");

            Add(Class04,
                "Statistically it's about one in nine, so — yes. Lucky.",
                "確率で言うと九分の一くらいだから、まあ……うん。運がいい。",
                "از نظر آماری حدود یک به نه است، پس آره. خوش شانسیم.");

            Add(Class05,
                "Did you work that out just now?",
                "それ、今その場で計算した?",
                "الان همینجا حسابش کردی؟");

            Add(Class06,
                "I worked it out yesterday.",
                "昨日のうちに計算してた。",
                "دیروز حسابش کردم.");

            Add(Class07,
                "He turns to sit down and catches his knee hard on the desk frame.",
                "座ろうとして振り向いた拍子に、机の脚に膝を強くぶつける。",
                "میچرخد که بنشیند و زانویش محکم به پایه میز میخورد.");

            Add(Class08,
                "— ah.",
                "——っ。",
                "آخ.");

            Add(Class09,
                "It is a small sound. It is over almost before it starts.",
                "小さな声だった。始まる前に終わってしまうくらいの。",
                "صدای کوچکی بود. تقریبا قبل از اینکه شروع شود تمام شد.");

            Add(Class10,
                "Yua watches him for one second longer than a person watches.",
                "結愛は、人が見るよりきっかり一秒だけ長く、その様子を見ている。",
                "یوآ دقیقا یک ثانیه بیشتر از آنچه آدمها نگاه میکنند، نگاهش میکند.");

            Add(Class11,
                "Careful.",
                "気をつけて。",
                "مواظب باش.");

            Add(Class12,
                "She says it kindly. She does not look away.",
                "優しい声だった。それでも、目はそらさない。",
                "با مهربانی میگوید. نگاهش را برنمیدارد.");

            Add(Class13,
                "I'm fine. I'm just — I'm clumsy on that side.",
                "平気平気。ただ、その……こっち側だけ、ちょっと不器用でさ。",
                "چیزی نیست. فقط... من از این طرف یه کم دست و پا چلفتیم.");

            Add(Class14,
                "I know.",
                "知ってる。",
                "میدونم.");

            Add(Class15,
                "Neither of them asks how.",
                "どうして知っているのか、どちらも訊かない。",
                "هیچکدامشان نمیپرسد از کجا.");

            // -----------------------------------------------------------------
            //  Scene 3 — the roof
            // -----------------------------------------------------------------

            Add(Roof01,
                "The roof is chainlink and potted plants and a bench somebody dragged up here years ago that nobody has ever taken down.",
                "屋上は金網と鉢植えと、何年も前に誰かが運び上げて、誰も下ろさなかったベンチでできている。",
                "پشت بام یعنی توری فلزی و گلدان و نیمکتی که سالها پیش یک نفر آورده بالا و هیچکس هیچوقت پایین نبرده.");

            Add(Roof02,
                "Can I tell you something stupid?",
                "ちょっと馬鹿なこと言ってもいい?",
                "میتونم یه چیز احمقانه بگم؟");

            Add(Roof03,
                "Please.",
                "どうぞ。",
                "بفرما.");

            Add(Roof04,
                "I want to get strong. Not — not sports strong.",
                "強くなりたいんだ。その、運動的な意味じゃなくて。",
                "میخوام قوی بشم. نه... نه قوی به معنی ورزشی.");

            Add(Roof05,
                "Strong how, then?",
                "じゃあ、どういう意味で?",
                "پس به چه معنی؟");

            Add(Roof06,
                "Strong enough that nobody standing near me gets hurt.",
                "そばにいる人が誰も傷つかないくらい、強く。",
                "اونقدر قوی که هیچکس نزدیک من آسیب نبینه.");

            Add(Roof07,
                "That's a heavy thing to want on your first week of school.",
                "入学して一週目に望むには、ずいぶん重たい願いだね。",
                "برای هفته اول مدرسه آرزوی سنگینیه.");

            Add(Roof08,
                "It isn't new. I decided it a long time ago.",
                "今思いついたわけじゃない。ずっと前に決めたことなんだ。",
                "تازه نیست. خیلی وقت پیش تصمیمش رو گرفتم.");

            Add(Roof09,
                "A long time ago.",
                "ずっと前に。",
                "خیلی وقت پیش.");

            Add(Roof10,
                "...I didn't ask when.",
                "……いつのことか、わたしは訊かなかった。",
                "من نپرسیدم کِی.");

            Add(Roof11,
                "She does not know why she didn't. It did not feel like a question she was allowed to have.",
                "どうして訊かなかったのか、自分でも分からない。訊いていい質問だという気がしなかった。",
                "خودش هم نمیداند چرا نپرسید. حس نکرد که آن سوال اجازه اش را دارد.");

            Add(Roof12,
                "Anyway. That was the stupid thing. Do you want half of this? My mother packs lunch like I'm two people.",
                "まあ、馬鹿な話はこれで終わり。これ半分いる? うちの母さん、二人分みたいに詰めるんだ。",
                "خب، همین بود چیز احمقانه ام. نصفش رو میخوای؟ مامانم جوری ناهار میبنده انگار من دو نفرم.");

            Add(Roof13,
                "Give me the egg one.",
                "卵のやつちょうだい。",
                "اون تخم مرغیه رو بده.");

            Add(Roof14,
                "You can't have the egg one.",
                "卵のやつはだめ。",
                "تخم مرغیه رو نمیتونی بگیری.");

            Add(Roof15,
                "Haru-pi.",
                "陽翔ぴ。",
                "هارو پی.");

            Add(Roof16,
                "...Take the egg one.",
                "……卵のやつ、どうぞ。",
                "بگیرش. تخم مرغیه مال تو.");

            Add(Roof17,
                "Four seconds. It took four seconds.",
                "四秒。四秒しかかからなかった。",
                "چهار ثانیه. فقط چهار ثانیه طول کشید.");

            // -----------------------------------------------------------------
            //  Scene 4 — the west corridor
            // -----------------------------------------------------------------

            Add(Hall01,
                "By the time the last bell goes, the west corridor has filled with the kind of low gold light that makes everything look like it already happened.",
                "最終下校のチャイムが鳴るころ、西廊下には、すべてがもう過ぎ去ったことのように見える低い金色の光が満ちている。",
                "وقتی زنگ آخر میخورد، راهروی غربی پر شده از آن نور طلایی کم ارتفاعی که همه چیز را شبیه چیزی میکند که قبلا اتفاق افتاده.");

            Add(Hall02,
                "Walk with me? Only if you're going that way.",
                "一緒に帰る? そっち方面ならでいいんだけど。",
                "با هم بریم؟ فقط اگه مسیرت همون طرفیه.");

            Add(Hall03,
                "You didn't have to add the second part.",
                "後半はいらなかったよ、それ。",
                "لازم نبود قسمت دومش رو بگی.");

            Add(Hall04,
                "They walk on. Neither of them mentions it again.",
                "二人は歩き出す。どちらも、もう二度とその話をしない。",
                "راه میافتند. هیچکدامشان دیگر حرفش را نمیزند.");

            // -----------------------------------------------------------------
            //  Interlude — the machine room
            //
            //  The document asks for this beat in every act: the sound arrives at
            //  random, Yua is frightened by it, she says she has been since
            //  childhood, Haru says he knows, and she does not ask how.
            //  The refusal to ask is the point, so the narration says so out loud
            //  once and then never again.
            // -----------------------------------------------------------------

            Add(Boiler01,
                "Somewhere under the floor a machine turns over and settles into a low, uneven hum.",
                "床下のどこかで機械が動き出し、低く不規則な唸りに落ち着く。",
                "جایی زیر کف، یک ماشین روشن میشود و روی یک همهمه کوتاه و ناموزون آرام میگیرد.");

            Add(Boiler02,
                "Yua stops in the middle of the corridor.",
                "結愛が廊下の真ん中で足を止める。",
                "یوآ وسط راهرو می ایستد.");

            Add(Boiler03,
                "I don't like that sound.",
                "……この音、苦手。",
                "از این صدا خوشم نمیاد.");

            Add(Boiler04,
                "The machine room. I've been scared of it since I was small. I don't know why I told you that.",
                "機械室。小さいころからずっと怖い。……なんで今それ言ったんだろう。",
                "موتورخانه. از بچگی ازش میترسم. نمیدونم چرا اینو بهت گفتم.");

            Add(Boiler05,
                "I know.",
                "知ってるよ。",
                "میدونم.");

            Add(Boiler06,
                "He does not say it like a guess. He says it like something he has been carrying for a while.",
                "推測を口にした声じゃなかった。ずっと抱えてきた事実を置いた、そういう声だった。",
                "طوری نمیگوید که انگار حدس زده. طوری میگوید که انگار مدتهاست دارد حملش میکند.");

            Add(Boiler07,
                "She should ask. She knows she should ask.",
                "訊くべきだ。訊くべきだと、自分でも分かっている。",
                "باید بپرسد. خودش هم میداند که باید بپرسد.");

            Add(Boiler08,
                "...Let's go.",
                "……行こう。",
                "بریم.");

            // -----------------------------------------------------------------
            //  Interlude — his leg
            // -----------------------------------------------------------------

            Add(Leg01,
                "Haru stops walking. He does not make any sound about it, which is somehow worse than if he had.",
                "陽翔が立ち止まる。声ひとつ上げない。上げてくれた方がまだましだった。",
                "هارو می ایستد. هیچ صدایی درنمی آورد، و همین یک جوری بدتر از این است که درمی آورد.");

            Add(Leg02,
                "Sorry. Give me a second — it does this sometimes.",
                "ごめん、ちょっとだけ待って。たまにこうなるんだ。",
                "ببخشید. یه لحظه وایسا. گاهی اینجوری میشه.");

            Add(Leg03,
                "He does not say what \"it\" is. She does not ask.",
                "「こう」が何なのか、彼は言わない。彼女も訊かない。",
                "نمیگوید «اینجوری» یعنی چه. او هم نمیپرسد.");

            Add(Leg04,
                "She waits. She always waits. It has never once occurred to her that waiting is not the same thing as asking.",
                "彼女は待つ。いつも待つ。待つことと訊くことは同じではない、と気づいたことは一度もない。",
                "منتظر میماند. همیشه منتظر میماند. حتی یک بار هم به ذهنش نرسیده که منتظر ماندن با پرسیدن یکی نیست.");

            Add(Leg05,
                "Okay. Good. Let's go.",
                "うん、大丈夫。行こう。",
                "خب. خوب شد. بریم.");

            // -----------------------------------------------------------------
            //  Scene 5 — the café
            // -----------------------------------------------------------------

            Add(Cafe01,
                "The café by the station is pink in a way that is clearly deliberate. There are fairy lights up in April.",
                "駅前の喫茶店は、明らかに狙ってピンクにしてある。四月なのに電飾が下がっている。",
                "کافه نزدیک ایستگاه به شکلی صورتی است که معلوم است عمدی بوده. توی فروردین هم چراغهای ریسه ای آویزان است.");

            Add(Cafe02,
                "Is this — is this a thing people do? On the first week?",
                "これって……こういうの、普通するもの? 一週目で。",
                "این... این کاریه که آدما میکنن؟ هفته اول؟");

            Add(Cafe03,
                "It's a thing we're doing.",
                "わたしたちがしてるよ、今。",
                "کاریه که ما داریم میکنیم.");

            Add(Cafe04,
                "That's not an answer.",
                "答えになってないよ、それ。",
                "این که جواب نشد.");

            Add(Cafe05,
                "It's the answer you're getting.",
                "あなたがもらえる答えはこれだけ。",
                "همین جوابیه که گیرت میاد.");

            Add(Cafe06,
                "She orders for both of them without looking at him.",
                "彼の方を見もせずに、二人分を注文する。",
                "بدون اینکه نگاهش کند برای هردویشان سفارش میدهد.");

            Add(Cafe07,
                "What arrives in front of Haru is the most bitter thing on the menu.",
                "陽翔の前に置かれたのは、メニューでいちばん苦いものだった。",
                "چیزی که جلوی هارو گذاشته میشود، تلخ ترین چیز منو است.");

            Add(Cafe08,
                "He drinks it.",
                "彼はそれを飲む。",
                "او مینوشدش.");

            Add(Cafe09,
                "It's good.",
                "おいしいよ。",
                "خوشمزه است.");

            Add(Cafe10,
                "Is it?",
                "本当に?",
                "جدی؟");

            Add(Cafe11,
                "...It's good.",
                "……おいしい。",
                "خوشمزه است.");

            Add(Cafe12,
                "It is not good. She read the menu. She knew precisely what she was ordering.",
                "おいしいわけがない。彼女はメニューを読んでいた。何を頼んだか、正確に分かっていた。",
                "خوشمزه نیست. او منو را خوانده بود. دقیقا میدانست دارد چه سفارش میدهد.");

            Add(Cafe13,
                "He would have said that about anything I put in front of him.",
                "わたしが差し出したものなら、この人は何にでもそう言っただろう。",
                "هرچیزی جلوش میگذاشتم همین رو میگفت.");

            Add(Cafe14,
                "Something in my chest lies down and goes quiet, and I decide not to look at what it was.",
                "胸の奥で何かが横になって、静かになる。それが何だったのかは、見ないことにする。",
                "چیزی توی سینه ام دراز میکشد و ساکت میشود، و تصمیم میگیرم نگاه نکنم که چه بود.");

            Add(Cafe15,
                "You've got that face on again.",
                "また、その顔してる。",
                "دوباره همون قیافه رو گرفتی.");

            Add(Cafe16,
                "What face.",
                "どの顔。",
                "کدوم قیافه.");

            Add(Cafe17,
                "The one where you're deciding something about me.",
                "僕について何か決めてるときの顔。",
                "همونی که وقتی داری درباره من یه تصمیمی میگیری میگیریش.");

            Add(Cafe18,
                "...Drink your coffee, Haru-pi.",
                "……コーヒー飲んで、陽翔ぴ。",
                "قهوه ات رو بخور، هارو پی.");

            Add(Cafe19,
                "He does. All of it.",
                "彼は飲む。最後の一滴まで。",
                "میخورد. تا آخرش.");

            // -----------------------------------------------------------------
            //  Scene 6 — the road home, and the platform
            // -----------------------------------------------------------------

            Add(Street01,
                "The road home runs past a vending machine that has been pink since before either of them was born.",
                "帰り道には、二人が生まれる前からずっとピンクのままの自販機がある。",
                "مسیر خانه از کنار دستگاه نوشیدنی ای رد میشود که از قبل از به دنیا آمدن هردویشان صورتی بوده.");

            Add(Street02,
                "Haru buys two drinks and hands her one without asking which she wants.",
                "陽翔は二本買って、どっちがいいかも訊かずに一本差し出す。",
                "هارو دو تا نوشیدنی میخرد و بدون اینکه بپرسد کدام را میخواهد، یکی را به او میدهد.");

            Add(Street03,
                "...How did you know I like this one.",
                "……なんでこれが好きだって知ってるの。",
                "از کجا میدونستی این یکی رو دوست دارم.");

            Add(Street04,
                "Lucky guess.",
                "当てずっぽう。",
                "شانسی حدس زدم.");

            Add(Street05,
                "It was not a guess.",
                "当てずっぽうではなかった。",
                "حدس نبود.");

            Add(Platform01,
                "The platform at this hour is gold and nearly empty. Two crows, one vending machine, a row of sakura leaning out over the tracks.",
                "この時間のホームは金色で、ほとんど無人だ。カラスが二羽、自販機がひとつ、線路の上に張り出した桜が一列。",
                "سکو در این ساعت طلایی است و تقریبا خالی. دو تا کلاغ، یک دستگاه نوشیدنی، و ردیفی از شکوفه های گیلاس که روی ریل خم شده اند.");

            Add(Platform02,
                "Same time tomorrow?",
                "明日も同じ時間?",
                "فردا هم همین ساعت؟");

            Add(Platform03,
                "Same time tomorrow.",
                "明日も同じ時間。",
                "فردا هم همین ساعت.");

            Add(Platform04,
                "The train takes him. She stays on the platform a little longer than she needs to.",
                "電車が彼を連れていく。彼女は必要よりも少しだけ長く、ホームに残る。",
                "قطار او را میبرد. یوآ کمی بیشتر از آنچه لازم است روی سکو میماند.");

            Add(Platform05,
                "That was easy.",
                "簡単だった。",
                "چقدر آسون بود.");

            Add(Platform06,
                "Nothing is ever that easy. Not for me.",
                "こんなに簡単なことなんて、ひとつもない。わたしには。",
                "هیچ چیز هیچوقت اینقدر آسان نیست. نه برای من.");

            // -----------------------------------------------------------------
            //  Scene 7 — the rain
            // -----------------------------------------------------------------

            Add(Rain01,
                "Days start folding into each other the way days do. Class, roof, café, station. Class, roof, café, station.",
                "日々が、日々のやり方で重なっていく。教室、屋上、喫茶店、駅。教室、屋上、喫茶店、駅。",
                "روزها شروع میکنند به تا خوردن روی هم، همانطور که روزها میکنند. کلاس، پشت بام، کافه، ایستگاه. کلاس، پشت بام، کافه، ایستگاه.");

            Add(Rain02,
                "On the Thursday it rains, and the classroom goes the colour of the inside of a shell.",
                "木曜に雨が降って、教室は貝殻の内側みたいな色になる。",
                "پنجشنبه باران میگیرد و کلاس به رنگ داخل یک صدف درمی آید.");

            Add(Rain03,
                "I brought an umbrella.",
                "傘、持ってきた。",
                "چتر آوردم.");

            Add(Rain04,
                "I can see the umbrella.",
                "傘は見えてる。",
                "چتر رو میبینم.");

            Add(Rain05,
                "I brought a big umbrella.",
                "大きい傘を持ってきた。",
                "چتر بزرگ آوردم.");

            Add(Rain06,
                "...You planned this on Monday, didn't you.",
                "……それ、月曜から決めてたでしょ。",
                "این نقشه رو دوشنبه کشیدی، نه؟");

            Add(Rain07,
                "I planned this on Monday.",
                "月曜から決めてた。",
                "دوشنبه کشیدم.");

            Add(Rain08,
                "She laughs, and she means it, and she notices that she means it, and files that away, and does not think about it again.",
                "彼女は笑う。本心から笑う。本心なのだと気づいて、それをしまい込み、二度と考えない。",
                "میخندد، و واقعا از ته دل میخندد، و متوجه میشود که از ته دل است، و آن را یک جایی بایگانی میکند و دیگر بهش فکر نمیکند.");

            // -----------------------------------------------------------------
            //  Scene 8 — the café, wet
            //
            //  The slip in Wet04 is the act's load-bearing line. It is the only
            //  moment either of them comes close to saying the thing the whole
            //  game is built on, and it is immediately taken back, and neither
            //  of them touches it.
            // -----------------------------------------------------------------

            Add(Wet01,
                "The rain comes down the café window in long threads. Everything in the room is softer than it was in April.",
                "喫茶店の窓を、雨が長い糸になって流れていく。部屋のすべてが四月より柔らかい。",
                "باران روی شیشه کافه به شکل نخهای بلند پایین می آید. همه چیز توی اتاق نرمتر از فروردین است.");

            Add(Wet02,
                "Yua-pi. Can I say a serious thing?",
                "結愛ぴ。真面目な話、していい?",
                "یوآ پی. میتونم یه چیز جدی بگم؟");

            Add(Wet03,
                "You've been building up to one for twenty minutes.",
                "二十分前からその助走してるでしょ。",
                "بیست دقیقه است داری برای گفتنش خیز برمیداری.");

            Add(Wet04,
                "I'm really glad we met again.",
                "また会えて、本当によかった。",
                "واقعا خوشحالم که دوباره همدیگه رو دیدیم.");

            Add(Wet05,
                "...Met. I'm glad we met.",
                "……会えて。会えてよかった、って意味。",
                "دیدیم. خوشحالم که همدیگه رو دیدیم.");

            Add(Wet06,
                "Neither of them corrects it out loud. For a moment neither of them looks at the other.",
                "どちらも、それを口に出して直さない。しばらくのあいだ、二人とも相手を見ない。",
                "هیچکدامشان با صدای بلند اصلاحش نمیکند. برای یک لحظه هیچکدام به آن یکی نگاه نمیکند.");

            Add(Wet07,
                "...Me too.",
                "……わたしも。",
                "منم همینطور.");

            Add(Wet08,
                "She says it in exactly the voice she used to order his coffee.",
                "彼のコーヒーを注文したときと、寸分たがわず同じ声だった。",
                "دقیقا با همان لحنی میگوید که قهوه اش را سفارش داد.");

            // -----------------------------------------------------------------
            //  Scene 9 — the last lamp
            // -----------------------------------------------------------------

            Add(Night01,
                "On the way home the streetlamps come on one at a time, like something counting.",
                "帰り道、街灯がひとつずつ点いていく。何かが数を数えているみたいに。",
                "توی راه خانه، چراغهای خیابان یکی یکی روشن میشوند، انگار چیزی دارد میشمارد.");

            Add(Night02,
                "Can I say another strange thing?",
                "もうひとつ変なこと言ってもいい?",
                "میتونم یه چیز عجیب دیگه بگم؟");

            Add(Night03,
                "That's four today.",
                "今日これで四つ目。",
                "این چهارمیه امروز.");

            Add(Night04,
                "It's five. You missed one.",
                "五つ目。ひとつ数え落としてる。",
                "پنجمیه. یکیش رو نشمردی.");

            Add(Night05,
                "I'd like to stay next to you. For a long time. Longer than is reasonable for someone I met last week.",
                "君のそばにいたい。ずっと。先週会ったばかりの相手に望むには、長すぎるくらいずっと。",
                "دلم میخواد کنارت بمونم. برای مدت طولانی. طولانی تر از چیزی که برای کسی که هفته پیش دیدمش منطقی باشه.");

            Add(Night06,
                "...That is strange.",
                "……それは変だ。",
                "این واقعا عجیبه.");

            Add(Night07,
                "Is that a no?",
                "それ、断り?",
                "یعنی نه؟");

            Add(Night08,
                "It isn't a no.",
                "断りじゃない。",
                "نه نیست.");

            Add(Night09,
                "He goes off down the hill with one hand half raised, the way he always does.",
                "彼は片手を半分だけ上げて坂を下っていく。いつもそうするみたいに。",
                "با یک دست که نیمه بالا برده، سرازیری را پایین میرود. همانطور که همیشه میکند.");

            Add(Night10,
                "Yua stands under the last lamp for a while.",
                "結愛はしばらく、最後の街灯の下に立っている。",
                "یوآ مدتی زیر آخرین چراغ می ایستد.");

            Add(Night11,
                "I want him.",
                "あの人が欲しい。",
                "من اونو میخوام.");

            Add(Night12,
                "Not near me. Not fond of me. Not glad to see me.",
                "そばにいてほしいんじゃない。好かれたいんじゃない。会えて嬉しいと思われたいんじゃない。",
                "نه نزدیک من. نه اینکه دوستم داشته باشه. نه اینکه از دیدنم خوشحال بشه.");

            Add(Night13,
                "Mine.",
                "わたしのもの。",
                "مال من.");

            Add(Night14,
                "...Anyway. Tomorrow. Same time.",
                "……ともかく。明日。同じ時間に。",
                "به هرحال. فردا. همین ساعت.");

            Add(Night15,
                "Somewhere between the two of them, too thin for either to see, something has already begun to fray.",
                "二人のあいだのどこかで、どちらの目にも映らないほど細い何かが、もうほつれはじめている。",
                "جایی بین آن دو، آنقدر نازک که هیچکدام نمیبینندش، چیزی از همین حالا شروع کرده به رشته رشته شدن.");
        }
    }
}
