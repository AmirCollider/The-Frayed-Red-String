// -----------------------------------------------------------------------------
//  The Frayed Red String
//  UiSpriteLibrary.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Localization;
using UnityEngine;

namespace TheFrayedRedString.UI
{
    /// <summary>
    /// Sprites the runtime needs to swap at play time but cannot reach through
    /// a scene reference.
    /// </summary>
    /// <remarks>
    /// The language button only ever holds one flag at a time, so the other
    /// flags exist nowhere in the loaded scene. This asset carries all of them.
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
        [SerializeField] private Sprite _persianFlag;

        [Header("Choice buttons")]
        [Tooltip("Kind / friendly option. Blue is the colour the story assigns to it.")]
        [SerializeField] private Sprite _choiceBlue;

        [Tooltip("Cruel / controlling option.")]
        [SerializeField] private Sprite _choiceGreen;

        [Tooltip("Neutral option, used for choices that carry no moral weight.")]
        [SerializeField] private Sprite _choiceWhite;

        [Header("Pause menu")]
        [SerializeField] private Sprite _pauseResume;
        [SerializeField] private Sprite _pauseSave;
        [SerializeField] private Sprite _pauseLoad;
        [SerializeField] private Sprite _pauseMenu;
        [SerializeField] private Sprite _backToMenu;

        /// <summary>Flag shown while the game is in English.</summary>
        public Sprite EnglishFlag => _englishFlag;

        /// <summary>Flag shown while the game is in Japanese.</summary>
        public Sprite JapaneseFlag => _japaneseFlag;

        /// <summary>Flag shown while the game is in Persian.</summary>
        public Sprite PersianFlag => _persianFlag;

        /// <summary>Background of a kind, friendly choice.</summary>
        public Sprite ChoiceBlue => _choiceBlue;

        /// <summary>Background of a cruel, controlling choice.</summary>
        public Sprite ChoiceGreen => _choiceGreen;

        /// <summary>Background of a choice that scores nothing either way.</summary>
        public Sprite ChoiceWhite => _choiceWhite;

        /// <summary>Pause-menu button art.</summary>
        public Sprite PauseResume => _pauseResume;

        /// <inheritdoc cref="PauseResume"/>
        public Sprite PauseSave => _pauseSave;

        /// <inheritdoc cref="PauseResume"/>
        public Sprite PauseLoad => _pauseLoad;

        /// <inheritdoc cref="PauseResume"/>
        public Sprite PauseMenu => _pauseMenu;

        /// <inheritdoc cref="PauseResume"/>
        public Sprite BackToMenu => _backToMenu;

        /// <summary>
        /// Loads the generated asset, or returns <c>null</c> when it is missing.
        /// A missing library is not fatal: the language toggle still switches
        /// language, it just cannot swap the flag artwork, and the story UI
        /// falls back to sprites it draws itself.
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

        /// <summary>The flag for one language, or <c>null</c> when it is missing.</summary>
        public Sprite FlagFor(GameLanguage language)
        {
            switch (language)
            {
                case GameLanguage.Japanese: return _japaneseFlag;
                case GameLanguage.Persian: return _persianFlag;
                default: return _englishFlag;
            }
        }

        /// <summary>Assigns the flags. Used by the Editor-side generator.</summary>
        public void SetFlags(Sprite english, Sprite japanese, Sprite persian)
        {
            _englishFlag = english;
            _japaneseFlag = japanese;
            _persianFlag = persian;
        }

        /// <summary>Assigns the choice-button art. Used by the Editor-side generator.</summary>
        public void SetChoiceBackgrounds(Sprite blue, Sprite green, Sprite white)
        {
            _choiceBlue = blue;
            _choiceGreen = green;
            _choiceWhite = white;
        }

        /// <summary>Assigns the pause-menu art. Used by the Editor-side generator.</summary>
        public void SetPauseButtons(Sprite resume, Sprite save, Sprite load, Sprite menu, Sprite backToMenu)
        {
            _pauseResume = resume;
            _pauseSave = save;
            _pauseLoad = load;
            _pauseMenu = menu;
            _backToMenu = backToMenu;
        }

        /// <summary>True when a flag exists for every language the game ships.</summary>
        public bool HasFlags => _englishFlag != null && _japaneseFlag != null && _persianFlag != null;
    }
}
