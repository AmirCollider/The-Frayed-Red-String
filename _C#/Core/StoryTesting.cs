// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryTesting.cs
// -----------------------------------------------------------------------------

using UnityEngine;

namespace TheFrayedRedString.Core
{
    /// <summary>
    /// The one thing about this game that cannot be tested by playing it.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Two of the three endings are behind five real minutes of not touching
    /// anything, twice — which is right for a player and impossible for whoever
    /// has to check that the branch works. Nobody is going to sit through three
    /// hundred seconds forty times, so in practice the feature would be tested
    /// once, badly, and then trusted.
    /// </para>
    /// <para>
    /// So the wait is shortenable from the Editor and nowhere else. The switch
    /// lives in EditorPrefs, is read only inside <c>UNITY_EDITOR</c>, and
    /// compiles to a constant zero in a build — there is no way for a shipped
    /// copy of the game to have a short five minutes, whatever anybody clicks.
    /// </para>
    /// <para>
    /// It also announces itself in the console on every play, because a
    /// half-second patience threshold left on by accident makes every other
    /// thing you test that day behave strangely for no visible reason.
    /// </para>
    /// </remarks>
    public static class StoryTesting
    {
#if UNITY_EDITOR
        /// <summary>EditorPrefs key holding the shortened wait, in seconds.</summary>
        public const string PatienceKey = "TFRS.Test.PatienceSeconds";

        private static bool _announced;

        /// <summary>
        /// The shortened wait, or zero when the real five minutes are in force.
        /// </summary>
        public static float PatienceSecondsOverride
        {
            get
            {
                float seconds = UnityEditor.EditorPrefs.GetFloat(PatienceKey, 0f);

                if (seconds > 0f && !_announced && Application.isPlaying)
                {
                    _announced = true;

                    Debug.LogWarning(
                        $"[Testing] The five minutes of patience are shortened to {seconds:0.#}s for this " +
                        "Editor session. The endings can be reached almost immediately. Turn it off from " +
                        "The Frayed Red String ▸ Endings ▸ Use The Real Five Minutes. Builds are never " +
                        "affected.");
                }

                return seconds;
            }

            set
            {
                UnityEditor.EditorPrefs.SetFloat(PatienceKey, Mathf.Max(0f, value));
                _announced = false;
            }
        }

        /// <summary>True while the wait is shortened.</summary>
        public static bool IsShortened => PatienceSecondsOverride > 0f;
#else
        /// <summary>Always zero outside the Editor. See the remarks above.</summary>
        public static float PatienceSecondsOverride => 0f;

        /// <summary>Always false outside the Editor.</summary>
        public static bool IsShortened => false;
#endif
    }
}
