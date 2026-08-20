// -----------------------------------------------------------------------------
//  The Frayed Red String
//  Endings.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Localization;
using UnityEngine;

namespace TheFrayedRedString.Narrative
{
    /// <summary>
    /// Which ending a playthrough has earned, and the one sentence that offers
    /// it.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The whole architecture of the design document, in one small file. There
    /// are three endings and the player is told about none of them.
    /// </para>
    /// <para>
    /// The default is the bitter one and it needs no condition: press green,
    /// press blue, press nothing in particular, reach act seven, watch Haru do
    /// what he does. That is the game most people will finish.
    /// </para>
    /// <para>
    /// The other two are not branches in the script. They are an offer that
    /// appears, once, in the middle of whichever act the player happens to be
    /// in — and only if two separate things are true at the same time.
    /// </para>
    /// <para>
    /// The first is that they have sat with one of the game's silences for five
    /// real minutes without touching anything. The second is that the whole
    /// playthrough has meant one thing: every counted press blue, or every
    /// counted press green. All blue and Yua is the one who speaks. All green
    /// and it is Haru. A single press of the other colour, anywhere, at any
    /// point, and there is no offer at all and the five minutes buy nothing.
    /// </para>
    /// <para>
    /// That symmetry is the whole design. Neither ending is a reward for having
    /// been mostly one thing, and the bitter one is not a punishment — it is
    /// what happens to a player who was trying options out, which is nearly
    /// everybody, and it is the ending the game is really about.
    /// </para>
    /// <para>
    /// The closing card of the game says both halves of that out loud, and it is
    /// the only explanation anybody gets: that decisions leave a mark whether or
    /// not you see the result, and that five minutes of listening can take
    /// something off somebody's back.
    /// </para>
    /// </remarks>
    public static class Endings
    {
        /// <summary>Asset name of the secret ending: Yua's confession.</summary>
        public const string GoodAssetName = "Ending_Good";

        /// <summary>Asset name of the open ending: Haru's anger, and leaving.</summary>
        public const string NormalAssetName = "Ending_Normal";

        /// <summary>
        /// The ending this playthrough has earned, or <c>null</c> when neither
        /// has been written yet.
        /// </summary>
        public static ActAsset ForCurrentRun()
        {
            string wanted = AssetNameForCurrentRun();

            if (wanted == null)
            {
                // A mixed run, or one that has not reached a choice yet. Nothing
                // is offered and nothing says so.
                return null;
            }

            ActAsset found = ActLibrary.FindByName(wanted);

            if (found == null || found.Count == 0)
            {
                Debug.LogWarning(
                    $"[Story] The player earned '{wanted}' and there is no act of that name with any beats " +
                    "in it, so nothing is offered and the scene carries on. Build it from " +
                    "The Frayed Red String ▸ Build The Two Endings.");

                return null;
            }

            return found;
        }

        /// <summary>
        /// Which ending this playthrough qualifies for, or <c>null</c> for none.
        /// </summary>
        /// <remarks>
        /// The entire condition, in three lines. Kept separate from
        /// <see cref="ForCurrentRun"/> so that the Editor's reporting tool can
        /// ask the question without the missing-asset warning that goes with
        /// actually trying to play one.
        /// </remarks>
        public static string AssetNameForCurrentRun()
        {
            if (StorySession.IsPureKindRun)
            {
                return GoodAssetName;
            }

            return StorySession.IsPureCruelRun ? NormalAssetName : null;
        }

        /// <summary>
        /// The offer itself: the sentence that appears, and the one that does
        /// not take it.
        /// </summary>
        /// <remarks>
        /// Both are Neutral. A choice that decides the ending of the game must
        /// not also be scored as kind or controlling — the count it would change
        /// is the count that chose which offer this is.
        /// </remarks>
        public static ChoiceData[] Offer()
        {
            string speak = StorySession.IsPureKindRun
                ? LocKeys.EndingSpeakYua
                : LocKeys.EndingSpeakHaru;

            // Only ever called after ForCurrentRun has returned something, so
            // one of the two is true and the fallback above is not a guess.

            return new[]
            {
                new ChoiceData
                {
                    Tone = ChoiceTone.Neutral,
                    Text = Line(LocalizationService.Get(speak))
                },
                new ChoiceData
                {
                    Tone = ChoiceTone.Neutral,
                    Text = Line(LocalizationService.Get(LocKeys.EndingSayNothing))
                }
            };
        }

        /// <summary>
        /// Wraps an already-translated string as a line.
        /// </summary>
        /// <remarks>
        /// These two come from the interface table rather than from an act, so
        /// they are resolved once here rather than carrying three languages the
        /// way a scripted line does. Re-offering after a language change is not
        /// a case that can arise: the panel is up for one decision and the
        /// language button is behind the pause menu.
        /// </remarks>
        private static LocalizedLine Line(string text)
        {
            return new LocalizedLine(text, text, text);
        }
    }
}
