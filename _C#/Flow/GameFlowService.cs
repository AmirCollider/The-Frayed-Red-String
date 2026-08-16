// -----------------------------------------------------------------------------
//  The Frayed Red String
//  GameFlowService.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Core;
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
        /// <summary>Leaves the content warning for the main menu.</summary>
        public static void EnterMainMenu()
        {
            SceneTransitionService.LoadScene(SceneNames.MainMenu);
        }

        /// <summary>
        /// Starts a fresh playthrough.
        /// </summary>
        /// <remarks>
        /// The first story scene has not been built yet. Rather than fail
        /// silently or throw, the button reports the situation and plays the
        /// "unavailable" sound, so the menu stays honest about what exists.
        /// </remarks>
        public static void StartNewGame()
        {
            if (!SceneTransitionService.IsSceneAvailable(SceneNames.FirstAct))
            {
                Debug.LogWarning(
                    $"[GameFlow] Start was pressed, but scene '{SceneNames.FirstAct}' does not exist yet. " +
                    "Create it and add it to Build Settings to make this button live.");

                AudioService.Play(SfxId.Denied);
                return;
            }

            SceneTransitionService.LoadScene(SceneNames.FirstAct);
        }

        /// <summary>Continues from a save slot.</summary>
        public static void LoadSlot(int slotNumber)
        {
            SaveSlotData slot = SaveService.Read(slotNumber);

            if (!slot.Exists)
            {
                AudioService.Play(SfxId.Denied);
                return;
            }

            if (!SceneTransitionService.IsSceneAvailable(SceneNames.FirstAct))
            {
                Debug.LogWarning(
                    $"[GameFlow] Slot {slotNumber} holds a save from act {slot.ActNumber}, " +
                    $"but no story scene exists to load it into yet.");

                AudioService.Play(SfxId.Denied);
                return;
            }

            SceneTransitionService.LoadScene(SceneNames.FirstAct);
        }

        /// <summary>
        /// Wipes every save. The narrative requires this after an ending plays:
        /// a replay must begin from the very start.
        /// </summary>
        public static void EraseAllProgress()
        {
            SaveService.DeleteAll();
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
