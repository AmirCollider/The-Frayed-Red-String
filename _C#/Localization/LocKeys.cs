// -----------------------------------------------------------------------------
//  The Frayed Red String
//  LocKeys.cs
// -----------------------------------------------------------------------------

namespace TheFrayedRedString.Localization
{
    /// <summary>
    /// Every localisation key in the game. Referencing constants instead of raw
    /// strings turns a typo into a compile error and makes "who uses this line?"
    /// answerable from the IDE.
    /// </summary>
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

        // --- Load panel ------------------------------------------------------

        public const string LoadPanelTitle = "load.title";
        public const string LoadSlotEmpty = "load.slot.empty";

        /// <summary>Format string with one argument: the season number.</summary>
        public const string LoadSlotSeason = "load.slot.season";

        /// <summary>Format string with one argument: the formatted play time.</summary>
        public const string LoadSlotPlayTime = "load.slot.playTime";

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
