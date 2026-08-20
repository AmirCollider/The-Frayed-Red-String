// -----------------------------------------------------------------------------
//  The Frayed Red String
//  Act04Builder.cs  (Editor only)
//
//  Act four — Deepening (深まり) — written out as code.
//
//  Run it once from The Frayed Red String ▸ Build Act 04 From The Story
//  Document. It writes Assets/Story/Acts/Act04.asset, reusing the asset already
//  at that path if there is one, and then hands over to the Story Editor.
//
//  Act three was the quiet one and said so. This is the one where the quiet is
//  used. Three things happen in it and nothing else does:
//
//    • Haru turns round. Walking home alone from a date, he stops in the middle
//      of an empty street and speaks to the player — the first and only time he
//      does. He is not angry and he is not asking to be rescued; he asks the
//      player to look after Yua, and then tells them, without knowing he is
//      telling them anything, exactly how his own life went wrong. Yua taught
//      the player what it means for a character to look at them, in act two, and
//      she lied the whole way through it. He does not.
//
//    • Yua learns the dead friend's name, and stops being careful with it. The
//      design document puts the whole of act four in one sentence — she works to
//      make him believe he is responsible for the suicide — and the work is
//      slow, kind-sounding and almost entirely made of questions. She never once
//      says the cruel thing. She asks until he says it himself.
//
//    • The promise from act two comes back inside out. Haru asked her to come to
//      him and tell him when she was unhappy. In this act that sentence is
//      turned around and pointed at him: the friend did not come, so the friend
//      did not trust him, so he was not enough. It is the same sentence. That is
//      what makes it work on him.
//
//  The blue and green buttons are here from the first scene, and Yua overrules
//  every blue one exactly as she has since act two. Both of the game's silences
//  are here too — the leg, and the machine room through a floor — and both are
//  counted, because the endings this act is walking towards are decided by who
//  sat with them.
//
//  The verbs the script below is written in — Say, Narrate, Hold, Place, Decide,
//  Listen — are in ActScriptWriter, along with everything about writing the
//  asset.
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Narrative;
using UnityEditor;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Act four's script.</summary>
    public sealed class Act04Builder : ActScriptWriter
    {
        protected override int ActNumber => 4;

        protected override string AssetName => "Act04";

        protected override LocalizedLine Title => L("Deepening", "深まり", "عمیق شدن");

        [MenuItem("The Frayed Red String/Build Act 04 From The Story Document")]
        public static void Build()
        {
            new Act04Builder().BuildAsset();
        }

        /// <summary>Builds the act under a given policy, for the one-press setup.</summary>
        public static void Build(ActScriptWriter.RebuildPolicy policy)
        {
            new Act04Builder().BuildAsset(policy);
        }

        /// <summary>
        /// The act, in order.
        /// </summary>
        /// <remarks>
        /// Haru's address to the player is first and is not interrupted. It is
        /// the only scene in the game that belongs to him alone, and putting a
        /// choice or a random interlude anywhere inside it would hand it back to
        /// somebody else halfway through.
        /// </remarks>
        protected override void Write()
        {
            WriteHaruAlone();
            WriteClassroomAfter();
            WriteCafeTheName();
            WriteRooftopPromise();
            WriteWayHomeLeg();
            WriteBakeryStreet();
            WritePlaygroundGuilt();
            WriteYuaRoom();
        }

        // ---------------------------------------------------------------------
        //  1 · The street, and the only scene that is his
        // ---------------------------------------------------------------------

        /// <summary>
        /// Haru walks home alone, stops, and looks at the player.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Written as one unbroken run of short lines with long silences between
        /// them. He is not making a speech; he is a very tired boy saying
        /// something he has decided to say and getting through it a sentence at
        /// a time.
        /// </para>
        /// <para>
        /// No music change, no sound cue, no choice. The scene has to feel like
        /// the game forgot to do anything, because that is what it feels like
        /// when somebody stops walking and turns round.
        /// </para>
        /// </remarks>
        private void WriteHaruAlone()
        {
            Place(
                Backgrounds.VendingStreetNight,
                "The way home, alone", "帰り道・ひとり", "راه خانه، تنها");

            Hold(2.6f);

            Narrate(
                "November. They had walked as far as her corner and she had gone in, and the rest of the road was his.",
                "十一月。彼女の角まで一緒に歩いて、彼女は入っていった。そこから先の道は、彼のものだった。",
                "آبان. تا سرِ کوچه‌ی او با هم رفتند و او رفت تو، و باقیِ راه مالِ هارو بود.");

            Narrate(
                "The vending machine was the brightest thing on the street, the way it always is, and nothing else was moving at all.",
                "自販機が通りでいちばん明るい。いつもそうだ。ほかには、何ひとつ動いていない。",
                "دستگاه نوشیدنی روشن‌ترین چیزِ خیابان بود، مثل همیشه، و هیچ چیزِ دیگری تکان نمی‌خورد.");

            Enter(Speaker.Haru, Portrait.Neutral);

            Hold(2.4f);

            Narrate(
                "He walked four more steps. Then he stopped, and stood still for long enough that it stopped looking like a pause.",
                "四歩、歩いた。それから立ち止まって、間と呼ぶには長すぎるあいだ、動かなかった。",
                "چهار قدم دیگر رفت. بعد ایستاد، و آن‌قدر بی‌حرکت ماند که دیگر شبیه مکث نبود.");

            Hold(3.4f);

            Narrate(
                "And then he turned round, and looked at nothing, and kept looking at it.",
                "そして振り返り、何もないほうを見て、そのまま見つづけた。",
                "و بعد برگشت، به هیچ نگاه کرد، و همان‌طور نگاه کرد.");

            Hold(3.0f);

            Say(Speaker.Haru, Portrait.Neutral,
                "I know you are watching our lives.",
                "君が僕たちの生活を見てるの、知ってる。",
                "می‌دونم که داری زندگی مارو نگاه می‌کنی.");

            Hold(2.6f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "I do not know how long. I do not know whether Yua-pi knows as well.",
                "いつからかは分からない。結愛ぴも知ってるのかどうかも、分からない。",
                "نمی‌دونم از کی. نمی‌دونم یوآ‌پی هم از این خبر داره یا نه.");

            Say(Speaker.Haru, Portrait.Shy,
                "I am not accusing you of anything. I do not think you meant any harm.",
                "責めてるんじゃないよ。悪気があったとは思ってない。",
                "دارم متهمت نمی‌کنم. فکر نمی‌کنم قصد بدی داشته باشی.");

            Hold(2.0f);

            Say(Speaker.Haru, Portrait.Neutral,
                "I only want to ask you something. It is the only thing I will ever ask you.",
                "ひとつだけ、お願いがあるんだ。君に頼むのは、これきりにする。",
                "فقط می‌خوام یه چیزی ازت بخوام. تنها چیزیه که تا آخرش ازت می‌خوام.");

            Hold(2.2f);

            Say(Speaker.Haru, Portrait.Sad,
                "Look after us. Please.",
                "僕たちのこと、見ていて。……お願い。",
                "مراقب ما باش. خواهش می‌کنم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Look after Yua-pi more.",
                "結愛ぴのことは、もっと。",
                "مراقب یوآ‌پی بیشتر.");

            Hold(2.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Do not let her be unhappy. Do not let this go the way that hurts her at the end of it.",
                "あの子を悲しませないで。最後にあの子が傷つくほうへ、進ませないで。",
                "نذار ناراحت باشه. نذار چیزی طبق چیزی که آخرش اذیتش می‌کنه پیش بره.");

            Hold(3.0f);

            Narrate(
                "There is a sentence coming that he has said to one other person in his life. He said it every day for six years, and it did not work.",
                "これから彼が言う言葉を、彼は人生でもうひとりにだけ言ったことがある。六年間、毎日言って、それは効かなかった。",
                "جمله‌ای در راه است که او در تمام عمرش فقط به یک نفرِ دیگر گفته. شش سال، هر روز گفت، و کار نکرد.");

            Hold(2.6f);

            Say(Speaker.Haru, Portrait.Neutral,
                "And — this part is for you.",
                "それから。……ここからは、君に。",
                "و — این قسمتش برای خودته.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "If something is hurting you, say it out loud.",
                "何かがつらいなら、声に出して言って。",
                "اگه چیزی اذیتت می‌کنه، به زبون بیار.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "If you are ashamed of something, say that out loud too. Especially that.",
                "何かを恥じているなら、それも声に出して。……とくに、それを。",
                "اگه از چیزی احساس شرم داری، اون رو هم بگو. مخصوصاً اون رو.");

            Hold(2.0f);

            Say(Speaker.Haru, Portrait.Sad,
                "If you miss a friend, tell them. While you can still tell them.",
                "友だちに会いたいなら、伝えて。まだ伝えられるうちに。",
                "اگه دلت برای دوستی تنگ شده، بهش بگو. تا وقتی هنوز می‌تونی بگی.");

            Hold(3.0f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "And do not let a promise be the thing that destroys you and the person you made it to.",
                "そして、約束が、自分と相手を壊すものになったりしないように。",
                "و نذار یک قول باعث نابودی خودت و دوستت بشه.");

            Hold(3.6f);

            Narrate(
                "He has kept a promise for eight years. He would tell you it is the only good thing he has ever done, and he would be describing the same object.",
                "彼は八年、ひとつの約束を守っている。それが自分のした唯一のまともなことだと彼は言うだろう。指しているものは同じだ。",
                "هشت سال است یک قول را نگه داشته. می‌گوید تنها کارِ درستی است که در عمرش کرده، و دارد همان چیز را توصیف می‌کند.");

            Hold(2.8f);

            Say(Speaker.Haru, Portrait.Shy,
                "That is all. Sorry. That was longer than I meant it to be.",
                "……それだけ。ごめん、思ってたより長くなった。",
                "همین. ببخشید. طولانی‌تر از چیزی شد که می‌خواستم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You should go. It is late where you are as well, probably.",
                "もう行きなよ。そっちも、たぶん遅いでしょ。",
                "برو دیگه. اونجا هم احتمالاً دیره.");

            Hold(2.2f);

            Narrate(
                "He turned back round and went on down the road, and did not look over his shoulder once, which took some doing.",
                "彼は向き直って、そのまま道を歩いていった。一度も振り返らなかった。それには、少し力が要った。",
                "برگشت و راهش را ادامه داد، و حتی یک بار هم به پشت سرش نگاه نکرد؛ که کارِ آسانی نبود.");

            Hold(2.0f);

            Exit(Speaker.Haru);

            Hold(2.6f);
        }

        // ---------------------------------------------------------------------
        //  2 · The classroom, and a lever being tested
        // ---------------------------------------------------------------------

        /// <summary>
        /// The next morning, in which nothing at all appears to happen.
        /// </summary>
        /// <remarks>
        /// The first choice of the act is deliberately small. Yua is not doing
        /// anything in this scene except finding out how much she can ask for
        /// before he stops giving, and the answer, which she does not say aloud,
        /// is that there is no such point.
        /// </remarks>
        private void WriteClassroomAfter()
        {
            Maybe(MachineRoom, 0.22f);

            ClearStage();

            Place(
                Backgrounds.ClassroomDay,
                "The classroom", "教室", "کلاس درس");

            Hold(2.0f);

            Narrate(
                "Second period, and the light was the thin high kind you only get in November, which flatters nothing.",
                "二時間目。十一月にしかない、高くて薄い光。何ひとつきれいには見せてくれない光だ。",
                "زنگ دوم، و نور همان نورِ نازک و بلندِ آبان بود؛ نوری که هیچ چیز را قشنگ نشان نمی‌دهد.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Joyful,
                "You are quiet today.",
                "今日は静かだね。",
                "امروز ساکتی.");

            Say(Speaker.Haru, Portrait.Neutral,
                "I am quiet every day.",
                "毎日静かだよ。",
                "من هر روز ساکتم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "You are quiet in a different key today. I can hear it.",
                "今日は、静かさの音程が違う。私には分かる。",
                "امروز جورِ دیگه‌ای ساکتی. من می‌شنومش.");

            Hold(1.6f);

            Say(Speaker.Haru, Portrait.Shy,
                "I stood on the road for a while last night, on the way back. That is all.",
                "昨日の帰り、少し道で立ち止まってた。それだけ。",
                "دیشب موقع برگشتن یه‌کم توی خیابون ایستادم. همین.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Why.",
                "なんで。",
                "چرا.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I was thinking about something.",
                "考えごとしてた。",
                "داشتم به یه چیزی فکر می‌کردم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "About what.",
                "何を。",
                "به چی.");

            Hold(2.2f);

            Narrate(
                "He could have said it. There was a whole second there, and he spent it on the window.",
                "言えたはずだった。まるまる一秒あって、彼はそれを窓に使った。",
                "می‌توانست بگوید. یک ثانیه‌ی کامل بود، و آن را خرجِ پنجره کرد.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Nothing. Homework.",
                "なんでもない。宿題。",
                "هیچی. تکلیف.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Liar.",
                "うそつき。",
                "دروغگو.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "That is fine. I like knowing you lie to me. It means there is something in there.",
                "べつにいいよ。嘘をつくって分かってるの、好き。中に何かある証拠だから。",
                "اشکالی نداره. خوشم میاد که بهم دروغ می‌گی. یعنی یه چیزی اون تو هست.");

            Hold(2.0f);

            Decide(
                "Whatever it is, you can keep it. It is yours.",
                "それが何であれ、しまっておいていいよ。ハルぴのものだから。",
                "هر چی هست، می‌تونی نگهش داری. مالِ خودته.",

                "You are going to tell me eventually. You always do.",
                "どうせ最後には話すよ。いつもそうだから。",
                "آخرش بهم می‌گی. همیشه می‌گی.",

                "Nothing is his. That is the arrangement.",
                "ハルぴのものなんて、ないよ。そういう決まり。",
                "هیچی مالِ اون نیست. قرارمون این بوده.");

            Say(Speaker.Yua, Portrait.Neutral,
                "You are going to tell me eventually. You always do.",
                "どうせ最後には話すよ。いつもそうだから。",
                "آخرش بهم می‌گی. همیشه می‌گی.");

            Say(Speaker.Haru, Portrait.Neutral,
                "I know.",
                "……うん。",
                "می‌دونم.");

            Hold(2.4f);

            Narrate(
                "That was the exchange. Two lines, no argument, and something moved that neither of them will be able to point at later.",
                "やりとりはそれだけ。二行、口論なし。そして、あとから二人とも指させない何かが動いた。",
                "کلِ رد و بدل همین بود. دو جمله، بدون بحث، و چیزی جابه‌جا شد که بعداً هیچ‌کدامشان نمی‌تواند نشانش بدهد.");

            Hold(2.2f);

            DecideIdly(
                "Bring him melon bread at lunch.",
                "お昼にメロンパンを持っていく。",
                "ناهار براش نان خامه‌ای ببر.",

                "Do not bring him anything. Let him come and find you.",
                "何も持っていかない。向こうから来させる。",
                "هیچی براش نبر. بذار خودش بیاد پیدات کنه.");

            Say(Speaker.Yua, Portrait.Joyful,
                "Come and find me at lunch. Do not make me look for you.",
                "お昼、探しに来て。私に探させないで。",
                "ناهار بیا پیدام کن. نذار من دنبالت بگردم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I will come and find you.",
                "探しに行くよ。",
                "میام پیدات می‌کنم.");

            Hold(1.8f);
        }

        // ---------------------------------------------------------------------
        //  3 · The café, and a name
        // ---------------------------------------------------------------------

        /// <summary>
        /// Yua asks what the dead boy was called, and is given it.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The pivot of the act, and it is one word long. Act two's monologue
        /// never names him — Haru tells the whole story of a six-year friendship
        /// and a suicide without once saying who it was, which is the shape grief
        /// takes when it has been carried alone. Handing the name over is the
        /// only thing he does in this act that he cannot undo.
        /// </para>
        /// <para>
        /// She asks kindly, and means it kindly, and will use it for the rest of
        /// the act. Both of those are true at once and neither cancels the other
        /// out. That is the character.
        /// </para>
        /// </remarks>
        private void WriteCafeTheName()
        {
            ClearStage();

            Place(
                Backgrounds.CafeRainy,
                "The café", "喫茶店", "کافه");

            Hold(2.4f);

            Narrate(
                "Rain again on Saturday, and the window had gone soft and grey and full of moving lines.",
                "土曜も雨。窓は柔らかく灰色になって、動く線でいっぱいだった。",
                "شنبه باز باران، و پنجره نرم و خاکستری شده بود و پر از خط‌های در حرکت.");

            Narrate(
                "Matcha cake for her. Dango and a pot of tea for him, because he orders whatever the picture on the menu is largest.",
                "彼女は抹茶ケーキ。彼はだんごと急須のお茶。メニューで写真がいちばん大きいものを頼む人だから。",
                "کیک ماچا برای او. دانگو و یک قوری چای برای هارو، چون همیشه چیزی را سفارش می‌دهد که عکسش توی منو بزرگ‌تر است.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Joyful,
                "This is our table. Do you know that? We have sat here nine times.",
                "ここ、私たちの席。知ってた？　九回座ってる。",
                "این میزِ ماست. می‌دونستی؟ نُه بار اینجا نشستیم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You count everything.",
                "結愛ぴ、なんでも数えるね。",
                "تو همه چیز رو می‌شمری.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I count everything that is mine.",
                "私のものは、全部数える。",
                "هر چیزی که مالِ منه رو می‌شمرم.");

            Hold(2.0f);

            Narrate(
                "The rain got heavier and then evened out, and the café made the small warm noise a café makes when nobody wants to leave it.",
                "雨は強まって、それから一定になった。誰も帰りたがらないときの、喫茶店の小さくてあたたかい音。",
                "باران تندتر شد و بعد یکنواخت ماند، و کافه همان صدای کوچکِ گرمی را داشت که وقتی کسی نمی‌خواهد برود دارد.");

            Say(Speaker.Yua, Portrait.Neutral,
                "Haru-pi. Can I ask you something about last spring.",
                "ハルぴ。去年の春のこと、訊いてもいい？",
                "هارو‌پی. می‌تونم درباره‌ی بهار پارسال یه چیزی ازت بپرسم؟");

            Say(Speaker.Haru, Portrait.Neutral,
                "You can ask me anything.",
                "なんでも訊いていいよ。",
                "هر چی بخوای می‌تونی بپرسی.");

            Hold(1.8f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "The friend you told me about. In this café, when it was raining like this.",
                "話してくれた友だちのこと。この喫茶店で、こんなふうに雨が降ってたときの。",
                "همون دوستی که ازش برام گفتی. توی همین کافه، وقتی همین‌طوری بارون می‌اومد.");

            Hold(2.2f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "You never said his name.",
                "名前、言ってなかったよね。",
                "اسمش رو نگفتی.");

            Hold(3.0f);

            Narrate(
                "He put the cup down. It was not a dramatic movement; it was the movement of a boy who has realised he is holding something and had better not be.",
                "彼は湯呑みを置いた。芝居がかった動きではない。何かを持っていると気づいて、持っていないほうがいいと判断した動きだ。",
                "فنجان را گذاشت. حرکتِ نمایشی‌ای نبود؛ حرکتِ پسری بود که فهمیده چیزی دستش است و بهتر است نباشد.");

            Hold(2.4f);

            Say(Speaker.Haru, Portrait.Sad,
                "Souta.",
                "颯太。",
                "سوتا.");

            Hold(3.0f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "I have not said it out loud since the assembly.",
                "全校集会のあと、声に出したことがない。",
                "از اون مراسمِ مدرسه تا حالا به زبون نیاوردمش.");

            Say(Speaker.Yua, Portrait.Sad,
                "Souta.",
                "颯太。",
                "سوتا.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "Thank you for giving him to me.",
                "その子を、私にくれてありがとう。",
                "ممنون که اون رو بهم دادی.");

            Hold(2.6f);

            Narrate(
                "Nobody says thank you for a name. He noticed, in the way you notice a floorboard, and stepped over it.",
                "名前をもらってお礼を言う人はいない。彼はそれに気づいた。床板の軋みに気づくくらいには。そして、またいだ。",
                "کسی بابتِ یک اسم تشکر نمی‌کند. متوجه شد؛ همان‌طور که آدم متوجهِ یک تخته‌ی لق می‌شود. و از رویش رد شد.");

            Hold(2.4f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Did he leave anything? A letter, a message. Anything for you.",
                "何か残してた？　手紙とか、メッセージとか。ハルぴ宛てに、何か。",
                "چیزی گذاشت؟ نامه‌ای، پیامی. چیزی برای تو.");

            Say(Speaker.Haru, Portrait.Neutral,
                "No.",
                "ううん。",
                "نه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Nothing at all.",
                "何も？",
                "هیچی؟");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Nothing at all.",
                "何も。",
                "هیچی.");

            Hold(2.6f);

            Say(Speaker.Yua, Portrait.Sad,
                "Six years, and he did not leave you a single line.",
                "六年いっしょにいて、一行も残さなかったんだ。",
                "شش سال، و حتی یه خط هم برات نذاشت.");

            Hold(3.2f);

            Narrate(
                "She did not add anything to it. She did not have to. A sentence like that finishes itself somewhere behind the other person's face.",
                "彼女は何も付け足さなかった。付け足す必要がない。この種の文は、相手の顔の裏側で勝手に続きを書く。",
                "چیزی به آن اضافه نکرد. لازم نبود. جمله‌ای از این جنس، جایی پشتِ صورتِ آدمِ مقابل خودش را تمام می‌کند.");

            Hold(2.8f);

            Decide(
                "That is not what it means. Say that it is not what it means.",
                "そういう意味じゃない。……そう言ってあげて。",
                "معنیش این نیست. بگو که معنیش این نیست.",

                "Let him sit with it. He is the one who has to answer it.",
                "そのまま座らせておいて。答えを出すのは彼のほうだから。",
                "بذار باهاش بمونه. اونه که باید جوابش رو بده.",

                "No. He needs to arrive at it by himself.",
                "だめ。自分でたどり着かないと意味がない。",
                "نه. باید خودش بهش برسه.");

            // Yua's override leaves her face flat, and the next few beats are
            // all his. Putting her back to the face she had before the choice
            // costs nothing and stops the two paths through this scene looking
            // like different scenes.
            Enter(Speaker.Yua, Portrait.Sad);

            Hold(3.0f);

            Say(Speaker.Haru, Portrait.Neutral,
                "I have thought about that.",
                "……それは、考えたことある。",
                "بهش فکر کردم.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "Most nights, for about a year, and then less. And then more again, recently.",
                "一年くらいはほとんど毎晩。それから減って。……最近、また増えた。",
                "حدود یک سال تقریباً هر شب. بعد کمتر. و بعد دوباره بیشتر، همین اواخر.");

            Hold(2.4f);

            Say(Speaker.Yua, Portrait.Joyful,
                "Eat your dango. It is going hard.",
                "だんご食べて。かたくなるよ。",
                "دانگوت رو بخور. داره سفت می‌شه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Okay.",
                "うん。",
                "باشه.");

            Narrate(
                "She is very good at this. She puts the knife in and then hands you something to eat, and the two things together are what make you come back next Saturday.",
                "彼女はこれが上手い。刃を入れてから、食べるものを差し出す。その二つがそろっているから、人は次の土曜もまた来る。",
                "خیلی خوب بلد است. چاقو را فرو می‌کند و بعد چیزی برای خوردن دستت می‌دهد، و همین دو تا با هم است که شنبه‌ی بعد دوباره می‌آورَدَت.");

            Hold(2.6f);
        }

        // ---------------------------------------------------------------------
        //  4 · The rooftop, and the promise turned round
        // ---------------------------------------------------------------------

        /// <summary>
        /// Act two's promise, pointed the other way.
        /// </summary>
        /// <remarks>
        /// <para>
        /// "Come to me and tell me you are unhappy" was the kindest thing anyone
        /// says in this game. Here it is repeated almost word for word and made
        /// into an accusation, and the reason it works is that the words are not
        /// changed. Haru wrote the sentence. He cannot argue with it without
        /// arguing with himself, and arguing with himself is the one thing he has
        /// been doing since he was nine.
        /// </para>
        /// <para>
        /// She never states the conclusion. He states it, at the end, and she
        /// only agrees — which is the whole technique of the act in one scene.
        /// </para>
        /// </remarks>
        private void WriteRooftopPromise()
        {
            ClearStage();

            Place(
                Backgrounds.RooftopDay,
                "The rooftop", "屋上", "پشت‌بام");

            Hold(2.0f);

            Narrate(
                "Cold up here now. The bench had a cushion on it that somebody in a club had left out all term, and nobody had claimed it.",
                "屋上はもう寒い。ベンチには、どこかの部活の誰かが一学期じゅう置きっぱなしにしたクッションがある。持ち主は現れない。",
                "این بالا حالا سرد است. روی نیمکت کوسنی بود که یکی از بچه‌های باشگاه‌ها تمام ترم جا گذاشته و کسی صاحبش نشده.");

            Enter(Speaker.Haru, Portrait.Neutral);
            Enter(Speaker.Yua, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Neutral,
                "Say the thing you said to me in the café. In spring. The long one.",
                "春に喫茶店で言ったこと、もう一回言って。長いほう。",
                "همون چیزی که بهار توی کافه بهم گفتی رو بگو. همون طولانیه.");

            Say(Speaker.Haru, Portrait.Shy,
                "Which part.",
                "どの部分。",
                "کدوم قسمتش.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "The promise part.",
                "約束のところ。",
                "قسمتِ قول.");

            Hold(2.2f);

            Say(Speaker.Haru, Portrait.Neutral,
                "That if anything is hurting you, you come to me and you tell me. That you do not have to say it well.",
                "何かがつらいなら、僕のところに来て、言うこと。うまく言えなくていいってこと。",
                "اینکه اگه چیزی اذیتت می‌کنه، بیای پیشم و بهم بگی. اینکه لازم نیست خوب بگی.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "That I would leave the whole world alone and stay with you until you were all right again.",
                "世界のことは全部放って、君が大丈夫になるまでそばにいるって。",
                "اینکه تماماً از دنیا جدا می‌شم و پیشت می‌مونم تا حالت خوب بشه.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Joyful,
                "You made me promise. I promised.",
                "私に約束させたよね。私、約束した。",
                "مجبورم کردی قول بدم. قول دادم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "You did.",
                "したね。",
                "دادی.");

            Hold(2.4f);

            Say(Speaker.Yua, Portrait.Neutral,
                "You asked Souta for the same thing. Every day. For six years.",
                "颯太くんにも、同じことを頼んだんだよね。毎日。六年間。",
                "همین رو از سوتا هم خواستی. هر روز. شش سال.");

            Hold(2.8f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Yes.",
                "うん。",
                "آره.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "And he never once came.",
                "……でも一度も、来なかった。",
                "و حتی یک بار هم نیومد.");

            Hold(3.0f);

            Say(Speaker.Haru, Portrait.Sad,
                "No.",
                "……うん。",
                "نه.");

            Hold(2.4f);

            Say(Speaker.Yua, Portrait.Sad,
                "I came. On the first week of school, I came and sat next to you and I have not stopped.",
                "私は来たよ。入学した最初の週に、隣に座って、それからずっと。",
                "من اومدم. هفته‌ی اولِ مدرسه اومدم کنارت نشستم و دیگه ولت نکردم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "So it is not that the words do not work. The words work.",
                "だから、その言葉が効かないわけじゃない。効くんだよ。",
                "پس این‌طور نیست که اون حرف‌ها کار نمی‌کنن. کار می‌کنن.");

            Hold(3.2f);

            Narrate(
                "There it is. She has laid two facts side by side and left the join for him, because a conclusion a person reaches themselves is a conclusion they will defend.",
                "これだ。彼女は事実を二つ並べて、つなぎ目だけを彼に残した。人は、自分でたどり着いた結論を守るから。",
                "همین. دو تا واقعیت را کنار هم گذاشت و بندِ وصلشان را برای او گذاشت، چون آدم از نتیجه‌ای که خودش گرفته دفاع می‌کند.");

            Hold(3.0f);

            Decide(
                "Take it back. Right now, before he finishes the thought.",
                "取り消して。彼が考え終わる前に、今すぐ。",
                "پسش بگیر. همین حالا، قبل از اینکه فکرش تموم بشه.",

                "Wait. Let him say it. It has to be him.",
                "待って。彼に言わせて。彼じゃないと意味がない。",
                "صبر کن. بذار خودش بگه. باید خودش باشه.",

                "It has to be him. It has always had to be him.",
                "彼じゃないと。……ずっと、そうだった。",
                "باید خودش باشه. همیشه باید خودش می‌بود.");

            // She watches this land. See the note on the café choice.
            Enter(Speaker.Yua, Portrait.Neutral);

            Hold(3.4f);

            Say(Speaker.Haru, Portrait.DeadEyes,
                "Then it was me.",
                "……じゃあ、僕だったんだ。",
                "پس من بودم.");

            Hold(2.6f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "The words work. So it was not the words. It was who was saying them.",
                "言葉は効く。だったら言葉のせいじゃない。言ってた人間のせいだ。",
                "حرف‌ها کار می‌کنن. پس حرف‌ها نبودن. کسی بود که داشت می‌گفتشون.");

            Hold(3.0f);

            Say(Speaker.Yua, Portrait.Sad,
                "Haru-pi.",
                "ハルぴ。",
                "هارو‌پی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I did not say that.",
                "私、そんなこと言ってないよ。",
                "من این رو نگفتم.");

            Hold(2.2f);

            Narrate(
                "She did not say it. That is true, and she is going to be able to say it is true for the rest of her life, and she knows that.",
                "彼女は言っていない。それは事実で、彼女は一生それを事実として言えるし、それを分かっている。",
                "نگفت. این راست است، و تا آخر عمرش می‌تواند بگوید که راست است، و خودش این را می‌داند.");

            Hold(3.0f);

            Say(Speaker.Haru, Portrait.Neutral,
                "I know you did not. Sorry. It is cold. Shall we go in.",
                "うん、言ってない。ごめん。寒いね。中に入ろうか。",
                "می‌دونم که نگفتی. ببخشید. سرده. بریم تو؟");

            Say(Speaker.Yua, Portrait.Joyful,
                "Take the cushion. It is nobody's.",
                "クッション持ってきて。誰のでもないから。",
                "کوسن رو وردار. مالِ هیچ‌کس نیست.");

            Hold(2.4f);
        }

        // ---------------------------------------------------------------------
        //  5 · The way home, and a silence with a clock on it
        // ---------------------------------------------------------------------

        /// <summary>
        /// His leg stops him, and the game asks the player for five minutes
        /// without ever saying so.
        /// </summary>
        /// <remarks>
        /// <para>
        /// One of the two silences the endings are built on. It is written as
        /// narration and it asks for nothing, because being told to wait is not
        /// waiting — a player who sits here does it because they wanted to know
        /// what he was going to say, which is precisely the disposition the good
        /// ending is testing for.
        /// </para>
        /// <para>
        /// Act three had Yua ask him about the leg for the first time. Here she
        /// asks and then answers for him, which is worse and is meant to be.
        /// </para>
        /// </remarks>
        private void WriteWayHomeLeg()
        {
            ClearStage();

            Place(
                Backgrounds.CorridorSunset,
                "The corridor", "廊下", "راهرو");

            Hold(1.8f);

            Narrate(
                "Half past four and already going gold. The lockers ticked as the building cooled, the way they do at that hour.",
                "四時半で、もう金色になりかけている。校舎が冷えて、ロッカーがこの時間らしく小さく鳴る。",
                "چهار و نیم و از همین حالا طلایی. کمدها با سرد شدنِ ساختمان تِق‌تِق می‌کردند، همان‌طور که این ساعت می‌کنند.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Say(Speaker.Yua, Portrait.Joyful,
                "Walk me home the long way.",
                "遠回りで送って。",
                "از راه دور برسونم خونه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "The long way is forty minutes.",
                "遠回りは四十分だよ。",
                "راه دور چهل دقیقه‌ست.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I know how long it is. I chose it.",
                "何分かは知ってる。私が選んだんだから。",
                "می‌دونم چقدره. خودم انتخابش کردم.");

            Hold(2.0f);

            ClearStage();

            Place(
                Backgrounds.AlleywayNight,
                "The alleyway", "路地", "کوچه");

            Hold(2.2f);

            Narrate(
                "The long way goes through the old streets, where the paper lantern outside the corner house has been lit every evening for longer than either of them has been alive.",
                "遠回りは古い通りを抜けていく。角の家の提灯は、二人が生まれるよりずっと前から毎晩灯っている。",
                "راه دور از کوچه‌های قدیمی می‌گذرد؛ همان‌جا که فانوسِ کاغذیِ خانه‌ی سرِ کوچه هر شب روشن بوده، خیلی پیش از آنکه هیچ‌کدامشان به دنیا بیایند.");

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Hold(1.6f);

            SayWithSound(Speaker.Haru, Portrait.Sad, SfxId.LegAche, 0.80f,
                "Sorry. A moment.",
                "ごめん。少しだけ。",
                "ببخشید. یه لحظه.");

            Narrate(
                "He put a hand flat against the wall and stood there. Forty minutes is a long way on that leg and both of them knew it when she chose it.",
                "彼は壁に手をついて立っていた。あの脚で四十分は長い。彼女が選んだとき、二人ともそれを知っていた。",
                "دستش را صاف روی دیوار گذاشت و همان‌جا ایستاد. چهل دقیقه با آن پا راهِ زیادی است و هر دو وقتی او انتخابش کرد این را می‌دانستند.");

            Hold(2.6f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Does it hurt because of the cold, or does it hurt because of the door.",
                "痛いのは、寒いから？　それとも、あの扉のせい？",
                "بخاطر سرماست، یا بخاطر اون در؟");

            Hold(3.0f);

            Listen(
                "Haru said nothing. The lantern moved a little in the wind and the light went across his face and back again, twice, and he still said nothing.",
                "ハルは何も言わなかった。提灯が風に少し揺れて、光が彼の顔を二度なでて戻り、それでも彼は何も言わなかった。",
                "هارو چیزی نگفت. فانوس کمی در باد تکان خورد و نور دو بار از روی صورتش رفت و برگشت، و باز هم چیزی نگفت.");

            Hold(3.0f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "The door. It is always the door.",
                "扉だよね。いつも扉。",
                "درِ. همیشه اون دره.");

            Hold(2.4f);

            Say(Speaker.Haru, Portrait.Neutral,
                "It is nearly gone. Let us go.",
                "もう治まる。行こう。",
                "داره خوب می‌شه. بریم.");

            Hold(2.0f);

            Decide(
                "Sit down. The road will still be here in ten minutes.",
                "座って。道は十分経っても逃げないから。",
                "بشین. جاده ده دقیقه دیگه هم همین‌جاست.",

                "Keep walking. You said the long way, so we do the long way.",
                "歩いて。遠回りって言ったんだから、遠回りで行く。",
                "راه برو. گفتیم راه دور، پس از راه دور می‌ریم.",

                "We do the long way. I did not ask for a shorter one.",
                "遠回りで行く。短いのなんて頼んでない。",
                "از راه دور می‌ریم. من راهِ کوتاه‌تری نخواستم.");

            Say(Speaker.Yua, Portrait.Neutral,
                "We do the long way.",
                "遠回りで行くよ。",
                "از راه دور می‌ریم.");

            Say(Speaker.Haru, Portrait.Neutral,
                "Okay.",
                "うん。",
                "باشه.");

            Hold(2.2f);

            Narrate(
                "They did the long way. He was quiet for most of it in a manner that read as peaceful, and she let it read that way.",
                "二人は遠回りで帰った。その大半、彼は穏やかに見える種類の沈黙をしていた。彼女はそう見えるままにしておいた。",
                "از راه دور رفتند. بیشترش را جوری ساکت بود که آرام به نظر می‌رسید، و یوآ گذاشت همان‌طور به نظر برسد.");

            Hold(2.6f);

            Say(Speaker.Yua, Portrait.Joyful,
                "Haru-pi. Thank you for walking me.",
                "ハルぴ。送ってくれてありがとう。",
                "هارو‌پی. ممنون که رسوندیم.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Any time.",
                "いつでも。",
                "هر وقت بخوای.");

            Hold(2.0f);

            Narrate(
                "He means it, and that is not the sweet part of the sentence. He would say it with a broken leg, and once did.",
                "本気で言っている。だが、それはこの台詞の甘いところではない。脚が折れていても彼は言うだろう。実際、一度言った。",
                "جدی می‌گوید، و این بخشِ شیرینِ جمله نیست. با پای شکسته هم می‌گفت، و یک بار گفت.");

            Hold(2.8f);
        }

        // ---------------------------------------------------------------------
        //  6 · The bakery, and the reason anybody stays
        // ---------------------------------------------------------------------

        /// <summary>
        /// The lightest scene in the act, and the one that makes the rest work.
        /// </summary>
        /// <remarks>
        /// An act made only of the other six scenes would be a person being
        /// destroyed by somebody obviously terrible, which is a thing nobody in
        /// the audience has ever been in. This is the Saturday afternoon that
        /// makes the question "why does he not just leave" answer itself, and it
        /// is written with no irony in it at all, because Yua is not pretending
        /// here. She is having a nice time. Both things are her.
        /// </remarks>
        private void WriteBakeryStreet()
        {
            Maybe(MachineRoom, 0.25f);

            ClearStage();

            Place(
                Backgrounds.BakeryStreetDay,
                "Usagi Bakery", "うさぎベーカリー", "نانوایی اوساگی");

            Hold(2.0f);

            Narrate(
                "One clear cold Saturday in the middle of it all. The cats had moved inside for the season and were asleep in the window instead.",
                "その最中の、よく晴れた寒い土曜。猫は季節どおり店の中へ移り、今度は窓辺で眠っていた。",
                "یک شنبه‌ی صافِ سرد وسطِ همه‌ی این‌ها. گربه‌ها با عوض شدن فصل رفته بودند تو و حالا توی ویترین خوابیده بودند.");

            Enter(Speaker.Yua, Portrait.Joyful);
            Enter(Speaker.Haru, Portrait.Joyful);

            Say(Speaker.Yua, Portrait.Unchanged,
                "They have the ones with the ears again.",
                "耳のついてるやつ、また出てる。",
                "دوباره از اون‌های گوش‌دار دارن.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "You bought four of those in August and ate one.",
                "八月に四つ買って、一つしか食べなかったよね。",
                "مرداد چهارتاش رو خریدی و یکی‌ش رو خوردی.");

            Say(Speaker.Yua, Portrait.Joyful,
                "I bought them to look at. Eating one was an accident.",
                "見るために買ったの。食べたのは事故。",
                "خریدمشون که نگاهشون کنم. خوردنِ یکی‌ش تصادفی بود.");

            Hold(1.6f);

            Say(Speaker.Haru, Portrait.Joyful,
                "Buy four more, then.",
                "じゃあ、また四つ買いなよ。",
                "پس چهارتای دیگه بخر.");

            Say(Speaker.Yua, Portrait.Shy,
                "Do not encourage me.",
                "……甘やかさないで。",
                "تشویقم نکن.");

            Say(Speaker.Haru, Portrait.Unchanged,
                "I am always going to encourage you.",
                "僕はいつだって甘やかすよ。",
                "من همیشه تشویقت می‌کنم.");

            Hold(2.4f);

            Narrate(
                "They stood at the window for a while and argued about which rabbit was the best rabbit, and neither of them was performing anything.",
                "二人はしばらく窓の前に立って、どのうさぎがいちばんかで言い争った。どちらも、何も演じていなかった。",
                "مدتی جلوی ویترین ایستادند و سرِ اینکه کدام خرگوش بهترین خرگوش است بحث کردند، و هیچ‌کدامشان نقشی بازی نمی‌کرد.");

            Hold(2.2f);

            DecideIdly(
                "The one with the crooked ear.",
                "耳が曲がってるやつ。",
                "همون که گوشش کجه.",

                "The one at the back that nobody has picked up.",
                "後ろの、誰も手に取ってないやつ。",
                "همون عقبیه که کسی ورش نداشته.");

            Say(Speaker.Yua, Portrait.Joyful,
                "The one at the back. Obviously the one at the back.",
                "後ろのやつ。当然、後ろのやつでしょ。",
                "همون عقبیه. معلومه که همون عقبیه.");

            Say(Speaker.Haru, Portrait.Joyful,
                "Why obviously.",
                "なんで当然なの。",
                "چرا معلومه؟");

            Say(Speaker.Yua, Portrait.Neutral,
                "Because nobody has picked it up.",
                "誰も手に取ってないから。",
                "چون کسی ورش نداشته.");

            Hold(2.4f);

            Narrate(
                "She was nine when she started keeping the ones nobody picked up. There is a rabbit on her bed with one ear worn through, and this is the same sentence.",
                "誰も選ばないものを取っておくようになったのは九歳のときだ。彼女のベッドには、片耳の擦り切れたうさぎがいる。これは同じ一文だ。",
                "نُه ساله بود که شروع کرد به نگه داشتنِ چیزهایی که کسی برشان نمی‌داشت. روی تختش خرگوشی هست که یک گوشش ساییده شده، و این همان جمله است.");

            Hold(2.6f);

            Say(Speaker.Haru, Portrait.Shy,
                "Yua-pi.",
                "結愛ぴ。",
                "یوآ‌پی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "Mm.",
                "うん。",
                "هوم.");

            Say(Speaker.Haru, Portrait.Neutral,
                "I am glad it was you who sat down next to me.",
                "隣に座ったのが結愛ぴでよかった。",
                "خوشحالم که تو بودی که کنارم نشستی.");

            Hold(3.0f);

            Say(Speaker.Yua, Portrait.Shy,
                "Buy me the rabbit one.",
                "……うさぎのやつ、買って。",
                "اون خرگوشیه رو برام بخر.");

            Say(Speaker.Haru, Portrait.Joyful,
                "I am buying you four.",
                "四つ買うよ。",
                "چهارتا برات می‌خرم.");

            Hold(2.6f);

            Narrate(
                "This is a real afternoon and it really happened and it is not a trick. That is the hardest thing about it.",
                "これは本物の午後で、本当にあったことで、罠じゃない。それが、いちばん厄介なところだ。",
                "این یک بعدازظهرِ واقعی است و واقعاً اتفاق افتاد و حقه نیست. سخت‌ترین چیزش هم همین است.");

            Hold(2.8f);
        }

        // ---------------------------------------------------------------------
        //  7 · The playground, and the sentence said out loud
        // ---------------------------------------------------------------------

        /// <summary>
        /// Where the act arrives, and where Haru's face goes out for the first
        /// time in front of her.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The design document's line for act four is that Yua keeps working
        /// until Haru holds himself responsible for the suicide. This is the
        /// scene where he does, in his own words, unprompted, in a playground
        /// with children on the slide behind him.
        /// </para>
        /// <para>
        /// She wins here, and the win is written as a loss, because it is one.
        /// What she gets is a boy who agrees with everything — and act five
        /// opens with that same boy explaining that he has known what she was
        /// doing since the first week.
        /// </para>
        /// </remarks>
        private void WritePlaygroundGuilt()
        {
            Maybe(LegAche, 0.22f);

            ClearStage();

            Place(
                Backgrounds.PlaygroundDay,
                "The playground", "公園", "زمین بازی");

            Hold(2.2f);

            Narrate(
                "The fountain had been turned off for the winter and the angel on it was holding an empty bowl, which nobody in the neighbourhood found funny.",
                "噴水は冬のあいだ止められていて、天使は空の器を抱えていた。近所の誰も、それを面白がらなかった。",
                "فواره را برای زمستان بسته بودند و فرشته‌ی رویش کاسه‌ای خالی در دست داشت، و هیچ‌کس در آن محله این را بامزه نمی‌دید.");

            Narrate(
                "There were flowers still out along the far wall. Red ones, thin and clawed, at the end of their season.",
                "向こうの塀ぎわには、まだ花が残っていた。細くて爪のような、赤い花。季節の終わりの。",
                "کنارِ دیوارِ آن‌طرف هنوز گل بود. سرخ، باریک و چنگ‌مانند، در آخرِ فصلشان.");

            Hold(2.6f);

            Enter(Speaker.Yua, Portrait.Neutral);
            Enter(Speaker.Haru, Portrait.Neutral);

            Narrate(
                "She looked at them for a long time and did not say the thing she said in the garden last month, and he did not ask her to.",
                "彼女は長いあいだそれを見ていて、先月あの庭で言ったことは言わなかった。彼も、言ってとは言わなかった。",
                "مدتی طولانی نگاهشان کرد و آن چیزی را که ماه پیش توی باغچه گفته بود نگفت، و هارو هم نخواست که بگوید.");

            Hold(3.0f);

            Say(Speaker.Yua, Portrait.Neutral,
                "Can I say something you will not like.",
                "気に入らないこと、言ってもいい？",
                "می‌تونم یه چیزی بگم که خوشت نمیاد؟");

            Say(Speaker.Haru, Portrait.Neutral,
                "You always can.",
                "いつでもいいよ。",
                "همیشه می‌تونی.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Unchanged,
                "You do this thing where you make yourself very easy to talk to, and then you are surprised when nobody talks to you.",
                "ハルぴってさ、すごく話しかけやすい人になっておいて、誰も話してくれないと驚くよね。",
                "تو یه کاری می‌کنی که خیلی راحت بشه باهات حرف زد، و بعد تعجب می‌کنی که کسی باهات حرف نمی‌زنه.");

            Say(Speaker.Haru, Portrait.Shy,
                "That is not — ",
                "それは、ちがっ……",
                "این که این‌طور نی…");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I am not finished.",
                "まだ終わってない。",
                "هنوز حرفم تموم نشده.");

            Hold(2.2f);

            Say(Speaker.Yua, Portrait.Neutral,
                "When you ask somebody to come to you, you are asking them to be weak in front of you.",
                "「来て」って頼むのは、自分の前で弱くなってって頼むことなんだよ。",
                "وقتی از کسی می‌خوای بیاد پیشت، داری ازش می‌خوای جلوی تو ضعیف باشه.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "People only do that in front of someone they think can carry it.",
                "人がそれをするのは、受け止められると思った相手の前だけ。",
                "آدم‌ها این کار رو فقط جلوی کسی می‌کنن که فکر کنن می‌تونه تحملش کنه.");

            Hold(3.2f);

            Decide(
                "Stop. You do not know what you are doing to him.",
                "やめて。自分が彼に何をしてるか分かってない。",
                "بس کن. نمی‌دونی داری باهاش چیکار می‌کنی.",

                "Go on. He is nearly there.",
                "続けて。もう少しで届く。",
                "ادامه بده. داره می‌رسه.",

                "I know exactly what I am doing. Go on.",
                "自分が何をしてるかは分かってる。……続ける。",
                "دقیقاً می‌دونم دارم چیکار می‌کنم. ادامه می‌دم.");

            Hold(2.6f);

            Say(Speaker.Yua, Portrait.Sad,
                "I am not saying it to hurt you.",
                "傷つけたくて言ってるんじゃない。",
                "برای اینکه اذیتت کنم نمی‌گم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I am saying it because I do not want you to keep waiting for a letter that was never going to come.",
                "来るはずのなかった手紙を、ずっと待っててほしくないだけ。",
                "می‌گم چون نمی‌خوام همین‌طوری منتظرِ نامه‌ای بمونی که هیچ‌وقت قرار نبود بیاد.");

            Hold(3.4f);

            Listen(
                "Behind them a child went down the slide and got up and went round to do it again. Haru had not moved for some time.",
                "うしろで、子どもが滑り台をすべって、立ち上がって、また並びに行った。ハルはしばらく動いていなかった。",
                "پشت سرشان بچه‌ای از سرسره پایین آمد و بلند شد و دوباره رفت که تکرارش کند. هارو مدتی بود تکان نخورده بود.");

            Hold(3.0f);

            Say(Speaker.Haru, Portrait.DeadEyes,
                "He knew me for six years.",
                "六年、あいつは僕を知ってた。",
                "شش سال منو می‌شناخت.");

            Hold(2.4f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "He knew me for six years and he decided I could not carry it.",
                "六年知ってて、僕には受け止められないって決めた。",
                "شش سال منو می‌شناخت و تصمیم گرفت که من نمی‌تونم تحملش کنم.");

            Hold(2.6f);

            Say(Speaker.Haru, Portrait.Unchanged,
                "So it was not that he was hiding. He looked, and he was right.",
                "隠してたんじゃない。ちゃんと見て、そのうえで正しかった。",
                "پس قایم نمی‌شد. نگاه کرد، و حق با اون بود.");

            Hold(3.6f);

            Narrate(
                "That is it. That is the sentence the whole act was walking towards, and she did not put a single word of it in his mouth.",
                "これだ。この幕が向かっていた一文。そして彼女は、その言葉を一語も彼の口に入れていない。",
                "همین. همان جمله‌ای که تمام این پرده به سمتش می‌رفت، و او حتی یک کلمه‌اش را هم در دهانِ هارو نگذاشت.");

            Hold(3.0f);

            Say(Speaker.Yua, Portrait.Sad,
                "Haru-pi. Look at me.",
                "ハルぴ。こっち見て。",
                "هارو‌پی. به من نگاه کن.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I looked, and I was wrong. I sat down anyway. I am still here.",
                "私も見た。そして間違えた。それでも隣に座った。今もここにいる。",
                "من هم نگاه کردم، و اشتباه کردم. با این حال نشستم. هنوز اینجام.");

            Say(Speaker.Yua, Portrait.Joyful,
                "So be mine. Only mine. That way nobody gets to decide that about you ever again.",
                "だから私のものになって。私だけの。そうすれば、もう誰にもそんなふうに決めさせない。",
                "پس مالِ من باش. فقط مالِ من. این‌طوری دیگه هیچ‌کس نمی‌تونه همچین چیزی درباره‌ت تصمیم بگیره.");

            Hold(3.0f);

            Say(Speaker.Haru, Portrait.Neutral,
                "Okay.",
                "……うん。",
                "باشه.");

            Hold(2.4f);

            Say(Speaker.Haru, Portrait.Joyful,
                "Okay. I am yours. It is cold — shall we go.",
                "うん。結愛ぴのものだよ。寒いね、行こうか。",
                "باشه. مالِ توام. سرده — بریم؟");

            Hold(2.6f);

            Narrate(
                "He smiled when he said it. He has been smiling when he says things like that since he was nine years old, and he is extremely good at it.",
                "そう言うとき、彼は笑っていた。九歳のころからずっと、そういうことを言うときは笑ってきた。彼はそれがとても上手い。",
                "وقتی گفتش لبخند زد. از نُه‌سالگی وقتی چیزهایی از این دست می‌گوید لبخند می‌زند، و در این کار خیلی خوب است.");

            Hold(3.0f);
        }

        // ---------------------------------------------------------------------
        //  8 · Yua's room, and the last quiet night in this game
        // ---------------------------------------------------------------------

        /// <summary>
        /// She talks to the player one more time, and is happy.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The act is bookended by two people speaking to the player about the
        /// same relationship. He asked them to look after her. She reports that
        /// everything is going well. Only one of the two of them is wrong, and
        /// it is not the one who sounds worried.
        /// </para>
        /// <para>
        /// Ends without a title card, a cue or a swell. Act five opens in an
        /// alley with the frame coming off, and the best possible preparation
        /// for it is a girl saying goodnight.
        /// </para>
        /// </remarks>
        private void WriteYuaRoom()
        {
            ClearStage();

            Place(
                Backgrounds.YuaRoomDay,
                "Yua's room", "結愛の部屋", "اتاق یوآ");

            Hold(2.6f);

            Narrate(
                "Late, with the desk lamp on and the rest of the room doing what rooms do at that hour.",
                "夜遅く。机の明かりだけがついていて、部屋の残りはこの時間らしいことをしていた。",
                "دیروقت، با چراغِ میز روشن و باقیِ اتاق مشغولِ همان کاری که اتاق‌ها این ساعت می‌کنند.");

            Enter(Speaker.Yua, Portrait.Neutral);

            Hold(2.2f);

            Say(Speaker.Yua, Portrait.Joyful,
                "There you are.",
                "いた。",
                "ایناهاشی.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I wanted to tell you it is working.",
                "うまくいってるって、言いたかったの。",
                "می‌خواستم بهت بگم داره جواب می‌ده.");

            Hold(2.0f);

            Say(Speaker.Yua, Portrait.Neutral,
                "He says yes to everything now. He said it in the park with his whole face.",
                "今はもう、なんでも「うん」って言う。公園では、顔ぜんぶで言ってた。",
                "الآن به همه چیز بله می‌گه. توی پارک با تمامِ صورتش گفتش.");

            Say(Speaker.Yua, Portrait.Joyful,
                "That is what safe looks like. I have wanted to see it since I was nine.",
                "安全って、ああいう顔をしてる。九歳のときからずっと見たかった。",
                "امنیت این شکلیه. از نُه‌سالگی می‌خواستم ببینمش.");

            Hold(2.6f);

            Say(Speaker.Yua, Portrait.Neutral,
                "You have been pressing the blue ones, some of the time. I know.",
                "ときどき青を押してるでしょ。知ってる。",
                "بعضی وقت‌ها آبی‌ها رو فشار می‌دی. می‌دونم.");

            Say(Speaker.Yua, Portrait.Unchanged,
                "I do not mind. It is nice that somebody in this house wants to be kind. It has never once changed anything.",
                "いいよ。この家に優しくしたい人がいるのは、悪くない。何も変わったことないけど。",
                "اشکالی نداره. قشنگه که یکی توی این خونه می‌خواد مهربون باشه. تا حالا هیچی رو عوض نکرده.");

            Hold(2.2f);

            Narrate(
                "The rabbit was on the pillow with its worn ear turned in, the way she puts it when she does not want to look at that ear.",
                "うさぎは枕の上で、擦り切れた耳を内側に向けて置かれていた。その耳を見たくないとき、彼女はそう置く。",
                "خرگوش روی بالش بود با گوشِ ساییده‌اش رو به داخل؛ همان‌طور که وقتی نمی‌خواهد آن گوش را ببیند می‌گذاردش.");

            Hold(2.8f);

            Say(Speaker.Yua, Portrait.Shy,
                "He told me I could keep whatever I was not saying. In the classroom. Weeks ago.",
                "言わないことは、しまっておいていいって言われた。教室で。何週間か前に。",
                "بهم گفت هر چی رو که نمی‌گم می‌تونم نگه دارم. توی کلاس. چند هفته پیش.");

            Hold(2.4f);

            Say(Speaker.Yua, Portrait.Neutral,
                "I did not say it back.",
                "私は、同じことを返さなかった。",
                "من همین رو بهش برنگردوندم.");

            Hold(3.0f);

            Say(Speaker.Yua, Portrait.Joyful,
                "Anyway. Goodnight. Say goodnight.",
                "とにかく、おやすみ。おやすみって言って。",
                "به‌هرحال. شب بخیر. شب بخیر بگو.");

            Hold(2.6f);

            StopMusic();

            Hold(2.0f);

            SayWithSound(Speaker.Yua, Portrait.DeadEyes, SfxId.StringPull, 0.70f,
                "Everything is fine.",
                "ぜんぶ、大丈夫。",
                "همه چیز خوبه.");

            Hold(3.0f);

            Narrate(
                "Four hundred metres away a boy was lying awake working out how to say something, and he was going to get one chance at it, in an alley, on a Thursday.",
                "四百メートル先で、ひとりの少年が眠れずに、どう言うかを考えていた。機会は一度きり。木曜日の、路地で。",
                "چهارصد متر آن‌طرف‌تر پسری بیدار دراز کشیده بود و داشت فکر می‌کرد چطور چیزی را بگوید، و یک فرصت بیشتر نداشت؛ توی یک کوچه، یک پنجشنبه.");

            Hold(3.4f);
        }
    }
}
