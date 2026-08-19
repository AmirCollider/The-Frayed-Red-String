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
    /// <summary>What kind of thing a beat is.</summary>
    public enum StoryBeatKind
    {
        /// <summary>A spoken or narrated line. Waits for the player.</summary>
        Line,

        /// <summary>Change of location. Waits for the transition.</summary>
        Background,

        /// <summary>Bring a character on, or change their expression.</summary>
        Enter,

        /// <summary>Take a character off.</summary>
        Exit,

        /// <summary>Clear the stage.</summary>
        ClearStage,

        /// <summary>Name the place in the corner. Does not wait.</summary>
        Caption,

        /// <summary>The act's title card. Waits.</summary>
        TitleCard,

        /// <summary>Play one sound. Does not wait.</summary>
        Sound,

        /// <summary>Start a music loop, or stop the current one.</summary>
        Music,

        /// <summary>Hold for a fixed time with the dialogue box hidden.</summary>
        Beat,

        /// <summary>Offer the player a choice. Waits.</summary>
        Choice,

        /// <summary>Maybe play a short act inline. Rolled once, when reached.</summary>
        Interlude,

        /// <summary>
        /// Open the frame the game has been played inside since act one.
        /// </summary>
        /// <remarks>
        /// Act five's turn, per the design document: Haru takes hold of the
        /// picture and widens it. It is a beat rather than a hard-coded moment
        /// so the act that does it can be moved or rewritten without touching
        /// any code.
        /// </remarks>
        OpenFrame,

        /// <summary>Put the frame back.</summary>
        CloseFrame,

        /// <summary>End of the act.</summary>
        End
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
