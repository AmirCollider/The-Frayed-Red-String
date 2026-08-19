// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryAssetBuilder.cs  (Editor only)
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using TheFrayedRedString.Narrative;
using UnityEditor;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>
    /// Keeps the act library and the stage settings in step with the project.
    /// </summary>
    /// <remarks>
    /// Acts are ordinary assets that can live anywhere, so the runtime cannot
    /// find them by folder. This scans for them and records the result where
    /// <see cref="ActLibrary"/> can load it — the same arrangement the music and
    /// sprite libraries use, for the same reason.
    /// </remarks>
    [InitializeOnLoad]
    public static class StoryAssetBuilder
    {
        private const string ResourcesFolder = "Assets/Resources";
        private const string LibraryFolder = ResourcesFolder + "/TFRS";
        private const string ActsFolder = "Assets/Story/Acts";

        private const string LibraryPath = LibraryFolder + "/" + ActLibrary.ResourceName + ".asset";
        private const string SettingsPath = LibraryFolder + "/" + StageSettings.ResourceName + ".asset";

        static StoryAssetBuilder()
        {
            EditorApplication.delayCall += Rebuild;
        }

        /// <summary>The folder new acts are created in.</summary>
        public static string ActFolder => ActsFolder;

        /// <summary>Creates or refreshes the act library and the stage settings.</summary>
        [MenuItem("The Frayed Red String/Rebuild Act Library")]
        public static void Rebuild()
        {
            EnsureStageSettings();

            List<ActAsset> acts = FindActs();

            ActLibrary library = AssetDatabase.LoadAssetAtPath<ActLibrary>(LibraryPath);
            bool created = false;

            if (library == null)
            {
                AssetPaths.EnsureFolder(ResourcesFolder);
                AssetPaths.EnsureFolder(LibraryFolder);

                library = ScriptableObject.CreateInstance<ActLibrary>();
                AssetDatabase.CreateAsset(library, LibraryPath);
                created = true;
            }
            else if (IsUpToDate(library, acts))
            {
                return;
            }

            library.SetActs(acts.ToArray());

            EditorUtility.SetDirty(library);
            AssetDatabase.SaveAssets();

            Debug.Log($"[Story] {(created ? "Generated" : "Refreshed")} the act library with {acts.Count} act(s).");
        }

        /// <summary>The stage settings asset, created on first use.</summary>
        public static StageSettings EnsureStageSettings()
        {
            StageSettings settings = AssetDatabase.LoadAssetAtPath<StageSettings>(SettingsPath);

            if (settings != null)
            {
                return settings;
            }

            AssetPaths.EnsureFolder(ResourcesFolder);
            AssetPaths.EnsureFolder(LibraryFolder);

            settings = ScriptableObject.CreateInstance<StageSettings>();
            AssetDatabase.CreateAsset(settings, SettingsPath);
            AssetDatabase.SaveAssets();

            Debug.Log($"[Story] Created stage settings at {SettingsPath}.");
            return settings;
        }

        /// <summary>
        /// Every act in the project, numbered ones first and in order.
        /// </summary>
        /// <remarks>
        /// Interludes have no act number and sort to the end. They are still
        /// listed so the editor can offer them, and so an act that references one
        /// keeps working if it is moved.
        /// </remarks>
        public static List<ActAsset> FindActs()
        {
            List<ActAsset> acts = new List<ActAsset>();
            string[] guids = AssetDatabase.FindAssets("t:" + nameof(ActAsset));

            for (int i = 0; i < guids.Length; i++)
            {
                ActAsset act = AssetDatabase.LoadAssetAtPath<ActAsset>(AssetDatabase.GUIDToAssetPath(guids[i]));

                if (act != null)
                {
                    acts.Add(act);
                }
            }

            acts.Sort((a, b) =>
            {
                bool aNumbered = a.ActNumber > 0;
                bool bNumbered = b.ActNumber > 0;

                if (aNumbered != bNumbered)
                {
                    return aNumbered ? -1 : 1;
                }

                int byNumber = a.ActNumber.CompareTo(b.ActNumber);
                return byNumber != 0 ? byNumber : string.CompareOrdinal(a.name, b.name);
            });

            return acts;
        }

        /// <summary>Creates a new, empty act asset and returns it.</summary>
        public static ActAsset CreateAct(int actNumber, string assetName)
        {
            AssetPaths.EnsureFolder("Assets/Story");
            AssetPaths.EnsureFolder(ActsFolder);

            ActAsset act = ScriptableObject.CreateInstance<ActAsset>();
            act.ActNumber = actNumber;
            act.ShowTitleCard = actNumber > 0;

            string path = AssetDatabase.GenerateUniqueAssetPath($"{ActsFolder}/{assetName}.asset");
            AssetDatabase.CreateAsset(act, path);
            AssetDatabase.SaveAssets();

            Rebuild();
            return act;
        }

        private static bool IsUpToDate(ActLibrary library, List<ActAsset> acts)
        {
            ActAsset[] stored = library.Acts;

            if (stored == null || stored.Length != acts.Count)
            {
                return false;
            }

            for (int i = 0; i < stored.Length; i++)
            {
                if (stored[i] != acts[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
