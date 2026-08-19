// -----------------------------------------------------------------------------
//  The Frayed Red String
//  EntranceAnimator.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Tweening;
using TheFrayedRedString.Utilities;
using UnityEngine;

namespace TheFrayedRedString.Presentation
{
    /// <summary>
    /// Staggered fade-ins for the elements of a scene.
    /// </summary>
    /// <remarks>
    /// Entrances animate opacity only. Idle motion owns every transform in the
    /// game (see <c>AmbientMotion</c>), and two systems writing the same
    /// transform is how animation code starts to stutter. Keeping entrances on
    /// the alpha channel means a menu element can arrive and breathe at the same
    /// time without either effect knowing about the other.
    /// </remarks>
    public static class EntranceAnimator
    {
        /// <summary>Default length of one element's fade.</summary>
        public const float DefaultDuration = 0.75f;

        /// <summary>Default gap between consecutive elements in a sequence.</summary>
        public const float DefaultStagger = 0.12f;

        /// <summary>
        /// Fades a single element up from invisible.
        /// </summary>
        /// <param name="target">Element to fade. Ignored when null.</param>
        /// <param name="delay">Seconds to wait before starting.</param>
        /// <param name="duration">Length of the fade.</param>
        public static void FadeIn(GameObject target, float delay, float duration = DefaultDuration)
        {
            if (target == null)
            {
                return;
            }

            CanvasGroup group = UnityUtility.GetOrAdd<CanvasGroup>(target);
            group.alpha = 0f;

            // An invisible button must not be clickable. Interaction is restored
            // the moment the element becomes visible.
            group.blocksRaycasts = false;
            group.interactable = false;

            TweenRunner.Play(
                duration,
                t =>
                {
                    if (group != null)
                    {
                        group.alpha = t;
                    }
                },
                EaseType.OutCubic,
                delay,
                () =>
                {
                    if (group == null)
                    {
                        return;
                    }

                    group.alpha = 1f;
                    group.blocksRaycasts = true;
                    group.interactable = true;
                },
                group);
        }

        /// <summary>
        /// Fades a list of elements up one after another, in the order given.
        /// </summary>
        /// <param name="targets">Elements to fade. Null entries are skipped without consuming a beat.</param>
        /// <param name="startDelay">Delay before the first element.</param>
        /// <param name="stagger">Gap between consecutive elements.</param>
        /// <param name="duration">Length of each element's fade.</param>
        public static void FadeInSequence(
            GameObject[] targets,
            float startDelay = 0f,
            float stagger = DefaultStagger,
            float duration = DefaultDuration)
        {
            if (targets == null)
            {
                return;
            }

            int step = 0;
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i] == null)
                {
                    continue;
                }

                FadeIn(targets[i], startDelay + step * stagger, duration);
                step++;
            }
        }
    }
}
