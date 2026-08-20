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
        private StoryGrade _grade;
        private VideoScreenView _film;

        private ActAsset _act;
        private Coroutine _playback;

        private bool _advanceQueued;
        private int _pendingChoice = -1;

        /// <summary>
        /// The line on screen right now, so a language change can redraw it.
        /// </summary>
        /// <remarks>
        /// Held here rather than read back off the current beat, because not
        /// every line in the game belongs to a Line beat: Yua's refusal in act
        /// two is spoken from inside a Choice. Remembering what was actually
        /// said is the only version of this that keeps working as the story
        /// grows more ways to say something.
        /// </remarks>
        private Speaker _spokenSpeaker;

        private LocalizedLine _spokenLine;

        /// <summary>The recording behind the line on screen, if any.</summary>
        private string _spokenVoiceClip;

        private bool _lineOnScreen;

        /// <summary>
        /// True while the act is playing itself and the player cannot advance
        /// it.
        /// </summary>
        /// <remarks>
        /// Held by the director rather than by the act, so it resets with every
        /// scene: an act that ends inside a cinema sequence cannot leave the
        /// next one un-clickable.
        /// </remarks>
        private bool _cinema;

        /// <summary>
        /// The ending the player has just chosen, waiting to replace the rest of
        /// the act.
        /// </summary>
        private ActAsset _pendingEnding;

        /// <summary>
        /// How deep the story is inside interludes right now.
        /// </summary>
        /// <remarks>
        /// An interlude is an act, and an act can contain an Interlude beat, so
        /// two of them can name each other. The Check tab catches an act that
        /// names itself; it cannot catch a ring of three, and a ring of three is
        /// not a wrong picture — it is a coroutine that never returns and a game
        /// that stops responding with nothing in the console.
        /// </remarks>
        private int _interludeDepth;

        /// <summary>How many interludes deep the story may go before it stops.</summary>
        private const int MaxInterludeDepth = 4;

        /// <summary>Raised when the act reaches its end.</summary>
        public event Action Finished;

        /// <summary>
        /// Suspends the story. Set while the pause menu is up.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A flag rather than <see cref="Time.timeScale"/>. Everything in this
        /// game already runs on unscaled time so that menus keep breathing, which
        /// means zeroing the time scale would pause nothing at all here.
        /// </para>
        /// <para>
        /// The flag on its own was not enough either, and that is what made the
        /// pause menu a menu you could read a scene through: it only stopped the
        /// director from accepting a click. The typing carried on, every hold
        /// counted down, and the story walked from beat to beat behind the veil.
        /// Setting it now stops <see cref="StoryClock"/> as well, which is the
        /// clock all of that is actually running on.
        /// </para>
        /// </remarks>
        public bool IsPaused
        {
            get => StoryClock.IsPaused;
            set => StoryClock.IsPaused = value;
        }

        /// <summary>The beat currently being played, for saving.</summary>
        public int CurrentBeat { get; private set; }

        /// <summary>True while an act is running.</summary>
        public bool IsPlaying => _playback != null;

        /// <summary>
        /// True once the story has finished for good rather than merely reaching
        /// the end of an act.
        /// </summary>
        /// <remarks>
        /// Set by an EndGame beat, which act seven and both endings finish with.
        /// The scene reads it to decide whether to walk on to the next act or to
        /// wipe the saves and go back to the title, which the design document
        /// requires: a replay starts at the very beginning.
        /// </remarks>
        public bool GameEnded { get; private set; }

        /// <summary>Wires the director to the views it drives.</summary>
        public void Initialize(
            VisualNovelStage stage,
            DialogueBoxView dialogue,
            StoryOverlayView overlay,
            ChoicePanelView choices,
            StoryFrameView frame,
            StoryGrade grade = null,
            VideoScreenView film = null)
        {
            _stage = stage;
            _dialogue = dialogue;
            _overlay = overlay;
            _choices = choices;
            _frame = frame;
            _grade = grade;
            _film = film;
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

            // Nothing said by the act that just ended is on screen any more, and
            // a language change during the new act's opening fade must not redraw
            // the last line of the old one.
            _lineOnScreen = false;

            if (_act == null || _act.Count == 0)
            {
                Debug.LogWarning(
                    "[Story] This scene has no act to play. Open " +
                    "The Frayed Red String > Story Editor and make sure an act exists with the right number.");

                Finished?.Invoke();
                return;
            }

            _interludeDepth = 0;
            _cinema = false;
            _pendingEnding = null;
            GameEnded = false;
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
            if (!IsPaused && !_cinema)
            {
                _advanceQueued = true;
            }
        }

        /// <summary>Re-renders the current line after a language change.</summary>
        public void RefreshLanguage()
        {
            if (_lineOnScreen)
            {
                _dialogue.ShowLineInstantly(_spokenSpeaker, _spokenLine.Current());
            }
        }

        private void Update()
        {
            if (IsPaused)
            {
                return;
            }

            if (_cinema)
            {
                // Nothing to queue. Escape still reaches the pause menu, which
                // is handled by the scene rather than here.
                return;
            }

            if (InputService.AdvancePressedThisFrame)
            {
                _advanceQueued = true;
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
            else
            {
                if (!string.IsNullOrEmpty(_act.MusicTrack))
                {
                    MusicService.PlayLoop(_act.MusicTrack);
                }

                // The act's own "show the title card" tick was being read by
                // nobody: the card only ever appeared where a TitleCard beat had
                // been written by hand, so an act that asked for one and had no
                // such beat simply started without it.
                //
                // Only when the act has no card of its own, and only from the
                // top — resuming a save is walking back into a scene already in
                // progress, and being re-introduced to it would be strange.
                if (_act.ShowTitleCard && !HasTitleCardBeat())
                {
                    yield return PlayTitleCard();
                }
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

                if (_pendingEnding != null)
                {
                    break;
                }
            }

            if (_pendingEnding != null)
            {
                yield return PlayEnding();
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
                    _overlay.ShowCaption(beat.Caption, GameConfig.CaptionHoldDuration);
                    break;

                case StoryBeatKind.TitleCard:
                    yield return PlayTitleCard();
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
                    yield return StoryClock.Wait(
                        beat.Seconds > 0f ? beat.Seconds : GameConfig.FrameOpenDuration);
                    break;

                case StoryBeatKind.CloseFrame:
                    _frame?.Close(beat.Seconds > 0f ? beat.Seconds : GameConfig.FrameOpenDuration);
                    break;

                case StoryBeatKind.PullDownDialogue:
                    yield return PullDownDialogue(beat);
                    break;

                case StoryBeatKind.Grade:
                    _grade?.FadeTo(
                        beat.Amount,
                        beat.Seconds > 0f ? beat.Seconds : GameConfig.GradeChangeDuration);

                    yield return StoryClock.Wait(
                        beat.Seconds > 0f ? beat.Seconds : GameConfig.GradeChangeDuration);

                    break;

                case StoryBeatKind.Stain:
                    // Zero seconds by default, because the one thing this exists
                    // for happens between two frames.
                    _grade?.Stain(beat.Tint, Mathf.Max(0f, beat.Seconds));

                    if (beat.Seconds > 0f)
                    {
                        yield return StoryClock.Wait(beat.Seconds);
                    }

                    break;

                case StoryBeatKind.CutMusic:
                    MusicService.CutOff();
                    break;

                case StoryBeatKind.EnterCinema:
                    _cinema = true;
                    _dialogue.SetPromptSuppressed(true);

                    // A press that arrived on the way in belongs to the scene
                    // before this one and must not skip the first line of the
                    // film.
                    _advanceQueued = false;
                    break;

                case StoryBeatKind.ExitCinema:
                    _cinema = false;
                    _dialogue.SetPromptSuppressed(false);
                    _advanceQueued = false;
                    break;

                case StoryBeatKind.Video:
                    yield return PlayFilm(beat);
                    break;

                case StoryBeatKind.EndGame:
                    GameEnded = true;
                    break;
            }
        }

        /// <summary>
        /// Abandons the rest of the act and plays the ending the player earned.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The endings do not live at the end of anything. The design document
        /// is explicit that a player can reach one from act three, or four, or
        /// wherever they happened to be sitting when the five minutes ran out —
        /// so an ending is an act with no number that simply replaces whatever
        /// was going to happen next.
        /// </para>
        /// <para>
        /// The stage is cleared first. Whatever scene the offer arrived in is
        /// not the scene the ending opens in, and an ending inheriting two
        /// characters from a corridor it has nothing to do with is the one way
        /// this could look like a bug rather than a decision.
        /// </para>
        /// </remarks>
        private IEnumerator PlayEnding()
        {
            ActAsset ending = _pendingEnding;
            _pendingEnding = null;

            HideDialogue(GameConfig.DialogueFadeDuration);
            _stage.HideAll(GameConfig.CharacterFadeDuration);

            yield return StoryClock.Wait(GameConfig.DialogueFadeDuration);

            for (int i = 0; i < ending.Count; i++)
            {
                BeatData beat = ending.At(i);

                if (beat == null || beat.Kind == StoryBeatKind.End)
                {
                    continue;
                }

                yield return PlayBeat(beat);
            }

            // Belt and braces. Both endings finish with an EndGame beat, and an
            // ending that somebody forgot to end must still not drop the player
            // back into act four.
            GameEnded = true;
        }

        /// <summary>
        /// Offers the ending the player has just earned.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Two options, and the second one is doing nothing. A single button
        /// with no way past it would trap a player who waited five minutes by
        /// accident — and, worse, would announce that something had been earned.
        /// The refusal has to be on screen for the offer to be a choice.
        /// </para>
        /// <para>
        /// Offered once per line. Whichever way it goes, this silence is spent.
        /// </para>
        /// </remarks>
        private IEnumerator OfferEnding()
        {
            ActAsset ending = Endings.ForCurrentRun();

            if (ending == null)
            {
                yield break;
            }

            ChoiceData[] offer = Endings.Offer();

            HideDialogue(GameConfig.DialogueFadeDuration);

            _pendingChoice = -1;
            _choices.Present(offer, index => _pendingChoice = index);

            while (_pendingChoice < 0)
            {
                yield return null;
            }

            bool taken = _pendingChoice == 0;
            _pendingChoice = -1;
            _advanceQueued = false;

            if (taken)
            {
                _pendingEnding = ending;
            }
        }

        /// <summary>
        /// Plays a film and waits for it, or carries straight on when there is
        /// none.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Everything the game normally draws is taken off first. A dialogue box
        /// left fading out underneath a full-screen credits sequence is the sort
        /// of thing that only shows up in a recording.
        /// </para>
        /// <para>
        /// Skippable outside cinema mode and not inside it, which is the same
        /// rule the rest of the game follows: the player's ability to move the
        /// story on belongs to the mode, not to the beat.
        /// </para>
        /// </remarks>
        private IEnumerator PlayFilm(BeatData beat)
        {
            HideDialogue(GameConfig.DialogueFadeDuration);
            _stage.HideAll(GameConfig.CharacterFadeDuration);

            yield return StoryClock.Wait(GameConfig.DialogueFadeDuration);

            if (_film == null)
            {
                yield break;
            }

            bool done = false;

            _film.Play(beat.Film, () => done = true);

            _advanceQueued = false;

            while (!done)
            {
                if (!_cinema && !IsPaused && _advanceQueued)
                {
                    _advanceQueued = false;
                    _film.Stop();
                    break;
                }

                yield return null;
            }
        }

        /// <summary>
        /// Haru pulls the dialogue box off the screen, and the story waits for
        /// him to finish doing it.
        /// </summary>
        /// <remarks>
        /// Waited on rather than started and left behind. Everything else in
        /// this act is going to keep happening on top of it otherwise, and the
        /// gesture is one of about four things in the whole game that the player
        /// is meant to stop and watch.
        /// </remarks>
        private IEnumerator PullDownDialogue(BeatData beat)
        {
            float duration = beat.Seconds > 0f ? beat.Seconds : GameConfig.DialoguePullDownDuration;

            _lineOnScreen = false;
            _dialogue.PullDown(duration);

            yield return StoryClock.Wait(duration);
        }

        /// <summary>Names the act over a darkened screen.</summary>
        private IEnumerator PlayTitleCard()
        {
            HideDialogue(GameConfig.DialogueFadeDuration);

            yield return _overlay.PlayTitleCard(
                Mathf.Max(1, _act.ActNumber),
                _act.Title.Current(),
                GameConfig.TitleCardHoldDuration);
        }

        /// <summary>True when the act names itself somewhere in its own script.</summary>
        private bool HasTitleCardBeat()
        {
            for (int i = 0; i < _act.Count; i++)
            {
                BeatData beat = _act.At(i);

                if (beat != null && beat.Kind == StoryBeatKind.TitleCard)
                {
                    return true;
                }
            }

            return false;
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

            Speak(beat.Speaker, beat.Text, beat.VoiceClip, beat.TypeSpeed);

            yield return WaitForLine(beat.MeasurePatience);
        }

        /// <summary>Puts one line on screen and remembers it.</summary>
        /// <remarks>
        /// The recording, when there is one, starts with the text rather than
        /// after it. A voiced line whose audio begins once the typing has
        /// finished is two performances of the same sentence in a row.
        /// </remarks>
        private void Speak(Speaker speaker, LocalizedLine line, string voiceClip = null, float typeSpeed = 0f)
        {
            _spokenSpeaker = speaker;
            _spokenLine = line;
            _spokenVoiceClip = voiceClip;
            _lineOnScreen = true;

            // Whatever was still being said belongs to the line before this one.
            VoiceService.Silence();

            _dialogue.Show(GameConfig.DialogueFadeDuration);
            _dialogue.ShowLine(speaker, line.Current(), typeSpeed);

            VoiceService.Speak(voiceClip);
        }

        /// <summary>
        /// Takes the box away, and with it the line a language change would
        /// otherwise redraw into an empty box.
        /// </summary>
        private void HideDialogue(float duration)
        {
            _lineOnScreen = false;
            _dialogue.Hide(duration);
        }

        /// <summary>
        /// Waits out one line: the first press finishes the typing, the second
        /// moves on.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The queue is cleared before waiting rather than after. A press that
        /// arrived during a transition belongs to the transition, and letting it
        /// carry over would silently eat the first line of every new scene.
        /// </para>
        /// <para>
        /// There is no hold-to-skip. A key that runs the text past at eight
        /// times speed is a key that skips the game, and this one is a game
        /// about being made to sit through things.
        /// </para>
        /// </remarks>
        private IEnumerator WaitForLine(bool measurePatience = false)
        {
            _advanceQueued = false;

            if (_cinema)
            {
                yield return WatchLine();
                yield break;
            }

            while (_dialogue.IsTyping)
            {
                if (!IsPaused && _advanceQueued)
                {
                    _advanceQueued = false;
                    _dialogue.CompleteLine();
                    break;
                }

                yield return null;
            }

            float held = 0f;
            bool counted = false;

            // The room while somebody is sitting in it. See DriftVeil.
            float restingVeil = _grade != null ? _grade.Amount : -1f;
            string pendingEnding = measurePatience ? Endings.AssetNameForCurrentRun() : null;
            float nextHeartbeat = GameConfig.PatienceHeartbeatSlowest;

            while (true)
            {
                if (!IsPaused && _advanceQueued)
                {
                    _advanceQueued = false;
                    RestoreVeil(restingVeil);
                    yield break;
                }

                // Counted from the moment the typing stops, so the wait is the
                // same length in every language and at every reading speed. It
                // is also counted on the story's clock, so a game left paused
                // in a menu is not a game left sitting with someone.
                if (measurePatience && !counted)
                {
                    held += StoryClock.DeltaTime;

                    DriftVeil(restingVeil, pendingEnding, held);

                    if (held >= nextHeartbeat)
                    {
                        AudioService.Play(SfxId.Heartbeat, GameConfig.PatienceHeartbeatVolume);
                        nextHeartbeat = held + HeartbeatGap(held);
                    }

                    if (held >= GameConfig.PatienceSeconds)
                    {
                        counted = true;
                        StorySession.RecordPatience();

                        RestoreVeil(restingVeil);

                        // Nothing said the wait was worth anything until this
                        // moment, and this is the only moment it ever says so.
                        yield return OfferEnding();

                        if (_pendingEnding != null)
                        {
                            yield break;
                        }
                    }
                }

                yield return null;
            }
        }

        /// <summary>
        /// Waits out one line of a scene the player is watching rather than
        /// reading.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The typing is allowed to finish and then the line is held for as long
        /// as it takes to read, which is worked out from its length in the
        /// language actually on screen — a Persian line and its English
        /// translation are not the same number of characters and should not get
        /// the same time.
        /// </para>
        /// <para>
        /// A recorded reading, if there is one, wins outright. The clip is the
        /// performance; holding for a character count while a voice is still
        /// speaking would cut it off mid-sentence.
        /// </para>
        /// <para>
        /// Patience is not measured here and cannot be. The five minutes are
        /// earned by a player choosing to sit with something, and a player who
        /// has no button to press has not chosen anything.
        /// </para>
        /// </remarks>
        private IEnumerator WatchLine()
        {
            while (_dialogue.IsTyping)
            {
                yield return null;
            }

            float hold = Mathf.Clamp(
                _spokenLine.Current().Length * GameConfig.CinemaSecondsPerCharacter,
                GameConfig.CinemaMinimumLineSeconds,
                GameConfig.CinemaMaximumLineSeconds);

            float spoken = VoiceService.LengthOf(_spokenVoiceClip);

            if (spoken > 0f)
            {
                hold = Mathf.Max(hold, spoken);
            }

            yield return StoryClock.Wait(hold + GameConfig.CinemaLineGapSeconds);
        }

        /// <summary>
        /// Moves the room, very slightly, while somebody sits with a silence.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The design document's requirement that the five minutes not feel like
        /// a paused game, and that the two pending endings feel marginally
        /// different without the difference being noticeable. Warmer as Yua's
        /// ending approaches, cooler as Haru's does, over the whole five minutes,
        /// by an amount that is below the threshold of a thing you could point
        /// at.
        /// </para>
        /// <para>
        /// Nothing happens at all on a run that has earned neither, which is the
        /// honest version: there is nothing coming, so the room has no reason to
        /// change. A player cannot tell the difference between that and the
        /// drift, which is the intention — it must never become a way of finding
        /// out whether the wait is worth anything.
        /// </para>
        /// </remarks>
        private void DriftVeil(float restingVeil, string pendingEnding, float held)
        {
            if (_grade == null || restingVeil < 0f || pendingEnding == null)
            {
                return;
            }

            float progress = Mathf.Clamp01(held / Mathf.Max(0.001f, GameConfig.PatienceSeconds));

            float direction = pendingEnding == Endings.GoodAssetName ? 1f : -1f;

            _grade.SetAmount(restingVeil + direction * GameConfig.PatienceGradeDrift * progress);
        }

        /// <summary>
        /// How long until the next beat, given how long somebody has been
        /// sitting here.
        /// </summary>
        /// <remarks>
        /// Unlike the veil, this one is on for every run — including the mixed
        /// ones that have earned nothing. The pulse is there so the game does
        /// not read as frozen, and a player being able to tell from the silence
        /// whether their playthrough still qualifies would be the one thing this
        /// mechanic must never leak.
        /// </remarks>
        private static float HeartbeatGap(float held)
        {
            float progress = Mathf.Clamp01(held / Mathf.Max(0.001f, GameConfig.PatienceSeconds));

            return Mathf.Lerp(
                GameConfig.PatienceHeartbeatSlowest,
                GameConfig.PatienceHeartbeatFastest,
                progress);
        }

        /// <summary>Puts the room back where the act left it.</summary>
        private void RestoreVeil(float restingVeil)
        {
            if (_grade != null && restingVeil >= 0f)
            {
                _grade.SetAmount(restingVeil);
            }
        }

        private IEnumerator PlayBackground(BeatData beat)
        {
            bool changing = !string.Equals(_stage.CurrentBackground, beat.Background, StringComparison.Ordinal);

            float fade = beat.FadeSeconds >= 0f ? beat.FadeSeconds : GameConfig.BackgroundFadeDuration;

            // Short enough to be a cut rather than a move. A cut gets no chime
            // and no run-up: both of those are there to make a change of place
            // feel like going somewhere, and act six's montage is not going
            // anywhere — it is one picture being interrupted by another.
            bool cut = fade <= GameConfig.BackgroundCutThreshold;

            if (changing)
            {
                if (cut)
                {
                    HideDialogue(0f);
                }
                else
                {
                    HideDialogue(GameConfig.DialogueFadeDuration);

                    if (!beat.PlaySound)
                    {
                        AudioService.Play(SfxId.WishStar, 0.8f);
                    }

                    yield return StoryClock.Wait(GameConfig.DialogueFadeDuration * 0.6f);
                }
            }

            bool done = false;
            _stage.ShowBackground(beat.Background, fade, () => done = true);

            while (!done)
            {
                yield return null;
            }

            if (!beat.Caption.IsEmpty)
            {
                _overlay.ShowCaption(beat.Caption, GameConfig.CaptionHoldDuration);
            }
        }

        /// <summary>Holds on the picture with the box out of the way.</summary>
        private IEnumerator HoldOnPicture(float seconds)
        {
            HideDialogue(GameConfig.DialogueFadeDuration);

            yield return StoryClock.Wait(seconds);
        }

        private IEnumerator PlayChoice(BeatData beat)
        {
            if (beat.Choices == null || beat.Choices.Length == 0)
            {
                yield break;
            }

            HideDialogue(GameConfig.DialogueFadeDuration);

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

            if (beat.YuaOverridesKindness && choice.Tone == ChoiceTone.Kind)
            {
                yield return PlayKindnessOverride(beat);
            }
        }

        /// <summary>
        /// Yua takes a kind choice back.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The design document's psychological override, and the reason act two
        /// exists. The blue button is offered honestly and pressed honestly, and
        /// then refused by the character the player thought they were being kind
        /// to. Her face goes flat for a beat — the only place in the act it does
        /// — and the scene carries on down the green path as though the option
        /// had never been on screen.
        /// </para>
        /// <para>
        /// What is deliberately not undone is the count. The press was recorded
        /// as kind before this ran, because the endings are decided by what the
        /// player reached for and not by what she allowed them to have. A
        /// playthrough that presses blue every time and is refused every time is
        /// still a pure run, and still opens the secret ending.
        /// </para>
        /// <para>
        /// Her expression is left flat when this returns. The beat after the
        /// choice is expected to set a face, exactly as it must for the green
        /// path, so the script says what she looks like next either way.
        /// </para>
        /// </remarks>
        private IEnumerator PlayKindnessOverride(BeatData beat)
        {
            AudioService.Play(SfxId.StringPull, 0.9f);

            _stage.ShowCharacter(Speaker.Yua, Portrait.DeadEyes, GameConfig.CharacterFadeDuration * 0.5f);
            _stage.SetFocus(Speaker.Yua, GameConfig.CharacterFocusDuration);

            yield return HoldOnPicture(GameConfig.ChoiceOverrideHoldDuration);

            if (beat.OverrideLine.IsEmpty)
            {
                yield break;
            }

            Speak(Speaker.Yua, beat.OverrideLine);

            yield return WaitForLine();
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

            if (_interludeDepth >= MaxInterludeDepth)
            {
                Debug.LogWarning(
                    $"[Story] '{interlude.name}' was reached {MaxInterludeDepth} interludes deep and was " +
                    "skipped. Two interludes that play each other never finish; check their Interlude beats.");

                yield break;
            }

            _interludeDepth++;

            for (int i = 0; i < interlude.Count; i++)
            {
                BeatData inner = interlude.At(i);

                if (inner == null || inner.Kind == StoryBeatKind.End)
                {
                    continue;
                }

                yield return PlayBeat(inner);
            }

            _interludeDepth--;
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
            float grade = -1f;
            Color? stain = null;

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

                    case StoryBeatKind.Grade:
                        grade = beat.Amount;
                        break;

                    case StoryBeatKind.Stain:
                        stain = beat.Tint;
                        break;

                    case StoryBeatKind.EnterCinema:
                        _cinema = true;
                        _dialogue.SetPromptSuppressed(true);
                        break;

                    case StoryBeatKind.ExitCinema:
                        _cinema = false;
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

            // A save taken after act five has taken the veil off must not
            // reopen behind it, and one taken after the corridor must not
            // reopen with the floor clean.
            if (_grade != null)
            {
                if (grade >= 0f)
                {
                    _grade.SetAmount(grade);
                }

                if (stain.HasValue)
                {
                    _grade.Stain(stain.Value, 0f);
                }
            }
        }
    }
}
