// -----------------------------------------------------------------------------
//  The Frayed Red String
//  CharacterArt.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Localization;

namespace TheFrayedRedString.Narrative
{
    /// <summary>Who is speaking a line.</summary>
    public enum Speaker
    {
        /// <summary>No name plate. Description, interiority, stage direction.</summary>
        Narrator,

        /// <summary>Yua. The player character, and the one running the game.</summary>
        Yua,

        /// <summary>Haru.</summary>
        Haru,

        /// <summary>
        /// A line addressed to the person holding the controller rather than to
        /// anyone in the scene.
        /// </summary>
        /// <remarks>
        /// Unused in act one — the fourth wall does not come down until act two —
        /// but the plate style is defined here so that when it does, the change
        /// is one enum value in a script rather than a new presentation path.
        /// </remarks>
        Player
    }

    /// <summary>
    /// The expression a character is wearing.
    /// </summary>
    /// <remarks>
    /// Deliberately named for the emotion rather than for the file, because the
    /// two characters spell the same emotion differently on disk — Yua looks
    /// down when she is shy and Haru looks away, and her anger is a glare where
    /// his is a frown. A script should not have to know that.
    /// </remarks>
    public enum Portrait
    {
        /// <summary>Leave whatever is already on screen.</summary>
        Unchanged,

        Neutral,
        Joyful,
        Shy,
        Sad,
        Angry,
        DeadEyes,
        Manic,
        Crying
    }

    /// <summary>Which half of the stage a character stands on.</summary>
    public enum StageSide
    {
        Left,
        Right
    }

    /// <summary>Resolves characters and expressions to asset names and labels.</summary>
    public static class CharacterArt
    {
        /// <summary>
        /// The sprite file name for one character's expression, without the
        /// extension. Returns <c>null</c> for speakers who have no body on
        /// stage.
        /// </summary>
        public static string SpriteName(Speaker speaker, Portrait portrait)
        {
            if (portrait == Portrait.Unchanged)
            {
                return null;
            }

            switch (speaker)
            {
                case Speaker.Yua: return "Yua" + YuaSuffix(portrait);
                case Speaker.Haru: return "Haru" + HaruSuffix(portrait);
                default: return null;
            }
        }

        /// <summary>The localisation key of a speaker's name plate, or <c>null</c> for none.</summary>
        public static string NameKey(Speaker speaker)
        {
            switch (speaker)
            {
                case Speaker.Yua: return LocKeys.SpeakerYua;
                case Speaker.Haru: return LocKeys.SpeakerHaru;
                default: return null;
            }
        }

        /// <summary>The side of the stage a character occupies by default.</summary>
        /// <remarks>
        /// Fixed for the whole game. A character who changes sides between
        /// scenes makes the player re-locate them every time, and act one has
        /// only two people in it.
        /// </remarks>
        public static StageSide HomeSide(Speaker speaker)
        {
            return speaker == Speaker.Haru ? StageSide.Right : StageSide.Left;
        }

        private static string YuaSuffix(Portrait portrait)
        {
            switch (portrait)
            {
                case Portrait.Joyful: return "JoyfulHappyLaugh";
                case Portrait.Shy: return "ShyBlushingLookDown";
                case Portrait.Sad: return "SadImploringTearful";
                case Portrait.Angry: return "AnnoyedAngryGlare";
                case Portrait.DeadEyes: return "DeadEyesPokerFace";
                case Portrait.Manic: return "InsaneManicSmile";
                case Portrait.Crying: return "SorrowfulCryingTears";
                default: return "NeutralGentleSmile";
            }
        }

        private static string HaruSuffix(Portrait portrait)
        {
            switch (portrait)
            {
                case Portrait.Joyful: return "JoyfulHappyLaugh";
                case Portrait.Shy: return "ShyBlushingLookAway";
                case Portrait.Sad: return "SadImploringTearful";
                case Portrait.Angry: return "SeriousAngryFrown";
                case Portrait.DeadEyes: return "DeadEyesPokerFace";
                case Portrait.Manic: return "InsaneManicSmile";
                case Portrait.Crying: return "SorrowfulCryingTears";
                default: return "NeutralGentleSmile";
            }
        }
    }

    /// <summary>
    /// Background file names, so a script never spells one out as a literal.
    /// </summary>
    public static class Backgrounds
    {
        public const string SchoolAlleyDay = "CherryBlossomSchoolAlleyDay";
        public const string ClassroomDay = "SunnyClassroomDay";
        public const string ClassroomRainy = "OvercastClassroomRainy";
        public const string CorridorSunset = "SchoolCorridorSunset";
        public const string RooftopDay = "SchoolRooftopSunnyDay";
        public const string BakeryStreetDay = "UsagiBakeryStreetDay";
        public const string VendingStreetDay = "PastelStreetVendingDay";
        public const string VendingStreetNight = "PastelStreetVendingNight";
        public const string AlleywayNight = "TraditionalAlleywayNight";
        public const string CafeDay = "CozyCafeDay";
        public const string CafeRainy = "CozyCafeDimRainy";
        public const string PlaygroundDay = "PastelPlaygroundDay";
        public const string TrainPlatformSunset = "TrainPlatformSunset";
        public const string YuaRoomDay = "YuaRoomSunnyDay";
    }
}
