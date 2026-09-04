// -----------------------------------------------------------------------------
//  The Frayed Red String
//  VoiceLibrary.cs
// -----------------------------------------------------------------------------

using System;
using UnityEngine;

namespace TheFrayedRedString.Audio
{
    /// <summary>
    /// Recorded lines, reachable by file name at runtime.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The same arrangement as <see cref="MusicLibrary"/> and for the same
    /// reason: a script asks for a clip by name and cannot hold a reference to
    /// one, because the script is an asset and the clip may not exist yet.
    /// </para>
    /// <para>
    /// Generated and kept in sync by <c>VoiceLibraryBuilder</c> in the Editor
    /// assembly, which scans <c>Assets/Audio/Voice</c>. Dropping a .wav or .mp3
    /// in that folder and naming it after the line is the entire workflow — no
    /// act has to be rebuilt and no code has to change.
    /// </para>
    /// <para>
    /// A project with no recordings at all is the normal case for most of
    /// development, and is not an error. Every voiced line simply plays with the
    /// typewriter voice it already had.
    /// </para>
    /// </remarks>
    [CreateAssetMenu(
        fileName = ResourceName,
        menuName = "The Frayed Red String/Voice Library",
        order = 2)]
    public sealed class VoiceLibrary : ScriptableObject
    {
        /// <summary>File name of the generated asset.</summary>
        public const string ResourceName = "VoiceLibrary";

        /// <summary>Path passed to <see cref="Resources.Load"/>.</summary>
        public const string ResourcePath = "TFRS/" + ResourceName;

        /// <summary>One named recording.</summary>
        [Serializable]
        public struct Entry
        {
            [Tooltip("Asset file name, without extension and with whitespace trimmed.")]
            public string Name;

            public AudioClip Clip;
        }

        private static VoiceLibrary _cached;
        private static bool _lookupAttempted;

        [SerializeField] private Entry[] _lines = Array.Empty<Entry>();

        /// <summary>Every recording in the library.</summary>
        public Entry[] Lines => _lines;

        /// <summary>
        /// Loads the generated asset, or returns <c>null</c> when there is none.
        /// </summary>
        /// <remarks>
        /// Silent about a missing library, unlike the music and sprite ones. A
        /// game with no music is a game somebody forgot to build; a game with no
        /// recordings is this game, today, and warning about it on every boot
        /// would train everybody to ignore the console.
        /// </remarks>
        public static VoiceLibrary Load()
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
            _cached = Resources.Load<VoiceLibrary>(ResourcePath);

            return _cached;
        }

        /// <summary>Clears the cached lookup between play sessions.</summary>
        public static void ResetStatics()
        {
            _cached = null;
            _lookupAttempted = false;
        }

        /// <summary>Finds a recording by file name. Returns <c>null</c> when absent.</summary>
        public AudioClip Find(string lineName)
        {
            if (string.IsNullOrEmpty(lineName) || _lines == null)
            {
                return null;
            }

            for (int i = 0; i < _lines.Length; i++)
            {
                if (string.Equals(_lines[i].Name, lineName, StringComparison.OrdinalIgnoreCase))
                {
                    return _lines[i].Clip;
                }
            }

            return null;
        }

        /// <summary>Replaces the table. Used by the Editor-side generator.</summary>
        public void SetLines(Entry[] lines)
        {
            _lines = lines ?? Array.Empty<Entry>();
        }
    }
}
