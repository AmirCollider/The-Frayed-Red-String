// -----------------------------------------------------------------------------
//  The Frayed Red String
//  ILocalizedView.cs
// -----------------------------------------------------------------------------

namespace TheFrayedRedString.Localization
{
    /// <summary>
    /// Anything on screen that has to be redrawn when the language changes.
    /// </summary>
    /// <remarks>
    /// Implementing this is what gets a component swept by
    /// <see cref="LocalizationRefresher"/>. Components may also subscribe to
    /// <see cref="LocalizationService.LanguageChanged"/> directly for their own
    /// reasons, but they should not rely on that subscription being live: a
    /// component on a disabled GameObject is unsubscribed, and would otherwise
    /// come back showing the previous language.
    /// </remarks>
    public interface ILocalizedView
    {
        /// <summary>
        /// Re-applies the current language to this view.
        /// </summary>
        /// <remarks>
        /// Must be safe to call at any time, including before the component has
        /// been initialised by its owner and while its GameObject is inactive.
        /// </remarks>
        void Refresh();
    }
}
