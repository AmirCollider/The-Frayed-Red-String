// -----------------------------------------------------------------------------
//  The Frayed Red String
//  GameLanguage.cs
// -----------------------------------------------------------------------------

namespace TheFrayedRedString.Localization
{
    /// <summary>Languages the game ships in.</summary>
    public enum GameLanguage
    {
        English = 0,
        Japanese = 1
    }

    /// <summary>Helpers for cycling and describing languages.</summary>
    public static class GameLanguageExtensions
    {
        /// <summary>Returns the language the toggle button would switch to.</summary>
        public static GameLanguage Next(this GameLanguage language)
        {
            return language == GameLanguage.English
                ? GameLanguage.Japanese
                : GameLanguage.English;
        }

        /// <summary>Short display code, e.g. for debug overlays.</summary>
        public static string ToCode(this GameLanguage language)
        {
            return language == GameLanguage.Japanese ? "JA" : "EN";
        }
    }
}
