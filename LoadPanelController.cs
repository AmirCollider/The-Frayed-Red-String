// ==========================================
// LoadPanelController - Auto-Built Save/Load Browser (empty vs saved row prefabs)
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ==========================================
// LoadPanelMode - Browser Operates as a Loader or a Saver
// ==========================================
public enum LoadPanelMode
{
    Load = 0,
    Save = 1
}

public class LoadPanelController : MonoBehaviour
{
    // ==========================================
    // Inspector — Panel Root
    // ==========================================
    [Header("Panel Root")]
    [SerializeField] private CanvasGroup panelCanvasGroup;
    [SerializeField] private RectTransform panelContent;

    // ==========================================
    // Inspector — Header
    // ==========================================
    [Header("Header")]
    [SerializeField] private TextMeshProUGUI titleText;

    // ==========================================
    // Inspector — Overlay / Back
    // ==========================================
    [Header("Overlay / Back")]
    [SerializeField] private Button overlayCloseButton;
    [SerializeField] private Button backButton;

    // ==========================================
    // Inspector — Row Generation (TWO prefabs: empty slot vs saved slot)
    // ==========================================
    [Header("Row Generation")]
    [SerializeField] private RectTransform slotsContainer;
    [SerializeField] private SaveSlotRow emptyRowPrefab;
    [SerializeField] private SaveSlotRow savedRowPrefab;

    // ==========================================
    // Inspector — Animation (UNSCALED — runs while paused at Time.timeScale = 0)
    // ==========================================
    [Header("Animation (unscaled time)")]
    [SerializeField] private float openDuration = 0.18f;
    [SerializeField] private float closeDuration = 0.14f;
    [SerializeField] private float openScaleFrom = 0.96f;

    // ==========================================
    // Private State
    // ==========================================
    private bool _isOpen;
    private LoadPanelMode _mode = LoadPanelMode.Load;
    private int _actNumber;
    private Coroutine _animCo;
    private readonly List<GameObject> _spawned = new List<GameObject>();

    // ==========================================
    // IsOpen - Public Query (used by PauseMenuController for Escape routing)
    // ==========================================
    public bool IsOpen => _isOpen;

    // ==========================================
    // Awake - Force Hidden and Wire Static Buttons
    // ==========================================
    private void Awake()
    {
        ForceClosed();
        if (overlayCloseButton != null) overlayCloseButton.onClick.AddListener(Close);
        if (backButton != null) backButton.onClick.AddListener(Close);
    }

    // ==========================================
    // Open - Enter a Mode, Rebuild Rows, Animate In
    // ==========================================
    public void Open(LoadPanelMode mode, int actNumber)
    {
        _mode = mode;
        _actNumber = actNumber;

        gameObject.SetActive(true);
        Rebuild();

        if (_isOpen) return;
        _isOpen = true;
        KillAnim();
        _animCo = StartCoroutine(Anim(true));
    }

    // ==========================================
    // Close - Animate Out and Deactivate
    // ==========================================
    public void Close()
    {
        if (!_isOpen) return;
        _isOpen = false;
        KillAnim();
        _animCo = StartCoroutine(Anim(false));
    }

    // ==========================================
    // Rebuild - Destroy Old Rows, Spawn the Correct Prefab per Slot
    // ==========================================
    private void Rebuild()
    {
        if (titleText != null)
            titleText.text = _mode == LoadPanelMode.Save ? "SAVE" : "LOAD";

        for (int k = 0; k < _spawned.Count; k++)
            if (_spawned[k] != null) Destroy(_spawned[k]);
        _spawned.Clear();

        if (slotsContainer == null || emptyRowPrefab == null || savedRowPrefab == null)
        {
            Debug.LogError("[LoadPanelController] slotsContainer / emptyRowPrefab / savedRowPrefab not assigned.");
            return;
        }

        emptyRowPrefab.gameObject.SetActive(false);
        savedRowPrefab.gameObject.SetActive(false);

        int count = SaveSystem.Instance != null ? SaveSystem.SLOT_COUNT : 3;
        for (int i = 0; i < count; i++)
        {
            bool has = SaveSystem.Instance != null && SaveSystem.Instance.HasSlot(i);
            int idx = i;

            if (has)
            {
                SaveSlotRow row = Instantiate(savedRowPrefab, slotsContainer);
                row.gameObject.SetActive(true);
                row.Bind(idx, () => OnLoadOrSave(idx), () => OnDelete(idx));
                row.SetLoadInteractable(true);
                row.ApplyScreenshot(SaveSystem.Instance.SlotScreenshotPath(idx));
                _spawned.Add(row.gameObject);
            }
            else
            {
                SaveSlotRow row = Instantiate(emptyRowPrefab, slotsContainer);
                row.gameObject.SetActive(true);
                // In SAVE mode an empty slot must be clickable (to save into it); in LOAD mode it stays dead.
                row.Bind(idx, () => OnLoadOrSave(idx), null);
                row.SetLoadInteractable(_mode == LoadPanelMode.Save);
                _spawned.Add(row.gameObject);
            }
        }
    }

    // ==========================================
    // OnLoadOrSave - Save Into the Slot (Save mode) or Load It (Load mode)
    // ==========================================
    private void OnLoadOrSave(int i)
    {
        if (SaveSystem.Instance == null) return;

        if (_mode == LoadPanelMode.Save)
        {
            SaveSystem.Instance.SaveToSlot(i, _actNumber);
            Rebuild();
        }
        else
        {
            if (!SaveSystem.Instance.HasSlot(i)) return;
            Close();
            SaveSystem.Instance.RequestLoad(i);
        }
    }

    // ==========================================
    // OnDelete - Erase a Slot and Rebuild the List
    // ==========================================
    private void OnDelete(int i)
    {
        if (SaveSystem.Instance == null) return;
        SaveSystem.Instance.DeleteSlot(i);
        Rebuild();
    }

    // ==========================================
    // Anim - Unscaled Alpha + Scale Tween (in or out)
    // ==========================================
    private IEnumerator Anim(bool open)
    {
        float dur = open ? openDuration : closeDuration;
        float fromA = open ? 0f : (panelCanvasGroup != null ? panelCanvasGroup.alpha : 1f);
        float toA = open ? 1f : 0f;
        float fromS = open ? openScaleFrom : 1f;
        float toS = open ? 1f : openScaleFrom;

        if (panelCanvasGroup != null) { panelCanvasGroup.interactable = open; panelCanvasGroup.blocksRaycasts = open; }

        float t = 0f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t / dur));
            if (panelCanvasGroup != null) panelCanvasGroup.alpha = Mathf.Lerp(fromA, toA, p);
            if (panelContent != null) panelContent.localScale = Vector3.one * Mathf.Lerp(fromS, toS, p);
            yield return null;
        }

        if (panelCanvasGroup != null) panelCanvasGroup.alpha = toA;
        if (panelContent != null) panelContent.localScale = Vector3.one * toS;
        if (!open) ForceClosed();
        _animCo = null;
    }

    // ==========================================
    // ForceClosed - Instant Zero-Alpha and Deactivate
    // ==========================================
    private void ForceClosed()
    {
        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.alpha = 0f;
            panelCanvasGroup.interactable = false;
            panelCanvasGroup.blocksRaycasts = false;
        }
        if (panelContent != null) panelContent.localScale = Vector3.one * openScaleFrom;
        gameObject.SetActive(false);
    }

    // ==========================================
    // KillAnim - Stop the Active Animation Coroutine
    // ==========================================
    private void KillAnim()
    {
        if (_animCo != null) { StopCoroutine(_animCo); _animCo = null; }
    }
}