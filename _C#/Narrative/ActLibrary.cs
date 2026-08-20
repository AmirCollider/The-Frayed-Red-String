// -----------------------------------------------------------------------------
//  The Frayed Red String
//  ActLibrary.cs
// -----------------------------------------------------------------------------

using System;
using UnityEngine;

namespace TheFrayedRedString.Narrative
{
    /// <summary>
    /// Every act in the game, reachable by number at runtime.
    /// </summary>
    /// <remarks>
    /// A scene knows which act it is; a save slot knows which act it was taken
    /// in. Neither has a reference to an <see cref="ActAsset"/>, and neither
    /// should need one — so the acts are collected here and looked up by the
    /// number they already have. Generated and kept in step by
    /// <c>ActLibraryBuilder</c>, which simply scans the project for acts.
    /// </remarks>
    [CreateAssetMenu(
        fileName = ResourceName,
        menuName = "The Frayed Red String/Act Library",
        order = 11)]
    public sealed class ActLibrary : ScriptableObject
    {
        /// <summary>File name of the generated asset.</summary>
        public const string ResourceName = "ActLibrary";

        /// <summary>Path passed to <see cref="Resources.Load"/>.</summary>
        public const string ResourcePath = "TFRS/" + ResourceName;

        private static ActLibrary _cached;
        private static bool _lookupAttempted;

        [Tooltip("Acts, in any order. Looked up by their own ActNumber.")]
        [SerializeField] private ActAsset[] _acts = Array.Empty<ActAsset>();

        /// <summary>Every act in the library.</summary>
        public ActAsset[] Acts => _acts;

        /// <summary>Loads the library, or returns <c>null</c> when it is missing.</summary>
        public static ActLibrary Load()
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
            _cached = Resources.Load<ActLibrary>(ResourcePath);

            if (_cached == null)
            {
                Debug.LogWarning(
                    $"[Story] No act library at Resources/{ResourcePath}. " +
                    "Acts cannot be found by number, so scenes will have no script. " +
                    "Re-import the project, or run The Frayed Red String > Rebuild Act Library.");
            }

            return _cached;
        }

        /// <summary>Clears the cached lookup between play sessions.</summary>
        public static void ResetStatics()
        {
            _cached = null;
            _lookupAttempted = false;
        }

        /// <summary>The act with this number, or <c>null</c>.</summary>
        public static ActAsset Find(int actNumber)
        {
            ActLibrary library = Load();
            if (library == null || library._acts == null)
            {
                return null;
            }

            for (int i = 0; i < library._acts.Length; i++)
            {
                if (library._acts[i] != null && library._acts[i].ActNumber == actNumber)
                {
                    return library._acts[i];
                }
            }

            return null;
        }

        /// <summary>
        /// The act with this asset name, or <c>null</c>.
        /// </summary>
        /// <remarks>
        /// By name rather than by number because the two endings have no number
        /// — they are not part of the run of seven, they are what replaces the
        /// rest of it — and an act with no number is exactly what the library
        /// already collects and sorts to the back.
        /// </remarks>
        public static ActAsset FindByName(string assetName)
        {
            ActLibrary library = Load();

            if (library == null || library._acts == null || string.IsNullOrEmpty(assetName))
            {
                return null;
            }

            for (int i = 0; i < library._acts.Length; i++)
            {
                if (library._acts[i] != null
                    && string.Equals(library._acts[i].name, assetName, StringComparison.OrdinalIgnoreCase))
                {
                    return library._acts[i];
                }
            }

            return null;
        }

        /// <summary>
        /// The title of an act, in the player's language, for a save slot.
        /// </summary>
        /// <remarks>
        /// Falls back to a plain number rather than to an empty card: a save
        /// from an act whose asset has been renamed or removed is still a save,
        /// and the slot has to say something.
        /// </remarks>
        public static string TitleOf(int actNumber)
        {
            ActAsset act = Find(actNumber);

            if (act != null && !act.Title.IsEmpty)
            {
                return act.Title.Current();
            }

            return $"Act {actNumber:00}";
        }

        /// <summary>Replaces the act table. Used by the Editor-side generator.</summary>
        public void SetActs(ActAsset[] acts)
        {
            _acts = acts ?? Array.Empty<ActAsset>();
        }
    }
}
