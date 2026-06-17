// ==========================================
// MetaFileSystem - Dynamic File Corruption / Per-Save Anomaly Counter
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;
using UnityEngine.Events;

public class MetaFileSystem : MonoBehaviour
{
    // ==========================================
    // Singleton Instance (persistent)
    // ==========================================
    public static MetaFileSystem Instance { get; private set; }

    // ==========================================
    // PlayerPrefs Key
    // ==========================================
    private const string PREF_CORRUPTION = "MetaCorruptionCount";

    // ==========================================
    // Corruption State — Rises One Notch Per Save (persists across playthroughs)
    // ==========================================
    public int CorruptionCount { get; private set; }

    // ==========================================
    // Event — Broadcast When Corruption Changes (UI, subliminal filter hooks)
    // ==========================================
    [System.Serializable] public class CorruptionEvent : UnityEvent<int> { }
    public CorruptionEvent OnCorruptionChanged = new CorruptionEvent();

    // ==========================================
    // Awake - Singleton Enforcement, Persistence, Load Count
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
        CorruptionCount = PlayerPrefs.GetInt(PREF_CORRUPTION, 0);
    }

    // ==========================================
    // RegisterSaveAnomaly - Each Save Frays the File System One Notch Further
    // ==========================================
    public void RegisterSaveAnomaly()
    {
        CorruptionCount++;
        PlayerPrefs.SetInt(PREF_CORRUPTION, CorruptionCount);
        PlayerPrefs.Save();
        OnCorruptionChanged.Invoke(CorruptionCount);
    }

    // ==========================================
    // ResetCorruption - Wipe Anomalies (debug / fresh file)
    // ==========================================
    public void ResetCorruption()
    {
        CorruptionCount = 0;
        PlayerPrefs.SetInt(PREF_CORRUPTION, 0);
        PlayerPrefs.Save();
        OnCorruptionChanged.Invoke(0);
    }
}