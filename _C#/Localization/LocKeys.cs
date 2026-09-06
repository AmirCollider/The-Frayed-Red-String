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

        // --- Story ------------------------------------------------------------

        /// <summary>
        /// Shown by an act scene whose act has not been written yet, instead of
        /// silently skipping through to the next one.
        /// </summary>
        public const string StoryToBeContinued = "story.toBeContinued";

        /// <summary>
        /// The option offered after five minutes of silence on a pure run.
        /// </summary>
        /// <remarks>
        /// The design document's secret ending, and its condition: never once
        /// having taken the controlling option, and then sitting with one of the
        /// game's two silences long enough for it to count. What appears is a
        /// single sentence offering to let Yua speak.
        /// </remarks>
        public const string EndingSpeakYua = "ending.speak.yua";

        /// <summary>The same offer on a run that took the green option at least once.</summary>
        public const string EndingSpeakHaru = "ending.speak.haru";

        /// <summary>Letting the moment pass, which carries on with the act.</summary>
        public const string EndingSayNothing = "ending.sayNothing";

        // --- Speaker names ---------------------------------------------------

        public const string SpeakerYua = "speaker.yua";
        public const string SpeakerHaru = "speaker.haru";

        /// <summary>
        /// The name plate the nine-year-olds wear.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Act six's, where it is simply true, and — more usefully — the only
        /// visible sign anywhere in the game that a moment is one of the ones
        /// worth sitting with. A line arrives under a plate reading "aged nine"
        /// a beat before each of the measured silences, and nothing explains it.
        /// </para>
        /// <para>
        /// It works because the player has no idea what it means the first time
        /// and complete certainty by act six. Which is the same shape as
        /// everything else this game does with information.
        /// </para>
        /// </remarks>
        public const string SpeakerYuaChild = "speaker.yua.child";

        /// <summary>See <see cref="SpeakerYuaChild"/>.</summary>
        public const string SpeakerHaruChild = "speaker.haru.child";

        /// <summary>
        /// The girl from their class, who is on screen for nine lines of act
        /// one and is never named.
        /// </summary>
        /// <remarks>
        /// Her plate says what she is rather than who she is, in all three
        /// languages, and that is the point: she is the outside looking at the
        /// two of them, and a name would make her a fourth character the player
        /// then expects to see again.
        /// </remarks>
        public const string SpeakerClassmate = "speaker.classmate";

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
