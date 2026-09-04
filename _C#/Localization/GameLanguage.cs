// -----------------------------------------------------------------------------
//  The Frayed Red String
//  GameLanguage.cs
// -----------------------------------------------------------------------------

namespace TheFrayedRedString.Localization
{
    /// <summary>Languages the game ships in.</summary>
    /// <remarks>
    /// The numeric values are persisted in PlayerPrefs, so they must never be
    /// reordered — a new language is appended, never inserted.
    /// </remarks>
    public enum GameLanguage
    {
        English = 0,
        Japanese = 1,
        Persian = 2
    }

    /// <summary>Helpers for cycling and describing languages.</summary>
    public static class GameLanguageExtensions
    {
        /// <summary>
        /// Returns the language the toggle button would switch to.
        /// </summary>
        /// <remarks>
        /// The cycle is English → Japanese → Persian → English. The button shows
        /// one flag at a time, so a cycle is the only affordance that reaches
        /// every language without a dropdown.
        /// </remarks>
        public static GameLanguage Next(this GameLanguage language)
        {
            switch (language)
            {
                case GameLanguage.English: return GameLanguage.Japanese;
                case GameLanguage.Japanese: return GameLanguage.Persian;
                default: return GameLanguage.English;
            }
        }

        /// <summary>Short display code, e.g. for debug overlays.</summary>
        public static string ToCode(this GameLanguage language)
        {
            switch (language)
            {
                case GameLanguage.Japanese: return "JA";
                case GameLanguage.Persian: return "FA";
                default: return "EN";
            }
        }

        /// <summary>
        /// True when this language is written right to left.
        /// </summary>
        /// <remarks>
        /// Everything that lays text out — alignment, the typewriter's reveal
        /// direction, the shaping pass — branches on this rather than on the
        /// language itself, so adding Arabic or Hebrew later touches nothing
        /// outside this file.
        /// </remarks>
        public static bool IsRightToLeft(this GameLanguage language)
        {
            return language == GameLanguage.Persian;
        }

        /// <summary>
        /// True when this language needs Arabic-script shaping before it can be
        /// handed to TextMeshPro.
        /// </summary>
        public static bool NeedsArabicShaping(this GameLanguage language)
        {
            return language == GameLanguage.Persian;
        }
    }
}
