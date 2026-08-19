// -----------------------------------------------------------------------------
//  The Frayed Red String
//  SfxId.cs
// -----------------------------------------------------------------------------

namespace TheFrayedRedString.Audio
{
    /// <summary>
    /// Every sound the game can make. All of them are synthesised at runtime by
    /// <see cref="ProceduralSfxLibrary"/> — the project intentionally ships
    /// without sound-effect files.
    /// </summary>
    /// <remarks>
    /// Split into an interface group, which is warmed up at boot because the
    /// player can trigger it on the first frame, and a story group, which is
    /// built when an act loads. See <see cref="ProceduralSfxLibrary.WarmUpCore"/>.
    /// </remarks>
    public enum SfxId
    {
        // --- Interface -------------------------------------------------------

        /// <summary>Soft tap for clicks that do not land on an interactive element.</summary>
        Tap,

        /// <summary>Primary click, played the moment a button is pressed.</summary>
        Click,

        /// <summary>Airy blip when the pointer enters a button.</summary>
        Hover,

        /// <summary>Rising two-tone chime for accepting / advancing.</summary>
        Confirm,

        /// <summary>Falling two-tone chime for closing a panel or going back.</summary>
        Cancel,

        /// <summary>Bright flutter used by the language toggle.</summary>
        Toggle,

        /// <summary>Muted thud for a button that cannot be used right now.</summary>
        Denied,

        // --- Typewriter voices -----------------------------------------------

        /// <summary>
        /// Yua's voice grain. Bright and glassy, struck high on a music box.
        /// </summary>
        /// <remarks>
        /// She is the pastel surface the game wears in act one, so her line
        /// should be the prettiest sound in it. That prettiness is also the
        /// joke: it never changes, no matter what she is actually saying.
        /// </remarks>
        TypeYua,

        /// <summary>
        /// Haru's voice grain. Lower, rounder and woodier, with a longer tail.
        /// </summary>
        TypeHaru,

        /// <summary>Narration. Barely a sound at all — closer to a breath than a note.</summary>
        TypeNarrator,

        // --- Story punctuation -----------------------------------------------

        /// <summary>
        /// A ribbon pulled loose and a lid lifted: paper, then a warm chord
        /// blooming out of it. Used when an act announces itself.
        /// </summary>
        GiftBox,

        /// <summary>
        /// A wish going up and bursting. Four rising bells and a wide scatter of
        /// sparks. Used when the scene changes location.
        /// </summary>
        WishStar,

        /// <summary>Soft paper turn, for the dialogue box arriving or leaving.</summary>
        PageTurn,

        /// <summary>Petals moving in wind. Almost inaudible; pure atmosphere.</summary>
        Petal,

        /// <summary>Two low thuds, lub-dub. Yua's fear given a pulse.</summary>
        Heartbeat,

        /// <summary>
        /// The machine room. A low detuned drone with metal in it, heard through
        /// a floor.
        /// </summary>
        /// <remarks>
        /// The one deliberately ugly sound in the palette. Every other cue in
        /// this game is tuned to the same bright pentatonic; this one is not in
        /// the key, and that is precisely why it registers as wrong before the
        /// player is told anything is wrong.
        /// </remarks>
        BoilerRoom,

        /// <summary>A dull, low throb. Haru's leg, when he stops walking.</summary>
        LegAche,

        // --- Choices ---------------------------------------------------------

        /// <summary>The moment two options appear and the game waits.</summary>
        ChoiceAppear,

        /// <summary>Selecting the kind option. An open, consonant third.</summary>
        ChoiceKind,

        /// <summary>
        /// Selecting the cruel option. The same shape, a major second apart —
        /// sweet enough to pass for the other one, and not quite right.
        /// </summary>
        ChoiceCruel,

        /// <summary>
        /// The red string going taut: Yua taking a kind choice back out of the
        /// player's hands, in act two.
        /// </summary>
        /// <remarks>
        /// The third sound that is not in the key, after the machine room and
        /// the leg. Those two are the costume slipping behind the player's back;
        /// this one is it slipping in front of them, and it should not be sweet.
        /// </remarks>
        StringPull
    }
}
