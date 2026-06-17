// ==========================================
// LocalizedFontController - Per-Act TMP Font Swapper (EN/JP by Language)
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizedFontController : MonoBehaviour
{
    // ==========================================
    // Singleton Instance (per-scene — no DontDestroyOnLoad)
    // ==========================================
    public static LocalizedFontController Instance { get; private set; }

    // ==========================================
    // Inspector — Phase Font Pair (assign THIS act's EN / JP SDF asset)
    // ==========================================
    [Header("Phase Fonts (per act)")]
    [SerializeField] private TMP_FontAsset englishFont;
    [SerializeField] private TMP_FontAsset japaneseFont;

    // ==========================================
    // Inspector — Static TMP Targets (DialogueText, SpeakerLabel, MonologueText)
    //   Runtime-instantiated choice labels are handled by ChoiceUI, not listed here.
    // ==========================================
    [Header("Static TMP Targets")]
    [SerializeField] private List<TMP_Text> targets = new List<TMP_Text>();

    // ==========================================
    // Inspector — Auto-Collect (find every TMP text in the scene automatically)
    // ==========================================
    [Header("Auto-Collect")]
    [SerializeField] private bool autoCollectTargets = true;
    [SerializeField] private bool includeInactive = true;

    // ==========================================
    // CurrentFont - Language-Correct Font for This Phase (EN fallback)
    // ==========================================
    public TMP_FontAsset CurrentFont
    {
        get
        {
            bool jp = GameManager.Instance != null
                   && GameManager.Instance.CurrentLanguage == ConstantsConfig.LANG_JAPANESE;
            TMP_FontAsset resolved = jp ? japaneseFont : englishFont;
            return resolved != null ? resolved : englishFont;
        }
    }

    // ==========================================
    // Awake - Singleton Enforcement
    // ==========================================
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        CollectTargets();
    }

    // ==========================================
    // OnEnable - Subscribe to Language Change and Apply Immediately
    // ==========================================
    private void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnLanguageChanged.AddListener(OnLanguageChanged);
        ApplyFont();
    }

    // ==========================================
    // Start - Re-Apply After All Scene Objects Have Initialized
    // ==========================================
    private void Start()
    {
        ApplyFont();
    }

    // ==========================================
    // OnDisable - Unsubscribe from Language Change
    // ==========================================
    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnLanguageChanged.RemoveListener(OnLanguageChanged);
    }

    // ==========================================
    // OnDestroy - Release Singleton Reference
    // ==========================================
    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    // ==========================================
    // OnLanguageChanged - Language Toggle Callback (re-apply matching font)
    // ==========================================
    private void OnLanguageChanged(string languageCode)
    {
        ApplyFont();
    }

    // ==========================================
    // CollectTargets - Auto-Discover Every TMP Text in the Loaded Scene
    //   Includes inactive objects (the dialogue box deactivates itself in Awake)
    //   and merges with any targets assigned by hand in the Inspector. Runtime
    //   choice-button labels do not exist yet at Awake, so they are not captured.
    // ==========================================
    public void CollectTargets()
    {
        if (!autoCollectTargets) return;
        if (targets == null) targets = new List<TMP_Text>();

        FindObjectsInactive inactiveMode = includeInactive
            ? FindObjectsInactive.Include
            : FindObjectsInactive.Exclude;

        TMP_Text[] found = FindObjectsByType<TMP_Text>(inactiveMode);
        foreach (TMP_Text t in found)
            if (t != null && !targets.Contains(t)) targets.Add(t);
    }

    // ==========================================
    // ApplyFont - Push CurrentFont onto Every Assigned Static TMP Target
    // ==========================================
    public void ApplyFont()
    {
        TMP_FontAsset font = CurrentFont;
        if (font == null || targets == null) return;

        foreach (TMP_Text t in targets)
            if (t != null) t.font = font;
    }

    // ==========================================
    // RegisterTarget - Add a TMP Target at Runtime and Apply the Current Font
    // ==========================================
    public void RegisterTarget(TMP_Text target)
    {
        if (target == null) return;
        if (!targets.Contains(target)) targets.Add(target);
        target.font = CurrentFont;
    }
}