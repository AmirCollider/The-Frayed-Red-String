// -----------------------------------------------------------------------------
//  The Frayed Red String
//  EndingTools.cs  (Editor only)
//
//  How to test the two endings without sitting still for five minutes.
// -----------------------------------------------------------------------------

using TheFrayedRedString.Core;
using TheFrayedRedString.Narrative;
using UnityEditor;
using UnityEngine;

namespace TheFrayedRedString.EditorTools
{
    /// <summary>Menu items for reaching an ending on purpose.</summary>
    public static class EndingTools
    {
        private const string Menu = "The Frayed Red String/Endings/";

        [MenuItem(Menu + "Shorten The Five Minutes To 10 Seconds", priority = 0)]
        public static void ShortenPatience()
        {
            StoryTesting.PatienceSecondsOverride = 10f;

            Debug.Log(
                "[Testing] Ten seconds instead of five minutes, in the Editor only.\n" +
                "  Play any act, reach a line marked as one of the silences — Haru's leg, or the machine " +
                "room — and stop touching anything.\n" +
                "  An offer appears only on a run that has pressed one colour and never the other: all " +
                "blue gives Yua's ending, all green gives Haru's, and one of each gives nothing at all.");
        }

        [MenuItem(Menu + "Use The Real Five Minutes", priority = 1)]
        public static void RestorePatience()
        {
            StoryTesting.PatienceSecondsOverride = 0f;
            Debug.Log("[Testing] The five minutes are back to five minutes.");
        }

        [MenuItem(Menu + "Use The Real Five Minutes", validate = true)]
        private static bool CanRestore()
        {
            return StoryTesting.IsShortened;
        }

        /// <summary>
        /// Says which ending the run in progress would be offered, and why.
        /// </summary>
        /// <remarks>
        /// The condition is one fact about the entire playthrough and there is
        /// nothing on screen that reports it — deliberately, because a counter
        /// would turn the whole mechanic into a score. This is the version for
        /// whoever has to check it works.
        /// </remarks>
        [MenuItem(Menu + "What Ending Is This Run Earning?", priority = 20)]
        public static void ReportRun()
        {
            if (!Application.isPlaying)
            {
                Debug.Log("[Testing] Press Play first — a run has to exist before it can be earning anything.");
                return;
            }

            string wanted = Endings.AssetNameForCurrentRun();
            ActAsset ending = wanted == null ? null : ActLibrary.FindByName(wanted);

            string verdict;

            if (wanted == null)
            {
                verdict = StorySession.HasMixedChoices
                    ? "nothing. Both colours have been pressed, so no offer will ever appear on this run " +
                      "however long anybody waits. This is the bitter ending, and it is the default."
                    : "nothing yet. No counted choice has been made — white options are not counted — so " +
                      "there is no run to qualify.";
            }
            else
            {
                verdict =
                    $"{(wanted == Endings.GoodAssetName ? "Yua speaking (the secret ending)" : "Haru speaking (the open ending)")}, " +
                    $"and that act is {(ending != null && ending.Count > 0 ? "written" : "MISSING — build it from this menu")}. " +
                    "It still has to be redeemed by sitting through one of the measured silences: " +
                    $"{GameConfig.PatienceSeconds:0.#}s right now.";
            }

            Debug.Log(
                $"[Testing] Act {StorySession.ActNumber:00}, beat {StorySession.LineIndex:000}.\n" +
                $"  blue pressed  : {StorySession.KindChoices}\n" +
                $"  green pressed : {StorySession.CruelChoices}\n" +
                $"  silences sat through : {StorySession.PatientMoments}\n" +
                $"  → {verdict}");
        }

        /// <summary>
        /// Puts the run in progress into a state that qualifies, without having
        /// to play the choices.
        /// </summary>
        /// <remarks>
        /// Reaching a qualifying run legitimately means pressing the same colour
        /// at every choice from act two onward and never slipping once, which is
        /// twenty minutes of careful play before the thing being tested even
        /// becomes reachable. These two set the counters directly.
        /// </remarks>
        [MenuItem(Menu + "Pretend This Run Pressed Only Blue", priority = 21)]
        public static void ForcePureKind()
        {
            if (!RequirePlaying())
            {
                return;
            }

            StorySession.OverrideChoiceCounts(6, 0);
            Debug.Log("[Testing] This run now counts as all blue. It qualifies for Yua's ending.");
        }

        [MenuItem(Menu + "Pretend This Run Pressed Only Green", priority = 22)]
        public static void ForcePureCruel()
        {
            if (!RequirePlaying())
            {
                return;
            }

            StorySession.OverrideChoiceCounts(0, 6);
            Debug.Log("[Testing] This run now counts as all green. It qualifies for Haru's ending.");
        }

        [MenuItem(Menu + "Pretend This Run Pressed Both", priority = 23)]
        public static void ForceMixed()
        {
            if (!RequirePlaying())
            {
                return;
            }

            StorySession.OverrideChoiceCounts(3, 3);
            Debug.Log(
                "[Testing] This run now counts as mixed, which is the common case: no offer will appear " +
                "however long anybody waits, and the game ends on act seven.");
        }

        private static bool RequirePlaying()
        {
            if (Application.isPlaying)
            {
                return true;
            }

            Debug.Log("[Testing] Press Play first — there is no run to change yet.");
            return false;
        }

        [MenuItem(Menu + "Build The Two Endings", priority = 40)]
        public static void BuildEndings()
        {
            EndingsBuilder.BuildBoth();
        }
    }
}
