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
            string path = $"{ActsFolder}/{AssetName}.asset";
            ActAsset act = AssetDatabase.LoadAssetAtPath<ActAsset>(path);

            // Anything already in there was written by a person, and replacing it
            // is the one thing this cannot undo by being run again.
            if (act != null && act.Count > 0 && !EditorUtility.DisplayDialog(
                    $"Rebuild act {ActNumber:00}?",
                    $"{AssetName}.asset already holds {act.Count} beat(s).\n\n" +
                    "Building replaces every one of them with the script from the design document. " +
                    "Anything written in the Story Editor since the last build is lost.",
                    "Replace it",
                    "Leave it alone"))
            {
                return;
            }

            Script.Clear();

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
            act.ShowTitleCard = true;
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

        /// <summary>One spoken line.</summary>
        protected void Say(
            Speaker speaker, Portrait portrait, string english, string japanese, string persian)
        {
            Script.Add(new BeatData
            {
                Kind = StoryBeatKind.Line,
                Speaker = speaker,
                Portrait = portrait,
                Text = L(english, japanese, persian)
            });
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
