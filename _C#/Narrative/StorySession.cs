// -----------------------------------------------------------------------------
//  The Frayed Red String
//  StorySession.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.SaveSystem;
using UnityEngine;

namespace TheFrayedRedString.Narrative
{
    /// <summary>
    /// What the game knows about the playthrough currently in progress.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Static, and deliberately so. This state has to survive a scene load —
    /// every act is its own scene — and it is read by the save slots, the pause
    /// menu and the story director, none of which have a reference to the
    /// others. A DontDestroyOnLoad component would be the alternative and would
    /// buy nothing except a component to find.
    /// </para>
    /// <para>
    /// The two choice counters are not used in act one, which has no blue and
    /// green buttons: the mechanic is introduced in act two, and the endings are
    /// decided by it from act three onward. They live here now so that the acts
    /// which do use them have somewhere to record it, and so that a save written
    /// today already carries the shape of the data.
    /// </para>
    /// </remarks>
    public static class StorySession
    {
#if UNITY_EDITOR
        /// <summary>
        /// Where the Story Editor leaves a beat to jump to when it presses Play.
        /// </summary>
        /// <remarks>
        /// SessionState rather than a static field: pressing Play reloads the
        /// scripting domain, and a static field does not survive that. It does
        /// not survive closing Unity either, which is right — a jump is for the
        /// run you are about to do.
        /// </remarks>
        internal const string PlaytestActKey = "TFRS.Playtest.Act";

        internal const string PlaytestBeatKey = "TFRS.Playtest.Beat";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ApplyPlaytestJump()
        {
            int act = UnityEditor.SessionState.GetInt(PlaytestActKey, 0);

            if (act <= 0)
            {
                return;
            }

            int beat = UnityEditor.SessionState.GetInt(PlaytestBeatKey, 0);

            // Taken once. Leaving it set would jump every subsequent Play, which
            // is the kind of thing that gets blamed on the save system.
            UnityEditor.SessionState.EraseInt(PlaytestActKey);
            UnityEditor.SessionState.EraseInt(PlaytestBeatKey);

            BeginAt(act, beat);

            Debug.Log($"[Story] Playtest: starting act {act:00} at beat {beat:000}.");
        }
#endif

        /// <summary>
        /// The slot this playthrough is bound to, or 0 for a run that has never
        /// been saved.
        /// </summary>
        public static int ActiveSlot { get; private set; }

        /// <summary>Act currently being played, 1-based.</summary>
        public static int ActNumber { get; private set; } = 1;

        /// <summary>How far through the current act's script the player is.</summary>
        public static int LineIndex { get; set; }

        /// <summary>Total seconds played on this file.</summary>
        public static float PlaySeconds { get; private set; }

        /// <summary>
        /// The background on screen right now, by asset name.
        /// </summary>
        /// <remarks>
        /// Kept so a save can show where it was taken. A card that says "Season
        /// 01, fifty minutes" and shows a grey rectangle is three save files
        /// that look identical; the same card showing the cherry-blossom alley
        /// is a memory the player recognises before they have read a word of it.
        /// The stage writes this as it changes the picture.
        /// </remarks>
        public static string BackgroundName { get; set; }

        /// <summary>Times the player has taken the kind option.</summary>
        public static int KindChoices { get; private set; }

        /// <summary>Times the player has taken the controlling option.</summary>
        public static int CruelChoices { get; private set; }

        /// <summary>
        /// Where the act should pick up from, consumed once by the act that
        /// starts next.
        /// </summary>
        /// <remarks>
        /// Separate from <see cref="LineIndex"/>, which moves continuously as the
        /// player reads. This is the one-shot instruction "you were loaded, skip
        /// ahead", and it has to be cleared on use or replaying the act from the
        /// menu would silently jump back into the middle of it.
        /// </remarks>
        public static int PendingResumeLine { get; private set; }

        /// <summary>
        /// True when the player has never once taken the controlling option.
        /// </summary>
        /// <remarks>
        /// The condition for the secret ending, per the design document: a
        /// single green choice anywhere in the game closes it off permanently.
        /// </remarks>
        public static bool IsPureKindRun => CruelChoices == 0;

