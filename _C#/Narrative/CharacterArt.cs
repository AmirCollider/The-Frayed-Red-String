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
        Narrator = 0,
        /// <summary>Yua. The player character, and the one running the game.</summary>
        Yua = 1,
        /// <summary>Haru.</summary>
        Haru = 2,
        /// <summary>
        /// A line addressed to the person holding the controller rather than to
        /// anyone in the scene.
        /// </summary>
        /// <remarks>
        /// Unused in act one — the fourth wall does not come down until act two —
        /// but the plate style is defined here so that when it does, the change
        /// is one enum value in a script rather than a new presentation path.
        /// </remarks>
        Player = 3,

        /// <summary>
        /// Yua, aged nine, in the act six flashback.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A separate speaker rather than a flag on Yua. Everything that asks
        /// "who is this" — the sprite name, the name plate, which side of the
        /// stage they stand on, how tall they are drawn — wants a different
        /// answer for the child, and threading a boolean through all four of
        /// those is how you end up with a nine-year-old at adult height in one
        /// scene out of twenty.
        /// </para>
        /// <para>
        /// Numbered after the four that existed first, and it has to be: an act
        /// asset stores a speaker as its integer, so inserting one anywhere
        /// else re-points every line already written.
        /// </para>
        /// </remarks>
        YuaChild = 4,

        /// <summary>Haru, aged nine. See <see cref="YuaChild"/>.</summary>
        HaruChild = 5
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
        Unchanged = 0,
        Neutral = 1,
        Joyful = 2,
        Shy = 3,
        Sad = 4,
        Angry = 5,
        DeadEyes = 6,
        Manic = 7,
        Crying = 8
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

                // The children reuse their older selves' expression spellings,
                // so act six can be written today against art that does not
                // exist yet and the file names are already decided when it does.
                case Speaker.YuaChild: return "YuaChild" + YuaSuffix(portrait);
                case Speaker.HaruChild: return "HaruChild" + HaruSuffix(portrait);

                default: return null;
            }
        }

        /// <summary>The localisation key of a speaker's name plate, or <c>null</c> for none.</summary>
        public static string NameKey(Speaker speaker)
        {
            switch (speaker)
            {
                case Speaker.Yua:
                case Speaker.YuaChild:
                    return LocKeys.SpeakerYua;

                case Speaker.Haru:
                case Speaker.HaruChild:
                    return LocKeys.SpeakerHaru;

                default:
                    return null;
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
            return IsHaru(speaker) ? StageSide.Right : StageSide.Left;
        }

        /// <summary>True for Haru at either age.</summary>
        public static bool IsHaru(Speaker speaker)
        {
            return speaker == Speaker.Haru || speaker == Speaker.HaruChild;
        }

        /// <summary>True for Yua at either age.</summary>
        public static bool IsYua(Speaker speaker)
        {
            return speaker == Speaker.Yua || speaker == Speaker.YuaChild;
        }

        /// <summary>
        /// True for the nine-year-old versions, who stand where their older
        /// selves stand and are drawn smaller.
        /// </summary>
        public static bool IsChild(Speaker speaker)
        {
            return speaker == Speaker.YuaChild || speaker == Speaker.HaruChild;
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

        // ---------------------------------------------------------------------
        //  Act six — the flashback
        //
        //  None of these have been drawn yet. Naming them here is what lets act
        //  six be written, played and judged before they are: a background with
        //  no file behind it gets a stand-in generated from its own name, so the
        //  scenes change, the pacing is real, and the only thing missing is the
        //  picture. Drop a file with the matching name into
        //  Assets/Images/Backgrounds and it takes over with no code change.
        // ---------------------------------------------------------------------

        /// <summary>The primary-school classroom, eight years earlier.</summary>
        public const string ElementaryClassroomDay = "ElementaryClassroomDay";

        /// <summary>The corridor outside it.</summary>
        public const string ElementaryHallwayDay = "ElementaryHallwayDay";

        /// <summary>Where the two of them played, most days, for six years.</summary>
        public const string ElementaryYardDay = "ElementaryYardDay";

        /// <summary>The riverbank on the way home.</summary>
        public const string RiverbankChildhoodDusk = "RiverbankChildhoodDusk";

        /// <summary>
        /// The garden of red spider lilies.
        /// </summary>
        /// <remarks>
        /// The single most important background in the game and the one the
        /// whole of act three was pointing at. It has been mentioned twice by
        /// two people who both refused to say why.
        /// </remarks>
        public const string SpiderLilyGardenDusk = "SpiderLilyGardenDusk";

        /// <summary>The road the three of them took her down.</summary>
        public const string BackLaneDusk = "BackLaneDusk";

        /// <summary>Outside the machine room, where Haru's leg was broken.</summary>
        public const string MachineRoomDoorDusk = "MachineRoomDoorDusk";

        /// <summary>Inside it. Used once, in the dark, with nothing in shot.</summary>
        public const string MachineRoomDark = "MachineRoomDark";

        /// <summary>
        /// The alleyway act five ended in, afterwards.
        /// </summary>
        /// <remarks>
        /// Act seven's, and the only background in the game that is a version of
        /// another one. It should read as the same wall and the same lantern
        /// with something having happened in front of them.
        /// </remarks>
        public const string AlleywayAftermath = "TraditionalAlleywayAftermath";
    }
}
