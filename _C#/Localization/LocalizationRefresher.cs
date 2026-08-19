// -----------------------------------------------------------------------------
//  The Frayed Red String
//  LocalizationRefresher.cs
// -----------------------------------------------------------------------------

using UnityEngine;

namespace TheFrayedRedString.Localization
{
    /// <summary>
    /// Redraws every localised view in the game whenever the language changes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Views also subscribe to <see cref="LocalizationService.LanguageChanged"/>
    /// individually, but that alone is not enough to be correct. A component on
    /// a disabled GameObject has unsubscribed in OnDisable, so it never hears
    /// the event; whether it catches up depends on whether it happens to
    /// refresh on its next OnEnable, and on the order those OnEnables run in.
    /// The symptom is the worst kind: some labels translate and others do not,
    /// varying with what was open at the moment the player pressed the button.
    /// </para>
    /// <para>
    /// This sweep removes that whole class of bug. It walks every loaded object,
    /// inactive ones included, and refreshes anything that implements
    /// <see cref="ILocalizedView"/> — so being visible, enabled or subscribed
    /// stops mattering.
    /// </para>
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class LocalizationRefresher : MonoBehaviour
    {
        /// <summary>Attaches the refresher to the persistent service host.</summary>
        public static LocalizationRefresher Install(GameObject host)
        {
            if (host == null)
            {
                return null;
            }

            LocalizationRefresher existing = host.GetComponent<LocalizationRefresher>();
            return existing != null ? existing : host.AddComponent<LocalizationRefresher>();
        }

        /// <summary>
        /// Refreshes every localised view currently loaded, active or not.
        /// </summary>
        /// <returns>How many views were refreshed.</returns>
        public static int RefreshAll()
        {
            // Sweeping MonoBehaviour and filtering by interface costs one pass
            // over the scene's components. That is a few dozen objects in this
            // game, on an action the player takes by hand — and in exchange any
            // component that implements ILocalizedView is covered automatically,
            // with no registration step to forget.
            MonoBehaviour[] behaviours = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include);

            int refreshed = 0;

            for (int i = 0; i < behaviours.Length; i++)
            {
                if (behaviours[i] is ILocalizedView view)
                {
                    view.Refresh();
                    refreshed++;
                }
            }

            return refreshed;
        }

        private void OnEnable()
        {
            LocalizationService.LanguageChanged += OnLanguageChanged;
        }

        private void OnDisable()
        {
            LocalizationService.LanguageChanged -= OnLanguageChanged;
        }

        private void OnLanguageChanged(GameLanguage language)
        {
            RefreshAll();
        }
    }
}
