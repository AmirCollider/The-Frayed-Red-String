// -----------------------------------------------------------------------------
//  The Frayed Red String
//  GameConfig.cs
//
//  Central, designer-facing tuning values. Every magic number that controls how
//  the game *feels* lives here so that pacing can be tuned in one place instead
//  of being scattered across a dozen behaviours.
// -----------------------------------------------------------------------------

namespace TheFrayedRedString.Core
{
    /// <summary>
    /// Immutable tuning constants shared by the whole runtime.
    /// </summary>
    public static class GameConfig
    {
        // ---------------------------------------------------------------------
        //  Scene transitions
        // ---------------------------------------------------------------------

        /// <summary>Duration of the very first fade-in when the game boots.</summary>
        public const float BootFadeInDuration = 1.60f;

        /// <summary>
        /// How long the boot safety net waits before assuming no scene
        /// controller is going to reveal the screen.
        /// </summary>
        public const float BootRevealSafetyDelay = 3.0f;

        /// <summary>Duration of the fade to black before a scene is unloaded.</summary>
        public const float SceneFadeOutDuration = 1.25f;

        /// <summary>Duration of the fade from black after a scene is loaded.</summary>
        public const float SceneFadeInDuration = 1.50f;

        /// <summary>
        /// Extra beat of pure black between the two halves of a transition.
        /// Gives the cut a deliberate, unhurried rhythm.
        /// </summary>
        public const float SceneFadeHoldDuration = 0.35f;

        // ---------------------------------------------------------------------
        //  Warning scene
        // ---------------------------------------------------------------------

        /// <summary>
        /// How long the player must sit with the content warning before the
        /// "press Enter" prompt is allowed to appear.
        /// </summary>
        public const float WarningPromptDelay = 5.00f;

        /// <summary>Duration of the prompt's reveal animation.</summary>
        public const float WarningPromptRevealDuration = 0.85f;

        /// <summary>Scale the prompt springs up from when it is revealed.</summary>
        public const float WarningPromptRevealFromScale = 0.45f;

        /// <summary>
        /// Small pause after the confirmation sound so the click is heard in full
        /// before the screen starts fading.
        /// </summary>
        public const float WarningConfirmSettleDuration = 0.28f;

        // ---------------------------------------------------------------------
        //  Main menu
        // ---------------------------------------------------------------------

        /// <summary>Fade duration used when the load panel opens.</summary>
        public const float PanelOpenDuration = 0.45f;

        /// <summary>Fade duration used when the load panel closes.</summary>
        public const float PanelCloseDuration = 0.30f;

        /// <summary>
        /// Scale the load panel grows from when it opens.
        /// </summary>
        /// <remarks>
        /// Close to 1 on purpose. The whole panel is scaled, so every element
        /// inside it travels in proportion to its distance from the centre — and
        /// the further ones are the ones already touching an edge.
        /// </remarks>
        public const float PanelOpenFromScale = 0.96f;

        /// <summary>Duration of one half of the language-flag flip.</summary>
        public const float LanguageFlipHalfDuration = 0.16f;

        // ---------------------------------------------------------------------
        //  Story scenes
        // ---------------------------------------------------------------------

        /// <summary>
        /// Draw order of the canvas the story is played on.
        /// </summary>
        /// <remarks>
        /// The three story layers are given explicit orders rather than left to
        /// hierarchy position. Act01.unity happens to list its menu canvas
        /// before its background canvas, which with equal sorting orders draws
        /// the background over the menu — the kind of ordering accident that is
        /// invisible in the scene view and obvious the moment the game runs.
        /// </remarks>
        public const int BackgroundCanvasOrder = 0;

        /// <summary>Draw order of the dialogue, captions and choices.</summary>
        public const int StoryCanvasOrder = 100;

        /// <summary>Draw order of the pause layer, above everything but the fade curtain.</summary>
        public const int PauseCanvasOrder = 500;

        /// <summary>
        /// Draw order of the film screen, above the pause layer.
        /// </summary>
        /// <remarks>
        /// A film replaces the picture rather than sitting inside it, so nothing
        /// the game normally draws belongs over it — not the frame, not the
        /// dialogue box, and not a pause menu that happens to be open when it
        /// starts.
        /// </remarks>
        public const int FilmCanvasOrder = 700;

        /// <summary>Characters revealed per second while reading normally.</summary>
        public const float TypeSpeedNormal = 45f;

