// -----------------------------------------------------------------------------
//  The Frayed Red String
//  TweenRunner.cs
// -----------------------------------------------------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace TheFrayedRedString.Tweening
{
    /// <summary>
    /// Persistent coroutine host that drives every tween in the game.
    /// </summary>
    /// <remarks>
    /// Tweens run on unscaled time by default: menus and transitions must keep
    /// animating even while <see cref="Time.timeScale"/> is zero.
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class TweenRunner : MonoBehaviour
    {
        private static TweenRunner _instance;

        /// <summary>
        /// The shared runner, created on demand so tweens can be started from
        /// anywhere without worrying about boot order.
        /// </summary>
        public static TweenRunner Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                GameObject host = new GameObject("[TFRS] Tween Runner");
                DontDestroyOnLoad(host);
                _instance = host.AddComponent<TweenRunner>();
                return _instance;
            }
        }

        /// <summary>Attaches the runner to an existing service host.</summary>
        public static TweenRunner Install(GameObject host)
        {
            if (_instance == null && host != null)
            {
                _instance = host.AddComponent<TweenRunner>();
            }

            return _instance;
        }

        /// <summary>Clears the cached instance between play sessions.</summary>
        public static void ResetStatics()
        {
            _instance = null;
        }

        /// <summary>
        /// Interpolates 0..1 over <paramref name="duration"/> seconds, invoking
        /// <paramref name="onUpdate"/> with the eased value every frame.
        /// </summary>
        /// <param name="duration">Length in seconds. Zero applies the end value immediately.</param>
        /// <param name="onUpdate">Receives the eased 0..1 progress.</param>
        /// <param name="ease">Curve applied to the raw progress.</param>
        /// <param name="delay">Seconds to wait before the first step.</param>
        /// <param name="onComplete">Invoked after the final step, never on cancel.</param>
        /// <param name="owner">
        /// Optional lifetime guard. When the owner is destroyed the tween stops
        /// instead of writing to a dead object.
        /// </param>
        /// <param name="useUnscaledTime">Ignore <see cref="Time.timeScale"/>.</param>
        public static TweenHandle Play(
            float duration,
            Action<float> onUpdate,
            EaseType ease = EaseType.OutCubic,
            float delay = 0f,
            Action onComplete = null,
            UnityEngine.Object owner = null,
            bool useUnscaledTime = true)
        {
            TweenHandle handle = new TweenHandle();

            // A tween with no per-frame callback is still a valid timer — that
            // is exactly what After() is. Substituting a no-op keeps the delay
            // and completion path intact.
            onUpdate ??= NoOp;

            TweenRunner runner = Instance;
            Coroutine routine = runner.StartCoroutine(
                Step(handle, duration, onUpdate, ease, delay, onComplete, owner, useUnscaledTime));

            handle.CancelRequested = () =>
            {
                if (runner != null && routine != null)
                {
                    runner.StopCoroutine(routine);
                }
            };

            return handle;
        }

        /// <summary>Invokes <paramref name="action"/> after a delay.</summary>
        public static TweenHandle After(float delay, Action action, UnityEngine.Object owner = null)
        {
            return Play(0f, null, EaseType.Linear, delay, action, owner);
        }

        private static void NoOp(float _)
        {
        }

        private static IEnumerator Step(
            TweenHandle handle,
            float duration,
            Action<float> onUpdate,
            EaseType ease,
            float delay,
            Action onComplete,
            UnityEngine.Object owner,
            bool useUnscaledTime)
        {
            bool hasOwner = owner != null;

            if (delay > 0f)
            {
                float waited = 0f;
                while (waited < delay)
                {
                    if (hasOwner && owner == null)
                    {
                        handle.IsPlaying = false;
                        yield break;
                    }

                    waited += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                    yield return null;
                }
            }

            if (hasOwner && owner == null)
            {
                handle.IsPlaying = false;
                yield break;
            }

            if (duration > 0f)
            {
                float elapsed = 0f;
                while (elapsed < duration)
                {
                    if (hasOwner && owner == null)
                    {
                        handle.IsPlaying = false;
                        yield break;
                    }

                    onUpdate(Ease.Evaluate(ease, elapsed / duration));

                    elapsed += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                    yield return null;
                }
            }

            if (hasOwner && owner == null)
            {
                handle.IsPlaying = false;
                yield break;
            }

            onUpdate(1f);

            handle.IsPlaying = false;
            handle.IsCompleted = true;
            handle.CancelRequested = null;

            onComplete?.Invoke();
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
                return;
            }

            _instance = this;
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}
