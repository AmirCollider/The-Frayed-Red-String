// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryPlaytest.cs  (Editor only)
//
//  Playing the game from the middle of it.
//
//  An act is a hundred and thirty beats long and there is deliberately no key
//  that skips them — this is a game about being made to sit through things, and
//  a fast-forward key is a fast-forward key whoever is holding it. Which leaves
//  the writer with no way to look at beat ninety without reading eighty-nine
//  first, and a beat nobody can look at is a beat nobody checks.
//
//  So the jump lives here, in the Editor, where the player can never reach it.
// -----------------------------------------------------------------------------

using TheFrayedRedString.Core;
using TheFrayedRedString.Narrative;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Starts the game at a chosen act and beat.</summary>
    public static class StoryPlaytest
    {
        /// <summary>
        /// Enters play mode with <paramref name="act"/> loaded and running from
        /// <paramref name="beat"/>.
        /// </summary>
        /// <remarks>
        /// Everything before the beat is applied silently first — the background,
        /// who is on stage, the music, whether the frame is open — because that
        /// is what loading a save does, and testing a different path from the one
        /// players take tests the wrong thing.
        /// </remarks>
        public static void PlayFrom(ActAsset act, int beat)
        {
            if (act == null)
            {
                Debug.LogWarning("[Story] There is no act to play.");
                return;
            }

            if (act.ActNumber <= 0)
            {
                Debug.LogWarning(
                    $"[Story] '{act.name}' has no act number, so no scene plays it. Interludes are played " +
                    "by an Interlude beat inside a numbered act — put one in the act you want to test it " +
                    "from and play that.");
                return;
            }

            if (EditorApplication.isPlaying)
            {
                Debug.LogWarning("[Story] Stop play mode first, then press this again.");
                return;
            }

            string sceneName = SceneNames.ForAct(act.ActNumber);
            string scenePath = FindScene(sceneName);

            if (scenePath == null)
            {
                Debug.LogWarning(
                    $"[Story] Act {act.ActNumber:00} needs a scene called '{sceneName}.unity' and there " +
                    "is not one. Make it, then run The Frayed Red String > Prepare This Scene As An Act.");
                return;
            }

            // Ask before throwing away work. The alternative is a button that
            // silently discards the scene somebody was in the middle of editing.
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }

            SessionState.SetInt(StorySessionKeys.Act, act.ActNumber);
            SessionState.SetInt(StorySessionKeys.Beat, Mathf.Max(0, beat));

            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            EditorApplication.isPlaying = true;
        }

        /// <summary>Plays an act from its first beat.</summary>
        public static void PlayFromStart(ActAsset act)
        {
            PlayFrom(act, 0);
        }

        /// <summary>
        /// Starts the game where a player starts it: at the content warning.
        /// </summary>
        [MenuItem("The Frayed Red String/Play The Whole Game &g")]
        public static void PlayFromTheTop()
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                return;
            }

            string scenePath = FindScene(SceneNames.Warning);

            if (scenePath == null)
            {
                Debug.LogWarning($"[Story] No scene called '{SceneNames.Warning}.unity' was found.");
                return;
            }

            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }

            SessionState.EraseInt(StorySessionKeys.Act);
            SessionState.EraseInt(StorySessionKeys.Beat);

            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            EditorApplication.isPlaying = true;
        }

        /// <summary>The path of a scene by name, or null.</summary>
        private static string FindScene(string sceneName)
        {
            return AssetPaths.FindPathByExactName("t:Scene " + sceneName, sceneName);
        }
    }

    /// <summary>
    /// The two SessionState keys the jump is passed through.
    /// </summary>
    /// <remarks>
    /// Mirrored here rather than reached for on <see cref="StorySession"/>,
    /// whose copies are internal to the runtime assembly and therefore not
    /// visible from this one.
    /// </remarks>
    internal static class StorySessionKeys
    {
        public const string Act = "TFRS.Playtest.Act";
        public const string Beat = "TFRS.Playtest.Beat";
    }
}