        /// <summary>Length of the dialogue box's fade, either direction.</summary>
        public const float DialogueFadeDuration = 0.35f;

        /// <summary>Length of a crossfade between two backgrounds.</summary>
        public const float BackgroundFadeDuration = 1.10f;

        /// <summary>How long a character takes to walk on or off.</summary>
        public const float CharacterFadeDuration = 0.50f;

        /// <summary>How long the lighting takes to move to a new speaker.</summary>
        public const float CharacterFocusDuration = 0.30f;

        /// <summary>How long a place name stays in the corner.</summary>
        public const float CaptionHoldDuration = 2.40f;

        /// <summary>How long an act's title card holds at full opacity.</summary>
        public const float TitleCardHoldDuration = 2.60f;

        /// <summary>
        /// How long the frame takes to open when the story opens it.
        /// </summary>
        /// <remarks>
        /// Slow on purpose. The frame coming off is the moment the game stops
        /// pretending, and a fast version of it reads as a screen-size glitch
        /// rather than as something a character did.
        /// </remarks>
        public const float FrameOpenDuration = 2.40f;

        /// <summary>
        /// How long Yua's face stays flat when she takes a kind choice back.
        /// </summary>
        /// <remarks>
        /// Long enough to be certain it happened and short enough to be denied
        /// afterwards. Act two's whole trick is that the player cannot quite
        /// point at the moment the game turned on them.
        /// </remarks>
        public const float ChoiceOverrideHoldDuration = 1.40f;

        /// <summary>
        /// How long a player has to sit with one of the game's silences before
        /// it counts as having been listened to.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Five minutes, from the design document, and the number the endings
        /// are built on. The lines that measure it are the ones where Haru's leg
        /// stops him walking and where the machine room reaches Yua through a
        /// floor — the two places the story stops being sweet and nobody in it
        /// says why.
        /// </para>
        /// <para>
        /// Nothing on screen asks for the wait, and nothing marks it when it
        /// happens. A prompt would turn patience into a puzzle, and the game
        /// closes by telling the player what it was actually about.
        /// </para>
        /// </remarks>
        public static float PatienceSeconds => StoryTesting.PatienceSecondsOverride > 0f
            ? StoryTesting.PatienceSecondsOverride
            : DesignedPatienceSeconds;

        /// <summary>The real five minutes, as shipped.</summary>
        public const float DesignedPatienceSeconds = 300f;

        /// <summary>
        /// How far the veil drifts across a silence the player is sitting
        /// through.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The design document asks that the five minutes never feel like the
        /// game has stopped — that the characters and the sound and the room
        /// stay alive, and that what happens is slightly different depending on
        /// which ending is waiting, without being noticeable.
        /// </para>
        /// <para>
        /// This is that, and the number is small on purpose. Six per cent of a
        /// wash that is itself seventeen per cent opaque, moved over five
        /// minutes, is not visible as a change; it is only visible as the room
        /// having got very slightly warmer or colder by the time somebody looks
        /// up. Anything a player could point at would turn the wait into a
        /// progress bar.
        /// </para>
        /// </remarks>
        public const float PatienceGradeDrift = 0.06f;

        /// <summary>
        /// Seconds between heartbeats at the start of a silence, and at the end
        /// of one.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The design document asks that the five minutes have a pulse in them —
        /// movement, a heartbeat, something — so that the game does not read as
        /// frozen and the player can tell that it is still running. This is that
        /// part, and unlike the veil drift it is meant to be noticed.
        /// </para>
        /// <para>
        /// It closes up as the wait goes on, from one every twelve seconds to
        /// one every five. Nothing announces the change and nobody is going to
        /// time it; what it produces is a scene that feels very slightly more
        /// urgent by the end than it did at the start, which is the honest shape
        /// of sitting with somebody who is not saying anything.
        /// </para>
        /// </remarks>
        public const float PatienceHeartbeatSlowest = 12f;

        /// <summary>The closest the heartbeats get, at the end of the wait.</summary>
        public const float PatienceHeartbeatFastest = 5f;

        /// <summary>How loud they are. Well under the dialogue.</summary>
        public const float PatienceHeartbeatVolume = 0.35f;

        // ---------------------------------------------------------------------
        //  Story typography
        //
        //  Sizes are in the canvas' 1920 × 1080 reference space, so they scale
        //  with the window rather than with the monitor.
        // ---------------------------------------------------------------------

