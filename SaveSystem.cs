// ==========================================
// SaveSystem - MetaFileSystem Save/Load Core (Mid-Scene Snapshots, 3 Slots)
// AmirCollider Games - The Frayed Red String
// ==========================================

using System;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    // ==========================================
    // Singleton Instance (persistent)
    // ==========================================
    public static SaveSystem Instance { get; private set; }

    // ==========================================
    // Slot Configuration
    // ==========================================
    public const int SLOT_COUNT = 3;

    // ==========================================
    // PlayerPrefs Key Templates
    // ==========================================
    private const string KEY_HAS_DATA = "SaveSlot_{0}_HasData";
    private const string KEY_CHAPTER = "SaveSlot_{0}_Chapter";
    private const string KEY_ACT = "SaveSlot_{0}_Act";
    private const string KEY_MINUTES = "SaveSlot_{0}_Minutes";
    private const string KEY_DATE = "SaveSlot_{0}_Date";
    private const string KEY_BLOB = "SaveSlot_{0}_Blob";

    // ==========================================
    // Play-Clock State
    // ==========================================
    private float _sessionStartRealtime;
    private int _baselineMinutes;

    // ==========================================
    // Pending Load Handoff (read by the target Act manager on scene load)
    // ==========================================
    private SaveData _pendingLoad;
    private int _pendingLoadAct = -1;

    // ==========================================
    // Awake - Singleton Enforcement, Persistence, Start the Play-Clock
    // ==========================================
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _sessionStartRealtime = Time.realtimeSinceStartup;
        _baselineMinutes = 0;
    }

    // ==========================================
    // BeginNewGameClock - Reset Play-Clock When Starting a Fresh Game
    // ==========================================
    public void BeginNewGameClock()
    {
        _baselineMinutes = 0;
        _sessionStartRealtime = Time.realtimeSinceStartup;
    }

    // ==========================================
    // CurrentPlayMinutes - Baseline Plus This Session's Elapsed Real Minutes
    // ==========================================
    private int CurrentPlayMinutes()
        => _baselineMinutes + Mathf.FloorToInt((Time.realtimeSinceStartup - _sessionStartRealtime) / 60f);

    // ==========================================
    // Slot Queries (read by the Load Panel)
    // ==========================================
    public bool HasSlot(int i) => PlayerPrefs.GetInt(Fmt(KEY_HAS_DATA, i), 0) == 1;
    public int SlotAct(int i) => PlayerPrefs.GetInt(Fmt(KEY_ACT, i), 1);
    public int SlotMinutes(int i) => PlayerPrefs.GetInt(Fmt(KEY_MINUTES, i), 0);
    public string SlotChapter(int i) => PlayerPrefs.GetString(Fmt(KEY_CHAPTER, i), "");
    public string SlotDate(int i) => PlayerPrefs.GetString(Fmt(KEY_DATE, i), "");

    // ==========================================
    // ReadSlot - Deserialize the Full SaveData Blob for a Slot
    // ==========================================
    public SaveData ReadSlot(int i)
    {
        if (!HasSlot(i)) return null;
        string json = PlayerPrefs.GetString(Fmt(KEY_BLOB, i), "");
        if (string.IsNullOrEmpty(json)) return null;
        return JsonUtility.FromJson<SaveData>(json);
    }

    // ==========================================
    // CaptureCurrent - Snapshot the Live Runtime State Into a SaveData
    // ==========================================
    public SaveData CaptureCurrent(int actNumber)
    {
        SaveData d = new SaveData();
        d.actNumber = actNumber;
        d.entryIndex = DialogueSystem.Instance != null ? DialogueSystem.Instance.CurrentIndex : 0;
        d.language = GameManager.Instance != null ? GameManager.Instance.CurrentLanguage : ConstantsConfig.LANG_ENGLISH;

        if (Act1Manager.Instance != null)
            d.affectionScore = Act1Manager.Instance.AffectionScore;

        if (BackgroundManager.Instance != null)
            d.backgroundId = (int)BackgroundManager.Instance.CurrentBackground;
        if (FrameController.Instance != null)
            d.frameState = (int)FrameController.Instance.CurrentState;
        if (ColorGrader.Instance != null)
            d.colorPalette = (int)ColorGrader.Instance.CurrentPalette;

        if (CharacterRegistry.Instance != null)
        {
            CharacterSpriteController haru = CharacterRegistry.Instance.Get(ConstantsConfig.SPEAKER_HARU);
            CharacterSpriteController yua = CharacterRegistry.Instance.Get(ConstantsConfig.SPEAKER_YUA);
            if (haru != null) { d.haruState = (int)haru.CurrentState; d.haruPosition = (int)haru.CurrentPosition; }
            if (yua != null) { d.yuaState = (int)yua.CurrentState; d.yuaPosition = (int)yua.CurrentPosition; }
        }

        if (BranchRecord.Instance != null)
            d.branchRecordJson = BranchRecord.Instance.ExportJson();

        d.playMinutes = CurrentPlayMinutes();
        d.dateLabel = DateTime.Now.ToString("yyyy-MM-dd  HH:mm");
        d.chapterLabel = ChapterLabelForAct(actNumber);
        return d;
    }

    // ==========================================
    // SaveToSlot - Capture, Write All Keys, and Register a MetaFileSystem Anomaly
    // ==========================================
    public bool SaveToSlot(int slot, int actNumber)
    {
        if (slot < 0 || slot >= SLOT_COUNT) return false;

        SaveData d = CaptureCurrent(actNumber);
        PlayerPrefs.SetInt(Fmt(KEY_HAS_DATA, slot), 1);
        PlayerPrefs.SetInt(Fmt(KEY_ACT, slot), d.actNumber);
        PlayerPrefs.SetInt(Fmt(KEY_MINUTES, slot), d.playMinutes);
        PlayerPrefs.SetString(Fmt(KEY_CHAPTER, slot), d.chapterLabel);
        PlayerPrefs.SetString(Fmt(KEY_DATE, slot), d.dateLabel);
        PlayerPrefs.SetString(Fmt(KEY_BLOB, slot), JsonUtility.ToJson(d));
        PlayerPrefs.Save();

        if (MetaFileSystem.Instance != null)
            MetaFileSystem.Instance.RegisterSaveAnomaly();

        // ==========================================
        // Auto-Screenshot — capture the current scene (dialogue box hidden) for the card
        // ==========================================
        if (SaveScreenshot.Instance != null)
            SaveScreenshot.Instance.Capture(slot);

        return true;
    }

    // ==========================================
    // SlotScreenshotPath - Persistent PNG Path for a Slot (used by the saved-row prefab)
    // ==========================================
    public string SlotScreenshotPath(int i) => SaveScreenshot.PathForSlot(i);

    // ==========================================
    // DeleteSlot - Erase All Keys for a Slot
    // ==========================================
    public void DeleteSlot(int slot)
    {
        PlayerPrefs.DeleteKey(Fmt(KEY_HAS_DATA, slot));
        PlayerPrefs.DeleteKey(Fmt(KEY_CHAPTER, slot));
        PlayerPrefs.DeleteKey(Fmt(KEY_ACT, slot));
        PlayerPrefs.DeleteKey(Fmt(KEY_MINUTES, slot));
        PlayerPrefs.DeleteKey(Fmt(KEY_DATE, slot));
        PlayerPrefs.DeleteKey(Fmt(KEY_BLOB, slot));
        PlayerPrefs.Save();
    }

    // ==========================================
    // RequestLoad - Stage the Snapshot and Route to Its Act Scene
    // ==========================================
    public void RequestLoad(int slot)
    {
        SaveData d = ReadSlot(slot);
        if (d == null) return;

        _pendingLoad = d;
        _pendingLoadAct = d.actNumber;
        _baselineMinutes = d.playMinutes;
        _sessionStartRealtime = Time.realtimeSinceStartup;

        if (GameManager.Instance != null && !string.IsNullOrEmpty(d.language))
            GameManager.Instance.SetLanguage(d.language);

        if (SceneController.Instance != null)
            SceneController.Instance.LoadActOrMenu(d.actNumber);
    }

    // ==========================================
    // ConsumePendingLoad - The Target Act Manager Claims Its Snapshot Once on Start
    // ==========================================
    public SaveData ConsumePendingLoad(int actNumber)
    {
        if (_pendingLoad == null || _pendingLoadAct != actNumber) return null;
        SaveData d = _pendingLoad;
        _pendingLoad = null;
        _pendingLoadAct = -1;
        return d;
    }

    // ==========================================
    // Fmt - PlayerPrefs Key Format Helper
    // ==========================================
    private static string Fmt(string template, int index) => string.Format(template, index);

    // ==========================================
    // ChapterLabelForAct - Human-Readable Card Title
    // ==========================================
    private static string ChapterLabelForAct(int act)
    {
        switch (act)
        {
            case 1: return "Act I — The Mirage";
            case 2: return "Act II — The Trap";
            case 3: return "Act III — The Strip";
            default: return "Act " + act;
        }
    }
}