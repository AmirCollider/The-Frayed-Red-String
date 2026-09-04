// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StoryProgress.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Core;
using UnityEngine;

namespace TheFrayedRedString.SaveSystem
{
    /// <summary>
    /// The one thing about a playthrough that outlives it.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Every save is erased when the story ends — that is the design and it is
    /// not negotiable, because the secret ending is a fact about a whole
    /// playthrough and a mid-run save would make it farmable. But the design
    /// also asks for the title screen to change afterwards: the game cuts back
    /// to the menu, and the picture there is the two of them as children with
    /// the rabbit still whole.
    /// </para>
    /// <para>
    /// So one flag survives, and it is deliberately not a save. It records that
    /// somebody in this copy of the game has seen an ending. It cannot be loaded
    /// from, it grants nothing, and starting again still starts at the very
    /// beginning with every counter at zero.
    /// </para>
    /// </remarks>
    public static class StoryProgress
    {
        private const string SeenEndingKey = GameConfig.PrefsPrefix + "SeenAnEnding";

        /// <summary>True once any ending has been reached on this machine.</summary>
        public static bool HasSeenAnEnding
        {
            get => PlayerPrefs.GetInt(SeenEndingKey, 0) != 0;
            private set
            {
                PlayerPrefs.SetInt(SeenEndingKey, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        /// <summary>Records that the story has been finished.</summary>
        public static void RecordEndingSeen()
        {
            HasSeenAnEnding = true;
        }

        /// <summary>
        /// Forgets that the story was ever finished.
        /// </summary>
        /// <remarks>
        /// For testing the title screen's two states, which is otherwise a
        /// one-way door per machine.
        /// </remarks>
        public static void Forget()
        {
            HasSeenAnEnding = false;
        }
    }
}
