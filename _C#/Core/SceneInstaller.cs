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
using TheFrayedRedString.Motion;
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

            AmbientMotionInstaller.Install(scene);
            Light2DAmbientPulse.InstallAll(scene);
            SceneAudioInstaller.Install(scene);

            // Last: the controller's Start runs after everything above exists,
            // so it can assume motion channels and sounds are already in place.
            AttachSceneController(scene);
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
                    // Scenes with no controller of their own still receive the
                    // ambient motion, audio and localisation passes above.
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
