// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryScript.cs
//
//  An act, expressed in code.
//
//  SUPERSEDED BY ActAsset. This file and the three that use it — Act01Script,
//  StoryInterludes and Act01Strings — exist only so that the act already written
//  in C# can be imported into an asset exactly as it stands. Run
//  The Frayed Red String > Story Editor > Import Act 01 From Code once, check the
//  result, and then all four files can be deleted: nothing at runtime reads them.
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using TheFrayedRedString.Audio;

namespace TheFrayedRedString.Narrative
{
    /// <summary>One option in a choice.</summary>
    public readonly struct StoryChoice
    {
        /// <summary>Localisation key of the option's text.</summary>
        public readonly string TextKey;

        /// <summary>Which counter this option feeds, and how it is drawn.</summary>
        public readonly ChoiceTone Tone;

        public StoryChoice(string textKey, ChoiceTone tone)
        {
            TextKey = textKey;
            Tone = tone;
        }
    }

    /// <summary>One instruction in a script.</summary>
    public sealed class StoryBeat
    {
        public StoryBeatKind Kind;

        /// <summary>Who is speaking, entering or leaving.</summary>
        public Speaker Speaker;

        /// <summary>Expression to adopt, for <see cref="StoryBeatKind.Enter"/>.</summary>
        public Portrait Portrait;

        /// <summary>Localisation key, for a line, caption or title card.</summary>
        public string TextKey;

        /// <summary>Background or music asset name.</summary>
        public string AssetName;

        /// <summary>Sound to fire alongside this beat.</summary>
        public SfxId? Sound;

        /// <summary>Seconds, for a hold.</summary>
        public float Seconds;

        /// <summary>Options, for a choice.</summary>
        public StoryChoice[] Choices;

        /// <summary>Sub-script, for an interlude.</summary>
        public StoryScript Interlude;

        /// <summary>Probability an interlude plays, 0..1.</summary>
        public float Chance;
    }

    /// <summary>
    /// A sequence of beats, with a fluent builder for writing one readably.
    /// </summary>
    public sealed class StoryScript
    {
        private readonly List<StoryBeat> _beats = new List<StoryBeat>(128);

        /// <summary>Localisation key of this script's title, when it is an act.</summary>
        public string TitleKey { get; }

        /// <summary>Act number this script belongs to, 1-based.</summary>
        public int ActNumber { get; }

        public StoryScript(string titleKey = null, int actNumber = 0)
        {
            TitleKey = titleKey;
            ActNumber = actNumber;
        }

        /// <summary>The beats, in order.</summary>
        public IReadOnlyList<StoryBeat> Beats => _beats;

        /// <summary>Number of beats.</summary>
        public int Count => _beats.Count;

        /// <summary>The beat at an index, or <c>null</c> when out of range.</summary>
        public StoryBeat At(int index)
        {
            return index >= 0 && index < _beats.Count ? _beats[index] : null;
        }

        // ---------------------------------------------------------------------
        //  Builder
        // ---------------------------------------------------------------------

        /// <summary>Moves the scene to a new location.</summary>
        public StoryScript Place(string backgroundName, string captionKey = null)
        {
            Add(new StoryBeat
            {
                Kind = StoryBeatKind.Background,
                AssetName = backgroundName,
                TextKey = captionKey
            });

            return this;
        }

        /// <summary>Narration: no name plate, no portrait change.</summary>
        public StoryScript Narrate(string textKey)
        {
            return Line(Speaker.Narrator, textKey, Portrait.Unchanged);
        }

        /// <summary>Yua speaks.</summary>
        public StoryScript Yua(string textKey, Portrait portrait = Portrait.Unchanged)
        {
            return Line(Speaker.Yua, textKey, portrait);
        }

        /// <summary>Haru speaks.</summary>
        public StoryScript Haru(string textKey, Portrait portrait = Portrait.Unchanged)
        {
            return Line(Speaker.Haru, textKey, portrait);
        }

