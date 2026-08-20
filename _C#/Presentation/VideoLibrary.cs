// -----------------------------------------------------------------------------
//  The Frayed Red String
//  VideoLibrary.cs
// -----------------------------------------------------------------------------

using System;
using UnityEngine;

namespace TheFrayedRedString.Presentation
{
    /// <summary>
    /// Films the story can ask for by name.
    /// </summary>
    /// <remarks>
    /// The same shape as the music and voice libraries. Entirely optional: a
    /// film can also be a file in <c>StreamingAssets/Video</c>, which
    /// <see cref="VideoScreenView"/> falls back to, and for a long credits
    /// sequence that is usually the better place for it.
    /// </remarks>
    [CreateAssetMenu(
        fileName = ResourceName,
        menuName = "The Frayed Red String/Video Library",
        order = 4)]
    public sealed class VideoLibrary : ScriptableObject
    {
        /// <summary>File name of the generated asset.</summary>
        public const string ResourceName = "VideoLibrary";

        /// <summary>Path passed to <see cref="Resources.Load"/>.</summary>
        public const string ResourcePath = "TFRS/" + ResourceName;

        /// <summary>One named film.</summary>
        /// <remarks>
        /// The clip is held as a plain <see cref="UnityEngine.Object"/> so this
        /// file compiles in a project without the Video module. Everything that
        /// actually plays it is behind <c>TFRS_VIDEO</c>; this only has to be
        /// able to store and find it.
        /// </remarks>
        [Serializable]
        public struct Entry
        {
            [Tooltip("Asset file name, without extension.")]
            public string Name;

            [Tooltip("The VideoClip itself.")]
            public UnityEngine.Object Clip;
        }

        private static VideoLibrary _cached;
        private static bool _lookupAttempted;

        [SerializeField] private Entry[] _films = Array.Empty<Entry>();

        /// <summary>Every film in the library.</summary>
        public Entry[] Films => _films;

        /// <summary>Loads the generated asset, or returns <c>null</c>.</summary>
        /// <remarks>
        /// Silent when missing. A project with no films is this one, and will be
        /// until the credits are cut.
        /// </remarks>
        public static VideoLibrary Load()
        {
            if (_cached != null)
            {
                return _cached;
            }

            if (_lookupAttempted)
            {
                return null;
            }

            _lookupAttempted = true;
            _cached = Resources.Load<VideoLibrary>(ResourcePath);

            return _cached;
        }

        /// <summary>Clears the cached lookup between play sessions.</summary>
        public static void ResetStatics()
        {
            _cached = null;
            _lookupAttempted = false;
        }

#if TFRS_VIDEO
        /// <summary>The clip of that name, or <c>null</c>.</summary>
        public static UnityEngine.Video.VideoClip Find(string filmName)
        {
            VideoLibrary library = Load();

            if (library == null || string.IsNullOrEmpty(filmName) || library._films == null)
            {
                return null;
            }

            for (int i = 0; i < library._films.Length; i++)
            {
                if (string.Equals(library._films[i].Name, filmName, StringComparison.OrdinalIgnoreCase))
                {
                    return library._films[i].Clip as UnityEngine.Video.VideoClip;
                }
            }

            return null;
        }
#endif

        /// <summary>Replaces the table. Used by the Editor-side generator.</summary>
        public void SetFilms(Entry[] films)
        {
            _films = films ?? Array.Empty<Entry>();
        }
    }
}
