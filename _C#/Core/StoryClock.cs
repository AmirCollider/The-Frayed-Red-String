// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryClock.cs
// -----------------------------------------------------------------------------

using System.Collections;
using UnityEngine;

namespace TheFrayedRedString.Core
{
    /// <summary>
    /// The clock the story runs on, and the one thing that can stop it.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Pausing this game cannot be done with <see cref="Time.timeScale"/>.
    /// Everything here already runs on unscaled time on purpose — the menus have
    /// to keep breathing while the story is held — so zeroing the time scale
    /// pauses precisely nothing. That is why the pause menu used to come up over
    /// a game that carried straight on typing underneath it.
    /// </para>
    /// <para>
    /// So the story gets a clock of its own. It reads unscaled time and returns
    /// zero while the game is paused, and everything that belongs to the story
    /// rather than to the interface asks it for the time instead of asking
    /// Unity: the typewriter, every wait in the director, the overlays, and the
    /// tweens that move the picture.
    /// </para>
    /// <para>
    /// What deliberately does not use it: the fade curtain, the pause menu's own
    /// animation, and the idle motion under everything. A paused game that also
    /// freezes the menu you paused it with reads as a crash.
    /// </para>
    /// </remarks>
    public static class StoryClock
    {
        /// <summary>True while the story is held.</summary>
        public static bool IsPaused { get; set; }

        /// <summary>Seconds since the last frame, or zero while paused.</summary>
        public static float DeltaTime => IsPaused ? 0f : Time.unscaledDeltaTime;

        /// <summary>Clears static state between play sessions.</summary>
        public static void ResetStatics()
        {
            IsPaused = false;
        }

        /// <summary>
        /// Waits out a number of story seconds — which is to say, not at all
        /// while the game is paused.
        /// </summary>
        /// <remarks>
        /// The replacement for <c>WaitForSecondsRealtime</c> anywhere the story
        /// is what is waiting. A three-second hold on a picture is three seconds
        /// the player chose to spend looking at it, and it should not be spent
        /// while they are in a menu.
        /// </remarks>
        public static IEnumerator Wait(float seconds)
        {
            float remaining = Mathf.Max(0f, seconds);

            while (remaining > 0f)
            {
                yield return null;
                remaining -= DeltaTime;
            }
        }
    }
}
