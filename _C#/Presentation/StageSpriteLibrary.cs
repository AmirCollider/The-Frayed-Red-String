// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StageSpriteLibrary.cs
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using TheFrayedRedString.Narrative;
using UnityEngine;

namespace TheFrayedRedString.Presentation
{
    /// <summary>
    /// Every background and character sprite the story can ask for, reachable
    /// by name at runtime.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A visual novel picks its art from a script, not from a scene reference:
    /// act one alone moves through nine locations and sixteen character poses,
    /// none of which exist in Act01.unity. Dragging them all into a component
    /// would make the scene file enormous and the script un-reorderable.
    /// </para>
    /// <para>
    /// Generated and kept in sync by <c>StageSpriteLibraryBuilder</c> in the
    /// Editor assembly, which scans <c>Assets/Images/Backgrounds</c> and
    /// <c>Assets/Images/Characters</c>. Dropping new art into those folders is
    /// the entire workflow for making it usable from a script.
    /// </para>
    /// </remarks>
    [CreateAssetMenu(
        fileName = ResourceName,
        menuName = "The Frayed Red String/Stage Sprite Library",
        order = 3)]
    public sealed class StageSpriteLibrary : ScriptableObject
    {
        /// <summary>File name of the generated asset.</summary>
        public const string ResourceName = "StageSpriteLibrary";

        /// <summary>Path passed to <see cref="Resources.Load"/>.</summary>
        public const string ResourcePath = "TFRS/" + ResourceName;

        /// <summary>One named sprite.</summary>
        [Serializable]
        public struct Entry
        {
            [Tooltip("Asset file name, without extension and with whitespace trimmed.")]
            public string Name;

            public Sprite Sprite;
        }

        private static StageSpriteLibrary _cached;
        private static bool _lookupAttempted;

        [SerializeField] private Entry[] _backgrounds = Array.Empty<Entry>();
        [SerializeField] private Entry[] _characters = Array.Empty<Entry>();

        /// <summary>Every background in the library.</summary>
        public Entry[] BackgroundEntries => _backgrounds;

        /// <summary>Every character pose in the library.</summary>
        public Entry[] CharacterEntries => _characters;

        /// <summary>
        /// Loads the generated asset, or returns <c>null</c> when it is missing.
        /// A missing library is not fatal: the act still plays, against an empty
        /// stage.
        /// </summary>
        public static StageSpriteLibrary Load()
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
            _cached = Resources.Load<StageSpriteLibrary>(ResourcePath);

            if (_cached == null)
            {
                Debug.LogWarning(
                    $"[Stage] No stage sprite library found at Resources/{ResourcePath}. " +
                    "Backgrounds and characters will not appear. Re-import the project in the Editor, " +
                    "or run The Frayed Red String > Rebuild Stage Sprite Library.");
            }

            return _cached;
        }

        /// <summary>Clears the cached lookup between play sessions.</summary>
        public static void ResetStatics()
        {
            _cached = null;
            _lookupAttempted = false;
            Reported.Clear();
        }

        /// <summary>Finds a background by file name. Returns <c>null</c> when absent.</summary>
        public Sprite FindBackground(string backgroundName)
        {
            return Find(_backgrounds, backgroundName);
        }

        /// <summary>Finds a character pose. Returns <c>null</c> when absent.</summary>
        public Sprite FindCharacter(Speaker speaker, Portrait portrait)
        {
            string spriteName = CharacterArt.SpriteName(speaker, portrait);
            return spriteName == null ? null : Find(_characters, spriteName);
        }

        /// <summary>Finds a character pose by raw sprite name.</summary>
        public Sprite FindCharacter(string spriteName)
        {
            return Find(_characters, spriteName);
        }

        /// <summary>Replaces both tables. Used by the Editor-side generator.</summary>
        public void SetEntries(Entry[] backgrounds, Entry[] characters)
        {
            _backgrounds = backgrounds ?? Array.Empty<Entry>();
            _characters = characters ?? Array.Empty<Entry>();
        }

        /// <summary>
        /// Names already reported as missing, so the warning is said once.
        /// </summary>
        /// <remarks>
        /// The Story Editor's preview asks for sprites on every repaint. Without
        /// this, one missing portrait would write thousands of identical lines
        /// to the console while the window merely sits open.
        /// </remarks>
        private static readonly HashSet<string> Reported = new HashSet<string>();

        /// <summary>
        /// Linear search, on purpose.
        /// </summary>
        /// <remarks>
        /// The two tables hold roughly fifteen entries each and are read once
        /// per scene change or portrait swap — a handful of times a minute at
        /// the very most. A dictionary would cost a serialisation callback to
        /// build and would save nothing measurable.
        /// </remarks>
        private static Sprite Find(Entry[] entries, string name)
        {
            if (string.IsNullOrEmpty(name) || entries == null)
            {
                return null;
            }

            for (int i = 0; i < entries.Length; i++)
            {
                if (string.Equals(entries[i].Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return entries[i].Sprite;
                }
            }

            if (Reported.Add(name))
            {
                Debug.LogWarning($"[Stage] No sprite named '{name}' is in the stage library.");
            }

            return null;
        }
    }
}
