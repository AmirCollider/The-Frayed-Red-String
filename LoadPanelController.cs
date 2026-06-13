// ==========================================
// LoadPanelController - Animated Save Slot Browser Panel
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LoadPanelController : MonoBehaviour
{
    // ==========================================
    // PlayerPrefs Key Templates (format with slot index)
    // ==========================================
    private const string KEY_HAS_DATA = "SaveSlot_{0}_HasData";
    private const string KEY_CHAPTER = "SaveSlot_{0}_Chapter";
    private const string KEY_ACT = "SaveSlot_{0}_Act";
    private const string KEY_MINUTES = "SaveSlot_{0}_Minutes";
    private const string KEY_DATE = "SaveSlot_{0}_Date";
    private const string KEY_SCREENSHOT = "SaveSlot_{0}_ScreenshotPath";

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
    [SerializeField] private TextMeshProUGUI noSaveText;
    [SerializeField] private TextMeshProUGUI slotIndicatorText;

    // ==========================================
    // Inspector — Overlay (transparent button closes panel)
    // ==========================================
    [Header("Overlay")]
    [SerializeField] private Button overlayCloseButton;

    // ==========================================
    // Inspector — Navigation
    // ==========================================
    [Header("Navigation")]
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;

    // ==========================================
    // Inspector — Save Slot Rows (assign 3 entries)
    // ==========================================
    [Header("Save Slots (size = 3)")]
    [SerializeField] private SaveSlotUI[] slots;

    // ==========================================
    // Inspector — Animation Timing
    // ==========================================
    [Header("Animation")]
    [SerializeField] private float openDuration = 0.22f;
    [SerializeField] private float closeDuration = 0.16f;
    [SerializeField] private float openScaleFrom = 0.94f;
    [SerializeField] private float slotFadeDuration = 0.18f;

    // ==========================================
    // Private State
    // ==========================================
    private bool _isOpen;
    private int _currentSlotIndex;
    private Coroutine _animCo;
    private Coroutine _slotFadeCo;

    // ==========================================
    // Awake — Wire Buttons and Force Hidden on Scene Load
    // ==========================================
    private void Awake()
    {
        ForceClosed();
        WireButtons();
    }

    // ==========================================
    // Update — Keyboard Navigation (Arrow Keys / A-D / Escape)
    // ==========================================
    private void Update()
    {
        if (!_isOpen) return;

        if (Keyboard.current == null) return;

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame)
        {
            TryNavigate(-1);
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame)
        {
            TryNavigate(1);
        }
        else if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Close();
        }
    }

    // ==========================================
    // WireButtons — Assign onClick Listeners for All Interactive Elements
    // ==========================================
    private void WireButtons()
    {
        if (overlayCloseButton != null) overlayCloseButton.onClick.AddListener(Close);
        if (backButton != null) backButton.onClick.AddListener(Close);
        if (prevButton != null) prevButton.onClick.AddListener(() => TryNavigate(-1));
        if (nextButton != null) nextButton.onClick.AddListener(() => TryNavigate(1));

        for (int i = 0; i < slots.Length; i++)
        {
            int idx = i;
            if (slots[i].loadButton != null) slots[i].loadButton.onClick.AddListener(() => OnLoadSlot(idx));
            if (slots[i].deleteButton != null) slots[i].deleteButton.onClick.AddListener(() => OnDeleteSlot(idx));
        }
    }

    // ==========================================
    // Open — Reset to Slot 0, Refresh Data, and Animate In
    // ==========================================
    public void Open()
    {
        if (_isOpen) return;
        _isOpen = true;
        _currentSlotIndex = 0;
        gameObject.SetActive(true);
        RefreshAllSlots();
        ShowSlotImmediate(0);
        KillAnim();
        _animCo = StartCoroutine(AnimateIn());
    }

    // ==========================================
    // Close — Animate Out and Deactivate
    // ==========================================
    public void Close()
    {
        if (!_isOpen) return;
        _isOpen = false;
        KillAnim();
        _animCo = StartCoroutine(AnimateOut());
    }

    // ==========================================
    // TryNavigate — Clamp Index and Start Fade Transition
    // ==========================================
    private void TryNavigate(int direction)
    {
        int next = _currentSlotIndex + direction;
        if (next < 0 || next >= slots.Length) return;
        if (_slotFadeCo != null) return;
        _slotFadeCo = StartCoroutine(FadeToSlot(next));
    }

    // ==========================================
    // FadeToSlot — Cross-Fade Out Current Slot, Cross-Fade In Target Slot
    // ==========================================
    private IEnumerator FadeToSlot(int newIndex)
    {
        float half = slotFadeDuration * 0.5f;

        // Fade out current slot
        CanvasGroup outCG = GetSlotCG(_currentSlotIndex);
        if (outCG != null)
        {
            float t = 0f;
            while (t < half)
            {
                t += Time.deltaTime;
                outCG.alpha = Mathf.Lerp(1f, 0f, t / half);
                yield return null;
            }
            outCG.alpha = 0f;
            outCG.interactable = false;
            outCG.blocksRaycasts = false;
        }
        slots[_currentSlotIndex].slotRect.gameObject.SetActive(false);

        // Switch index and update header / indicator
        _currentSlotIndex = newIndex;
        UpdateIndicator();
        UpdateHeaderText();

        // Fade in new slot
        slots[_currentSlotIndex].slotRect.gameObject.SetActive(true);
        CanvasGroup inCG = GetSlotCG(_currentSlotIndex);
        if (inCG != null)
        {
            inCG.alpha = 0f;
            inCG.interactable = false;
            inCG.blocksRaycasts = false;
            float t = 0f;
            while (t < half)
            {
                t += Time.deltaTime;
                inCG.alpha = Mathf.Lerp(0f, 1f, t / half);
                yield return null;
            }
            inCG.alpha = 1f;
            inCG.interactable = true;
            inCG.blocksRaycasts = true;
        }

        _slotFadeCo = null;
    }

    // ==========================================
    // ShowSlotImmediate — Instantly Display Target Slot, Hide All Others
    // ==========================================
    private void ShowSlotImmediate(int index)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].slotRect == null) continue;
            bool active = (i == index);
            slots[i].slotRect.gameObject.SetActive(active);
            CanvasGroup cg = GetSlotCG(i);
            if (cg != null)
            {
                cg.alpha = active ? 1f : 0f;
                cg.interactable = active;
                cg.blocksRaycasts = active;
            }
        }
        UpdateIndicator();
        UpdateHeaderText();
    }

    // ==========================================
    // RefreshAllSlots — Read PlayerPrefs and Populate All Slot UIs
    // ==========================================
    private void RefreshAllSlots()
    {
        for (int i = 0; i < slots.Length; i++)
            RefreshSlot(i);
    }

    // ==========================================
    // RefreshSlot — Populate a Single Slot Row from PlayerPrefs
    // ==========================================
    private void RefreshSlot(int i)
    {
        SaveSlotUI ui = slots[i];
        bool hasData = PlayerPrefs.GetInt(Fmt(KEY_HAS_DATA, i), 0) == 1;

        if (ui.slotNumberText != null)
            ui.slotNumberText.text = $"SLOT  {i + 1}";

        if (hasData)
        {
            string chapter = PlayerPrefs.GetString(Fmt(KEY_CHAPTER, i), "Unknown Chapter");
            int minutes = PlayerPrefs.GetInt(Fmt(KEY_MINUTES, i), 0);
            string date = PlayerPrefs.GetString(Fmt(KEY_DATE, i), "");

            if (ui.chapterText != null) ui.chapterText.text = chapter;
            if (ui.timestampText != null) ui.timestampText.text = $"{FmtTime(minutes)}   ·   {date}";

            if (ui.loadButton != null) ui.loadButton.gameObject.SetActive(true);
            if (ui.deleteButton != null) ui.deleteButton.gameObject.SetActive(true);
            if (ui.emptyOverlay != null) ui.emptyOverlay.SetActive(false);

            LoadScreenshot(ui, i);
        }
        else
        {
            if (ui.chapterText != null) ui.chapterText.text = "";
            if (ui.timestampText != null) ui.timestampText.text = "";

            if (ui.loadButton != null) ui.loadButton.gameObject.SetActive(false);
            if (ui.deleteButton != null) ui.deleteButton.gameObject.SetActive(false);
            if (ui.emptyOverlay != null) ui.emptyOverlay.SetActive(true);

            if (ui.screenshotImage != null)
                ui.screenshotImage.gameObject.SetActive(false);
        }
    }

    // ==========================================
    // LoadScreenshot — Read PNG from Persistent Path, Build and Assign Sprite
    // ==========================================
    private void LoadScreenshot(SaveSlotUI ui, int i)
    {
        if (ui.screenshotImage == null) return;

        string path = PlayerPrefs.GetString(Fmt(KEY_SCREENSHOT, i), "");
        if (!string.IsNullOrEmpty(path) && File.Exists(path))
        {
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            ui.screenshotImage.sprite = Sprite.Create(
                tex,
                new Rect(0f, 0f, tex.width, tex.height),
                new Vector2(0.5f, 0.5f));
            ui.screenshotImage.gameObject.SetActive(true);
        }
        else
        {
            ui.screenshotImage.gameObject.SetActive(false);
        }
    }

    // ==========================================
    // UpdateHeaderText — Swap TitleText / NoSaveText Based on Current Slot Data
    // ==========================================
    private void UpdateHeaderText()
    {
        bool hasData = PlayerPrefs.GetInt(Fmt(KEY_HAS_DATA, _currentSlotIndex), 0) == 1;

        if (titleText != null) titleText.text = hasData ? "LOAD GAME" : "";
        if (noSaveText != null) noSaveText.gameObject.SetActive(!hasData);
    }

    // ==========================================
    // UpdateIndicator — Refresh "1 / 3" Label and Prev/Next Button States
    // ==========================================
    private void UpdateIndicator()
    {
        if (slotIndicatorText != null)
            slotIndicatorText.text = $"{_currentSlotIndex + 1}  /  {slots.Length}";

        if (prevButton != null)
            prevButton.interactable = (_currentSlotIndex > 0);

        if (nextButton != null)
            nextButton.interactable = (_currentSlotIndex < slots.Length - 1);
    }

    // ==========================================
    // OnLoadSlot — Read Act Index from Prefs and Load Scene
    // ==========================================
    private void OnLoadSlot(int i)
    {
        if (PlayerPrefs.GetInt(Fmt(KEY_HAS_DATA, i), 0) == 0) return;
        int act = PlayerPrefs.GetInt(Fmt(KEY_ACT, i), 1);
        Close();
        if (SceneController.Instance != null)
            SceneController.Instance.LoadAct(act);
    }

    // ==========================================
    // OnDeleteSlot — Erase All PlayerPrefs Keys for the Slot
    // ==========================================
    private void OnDeleteSlot(int i)
    {
        PlayerPrefs.DeleteKey(Fmt(KEY_HAS_DATA, i));
        PlayerPrefs.DeleteKey(Fmt(KEY_CHAPTER, i));
        PlayerPrefs.DeleteKey(Fmt(KEY_ACT, i));
        PlayerPrefs.DeleteKey(Fmt(KEY_MINUTES, i));
        PlayerPrefs.DeleteKey(Fmt(KEY_DATE, i));
        PlayerPrefs.DeleteKey(Fmt(KEY_SCREENSHOT, i));
        PlayerPrefs.Save();
        RefreshSlot(i);
        UpdateHeaderText();
    }

    // ==========================================
    // GetSlotCG — Return CanvasGroup on Slot Row Root, Null-Safe
    // ==========================================
    private CanvasGroup GetSlotCG(int i)
    {
        if (slots == null || i < 0 || i >= slots.Length || slots[i].slotRect == null) return null;
        return slots[i].slotRect.GetComponent<CanvasGroup>();
    }

    // ==========================================
    // AnimateIn — Fade + Scale Panel In
    // ==========================================
    private IEnumerator AnimateIn()
    {
        SetCG(0f, false);
        if (panelContent != null)
            panelContent.localScale = Vector3.one * openScaleFrom;

        float t = 0f;
        while (t < openDuration)
        {
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0f, 1f, t / openDuration);
            SetCG(p, false);
            if (panelContent != null)
                panelContent.localScale = Vector3.Lerp(Vector3.one * openScaleFrom, Vector3.one, p);
            yield return null;
        }

        SetCG(1f, true);
        if (panelContent != null) panelContent.localScale = Vector3.one;
        _animCo = null;
    }

    // ==========================================
    // AnimateOut — Fade + Scale Panel Out, Then Deactivate
    // ==========================================
    private IEnumerator AnimateOut()
    {
        float startAlpha = panelCanvasGroup != null ? panelCanvasGroup.alpha : 1f;
        float t = 0f;

        while (t < closeDuration)
        {
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0f, 1f, t / closeDuration);
            SetCG(Mathf.Lerp(startAlpha, 0f, p), false);
            if (panelContent != null)
                panelContent.localScale = Vector3.Lerp(Vector3.one, Vector3.one * openScaleFrom, p);
            yield return null;
        }

        ForceClosed();
        _animCo = null;
    }

    // ==========================================
    // SetCG — Apply Alpha and Interaction State to Panel CanvasGroup
    // ==========================================
    private void SetCG(float alpha, bool interact)
    {
        if (panelCanvasGroup == null) return;
        panelCanvasGroup.alpha = alpha;
        panelCanvasGroup.interactable = interact;
        panelCanvasGroup.blocksRaycasts = interact;
    }

    // ==========================================
    // ForceClosed — Instantly Zero-Alpha and Deactivate
    // ==========================================
    private void ForceClosed()
    {
        SetCG(0f, false);
        if (panelContent != null)
            panelContent.localScale = Vector3.one * openScaleFrom;
        gameObject.SetActive(false);
    }

    // ==========================================
    // KillAnim — Stop All Active Animation Coroutines
    // ==========================================
    private void KillAnim()
    {
        if (_animCo != null) { StopCoroutine(_animCo); _animCo = null; }
        if (_slotFadeCo != null) { StopCoroutine(_slotFadeCo); _slotFadeCo = null; }
    }

    // ==========================================
    // Fmt — PlayerPrefs Key String Format Helper
    // ==========================================
    private static string Fmt(string template, int index)
        => string.Format(template, index);

    // ==========================================
    // FmtTime — Total Minutes to H:MM Display String
    // ==========================================
    private static string FmtTime(int totalMinutes)
    {
        int h = totalMinutes / 60;
        int m = totalMinutes % 60;
        return h > 0 ? $"{h}:{m:D2}" : $"0:{m:D2}";
    }
}

// ==========================================
// SaveSlotUI — Serializable Inspector Binding for a Single Slot Row
// ==========================================
[System.Serializable]
public class SaveSlotUI
{
    public RectTransform slotRect;
    public TextMeshProUGUI slotNumberText;
    public TextMeshProUGUI chapterText;
    public TextMeshProUGUI timestampText;
    public Button loadButton;
    public Button deleteButton;
    public GameObject emptyOverlay;
    public Image screenshotImage;
}