        public const float DialogueFontSize = 40f;
        public const float SpeakerFontSize = 32f;
        public const float ChoiceFontSize = 36f;
        public const float CaptionFontSize = 30f;
        public const float SeasonFontSize = 40f;
        public const float ActTitleFontSize = 88f;

        // ---------------------------------------------------------------------
        //  Audio
        // ---------------------------------------------------------------------

        /// <summary>Number of pooled voices used for one-shot UI sounds.</summary>
        public const int SfxVoiceCount = 10;

        /// <summary>Default master volume applied to every generated sound.</summary>
        public const float DefaultSfxVolume = 0.65f;

        /// <summary>
        /// Random pitch spread applied to UI sounds so repeated clicks never
        /// sound mechanically identical.
        /// </summary>
        public const float SfxPitchJitter = 0.045f;

        /// <summary>Default master volume for background music.</summary>
        public const float DefaultMusicVolume = 0.45f;

        /// <summary>How long music takes to rise from silence.</summary>
        public const float MusicFadeInDuration = 2.2f;

        /// <summary>How long music takes to fall to silence.</summary>
        public const float MusicFadeOutDuration = 1.0f;

        /// <summary>
        /// Default master volume for recorded lines.
        /// </summary>
        /// <remarks>
        /// Louder than the music and the effects, because from act five onward
        /// it is the thing the player is actually listening to. Nothing is voiced
        /// before then, so this affects one act.
        /// </remarks>
        public const float DefaultVoiceVolume = 0.85f;

        // ---------------------------------------------------------------------
        //  Act five
        // ---------------------------------------------------------------------

        /// <summary>
        /// How strongly the picture is veiled for the first four acts.
        /// </summary>
        /// <remarks>
        /// The design document's pane of frosted glass: a soft pink wash over
        /// everything, weak enough that nobody looking at one screenshot would
        /// call it an effect, and obvious the moment it is taken away. Act five
        /// takes it away.
        /// </remarks>
        public const float StoryGradeDefault = 1f;

        /// <summary>How long the veil takes to lift when the story lifts it.</summary>
        public const float GradeChangeDuration = 3.0f;

        /// <summary>
        /// How long Haru takes to drag the dialogue box off the bottom of the
        /// screen.
        /// </summary>
        /// <remarks>
        /// Slow, and slower at the start than at the end. A box that leaves in
        /// a fifth of a second reads as the interface being hidden; one that is
        /// hauled down over two seconds reads as somebody moving it, which is
        /// the entire point of the gesture.
        /// </remarks>
        public const float DialoguePullDownDuration = 2.0f;

        // ---------------------------------------------------------------------
        //  Act six
        // ---------------------------------------------------------------------

        /// <summary>
        /// Reading time allowed per character of a line while the story is
        /// playing itself.
        /// </summary>
        /// <remarks>
        /// Counted after the typing has finished, so it is time to take the line
        /// in rather than time to watch it appear. Deliberately generous: a
        /// player who cannot click is a player who has to be given enough, and
        /// the cost of being slightly too slow is patience where the cost of
        /// being slightly too fast is a scene nobody could read.
        /// </remarks>
        public const float CinemaSecondsPerCharacter = 0.045f;

        /// <summary>Shortest a line ever stays up in cinema mode.</summary>
        /// <remarks>
        /// Two people saying "…" and "うん" to each other still needs a beat
        /// between them, or the exchange reads as one flicker.
        /// </remarks>
        public const float CinemaMinimumLineSeconds = 1.60f;

        /// <summary>Longest, regardless of length.</summary>
        public const float CinemaMaximumLineSeconds = 9.00f;

        /// <summary>
        /// Extra pause after a line, so lines do not run into each other.
        /// </summary>
        public const float CinemaLineGapSeconds = 0.45f;

        /// <summary>
        /// At or below this, a background change is treated as a cut rather than
        /// as a move.
        /// </summary>
        public const float BackgroundCutThreshold = 0.15f;

        // ---------------------------------------------------------------------
        //  Persistence keys
        // ---------------------------------------------------------------------

        public const string PrefsPrefix = "TFRS.";
        public const string PrefsLanguage = PrefsPrefix + "Language";
        public const string PrefsSfxVolume = PrefsPrefix + "SfxVolume";
        public const string PrefsMusicVolume = PrefsPrefix + "MusicVolume";
        public const string PrefsVoiceVolume = PrefsPrefix + "VoiceVolume";
    }
}
