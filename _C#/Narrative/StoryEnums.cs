// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryEnums.cs
//
//  The vocabulary an act is written in. Kept in a file of its own because both
//  the asset format and the runtime depend on it, and neither should have to
//  outlive the other.
// -----------------------------------------------------------------------------

namespace TheFrayedRedString.Narrative
{
    /// <summary>
    /// What kind of thing a beat is.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Every value is pinned to a number, and they have to be. Unity stores a
    /// serialized enum as its integer, so an act asset on disk holds 14 and not
    /// "End" — which means adding a kind anywhere except the very bottom of the
    /// list silently rewrites every act already saved. Act five's four kinds
    /// were added in the middle of this enum and turned every End beat in the
    /// project into a PullDownDialogue, which is exactly the class of bug that
    /// is invisible until somebody opens a scene six months later.
    /// </para>
    /// <para>
    /// With the numbers written down, the order in this file is free to be
    /// whatever reads best, and a new kind is a new number at the end.
    /// </para>
    /// </remarks>
    public enum StoryBeatKind
    {
        /// <summary>A spoken or narrated line. Waits for the player.</summary>
        Line = 0,
        /// <summary>Change of location. Waits for the transition.</summary>
        Background = 1,
        /// <summary>Bring a character on, or change their expression.</summary>
        Enter = 2,
        /// <summary>Take a character off.</summary>
        Exit = 3,
        /// <summary>Clear the stage.</summary>
        ClearStage = 4,
        /// <summary>Name the place in the corner. Does not wait.</summary>
        Caption = 5,
        /// <summary>The act's title card. Waits.</summary>
        TitleCard = 6,
        /// <summary>Play one sound. Does not wait.</summary>
        Sound = 7,
        /// <summary>Start a music loop, or stop the current one.</summary>
        Music = 8,
        /// <summary>Hold for a fixed time with the dialogue box hidden.</summary>
        Beat = 9,
        /// <summary>Offer the player a choice. Waits.</summary>
        Choice = 10,
        /// <summary>Maybe play a short act inline. Rolled once, when reached.</summary>
        Interlude = 11,
        /// <summary>
        /// Open the frame the game has been played inside since act one.
        /// </summary>
        /// <remarks>
        /// Act five's turn, per the design document: Haru takes hold of the
        /// picture and widens it. It is a beat rather than a hard-coded moment
        /// so the act that does it can be moved or rewritten without touching
        /// any code.
        /// </remarks>
        OpenFrame = 12,
        /// <summary>Put the frame back.</summary>
        CloseFrame = 13,
        /// <summary>
        /// Haru takes hold of the dialogue box and pulls it off the bottom of
        /// the screen.
        /// </summary>
        /// <remarks>
        /// Act five's, and the design document is specific about it: he puts a
        /// hand on the bar and drags it down until it is gone. It is a beat of
        /// its own rather than part of <see cref="OpenFrame"/> because the two
        /// are separate gestures a few lines apart — the box first, so the
        /// player is looking at an unobstructed picture when the border starts
        /// to move.
        /// </remarks>
        PullDownDialogue = 15,
        /// <summary>
        /// Changes how much of the pastel haze the picture is seen through.
        /// </summary>
        /// <remarks>
        /// The game has been played behind a soft pink veil since act one. The
        /// design document calls it the pane of frosted glass and has act five
        /// take it away, which is the whole of "the graphics become realistic"
        /// in a game that is never going to be three-dimensional: the same art,
        /// with nothing in front of it.
        /// </remarks>
        Grade = 16,
        /// <summary>
        /// Throws a colour across the screen and leaves it there.
        /// </summary>
        /// <remarks>
        /// One thing in this game needs it. Written as a colour and a duration
        /// rather than as an effect called Blood, so the act that uses it says
        /// what it wants and the presentation is not named after a single line
        /// of one script.
        /// </remarks>
        Stain = 17,
        /// <summary>
        /// Stops the music in a single frame, with no fade at all.
        /// </summary>
        /// <remarks>
        /// Distinct from a <see cref="Music"/> beat with an empty track, which
        /// fades out over about a second and reads as a scene ending. This is
        /// the design document's 0 ms cut: the floor going out from under the
        /// sound.
        /// </remarks>
        CutMusic = 18,
        /// <summary>
        /// Hands the act over to itself: from here the story plays on its own
        /// and the player cannot advance, skip or hurry it.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Act six's, and the design document is unambiguous — the player has no
        /// control over anything and is watching a film. Everything still moves,
        /// the text still types, the music still plays; what is gone is the
        /// click that turns the page. Each line stays up for as long as it takes
        /// to read and then goes by itself.
        /// </para>
        /// <para>
        /// Pausing deliberately still works. A scene that cannot be paused is
        /// not cinematic, it is a hang, and the player has to be able to reach
        /// the menu and their saves in a sequence several minutes long.
        /// </para>
        /// </remarks>
        EnterCinema = 19,

        /// <summary>
        /// Plays a film, full screen, over everything.
        /// </summary>
        /// <remarks>
        /// Act seven's ending credits. A name with no film behind it — which is
        /// every name today — logs one line and the act carries straight on, so
        /// the ending can be built and played through before the video is cut.
        /// </remarks>
        Video = 21,

        /// <summary>
        /// The story is over: wipe the saves and go back to the title.
        /// </summary>
        /// <remarks>
        /// The design document's rule, and it is not a formality — a replay has
        /// to begin from the very start, because the condition for the secret
        /// ending is a fact about a whole playthrough and a save file part-way
        /// through one would let it be farmed.
        /// </remarks>
        EndGame = 22,

        /// <summary>
        /// A character stops talking to the scene and starts talking to the
        /// player.
        /// </summary>
        /// <remarks>
        /// Four scenes: Yua in her room in act two, Haru in the street and Yua
        /// in her room in act four, and Haru at the end of act five. The room
        /// dims away, the line comes slower than anywhere else in the game, and
        /// the box sits on a heavier ground. None of that is subtle and it is
        /// not meant to be — these are the moments the game is asking to be
        /// taken seriously, and it had been asking in the same voice it orders
        /// coffee in.
        /// </remarks>
        EnterAside = 23,

        /// <summary>Gives the scene back to the room.</summary>
        ExitAside = 24,

        /// <summary>Gives the player the story back.</summary>
        /// <remarks>
        /// Not needed at the end of an act — the mode belongs to the director
        /// and a new act starts with it off — but an act that goes to film for
        /// one scene and then returns needs a way to say so.
        /// </remarks>
        ExitCinema = 20,

        /// <summary>End of the act.</summary>
        End = 14
    }

    /// <summary>The moral colour of a choice, and the button art it gets.</summary>
    public enum ChoiceTone
    {
        /// <summary>Blue. Kind, honest, and — from act two — quietly overruled.</summary>
        Kind,

        /// <summary>Green. Controlling.</summary>
        Cruel,

        /// <summary>White. Carries no weight and is not counted.</summary>
        Neutral
    }
}
