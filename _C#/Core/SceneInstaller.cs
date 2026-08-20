// -----------------------------------------------------------------------------
//  The Frayed Red String
//  SceneInstaller.cs
//
//  Everything a scene needs at runtime is attached here rather than authored in
//  the .unity file. That keeps the scenes exactly as they were hand-built — no
//  components to wire, no references to lose on a merge — and means a scene
//  added later is animated, audible and localised the moment it loads.
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Localization;
using TheFrayedRedString.Motion;
using TheFrayedRedString.Presentation;
using TheFrayedRedString.Scenes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace TheFrayedRedString.Core
{
    /// <summary>
    /// Installs the runtime systems into every scene as it loads.
    /// </summary>
    public static class SceneInstaller
    {
        private const string ControllerHostName = "[TFRS] Scene Controller";

        private static bool _subscribed;

        /// <summary>Starts listening for scene loads and processes the current scene.</summary>
        public static void Enable()
        {
            if (_subscribed)
            {
                return;
            }

            _subscribed = true;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        /// <summary>Clears static state between play sessions.</summary>
        public static void ResetStatics()
        {
            if (_subscribed)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }

            _subscribed = false;
        }

        /// <summary>
        /// Installs into a scene that is already loaded.
        /// </summary>
        /// <remarks>
        /// The very first scene is live before any code of ours has had a chance
        /// to subscribe to <see cref="SceneManager.sceneLoaded"/>, so the boot
        /// path calls this explicitly. Every later scene arrives through the
        /// event.
        /// </remarks>
        public static void InstallInto(Scene scene)
        {
            if (!scene.IsValid() || !scene.isLoaded)
            {
                return;
            }

            EnsureEventSystem();
            AudioService.EnsureListener();

            // Every scene begins unpaused. The flag is static so it survives a
            // scene load, and a player who quits to the title from the pause
            // menu would otherwise arrive in the next act with the story already
            // held — a game that has stopped and nothing on screen saying so.
            StoryClock.IsPaused = false;

            // The font template is a component in a scene, so the previous
            // scene's has just been destroyed. Anything built after this point
            // has to go looking for the new scene's own label instead of
            // copying a dead one — which copies nothing, and leaves Persian as
            // a row of empty boxes.
            StoryFonts.ForgetTemplate();

            // And the scene's own labels get the language fonts they were never
            // given. A label set up with one font draws every script from it,
            // which is fine until it is asked for Japanese out of a Persian face
            // — the content-warning screen, which shows two languages at once
            // from one label, is that case by construction.
            int completed = StoryFonts.CompleteAuthoredFonts();

            if (completed > 0)
            {
                Debug.Log(
                    $"[Fonts] Filled in the missing language fonts on {completed} label(s) in " +
                    $"'{scene.name}'. Set them by hand on the Direct Font component to choose your own.");
            }

            AmbientMotionInstaller.Install(scene);
            Light2DAmbientPulse.InstallAll(scene);
            SceneAudioInstaller.Install(scene);

            // Last: the controller's Start runs after everything above exists,
            // so it can assume motion channels and sounds are already in place.
            AttachSceneController(scene);

            // And once the scene has had a few frames to build its fonts, every
            // label is laid out again. The first frames of a scene are exactly
            // where a label gets drawn before the font for its script exists —
            // which is why the content warning came up as boxes.
            LocalizationRefresher.Settle();
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            InstallInto(scene);
        }

        /// <summary>
        /// Creates the controller that owns this particular scene's behaviour.
        /// </summary>
        /// <remarks>
        /// If a controller was placed in the scene by hand — which is how you
        /// would tune its serialized fields in the Inspector — that one is left
        /// alone and no duplicate is created.
        /// </remarks>
        private static void AttachSceneController(Scene scene)
        {
            switch (scene.name)
            {
                case SceneNames.Warning:
                    EnsureController<WarningSceneController>();
                    break;

                case SceneNames.MainMenu:
                    EnsureController<MainMenuSceneController>();
                    break;

                default:
                    // Every act scene shares one controller, which works out
                    // which act it is from the scene's name. That is what makes
                    // a new act a scene plus an asset rather than a scene plus a
                    // class — see ActSceneController.
                    if (SceneNames.IsActScene(scene.name))
                    {
                        EnsureController<ActSceneController>();
                    }

                    break;
            }
        }

        private static void EnsureController<T>() where T : Component
        {
            // FindAny rather than FindFirst: this only asks whether a controller
            // exists at all, and there is never more than one, so paying for a
            // deterministic ordering would buy nothing.
            T existing = Object.FindAnyObjectByType<T>(FindObjectsInactive.Include);
            if (existing != null)
            {
                return;
            }

            GameObject host = new GameObject(ControllerHostName);
            host.AddComponent<T>();
        }

        /// <summary>
        /// Guarantees the scene can receive UI input. Both authored scenes ship
        /// with an EventSystem, but without one no button would ever respond and
        /// the failure is silent and baffling.
        /// </summary>
        private static void EnsureEventSystem()
        {
            if (EventSystem.current != null)
            {
                return;
            }

            if (Object.FindAnyObjectByType<EventSystem>(FindObjectsInactive.Include) != null)
            {
                return;
            }

            Debug.LogWarning("[SceneInstaller] No EventSystem in this scene; creating one so UI input works.");

            GameObject host = new GameObject("EventSystem");
            host.AddComponent<EventSystem>();
            host.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
        }
    }
}
