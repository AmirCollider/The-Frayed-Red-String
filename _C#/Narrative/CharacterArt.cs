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
        DrinkFinished = 22,

        // ---------------------------------------------------------------------
        //  The middle register
        //
        //  Counted across acts one and two, ninety per cent of the game was
        //  three faces: Unchanged, Joyful, Neutral. Not because the art was
        //  missing — Sad, Crying and Manic all existed and went unused — but
        //  because the signature faces are rationed on purpose and there was
        //  nothing between an ordinary smile and dead eyes.
        //
        //  These eight are that middle. They are the faces a comedy needs:
        //  somebody being caught out, somebody refusing to be impressed,
        //  somebody pleased with themselves. Acts one to four have to genuinely
        //  BE the sweet game, and a sweet game with three expressions is a
        //  slideshow.
        //
        //  Unlike the signature faces, these are not rationed.
        // ---------------------------------------------------------------------

        /// <summary>
        /// Arms folded, mouth set, refusing to find it funny. Yua's.
        /// </summary>
        /// <remarks>
        /// The face for every argument she loses about something that does not
        /// matter — which, per the manual, is one per scene. Haru has no half of
        /// this and does not need one: he does not sulk, he concedes.
        /// </remarks>
        Pout = 23,

        /// <summary>
        /// Pleased with herself, and not hiding it. Yua's.
        /// </summary>
        /// <remarks>
        /// The most load-bearing of the eight. This is her reward face — the
        /// small warmth that arrives immediately after she has got the thing she
        /// wanted. Joyful has been standing in for it, and Joyful means
        /// something else entirely.
        /// </remarks>
        Smug = 24,

        /// <summary>Caught off guard. Both of them have one.</summary>
        Surprised = 25,

        /// <summary>
        /// Eyes shut, arms folded, declining to react.
        /// </summary>
        /// <remarks>
        /// Not <see cref="DeadEyes"/> and must never be used where that one
        /// belongs. This is a joke landing badly; that one is nobody being home.
        /// They look alike enough that using this in the scene before a DeadEyes
        /// moment spends the DeadEyes.
        /// </remarks>
        Bored = 26,

        /// <summary>
        /// One hand at her chest, the other raised with a finger up. Yua's.
        /// </summary>
        /// <remarks>
        /// The file is called YuaThinkingLookingUp and she does not look up —
        /// the drawing points instead, which is better: looking up is thinking,
        /// and pointing is having already decided. The name stays because the
        /// file does.
        /// </remarks>
        Thinking = 27,

        /// <summary>
        /// Smiling with a hand at his neck, and it is not working. Haru's.
        /// </summary>
        /// <remarks>
        /// His whole character in one drawing, and the face for every line where
        /// his leg has stopped him and he says something else. A real smile
        /// there would be a lie the picture tells; this one is the truth the
        /// picture tells while he lies.
        /// </remarks>
        StrainedSmile = 28,

        /// <summary>Hand behind his head, caught out, apologising. Haru's.</summary>
        Sheepish = 29,

        /// <summary>
        /// A hand at his hip, easy and certain. Haru's.
        /// </summary>
        /// <remarks>
        /// For the unimportant argument he wins in every scene. Angry is far too
        /// much for that, and Neutral does not look like somebody who is right.
        /// </remarks>
        CalmSerious = 30,

        // ---------------------------------------------------------------------
        //  Autumn and winter, and two hands round something warm
        //
        //  Added for acts one and two once the seasons were right. Between
        //  October and March these two are outdoors in clothes that are not
        //  enough, and the game had no way to show it: every sprite stood with
        //  its arms down as though it were still September.
        // ---------------------------------------------------------------------

        /// <summary>
        /// Arms folded across herself against the cold. Yua's.
        /// </summary>
        /// <remarks>
        /// She is in a summer dress in November and December and will not admit
        /// it, which is the joke and also the character. Haru has no drawing of
        /// this and does not need one — he puts his hands in his pockets
        /// instead, which is what <see cref="Pockets"/> is, and that is where
        /// this falls through to for him.
        /// </remarks>
        ColdHug = 31,

        /// <summary>
        /// One hand up, five fingers open. Yua's.
        /// </summary>
        /// <remarks>
        /// The last picture of a count, and the one the scene is built on.
        /// See <see cref="CountingOne"/> for why the other four exist.
        /// </remarks>
        Counting = 32,

        /// <summary>Both hands round a hot can. Both of them have one.</summary>
        HoldCan = 33,

        /// <summary>Both hands round a hot cup, still full.</summary>
        /// <remarks>
        /// The café in December, and the winter answer to
        /// <see cref="DrinkFull"/> — that one is an iced matcha with a straw and
        /// belongs to September.
        /// </remarks>
        WarmCup = 34,

        /// <summary>
        /// The cup, finished.
        /// </summary>
        /// <remarks>
        /// Haru's only. Yua falls through to <see cref="WarmCup"/> and that is
        /// exact rather than a compromise: in the scene this exists for, her cup
        /// goes cold in her hands and she never touches it.
        /// </remarks>
        WarmCupEmpty = 35,

        /// <summary>Both hands in his pockets. Haru's.</summary>
        /// <remarks>
        /// What he does instead of holding the warm thing he has just given
        /// away. The narrator says so out loud once, in act two.
        /// </remarks>
        Pockets = 36,

        // ---------------------------------------------------------------------
        //  The rest of the count
        //
        //  Act one's playground scene has her count to five out loud, and until
        //  these existed there was exactly one drawing for it: the open hand.
        //  So the count ran one, two, three, four on a hand that was already
        //  showing five, and the number the whole act has been quietly building
        //  towards arrived on a picture the player had been staring at for four
        //  frames.
        //
        //  Five drawings, five numbers. The hand fills up as she counts, and on
        //  "five" it is full and her face has already gone somewhere else.
        // ---------------------------------------------------------------------

        /// <summary>One finger. Yua's.</summary>
        CountingOne = 37,

        /// <summary>Two fingers. Yua's.</summary>
        CountingTwo = 38,

        /// <summary>Three. Yua's.</summary>
        CountingThree = 39,

        /// <summary>Four. Yua's.</summary>
        CountingFour = 40
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

                // The winter half of the same idea: a hot can off the machine
                // and a cup in a cafe. A nine-year-old has neither, and neither
                // is a face the project still owes anybody a drawing of.
                case Portrait.HoldCan:
                case Portrait.WarmCup:
                case Portrait.WarmCupEmpty:
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
                case Portrait.Pout: return "PoutTsundere";
                case Portrait.ColdHug: return "ArmsHuggingCold";
                case Portrait.CountingOne: return "CountingFingers01";
                case Portrait.CountingTwo: return "CountingFingers02";
                case Portrait.CountingThree: return "CountingFingers03";
                case Portrait.CountingFour: return "CountingFingers04";
                case Portrait.Counting: return "CountingFingers";
                case Portrait.HoldCan: return "HoldWarmCan";

                // Her cup is never finished, so both states are the same
                // drawing. That is the scene, not a shortcut.
                case Portrait.WarmCup: return "WarmCupHoldUntouched";
                case Portrait.WarmCupEmpty: return "WarmCupHoldUntouched";

                // She does not stand with her hands in her pockets. The dress
                // does not have any.
                case Portrait.Pockets: return "NeutralGentleSmile";
                case Portrait.Smug: return "SmugMischievousSmile";
                case Portrait.Surprised: return "SurprisedTakenAback";
                case Portrait.Bored: return "BoredUnamused";
                case Portrait.Thinking: return "ThinkingLookingUp";

                // No drawing of her own, and none is wanted. She does not wear a
                // smile that is not working, and she is never the one caught out.
                case Portrait.StrainedSmile: return "SadImploringTearful";
                case Portrait.Sheepish: return "ShyBlushingLookDown";
                case Portrait.CalmSerious: return "NeutralGentleSmile";

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
                case Portrait.Surprised: return "SurprisedTakenAback";
                case Portrait.Bored: return "DeadpanFlat";
                case Portrait.HoldCan: return "HoldWarmCan";
                case Portrait.WarmCup: return "WarmCupHoldFull";
                case Portrait.WarmCupEmpty: return "WarmCupHoldEmpty";
                case Portrait.Pockets: return "HandsInPockets";

                // He does not fold his arms against the cold, he pockets his
                // hands. Same meaning, his gesture.
                case Portrait.ColdHug: return "HandsInPockets";

                // No counting hand and no scene that wants one.
                case Portrait.CountingOne:
                case Portrait.CountingTwo:
                case Portrait.CountingThree:
                case Portrait.CountingFour:
                case Portrait.Counting: return "NeutralGentleSmile";
                case Portrait.StrainedSmile: return "StrainedPainfulSmile";
                case Portrait.Sheepish: return "SheepishHandBehindHead";
                case Portrait.CalmSerious: return "CalmSerious";

                case Portrait.Smug: return "SmugPleasedWithSelf";

                // He does not sulk, he concedes; and he does not decide in front
                // of anyone. Both fall through to faces he does have.
                case Portrait.Pout: return "SeriousAngryFrown";
                case Portrait.Thinking: return "NeutralGentleSmile";

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
        /// <summary>
        /// The paved path up to the school, in autumn.
        /// </summary>
        /// <remarks>
        /// It used to be CherryBlossomSchoolAlleyDay, and that was wrong in a way
        /// that mattered. The game opens on the 2nd of September and ends on the
        /// 24th of March: blossom season is neither of those, and a path full of
        /// flowering cherry trees in the first week of the second term is the
        /// single most visible continuity error the project had.
        ///
        /// Replacing it also does something the old picture could not. Act one is
        /// called Cherry Blossom Mirage. There are no blossoms in it. The
        /// blossom is what the player brings with them from every other game
        /// that opens on a school path in the sun — and the path they actually
        /// walk down is covered in dead leaves. Nobody mentions it.
        /// </remarks>
        public const string SchoolAlleyDay = "AutumnSchoolAlleyDay";

        /// <summary>
        /// The same path in blossom.
        /// </summary>
        /// <remarks>
        /// Kept, and deliberately not used by acts one to five. It is for the
        /// flashback and for the ending named after a dream — the two places in
        /// this game where something is allowed to be beautiful and untrue at
        /// the same time.
        /// </remarks>
        public const string SchoolAlleySpring = "CherryBlossomSchoolAlleyDay";
        /// <summary>
        /// 1-A in the sun, with the maples turned outside the window.
        /// </summary>
        /// <remarks>
        /// The autumn one is the default because the game is set in autumn and
        /// winter and never once in spring. Every act from the first to the
        /// fifth wants this; the spring version is kept under
        /// <see cref="ClassroomSpringDay"/> for the flashback and the dream.
        ///
        /// Tomo is on the near windowsill in both, which is load-bearing: act
        /// one plants the pot, act one names it, and act two watches it lean.
        /// </remarks>
        public const string ClassroomDay = "SunnyClassroomAutumnDay";

        /// <summary>The same room with green outside. Flashback and dream only.</summary>
        public const string ClassroomSpringDay = "SunnyClassroomDay";

        /// <summary>The same room under grey light and rain, in autumn.</summary>
        public const string ClassroomRainy = "OvercastClassroomRainyAutumnDay";

        /// <summary>The rainy room with green outside. Flashback and dream only.</summary>
        public const string ClassroomRainySpring = "OvercastClassroomRainy";

        // ---------------------------------------------------------------------
        //  The stairwell
        //
        //  Between 1-A and the roof, and the only place in the school the game
        //  had no picture of. It matters because of one thing that happens on
        //  it: four floors is where Haru's leg stops him, twice, in two
        //  different acts, and a scene that happens on a staircase should not
        //  have to be played against a photograph of the roof it is on the way
        //  to.
        //
        //  The window on the half-landing carries the season and is the whole
        //  reason there are three of these.
        // ---------------------------------------------------------------------

        /// <summary>The half-landing with maples through the window.</summary>
        public const string StairsAutumn = "AutumnAfternoonSchoolStairs";

        /// <summary>The half-landing with snow through the window.</summary>
        public const string StairsWinter = "WinterAfternoonSchoolStairs";

        /// <summary>The half-landing with blossom. Flashback and dream only.</summary>
        public const string StairsSpring = "SpringAfternoonSchoolStairs";
        /// <summary>
        /// The second-floor corridor in autumn light.
        /// </summary>
        /// <remarks>
        /// The canonical one, and the one every act from the first to the fifth
        /// wants: the maples through the glass have turned. The spring version
        /// still exists under <see cref="CorridorSunsetSpring"/> and is not for
        /// anything that happens between September and March.
        ///
        /// Both versions carry the floor grille at the far end. Act two's second
        /// silence is built on it — the machine room comes up through it — so a
        /// corridor without one cannot be used for that scene.
        /// </remarks>
        public const string CorridorSunset = "SchoolCorridorAutumnSunset";

        /// <summary>The same corridor with cherry blossom through the windows.</summary>
        /// <remarks>
        /// Flashback and dream only. See <see cref="SchoolAlleySpring"/> for the
        /// reasoning: blossom in this game is a thing that is not there.
        /// </remarks>
        public const string CorridorSunsetSpring = "SchoolCorridorSunset";

        /// <summary>
        /// The same corridor at night, in winter, with rain on the glass.
        /// </summary>
        /// <remarks>
        /// The umbrella stand at the near end exists in THIS PICTURE ONLY. It
        /// is not in the autumn corridor and not in the spring one, which is a
        /// fact about the drawings and is also the funniest thing in the
        /// building: the school puts the umbrella stand out in winter and takes
        /// it away in autumn, when it also rains. Act two says so out loud.
        /// </remarks>
        public const string CorridorWinterRainyNight = "SchoolCorridorWinterRainyNight";

        /// <summary>The roof with the potted trees turned, and the city beyond.</summary>
        public const string RooftopDay = "SchoolRooftopAutumnSunnyDay";

        /// <summary>The roof in hydrangea season. Flashback and dream only.</summary>
        public const string RooftopSpringDay = "SchoolRooftopSunnyDay";

        /// <summary>The bakery street in autumn: pumpkins out, cats on the bench.</summary>
        public const string BakeryStreetDay = "UsagiBakeryStreetAutumnDay";

        /// <summary>The same street under snow, cats still on the bench.</summary>
        public const string BakeryStreetWinterDay = "UsagiBakeryStreetWinterDay";

        /// <summary>The green version. Flashback and dream only.</summary>
        public const string BakeryStreetSpringDay = "UsagiBakeryStreetDay";
        /// <summary>The corner with the machine, in autumn daylight.</summary>
        public const string VendingStreetDay = "PastelStreetVendingAutumnDay";

        /// <summary>The same corner with blossom in the background trees.</summary>
        public const string VendingStreetSpringDay = "PastelStreetVendingDay";

        /// <summary>
        /// The corner at night, with the beds still in flower and a low stone
        /// wall beside the machine.
        /// </summary>
        /// <remarks>
        /// Act one's last scene. The wall is load-bearing — the two of them sit
        /// on it and open a can each — and it was added to this picture on
        /// purpose so that they could.
        ///
        /// Right for September and wrong from November on: the beds are full of
        /// flowers and act two says they have gone.
        /// </remarks>
        public const string VendingStreetNight = "PastelStreetVendingNight";

        /// <summary>
        /// The corner at night under snow: the beds are bare earth and dead
        /// stalks, there is snow on the machine and along the wall.
        /// </summary>
        /// <remarks>
        /// DECEMBER AND AFTER, not November. The snow went into this picture
        /// after act two was written against a version that had none, and snow
        /// on the 5th of November in Kanagawa is a claim about the weather that
        /// nobody would believe.
        ///
        /// Act two's November scene therefore does not use it and asks for an
        /// autumn night version of this corner instead — see
        /// AboutProject/ArtRequest-Act01-Act02.md. Until that exists, the
        /// November scene is written for daylight.
        /// </remarks>
        public const string VendingStreetWinterNight = "PastelStreetVendingWinterNight";
        public const string AlleywayNight = "TraditionalAlleywayNight";

        /// <summary>
        /// The café in daylight. Act one's, in September.
        /// </summary>
        /// <remarks>
        /// Deliberately the one picture in the game with no season in it. The
        /// windows are bright and what is behind them is pale and out of focus,
        /// so this reads as any warm afternoon — which is what act one needs and
        /// what act two must not use.
        /// </remarks>
        public const string CafeDay = "CozyCafeDay";

        /// <summary>
        /// The café under low grey light, with rain on the glass.
        /// </summary>
        /// <remarks>
        /// Act two's heavy scene. The narration steams the windows up in its
        /// first line, so what is behind them stops mattering three seconds in —
        /// which is why the autumn picture serves a December evening without
        /// lying about anything the player can see.
        /// </remarks>
        public const string CafeRainy = "CozyCafeDimAutumnDimRainy";

        /// <summary>The rainy café with green outside. Flashback and dream only.</summary>
        public const string CafeRainySpring = "CozyCafeDimRainy";

        /// <summary>The playground in autumn. Children on the slide, the angel in the fountain.</summary>
        public const string PlaygroundDay = "PastelPlaygroundAutumnDay";

        /// <summary>The playground in hydrangea season. Flashback and dream only.</summary>
        public const string PlaygroundSpringDay = "PastelPlaygroundDay";
        /// <summary>The platform in autumn, with the trackside trees turned.</summary>
        public const string TrainPlatformSunset = "TrainPlatformAutumnSunset";

        /// <summary>The platform with the trackside cherries in flower.</summary>
        public const string TrainPlatformSpringSunset = "TrainPlatformSunset";

        /// <summary>
        /// The platform in winter: the trackside trees are bare and there is
        /// frost on the ground and on the seats.
        /// </summary>
        /// <remarks>
        /// Act two, the 19th of December. The narrator says the trees have
        /// nothing on them at all, and this is the picture that sentence is
        /// describing.
        /// </remarks>
        public const string TrainPlatformWinterSunset = "TrainPlatformWinterSunset";
        /// <summary>Yua's room with the maples through the window.</summary>
        public const string YuaRoomDay = "YuaRoomAutumnDay";

        /// <summary>Yua's room with a bare tree and snow through the window.</summary>
        /// <remarks>Act two's last scene is the 22nd of December.</remarks>
        public const string YuaRoomWinterDay = "YuaRoomWinterDay";

        /// <summary>Yua's room in spring. Flashback and dream only.</summary>
        public const string YuaRoomSpringDay = "YuaRoomSunnyDay";

        /// <summary>
        /// Haru's room, and the last place in the story that is still a room.
        /// </summary>
        /// <remarks>
        /// Act five ends here, not in an alleyway: he makes a poor excuse, walks
        /// her home to his own house, shows her up, and goes to the kitchen for a
        /// glass of water. The wardrobe is in shot the whole time.
        /// </remarks>
        public const string HaruRoomDay = "HaruRoomSunnyDay";

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