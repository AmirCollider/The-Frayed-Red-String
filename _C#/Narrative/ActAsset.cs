// -----------------------------------------------------------------------------
//  The Frayed Red String
//  ActAsset.cs
//
//  An act, as an asset rather than as code.
//
//  Everything the story is made of lives here: the lines, all three
//  translations of each, who says them, what face they wear, where the scene is,
//  what it sounds like and how long it holds. Nothing about an act needs a
//  recompile, and nothing about writing one needs C#.
//
//  The Story Editor window (The Frayed Red String > Story Editor) is the
//  intended way to edit these. The Inspector works too — every field is plain
//  serialized data — but the window is laid out for writing rather than for
//  poking at a list.
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using TheFrayedRedString.Audio;
using TheFrayedRedString.Localization;
using UnityEngine;

namespace TheFrayedRedString.Narrative
{
    /// <summary>One line of text in every language the game ships in.</summary>
    /// <remarks>
    /// Kept beside the line it belongs to rather than in a central string table
    /// behind a key. A key table is the right shape for interface text, which is
    /// reused in many places and changes rarely; it is the wrong shape for a
    /// script, where every line is used exactly once and the person editing it
    /// wants to see the three languages side by side.
    /// </remarks>
    [Serializable]
    public struct LocalizedLine
    {
        [TextArea(2, 8)] public string English;
        [TextArea(2, 8)] public string Japanese;
        [TextArea(2, 8)] public string Persian;

        public LocalizedLine(string english, string japanese, string persian)
        {
            English = english;
            Japanese = japanese;
            Persian = persian;
        }

        /// <summary>
        /// The text for a language, falling back to English when a translation
        /// has not been written yet.
        /// </summary>
        /// <remarks>
        /// Falling back rather than showing a marker is deliberate here. A gap
        /// in the interface table is a bug and should be loud; a gap in a script
        /// is just a line nobody has translated yet, and a playthrough in a
        /// half-finished language should still be playable.
        /// </remarks>
        public string For(GameLanguage language)
        {
            switch (language)
            {
                case GameLanguage.Japanese:
                    return string.IsNullOrEmpty(Japanese) ? English : Japanese;

                case GameLanguage.Persian:
                    return string.IsNullOrEmpty(Persian) ? English : Persian;

                default:
                    return English;
            }
        }

        /// <summary>The text in the language the player has selected.</summary>
        public string Current()
        {
            return For(LocalizationService.Current);
        }

        /// <summary>True when no language has any text at all.</summary>
        public bool IsEmpty =>
            string.IsNullOrEmpty(English)
            && string.IsNullOrEmpty(Japanese)
            && string.IsNullOrEmpty(Persian);
    }

    /// <summary>One option in a choice.</summary>
    [Serializable]
    public struct ChoiceData
    {
        public LocalizedLine Text;

        [Tooltip("Blue counts as kind, green as controlling, white counts as neither.")]
        public ChoiceTone Tone;
    }

    /// <summary>One instruction in an act.</summary>
    [Serializable]
    public sealed class BeatData
    {
        [Tooltip("What this beat does. The fields below change to match.")]
        public StoryBeatKind Kind = StoryBeatKind.Line;

        [Tooltip("Who is speaking, entering or leaving.")]
        public Speaker Speaker = Speaker.Narrator;

        [Tooltip("The expression they take. 'Unchanged' leaves whatever is on screen.")]
        public Portrait Portrait = Portrait.Unchanged;

        [Tooltip("The line itself, in each language.")]
        public LocalizedLine Text;

        [Tooltip("Background asset name, from Assets/Images/Backgrounds.")]
        public string Background;

        [Tooltip("Place name shown briefly in the corner when the background changes.")]
        public LocalizedLine Caption;

