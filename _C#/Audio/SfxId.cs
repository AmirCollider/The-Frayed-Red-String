// -----------------------------------------------------------------------------
//  The Frayed Red String
//  SfxId.cs
// -----------------------------------------------------------------------------

namespace TheFrayedRedString.Audio
{
    /// <summary>
    /// Every sound the UI can make. All of them are synthesised at runtime by
    /// <see cref="ProceduralSfxLibrary"/> — the project intentionally ships
    /// without audio files.
    /// </summary>
    public enum SfxId
    {
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
        Denied
    }
}
