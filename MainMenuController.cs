// ==========================================
// MainMenuController - Main Menu Scene Orchestrator
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    // ==========================================
    // Inspector References
    // ==========================================
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button exitButton;

    [Header("Menu Variant Art")]
    [SerializeField] private Image backgroundMenuImage;
    [SerializeField] private Sprite menuArtDefault;
    [SerializeField] private Sprite menuArtCompleted;

    [Header("Panels")]
    [SerializeField] private LoadPanelController loadPanel;

    // ==========================================
    // Start - Initialize Scene State
    // ==========================================
    private void Start()
    {
        SetButtonsInteractable(true);
        WireButtons();
        ApplyMenuVariant();
    }

    // ==========================================
    // WireButtons - Assign onClick Listeners
    // ==========================================
    private void WireButtons()
    {
        if (startButton != null) startButton.onClick.AddListener(OnStartClicked);
        if (loadButton != null) loadButton.onClick.AddListener(OnLoadClicked);
        if (exitButton != null) exitButton.onClick.AddListener(OnExitClicked);
    }

    // ==========================================
    // ApplyMenuVariant - Swap Background if Game Completed
    // ==========================================
    private void ApplyMenuVariant()
    {
        if (GameManager.Instance == null || backgroundMenuImage == null) return;

        backgroundMenuImage.sprite = GameManager.Instance.GameCompleted && menuArtCompleted != null
            ? menuArtCompleted
            : menuArtDefault;
    }

    // ==========================================
    // SetButtonsInteractable - Enable or Disable All Menu Buttons
    // ==========================================
    private void SetButtonsInteractable(bool state)
    {
        if (startButton != null) startButton.interactable = state;
        if (loadButton != null) loadButton.interactable = state;
        if (exitButton != null) exitButton.interactable = state;
    }

    // ==========================================
    // OnStartClicked - Begin Act 1
    // ==========================================
    private void OnStartClicked()
    {
        if (SceneController.Instance != null)
            SceneController.Instance.LoadAct(1);
    }

    // ==========================================
    // OnLoadClicked - Open Save Slot Browser Panel
    // ==========================================
    private void OnLoadClicked()
    {
        if (loadPanel != null)
            loadPanel.Open();
    }

    // ==========================================
    // OnExitClicked - Quit Application
    // ==========================================
    private void OnExitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}