// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryText.cs
// -----------------------------------------------------------------------------

using TMPro;

namespace TheFrayedRedString.Presentation
{
    /// <summary>
    /// Writes text to a label.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is a one-line wrapper, and it earns its place by being the seam
    /// where text handling used to live. An earlier version of this project
    /// carried a whole Persian layer here — glyph shaping, bidi reordering,
    /// line breaking, a font-swapping component and a font-asset generator —
    /// none of which was needed. The UnityDirectTMP package already on every
    /// label does all of it from the font's own OpenType tables, and does it
    /// better than a hand-written presentation-form table ever could.
    /// </para>
    /// <para>
    /// So there is nothing left to do but assign the string, and it is worth
    /// having one obvious place that says so. Text is handed to labels in
    /// reading order, in every language, and the label draws it correctly.
    /// </para>
    /// </remarks>
    public static class StoryText
    {
        /// <summary>Assigns text to a label, tolerating a null label.</summary>
        public static void Set(TMP_Text label, string text)
        {
            if (label != null)
            {
                label.text = text ?? string.Empty;
            }
        }
    }
}
