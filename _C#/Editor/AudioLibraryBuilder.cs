// -----------------------------------------------------------------------------
//  The Frayed Red String
//  AudioLibraryBuilder.cs  (Editor only)
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using TheFrayedRedString.Audio;
using UnityEditor;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>
    /// Bakes every clip in the music folder into a Resources asset.
    /// </summary>
    /// <remarks>
    /// Music lives outside Resources so it is not force-loaded at startup, but
    /// the runtime still needs to reach it by name. This scans the folder and
    /// records name → clip references in a single small asset, which means
    /// dropping a new track into <c>Assets/Audio/BackgroundMusics</c> is the
    /// entire workflow for adding music.
    /// </remarks>
    [InitializeOnLoad]
    public static class AudioLibraryBuilder
    {
        private const string MusicSourceFolder = "Assets/Audio/BackgroundMusics";

        private const string ResourcesFolder = "Assets/Resources";
        private const string LibraryFolder = ResourcesFolder + "/TFRS";
        private const string LibraryPath = LibraryFolder + "/" + MusicLibrary.ResourceName + ".asset";

        static AudioLibraryBuilder()
        {
            EditorApplication.delayCall += Rebuild;
        }

        /// <summary>Creates or refreshes the music library asset.</summary>
        [MenuItem("The Frayed Red String/Rebuild Music Library")]
        public static void Rebuild()
        {
            if (!AssetDatabase.IsValidFolder(MusicSourceFolder))
            {
                // Not an error: a project without a music folder simply has no
                // music yet.
                return;
            }

            List<MusicLibrary.Entry> entries = CollectTracks();

            MusicLibrary library = AssetDatabase.LoadAssetAtPath<MusicLibrary>(LibraryPath);
            bool created = false;

            if (library == null)
            {
                EnsureFolder(ResourcesFolder);
                EnsureFolder(LibraryFolder);

                library = ScriptableObject.CreateInstance<MusicLibrary>();
                AssetDatabase.CreateAsset(library, LibraryPath);
                created = true;
            }
            else if (IsUpToDate(library, entries))
            {
                return;
            }

            library.SetTracks(entries.ToArray());

            EditorUtility.SetDirty(library);
            AssetDatabase.SaveAssets();

            Debug.Log(
                $"[Audio] {(created ? "Generated" : "Refreshed")} the music library " +
                $"with {entries.Count} track(s) from {MusicSourceFolder}.");
        }

        private static List<MusicLibrary.Entry> CollectTracks()
        {
            List<MusicLibrary.Entry> entries = new List<MusicLibrary.Entry>();
            string[] guids = AssetDatabase.FindAssets("t:AudioClip", new[] { MusicSourceFolder });

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);

                if (clip == null)
                {
                    continue;
                }

                entries.Add(new MusicLibrary.Entry
                {
                    Name = Path.GetFileNameWithoutExtension(path),
                    Clip = clip
                });
            }

            return entries;
        }

        /// <summary>
        /// True when the stored table already matches the folder, so the asset
        /// is not rewritten (and version control not dirtied) on every reload.
        /// </summary>
        private static bool IsUpToDate(MusicLibrary library, List<MusicLibrary.Entry> entries)
        {
            MusicLibrary.Entry[] stored = library.Tracks;

            if (stored == null || stored.Length != entries.Count)
            {
                return false;
            }

            for (int i = 0; i < stored.Length; i++)
            {
                if (stored[i].Clip == null || stored[i].Name != entries[i].Name)
                {
                    return false;
                }
            }

            return true;
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                return;
            }

            string parent = Path.GetDirectoryName(path)?.Replace('\\', '/');
            string leaf = Path.GetFileName(path);

            if (!string.IsNullOrEmpty(parent) && !string.IsNullOrEmpty(leaf))
            {
                AssetDatabase.CreateFolder(parent, leaf);
            }
        }
    }
}
