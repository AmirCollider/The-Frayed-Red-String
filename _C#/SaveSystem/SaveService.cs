// -----------------------------------------------------------------------------
//  The Frayed Red String
//  SaveService.cs
// -----------------------------------------------------------------------------

using System;
using TheFrayedRedString.Core;
using UnityEngine;

namespace TheFrayedRedString.SaveSystem
{
    /// <summary>
    /// Reads and writes save slots.
    /// </summary>
    /// <remarks>
    /// Backed by PlayerPrefs for now. The whole of a playthrough's state is a
    /// handful of integers — which act, how far into it, and how the player has
    /// been treating Haru — so the storage format is deliberately flat; when
    /// there is more to persist, this class is the seam to swap for a
    /// file-backed implementation without touching the UI.
    /// </remarks>
    public static class SaveService
    {
        /// <summary>Number of save slots the load panel exposes.</summary>
        public const int SlotCount = 3;

        private const string KeyExists = GameConfig.PrefsPrefix + "Slot{0}.Exists";
        private const string KeyAct = GameConfig.PrefsPrefix + "Slot{0}.Act";
        private const string KeyLine = GameConfig.PrefsPrefix + "Slot{0}.Line";
        private const string KeyPlaySeconds = GameConfig.PrefsPrefix + "Slot{0}.PlaySeconds";
        private const string KeyKind = GameConfig.PrefsPrefix + "Slot{0}.Kind";
        private const string KeyCruel = GameConfig.PrefsPrefix + "Slot{0}.Cruel";
        private const string KeyPatience = GameConfig.PrefsPrefix + "Slot{0}.Patience";
        private const string KeyBackground = GameConfig.PrefsPrefix + "Slot{0}.Background";
        private const string KeySavedAt = GameConfig.PrefsPrefix + "Slot{0}.SavedAt";

        /// <summary>Raised whenever a slot is written or cleared.</summary>
        public static event Action SlotsChanged;

        /// <summary>Clears static state between play sessions.</summary>
        public static void ResetStatics()
        {
            SlotsChanged = null;
        }

        /// <summary>True when <paramref name="slotNumber"/> is within range.</summary>
        public static bool IsValidSlot(int slotNumber)
        {
            return slotNumber >= 1 && slotNumber <= SlotCount;
        }

        /// <summary>
        /// Reads a slot. Out-of-range or unwritten slots come back as
        /// <see cref="SaveSlotData.Empty"/> rather than throwing, because the
        /// load panel asks for all three every time it opens.
        /// </summary>
        public static SaveSlotData Read(int slotNumber)
        {
            if (!IsValidSlot(slotNumber))
            {
                return SaveSlotData.Empty(slotNumber);
            }

            if (PlayerPrefs.GetInt(Key(KeyExists, slotNumber), 0) == 0)
            {
                return SaveSlotData.Empty(slotNumber);
            }

            return new SaveSlotData
            {
                SlotNumber = slotNumber,
                Exists = true,
                ActNumber = PlayerPrefs.GetInt(Key(KeyAct, slotNumber), 1),
                LineIndex = PlayerPrefs.GetInt(Key(KeyLine, slotNumber), 0),
                PlaySeconds = PlayerPrefs.GetFloat(Key(KeyPlaySeconds, slotNumber), 0f),
                KindChoices = PlayerPrefs.GetInt(Key(KeyKind, slotNumber), 0),
                CruelChoices = PlayerPrefs.GetInt(Key(KeyCruel, slotNumber), 0),
                PatientMoments = PlayerPrefs.GetInt(Key(KeyPatience, slotNumber), 0),
                BackgroundName = PlayerPrefs.GetString(Key(KeyBackground, slotNumber), string.Empty),
                SavedAtUtcTicks = ReadTicks(slotNumber)
            };
        }

