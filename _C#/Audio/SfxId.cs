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
    /// <remarks>
    /// Pinned to explicit numbers for the same reason as
    /// <see cref="TheFrayedRedString.Narrative.StoryBeatKind"/>: a beat stores
    /// its sound as an integer, so inserting a value in the middle re-points
    /// every cue in every act already written. Add new sounds at the end and
    /// give them the next number.
    /// </remarks>
    public enum SfxId
    {
        // --- Interface -------------------------------------------------------

        /// <summary>Soft tap for clicks that do not land on an interactive element.</summary>
        Tap = 0,
        /// <summary>Primary click, played the moment a button is pressed.</summary>
        Click = 1,
        /// <summary>Airy blip when the pointer enters a button.</summary>
        Hover = 2,
        /// <summary>Rising two-tone chime for accepting / advancing.</summary>
        Confirm = 3,
        /// <summary>Falling two-tone chime for closing a panel or going back.</summary>
        Cancel = 4,
        /// <summary>Bright flutter used by the language toggle.</summary>
        Toggle = 5,
        /// <summary>Muted thud for a button that cannot be used right now.</summary>
        Denied = 6,
        // --- Typewriter voices -----------------------------------------------

        /// <summary>
        /// Yua's voice grain. Bright and glassy, struck high on a music box.
        /// </summary>
        /// <remarks>
        /// She is the pastel surface the game wears in act one, so her line
        /// should be the prettiest sound in it. That prettiness is also the
        /// joke: it never changes, no matter what she is actually saying.
        /// </remarks>
        TypeYua = 7,
        /// <summary>
        /// Haru's voice grain. Lower, rounder and woodier, with a longer tail.
        /// </summary>
        TypeHaru = 8,
        /// <summary>Narration. Barely a sound at all — closer to a breath than a note.</summary>
        TypeNarrator = 9,
        // --- Story punctuation -----------------------------------------------

        /// <summary>
        /// A ribbon pulled loose and a lid lifted: paper, then a warm chord
        /// blooming out of it. Used when an act announces itself.
        /// </summary>
        GiftBox = 10,
        /// <summary>
        /// A wish going up and bursting. Four rising bells and a wide scatter of
        /// sparks. Used when the scene changes location.
        /// </summary>
        WishStar = 11,
        /// <summary>Soft paper turn, for the dialogue box arriving or leaving.</summary>
        PageTurn = 12,
        /// <summary>Petals moving in wind. Almost inaudible; pure atmosphere.</summary>
        Petal = 13,
        /// <summary>Two low thuds, lub-dub. Yua's fear given a pulse.</summary>
        Heartbeat = 14,
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
        BoilerRoom = 15,
        /// <summary>A dull, low throb. Haru's leg, when he stops walking.</summary>
        LegAche = 16,
        // --- Choices ---------------------------------------------------------

        /// <summary>The moment two options appear and the game waits.</summary>
        ChoiceAppear = 17,
        /// <summary>Selecting the kind option. An open, consonant third.</summary>
        ChoiceKind = 18,
        /// <summary>
        /// Selecting the cruel option. The same shape, a major second apart —
        /// sweet enough to pass for the other one, and not quite right.
        /// </summary>
        ChoiceCruel = 19,
        // --- Act five ---------------------------------------------------------

        /// <summary>
        /// The frame and the veil coming off. Glass giving way.
        /// </summary>
        /// <remarks>
        /// The act is named after it. Not a shatter of falling shards — a single
        /// hard fracture, because what breaks in this act is one thing, once.
        /// </remarks>
        GlassBreak = 21,
        /// <summary>
        /// The last sound in act five.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The design document shows nothing at all here: Haru opens one hand
        /// like a butterfly, this plays, and the scene cuts. So it is the only
        /// thing carrying the moment and it has to be the right shape — a flat
        /// crack with no musical content whatsoever, and a long tail in a
        /// corridor, so the silence afterwards is a place rather than an absence.
        /// </para>
        /// <para>
        /// Deliberately not loud in the palette's terms. A sound that clips
        /// makes the player reach for the volume, which is the one thought that
        /// must not be in the room when this happens.
        /// </para>
        /// </remarks>
        Gunshot = 22,
        /// <summary>
        /// The red string going taut: Yua taking a kind choice back out of the
        /// player's hands, in act two.
        /// </summary>
        /// <remarks>
        /// The third sound that is not in the key, after the machine room and
        /// the leg. Those two are the costume slipping behind the player's back;
        /// this one is it slipping in front of them, and it should not be sweet.
        /// </remarks>
        StringPull = 20,

        // ---------------------------------------------------------------------
        //  Act one's room tone, such as it is
        //
        //  Five ordinary noises, added because the rewritten act one cues them
        //  and cueing something that does not exist is how a script ends up
        //  describing a game nobody built. All five are synthesised from the
        //  same recipe vocabulary as everything above — there is not a single
        //  audio file in this project and there is not meant to be.
        //
        //  Deliberately not here: rain on glass and the murmur of a classroom.
        //  Both are continuous beds rather than events, and the game has no
        //  ambience bus to put a bed on — a Music beat would stop them and a
        //  one-shot would end mid-scene. Faking them as four-second one-shots
        //  would be worse than their absence.
        // ---------------------------------------------------------------------

        /// <summary>A chair pushed back on a wooden floor.</summary>
        /// <remarks>
        /// The most ordinary sound in a school and the reason it is here: act
        /// one needs somebody to sit down audibly, twice, in a scene where
        /// sitting down is the whole event.
        /// </remarks>
        ChairScrape = 23,

        /// <summary>A knee catching the underside of a desk.</summary>
        /// <remarks>
        /// Act one, day one, and the one moment the player is meant to feel
        /// before they understand it. Blunt and low and over immediately — the
        /// sound of furniture, not the sound of pain. What it means arrives five
        /// acts later.
        /// </remarks>
        DeskKnock = 24,

        /// <summary>The end-of-period bell.</summary>
        /// <remarks>
        /// Rings four times across act one and is the act's only clock. Written
        /// as two struck partials a fifth or so apart rather than as a chime,
        /// because a school bell is a cheap electric thing and should sound like
        /// one next to the music box everything else is built from.
        /// </remarks>
        SchoolBell = 25,

        /// <summary>A vending machine taking a coin and delivering nothing.</summary>
        /// <remarks>
        /// The running argument of act one hangs off this machine, and the joke
        /// only works if the player has heard it swallow something.
        /// </remarks>
        VendingThunk = 26,

        /// <summary>A can landing in the tray.</summary>
        /// <remarks>
        /// Two impacts about a tenth of a second apart — the drop and the
        /// settle. The scene where two of them come out at once plays this
        /// twice, and that is the entire punchline.
        /// </remarks>
        CanDrop = 27
    }
}
