// ==========================================
// LoadPanelController - All-Slots Save/Load Browser (MetaFileSystem-Aware)
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
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
    // Inspector — Save Slot Cards (size = 3, all shown at once)
    // ==========================================
    [Header("Save Slots (size = 3)")]
    [SerializeField] private SaveSlotUI[] slots;

    // ==========================================
    // Inspector — Corruption Decoys (optional MetaFileSystem flavor)
    // ==========================================
    [Header("Corruption Decoys (optional)")]
    [SerializeField] private RectTransform corruptedContainer;
    [SerializeField] private RectTransform corruptedSlotTemplate;
    [SerializeField] private int maxDecoysShown = 6;

    // ==========================================
    // Inspector — Animation (UNSCALED — also runs while paused at Time.timeScale = 0)
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

    // ==========================================
    // IsOpen - Public Query (used by PauseMenuController for Escape routing)
    // ==========================================
    public bool IsOpen => _isOpen;

    // ==========================================
    // Awake - Force Hidden and Wire Buttons
    // ==========================================
    private void Awake()
    {
        ForceClosed();

        if (overlayCloseButton != null) overlayCloseButton.onClick.AddListener(Close);
        if (backButton != null) backButton.onClick.AddListener(Close);

        for (int i = 0; i < slots.Length; i++)
        {
            int idx = i;
            if (slots[i].loadButton != null) slots[i].loadButton.onClick.AddListener(() => OnSlotAction(idx));
            if (slots[i].deleteButton != null) slots[i].deleteButton.onClick.AddListener(() => OnDeleteSlot(idx));
        }
    }

    // ==========================================
    // Open - Enter Load or Save Mode, Refresh All Cards, Animate In
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
    // RefreshAll - Title, Every Slot Card, and the Anomaly Row
    // ==========================================
    private void RefreshAll()
    {
        if (titleText != null)
            titleText.text = _mode == LoadPanelMode.Save ? "SAVE" : "LOAD";

        for (int i = 0; i < slots.Length; i++)
            RefreshSlot(i);

        RefreshAnomalies();
    }

    // ==========================================
    // RefreshSlot - Populate a Single Card from SaveSystem and the Active Mode
    // ==========================================
    private void RefreshSlot(int i)
    {
        if (i < 0 || i >= slots.Length) return;

        SaveSlotUI ui = slots[i];
        bool has = SaveSystem.Instance != null && SaveSystem.Instance.HasSlot(i);

        if (ui.slotRect != null) ui.slotRect.gameObject.SetActive(true);
        if (ui.slotNumberText != null) ui.slotNumberText.text = $"SLOT  {i + 1}";
        if (ui.screenshotImage != null) ui.screenshotImage.gameObject.SetActive(false);

        if (has)
        {
            if (ui.chapterText != null) ui.chapterText.text = SaveSystem.Instance.SlotChapter(i);
            if (ui.timestampText != null)
                ui.timestampText.text = $"{FmtTime(SaveSystem.Instance.SlotMinutes(i))}   ·   {SaveSystem.Instance.SlotDate(i)}";
            if (ui.emptyOverlay != null) ui.emptyOverlay.SetActive(false);
            if (ui.deleteButton != null) ui.deleteButton.gameObject.SetActive(true);
        }
        else
        {
            if (ui.chapterText != null) ui.chapterText.text = "";
            if (ui.timestampText != null) ui.timestampText.text = "";
            if (ui.emptyOverlay != null) ui.emptyOverlay.SetActive(_mode == LoadPanelMode.Load);
            if (ui.deleteButton != null) ui.deleteButton.gameObject.SetActive(false);
        }

        if (ui.loadButton != null)
        {
            ui.loadButton.gameObject.SetActive(true);
            ui.loadButton.interactable = _mode == LoadPanelMode.Save || has;
        }

        if (ui.actionLabelText != null)
        {
            if (_mode == LoadPanelMode.Save)
                ui.actionLabelText.text = has ? "OVERWRITE" : "SAVE HERE";
            else
                ui.actionLabelText.text = has ? "LOAD" : "EMPTY";
        }
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
            RefreshSlot(i);
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
    // OnDeleteSlot - Erase a Slot and Refresh the Card
    // ==========================================
    private void OnDeleteSlot(int i)
    {
        if (SaveSystem.Instance == null) return;
        SaveSystem.Instance.DeleteSlot(i);
        RefreshSlot(i);
    }

    // ==========================================
    // RefreshAnomalies - Show the Per-Save Corruption Count and Optional Decoy Slots
    // ==========================================
    private void RefreshAnomalies()
    {
        int n = MetaFileSystem.Instance != null ? MetaFileSystem.Instance.CorruptionCount : 0;

        if (anomalyText != null)
            anomalyText.text = n > 0 ? $"⚠  FILE ANOMALIES DETECTED: {n}" : "";

        if (corruptedContainer == null || corruptedSlotTemplate == null) return;

        for (int c = corruptedContainer.childCount - 1; c >= 0; c--)
        {
            Transform child = corruptedContainer.GetChild(c);
            if (child == corruptedSlotTemplate.transform) continue;
            Destroy(child.gameObject);
        }
        corruptedSlotTemplate.gameObject.SetActive(false);

        int show = Mathf.Min(n, maxDecoysShown);
        for (int k = 0; k < show; k++)
        {
            RectTransform decoy = Instantiate(corruptedSlotTemplate, corruptedContainer);
            decoy.gameObject.SetActive(true);
            TextMeshProUGUI label = decoy.GetComponentInChildren<TextMeshProUGUI>(true);
            if (label != null)
                label.text = $"SLOT ?? · 0x{(0xC0FFEE + k * 0x1F3D):X6} · CORRUPTED";
        }
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

// ==========================================
// SaveSlotUI - Serializable Inspector Binding for a Single Slot Card
// ==========================================
[System.Serializable]
public class SaveSlotUI
{
    public RectTransform slotRect;
    public TextMeshProUGUI slotNumberText;
    public TextMeshProUGUI chapterText;
    public TextMeshProUGUI timestampText;
    public TextMeshProUGUI actionLabelText;
    public Button loadButton;
    public Button deleteButton;
    public GameObject emptyOverlay;
    public Image screenshotImage;
}