        /// <summary>
        /// One line of dialogue.
        /// </summary>
        /// <remarks>
        /// A portrait given here is applied before the line is typed, which is
        /// why almost every line in the script carries one: an expression that
        /// lands with the words reads as the character reacting, and one that
        /// lands a beat later reads as the game catching up.
        /// </remarks>
        public StoryScript Line(Speaker speaker, string textKey, Portrait portrait = Portrait.Unchanged)
        {
            Add(new StoryBeat
            {
                Kind = StoryBeatKind.Line,
                Speaker = speaker,
                TextKey = textKey,
                Portrait = portrait
            });

            return this;
        }

        /// <summary>Brings a character on stage.</summary>
        public StoryScript Enter(Speaker speaker, Portrait portrait = Portrait.Neutral)
        {
            Add(new StoryBeat
            {
                Kind = StoryBeatKind.Enter,
                Speaker = speaker,
                Portrait = portrait
            });

            return this;
        }

        /// <summary>Takes a character off stage.</summary>
        public StoryScript Exit(Speaker speaker)
        {
            Add(new StoryBeat { Kind = StoryBeatKind.Exit, Speaker = speaker });
            return this;
        }

        /// <summary>Clears every character.</summary>
        public StoryScript ClearStage()
        {
            Add(new StoryBeat { Kind = StoryBeatKind.ClearStage });
            return this;
        }

        /// <summary>Names the current place in the corner.</summary>
        public StoryScript Caption(string captionKey)
        {
            Add(new StoryBeat { Kind = StoryBeatKind.Caption, TextKey = captionKey });
            return this;
        }

        /// <summary>Shows the act's title card.</summary>
        public StoryScript TitleCard()
        {
            Add(new StoryBeat { Kind = StoryBeatKind.TitleCard, TextKey = TitleKey });
            return this;
        }

        /// <summary>Fires a sound without waiting for it.</summary>
        public StoryScript Sound(SfxId sound, float volume = 1f)
        {
            Add(new StoryBeat
            {
                Kind = StoryBeatKind.Sound,
                Sound = sound,
                Seconds = volume
            });

            return this;
        }

        /// <summary>Starts a music loop. Pass <c>null</c> to stop the music.</summary>
        public StoryScript Music(string trackName)
        {
            Add(new StoryBeat { Kind = StoryBeatKind.Music, AssetName = trackName });
            return this;
        }

        /// <summary>
        /// Holds on the picture with the dialogue box hidden.
        /// </summary>
        /// <remarks>
        /// The most under-used instruction in the set and one of the most
        /// valuable. A silence with nothing on screen but two characters is how
        /// act one says the things it is not willing to write down.
        /// </remarks>
        public StoryScript Beat(float seconds)
        {
            Add(new StoryBeat { Kind = StoryBeatKind.Beat, Seconds = seconds });
            return this;
        }

        /// <summary>Offers the player a choice.</summary>
        public StoryScript Choice(params StoryChoice[] options)
        {
            Add(new StoryBeat { Kind = StoryBeatKind.Choice, Choices = options });
            return this;
        }

        /// <summary>
        /// Splices in a sub-script with a given probability.
        /// </summary>
        /// <param name="build">
        /// Deferred so a script that is never reached costs nothing to build,
        /// and so the same interlude can be written once and dropped into
        /// several acts.
        /// </param>
        /// <param name="chance">0..1. Use 1 for a beat the act depends on.</param>
        public StoryScript Interlude(Func<StoryScript> build, float chance = 1f)
        {
            Add(new StoryBeat
            {
                Kind = StoryBeatKind.Interlude,
                Interlude = build?.Invoke(),
                Chance = chance
            });

            return this;
        }

        /// <summary>Marks the end of the act.</summary>
        public StoryScript End()
        {
            Add(new StoryBeat { Kind = StoryBeatKind.End });
            return this;
        }

        private void Add(StoryBeat beat)
        {
            _beats.Add(beat);
        }
    }
}
