// -----------------------------------------------------------------------------
//  The Frayed Red String
//  MusicLibrary.cs
// -----------------------------------------------------------------------------

using System;
using UnityEngine;

namespace TheFrayedRedString.Audio
{
    /// <summary>Names of the music tracks the game asks for by hand.</summary>
    public static class MusicTracks
    {
        /// <summary>
        /// The main menu loop.
        /// </summary>
        /// <remarks>
        /// Spelled exactly as the asset on disk — "BackGrung", not
        /// "BackGround". Lookups are by file name, so the typo is load-bearing;
        /// if the file is ever renamed, this constant has to follow it.
        /// </remarks>
        public const string MainMenu = "MainMenuBackGrungMusic";
    }

    /// <summary>
    /// Music clips baked into a Resources asset so the runtime can load them by
    /// name without a scene reference.
    /// </summary>
    /// <remarks>
    /// Generated and kept in sync by <c>AudioLibraryBuilder</c> in the Editor
    /// assembly, which scans <c>Assets/Audio/BackgroundMusics</c>. Dropping a
    /// new .mp3 in that folder is all it takes to make it loadable.
    /// </remarks>
    [CreateAssetMenu(
        fileName = ResourceName,
        menuName = "The Frayed Red String/Music Library",
        order = 1)]
    public sealed class MusicLibrary : ScriptableObject
    {
        /// <summary>File name of the generated asset.</summary>
        public const string ResourceName = "MusicLibrary";

        /// <summary>Path passed to <see cref="Resources.Load"/>.</summary>
        public const string ResourcePath = "TFRS/" + ResourceName;

        /// <summary>One named track.</summary>
        [Serializable]
        public struct Entry
        {
            [Tooltip("Asset file name, without extension.")]
            public string Name;

            public AudioClip Clip;
        }

        private static MusicLibrary _cached;
        private static bool _lookupAttempted;

        [SerializeField] private Entry[] _tracks = Array.Empty<Entry>();

        /// <summary>Every track in the library.</summary>
        public Entry[] Tracks => _tracks;

        /// <summary>
        /// Loads the generated asset, or returns <c>null</c> when it is missing.
        /// A missing library is not fatal — the game simply plays no music.
        /// </summary>
        public static MusicLibrary Load()
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
            _cached = Resources.Load<MusicLibrary>(ResourcePath);

            if (_cached == null)
            {
                Debug.LogWarning(
                    $"[Audio] No music library found at Resources/{ResourcePath}. " +
                    "Background music will not play. Re-import the project in the Editor to have it generated, " +
                    "or run The Frayed Red String > Rebuild Music Library.");
            }

            return _cached;
        }

        /// <summary>Clears the cached lookup between play sessions.</summary>
        public static void ResetStatics()
        {
            _cached = null;
            _lookupAttempted = false;
        }

        /// <summary>Finds a track by file name. Returns <c>null</c> when absent.</summary>
        public AudioClip Find(string trackName)
        {
            if (string.IsNullOrEmpty(trackName) || _tracks == null)
            {
                return null;
            }

            for (int i = 0; i < _tracks.Length; i++)
            {
                if (string.Equals(_tracks[i].Name, trackName, StringComparison.OrdinalIgnoreCase))
                {
                    return _tracks[i].Clip;
                }
            }

            return null;
        }

        /// <summary>Replaces the track table. Used by the Editor-side generator.</summary>
        public void SetTracks(Entry[] tracks)
        {
            _tracks = tracks ?? Array.Empty<Entry>();
        }
    }
}
