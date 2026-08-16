// -----------------------------------------------------------------------------
//  The Frayed Red String
//  LocalizedText.cs
// -----------------------------------------------------------------------------

using TMPro;
using UnityEngine;

namespace TheFrayedRedString.Localization
{
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

        [Tooltip("When set, this label ignores the active language and always renders in the chosen one.")]
        [SerializeField] private bool _useFixedLanguage;

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
            _useFixedLanguage = false;
            _formatArgs = formatArgs != null && formatArgs.Length > 0 ? formatArgs : null;
            Refresh();
        }

        /// <summary>
        /// Binds this label to a key in a language that never changes. The two
        /// warning labels use this: WarningTextTMPEN is pinned to English and
        /// WarningTextTMPJP to Japanese, and both stay visible at once.
        /// </summary>
        public void BindFixed(string key, GameLanguage language, params object[] formatArgs)
        {
            _key = key;
            _useFixedLanguage = true;
            _fixedLanguage = language;
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

            GameLanguage language = _useFixedLanguage ? _fixedLanguage : LocalizationService.Current;
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

            _label.text = value;
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
            if (!_useFixedLanguage)
            {
                Refresh();
            }
        }
    }
}
