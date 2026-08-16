// -----------------------------------------------------------------------------
//  The Frayed Red String
//  SaveSlotData.cs
// -----------------------------------------------------------------------------

using System;
using TheFrayedRedString.Localization;
using UnityEngine;

namespace TheFrayedRedString.SaveSystem
{
    /// <summary>
    /// The metadata one save slot shows in the load panel.
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

        /// <summary>Total seconds played on this file.</summary>
        public float PlaySeconds;

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
                PlaySeconds = 0f,
                SavedAtUtcTicks = 0L
            };
        }

        /// <summary>Localised act title, e.g. "Cherry Blossom Mirage".</summary>
        public string GetActTitle()
        {
            int index = Mathf.Clamp(ActNumber - 1, 0, LocKeys.ActNames.Length - 1);
            return LocalizationService.Get(LocKeys.ActNames[index]);
        }

        /// <summary>Play time rendered as hh:mm:ss.</summary>
        public string GetFormattedPlayTime()
        {
            TimeSpan span = TimeSpan.FromSeconds(Mathf.Max(0f, PlaySeconds));
            return $"{(int)span.TotalHours:00}:{span.Minutes:00}:{span.Seconds:00}";
        }
    }
}
