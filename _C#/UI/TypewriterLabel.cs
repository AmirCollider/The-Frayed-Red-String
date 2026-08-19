// -----------------------------------------------------------------------------
//  The Frayed Red String
//  TypewriterLabel.cs
// -----------------------------------------------------------------------------

using System;
using TheFrayedRedString.Audio;
using TheFrayedRedString.Core;
using TheFrayedRedString.Narrative;
using TheFrayedRedString.Presentation;
using TMPro;
using UnityDirectTMP;
using UnityEngine;

namespace TheFrayedRedString.UI
{
    /// <summary>
    /// Reveals a line of dialogue a character at a time, in the speaker's own
    /// voice.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The sound is the point of this component, not the animation. A
    /// typewriter that plays one identical blip per character is the single
    /// most machine-like noise a visual novel can make, and this game spends its
    /// first act pretending to be something warm. So each speaker gets a grain
    /// of their own — Yua high and glassy, Haru low and wooden — and the grain
    /// is pitched along a pentatonic line that advances with the text. What the
    /// player hears over a sentence is a short melody in a consistent voice,
    /// which reads as somebody talking rather than as a printer running.
    /// </para>
    /// <para>
    /// Punctuation is given real time. A comma costs a beat and a full stop
    /// costs three, so the rhythm has phrasing in it.
    /// </para>
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class TypewriterLabel : MonoBehaviour
    {
        /// <summary>
        /// Pitch contours, in semitones, walked one step per sounded grain.
        /// </summary>
        /// <remarks>
        /// All three stay inside a major pentatonic, so a line can never produce
        /// an interval that clashes with the music-box palette the rest of the
        /// game is tuned to. The contours differ in shape rather than in scale:
        /// Yua's climbs and falls back, Haru's rocks around its root, and the
        /// narrator's barely moves.
        /// </remarks>
        private static readonly int[] YuaContour = { 0, 2, 4, 7, 9, 7, 4, 2 };
        private static readonly int[] HaruContour = { 0, 2, 4, 7, 4, 2 };
        private static readonly int[] NarratorContour = { 0, 0, 2, 0, -3, 0 };

        [Header("Pacing")]
        [Tooltip("Characters revealed per second at normal speed.")]
        [SerializeField] private float _charactersPerSecond = GameConfig.TypeSpeedNormal;

        [Tooltip("Extra pause after a comma, in character-times.")]
        [SerializeField] private float _commaPause = 2.5f;

        [Tooltip("Extra pause after a sentence ends, in character-times.")]
        [SerializeField] private float _sentencePause = 5f;

        [Header("Voice")]
        [Tooltip("One grain is played every this many revealed characters.")]
        [SerializeField, Range(1, 6)] private int _charactersPerGrain = 2;

        [SerializeField, Range(0f, 1f)] private float _voiceVolume = 0.5f;

        private TMP_Text _label;
        private Speaker _speaker;
        private string _logical = string.Empty;

        private bool _growText;
        private int _revealed;
        private int _total;
        private float _characterBudget;
        private float _pauseCharacters;
        private int _grainCountdown;
        private int _contourStep;
        private bool _typing;

        /// <summary>Raised when the line finishes revealing, by any route.</summary>
        public event Action Completed;

        /// <summary>True while characters are still appearing.</summary>
        public bool IsTyping => _typing;


        /// <summary>Binds the component to the label it drives.</summary>
        public void Initialize(TMP_Text label)
        {
            _label = label;

            if (_label != null)
            {
                _label.richText = true;
            }
        }

        /// <summary>Starts revealing a line.</summary>
        /// <param name="logical">Text in reading order, already localised.</param>
        /// <param name="speaker">Whose voice the grains use.</param>
        public void Play(string logical, Speaker speaker)
        {
            if (_label == null)
            {
                return;
            }

            _logical = logical ?? string.Empty;
            _speaker = speaker;
            _contourStep = 0;
            _grainCountdown = 0;
            _characterBudget = 0f;
            _pauseCharacters = 0f;
            _revealed = 0;

            ChooseStrategy();

            if (_total <= 0)
            {
                _typing = false;
                ApplyReveal();
                Completed?.Invoke();
                return;
            }

            _typing = true;
            ApplyReveal();
        }

        /// <summary>Reveals the rest of the line at once.</summary>
        public void CompleteNow()
        {
            if (!_typing)
            {
                return;
            }

            _revealed = _total;
            _typing = false;

            ApplyReveal();
            Completed?.Invoke();
        }

        /// <summary>
        /// Replaces the line and shows all of it at once, with no typing and no
        /// voice.
        /// </summary>
        /// <remarks>
        /// Used when the player changes language mid-line. Retyping would be
        /// wrong twice over: they asked to read the sentence in another
        /// language, not to watch it appear again, and the grains would sound a
        /// second time for a line that has already been spoken.
        /// </remarks>
        public void ShowInstantly(string logical, Speaker speaker)
        {
            if (_label == null)
            {
                return;
            }

            _logical = logical ?? string.Empty;
            _speaker = speaker;

            ChooseStrategy();

            _revealed = _total;
            _typing = false;
            ApplyReveal();
        }

        /// <summary>Empties the label.</summary>
        public void Clear()
        {
            _typing = false;
            _logical = string.Empty;
            _total = 0;
            _revealed = 0;

            if (_label != null)
            {
                _label.text = string.Empty;
                _label.maxVisibleCharacters = int.MaxValue;
            }
        }

        private void Update()
        {
            if (!_typing || _label == null)
            {
                return;
            }

            // The story's clock, not Unity's. While the pause menu is up this
            // hands back zero and the line simply stops mid-word, which is what
            // a paused game is supposed to do.
            float rate = Mathf.Max(1f, _charactersPerSecond);
            _characterBudget += StoryClock.DeltaTime * rate;

            // Punctuation debt is paid in the same currency as characters, so a
            // pause automatically scales with the reveal speed.
            if (_pauseCharacters > 0f)
            {
                float paid = Mathf.Min(_pauseCharacters, _characterBudget);
                _pauseCharacters -= paid;
                _characterBudget -= paid;
            }

            bool changed = false;

            while (_characterBudget >= 1f && _revealed < _total && _pauseCharacters <= 0f)
            {
                _characterBudget -= 1f;

                char revealed = _logical[_revealed];
                _revealed++;
                changed = true;

                SoundFor(revealed);
                _pauseCharacters = PauseAfter(revealed);
            }

            if (changed)
            {
                ApplyReveal();
            }

            if (_revealed >= _total)
            {
                _typing = false;
                Completed?.Invoke();
            }
        }

        // ---------------------------------------------------------------------
        //  Reveal
        // ---------------------------------------------------------------------

        /// <summary>
        /// Decides how this particular line will be revealed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The reveal always counts in the text as it was written, never in the
        /// text as it is drawn. Those are the same thing in English and
        /// Japanese and opposite in Persian: DirectFont hands TextMeshPro a
        /// reordered string, so "show the first twenty characters" would show
        /// the last twenty words of a Persian line.
        /// </para>
        /// <para>
        /// Where the two orders agree, <c>maxVisibleCharacters</c> is used —
        /// the whole line is laid out once and the rest is simply not drawn, so
        /// nothing reflows as it fills in. Where they do not, the label is
        /// given a growing prefix instead and DirectFont shapes and places each
        /// one correctly. The cost is that a part-typed word at the end of a
        /// line can hop down to the next one as it grows, which is the ordinary
        /// behaviour of every visual novel that types text at all.
        /// </para>
        /// <para>
        /// Splitting the difference with a transparent rich-text tag was the
        /// obvious third option and does not work: the package documents that a
        /// tag in the middle of a right-to-left sentence cuts it into two runs
        /// laid out left-to-right against each other, so the line would jump
        /// about while typing.
        /// </para>
        /// </remarks>
        private void ChooseStrategy()
        {
            _total = _logical.Length;
            _growText = DirectJoining.NeedsShaping(_logical);

            // The whole line goes in first, even on the path that reveals it a
            // character at a time, because the package picks a font by looking
            // at the text — and it has to be looking at the finished line, not
            // at the first letter of it.
            _label.text = _logical;

            // Once per line. The line may be the first one in a language this
            // label has not drawn yet, and the font for it is otherwise built a
            // frame late — which shows up as a line of unjoined Persian, or of
            // boxes, that never corrects itself. Per character this would be
            // ruinous; per line it is free.
            StoryText.ApplyWeight(_label);
            StoryFonts.Refresh(_label);

            if (_growText)
            {
                _label.maxVisibleCharacters = int.MaxValue;
                return;
            }

            _label.maxVisibleCharacters = 0;
        }

        private void ApplyReveal()
        {
            if (_label == null)
            {
                return;
            }

            if (_growText)
            {
                _label.text = _revealed >= _total ? _logical : _logical.Substring(0, _revealed);
                return;
            }

            _label.maxVisibleCharacters = _revealed;
        }

        // ---------------------------------------------------------------------
        //  Voice
        // ---------------------------------------------------------------------

        private void SoundFor(char character)
        {
            // Silence on whitespace and punctuation. Sounding them makes the
            // rhythm even, and an even rhythm is the machine sound this whole
            // component exists to avoid.
            if (char.IsWhiteSpace(character) || char.IsPunctuation(character))
            {
                return;
            }

            if (--_grainCountdown > 0)
            {
                return;
            }

            _grainCountdown = Mathf.Max(1, _charactersPerGrain);

            int[] contour = ContourFor(_speaker);
            int semitone = contour[_contourStep % contour.Length];
            _contourStep++;

            AudioService.Play(VoiceFor(_speaker), _voiceVolume, Mathf.Pow(2f, semitone / 12f));
        }

        private static SfxId VoiceFor(Speaker speaker)
        {
            switch (speaker)
            {
                case Speaker.Yua: return SfxId.TypeYua;
                case Speaker.Haru: return SfxId.TypeHaru;
                default: return SfxId.TypeNarrator;
            }
        }

        private static int[] ContourFor(Speaker speaker)
        {
            switch (speaker)
            {
                case Speaker.Yua: return YuaContour;
                case Speaker.Haru: return HaruContour;
                default: return NarratorContour;
            }
        }

        /// <summary>
        /// How long to wait after a character, in character-times.
        /// </summary>
        /// <remarks>
        /// Japanese and Persian punctuation is listed alongside the Latin marks
        /// rather than relying on <see cref="char.IsPunctuation(char)"/>, which
        /// cannot tell a full stop from a comma and would give both the same
        /// beat.
        /// </remarks>
        private float PauseAfter(char character)
        {
            switch (character)
            {
                case '.':
                case '!':
                case '?':
                case '…':   // ellipsis
                case '。':   // 。
                case '！':   // ！
                case '？':   // ？
                case '؟':   // ؟
                    return _sentencePause;

                case ',':
                case ';':
                case ':':
                case '、':   // 、
                case '，':   // ，
                case '؛':   // ؛
                case '،':   // ،
                case '—':   // em dash
                    return _commaPause;

                default:
                    return 0f;
            }
        }
    }
}
