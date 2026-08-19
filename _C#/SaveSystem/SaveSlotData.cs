// -----------------------------------------------------------------------------
//  The Frayed Red String
//  SaveSlotData.cs
// -----------------------------------------------------------------------------

using System;
using TheFrayedRedString.Narrative;
using UnityEngine;

namespace TheFrayedRedString.SaveSystem
{
    /// <summary>
    /// One save slot: what the load panel shows on the card, plus enough of the
    /// playthrough to put the player back where they were.
    /// </summary>
    [Serializable]
    public struct SaveSlotData
    {
        /// <summary>Slot number, 1-based, matching SaveSlot01 … SaveSlot03.</summary>
        public int SlotNumber;

        /// <summary>False when the slot has never been written to.</summary>
        public bool Exists;

        /// <summary>Act the save was taken in, 1-based.</summary>
        public int ActNumber;

        /// <summary>How far into that act's script the player had read.</summary>
        public int LineIndex;

        /// <summary>Total seconds played on this file.</summary>
        public float PlaySeconds;

        /// <summary>Times the player has taken the kind option.</summary>
        public int KindChoices;

        /// <summary>
        /// Times the player has taken the controlling option.
        /// </summary>
        /// <remarks>
        /// Saved because the endings hang on it: one green choice anywhere in a
        /// playthrough rules out the secret ending permanently, so the count has
        /// to survive being put down and picked up again days later.
        /// </remarks>
        public int CruelChoices;

        /// <summary>
        /// The background that was on screen when the save was written, by
        /// asset name. Empty on a save taken before there was one.
        /// </summary>
        public string BackgroundName;

        /// <summary>UTC ticks of the last write, for sorting.</summary>
        public long SavedAtUtcTicks;

        /// <summary>An empty slot for the given number.</summary>
        public static SaveSlotData Empty(int slotNumber)
        {
            return new SaveSlotData
            {
                SlotNumber = slotNumber,
                Exists = false,
                ActNumber = 1,
                LineIndex = 0,
                PlaySeconds = 0f,
                KindChoices = 0,
                CruelChoices = 0,
                BackgroundName = null,
                SavedAtUtcTicks = 0L
            };
        }

        /// <summary>
        /// Localised act title, e.g. "Cherry Blossom Mirage".
        /// </summary>
        /// <remarks>
        /// Read from the act itself rather than from a table of act names. The
        /// title is part of the act, and keeping one copy means renaming an act
        /// in the Story Editor renames it on the save cards too.
        /// </remarks>
        public string GetActTitle()
        {
            return ActLibrary.TitleOf(ActNumber);
        }

        /// <summary>Play time rendered as hh:mm:ss.</summary>
        public string GetFormattedPlayTime()
        {
            TimeSpan span = TimeSpan.FromSeconds(Mathf.Max(0f, PlaySeconds));
            return $"{(int)span.TotalHours:00}:{span.Minutes:00}:{span.Seconds:00}";
        }
    }
}
