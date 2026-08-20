// -----------------------------------------------------------------------------
//  The Frayed Red String
//  SceneTransitionService.cs
// -----------------------------------------------------------------------------

using System;
using System.Collections;
using TheFrayedRedString.Audio;
using TheFrayedRedString.Core;
using TheFrayedRedString.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheFrayedRedString.Presentation
{
    /// <summary>
    /// Loads scenes behind the <see cref="ScreenFader"/> curtain: fade to black,
    /// load asynchronously, hold a beat, then fade back up.
    /// </summary>
    public static class SceneTransitionService
    {
        /// <summary>True while a transition is in flight.</summary>
        public static bool IsTransitioning { get; private set; }

        /// <summary>Clears static state between play sessions.</summary>
        public static void ResetStatics()
        {
            IsTransitioning = false;
        }

        /// <summary>
        /// Fades out, loads <paramref name="sceneName"/>, and leaves the curtain
        /// down. The incoming scene's controller raises it, so the new scene has
        /// a chance to arrange itself before it is revealed.
        /// </summary>
        /// <param name="sceneName">Scene to load. Must be in Build Settings.</param>
        /// <param name="fadeOutDuration">Length of the fade to black.</param>
        /// <param name="onFailed">
        /// Invoked instead of loading if the scene is not in Build Settings, so
        /// a missing scene surfaces as a warning rather than a black screen.
        /// </param>
        public static void LoadScene(
            string sceneName,
            float fadeOutDuration = GameConfig.SceneFadeOutDuration,
            Action onFailed = null)
        {
            LoadScene(sceneName, Color.black, fadeOutDuration, onFailed);
        }

        /// <summary>
        /// The same, through a curtain of a chosen colour.
        /// </summary>
        /// <remarks>
        /// Black for going back to the title, which is leaving; a colour for
        /// moving from one act to the next, which is not. It is one of the few
        /// places a scene change can say something about what kind of change it
        /// is, and it costs a single tint.
        /// </remarks>
        public static void LoadScene(
            string sceneName,
            Color curtain,
            float fadeOutDuration = GameConfig.SceneFadeOutDuration,
            Action onFailed = null)
        {
            if (IsTransitioning)
            {
                return;
            }

            if (!IsSceneAvailable(sceneName))
            {
                Debug.LogWarning(
                    $"[SceneTransition] Scene '{sceneName}' is not in Build Settings, so it cannot be loaded. " +
                    "Add it via File > Build Profiles > Scene List.");

                onFailed?.Invoke();
                return;
            }

            IsTransitioning = true;

            // Set before the fade rather than after: the curtain is already at
            // zero alpha, so changing its colour now is invisible, and changing
            // it once the screen is dark would be a visible shift on the way
            // back up.
            ScreenFader.SetColor(curtain);

            // Take the music down with the picture. A track that keeps playing
            // over a black screen belongs to a scene that is already gone; the
            // incoming scene asks for whatever it wants once it is up.
            MusicService.Stop(fadeOutDuration);

            ScreenFader.FadeOut(fadeOutDuration, () =>
            {
                TweenRunner.Instance.StartCoroutine(LoadRoutine(sceneName));
            });
        }

        /// <summary>
        /// True when a scene of this name is present in Build Settings and can
        /// therefore be loaded at runtime.
        /// </summary>
        /// <remarks>
        /// Walks the build list by index rather than asking whether a name is
        /// loadable, so the answer is the same in the Editor and in a player.
        /// </remarks>
        public static bool IsSceneAvailable(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                return false;
            }

            int sceneCount = SceneManager.sceneCountInBuildSettings;

            for (int i = 0; i < sceneCount; i++)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                string name = System.IO.Path.GetFileNameWithoutExtension(path);

                if (string.Equals(name, sceneName, System.StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static IEnumerator LoadRoutine(string sceneName)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            if (operation == null)
            {
                IsTransitioning = false;
                ScreenFader.FadeIn(GameConfig.SceneFadeInDuration);
                yield break;
            }

            while (!operation.isDone)
            {
                yield return null;
            }

            // A deliberate beat of pure black. Cutting straight from load to
            // fade-in reads as a stutter; the pause reads as a scene change.
            float held = 0f;
            while (held < GameConfig.SceneFadeHoldDuration)
            {
                held += Time.unscaledDeltaTime;
                yield return null;
            }

            IsTransitioning = false;
        }
    }
}
