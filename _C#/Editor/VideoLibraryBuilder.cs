// -----------------------------------------------------------------------------
//  The Frayed Red String
//  VideoLibraryBuilder.cs  (Editor only)
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using TheFrayedRedString.Presentation;
using UnityEditor;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>
    /// Bakes the films in <c>Assets/Video</c> into a Resources asset.
    /// </summary>
    /// <remarks>
    /// Only runs when the Video module is enabled, because without it Unity does
    /// not import an .mp4 as anything this could collect. A film in
    /// <c>StreamingAssets/Video</c> needs none of this and is found at runtime by
    /// file name.
    /// </remarks>
    [InitializeOnLoad]
    public static class VideoLibraryBuilder
    {
        private const string VideoFolder = "Assets/Video";

        private const string ResourcesFolder = "Assets/Resources";
        private const string LibraryFolder = ResourcesFolder + "/TFRS";
        private const string LibraryPath = LibraryFolder + "/" + VideoLibrary.ResourceName + ".asset";

        static VideoLibraryBuilder()
        {
            EditorApplication.delayCall += Rebuild;
        }

        /// <summary>Creates or refreshes the video library.</summary>
        [MenuItem("The Frayed Red String/Rebuild Video Library")]
        public static void Rebuild()
        {
#if !TFRS_VIDEO
            // Nothing to collect and nothing that could play it.
            return;
#else
            if (!AssetDatabase.IsValidFolder(VideoFolder))
            {
                return;
            }

            VideoLibrary.Entry[] films = Collect();

            VideoLibrary library = AssetDatabase.LoadAssetAtPath<VideoLibrary>(LibraryPath);
            bool created = false;

            if (library == null)
            {
                if (films.Length == 0)
                {
                    return;
                }

                AssetPaths.EnsureFolder(ResourcesFolder);
                AssetPaths.EnsureFolder(LibraryFolder);

                library = ScriptableObject.CreateInstance<VideoLibrary>();
                AssetDatabase.CreateAsset(library, LibraryPath);
                created = true;
            }
            else if (IsUpToDate(library, films))
            {
                return;
            }

            library.SetFilms(films);

            EditorUtility.SetDirty(library);
            AssetDatabase.SaveAssets();

            Debug.Log($"[Video] {(created ? "Generated" : "Refreshed")} the video library with {films.Length} film(s).");
#endif
        }

#if TFRS_VIDEO
        private static VideoLibrary.Entry[] Collect()
        {
            List<VideoLibrary.Entry> entries = new List<VideoLibrary.Entry>();

            foreach (string guid in AssetDatabase.FindAssets("t:VideoClip", new[] { VideoFolder }))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string key = Path.GetFileNameWithoutExtension(path)?.Trim();

                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }

                Object clip = AssetDatabase.LoadAssetAtPath<UnityEngine.Video.VideoClip>(path);

                if (clip != null)
                {
                    entries.Add(new VideoLibrary.Entry { Name = key, Clip = clip });
                }
            }

            entries.Sort((a, b) => string.CompareOrdinal(a.Name, b.Name));
            return entries.ToArray();
        }

        private static bool IsUpToDate(VideoLibrary library, VideoLibrary.Entry[] fresh)
        {
            VideoLibrary.Entry[] stored = library.Films;

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
#endif
    }
}
