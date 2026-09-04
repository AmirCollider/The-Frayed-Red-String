// -----------------------------------------------------------------------------
//  The Frayed Red String
//  TweenHandle.cs
// -----------------------------------------------------------------------------

using System;

namespace TheFrayedRedString.Tweening
{
    /// <summary>
    /// A cancellable reference to a running tween. Callers keep a handle so that
    /// a new animation can interrupt the previous one instead of fighting it —
    /// the language flip and the load panel both rely on this.
    /// </summary>
    public sealed class TweenHandle
    {
        internal Action CancelRequested;

        /// <summary>True while the tween is still being stepped.</summary>
        public bool IsPlaying { get; internal set; } = true;

        /// <summary>True once the tween reached its final value.</summary>
        public bool IsCompleted { get; internal set; }

        /// <summary>Stops the tween where it is. Safe to call more than once.</summary>
        public void Cancel()
        {
            if (!IsPlaying)
            {
                return;
            }

            IsPlaying = false;
            CancelRequested?.Invoke();
            CancelRequested = null;
        }
    }

    /// <summary>Convenience helpers for nullable handles.</summary>
    public static class TweenHandleExtensions
    {
        /// <summary>Cancels the handle when it exists and is still running.</summary>
        public static void CancelIfRunning(this TweenHandle handle)
        {
            if (handle != null && handle.IsPlaying)
            {
                handle.Cancel();
            }
        }
    }
}
