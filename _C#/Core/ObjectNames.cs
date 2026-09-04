// -----------------------------------------------------------------------------
//  The Frayed Red String
//  ObjectNames.cs
//
//  The two authored scenes were built by hand before any code existed, so the
//  runtime binds to them by name. Every name the code depends on is collected
//  here; if a GameObject is renamed in the editor, this is the only file that
//  needs to follow.
// -----------------------------------------------------------------------------

namespace TheFrayedRedString.Core
{
    /// <summary>Names of authored GameObjects the runtime binds to.</summary>
    public static class ObjectNames
    {
        // --- WarningScene ----------------------------------------------------

        public const string StartCanvas = "StartCanvas";
        public const string WarningTextEnglish = "WarningTextTMPEN";

        /// <summary>
        /// The second warning label. Named for Japanese because that is what the
        /// scene calls it, but it shows Persian instead once the player has
        /// selected Persian — see <c>LocalizedTextMode.Secondary</c>.
        /// </summary>
        public const string WarningTextJapanese = "WarningTextTMPJP";
        public const string EnterPrompt = "EnterButtonImage";

        // --- MainMenu --------------------------------------------------------

        public const string BackgroundCanvas = "BackgroundCanvas";
        public const string MenuBackground = "MainMenuBackGrundImage";
        public const string MenuCharacters = "MainMenuBackGrundYuaAndHaruImage";

        /// <summary>
        /// The picture the title screen wears once the story has been finished.
        /// </summary>
        /// <remarks>
        /// The design document's last image: the two of them as children, with
        /// the rabbit still whole. It is an asset name rather than a scene
        /// object because it replaces the sprite on
        /// <see cref="MenuCharacters"/> rather than being a second object in the
        /// scene — the menu should not carry a hidden copy of a picture that
        /// most players will never see.
        /// </remarks>
        public const string MenuCharactersChildhood = "MainMenuChildhoodImage";
        public const string MenuTitle = "MainMenuBackGrundGameFrayedRedStringTextImage";
        public const string WarningCard = "WarningCardPlusEighteenImage";

        public const string LanguageButtonMask = "ChangeLanguageButtonMaskMather";
        public const string LanguageButton = "ChangeLanguageButton";

        public const string MenuButtonsRoot = "BackgroundCanvasButtons";
        public const string StartGameButton = "StartGameButton";
        public const string OpenLoadMenuButton = "OpenLoadMenuButton";

        /// <summary>Authored with this exact spelling in MainMenu.unity.</summary>
        public const string ExitGameButton = "ExirGameButton";

        public const string LoadPanel = "LoadGameMenuPanel";

        /// <summary>
        /// The two backdrops the act scenes' copy of the panel carries. They are
        /// fitted to their frame, so they are held still and the panel moves as
        /// one piece — see AmbientMotionInstaller.
        /// </summary>
        public const string LoadPanelSheet = "LoadGameMenuBackGrundImage";

        public const string LoadPanelCard = "LoadGamePanelBackGrundImage";
        public const string LoadPanelTitle = "LoadPanelTitleTextTMP";
        public const string LoadPanelBackground = "LoadMenuPanelBackGrundImage";

        /// <summary>
        /// The game-title art shown inside the load panel. It occupies the same
        /// place on screen as <see cref="MenuTitle"/>, so exactly one of the two
        /// is ever visible.
        /// </summary>
        public const string LoadPanelTitleArt = "MainMenuBackGrundGameFrayedRedStringTextImageForLoadPanel";

        /// <summary>Save slots are named SaveSlot01 … SaveSlot03.</summary>
        public const string SaveSlotPrefix = "SaveSlot";

        public const string SlotEmptyLabel = "UnlivedSeasonTextTMP";
        public const string SlotScreenshot = "GameScreenShotImage";
        public const string SlotSeasonLabel = "SeasenTextTMP";
        public const string SlotSeasonNameLabel = "SeasenNameTextTMP";
        public const string SlotPlayTimeLabel = "PlayTimeTextTMP";

        /// <summary>Optional close button; the load panel works without it.</summary>
        public const string BackToMenuButton = "MenuButtonBackToMenu";

        // --- Act scenes ------------------------------------------------------

        /// <summary>
        /// The canvas Act01.unity carries its menu furniture on. The act adopts
        /// it as the pause layer rather than building a second one.
        /// </summary>
        public const string ActMenuCanvas = "MenuCanvas";

        /// <summary>The full-screen image the story's backgrounds are drawn into.</summary>
        public const string StoryBackground = "GameBackGrundImage";
    }
}
