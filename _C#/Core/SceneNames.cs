// -----------------------------------------------------------------------------
//  The Frayed Red String
//  SceneNames.cs
// -----------------------------------------------------------------------------

namespace TheFrayedRedString.Core
{
    /// <summary>
    /// Canonical scene names. Keeping them here means a scene rename only ever
    /// touches one file, and typos become compile errors instead of silent
    /// "scene could not be loaded" failures at runtime.
    /// </summary>
    public static class SceneNames
    {
        public const string Warning = "WarningScene";
        public const string MainMenu = "MainMenu";

        /// <summary>First story scene.</summary>
        public const string FirstAct = "Act01";

        /// <summary>
        /// The scene name for an act, by 1-based number.
        /// </summary>
        /// <remarks>
        /// Acts two through seven do not exist yet. Nothing has to change here
        /// when they do — adding <c>Act02.unity</c> and a script is enough,
        /// because everything that loads an act asks this and then checks
        /// whether the scene is actually in the build.
        /// </remarks>
        public static string ForAct(int actNumber)
        {
            return $"Act{actNumber:00}";
        }

        /// <summary>
        /// True when a scene name looks like an act.
        /// </summary>
        /// <remarks>
        /// Matched by shape rather than against a list, so the seven acts the
        /// story needs — and any the writer adds afterwards — are all picked up
        /// without this file being edited again.
        /// </remarks>
        public static bool IsActScene(string sceneName)
        {
            return !string.IsNullOrEmpty(sceneName)
                   && System.Text.RegularExpressions.Regex.IsMatch(sceneName, @"^Act\d+$");
        }
    }
}
