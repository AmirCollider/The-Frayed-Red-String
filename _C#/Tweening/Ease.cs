// -----------------------------------------------------------------------------
//  The Frayed Red String
//  Ease.cs
//
//  The project ships without a tweening package, so the runtime carries its own
//  minimal easing library. Keeping it dependency-free means the animation code
//  works in a fresh clone with nothing else installed.
// -----------------------------------------------------------------------------

using UnityEngine;

namespace TheFrayedRedString.Tweening
{
    /// <summary>Interpolation curves available to <see cref="TweenRunner"/>.</summary>
    public enum EaseType
    {
        Linear,
        InSine,
        OutSine,
        InOutSine,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        OutExpo,
        InOutExpo,
        OutBack,
        OutElastic
    }

    /// <summary>Evaluates the easing curves declared by <see cref="EaseType"/>.</summary>
    public static class Ease
    {
        private const float BackOvershoot = 1.70158f;

        /// <summary>Maps a normalised 0..1 time to a normalised 0..1 value.</summary>
        public static float Evaluate(EaseType type, float t)
        {
            t = Mathf.Clamp01(t);

            switch (type)
            {
                case EaseType.Linear:
                    return t;

                case EaseType.InSine:
                    return 1f - Mathf.Cos(t * Mathf.PI * 0.5f);

                case EaseType.OutSine:
                    return Mathf.Sin(t * Mathf.PI * 0.5f);

                case EaseType.InOutSine:
                    return -(Mathf.Cos(Mathf.PI * t) - 1f) * 0.5f;

                case EaseType.InQuad:
                    return t * t;

                case EaseType.OutQuad:
                    return 1f - (1f - t) * (1f - t);

                case EaseType.InOutQuad:
                    return t < 0.5f
                        ? 2f * t * t
                        : 1f - Mathf.Pow(-2f * t + 2f, 2f) * 0.5f;

                case EaseType.InCubic:
                    return t * t * t;

                case EaseType.OutCubic:
                    return 1f - Mathf.Pow(1f - t, 3f);

                case EaseType.InOutCubic:
                    return t < 0.5f
                        ? 4f * t * t * t
                        : 1f - Mathf.Pow(-2f * t + 2f, 3f) * 0.5f;

                case EaseType.OutExpo:
                    return Mathf.Approximately(t, 1f) ? 1f : 1f - Mathf.Pow(2f, -10f * t);

                case EaseType.InOutExpo:
                    if (t <= 0f)
                    {
                        return 0f;
                    }

                    if (t >= 1f)
                    {
                        return 1f;
                    }

                    return t < 0.5f
                        ? Mathf.Pow(2f, 20f * t - 10f) * 0.5f
                        : (2f - Mathf.Pow(2f, -20f * t + 10f)) * 0.5f;

                case EaseType.OutBack:
                {
                    const float c3 = BackOvershoot + 1f;
                    float inv = t - 1f;
                    return 1f + c3 * inv * inv * inv + BackOvershoot * inv * inv;
                }

                case EaseType.OutElastic:
                {
                    if (t <= 0f)
                    {
                        return 0f;
                    }

                    if (t >= 1f)
                    {
                        return 1f;
                    }

                    const float period = (2f * Mathf.PI) / 3f;
                    return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * period) + 1f;
                }

                default:
                    return t;
            }
        }
    }
}
