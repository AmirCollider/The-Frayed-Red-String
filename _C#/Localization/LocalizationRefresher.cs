// -----------------------------------------------------------------------------
//  The Frayed Red String
//  LocalizationRefresher.cs
// -----------------------------------------------------------------------------

using System.Collections;
using TheFrayedRedString.Presentation;
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
        /// <summary>
        /// How many frames the fonts are re-applied for after a change.
        /// </summary>
        /// <remarks>
        /// One would do if the font for a script were always ready the instant
        /// it is asked for. It is not: the package builds a font asset the frame
        /// after it sees the script change, and a large face — the Japanese one
        /// here is seven and a half megabytes — can take a moment more to
        /// rasterise the glyphs a wall of text needs. A label drawn in that gap
        /// is drawn with what was available at the time and never redrawn.
        /// Four frames is cheap and covers it.
        /// </remarks>
        private const int SettleFrames = 4;

        private static LocalizationRefresher _instance;

        private Coroutine _settling;

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
        /// Re-applies every label's font over the next few frames.
        /// </summary>
        /// <remarks>
        /// For the two moments when every label on screen may have changed the
        /// script it is drawing at once: the player switching language, and a
        /// scene that has just loaded. Safe to call when there is no refresher —
        /// it simply does nothing, which is the right answer before boot.
        /// </remarks>
        public static void Settle()
        {
            if (_instance == null)
            {
                return;
            }

            if (_instance._settling != null)
            {
                _instance.StopCoroutine(_instance._settling);
            }

            _instance._settling = _instance.StartCoroutine(_instance.SettleRoutine());
        }

        private IEnumerator SettleRoutine()
        {
            for (int frame = 0; frame < SettleFrames; frame++)
            {
                // End of frame, so this runs after the package's own LateUpdate
                // has had its turn at deciding which font the text wants.
                yield return new WaitForEndOfFrame();

                StoryFonts.RefreshAll();
            }

            _settling = null;
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
            _instance = this;
            LocalizationService.LanguageChanged += OnLanguageChanged;
        }

        private void OnDisable()
        {
            if (_instance == this)
            {
                _instance = null;
            }

            LocalizationService.LanguageChanged -= OnLanguageChanged;
        }

        private void OnLanguageChanged(GameLanguage language)
        {
            RefreshAll();

            // Every label on screen has just been handed text in a different
            // writing system. The fonts for it are built a frame from now.
            Settle();
        }
    }
}
