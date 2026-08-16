// -----------------------------------------------------------------------------
//  The Frayed Red String
//  FadeExtensions.cs
// -----------------------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrayedRedString.Tweening
{
    /// <summary>
    /// Opacity tweens for the three renderer families the game uses:
    /// <see cref="CanvasGroup"/>, uGUI <see cref="Graphic"/> and
    /// <see cref="SpriteRenderer"/>.
    /// </summary>
    /// <remarks>
    /// Fades deliberately never touch a transform. Idle motion is owned
    /// exclusively by <c>AmbientMotion</c>, so keeping the two concerns on
    /// separate channels means an entrance animation can never fight the
    /// breathing loop.
    /// </remarks>
    public static class FadeExtensions
    {
        /// <summary>Fades a canvas group to <paramref name="target"/> alpha.</summary>
        public static TweenHandle FadeTo(
            this CanvasGroup group,
            float target,
            float duration,
            EaseType ease = EaseType.OutCubic,
            float delay = 0f,
            Action onComplete = null)
        {
            if (group == null)
            {
                onComplete?.Invoke();
                return null;
            }

            float from = group.alpha;

            return TweenRunner.Play(
                duration,
                t =>
                {
                    if (group != null)
                    {
                        group.alpha = Mathf.LerpUnclamped(from, target, t);
                    }
                },
                ease,
                delay,
                onComplete,
                group);
        }

        /// <summary>Fades a uGUI graphic (Image or TextMeshProUGUI) to <paramref name="target"/> alpha.</summary>
        public static TweenHandle FadeTo(
            this Graphic graphic,
            float target,
            float duration,
            EaseType ease = EaseType.OutCubic,
            float delay = 0f,
            Action onComplete = null)
        {
            if (graphic == null)
            {
                onComplete?.Invoke();
                return null;
            }

            float from = graphic.color.a;

            return TweenRunner.Play(
                duration,
                t =>
                {
                    if (graphic == null)
                    {
                        return;
                    }

                    Color color = graphic.color;
                    color.a = Mathf.LerpUnclamped(from, target, t);
                    graphic.color = color;
                },
                ease,
                delay,
                onComplete,
                graphic);
        }

        /// <summary>Fades a sprite renderer to <paramref name="target"/> alpha.</summary>
        public static TweenHandle FadeTo(
            this SpriteRenderer renderer,
            float target,
            float duration,
            EaseType ease = EaseType.OutCubic,
            float delay = 0f,
            Action onComplete = null)
        {
            if (renderer == null)
            {
                onComplete?.Invoke();
                return null;
            }

            float from = renderer.color.a;

            return TweenRunner.Play(
                duration,
                t =>
                {
                    if (renderer == null)
                    {
                        return;
                    }

                    Color color = renderer.color;
                    color.a = Mathf.LerpUnclamped(from, target, t);
                    renderer.color = color;
                },
                ease,
                delay,
                onComplete,
                renderer);
        }

        /// <summary>Sets a sprite renderer's alpha without animating.</summary>
        public static void SetAlpha(this SpriteRenderer renderer, float alpha)
        {
            if (renderer == null)
            {
                return;
            }

            Color color = renderer.color;
            color.a = alpha;
            renderer.color = color;
        }

        /// <summary>Sets a graphic's alpha without animating.</summary>
        public static void SetAlpha(this Graphic graphic, float alpha)
        {
            if (graphic == null)
            {
                return;
            }

            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }
}
