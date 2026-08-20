// -----------------------------------------------------------------------------
//  The Frayed Red String
//  AmbientMotionInstaller.cs
//
//  Walks a scene and gives every visible element the idle motion that suits it.
//  The scenes were authored by hand with no scripts attached, so wiring 36
//  GameObjects by Inspector would be both tedious and fragile; classifying them
//  at load time means new art dropped into a scene animates automatically.
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using TheFrayedRedString.Core;
using TheFrayedRedString.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TheFrayedRedString.Motion
{
    /// <summary>
    /// Installs <see cref="AmbientMotion"/> across a whole scene, choosing a
    /// profile per element and honouring an exclusion list.
    /// </summary>
    public static class AmbientMotionInstaller
    {
        /// <summary>
        /// Elements that must never move, no matter what.
        /// </summary>
        /// <remarks>
        /// The two content-warning labels are long-form body text that the
        /// player is expected to actually read. Idle motion on a wall of text
        /// about suicide and abuse would be both hard to read and tonally wrong,
        /// so they are pinned. Excluding a name also prunes everything beneath
        /// it — which conveniently covers the TMP sub-mesh Unity generates as a
        /// child of the Japanese label.
        /// </remarks>
        public static readonly HashSet<string> GloballyExcluded = new HashSet<string>
        {
            ObjectNames.WarningTextEnglish,
            ObjectNames.WarningTextJapanese
        };

        /// <summary>
        /// Explicit profile assignments for the authored elements, applied
        /// before the generic heuristics run.
        /// </summary>
        private static readonly Dictionary<string, AmbientMotionProfile> NamedProfiles =
            new Dictionary<string, AmbientMotionProfile>
            {
                { ObjectNames.MenuBackground, AmbientMotionProfile.Background },
                { ObjectNames.MenuCharacters, AmbientMotionProfile.Character },
                { ObjectNames.MenuTitle, AmbientMotionProfile.Title },
                { ObjectNames.WarningCard, AmbientMotionProfile.Card },
                { ObjectNames.EnterPrompt, AmbientMotionProfile.Prompt },

                // Fitted to its frame with no spare pixels — see FittedArt.
                { ObjectNames.LoadPanelBackground, AmbientMotionProfile.FittedArt },

                // The same, for the two the act scenes' panel carries. A
                // backdrop that drifts while the cards and the heading sitting
                // on it do not is the one thing in a panel that reads as broken:
                // the panel looks like it has come apart. Held still here, and
                // the whole panel is given its own drift instead — see
                // LoadPanelController.
                { ObjectNames.LoadPanelSheet, AmbientMotionProfile.Inset },
                { ObjectNames.LoadPanelCard, AmbientMotionProfile.Inset },
                // Inside a panel and hard against its top edge: the free float
                // the outer title gets lifts this one out of the frame.
                { ObjectNames.LoadPanelTitleArt, AmbientMotionProfile.Inset }
            };

        /// <summary>
        /// Installs idle motion on every eligible element in <paramref name="scene"/>.
        /// </summary>
        /// <param name="scene">Scene to process. Inactive objects are included.</param>
        /// <param name="extraExclusions">Scene-specific names to leave alone.</param>
        /// <returns>How many elements were given motion.</returns>
        public static int Install(Scene scene, IEnumerable<string> extraExclusions = null)
        {
            if (!scene.IsValid() || !scene.isLoaded)
            {
                return 0;
            }

            HashSet<string> exclusions = new HashSet<string>(GloballyExcluded);
            if (extraExclusions != null)
            {
                foreach (string name in extraExclusions)
                {
                    exclusions.Add(name);
                }
            }

            List<GameObject> roots = new List<GameObject>(scene.rootCount);
            scene.GetRootGameObjects(roots);

            int installed = 0;
            for (int i = 0; i < roots.Count; i++)
            {
                installed += InstallRecursive(roots[i].transform, exclusions);
            }

            return installed;
        }

        private static int InstallRecursive(Transform current, HashSet<string> exclusions)
        {
            // An excluded name prunes its entire subtree. Anything nested under a
            // frozen element must stay frozen with it.
            if (exclusions.Contains(current.name))
            {
                return 0;
            }

            int installed = 0;

            if (TryClassify(current, out AmbientMotionProfile profile))
            {
                AmbientMotion motion = UnityUtility.GetOrAdd<AmbientMotion>(current.gameObject);
                motion.Configure(profile, UnityUtility.StableHash01(current.name + current.GetSiblingIndex()));
                installed++;
            }

            for (int i = 0; i < current.childCount; i++)
            {
                installed += InstallRecursive(current.GetChild(i), exclusions);
            }

            return installed;
        }

        /// <summary>
        /// Decides whether an element should move and how.
        /// </summary>
        /// <remarks>
        /// The rules run most-specific first: an explicit name assignment beats
        /// the structural heuristics, which in turn beat the generic fallback.
        /// </remarks>
        private static bool TryClassify(Transform target, out AmbientMotionProfile profile)
        {
            profile = default;

            if (!IsEligible(target))
            {
                return false;
            }

            if (NamedProfiles.TryGetValue(target.name, out AmbientMotionProfile named))
            {
                profile = named;
                return true;
            }

            // Save slots are checked before the generic button rule: they happen
            // to be Buttons, but a 1100×500 card pulsing like a menu button
            // reads as unstable. The slower card hover suits their size.
            if (target.name.StartsWith(ObjectNames.SaveSlotPrefix, System.StringComparison.Ordinal))
            {
                profile = AmbientMotionProfile.Card;
                return true;
            }

            // Interactive controls read as buttons regardless of what art they use.
            if (target.GetComponent<Selectable>() != null)
            {
                profile = AmbientMotionProfile.Button;
                return true;
            }

            if (target.GetComponent<TMP_Text>() != null)
            {
                profile = AmbientMotionProfile.Text;
                return true;
            }

            // A stretched, full-bleed Image is a background whatever it is named.
            if (target is RectTransform rect && IsFullBleed(rect))
            {
                profile = AmbientMotionProfile.Background;
                return true;
            }

            profile = AmbientMotionProfile.Card;
            return true;
        }

        /// <summary>
        /// True when an element is something the player can see and moving it is
        /// safe.
        /// </summary>
        private static bool IsEligible(Transform target)
        {
            GameObject go = target.gameObject;

            // TMP splits a label across extra child meshes when it needs a second
            // atlas — the Japanese warning text has one. Those children are
            // Graphics, but they are positioned by TMP itself; animating them
            // independently would tear the label apart.
            if (go.GetComponent<TMP_SubMeshUI>() != null || go.GetComponent<TMP_SubMesh>() != null)
            {
                return false;
            }

            // Canvas roots: their transform is driven by the CanvasScaler, and a
            // Screen Space Overlay canvas ignores it anyway.
            if (go.GetComponent<Canvas>() != null)
            {
                return false;
            }

            // Stencil masks define a fixed aperture. Breathing the aperture along
            // with its contents cancels the effect out and can flicker the
            // stencil buffer; the masked child animates instead.
            if (go.GetComponent<Mask>() != null || go.GetComponent<RectMask2D>() != null)
            {
                return false;
            }

            // Finally: is there anything to look at?
            bool isVisible = go.GetComponent<Graphic>() != null
                             || go.GetComponent<SpriteRenderer>() != null;

            return isVisible;
        }

        /// <summary>True when a RectTransform is stretched across its whole parent.</summary>
        private static bool IsFullBleed(RectTransform rect)
        {
            const float tolerance = 0.02f;

            return Mathf.Abs(rect.anchorMin.x) < tolerance
                   && Mathf.Abs(rect.anchorMin.y) < tolerance
                   && Mathf.Abs(rect.anchorMax.x - 1f) < tolerance
                   && Mathf.Abs(rect.anchorMax.y - 1f) < tolerance;
        }
    }
}
