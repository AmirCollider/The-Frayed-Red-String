// -----------------------------------------------------------------------------
//  The Frayed Red String
//  GameBootstrap.cs
//
//  The single entry point of the runtime. Nothing needs to be dragged into a
//  scene for the game to work: dropping this folder into the project is enough.
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Localization;
using TheFrayedRedString.Presentation;
using TheFrayedRedString.SaveSystem;
using TheFrayedRedString.Tweening;
using TheFrayedRedString.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheFrayedRedString.Core
{
    /// <summary>
    /// Creates the persistent services and hands the running scene to
    /// <see cref="SceneInstaller"/>.
    /// </summary>
    public static class GameBootstrap
    {
        private const string ServiceHostName = "[TFRS] Runtime Services";

        private static bool _initialized;

        /// <summary>The DontDestroyOnLoad object every service lives on.</summary>
        public static GameObject ServiceHost { get; private set; }

        /// <summary>
        /// Clears every piece of static state.
        /// </summary>
        /// <remarks>
        /// Required because Unity can be configured to skip the domain reload
        /// when entering play mode. Statics then survive from the previous
        /// session and the second Play press starts with stale singletons
        /// pointing at destroyed objects. Resetting here makes the two paths
        /// behave identically.
        /// </remarks>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _initialized = false;
            ServiceHost = null;

            TweenRunner.ResetStatics();
            AudioService.ResetStatics();
            MusicService.ResetStatics();
            ScreenFader.ResetStatics();
            SceneTransitionService.ResetStatics();
            LocalizationService.ResetStatics();
            SaveService.ResetStatics();
            UiSpriteLibrary.ResetStatics();
            MusicLibrary.ResetStatics();
            SceneInstaller.ResetStatics();
            ProceduralSfxLibrary.Clear();
        }

        /// <summary>
        /// Builds the service layer before the first scene is handed to us, so
        /// the fade curtain is already down when that scene appears.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;

            LocalizationService.Initialize();

            ServiceHost = new GameObject(ServiceHostName);
            Object.DontDestroyOnLoad(ServiceHost);

            TweenRunner.Install(ServiceHost);
            AudioService.Install(ServiceHost);
            MusicService.Install(ServiceHost);
            ScreenFader.Install(ServiceHost);
            GlobalPointerSfx.Install(ServiceHost);
            LocalizationRefresher.Install(ServiceHost);

            SceneInstaller.Enable();
        }

        /// <summary>
        /// Installs into the scene that was already loading while
        /// <see cref="Initialize"/> ran. Later scenes come through
        /// <see cref="SceneManager.sceneLoaded"/> instead.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InstallFirstScene()
        {
            SceneInstaller.InstallInto(SceneManager.GetActiveScene());

            // Safety net for a scene with no controller of its own, which would
            // otherwise sit behind black forever.
            //
            // The test is whether anyone *asked* for the reveal, not whether the
            // screen is still dark. A legitimate fade can easily still be
            // running here — the first frame of this game builds a multi-megabyte
            // font atlas — and checking darkness alone would report a failure
            // every launch while the fade was working perfectly.
            TweenRunner.After(GameConfig.BootRevealSafetyDelay, () =>
            {
                if (ScreenFader.RevealRequested || SceneTransitionService.IsTransitioning)
                {
                    return;
                }

                Debug.LogWarning(
                    "[Bootstrap] No scene controller asked for the screen to be revealed; doing it here. " +
                    "Scenes other than WarningScene and MainMenu have no controller of their own.");

                ScreenFader.FadeIn(GameConfig.BootFadeInDuration);
            });
        }
    }
}
