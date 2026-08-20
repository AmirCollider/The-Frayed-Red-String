// -----------------------------------------------------------------------------
//  The Frayed Red String
//  VoiceLibraryBuilder.cs  (Editor only)
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using TheFrayedRedString.Audio;
using UnityEditor;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>
    /// Bakes the recorded lines into a Resources asset the story can load by
    /// name.
    /// </summary>
    /// <remarks>
    /// The same scan-a-folder arrangement the music library uses. Nothing in the
    /// project has any recordings yet, and this is written so that staying that
    /// way costs nothing: an absent folder is not an error and produces no
    /// asset, no warning and no console noise.
    /// </remarks>
    [InitializeOnLoad]
    public static class VoiceLibraryBuilder
    {
        private const string VoiceFolder = "Assets/Audio/Voice";

        private const string ResourcesFolder = "Assets/Resources";
        private const string LibraryFolder = ResourcesFolder + "/TFRS";
        private const string LibraryPath = LibraryFolder + "/" + VoiceLibrary.ResourceName + ".asset";

        static VoiceLibraryBuilder()
        {
            EditorApplication.delayCall += Rebuild;
        }

        /// <summary>Creates or refreshes the voice library.</summary>
        [MenuItem("The Frayed Red String/Rebuild Voice Library")]
        public static void Rebuild()
        {
            if (!AssetDatabase.IsValidFolder(VoiceFolder))
            {
                // No recordings folder is the normal state of this project, and
                // will be until somebody records something.
                return;
            }

            VoiceLibrary.Entry[] lines = Collect();

            VoiceLibrary library = AssetDatabase.LoadAssetAtPath<VoiceLibrary>(LibraryPath);
            bool created = false;

            if (library == null)
            {
                AssetPaths.EnsureFolder(ResourcesFolder);
                AssetPaths.EnsureFolder(LibraryFolder);

                library = ScriptableObject.CreateInstance<VoiceLibrary>();
                AssetDatabase.CreateAsset(library, LibraryPath);
                created = true;
            }
            else if (IsUpToDate(library, lines))
            {
                return;
            }

            library.SetLines(lines);

            EditorUtility.SetDirty(library);
            AssetDatabase.SaveAssets();

            Debug.Log(
                $"[Voice] {(created ? "Generated" : "Refreshed")} the voice library with {lines.Length} " +
                "recording(s).");
        }

        /// <summary>
        /// Every clip in the folder, keyed by file name with the whitespace
        /// trimmed.
        /// </summary>
        /// <remarks>
        /// Sub-folders are included and their names are not part of the key, so
        /// the recordings can be filed per act or per character without a script
        /// having to know where they were put.
        /// </remarks>
        private static VoiceLibrary.Entry[] Collect()
        {
            Dictionary<string, AudioClip> found = new Dictionary<string, AudioClip>(128);

            foreach (string guid in AssetDatabase.FindAssets("t:AudioClip", new[] { VoiceFolder }))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string key = Path.GetFileNameWithoutExtension(path)?.Trim();

                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }

                AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);

                if (clip == null)
                {
                    continue;
                }

                if (found.ContainsKey(key))
                {
                    Debug.LogWarning(
                        $"[Voice] Two recordings are both called '{key}'. Only one of them can ever be " +
                        $"played, and which one is not defined. One of them is at '{path}'.");

                    continue;
                }

                found[key] = clip;
            }

            List<VoiceLibrary.Entry> entries = new List<VoiceLibrary.Entry>(found.Count);

            foreach (KeyValuePair<string, AudioClip> pair in found)
            {
                entries.Add(new VoiceLibrary.Entry { Name = pair.Key, Clip = pair.Value });
            }

            // Sorted so the generated asset has a stable order and does not
            // produce a diff every time the folder is enumerated differently.
            entries.Sort((a, b) => string.CompareOrdinal(a.Name, b.Name));

            return entries.ToArray();
        }

        private static bool IsUpToDate(VoiceLibrary library, VoiceLibrary.Entry[] fresh)
        {
            VoiceLibrary.Entry[] stored = library.Lines;

            if (stored == null || stored.Length != fresh.Length)
            {
                return false;
            }

            for (int i = 0; i < stored.Length; i++)
            {
                if (stored[i].Clip == null || stored[i].Name != fresh[i].Name)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
