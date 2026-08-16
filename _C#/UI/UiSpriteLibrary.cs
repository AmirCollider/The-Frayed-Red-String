// -----------------------------------------------------------------------------
//  The Frayed Red String
//  UiSpriteLibrary.cs
// -----------------------------------------------------------------------------

using UnityEngine;

namespace TheFrayedRedString.UI
{
    /// <summary>
    /// Sprites the runtime needs to swap at play time but cannot reach through
    /// a scene reference.
    /// </summary>
    /// <remarks>
    /// The language button only ever holds one flag at a time, so the other
    /// flag exists nowhere in the loaded scene. This asset carries both.
    /// It is generated and kept up to date by <c>UiSpriteLibraryBuilder</c> in
    /// the Editor assembly, so no manual wiring is required.
    /// </remarks>
    [CreateAssetMenu(
        fileName = ResourceName,
        menuName = "The Frayed Red String/UI Sprite Library",
        order = 0)]
    public sealed class UiSpriteLibrary : ScriptableObject
    {
        /// <summary>File name of the generated asset.</summary>
        public const string ResourceName = "UiSpriteLibrary";

        /// <summary>Path passed to <see cref="Resources.Load"/>.</summary>
        public const string ResourcePath = "TFRS/" + ResourceName;

        private static UiSpriteLibrary _cached;
        private static bool _lookupAttempted;

        [Header("Language flags")]
        [SerializeField] private Sprite _englishFlag;
        [SerializeField] private Sprite _japaneseFlag;

        /// <summary>Flag shown while the game is in English.</summary>
        public Sprite EnglishFlag => _englishFlag;

        /// <summary>Flag shown while the game is in Japanese.</summary>
        public Sprite JapaneseFlag => _japaneseFlag;

        /// <summary>
        /// Loads the generated asset, or returns <c>null</c> when it is missing.
        /// A missing library is not fatal: the language toggle still switches
        /// language, it just cannot swap the flag artwork.
        /// </summary>
        public static UiSpriteLibrary Load()
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
            _cached = Resources.Load<UiSpriteLibrary>(ResourcePath);

            if (_cached == null)
            {
                Debug.LogWarning(
                    $"[UI] No sprite library found at Resources/{ResourcePath}. " +
                    "Language switching will still work, but the flag image will not change. " +
                    "Re-import the project in the Editor to have it generated.");
            }

            return _cached;
        }

        /// <summary>Clears the cached lookup between play sessions.</summary>
        public static void ResetStatics()
        {
            _cached = null;
            _lookupAttempted = false;
        }

        /// <summary>Assigns the flags. Used by the Editor-side generator.</summary>
        public void SetFlags(Sprite english, Sprite japanese)
        {
            _englishFlag = english;
            _japaneseFlag = japanese;
        }

        /// <summary>True when both flags are present.</summary>
        public bool HasFlags => _englishFlag != null && _japaneseFlag != null;
    }
}
