// -----------------------------------------------------------------------------
//  The Frayed Red String
//  LocalizedText.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Presentation;
using TMPro;
using UnityEngine;

namespace TheFrayedRedString.Localization
{
    /// <summary>Which language a <see cref="LocalizedText"/> follows.</summary>
    public enum LocalizedTextMode
    {
        /// <summary>Follows whatever the player has selected.</summary>
        Active,

        /// <summary>Always renders one language, whatever is selected.</summary>
        Fixed,

        /// <summary>
        /// Renders the active language unless that language is English, in which
        /// case it falls back to Japanese.
        /// </summary>
        /// <remarks>
        /// This exists for the content-warning screen, which prints the warning
        /// twice so that it is legible to a player who cannot read the language
        /// the menu happens to be set to. The first label is pinned to English;
        /// this mode drives the second one, which shows Japanese normally and
        /// Persian once the player has switched to Persian — so the pairing is
        /// always English plus something else, never English twice.
        /// </remarks>
        Secondary
    }

    /// <summary>
    /// Drives one <see cref="TMP_Text"/> from the localisation table.
    /// </summary>
    /// <remarks>
    /// Text is applied in <c>Start</c> rather than <c>Awake</c>. The scenes use
    /// the DirectTMP package's <c>DirectFont</c> component to bind fonts at
    /// runtime; running a frame later guarantees this component has the last
    /// word on the string, whatever the font binder does on its own Awake.
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class LocalizedText : MonoBehaviour, ILocalizedView
    {
        [Header("Binding")]
        [Tooltip("Key from LocKeys. Missing keys render as #key# so gaps are visible.")]
        [SerializeField] private string _key;

        [Tooltip("Which language this label follows.")]
        [SerializeField] private LocalizedTextMode _mode = LocalizedTextMode.Active;

        [Tooltip("The language used when the mode is Fixed.")]
        [SerializeField] private GameLanguage _fixedLanguage = GameLanguage.English;

        private TMP_Text _label;
        private object[] _formatArgs;

        /// <summary>The key this label renders.</summary>
        public string Key => _key;

        /// <summary>
        /// Binds this label to a key that follows the active language.
        /// </summary>
        public void Bind(string key, params object[] formatArgs)
        {
            _key = key;
            _mode = LocalizedTextMode.Active;
            _formatArgs = formatArgs != null && formatArgs.Length > 0 ? formatArgs : null;
            Refresh();
        }

        /// <summary>
        /// Binds this label to a key in a language that never changes. The
        /// English half of the content warning uses this.
        /// </summary>
        public void BindFixed(string key, GameLanguage language, params object[] formatArgs)
        {
            _key = key;
            _mode = LocalizedTextMode.Fixed;
            _fixedLanguage = language;
            _formatArgs = formatArgs != null && formatArgs.Length > 0 ? formatArgs : null;
            Refresh();
        }

        /// <summary>
        /// Binds this label as the second of a bilingual pair; see
        /// <see cref="LocalizedTextMode.Secondary"/>.
        /// </summary>
        public void BindSecondary(string key, params object[] formatArgs)
        {
            _key = key;
            _mode = LocalizedTextMode.Secondary;
            _formatArgs = formatArgs != null && formatArgs.Length > 0 ? formatArgs : null;
            Refresh();
        }

        /// <summary>Re-applies the current translation to the label.</summary>
        public void Refresh()
        {
            if (_label == null)
            {
                _label = GetComponent<TMP_Text>();
            }

            if (_label == null || string.IsNullOrEmpty(_key))
            {
                return;
            }

            GameLanguage language = ResolveLanguage();
            string value = LocalizationDatabase.Get(_key, language);

            if (_formatArgs != null)
            {
                try
                {
                    value = string.Format(value, _formatArgs);
                }
                catch (System.FormatException)
                {
                    Debug.LogWarning($"[Localization] Key '{_key}' has a malformed format string.", this);
                }
            }

            // Handed over in reading order. The Direct Font on this label joins
            // and reorders it as needed — including choosing a per-language font
            // when one is assigned — so nothing here has to know which script it
            // just wrote.
            StoryText.Set(_label, value);
        }

        private GameLanguage ResolveLanguage()
        {
            switch (_mode)
            {
                case LocalizedTextMode.Fixed:
                    return _fixedLanguage;

                case LocalizedTextMode.Secondary:
                    return LocalizationService.Current == GameLanguage.English
                        ? GameLanguage.Japanese
                        : LocalizationService.Current;

                default:
                    return LocalizationService.Current;
            }
        }

        private void Awake()
        {
            _label = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            LocalizationService.LanguageChanged += OnLanguageChanged;
        }

        private void OnDisable()
        {
            LocalizationService.LanguageChanged -= OnLanguageChanged;
        }

        private void Start()
        {
            Refresh();
        }

        private void OnLanguageChanged(GameLanguage language)
        {
            // A pinned label is showing a fixed translation on purpose; a global
            // language switch must not disturb it.
            if (_mode != LocalizedTextMode.Fixed)
            {
                Refresh();
            }
        }
    }
}