        /// <summary>Clears everything between play sessions.</summary>
        public static void ResetStatics()
        {
            ActiveSlot = 0;
            ActNumber = 1;
            LineIndex = 0;
            PlaySeconds = 0f;
            KindChoices = 0;
            CruelChoices = 0;
            PendingResumeLine = 0;
            BackgroundName = null;
        }

        /// <summary>Starts a fresh playthrough from the top of act one.</summary>
        public static void BeginNewGame()
        {
            ActiveSlot = 0;
            ActNumber = 1;
            LineIndex = 0;
            PlaySeconds = 0f;
            KindChoices = 0;
            CruelChoices = 0;
            PendingResumeLine = 0;
            BackgroundName = null;
        }

        /// <summary>Restores a playthrough from a save slot.</summary>
        public static void Resume(SaveSlotData slot)
        {
            ActiveSlot = slot.SlotNumber;
            ActNumber = Mathf.Max(1, slot.ActNumber);
            LineIndex = Mathf.Max(0, slot.LineIndex);
            PlaySeconds = Mathf.Max(0f, slot.PlaySeconds);
            KindChoices = Mathf.Max(0, slot.KindChoices);
            CruelChoices = Mathf.Max(0, slot.CruelChoices);
            PendingResumeLine = LineIndex;
            BackgroundName = slot.BackgroundName;
        }

        /// <summary>
        /// Starts the next act scene partway through, for testing.
        /// </summary>
        /// <remarks>
        /// The Story Editor's "Play from here" uses this. It is the same
        /// machinery a loaded save runs on — the act primes its stage silently
        /// up to the beat and starts there — so what is tested is the real
        /// playback path and not a special one.
        /// </remarks>
        public static void BeginAt(int actNumber, int beatIndex)
        {
            BeginNewGame();

            ActNumber = Mathf.Max(1, actNumber);
            LineIndex = Mathf.Max(0, beatIndex);
            PendingResumeLine = LineIndex;
        }

        /// <summary>
        /// Takes the pending resume point, clearing it so it cannot be applied
        /// twice.
        /// </summary>
        public static int ConsumeResumeLine()
        {
            int line = PendingResumeLine;
            PendingResumeLine = 0;
            return line;
        }

        /// <summary>Records that the player has reached a new act.</summary>
        public static void EnterAct(int actNumber)
        {
            ActNumber = Mathf.Max(1, actNumber);
            LineIndex = 0;
        }

        /// <summary>
        /// Notes which act the player is now in, without moving them within it.
        /// </summary>
        /// <remarks>
        /// Called by an act scene as it loads. Distinct from
        /// <see cref="EnterAct"/>, which starts an act from the top: this one
        /// must not clear the line index, or loading a save would put the player
        /// in the right act at the wrong moment.
        /// </remarks>
        public static void EnterActScene(int actNumber)
        {
            ActNumber = Mathf.Max(1, actNumber);
        }

        /// <summary>Accumulates play time. Called once a frame by the act.</summary>
        public static void Tick(float deltaSeconds)
        {
            PlaySeconds += Mathf.Max(0f, deltaSeconds);
        }

        /// <summary>Records a choice against the ending counters.</summary>
        public static void RecordChoice(ChoiceTone tone)
        {
            switch (tone)
            {
                case ChoiceTone.Kind:
                    KindChoices++;
                    break;

                case ChoiceTone.Cruel:
                    CruelChoices++;
                    break;
            }
        }

        /// <summary>Writes the playthrough to a slot and binds to it.</summary>
        public static void WriteTo(int slotNumber)
        {
            if (!SaveService.IsValidSlot(slotNumber))
            {
                return;
            }

            ActiveSlot = slotNumber;

            SaveService.Write(new SaveSlotData
            {
                SlotNumber = slotNumber,
                Exists = true,
                ActNumber = ActNumber,
                LineIndex = LineIndex,
                PlaySeconds = PlaySeconds,
                KindChoices = KindChoices,
                CruelChoices = CruelChoices,
                BackgroundName = BackgroundName
            });
        }
    }
}
