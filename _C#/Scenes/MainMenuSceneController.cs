// -----------------------------------------------------------------------------
//  The Frayed Red String
//  MainMenuSceneController.cs
// -----------------------------------------------------------------------------

using TheFrayedRedString.Audio;
using TheFrayedRedString.Core;
using TheFrayedRedString.Flow;
using TheFrayedRedString.InputHandling;
using TheFrayedRedString.Presentation;
using TheFrayedRedString.UI;
using TheFrayedRedString.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrayedRedString.Scenes
{
    /// <summary>
    /// Drives MainMenu: wires the four buttons to real behaviour, brings the
    /// menu on screen in a staggered cascade, and owns the load panel.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class MainMenuSceneController : MonoBehaviour
    {
        [Header("Entrance")]
        [Tooltip("Delay before the first element starts fading in.")]
        [SerializeField] private float _entranceStartDelay = 0.15f;

        [Tooltip("Gap between consecutive elements in the cascade.")]
        [SerializeField] private float _entranceStagger = 0.14f;

        [Tooltip("Length of each element's fade.")]
        [SerializeField] private float _entranceDuration = 0.9f;

        private Transform _canvas;
        private LoadPanelController _loadPanel;

        private void Start()
        {
            _canvas = UnityUtility.FindInScene(ObjectNames.BackgroundCanvas) ?? FindFirstSceneCanvas();

            if (_canvas == null)
            {
                Debug.LogError("[MainMenu] No canvas found; the menu cannot be wired up.");
                return;
            }

            SetUpLoadPanel();
            SetUpLanguageToggle();
            SetUpMenuButtons();
            PlayEntrance();

            MusicService.PlayLoop(MusicTracks.MainMenu);
            ScreenFader.FadeIn(GameConfig.SceneFadeInDuration);
        }

        private void Update()
        {
            // Escape backs out of the load panel; the panel itself also handles
            // this, but only while it is open. Nothing here should quit the game
            // on a stray key press.
            if (InputService.CancelPressedThisFrame && _loadPanel != null && _loadPanel.IsOpen)
            {
                _loadPanel.Close();
            }
        }

        private void SetUpLoadPanel()
        {
            Transform panel = UnityUtility.FindDeep(_canvas, ObjectNames.LoadPanel);

            if (panel == null)
            {
                Debug.LogWarning($"[MainMenu] No '{ObjectNames.LoadPanel}' found; Load will do nothing.");
                return;
            }

            // The panel carries its own copy of the game-title art, sitting in
            // the same place on screen as the menu's. Hand the outer one to the
            // panel so it can cross-fade them and only ever show one.
            Graphic menuTitleArt = UnityUtility.FindDeep<Graphic>(_canvas, ObjectNames.MenuTitle);

            _loadPanel = UnityUtility.GetOrAdd<LoadPanelController>(panel.gameObject);
            _loadPanel.Initialize(menuTitleArt);
        }

        private void SetUpLanguageToggle()
        {
            Button languageButton = UnityUtility.FindDeep<Button>(_canvas, ObjectNames.LanguageButton);

            if (languageButton == null)
            {
                Debug.LogWarning($"[MainMenu] No '{ObjectNames.LanguageButton}' found; language cannot be switched here.");
                return;
            }

            UnityUtility.GetOrAdd<LanguageToggleButton>(languageButton.gameObject);
        }

        private void SetUpMenuButtons()
        {
            BindButton(ObjectNames.StartGameButton, GameFlowService.StartNewGame);
            BindButton(ObjectNames.OpenLoadMenuButton, ToggleLoadPanel);
            BindButton(ObjectNames.ExitGameButton, GameFlowService.QuitGame);
        }

        private void BindButton(string objectName, UnityEngine.Events.UnityAction action)
        {
            Button button = UnityUtility.FindDeep<Button>(_canvas, objectName);

            if (button == null)
            {
                Debug.LogWarning($"[MainMenu] No button named '{objectName}' was found.");
                return;
            }

            // The scenes were authored without listeners, but clearing first
            // keeps this idempotent if the controller is ever re-initialised.
            button.onClick.RemoveListener(action);
            button.onClick.AddListener(action);
        }

        private void ToggleLoadPanel()
        {
            if (_loadPanel == null)
            {
                AudioService.Play(SfxId.Denied);
                return;
            }

            _loadPanel.Toggle();
        }

        /// <summary>
        /// Brings the menu in back-to-front: the world first, then the
        /// characters standing in it, then the title, then the things the player
        /// can touch.
        /// </summary>
        private void PlayEntrance()
        {
            GameObject[] cascade =
            {
                Find(ObjectNames.MenuBackground),
                Find(ObjectNames.MenuCharacters),
                Find(ObjectNames.MenuTitle),
                Find(ObjectNames.StartGameButton),
                Find(ObjectNames.OpenLoadMenuButton),
                Find(ObjectNames.ExitGameButton),
                Find(ObjectNames.WarningCard),
                Find(ObjectNames.LanguageButtonMask)
            };

            EntranceAnimator.FadeInSequence(
                cascade,
                _entranceStartDelay,
                _entranceStagger,
                _entranceDuration);
        }

        private GameObject Find(string objectName)
        {
            Transform found = UnityUtility.FindDeep(_canvas, objectName);
            return found != null ? found.gameObject : null;
        }

        /// <summary>
        /// Finds a canvas belonging to this scene.
        /// </summary>
        /// <remarks>
        /// Deliberately scoped to the active scene's roots rather than using
        /// <c>FindAnyObjectByType</c>. The screen fader lives on a
        /// DontDestroyOnLoad canvas, and a global search could return that
        /// instead — leaving the menu wired to the fade curtain.
        /// </remarks>
        private static Transform FindFirstSceneCanvas()
        {
            UnityEngine.SceneManagement.Scene scene =
                UnityEngine.SceneManagement.SceneManager.GetActiveScene();

            System.Collections.Generic.List<GameObject> roots =
                new System.Collections.Generic.List<GameObject>(scene.rootCount);

            scene.GetRootGameObjects(roots);

            for (int i = 0; i < roots.Count; i++)
            {
                Canvas canvas = roots[i].GetComponentInChildren<Canvas>(true);
                if (canvas != null)
                {
                    return canvas.transform;
                }
            }

            return null;
        }
    }
}
