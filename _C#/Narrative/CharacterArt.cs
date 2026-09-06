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
        HaruChild = 5,

        /// <summary>
        /// A girl from their class. Never on stage, and never named twice.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The one person in act one who is neither of them, and she exists for
        /// a reason the Japanese script does not need: <c>ぴ</c> is real slang
        /// and a Japanese player hears the whole of what it means the first time
        /// Yua says it. A Persian or English player hears a nickname. So
        /// somebody outside the pair has to notice it out loud, once, and be
        /// told to mind her own business — which is how the suffix acquires the
        /// weight it already had in one of the three languages.
        /// </para>
        /// <para>
        /// Deliberately a speaker with no body. <see cref="CharacterArt"/>
        /// returns no sprite for her, so she is a name plate and a voice off to
        /// one side, and the stage does the right thing with her for free: the
        /// focus call finds neither slot holding her and steps both Yua and Haru
        /// back, which is what happens in a corridor when a third person starts
        /// talking. A third slot in
        /// <see cref="Presentation.VisualNovelStage"/> would be the alternative
        /// and it would need art nobody has drawn.
        /// </para>
        /// <para>
        /// Numbered after the five that existed first, and it has to be — an act
        /// asset stores a speaker as its integer.
        /// </para>
        /// </remarks>
        Classmate = 6
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
        Crying = 8,

        /// <summary>
        /// Wincing through a sudden physical pain — added for the desk-knock in
        /// act one.
        /// </summary>
        /// <remarks>
        /// Haru only. There is no Yua half of this pose and none is expected —
        /// nothing in the design document ever puts her through the same kind
        /// of moment — so <see cref="YuaSuffix"/> has no case for it and falls
        /// back to her neutral face if this is ever set for her by mistake.
        /// <see cref="HaruSuffix"/> answers for <see cref="Speaker.HaruChild"/>
        /// as well as <see cref="Speaker.Haru"/>, so a "HaruChildInjuredKnee-
        /// Grimace" sprite is now a valid (if not yet drawn) request too.
        /// </remarks>
        Injured = 9,

        // ---------------------------------------------------------------------
        //  Act one's two wordless sequences
        //
        //  Ten pictures of a lunch and three of a drink, and the numbers matter:
        //  a pose here is a moment of a shared action rather than a face, so
        //  both characters answer the same name with a different drawing of the
        //  same instant. Yua's fourth picture of the meal has her lifting a
        //  piece of sushi while Haru's has him lifting an octopus sausage, and
        //  the script that plays them says LunchFirstLift once and means both.
        //
        //  Written as portraits rather than as an animation format because
        //  nothing else about them is special: they are two people changing
        //  expression on a timer, which is what this enum and a held beat
        //  already do. See ActScriptWriter.Cel.
        // ---------------------------------------------------------------------

        /// <summary>The bento is still shut. Only Yua is holding anything.</summary>
        LunchOpen = 10,

        /// <summary>She opens it and shows him; he holds the empty lid out.</summary>
        LunchOffer = 11,

        /// <summary>Most of it is on his lid now.</summary>
        LunchShared = 12,

        /// <summary>Both lift their first piece.</summary>
        LunchFirstLift = 13,

        /// <summary>Both eat it.</summary>
        LunchFirstBite = 14,

        /// <summary>Their second.</summary>
        LunchSecondLift = 15,

        LunchSecondBite = 16,

        /// <summary>Their last.</summary>
        LunchThirdLift = 17,

        LunchThirdBite = 18,

        /// <summary>
        /// The box is shut again.
        /// </summary>
        /// <remarks>
        /// Haru has no drawing of his own for this one and does not need it —
        /// he is finished, with nothing left in his hands, which is his neutral
        /// face and nothing else. It falls through to it.
        /// </remarks>
        LunchFinished = 19,

        /// <summary>Two full cups: she is already drinking hers, he is holding his.</summary>
        DrinkFull = 20,

        /// <summary>Hers is gone. He makes himself start on the matcha he does not like.</summary>
        DrinkReluctant = 21,

        /// <summary>Both empty, and both of them pleased about it for different reasons.</summary>
        DrinkFinished = 22
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
                // Everything except the props: the flashback is a schoolyard
                // and a machine room, and a "YuaChildBento04LiftFirstSushi"
                // would be a name for a picture nobody is ever going to draw.
                case Speaker.YuaChild: return "YuaChild" + YuaSuffix(WithoutProps(portrait));
                case Speaker.HaruChild: return "HaruChild" + HaruSuffix(WithoutProps(portrait));

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

                // The children get a plate of their own, and it is the only
                // sign the game ever gives that a moment is worth waiting at.
                case Speaker.YuaChild: return LocKeys.SpeakerYuaChild;
                case Speaker.HaruChild: return LocKeys.SpeakerHaruChild;

                // A plate but no sprite. She is the only speaker in the game
                // that is a voice and nothing else, and the plate is the whole
                // point of her: the player has to see that the line came from
                // somebody who is not one of the two.
                case Speaker.Classmate: return LocKeys.SpeakerClassmate;

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

        /// <summary>
        /// True for a pose that only exists with something in the character's
        /// hands — a bento, a lid, a cup.
        /// </summary>
        /// <remarks>
        /// Worth being able to ask, because these are the only poses in the game
        /// that are not simply a face. Two things read it: the child speakers,
        /// who have no drawing of any of them, and the readiness report, which
        /// would otherwise count a nine-year-old's bento as art somebody still
        /// owes the project.
        /// </remarks>
        public static bool IsPropPose(Portrait portrait)
        {
            switch (portrait)
            {
                case Portrait.LunchOpen:
                case Portrait.LunchOffer:
                case Portrait.LunchShared:
                case Portrait.LunchFirstLift:
                case Portrait.LunchFirstBite:
                case Portrait.LunchSecondLift:
                case Portrait.LunchSecondBite:
                case Portrait.LunchThirdLift:
                case Portrait.LunchThirdBite:
                case Portrait.LunchFinished:
                case Portrait.DrinkFull:
                case Portrait.DrinkReluctant:
                case Portrait.DrinkFinished:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>The same pose with nothing in hand, for whoever has no props.</summary>
        private static Portrait WithoutProps(Portrait portrait)
        {
            return IsPropPose(portrait) ? Portrait.Neutral : portrait;
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

                // Her half of the lunch: six sushi and four octopus sausages,
                // most of which go to him, and the four pieces she keeps for
                // herself are the four pictures in the middle.
                case Portrait.LunchOpen: return "Bento01HoldClosedBox";
                case Portrait.LunchOffer: return "Bento02ShowFullFood";
                case Portrait.LunchShared: return "Bento03SharedMostlyEmpty";
                case Portrait.LunchFirstLift: return "Bento04LiftFirstSushi";
                case Portrait.LunchFirstBite: return "Bento05SavorFirstSushi";
                case Portrait.LunchSecondLift: return "Bento06LiftLastSushi";
                case Portrait.LunchSecondBite: return "Bento07SavorLastSushi";
                case Portrait.LunchThirdLift: return "Bento08LiftLastOctopus";
                case Portrait.LunchThirdBite: return "Bento09SavorLastOctopus";
                case Portrait.LunchFinished: return "Bento10ClosedFinishedSmile";

                // And her half of the café. She finishes first, and the last
                // picture is her sitting with her eyes shut while he catches up.
                case Portrait.DrinkFull: return "BobaSipFullCup";
                case Portrait.DrinkReluctant: return "BobaHoldEmptyCup";
                case Portrait.DrinkFinished: return "PeacefulClosedEyesSmile";

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
                case Portrait.Injured: return "InjuredKneeGrimace";

                // His half of the lunch, one picture behind hers all the way
                // through: she has to open the box before he has anything to
                // hold, so his first drawing lands on her second, and by the
                // time the box is shut again he is back to having empty hands
                // and no picture of his own — which is his neutral face, below.
                case Portrait.LunchOffer: return "Bento01HoldEmptyLid";
                case Portrait.LunchShared: return "Bento02FoodReceived";
                case Portrait.LunchFirstLift: return "Bento03LiftFirstOctopus";
                case Portrait.LunchFirstBite: return "Bento04SavorFirstOctopus";
                case Portrait.LunchSecondLift: return "Bento05LiftFirstSushi";
                case Portrait.LunchSecondBite: return "Bento06SavorFirstSushi";
                case Portrait.LunchThirdLift: return "Bento07LiftSecondOctopus";
                case Portrait.LunchThirdBite: return "Bento08SavorSecondOctopus";

                // The matcha he was ordered and does not like.
                case Portrait.DrinkFull: return "MatchaHoldFullCup";
                case Portrait.DrinkReluctant: return "MatchaSipReluctant";
                case Portrait.DrinkFinished: return "MatchaHoldEmptyCup";

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