// ==========================================
// LoadPanelController - Auto-Built Save/Load Browser (rows generated at runtime)
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
    [SerializeField] private TextMeshProUGUI anomalyText;

    // ==========================================
    // Inspector — Overlay / Back
    // ==========================================
    [Header("Overlay / Back")]
    [SerializeField] private Button overlayCloseButton;
    [SerializeField] private Button backButton;

    // ==========================================
    // Inspector — Row Generation (ONE prefab + ONE container; rows are auto-built)
    // ==========================================
    [Header("Row Generation")]
    [SerializeField] private RectTransform slotsContainer;     // parent with a Vertical Layout Group
    [SerializeField] private SaveSlotRow rowPrefab;            // the prefab below; one is spawned per slot

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
    private readonly List<SaveSlotRow> _spawnedRows = new List<SaveSlotRow>();
    private bool _rowsBuilt;

    // ==========================================
    // IsOpen - Public Query (used by PauseMenuController for Escape routing)
    // ==========================================
    public bool IsOpen => _isOpen;

    // ==========================================
    // Awake - Force Hidden and Wire the Static Buttons
    // ==========================================
    private void Awake()
    {
        ForceClosed();
        if (overlayCloseButton != null) overlayCloseButton.onClick.AddListener(Close);
        if (backButton != null) backButton.onClick.AddListener(Close);
    }

    // ==========================================
    // Open - Enter a Mode, Build/Refresh Rows, Animate In
    // ==========================================
    public void Open(LoadPanelMode mode, int actNumber)
    {
        _mode = mode;
        _actNumber = actNumber;

        if (_isOpen)
        {
            RefreshAll();
            return;
        }

        _isOpen = true;
        gameObject.SetActive(true);
        BuildRowsIfNeeded();
        RefreshAll();
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
    // BuildRowsIfNeeded - Instantiate Exactly SLOT_COUNT Rows From the Prefab Once
    // ==========================================
    private void BuildRowsIfNeeded()
    {
        if (_rowsBuilt) return;
        if (slotsContainer == null || rowPrefab == null)
        {
            Debug.LogError("[LoadPanelController] slotsContainer or rowPrefab is not assigned.");
            return;
        }

        // The prefab may live inside the container as a hidden template — never show it.
        rowPrefab.gameObject.SetActive(false);

        int count = SaveSystem.Instance != null ? SaveSystem.SLOT_COUNT : 3;
        for (int i = 0; i < count; i++)
        {
            SaveSlotRow row = Instantiate(rowPrefab, slotsContainer);
            row.gameObject.SetActive(true);
            int idx = i;
            row.Bind(idx, () => OnSlotAction(idx), () => OnDeleteSlot(idx));
            _spawnedRows.Add(row);
        }

        _rowsBuilt = true;
    }

    // ==========================================
    // RefreshAll - Title, Every Row, and the Anomaly Row
    // ==========================================
    private void RefreshAll()
    {
        if (titleText != null)
            titleText.text = _mode == LoadPanelMode.Save ? "SAVE" : "LOAD";

        for (int i = 0; i < _spawnedRows.Count; i++)
            RefreshRow(i);

        RefreshAnomalies();
    }

    // ==========================================
    // RefreshRow - Populate a Single Spawned Row From SaveSystem and the Active Mode
    // ==========================================
    private void RefreshRow(int i)
    {
        if (i < 0 || i >= _spawnedRows.Count) return;

        SaveSlotRow row = _spawnedRows[i];
        bool has = SaveSystem.Instance != null && SaveSystem.Instance.HasSlot(i);

        string chapter = has ? SaveSystem.Instance.SlotChapter(i) : "";
        string stamp = has
            ? $"{FmtTime(SaveSystem.Instance.SlotMinutes(i))}   ·   {SaveSystem.Instance.SlotDate(i)}"
            : "";

        string action;
        bool actionInteractable;
        if (_mode == LoadPanelMode.Save)
        {
            action = has ? "OVERWRITE" : "SAVE HERE";
            actionInteractable = true;
        }
        else
        {
            action = has ? "LOAD" : "EMPTY";
            actionInteractable = has;
        }

        bool showEmptyOverlay = !has && _mode == LoadPanelMode.Load;
        row.SetData(i, has, chapter, stamp, action, actionInteractable, showEmptyOverlay);
    }

    // ==========================================
    // OnSlotAction - Save Into / Load From the Tapped Slot
    // ==========================================
    private void OnSlotAction(int i)
    {
        if (SaveSystem.Instance == null) return;

        if (_mode == LoadPanelMode.Save)
        {
            SaveSystem.Instance.SaveToSlot(i, _actNumber);
            RefreshRow(i);
            RefreshAnomalies();
        }
        else
        {
            if (!SaveSystem.Instance.HasSlot(i)) return;
            Close();
            SaveSystem.Instance.RequestLoad(i);
        }
    }

    // ==========================================
    // OnDeleteSlot - Erase a Slot and Refresh Its Row
    // ==========================================
    private void OnDeleteSlot(int i)
    {
        if (SaveSystem.Instance == null) return;
        SaveSystem.Instance.DeleteSlot(i);
        RefreshRow(i);
    }

    // ==========================================
    // RefreshAnomalies - Show the Per-Save Corruption Count
    // ==========================================
    private void RefreshAnomalies()
    {
        int n = MetaFileSystem.Instance != null ? MetaFileSystem.Instance.CorruptionCount : 0;
        if (anomalyText != null)
            anomalyText.text = n > 0 ? $"⚠  FILE ANOMALIES DETECTED: {n}" : "";
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

    // ==========================================
    // FmtTime - Total Minutes to H:MM Display String
    // ==========================================
    private static string FmtTime(int totalMinutes)
    {
        int h = totalMinutes / 60;
        int m = totalMinutes % 60;
        return h > 0 ? $"{h}:{m:D2}" : $"0:{m:D2}";
    }
}