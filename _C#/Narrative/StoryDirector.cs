// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryDirector.cs
// -----------------------------------------------------------------------------

using System;
using System.Collections;
using TheFrayedRedString.Audio;
using TheFrayedRedString.Core;
using TheFrayedRedString.InputHandling;
using TheFrayedRedString.Presentation;
using TheFrayedRedString.UI;
using UnityEngine;

namespace TheFrayedRedString.Narrative
{
    /// <summary>
    /// Plays an <see cref="ActAsset"/>: reads each beat, tells the stage and the
    /// dialogue box what to do, and waits for the player where it should.
    /// </summary>
    /// <remarks>
    /// Written as a coroutine rather than as a state machine. Almost every beat
    /// is "start something, wait for it to finish, carry on", and a coroutine
    /// says that in one line where a state machine needs an enum member, a
    /// branch and a field to remember what it was in the middle of. The cost is
    /// that the sequence cannot be rewound — which is fine, because a visual
    /// novel only ever moves forward.
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class StoryDirector : MonoBehaviour
    {
        private VisualNovelStage _stage;
        private DialogueBoxView _dialogue;
        private StoryOverlayView _overlay;
        private ChoicePanelView _choices;
        private StoryFrameView _frame;

        private ActAsset _act;
        private Coroutine _playback;

        private bool _advanceQueued;
        private int _pendingChoice = -1;

        /// <summary>Raised when the act reaches its end.</summary>
        public event Action Finished;

        /// <summary>
        /// Suspends the story. Set while the pause menu is up.
        /// </summary>
        /// <remarks>
        /// A flag rather than <see cref="Time.timeScale"/>. Everything in this
        /// game already runs on unscaled time so that menus keep breathing, which
        /// means zeroing the time scale would pause nothing at all here.
        /// </remarks>
        public bool IsPaused { get; set; }

        /// <summary>The beat currently being played, for saving.</summary>
        public int CurrentBeat { get; private set; }

        /// <summary>True while an act is running.</summary>
        public bool IsPlaying => _playback != null;

        /// <summary>Wires the director to the views it drives.</summary>
        public void Initialize(
            VisualNovelStage stage,
            DialogueBoxView dialogue,
            StoryOverlayView overlay,
            ChoicePanelView choices,
            StoryFrameView frame)
        {
            _stage = stage;
            _dialogue = dialogue;
            _overlay = overlay;
            _choices = choices;
            _frame = frame;
        }

        /// <summary>
        /// Starts an act, optionally partway through.
        /// </summary>
        /// <param name="act">The act to play.</param>
        /// <param name="fromBeat">
        /// Beat to resume at. Everything before it is applied silently first, so
        /// a save taken in the café does not resume against an empty stage.
        /// </param>
        public void Play(ActAsset act, int fromBeat = 0)
        {
            Stop();

            _act = act;

            if (_act == null || _act.Count == 0)
            {
                Debug.LogWarning(
                    "[Story] This scene has no act to play. Open " +
                    "The Frayed Red String > Story Editor and make sure an act exists with the right number.");

                Finished?.Invoke();
                return;
            }

            _playback = StartCoroutine(PlayRoutine(Mathf.Clamp(fromBeat, 0, _act.Count - 1)));
        }

        /// <summary>Halts playback where it is.</summary>
        public void Stop()
        {
            if (_playback != null)
            {
                StopCoroutine(_playback);
                _playback = null;
            }
        }

        /// <summary>Reports a click that landed on the story rather than on a control.</summary>
        public void NotifyAdvanceClicked()
        {
            if (!IsPaused)
            {
                _advanceQueued = true;
            }
        }

        /// <summary>Re-renders the current line after a language change.</summary>
        public void RefreshLanguage()
        {
            BeatData beat = _act != null ? _act.At(CurrentBeat) : null;

            if (beat != null && beat.Kind == StoryBeatKind.Line)
            {
                _dialogue.ShowLineInstantly(beat.Speaker, beat.Text.Current());
            }
        }

        private void Update()
        {
            if (IsPaused)
            {
                return;
            }

            if (InputService.AdvancePressedThisFrame)
            {
                _advanceQueued = true;
            }

            if (_dialogue?.Typewriter != null)
            {
                _dialogue.Typewriter.SpeedMultiplier =
                    InputService.FastForwardHeld ? GameConfig.TypeSpeedFastMultiplier : 1f;
            }
        }

        // ---------------------------------------------------------------------
        //  Playback
        // ---------------------------------------------------------------------

        private IEnumerator PlayRoutine(int fromBeat)
        {
            if (fromBeat > 0)
            {
                PrimeStateUpTo(fromBeat);
            }
            else if (!string.IsNullOrEmpty(_act.MusicTrack))
            {
                MusicService.PlayLoop(_act.MusicTrack);
            }

            for (int i = fromBeat; i < _act.Count; i++)
            {
                CurrentBeat = i;
                StorySession.LineIndex = i;

                BeatData beat = _act.At(i);
                if (beat == null)
                {
                    continue;
                }

                if (beat.Kind == StoryBeatKind.End)
                {
                    break;
                }

                yield return PlayBeat(beat);
            }

            _playback = null;
            Finished?.Invoke();
        }

        private IEnumerator PlayBeat(BeatData beat)
        {
            // A sound attached to any beat fires as that beat begins, so a line
            // and its cue land together instead of a frame apart.
            if (beat.PlaySound)
            {
                AudioService.Play(beat.Sound, beat.SoundVolume);
            }

            switch (beat.Kind)
            {
                case StoryBeatKind.Line:
                    yield return PlayLine(beat);
                    break;

                case StoryBeatKind.Background:
                    yield return PlayBackground(beat);
                    break;

                case StoryBeatKind.Enter:
                    _stage.ShowCharacter(beat.Speaker, beat.Portrait, GameConfig.CharacterFadeDuration);
                    break;

                case StoryBeatKind.Exit:
                    _stage.HideCharacter(beat.Speaker, GameConfig.CharacterFadeDuration);
                    break;

                case StoryBeatKind.ClearStage:
                    _stage.HideAll(GameConfig.CharacterFadeDuration);
                    break;

                case StoryBeatKind.Caption:
                    _overlay.ShowCaption(beat.Caption.Current(), GameConfig.CaptionHoldDuration);
                    break;

                case StoryBeatKind.TitleCard:
                    _dialogue.Hide(GameConfig.DialogueFadeDuration);
                    yield return _overlay.PlayTitleCard(
                        Mathf.Max(1, _act.ActNumber),
                        _act.Title.Current(),
                        GameConfig.TitleCardHoldDuration);
                    break;

                case StoryBeatKind.Sound:
                    // Already played above; the beat exists so a cue can be
                    // placed on its own rather than hung off a line.
                    break;

                case StoryBeatKind.Music:
                    if (string.IsNullOrEmpty(beat.MusicTrack))
                    {
                        MusicService.Stop();
                    }
                    else
                    {
                        MusicService.PlayLoop(beat.MusicTrack);
                    }

                    break;

                case StoryBeatKind.Beat:
                    yield return HoldOnPicture(beat.Seconds);
                    break;

                case StoryBeatKind.Choice:
                    yield return PlayChoice(beat);
                    break;

                case StoryBeatKind.Interlude:
                    yield return PlayInterlude(beat);
                    break;

                case StoryBeatKind.OpenFrame:
                    _frame?.Open(beat.Seconds > 0f ? beat.Seconds : GameConfig.FrameOpenDuration);
                    yield return new WaitForSecondsRealtime(
                        beat.Seconds > 0f ? beat.Seconds : GameConfig.FrameOpenDuration);
                    break;

                case StoryBeatKind.CloseFrame:
                    _frame?.Close(beat.Seconds > 0f ? beat.Seconds : GameConfig.FrameOpenDuration);
                    break;
            }
        }

        private IEnumerator PlayLine(BeatData beat)
        {
            if (beat.Portrait != Portrait.Unchanged && beat.Speaker != Speaker.Narrator)
            {
                _stage.ShowCharacter(beat.Speaker, beat.Portrait, GameConfig.CharacterFadeDuration);
            }

            // Narration dims everybody: nobody on stage is the one talking, and
            // leaving the last speaker lit through a paragraph of description
            // makes it read as still theirs.
            _stage.SetFocus(beat.Speaker, GameConfig.CharacterFocusDuration);

            _dialogue.Show(GameConfig.DialogueFadeDuration);
            _dialogue.ShowLine(beat.Speaker, beat.Text.Current());

            yield return WaitForLine();
        }

        /// <summary>
        /// Waits out one line: the first press finishes the typing, the second
        /// moves on.
        /// </summary>
        /// <remarks>
        /// The queue is cleared before waiting rather than after. A press that
        /// arrived during a transition belongs to the transition, and letting it
        /// carry over would silently eat the first line of every new scene.
        /// </remarks>
        private IEnumerator WaitForLine()
        {
            _advanceQueued = false;

            while (_dialogue.IsTyping)
            {
                if (!IsPaused && (_advanceQueued || InputService.FastForwardHeld))
                {
                    _advanceQueued = false;
                    _dialogue.CompleteLine();
                    break;
                }

                yield return null;
            }

            while (true)
            {
                if (!IsPaused && (_advanceQueued || InputService.FastForwardHeld))
                {
                    _advanceQueued = false;
                    yield break;
                }

                yield return null;
            }
        }

        private IEnumerator PlayBackground(BeatData beat)
        {
            bool changing = !string.Equals(_stage.CurrentBackground, beat.Background, StringComparison.Ordinal);

            if (changing)
            {
                _dialogue.Hide(GameConfig.DialogueFadeDuration);

                if (!beat.PlaySound)
                {
                    AudioService.Play(SfxId.WishStar, 0.8f);
                }

                yield return new WaitForSecondsRealtime(GameConfig.DialogueFadeDuration * 0.6f);
            }

            bool done = false;
            _stage.ShowBackground(beat.Background, GameConfig.BackgroundFadeDuration, () => done = true);

            while (!done)
            {
                yield return null;
            }

            if (!beat.Caption.IsEmpty)
            {
                _overlay.ShowCaption(beat.Caption.Current(), GameConfig.CaptionHoldDuration);
            }
        }

        /// <summary>Holds on the picture with the box out of the way.</summary>
        private IEnumerator HoldOnPicture(float seconds)
        {
            _dialogue.Hide(GameConfig.DialogueFadeDuration);

            yield return new WaitForSecondsRealtime(Mathf.Max(0f, seconds));
        }

        private IEnumerator PlayChoice(BeatData beat)
        {
            if (beat.Choices == null || beat.Choices.Length == 0)
            {
                yield break;
            }

            _dialogue.Hide(GameConfig.DialogueFadeDuration);

            _pendingChoice = -1;
            _choices.Present(beat.Choices, index => _pendingChoice = index);

            while (_pendingChoice < 0)
            {
                yield return null;
            }

            int chosen = Mathf.Clamp(_pendingChoice, 0, beat.Choices.Length - 1);
            _pendingChoice = -1;

            ChoiceData choice = beat.Choices[chosen];

            AudioService.Play(choice.Tone == ChoiceTone.Cruel ? SfxId.ChoiceCruel : SfxId.ChoiceKind);
            StorySession.RecordChoice(choice.Tone);

            // Swallow the click that picked the option, so it does not also
            // advance the line that follows.
            _advanceQueued = false;
        }

        private IEnumerator PlayInterlude(BeatData beat)
        {
            ActAsset interlude = beat.Interlude;

            if (interlude == null || interlude.Count == 0)
            {
                yield break;
            }

            if (beat.Chance < 1f && UnityEngine.Random.value > beat.Chance)
            {
                yield break;
            }

            for (int i = 0; i < interlude.Count; i++)
            {
                BeatData inner = interlude.At(i);

                if (inner == null || inner.Kind == StoryBeatKind.End)
                {
                    continue;
                }

                yield return PlayBeat(inner);
            }
        }

        /// <summary>
        /// Replays the beats before a resume point for their side effects only.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Loading a save is loading a moment, not just a line number. Jumping
        /// straight to beat ninety would put the player in the café with a blank
        /// screen and nobody on it, because the background and the two
        /// characters were set by beats they never reached.
        /// </para>
        /// <para>
        /// So the stage instructions are re-run at zero duration and the lines
        /// are skipped. Interludes are skipped too: they are random by
        /// definition, and re-rolling them here could put the player in a scene
        /// their save was never taken in.
        /// </para>
        /// </remarks>
        private void PrimeStateUpTo(int index)
        {
            string background = null;
            string music = _act.MusicTrack;
            float frameOpenness = -1f;

            for (int i = 0; i < index && i < _act.Count; i++)
            {
                BeatData beat = _act.At(i);
                if (beat == null)
                {
                    continue;
                }

                switch (beat.Kind)
                {
                    case StoryBeatKind.Background:
                        background = beat.Background;
                        break;

                    case StoryBeatKind.Music:
                        music = beat.MusicTrack;
                        break;

                    case StoryBeatKind.Enter:
                        _stage.ShowCharacter(beat.Speaker, beat.Portrait, 0f);
                        break;

                    case StoryBeatKind.Exit:
                        _stage.HideCharacter(beat.Speaker, 0f);
                        break;

                    case StoryBeatKind.ClearStage:
                        _stage.HideAll(0f);
                        break;

                    case StoryBeatKind.OpenFrame:
                        frameOpenness = 1f;
                        break;

                    case StoryBeatKind.CloseFrame:
                        frameOpenness = 0f;
                        break;
                }
            }

            if (!string.IsNullOrEmpty(background))
            {
                _stage.ShowBackground(background, 0f);
            }

            if (!string.IsNullOrEmpty(music))
            {
                MusicService.PlayLoop(music);
            }

            if (frameOpenness >= 0f && _frame != null)
            {
                _frame.SetOpenness(frameOpenness);
            }
        }
    }
}
