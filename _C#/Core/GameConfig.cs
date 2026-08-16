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

        /// <summary>Scale the load panel grows from when it opens.</summary>
        public const float PanelOpenFromScale = 0.92f;

        /// <summary>Duration of one half of the language-flag flip.</summary>
        public const float LanguageFlipHalfDuration = 0.16f;

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

        // ---------------------------------------------------------------------
        //  Persistence keys
        // ---------------------------------------------------------------------

        public const string PrefsPrefix = "TFRS.";
        public const string PrefsLanguage = PrefsPrefix + "Language";
        public const string PrefsSfxVolume = PrefsPrefix + "SfxVolume";
        public const string PrefsMusicVolume = PrefsPrefix + "MusicVolume";
    }
}
