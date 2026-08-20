// -----------------------------------------------------------------------------
//  The Frayed Red String
//  GameFlowService.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Core;
using TheFrayedRedString.Narrative;
using TheFrayedRedString.Presentation;
using TheFrayedRedString.SaveSystem;
using UnityEngine;

namespace TheFrayedRedString.Flow
{
    /// <summary>
    /// The verbs of the game, expressed once. Buttons, keyboard shortcuts and
    /// story scripts all route through here rather than calling
    /// <see cref="UnityEngine.SceneManagement.SceneManager"/> themselves.
    /// </summary>
    public static class GameFlowService
    {
        /// <summary>
        /// The curtain one act draws across before the next one opens.
        /// </summary>
        /// <remarks>
        /// Not black. Black is what the game fades to when the player leaves it,
        /// and the walk from one act to the next is not leaving — a deep plum
        /// reads as the same evening carrying on, which is the whole of what act
        /// two is doing to act one.
        /// </remarks>
        private static readonly Color ActCurtain = new Color(0.10f, 0.04f, 0.09f, 1f);

        /// <summary>Leaves the content warning for the main menu.</summary>
        public static void EnterMainMenu()
        {
            SceneTransitionService.LoadScene(SceneNames.MainMenu);
        }

        /// <summary>Starts a fresh playthrough at the top of act one.</summary>
        public static void StartNewGame()
        {
            if (!SceneTransitionService.IsSceneAvailable(SceneNames.FirstAct))
            {
                Debug.LogWarning(
                    $"[GameFlow] Start was pressed, but scene '{SceneNames.FirstAct}' is not in Build Settings. " +
                    "Run The Frayed Red String > Repair Build Settings to add it.");

                AudioService.Play(SfxId.Denied);
                return;
            }

            StorySession.BeginNewGame();
            SceneTransitionService.LoadScene(SceneNames.FirstAct);
        }

        /// <summary>
        /// Continues from a save slot.
        /// </summary>
        /// <remarks>
        /// The slot carries the act it was taken in and how far into it the
        /// player had read, so this restores the session first and then loads
        /// that act's scene — the act itself picks the resume point up from
        /// <see cref="StorySession"/> when it starts.
        /// </remarks>
        public static void LoadSlot(int slotNumber)
        {
            SaveSlotData slot = SaveService.Read(slotNumber);

            if (!slot.Exists)
            {
                AudioService.Play(SfxId.Denied);
                return;
            }

            string sceneName = SceneNames.ForAct(slot.ActNumber);

            if (!SceneTransitionService.IsSceneAvailable(sceneName))
            {
                Debug.LogWarning(
                    $"[GameFlow] Slot {slotNumber} holds a save from act {slot.ActNumber}, " +
                    $"but scene '{sceneName}' does not exist yet.");

                AudioService.Play(SfxId.Denied);
                return;
            }

            StorySession.Resume(slot);
            SceneTransitionService.LoadScene(sceneName);
        }

        /// <summary>
        /// Moves to the next act, or back to the menu when there is no next act
        /// built yet.
        /// </summary>
        /// <remarks>
        /// Returning to the title is the honest ending for a game that has run
        /// out of story: the alternative is leaving the player on a dead screen
        /// with no way forward and no explanation. Progress is already recorded
        /// by the time this is called, so continuing from the menu picks up
        /// where they stopped once the next act exists.
        /// </remarks>
        public static void AdvanceToAct(int actNumber)
        {
            string sceneName = SceneNames.ForAct(actNumber);

            if (SceneTransitionService.IsSceneAvailable(sceneName))
            {
                SceneTransitionService.LoadScene(sceneName, ActCurtain);
                return;
            }

            Debug.Log(
                $"[GameFlow] Act {actNumber} has not been built yet, so the game returns to the menu. " +
                "Progress has been kept.");

            EnterMainMenu();
        }

        /// <summary>
        /// Wipes every save. The narrative requires this after an ending plays:
        /// a replay must begin from the very start.
        /// </summary>
        public static void EraseAllProgress()
        {
            SaveService.DeleteAll();
            StorySession.BeginNewGame();
        }

        /// <summary>
        /// The story is over. Erases the saves and returns to the title.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Called when an act reaches an EndGame beat, which is how all three
        /// endings finish. The erase is the design document's and it is load
        /// bearing rather than decorative: the secret ending is earned by never
        /// once having taken the controlling option across a whole playthrough,
        /// and a save file part-way through one would turn that into something
        /// to be reloaded at until it worked.
        /// </para>
        /// <para>
        /// The curtain is black here rather than the plum used between acts.
        /// Plum means the same evening carrying on; this is not that.
        /// </para>
        /// </remarks>
        public static void FinishGame()
        {
            // Before the erase, and separate from it. See StoryProgress.
            StoryProgress.RecordEndingSeen();

            EraseAllProgress();

            Debug.Log("[GameFlow] The story ended. Every save has been erased, as the design requires.");

            SceneTransitionService.LoadScene(SceneNames.MainMenu, Color.black);
        }

        /// <summary>Quits, fading out first so the exit does not feel abrupt.</summary>
        public static void QuitGame()
        {
            ScreenFader.FadeOut(GameConfig.SceneFadeOutDuration, PerformQuit);
        }

        private static void PerformQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
