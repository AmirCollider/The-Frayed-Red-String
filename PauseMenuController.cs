// ==========================================
// PauseMenuController - In-Scene Stop Menu (Resume / Save / Load / Quit) with Pause Gate
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    // ==========================================
    // Global Pause Flag — Read by DialogueSystem.Update to Suppress Advance Input
    // ==========================================
    public static bool IsPaused { get; private set; }

    // ==========================================
    // Inspector — Act Identity (1, 2 or 3) for SaveSystem.CaptureCurrent
    // ==========================================
    [Header("Act Identity (1, 2 or 3)")]
    [SerializeField] private int actNumber = 1;

    // ==========================================
    // Inspector — Root (this component lives on an ALWAYS-ACTIVE canvas object)
    // ==========================================
    [Header("Root")]
    [SerializeField] private CanvasGroup pauseRoot;
    [SerializeField] private RectTransform pausePanel;

    // ==========================================
    // Inspector — Buttons
    // ==========================================
    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button menuButton;

    // ==========================================
    // Inspector — Shared Save/Load Sub-Panel
    // ==========================================
    [Header("Sub-Panel")]
    [SerializeField] private LoadPanelController loadPanel;

    // ==========================================
    // Inspector — SFX
    // ==========================================
    [Header("SFX")]
    [SerializeField] private AudioEvent clickSfx;

    // ==========================================
    // Inspector — Animation (UNSCALED — must animate while Time.timeScale = 0)
    // ==========================================
    [Header("Animation (unscaled time)")]
    [SerializeField] private float fadeDuration = 0.15f;
    [SerializeField] private float scaleFrom = 0.96f;

    // ==========================================
    // Private State
    // ==========================================
    private Coroutine _animCo;

    // ==========================================
    // Awake - Reset Flags, Hide, Wire Buttons
    // ==========================================
    private void Awake()
    {
        IsPaused = false;
        ForceHidden();

        if (resumeButton != null) resumeButton.onClick.AddListener(Resume);
        if (saveButton != null) saveButton.onClick.AddListener(OnSaveClicked);
        if (loadButton != null) loadButton.onClick.AddListener(OnLoadClicked);
        if (menuButton != null) menuButton.onClick.AddListener(OnMenuClicked);
    }

    // ==========================================
    // OnDestroy - Never Leave the Game Frozen if the Scene Unloads While Paused
    // ==========================================
    private void OnDestroy()
    {
        IsPaused = false;
        Time.timeScale = 1f;
    }

    // ==========================================
    // Update - Escape Toggles Pause; Refresh Save Availability While Open
    // ==========================================
    private void Update()
    {
        if (Keyboard.current == null) return;

        bool loadOpen = loadPanel != null && loadPanel.IsOpen;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (loadOpen) { loadPanel.Close(); return; }
            if (IsPaused) Resume(); else Pause();
        }

        if (IsPaused && !loadOpen && saveButton != null)
            saveButton.interactable = DialogueSystem.Instance != null && DialogueSystem.Instance.CanSaveNow;
    }

    // ==========================================
    // Pause - Freeze the Game and Reveal the Stop Menu
    // ==========================================
    public void Pause()
    {
        if (IsPaused) return;
        IsPaused = true;
        Time.timeScale = 0f;

        if (saveButton != null)
            saveButton.interactable = DialogueSystem.Instance != null && DialogueSystem.Instance.CanSaveNow;

        KillAnim();
        _animCo = StartCoroutine(Fade(true));
    }

    // ==========================================
    // Resume - Unfreeze the Game and Hide the Stop Menu
    // ==========================================
    public void Resume()
    {
        if (!IsPaused) return;
        if (loadPanel != null && loadPanel.IsOpen) loadPanel.Close();
        IsPaused = false;
        Time.timeScale = 1f;
        KillAnim();
        _animCo = StartCoroutine(Fade(false));
    }

    // ==========================================
    // OnSaveClicked - Open the Sub-Panel in Save Mode (writes a slot, adds an anomaly)
    // ==========================================
    private void OnSaveClicked()
    {
        clickSfx?.Play();
        if (loadPanel != null) loadPanel.Open(LoadPanelMode.Save, actNumber);
    }

    // ==========================================
    // OnLoadClicked - Open the Sub-Panel in Load Mode
    // ==========================================
    private void OnLoadClicked()
    {
        clickSfx?.Play();
        if (loadPanel != null) loadPanel.Open(LoadPanelMode.Load, actNumber);
    }

    // ==========================================
    // OnMenuClicked - Unfreeze, Halt Dialogue, Return to Main Menu
    // ==========================================
    private void OnMenuClicked()
    {
        clickSfx?.Play();
        IsPaused = false;
        Time.timeScale = 1f;
        DialogueSystem.Instance?.Stop();
        if (SceneController.Instance != null) SceneController.Instance.LoadMainMenu();
    }

    // ==========================================
    // Fade - Unscaled Alpha + Scale Tween (in or out)
    // ==========================================
    private IEnumerator Fade(bool show)
    {
        float fromA = show ? 0f : (pauseRoot != null ? pauseRoot.alpha : 1f);
        float toA = show ? 1f : 0f;
        float fromS = show ? scaleFrom : 1f;
        float toS = show ? 1f : scaleFrom;

        if (pauseRoot != null) { pauseRoot.interactable = show; pauseRoot.blocksRaycasts = show; }

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.Clamp01(t / fadeDuration);
            if (pauseRoot != null) pauseRoot.alpha = Mathf.Lerp(fromA, toA, p);
            if (pausePanel != null) pausePanel.localScale = Vector3.one * Mathf.Lerp(fromS, toS, p);
            yield return null;
        }

        if (pauseRoot != null) pauseRoot.alpha = toA;
        if (pausePanel != null) pausePanel.localScale = Vector3.one * toS;
        _animCo = null;
    }

    // ==========================================
    // ForceHidden - Zero Alpha, No Raycasts (GameObject stays ACTIVE to catch Escape)
    // ==========================================
    private void ForceHidden()
    {
        if (pauseRoot != null)
        {
            pauseRoot.alpha = 0f;
            pauseRoot.interactable = false;
            pauseRoot.blocksRaycasts = false;
        }
        if (pausePanel != null) pausePanel.localScale = Vector3.one * scaleFrom;
    }

    // ==========================================
    // KillAnim - Stop the Active Fade Coroutine
    // ==========================================
    private void KillAnim()
    {
        if (_animCo != null) { StopCoroutine(_animCo); _animCo = null; }
    }
}