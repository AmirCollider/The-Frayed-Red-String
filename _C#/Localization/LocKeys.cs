// -----------------------------------------------------------------------------
//  The Frayed Red String
//  LocKeys.cs
// -----------------------------------------------------------------------------

namespace TheFrayedRedString.Localization
{
    /// <summary>
    /// Every interface localisation key in the game. Referencing constants
    /// instead of raw strings turns a typo into a compile error and makes
    /// "who uses this line?" answerable from the IDE.
    /// </summary>
    /// <remarks>
    /// Dialogue keys are not here. A script runs to hundreds of lines per act
    /// and lives beside its text in <c>Act01Strings</c> and its successors, so
    /// that reading an act means opening one file rather than cross-referencing
    /// two.
    /// </remarks>
    public static class LocKeys
    {
        // --- Warning scene ---------------------------------------------------

        public const string WarningBody = "warning.body";
        public const string WarningEnterPrompt = "warning.enterPrompt";

        // --- Main menu -------------------------------------------------------

        public const string MenuStart = "menu.start";
        public const string MenuLoad = "menu.load";
        public const string MenuGallery = "menu.gallery";
        public const string MenuSettings = "menu.settings";
        public const string MenuExit = "menu.exit";
        public const string MenuBack = "menu.back";

        // --- Load / save panel -----------------------------------------------

        public const string LoadPanelTitle = "load.title";

        /// <summary>Title shown when the same panel is opened to write a save.</summary>
        public const string SavePanelTitle = "save.title";

        public const string LoadSlotEmpty = "load.slot.empty";

        /// <summary>Format string with one argument: the season number.</summary>
        public const string LoadSlotSeason = "load.slot.season";

        /// <summary>Format string with one argument: the formatted play time.</summary>
        public const string LoadSlotPlayTime = "load.slot.playTime";

        /// <summary>Confirmation shown briefly after a slot is written.</summary>
        public const string SaveWritten = "save.written";

        // --- Pause menu ------------------------------------------------------

        public const string PauseTitle = "pause.title";
        public const string PauseResume = "pause.resume";
        public const string PauseSave = "pause.save";
        public const string PauseLoad = "pause.load";
        public const string PauseQuitToMenu = "pause.quit";

        // --- Speaker names ---------------------------------------------------

        public const string SpeakerYua = "speaker.yua";
        public const string SpeakerHaru = "speaker.haru";

        // --- Act / season names ----------------------------------------------

        public const string ActOne = "act.01";
        public const string ActTwo = "act.02";
        public const string ActThree = "act.03";
        public const string ActFour = "act.04";
        public const string ActFive = "act.05";
        public const string ActSix = "act.06";
        public const string ActSeven = "act.07";

        /// <summary>
        /// Act names in narrative order, so a save slot can name its act from an
        /// index without a switch statement.
        /// </summary>
        public static readonly string[] ActNames =
        {
            ActOne,
            ActTwo,
            ActThree,
            ActFour,
            ActFive,
            ActSix,
            ActSeven
        };
    }
}
