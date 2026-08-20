// -----------------------------------------------------------------------------
//  The Frayed Red String
//  Act07Builder.cs  (Editor only)
//
//  Act seven — Epilogue (エピローグ) — written out as code, and the last act.
//
//  Run it once from The Frayed Red String ▸ Build Act 07 From The Story
//  Document. It writes Assets/Story/Acts/Act07.asset.
//
//  Short, and three things long.
//
//    • The alley, afterwards. Haru is already dead and is still falling, and he
//      falls into her arms — which is act six's third special scene, mirrored,
//      eight years late. The document says it in one sentence and it should play
//      in about ninety seconds. Nothing is explained.
//
//    • The credits, as a film. This is the one place in the game that stops
//      being a stage with sprites on it: a full-screen MP4, scored to the
//      deliberately, aggressively cheerful J-Rock the document asks for, so that
//      the tonal whiplash does the work. The clip does not exist yet and the act
//      plays correctly without it — a missing film logs one line and the beat
//      carries straight on.
//
//    • The two sentences the game closes on, then the saves are erased and the
//      title comes back. Same closing words as both other endings, from
//      ActScriptWriter.WriteClosingWords.
//
//  ONE THING THAT IS EASY TO GET WRONG. Act five broke the frame, took the veil
//  off and left the dialogue box on the floor — but all three of those are built
//  fresh for every scene, so this act would otherwise open with the pastel wash
//  back on and the border restored, as though act five had not happened. So the
//  first two beats here put them back where act five left them. Act six does not
//  need that and deliberately does not do it: a memory is allowed to be a framed,
//  soft-focus thing, and it is the only act after five that should look like one.
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Narrative;
using UnityEditor;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Act seven's script.</summary>
    public sealed class Act07Builder : ActScriptWriter
    {
        /// <summary>
        /// The film the credits play, by name.
        /// </summary>
        /// <remarks>
        /// Reserved now so the beat is written and the act is finished. Drop an
        /// .mp4 of this name into <c>Assets/StreamingAssets/Video</c> — or
        /// <c>Assets/Video</c> — and it plays with no other change.
        /// </remarks>
        public const string CreditsFilm = "Act07Credits";

        protected override int ActNumber => 7;

        protected override string AssetName => "Act07";

        protected override LocalizedLine Title => L("Epilogue", "エピローグ", "اپیلوگ");

        /// <summary>No act music. The film brings its own.</summary>
        protected override string MusicTrack => string.Empty;

        [MenuItem("The Frayed Red String/Build Act 07 From The Story Document")]
        public static void Build()
        {
            new Act07Builder().BuildAsset();
        }

        /// <summary>Builds the act under a given policy, for the one-press setup.</summary>
        public static void Build(ActScriptWriter.RebuildPolicy policy)
        {
            new Act07Builder().BuildAsset(policy);
        }

        /// <summary>The act, in order.</summary>
        protected override void Write()
        {
            WriteTheAlley();
            WriteTheCredits();
            WriteTheEnd();
        }

        // ---------------------------------------------------------------------
        //  1 · The alley, afterwards
        // ---------------------------------------------------------------------

        /// <summary>
        /// Haru falls, and this time somebody catches him.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Written to be recognised rather than understood. The player watched
        /// this exact picture ninety seconds ago at the end of act six — a
        /// nine-year-old going over and somebody putting their arms out — and it
        /// is the same shot with the two of them swapped and everything else
        /// wrong. Nobody in the act says so.
        /// </para>
        /// <para>
        /// She is already dead and he is already dead and the scene is warm
        /// anyway. That is the joke the whole game has been building, and it is
        /// not a joke.
        /// </para>
        /// </remarks>
        private void WriteTheAlley()
        {
            // Act five broke both of these and every scene rebuilds them, so the
            // last act has to say so or it opens looking like act four.
            OpenFrame(0.01f);
            Grade(0f, 0.01f);

            Place(
                Backgrounds.AlleywayAftermath,
                "The alleyway", "路地", "کوچه");

            Hold(3.4f);

            Narrate(
                "Back to the alley, at the moment after the one the game stopped at.",
                "路地に戻る。ゲームが止まった、その次の瞬間に。",
                "برگشت به کوچه، به همان لحظه‌ی بعد از جایی که بازی ایستاد.");

            Hold(3.0f);

            Narrate(
                "He is not standing any more. He has not been standing for about a second, and the falling is not finished yet.",
                "彼はもう立っていない。一秒ほど前から立っていなくて、倒れきってもいない。",
                "دیگر ایستاده نیست. حدودِ یک ثانیه است که ایستاده نیست، و افتادنش هنوز تمام نشده.");

            Hold(3.6f);

            Narrate(
                "You have seen this before. Somebody going over all at once, with their hands nowhere near ready.",
                "この光景は見たことがある。いっぺんに倒れていく誰か。手はまるで間に合わないかたちで。",
                "این را قبلاً دیده‌ای. کسی که یک‌دفعه می‌افتد، و دست‌هایش اصلاً آماده نیست.");

            Hold(3.6f);

            Enter(Speaker.Yua, Portrait.Sad);

            Hold(2.6f);

            Narrate(
                "She was already on the ground, so she did not have to get there. She only had to put her arms out.",
                "彼女はもう地面にいたから、走る必要はなかった。ただ、腕を広げるだけでよかった。",
                "او از قبل روی زمین بود، پس لازم نبود برسد. فقط باید دست‌هایش را باز می‌کرد.");

            Hold(3.0f);

            SayWithSound(Speaker.Yua, Portrait.Crying, SfxId.Heartbeat, 0.55f,
                "Haru-pi.",
                "ハルぴ。",
                "هارو‌پی.");

            Hold(3.4f);

            Narrate(
                "He landed against her the way he had landed under her when they were nine, and she held on to him, and neither of those two facts helped anybody.",
                "九歳のときに彼女の下に入ったのと同じように、彼は彼女にもたれて落ちた。彼女は抱きとめた。そのどちらも、誰の役にも立たなかった。",
                "همان‌طور که در نُه‌سالگی زیرِ او افتاده بود، حالا روی او افتاد، و یوآ نگهش داشت، و هیچ‌کدام از این دو به دردِ کسی نخورد.");

            Hold(4.0f);

            Narrate(
                "The rabbit was still where it had fallen, a little way from her hand, on the side with the worn ear, facing up.",
                "うさぎは落ちたところにそのままあった。手から少し離れて、擦り切れた耳のほうを上にして。",
                "خرگوش هنوز همان‌جا بود که افتاده بود؛ کمی دورتر از دستش، روی همان طرفی که گوشش ساییده است، رو به بالا.");

            Hold(4.6f);

            ClearStage();

            Hold(3.0f);
        }

        // ---------------------------------------------------------------------
        //  2 · The credits
        // ---------------------------------------------------------------------

        /// <summary>
        /// A full-screen film, scored to the happiest music in the project.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The design document is very specific and the specificity is the whole
        /// idea: bright, high-energy J-Rock, of the kind a cheerful
        /// school-club anime plays over its end cards, laid directly on top of
        /// what just happened. Played straight it is unbearable, which is the
        /// intended effect and the reason it must not be scored sadly.
        /// </para>
        /// <para>
        /// Cut as a film rather than assembled from beats because it is the one
        /// sequence in the game with no interactivity, no localisation and no
        /// state — it is three minutes of edited footage and a song, and every
        /// engine feature that would be involved in faking that is a feature
        /// working against it.
        /// </para>
        /// </remarks>
        private void WriteTheCredits()
        {
            CutMusic();

            Hold(2.0f);

            PlayFilm(CreditsFilm);

            Hold(1.6f);
        }

        // ---------------------------------------------------------------------
        //  3 · The end
        // ---------------------------------------------------------------------

        /// <summary>
        /// The two sentences, and then the game takes the saves away.
        /// </summary>
        /// <remarks>
        /// The same closing words the other two endings get. A player who
        /// reached this one — the default, the one that needs no condition and
        /// that most people will finish on — is precisely who the second
        /// sentence is for, and giving them a lesser ending card because they
        /// did not know about a secret would be the game agreeing with Yua.
        /// </remarks>
        private void WriteTheEnd()
        {
            WriteClosingWords();

            EndGame();
        }
    }
}
