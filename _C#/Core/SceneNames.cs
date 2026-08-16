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

        /// <summary>
        /// First story scene. It does not exist yet; <see cref="Flow.GameFlowService"/>
        /// degrades gracefully instead of throwing when it is missing.
        /// </summary>
        public const string FirstAct = "Act01";
    }
}