        /// <summary>
        /// How long this background change takes, or negative for the usual
        /// crossfade.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Negative rather than zero, and that distinction is the whole reason
        /// the field exists: zero is a real answer here. Act six's closing
        /// montage cuts between two pictures on the frame, and a field that
        /// could not tell "cut instantly" apart from "nobody set this" would
        /// have every scene change in the game snapping.
        /// </para>
        /// <para>
        /// A short enough value also suppresses the chime and the dialogue
        /// box's polite fade-out, because those exist to make a transition feel
        /// like travel and a cut is not travel.
        /// </para>
        /// </remarks>
        [Tooltip("Seconds for this background change. Negative uses the normal crossfade.")]
        public float FadeSeconds = -1f;

        [Tooltip("Music track file name, from Assets/Audio/BackgroundMusics. Empty stops the music.")]
        public string MusicTrack;

        [Tooltip("Play a sound alongside this beat.")]
        public bool PlaySound;

        public SfxId Sound = SfxId.Petal;

        [Range(0f, 2f)] public float SoundVolume = 1f;

        [Tooltip("Seconds to hold, for a Beat of silence.")]
        public float Seconds = 1.5f;

        /// <summary>
        /// A recorded reading of this line, by file name.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Empty everywhere until act five, which the design document turns
        /// voiced. A name that has no file behind it plays nothing and warns
        /// once, exactly as a missing music track does, so an act can be written
        /// and played through today and the recordings dropped into
        /// <c>Assets/Audio/Voice</c> whenever they exist.
        /// </para>
        /// <para>
        /// One clip per line rather than per language. Three full recordings of
        /// a script is not a thing this project is going to have, and a Japanese
        /// reading under an English subtitle is the normal arrangement for the
        /// genre anyway.
        /// </para>
        /// </remarks>
        [Tooltip("Recorded line, from Assets/Audio/Voice. Empty means the typewriter voice is used.")]
        public string VoiceClip;

        /// <summary>
        /// The film a Video beat plays, by file name.
        /// </summary>
        /// <remarks>
        /// Looked for in the video library first and then in
        /// <c>StreamingAssets/Video</c>. A long credits sequence usually belongs
        /// in the second: it stays a file on disk that can be recut without
        /// reimporting the project.
        /// </remarks>
        [Tooltip("Film name, from Assets/Video or StreamingAssets/Video, without the extension.")]
        public string Film;

        /// <summary>
        /// Characters per second for this line, or zero for the usual speed.
        /// </summary>
        /// <remarks>
        /// The design document asks for the long speeches in the two spoken
        /// endings to pour out rather than being typed — fast and breathless, so
        /// they read as somebody who has stopped choosing their words. This is
        /// that, and it is per line because only some lines want it.
        /// </remarks>
        [Tooltip("Characters per second. 0 uses the normal reading speed; ~140 is a rush.")]
        public float TypeSpeed;

        /// <summary>
        /// How strongly a Grade beat veils the picture, and the colour a Stain
        /// beat throws across it.
        /// </summary>
        /// <remarks>
        /// One is the pastel haze the game has worn since act one; zero is the
        /// art with nothing in front of it. <see cref="Seconds"/> is how long it
        /// takes to get there.
        /// </remarks>
        [Range(0f, 1f)] public float Amount = 1f;

        [Tooltip("Colour thrown across the screen, for a Stain.")]
        public Color Tint = new Color(0.45f, 0.02f, 0.05f, 0.85f);

        [Tooltip("The options offered, for a Choice.")]
        public ChoiceData[] Choices = Array.Empty<ChoiceData>();

        [Tooltip("A short act played inline, for an Interlude.")]
        public ActAsset Interlude;

        [Tooltip("How likely an Interlude is to play. 1 always, 0.35 about a third of the time.")]
        [Range(0f, 1f)] public float Chance = 1f;

        /// <summary>
        /// Marks a line as one of the game's silences.
        /// </summary>
        /// <remarks>
        /// Set on the lines where Haru's leg stops him walking and where the
        /// machine room reaches Yua through a floor. A player who sits with one
        /// of those for <see cref="Core.GameConfig.PatienceSeconds"/> without
        /// clicking is counted as having listened, and the endings read that
        /// count. Nothing is shown either way — being told to wait is not
        /// waiting.
        /// </remarks>
        [Tooltip("Count this line towards the five minutes of patience the endings are built on.")]
        public bool MeasurePatience;

