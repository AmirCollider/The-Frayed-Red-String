// ==========================================
// GameManager - Persistent Singleton & Global State
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // ==========================================
    // Singleton Instance
    // ==========================================
    public static GameManager Instance { get; private set; }

    // ==========================================
    // Global State Properties
    // ==========================================
    public string CurrentLanguage { get; private set; } = ConstantsConfig.LANG_ENGLISH;
    public bool GameCompleted { get; private set; } = false;
    public int MenuVariant { get; private set; } = 0;

    // ==========================================
    // Global Events
    // ==========================================
    public UnityEvent<string> OnLanguageChanged = new UnityEvent<string>();
    public UnityEvent OnGameCompleted = new UnityEvent();

    // ==========================================
    // Awake - Singleton Enforcement & Persistence
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
        LoadGlobalState();
    }

    // ==========================================
    // LoadGlobalState - Read PlayerPrefs on Startup
    // ==========================================
    private void LoadGlobalState()
    {
        CurrentLanguage = PlayerPrefs.GetString(ConstantsConfig.PREF_LANGUAGE, ConstantsConfig.LANG_ENGLISH);
        GameCompleted = PlayerPrefs.GetInt(ConstantsConfig.PREF_GAME_COMPLETED, 0) == 1;
        MenuVariant = PlayerPrefs.GetInt(ConstantsConfig.PREF_MENU_VARIANT, 0);
    }

    // ==========================================
    // SetLanguage - Switch Active Game Language
    // ==========================================
    public void SetLanguage(string languageCode)
    {
        CurrentLanguage = languageCode;
        PlayerPrefs.SetString(ConstantsConfig.PREF_LANGUAGE, languageCode);
        PlayerPrefs.Save();
        OnLanguageChanged.Invoke(languageCode);
    }

    // ==========================================
    // SetGameCompleted - Lock End State & Unlock Variant Menu
    // ==========================================
    public void SetGameCompleted()
    {
        if (GameCompleted) return;

        GameCompleted = true;
        MenuVariant = 1;

        PlayerPrefs.SetInt(ConstantsConfig.PREF_GAME_COMPLETED, 1);
        PlayerPrefs.SetInt(ConstantsConfig.PREF_MENU_VARIANT, 1);
        PlayerPrefs.Save();

        OnGameCompleted.Invoke();
    }
}