// -----------------------------------------------------------------------------
//  The Frayed Red String
//  ActScriptWriter.cs  (Editor only)
//
//  The vocabulary an act is written in, when it is written as code.
//
//  The Story Editor is the intended way to write an act, and every act built
//  through here can be opened and edited there the moment it exists. Some acts
//  are still easier to write in a text editor first: act two is a hundred and
//  forty lines of three-language script with a fourteen-part monologue in the
//  middle of it, and breaking a monologue at the right places is a text-editor
//  job, not a list-widget job.
//
//  What a subclass writes is a sequence of short verbs — Say, Narrate, Hold,
//  Place, Decide — that read, top to bottom, roughly the way the act plays.
//  Everything about creating the asset, reusing the one already on disk so
//  nothing pointing at it breaks, and rebuilding the act library afterwards is
//  handled here, once.
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using TheFrayedRedString.Audio;
using TheFrayedRedString.Narrative;
using UnityEditor;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Base class for an act written as code.</summary>
    public abstract class ActScriptWriter
    {
        /// <summary>The folder acts live in.</summary>
        protected const string ActsFolder = "Assets/Story/Acts";

        private const string LegAchePath = ActsFolder + "/Interlude_LegAche.asset";
        private const string MachineRoomPath = ActsFolder + "/Interlude_MachineRoom.asset";

        /// <summary>The script, assembled by the writing verbs below.</summary>
        protected readonly List<BeatData> Script = new List<BeatData>(256);

        private ActAsset _legAche;
        private ActAsset _machineRoom;

        /// <summary>Haru's leg, as a short act that turns up at random.</summary>
        protected ActAsset LegAche => _legAche;

        /// <summary>The machine room, as a short act that turns up at random.</summary>
        protected ActAsset MachineRoom => _machineRoom;

        /// <summary>1-based act number. Save slots and the title card use it.</summary>
        protected abstract int ActNumber { get; }

        /// <summary>File name, without the extension.</summary>
        protected abstract string AssetName { get; }

        /// <summary>The act's name, in all three languages.</summary>
        protected abstract LocalizedLine Title { get; }

        /// <summary>Music that starts when the act does.</summary>
        protected virtual string MusicTrack => MusicTracks.ForAct(ActNumber);

        /// <summary>Appends the whole act, in order.</summary>
        protected abstract void Write();

        /// <summary>
        /// Writes the act to disk, asking first if there is something there.
        /// </summary>
        /// <remarks>
        /// The asset already at the path is reused rather than replaced, so its
        /// GUID survives and everything pointing at it — the act library, an
        /// Interlude beat in another act, a scene reference — keeps pointing at
        /// it. Only the contents change.
        /// </remarks>
        public void BuildAsset()
        {
            BuildAsset(RebuildPolicy.Ask);
        }

        /// <summary>What to do about an act that already has beats in it.</summary>
        public enum RebuildPolicy
        {
            /// <summary>Put the question to whoever pressed the menu item.</summary>
            Ask,

            /// <summary>Leave anything that already has beats exactly as it is.</summary>
            OnlyIfEmpty,

            /// <summary>
            /// Rewrite it unless somebody has edited it since it was generated.
            /// </summary>
            /// <remarks>
            /// The right default for a setup command, and the one that was
            /// missing. "Has beats in it" was being used to mean "somebody wrote
            /// this by hand", and it does not: an act generated last week has
            /// beats in it too, and leaving that alone is how a game gets built
            /// from a script its own builder no longer contains.
            /// </remarks>
            IfUnedited,

            /// <summary>Replace it, without asking.</summary>
            Always
        }

        /// <summary>
        /// A fingerprint of a script, for telling a hand edit apart from an old
        /// build.
        /// </summary>
        /// <remarks>
        /// Covers every field a person could change in the Story Editor and
        /// nothing else, so re-running a builder that has not changed produces
        /// the same string and re-running one that has does not. Order matters
        /// and is included, because reordering beats is an edit.
        /// </remarks>
        public static string SignatureOf(System.Collections.Generic.IReadOnlyList<BeatData> beats)
        {
            if (beats == null)
            {
                return string.Empty;
            }

            System.Text.StringBuilder text = new System.Text.StringBuilder(4096);

            for (int i = 0; i < beats.Count; i++)
            {
                BeatData beat = beats[i];

                if (beat == null)
                {
                    text.Append("|null");
                    continue;
                }

                text.Append('|').Append((int)beat.Kind)
                    .Append(',').Append((int)beat.Speaker)
                    .Append(',').Append((int)beat.Portrait)
                    .Append(',').Append(beat.Text.English)
                    .Append(',').Append(beat.Text.Japanese)
                    .Append(',').Append(beat.Text.Persian)
                    .Append(',').Append(beat.Background)
                    .Append(',').Append(beat.Caption.English)
                    .Append(',').Append(beat.MusicTrack)
                    .Append(',').Append(beat.PlaySound ? (int)beat.Sound : -1)
                    .Append(',').Append(beat.Seconds.ToString("0.###"))
                    .Append(',').Append(beat.FadeSeconds.ToString("0.###"))
                    .Append(',').Append(beat.MeasurePatience ? 1 : 0)
                    .Append(',').Append(beat.YuaOverridesKindness ? 1 : 0)
                    .Append(',').Append(beat.OverrideLine.English)
                    .Append(',').Append(beat.VoiceClip)
                    .Append(',').Append(beat.Film)
                    .Append(',').Append(beat.TypeSpeed.ToString("0.###"))
                    .Append(',').Append(beat.Amount.ToString("0.###"))
                    .Append(',').Append(beat.Chance.ToString("0.###"))
                    .Append(',').Append(beat.Interlude != null ? beat.Interlude.name : string.Empty);

                if (beat.Choices == null)
                {
                    continue;
                }

                for (int c = 0; c < beat.Choices.Length; c++)
                {
                    text.Append(';').Append((int)beat.Choices[c].Tone)
                        .Append(':').Append(beat.Choices[c].Text.English)
                        .Append(':').Append(beat.Choices[c].BranchLength);
                }
            }

            // A short stable digest rather than the whole string: this is stored
            // in every act asset and read on every setup run.
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(text.ToString()));
                return System.BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }

        /// <summary>
        /// True when nobody has edited this act since a builder wrote it.
        /// </summary>
        /// <remarks>
        /// An act with no signature has never been generated — act one, or
        /// anything started from scratch in the Story Editor — and is treated as
        /// hand-written, which is the safe direction to be wrong in.
        /// </remarks>
        public static bool IsUntouchedSinceGenerated(ActAsset act)
        {
            if (act == null || string.IsNullOrEmpty(act.GeneratedSignature))
            {
                return false;
            }

            return string.Equals(act.GeneratedSignature, SignatureOf(act.Beats), System.StringComparison.Ordinal);
        }

        /// <summary>
        /// Writes the act to disk under a given policy.
        /// </summary>
        /// <remarks>
        /// The policy exists for the one-press "prepare the whole game" command,
        /// which builds nine assets and cannot put nine dialogs in front of
        /// somebody. It asks once, up front, and then passes the answer down.
        /// </remarks>
        public void BuildAsset(RebuildPolicy policy)
        {
            string path = $"{ActsFolder}/{AssetName}.asset";
            ActAsset act = AssetDatabase.LoadAssetAtPath<ActAsset>(path);

            // Anything already in there may have been written by a person, and
            // replacing it is the one thing this cannot undo by being run again.
            if (act != null && act.Count > 0)
            {
                if (policy == RebuildPolicy.OnlyIfEmpty)
                {
                    return;
                }

                // Older than its builder is not the same thing as edited by
                // hand, and treating them alike is how a whole playthrough got
                // made from a script that had already been rewritten.
                if (policy == RebuildPolicy.IfUnedited && !IsUntouchedSinceGenerated(act))
                {
                    Debug.Log(
                        $"[Story] {AssetName} has been edited since it was generated, so it was left alone. " +
                        "Use The Frayed Red String ▸ Build Act " + $"{ActNumber:00}" +
                        " From The Story Document to overwrite it deliberately.");

                    return;
                }

                if (policy == RebuildPolicy.Ask && !EditorUtility.DisplayDialog(
                        $"Rebuild act {ActNumber:00}?",
                        $"{AssetName}.asset already holds {act.Count} beat(s).\n\n" +
                        "Building replaces every one of them with the script from the design document. " +
                        "Anything written in the Story Editor since the last build is lost.",
                        "Replace it",
                        "Leave it alone"))
                {
                    return;
                }
            }

            Script.Clear();
            _childhood = false;

            _legAche = LoadInterlude(LegAchePath);
            _machineRoom = LoadInterlude(MachineRoomPath);

            Write();

            if (act == null)
            {
                AssetPaths.EnsureFolder("Assets/Story");
                AssetPaths.EnsureFolder(ActsFolder);

                act = ScriptableObject.CreateInstance<ActAsset>();
                AssetDatabase.CreateAsset(act, path);
            }

            act.ActNumber = ActNumber;
            act.Title = Title;
            act.MusicTrack = MusicTrack;
            act.GeneratedSignature = SignatureOf(Script);
            // An act with no number is an interlude or an ending: it is played
            // inside somebody else's scene and has no business naming itself
            // over a darkened screen, least of all as "Season 01".
            act.ShowTitleCard = ActNumber > 0;
            act.Beats = new List<BeatData>(Script);

            EditorUtility.SetDirty(act);
            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(path);

            // So ActLibrary can find it, and so the act's scene has something to
            // play.
            StoryAssetBuilder.Rebuild();

            Selection.activeObject = act;
            EditorGUIUtility.PingObject(act);

            Debug.Log(
                $"[Story] Act {ActNumber:00} built: {act.Count} beats, {act.SpokenCount()} of them spoken. " +
                "Open The Frayed Red String ▸ Story Editor to read it, and its Check tab before playing it.");

            Script.Clear();
        }

        private static ActAsset LoadInterlude(string path)
        {
            ActAsset interlude = AssetDatabase.LoadAssetAtPath<ActAsset>(path);

            if (interlude == null)
            {
                Debug.LogWarning(
                    $"[Story] The interlude at '{path}' is missing. Interlude beats pointing at it are " +
                    "written anyway and simply do nothing, so the act plays correctly without them; the " +
                    "Check tab lists each one.");
            }

            return interlude;
        }

        // ---------------------------------------------------------------------
        //  The writing verbs
        //
        //  One beat each, and deliberately tiny, so that a script written with
        //  them reads as a script and not as a list of object initialisers.
        // ---------------------------------------------------------------------

        /// <summary>One line in all three languages.</summary>
        protected static LocalizedLine L(string english, string japanese, string persian)
        {
            return new LocalizedLine(english, japanese, persian);
        }

        /// <summary>Moves to a new place and names it in the corner.</summary>
        protected void Place(string background, string english, string japanese, string persian)
        {
            Script.Add(new BeatData
            {
                Kind = StoryBeatKind.Background,
                Background = background,
                Caption = L(english, japanese, persian)
            });
        }

        /// <summary>
        /// Cuts straight to another picture, on the frame.
        /// </summary>
        /// <remarks>
        /// No chime, no crossfade and no place name. Act six's closing montage
        /// is the only thing in the game that wants this — everywhere else, a
        /// change of location is somebody going somewhere and should take the
        /// second it takes.
        /// </remarks>
        protected void CutTo(string background)
        {
            Script.Add(new BeatData
            {
                Kind = StoryBeatKind.Background,
                Background = background,
                FadeSeconds = 0f
            });
        }

        /// <summary>Starts a music track by name.</summary>
        protected void SetMusic(string track)
        {
            Script.Add(new BeatData { Kind = StoryBeatKind.Music, MusicTrack = track });
        }

        /// <summary>One line of narration. Nobody on stage is speaking it.</summary>
        protected void Narrate(string english, string japanese, string persian)
        {
            Say(Speaker.Narrator, Portrait.Unchanged, english, japanese, persian);
        }

        /// <summary>
        /// One line of narration that also measures patience.
        /// </summary>
        /// <remarks>
        /// Written as narration on purpose. A character asking the player to
        /// wait would be the game handing them the answer; a sentence that
        /// simply describes two people standing still is the same instruction,
        /// given to nobody.
        /// </remarks>
        protected void Listen(string english, string japanese, string persian)
        {
            Say(Speaker.Narrator, Portrait.Unchanged, english, japanese, persian);
            Script[Script.Count - 1].MeasurePatience = true;
        }

        /// <summary>
        /// One short line in the nine-year-old's voice, from inside a
        /// seventeen-year-old.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Written immediately before each of the measured silences, and it is
        /// the only sign the game gives that one of them is a place worth
        /// staying. The plate says "aged nine", the line is a fragment nobody
        /// asked for, and nothing acknowledges either.
        /// </para>
        /// <para>
        /// The fragments are all act six's, said years early. On a first
        /// playthrough they are strange and slightly cold; after the flashback
        /// they are the exact sentences those two children said at that door,
        /// and the player who waited already knows what they were sitting with.
        /// </para>
        /// <para>
        /// Portrait is deliberately left unchanged. Nobody's face moves, nobody
        /// arrives, and the sprite on stage stays the adult one — the child is
        /// only in the plate and the voice.
        /// </para>
        /// </remarks>
        protected void ChildVoice(Speaker child, string english, string japanese, string persian)
        {
            Say(child, Portrait.Unchanged, english, japanese, persian);
        }

        /// <summary>
        /// The character turns away from the scene and speaks to the person
        /// holding the controller.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Four scenes in the game do this and all four were being played at the
        /// same weight as somebody ordering a coffee. From here the room dims
        /// away behind the speaker, the line comes slower than the game has ever
        /// typed, and the box sits on a heavier ground — so that when Yua says
        /// "you are there" or Haru asks the player to look after her, it reads
        /// as the thing it is.
        /// </para>
        /// <para>
        /// Always close it with <see cref="EndAside"/>. It is a mode, and an act
        /// that leaves it on plays its next ordinary scene in a dark room.
        /// </para>
        /// </remarks>
        protected void BeginAside()
        {
            Script.Add(new BeatData { Kind = StoryBeatKind.EnterAside });
        }

        /// <summary>Gives the scene back to the room.</summary>
        protected void EndAside()
        {
            Script.Add(new BeatData { Kind = StoryBeatKind.ExitAside });
        }

        /// <summary>One spoken line.</summary>
        protected void Say(
            Speaker speaker, Portrait portrait, string english, string japanese, string persian)
        {
            Script.Add(new BeatData
            {
                Kind = StoryBeatKind.Line,
                Speaker = _childhood ? AsChild(speaker) : speaker,
                Portrait = portrait,
                Text = L(english, japanese, persian),

                // Every line of the episode, not just one of them. A player who
                // stops anywhere inside it is offered the same thing.
                MeasurePatience = _childhood
            });
        }

        /// <summary>True between <see cref="BeginChildhood"/> and its end.</summary>
        private bool _childhood;

        /// <summary>
        /// Opens an episode where the older self is not the one answering.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Two things happen to every line written between here and
        /// <see cref="EndChildhood"/>, and they are two halves of one idea. The
        /// name plate reads as the nine-year-old for as long as the episode
        /// lasts, and every line in it measures the player's patience — so the
        /// sign and the thing it is a sign of cover exactly the same stretch of
        /// the game.
        /// </para>
        /// <para>
        /// The earlier version put the plate on a single line and the silence on
        /// the one after it, which meant a player who read the fragment, pressed
        /// on, and then stopped one line later had done the right thing at the
        /// wrong instant and got nothing for it. An episode has no wrong instant
        /// in it.
        /// </para>
        /// <para>
        /// Only Yua and Haru are moved. Narration inside the episode is still
        /// narration — it keeps its plate and gains only the silence — and a
        /// line already written in a child's voice is left exactly as it is.
        /// </para>
        /// <para>
        /// The sprite on stage does not change. Nobody arrives, nobody's face
        /// moves; the picture stays the seventeen-year-old and only the plate
        /// and the voice are nine.
        /// </para>
        /// </remarks>
        protected void BeginChildhood()
        {
            _childhood = true;
        }

        /// <summary>
        /// Closes the episode. The plates go back to Yua and Haru on the next
        /// line written.
        /// </summary>
        /// <remarks>
        /// Put it on the line where the subject is dropped and a new
        /// conversation starts — the "anyway", the "let us go" — because that
        /// line is the one the player reads as the moment it is over.
        /// </remarks>
        protected void EndChildhood()
        {
            _childhood = false;
        }

        /// <summary>The nine-year-old of whoever this is.</summary>
        private static Speaker AsChild(Speaker speaker)
        {
            switch (speaker)
            {
                case Speaker.Yua: return Speaker.YuaChild;
                case Speaker.Haru: return Speaker.HaruChild;
                default: return speaker;
            }
        }

        /// <summary>One spoken line with a cue on the same frame.</summary>
        protected void SayWithSound(
            Speaker speaker,
            Portrait portrait,
            SfxId sound,
            float volume,
            string english,
            string japanese,
            string persian)
        {
            Say(speaker, portrait, english, japanese, persian);

            BeatData beat = Script[Script.Count - 1];
            beat.PlaySound = true;
            beat.Sound = sound;
            beat.SoundVolume = volume;
        }

        protected void Enter(Speaker speaker, Portrait portrait)
        {
            Script.Add(new BeatData
            {
                Kind = StoryBeatKind.Enter,
                Speaker = speaker,
                Portrait = portrait
            });
        }

        protected void Exit(Speaker speaker)
        {
            Script.Add(new BeatData { Kind = StoryBeatKind.Exit, Speaker = speaker });
        }

        protected void ClearStage()
        {
            Script.Add(new BeatData { Kind = StoryBeatKind.ClearStage });
        }

        /// <summary>Silence on the picture, with the box out of the way.</summary>
        protected void Hold(float seconds)
        {
            Script.Add(new BeatData { Kind = StoryBeatKind.Beat, Seconds = seconds });
        }

        // ---------------------------------------------------------------------
        //  Wordless sequences
        //
        //  A stretch of the act where nobody says anything and the picture does
        //  the telling: the two of them eating a shared lunch, the two of them
        //  finishing a drink. One pose each, then the picture holds for a
        //  couple of seconds, then the next one — flat cuts, no crossfade,
        //  because a character changing expression on somebody already standing
        //  there is a cut everywhere else in this game too.
        //
        //  Deliberately built out of beats that already existed rather than out
        //  of a new kind of beat. There is nothing here a pose change and a held
        //  picture do not already do, and an act asset that needed a new beat
        //  kind to hold an animation would be one more thing the Story Editor,
        //  the save system and the readiness report each had to learn.
        // ---------------------------------------------------------------------

        /// <summary>How long one picture of a wordless sequence stays up.</summary>
        /// <remarks>
        /// Two seconds. Long enough to read what has changed in the picture,
        /// short enough that ten of them is a scene rather than an intermission.
        /// </remarks>
        protected const float CelSeconds = 2f;

        /// <summary>
        /// One picture of a wordless sequence, with both of them on the same
        /// moment of it.
        /// </summary>
        /// <remarks>
        /// The single argument is the usual case and the reason the poses of a
        /// sequence are named for the moment rather than for the drawing: Yua
        /// lifting a piece of sushi and Haru lifting an octopus sausage are the
        /// same instant of the same meal, so the script says it once.
        /// </remarks>
        protected void Cel(Portrait pose, float seconds = CelSeconds)
        {
            Cel(pose, pose, seconds);
        }

        /// <summary>
        /// One picture of a wordless sequence where the two of them are not on
        /// the same moment of it.
        /// </summary>
        /// <remarks>
        /// <see cref="Portrait.Unchanged"/> for either of them leaves that one
        /// exactly as the line before this left them, which is what the opening
        /// picture of a sequence usually wants: she has produced a lunch box and
        /// he has not reacted to it yet.
        /// </remarks>
        protected void Cel(Portrait yua, Portrait haru, float seconds = CelSeconds)
        {
            if (yua != Portrait.Unchanged)
            {
                Enter(Speaker.Yua, yua);
            }

            if (haru != Portrait.Unchanged)
            {
                Enter(Speaker.Haru, haru);
            }

            Hold(seconds);
        }

        /// <summary>One cue on its own, with nothing said over it.</summary>
        protected void Cue(SfxId sound, float volume)
        {
            Script.Add(new BeatData
            {
                Kind = StoryBeatKind.Sound,
                PlaySound = true,
                Sound = sound,
                SoundVolume = volume
            });
        }

        /// <summary>Takes the music away and leaves the room quiet.</summary>
        protected void StopMusic()
        {
            Script.Add(new BeatData { Kind = StoryBeatKind.Music, MusicTrack = string.Empty });
        }

        /// <summary>
        /// Stops the music in one frame, with no fade.
        /// </summary>
        /// <remarks>
        /// Not the same beat as <see cref="StopMusic"/>, which takes about a
        /// second and reads as a scene ending. This is the design document's
        /// 0 ms cut and reads as something going wrong.
        /// </remarks>
        protected void CutMusic()
        {
            Script.Add(new BeatData { Kind = StoryBeatKind.CutMusic });
        }

        /// <summary>
        /// One spoken line with a recording behind it.
        /// </summary>
        /// <param name="voiceClip">
        /// File name in <c>Assets/Audio/Voice</c>, without the extension. A name
        /// with no file behind it is not an error — the line plays with the
        /// typewriter voice, exactly as an unvoiced one does — so a whole act
        /// can be written and played through before a word of it is recorded.
        /// </param>
        protected void SayVoiced(
            Speaker speaker,
            Portrait portrait,
            string voiceClip,
            string english,
            string japanese,
            string persian)
        {
            Say(speaker, portrait, english, japanese, persian);
            Script[Script.Count - 1].VoiceClip = voiceClip;
        }

        /// <summary>Haru drags the dialogue box off the bottom of the screen.</summary>
        protected void PullDownDialogue(float seconds = 2.0f)
        {
            Script.Add(new BeatData { Kind = StoryBeatKind.PullDownDialogue, Seconds = seconds });
        }

        /// <summary>Opens the frame the game has been played inside since act one.</summary>
        protected void OpenFrame(float seconds = 2.4f)
        {
            Script.Add(new BeatData { Kind = StoryBeatKind.OpenFrame, Seconds = seconds });
        }

        /// <summary>
        /// Changes how much of the pastel veil the picture is seen through.
        /// </summary>
        /// <param name="amount">1 is how acts one to four look. 0 is the art bare.</param>
        protected void Grade(float amount, float seconds)
        {
            Script.Add(new BeatData { Kind = StoryBeatKind.Grade, Amount = amount, Seconds = seconds });
        }

        /// <summary>Throws a colour across the screen and leaves it there.</summary>
        /// <param name="seconds">Zero is a cut, which is what this is for.</param>
        protected void Stain(Color colour, float seconds = 0f)
        {
            Script.Add(new BeatData { Kind = StoryBeatKind.Stain, Tint = colour, Seconds = seconds });
        }

        /// <summary>Plays a film, full screen, over everything.</summary>
        /// <param name="film">
        /// File name in <c>Assets/Video</c> or <c>StreamingAssets/Video</c>,
        /// without the extension. A name with no film behind it carries straight
        /// on, so an ending can be built before its video exists.
        /// </param>
        protected void PlayFilm(string film)
        {
            Script.Add(new BeatData { Kind = StoryBeatKind.Video, Film = film });
        }

        /// <summary>
        /// The story is over: the saves are erased and the game returns to the
        /// title.
        /// </summary>
        protected void EndGame()
        {
            Script.Add(new BeatData { Kind = StoryBeatKind.EndGame });
        }

        /// <summary>
        /// The two sentences the game closes on, whichever ending was reached.
        /// </summary>
        /// <remarks>
        /// <para>
        /// From the design document, and the only time the game explains itself.
        /// They are shared by all three endings on purpose: the bitter one earns
        /// them as much as the secret one does, and a player who got the worst
        /// outcome is exactly who the second sentence is addressed to.
        /// </para>
        /// <para>
        /// Written here rather than in each ending so that changing the wording
        /// changes it in three places at once — these two paragraphs are the
        /// thesis of the whole thing and will be rewritten more than any other
        /// lines in the game.
        /// </para>
        /// </remarks>
        protected void WriteClosingWords()
        {
            Hold(2.6f);

            Narrate(
                "What you decide and what you try for stays in the world, even when nobody notices it, and even when you never get to see what it did.",
                "あなたが決めたこと、努めたことは、世界に残る。誰にも気づかれなくても、その結果を自分の目で見られなくても。",
                "تصمیم‌ها و تلاش‌های شما همیشه در جهان باقی می‌ماند، حتی اگر کسی متوجه آن‌ها نشود، و حتی اگر خودتان نتیجه‌ی تصمیم‌هایتان را به چشم نبینید.");

            Hold(4.2f);

            Narrate(
                "That is not a reason to give up, and it is not evidence that any of it came to nothing.",
                "それは、あきらめる理由にはならない。何にもならなかった証拠にも、ならない。",
                "نه دلیلی برای تسلیم شدن است و نه دلیلی بر بی‌نتیجه ماندنِ تصمیم‌های شما.");

            Hold(4.6f);

            Narrate(
                "Sometimes waiting five minutes for somebody, or listening to what hurts them for five minutes, is enough to take some of the weight off what they are carrying.",
                "ときには、誰かのために五分待つこと、その痛みを五分聞くことが、その人の背負っているものを少し軽くする。",
                "گاهی اوقات پنج دقیقه صبر کردن برای آدم‌ها، یا پنج دقیقه گوش دادن به دردشان، می‌تواند بارِ سنگینی را که روی دوششان حمل می‌کنند سبک کند.");

            Hold(6.0f);
        }

        /// <summary>
        /// Hands the act over to itself. From here the player cannot advance,
        /// skip or hurry anything.
        /// </summary>
        /// <remarks>
        /// Act six. Every line after this holds for as long as it takes to read
        /// and then goes on its own. Pausing still works, on purpose.
        /// </remarks>
        protected void BeginFilm()
        {
            Script.Add(new BeatData { Kind = StoryBeatKind.EnterCinema });
        }

        /// <summary>Gives the player the story back.</summary>
        protected void EndFilm()
        {
            Script.Add(new BeatData { Kind = StoryBeatKind.ExitCinema });
        }

        /// <summary>Takes a stain back off the screen.</summary>
        protected void ClearStain(float seconds)
        {
            Stain(new Color(0f, 0f, 0f, 0f), seconds);
        }

        /// <summary>
        /// A blue option and a green one, with Yua ready to overrule the blue.
        /// </summary>
        /// <remarks>
        /// Blue first every time. The kind option being the one on top, and the
        /// one the eye lands on, is what makes the refusal cost something.
        /// </remarks>
        protected void Decide(
            string kindEnglish, string kindJapanese, string kindPersian,
            string cruelEnglish, string cruelJapanese, string cruelPersian,
            string refusalEnglish, string refusalJapanese, string refusalPersian)
        {
            Script.Add(new BeatData
            {
                Kind = StoryBeatKind.Choice,
                Choices = new[]
                {
                    new ChoiceData { Tone = ChoiceTone.Kind, Text = L(kindEnglish, kindJapanese, kindPersian) },
                    new ChoiceData { Tone = ChoiceTone.Cruel, Text = L(cruelEnglish, cruelJapanese, cruelPersian) }
                },
                YuaOverridesKindness = true,
                OverrideLine = L(refusalEnglish, refusalJapanese, refusalPersian)
            });
        }

        /// <summary>
        /// Two white options that carry no weight and are not counted.
        /// </summary>
        /// <remarks>
        /// Worth having exactly because most of them are not like this. A game
        /// where every button is a moral test teaches the player to stop reading
        /// them; one harmless choice restores the doubt the blue and green ones
        /// depend on.
        /// </remarks>
        protected void DecideIdly(
            string firstEnglish, string firstJapanese, string firstPersian,
            string secondEnglish, string secondJapanese, string secondPersian)
        {
            Script.Add(new BeatData
            {
                Kind = StoryBeatKind.Choice,
                Choices = new[]
                {
                    new ChoiceData { Tone = ChoiceTone.Neutral, Text = L(firstEnglish, firstJapanese, firstPersian) },
                    new ChoiceData { Tone = ChoiceTone.Neutral, Text = L(secondEnglish, secondJapanese, secondPersian) }
                }
            });
        }

        /// <summary>
        /// Two white options that carry no weight, and a stretch of script
        /// behind each of them.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The version of this without branches was the honest mistake of the
        /// first act one: twelve buttons, nothing behind any of them, and the
        /// player learning across an hour that pressing things does not matter —
        /// which is the exact belief act two's refused blue button needs them
        /// not to have. A white option earns its place by being answered.
        /// </para>
        /// <para>
        /// Write each road as a lambda holding the usual verbs. What it adds to
        /// the script is measured and recorded on the option, so the two roads
        /// sit end to end after the choice and the director walks only the one
        /// that was taken. Both roads then rejoin whatever is written after this
        /// call, which is what keeps an act with a dozen choices in it one act
        /// long instead of four thousand.
        /// </para>
        /// <para>
        /// A road may hold spoken lines and nothing else. Anything that moves a
        /// character, changes the picture or touches the music belongs on the
        /// far side of the join, where both roads can see it — see
        /// <see cref="ChoiceData.BranchLength"/> for why the save system depends
        /// on that being true. Breaking the rule is reported here rather than
        /// left for the Check tab, because a builder can say so with the line
        /// number attached.
        /// </para>
        /// </remarks>
        protected void DecideIdly(
            string firstEnglish, string firstJapanese, string firstPersian, Action firstRoad,
            string secondEnglish, string secondJapanese, string secondPersian, Action secondRoad)
        {
            BeatData choice = new BeatData
            {
                Kind = StoryBeatKind.Choice,
                Choices = new[]
                {
                    new ChoiceData { Tone = ChoiceTone.Neutral, Text = L(firstEnglish, firstJapanese, firstPersian) },
                    new ChoiceData { Tone = ChoiceTone.Neutral, Text = L(secondEnglish, secondJapanese, secondPersian) }
                }
            };

            Script.Add(choice);

            choice.Choices[0].BranchLength = MeasureRoad(firstRoad, firstEnglish);
            choice.Choices[1].BranchLength = MeasureRoad(secondRoad, secondEnglish);
        }

        /// <summary>
        /// Runs one road of a choice and returns how many beats it wrote.
        /// </summary>
        private int MeasureRoad(Action road, string label)
        {
            int before = Script.Count;

            road?.Invoke();

            int written = Script.Count - before;

            for (int i = before; i < Script.Count; i++)
            {
                if (Script[i].Kind == StoryBeatKind.Line)
                {
                    continue;
                }

                Debug.LogError(
                    $"[Story] The '{label}' road of a choice contains a {Script[i].Kind} beat. A road may " +
                    "hold spoken lines only — anything that moves a character or changes the picture has to " +
                    "go after the two roads rejoin, or a save taken on one road will come back wearing the " +
                    "other one's stage.");
            }

            return written;
        }

        /// <summary>
        /// A short act played inline, some of the time.
        /// </summary>
        /// <remarks>
        /// Always written immediately before a change of scene. An interlude is
        /// an act and can set its own background and bring on its own people; if
        /// the act it interrupts has to go on standing in the same room
        /// afterwards, whatever it leaves behind is left behind. Placed here,
        /// the beats that follow replace all of it.
        /// </remarks>
        protected void Maybe(ActAsset interlude, float chance)
        {
            Script.Add(new BeatData
            {
                Kind = StoryBeatKind.Interlude,
                Interlude = interlude,
                Chance = chance
            });
        }
    }
}