        /// <summary>
        /// Writes a slot and notifies listeners.
        /// </summary>
        /// <remarks>
        /// One writer, taking the whole record. There used to be a second,
        /// convenience overload that took an act number and a play time and
        /// filled the rest of the struct with its default values — which, since
        /// this method writes every key unconditionally, quietly reset the line
        /// index, both choice counters and the background of whatever slot it
        /// touched. Nothing called it, which is the only reason it was never a
        /// bug report; it is gone rather than documented.
        /// </remarks>
        public static void Write(SaveSlotData data)
        {
            int slotNumber = data.SlotNumber;

            if (!IsValidSlot(slotNumber))
            {
                Debug.LogWarning($"[SaveService] Slot {slotNumber} is out of range 1..{SlotCount}.");
                return;
            }

            PlayerPrefs.SetInt(Key(KeyExists, slotNumber), 1);
            PlayerPrefs.SetInt(Key(KeyAct, slotNumber), Mathf.Max(1, data.ActNumber));
            PlayerPrefs.SetInt(Key(KeyLine, slotNumber), Mathf.Max(0, data.LineIndex));
            PlayerPrefs.SetFloat(Key(KeyPlaySeconds, slotNumber), Mathf.Max(0f, data.PlaySeconds));
            PlayerPrefs.SetInt(Key(KeyKind, slotNumber), Mathf.Max(0, data.KindChoices));
            PlayerPrefs.SetInt(Key(KeyCruel, slotNumber), Mathf.Max(0, data.CruelChoices));
            PlayerPrefs.SetInt(Key(KeyPatience, slotNumber), Mathf.Max(0, data.PatientMoments));
            PlayerPrefs.SetString(Key(KeyBackground, slotNumber), data.BackgroundName ?? string.Empty);
            PlayerPrefs.SetString(Key(KeySavedAt, slotNumber), DateTime.UtcNow.Ticks.ToString());
            PlayerPrefs.Save();

            SlotsChanged?.Invoke();
        }

        /// <summary>Clears one slot.</summary>
        public static void Delete(int slotNumber)
        {
            if (!IsValidSlot(slotNumber))
            {
                return;
            }

            PlayerPrefs.DeleteKey(Key(KeyExists, slotNumber));
            PlayerPrefs.DeleteKey(Key(KeyAct, slotNumber));
            PlayerPrefs.DeleteKey(Key(KeyLine, slotNumber));
            PlayerPrefs.DeleteKey(Key(KeyPlaySeconds, slotNumber));
            PlayerPrefs.DeleteKey(Key(KeyKind, slotNumber));
            PlayerPrefs.DeleteKey(Key(KeyCruel, slotNumber));
            PlayerPrefs.DeleteKey(Key(KeyPatience, slotNumber));
            PlayerPrefs.DeleteKey(Key(KeyBackground, slotNumber));
            PlayerPrefs.DeleteKey(Key(KeySavedAt, slotNumber));
            PlayerPrefs.Save();

            SlotsChanged?.Invoke();
        }

        /// <summary>
        /// Clears every slot. The story calls for wiping all saves once an
        /// ending has played, so a replay has to start from the beginning.
        /// </summary>
        public static void DeleteAll()
        {
            for (int slot = 1; slot <= SlotCount; slot++)
            {
                Delete(slot);
            }
        }

        /// <summary>True when at least one slot holds a save.</summary>
        public static bool HasAnySave()
        {
            for (int slot = 1; slot <= SlotCount; slot++)
            {
                if (PlayerPrefs.GetInt(Key(KeyExists, slot), 0) != 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static string Key(string template, int slotNumber)
        {
            return string.Format(template, slotNumber.ToString("00"));
        }

        /// <summary>
        /// Ticks are stored as a string: PlayerPrefs has no 64-bit integer type
        /// and a DateTime tick count overflows its 32-bit int.
        /// </summary>
        private static long ReadTicks(int slotNumber)
        {
            string raw = PlayerPrefs.GetString(Key(KeySavedAt, slotNumber), "0");
            return long.TryParse(raw, out long ticks) ? ticks : 0L;
        }
    }
}