        /// <summary>
        /// Yua's psychological override, from act two onward.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When set, taking the blue option makes her face go flat for a moment
        /// and the scene carry on down the green path as though the button had
        /// never been offered. The act is linear either way — what changes is
        /// who the player believes is holding the controller.
        /// </para>
        /// <para>
        /// The press is still counted as kind. The endings are decided by what
        /// the player reached for, not by what she allowed, so a playthrough
        /// that presses blue and is refused every single time still reaches the
        /// secret ending.
        /// </para>
        /// </remarks>
        [Tooltip("Yua refuses the blue option here, and the story carries on the controlling way regardless.")]
        public bool YuaOverridesKindness;

        [Tooltip("What Yua says as she takes the blue option back. Left empty, she only pulls the face.")]
        public LocalizedLine OverrideLine;

        [Tooltip("A note to yourself. Never shown to the player.")]
        [TextArea(1, 3)] public string Note;
    }

    /// <summary>One act of the story.</summary>
    [CreateAssetMenu(
        fileName = "Act00",
        menuName = "The Frayed Red String/Act",
        order = 10)]
    public sealed class ActAsset : ScriptableObject
    {
        [Header("Identity")]
        [Tooltip("1-based act number. Save slots and the title card use it. Interludes leave it at 0.")]
        public int ActNumber;

        [Tooltip("The act's name, shown on the title card and on save slots.")]
        public LocalizedLine Title;

        [Tooltip("Music that starts when the act does. Empty leaves whatever is already playing.")]
        public string MusicTrack;

        [Tooltip("Show the title card when the act begins.")]
        public bool ShowTitleCard = true;

        [Header("Script")]
        public List<BeatData> Beats = new List<BeatData>();

        /// <summary>
        /// A fingerprint of the script as it was last written by a builder.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The one thing this project could not tell apart, and it cost a whole
        /// playthrough: an act whose script has been edited by hand in the
        /// Story Editor, and an act that is simply older than the code that
        /// generates it. Both look like "an act with beats in it", so the
        /// one-press setup asked about them and defaulted to leaving them alone
        /// — which quietly meant a game built from last week's script while its
        /// builder had been rewritten.
        /// </para>
        /// <para>
        /// Written by the builder alongside the beats. Compare it against a
        /// fingerprint of the beats actually on disk and the answer is exact: if
        /// they match, nobody has touched this since it was generated and it is
        /// safe to regenerate; if they differ, somebody has, and it is not.
        /// </para>
        /// <para>
        /// Empty on an act that no builder has ever written — act one, and
        /// anything written from scratch in the Story Editor. Those are always
        /// treated as hand-written.
        /// </para>
        /// </remarks>
        [HideInInspector] public string GeneratedSignature;

        /// <summary>Number of beats in the act.</summary>
        public int Count => Beats != null ? Beats.Count : 0;

        /// <summary>The beat at an index, or <c>null</c> when out of range.</summary>
        public BeatData At(int index)
        {
            return Beats != null && index >= 0 && index < Beats.Count ? Beats[index] : null;
        }

        /// <summary>
        /// How many beats the player actually stops at.
        /// </summary>
        /// <remarks>
        /// Shown in the editor as the act's real length. The raw beat count is a
        /// poor measure of how long an act takes to read, because half of a
        /// well-written one is stage direction that never waits for input.
        /// </remarks>
        public int SpokenCount()
        {
            if (Beats == null)
            {
                return 0;
            }

            int spoken = 0;
            for (int i = 0; i < Beats.Count; i++)
            {
                if (Beats[i] != null && Beats[i].Kind == StoryBeatKind.Line)
                {
                    spoken++;
                }
            }

            return spoken;
        }
    }
}
