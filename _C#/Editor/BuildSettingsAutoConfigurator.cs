// -----------------------------------------------------------------------------
//  The Frayed Red String
//  BuildSettingsAutoConfigurator.cs  (Editor only)
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using TheFrayedRedString.Core;
using UnityEditor;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>
    /// Keeps WarningScene and MainMenu present in Build Settings.
    /// </summary>
    /// <remarks>
    /// <see cref="UnityEngine.SceneManagement.SceneManager.LoadScene(string)"/>
    /// can only load scenes listed in Build Settings, and a scene that is merely
    /// missing from that list fails at runtime with a message most people never
    /// see. Since the scene list is not part of the scripts being shared, this
    /// repairs it automatically on import.
    /// </remarks>
    [InitializeOnLoad]
    public static class BuildSettingsAutoConfigurator
    {
        /// <summary>Scenes the game needs, in the order they should boot.</summary>
        private static readonly string[] RequiredScenes =
        {
            SceneNames.Warning,
            SceneNames.MainMenu
        };

        static BuildSettingsAutoConfigurator()
        {
            // Asset queries are unreliable while the Editor is still importing,
            // so the work is deferred to the next editor tick.
            EditorApplication.delayCall += EnsureScenesRegistered;
        }

        /// <summary>Adds any missing required scene to Build Settings.</summary>
        [MenuItem("The Frayed Red String/Repair Build Settings")]
        public static void EnsureScenesRegistered()
        {
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            bool changed = false;

            for (int i = 0; i < RequiredScenes.Length; i++)
            {
                string sceneName = RequiredScenes[i];
                string scenePath = FindScenePath(sceneName);

                if (string.IsNullOrEmpty(scenePath))
                {
                    Debug.LogWarning($"[BuildSettings] No scene asset named '{sceneName}' exists in the project.");
                    continue;
                }

                if (ContainsPath(scenes, scenePath))
                {
                    continue;
                }

                // The warning screen is the game's first frame, so it belongs at
                // index 0; anything else is simply appended.
                if (i == 0)
                {
                    scenes.Insert(0, new EditorBuildSettingsScene(scenePath, true));
                }
                else
                {
                    scenes.Add(new EditorBuildSettingsScene(scenePath, true));
                }

                changed = true;
                Debug.Log($"[BuildSettings] Added '{sceneName}' to the scene list.");
            }

            if (changed)
            {
                EditorBuildSettings.scenes = scenes.ToArray();
            }
        }

        private static bool ContainsPath(List<EditorBuildSettingsScene> scenes, string path)
        {
            for (int i = 0; i < scenes.Count; i++)
            {
                if (scenes[i] != null && scenes[i].path == path)
                {
                    return true;
                }
            }

            return false;
        }

        private static string FindScenePath(string sceneName)
        {
            string[] guids = AssetDatabase.FindAssets($"t:Scene {sceneName}");

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);

                // FindAssets matches loosely, so confirm the file name exactly —
                // otherwise "MainMenu" would also match "MainMenuTest".
                if (System.IO.Path.GetFileNameWithoutExtension(path) == sceneName)
                {
                    return path;
                }
            }

            return null;
        }
    }
}
