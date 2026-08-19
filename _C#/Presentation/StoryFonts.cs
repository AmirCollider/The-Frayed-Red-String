// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryFonts.cs
// -----------------------------------------------------------------------------

using TMPro;
using UnityDirectTMP;
using UnityEngine;

namespace TheFrayedRedString.Presentation
{
    /// <summary>
    /// Gives a label built at runtime the same font setup the scene's authored
    /// labels already have.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The project draws its text with the UnityDirectTMP package: a
    /// <see cref="DirectFont"/> next to a label points at a .ttf and the label
    /// is rendered from it, with Persian joined and put in reading order by the
    /// font's own OpenType tables. Nothing else in this codebase has to know
    /// anything about scripts, shaping or direction.
    /// </para>
    /// <para>
    /// The catch is that the per-language font slots on that component are
    /// serialized private fields with no public setters — by design, since they
    /// are meant to be filled in the Inspector. So rather than guess at fonts
    /// from code, this copies a label that has already been set up by hand.
    /// Whatever fonts are assigned to the scene's own labels are the fonts the
    /// dialogue box gets, and changing them in the Inspector changes both.
    /// </para>
    /// </remarks>
    public static class StoryFonts
    {
        private static DirectFont _template;
        private static bool _searched;
        private static bool _warned;

        /// <summary>Clears the cached template between play sessions.</summary>
        public static void ResetStatics()
        {
            _template = null;
            _searched = false;
            _warned = false;
        }

        /// <summary>
        /// Uses an explicit label as the source of font settings, instead of
        /// whichever one happens to be found first.
        /// </summary>
        public static void SetTemplate(DirectFont template)
        {
            if (template == null)
            {
                return;
            }

            _template = template;
            _searched = true;
        }

        /// <summary>
        /// Attaches a <see cref="DirectFont"/> to a label and copies the
        /// template's settings onto it.
        /// </summary>
        /// <returns>The component, or <c>null</c> when there was nothing to copy.</returns>
        public static DirectFont Apply(TMP_Text label)
        {
            if (label == null)
            {
                return null;
            }

            DirectFont template = FindTemplate();

            DirectFont direct = label.GetComponent<DirectFont>();
            if (direct == null)
            {
                direct = label.gameObject.AddComponent<DirectFont>();
            }

            if (template == null || template == direct)
            {
                return direct;
            }

            // JsonUtility carries serialized private fields, which is the whole
            // point here — the per-language font slots cannot be reached any
            // other way from code. Object references travel as instance IDs,
            // which are only meaningful inside one session; that is fine,
            // because both components exist in the session doing the copying.
            JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(template), direct);
            direct.Rebuild();

            return direct;
        }

        /// <summary>
        /// Finds a label in the loaded scene that already has its fonts set up.
        /// </summary>
        /// <remarks>
        /// Inactive objects are included on purpose: in an act scene the only
        /// hand-authored labels are the save-slot cards inside a menu panel that
        /// starts hidden, and they are exactly the ones worth copying.
        /// </remarks>
        private static DirectFont FindTemplate()
        {
            if (_searched)
            {
                return _template;
            }

            _searched = true;
            _template = Object.FindAnyObjectByType<DirectFont>(FindObjectsInactive.Include);

            if (_template == null && !_warned)
            {
                _warned = true;
                Debug.LogWarning(
                    "[Fonts] No label with a Direct Font component was found in this scene, so the story " +
                    "interface has no font to copy and will fall back to TextMeshPro's default. " +
                    "Add a Direct Font to any label in the scene — the save-slot text is a good one — " +
                    "and the dialogue box will match it.");
            }

            return _template;
        }
    }
}
