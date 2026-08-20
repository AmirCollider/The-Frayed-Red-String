// -----------------------------------------------------------------------------
//  The Frayed Red String
//  LocalizationService.cs
// -----------------------------------------------------------------------------

using System;
using TheFrayedRedString.Core;
using UnityEngine;

namespace TheFrayedRedString.Localization
{
    /// <summary>
    /// Holds the active language, persists the player's choice, and notifies
    /// every bound label when it changes.
    /// </summary>
    public static class LocalizationService
    {
        private static bool _initialized;
        private static GameLanguage _current = GameLanguage.English;

        /// <summary>
        /// Raised whenever the language changes. <see cref="LocalizedText"/>
        /// subscribes to this, which is what makes the toggle button update
        /// every label on screen in one step.
        /// </summary>
        public static event Action<GameLanguage> LanguageChanged;

        /// <summary>The language currently displayed.</summary>
        public static GameLanguage Current => _current;

        /// <summary>Loads the persisted language. Safe to call repeatedly.</summary>
        public static void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;

            int stored = PlayerPrefs.GetInt(GameConfig.PrefsLanguage, (int)GameLanguage.English);
            _current = Enum.IsDefined(typeof(GameLanguage), stored)
                ? (GameLanguage)stored
                : GameLanguage.English;
        }

        /// <summary>Clears static state between play sessions.</summary>
        public static void ResetStatics()
        {
            _initialized = false;
            _current = GameLanguage.English;
            LanguageChanged = null;
        }

        /// <summary>
        /// Switches language, persists the choice and refreshes every listener.
        /// Setting the language that is already active is a no-op.
        /// </summary>
        public static void SetLanguage(GameLanguage language)
        {
            Initialize();

            if (_current == language)
            {
                return;
            }

            _current = language;
            PlayerPrefs.SetInt(GameConfig.PrefsLanguage, (int)_current);
            PlayerPrefs.Save();

            LanguageChanged?.Invoke(_current);
        }

        /// <summary>Flips between English and Japanese.</summary>
        public static GameLanguage ToggleLanguage()
        {
            SetLanguage(_current.Next());
            return _current;
        }

        /// <summary>Resolves a key in the active language.</summary>
        public static string Get(string key)
        {
            Initialize();
            return LocalizationDatabase.Get(key, _current);
        }

        /// <summary>Resolves a key in an explicitly chosen language.</summary>
        public static string Get(string key, GameLanguage language)
        {
            return LocalizationDatabase.Get(key, language);
        }

        /// <summary>
        /// Resolves a format-string key and fills in its arguments. Falls back to
        /// the raw template if the arguments do not match the format, so a bad
        /// translation can never throw during a scene load.
        /// </summary>
        public static string Format(string key, params object[] args)
        {
            string template = Get(key);

            if (args == null || args.Length == 0)
            {
                return template;
            }

            try
            {
                return string.Format(template, args);
            }
            catch (FormatException)
            {
                Debug.LogWarning($"[Localization] Key '{key}' has a malformed format string: {template}");
                return template;
            }
        }
    }
}
