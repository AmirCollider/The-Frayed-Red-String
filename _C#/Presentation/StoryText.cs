// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryText.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Localization;
using TMPro;

namespace TheFrayedRedString.Presentation
{
    /// <summary>
    /// Writes text to a label, and makes sure it is drawn with the right font
    /// and the right weight.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This was a one-line wrapper around <c>label.text = value</c>, on the
    /// grounds that UnityDirectTMP already does everything else. It does — but
    /// not necessarily in the frame the text was assigned, and that turned out
    /// to matter a great deal.
    /// </para>
    /// <para>
    /// The package picks a font by scanning the text for its writing system, and
    /// it does that scan from <c>LateUpdate</c>. So a label whose text changes
    /// from English to Persian is still holding the English font asset when the
    /// canvas draws it that frame — and TextMeshPro, having drawn it, has no
    /// reason to draw it again. The result is a line of Persian rendered by a
    /// font with no Persian in it: unjoined at best, a row of empty boxes at
    /// worst, and permanent either way until something else happens to dirty the
    /// label.
    /// </para>
    /// <para>
    /// Every language change in this game goes through here, so here is where it
    /// is fixed: assign, then ask the package to build the font for the new
    /// script now rather than next frame, then make the label lay itself out
    /// again with it.
    /// </para>
    /// </remarks>
    public static class StoryText
    {
        /// <summary>Assigns text to a label, tolerating a null label.</summary>
        public static void Set(TMP_Text label, string text)
        {
            if (label == null)
            {
                return;
            }

            label.text = text ?? string.Empty;

            ApplyWeight(label);
            StoryFonts.Refresh(label);
        }

        /// <summary>
        /// Sets the label's weight from the language it is about to draw.
        /// </summary>
        /// <remarks>
        /// English and Japanese are set bold; Persian is not. That is not a
        /// preference, it is what the fonts can do. TextMeshPro fakes a bold
        /// face it has not got by widening every glyph, and widening joined
        /// Arabic script thickens the joins until the letters run together — so
        /// the language that most needs its shapes intact is the one that has to
        /// stay at the weight it was drawn at.
        /// </remarks>
        public static void ApplyWeight(TMP_Text label)
        {
            if (label == null)
            {
                return;
            }

            FontStyles wanted = LocalizationService.Current.IsRightToLeft()
                ? label.fontStyle & ~FontStyles.Bold
                : label.fontStyle | FontStyles.Bold;

            if (label.fontStyle != wanted)
            {
                label.fontStyle = wanted;
            }
        }
    }
}
