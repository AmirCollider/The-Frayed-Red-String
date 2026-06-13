// ==========================================
// ConstantsConfig - Global Constants & Configuration
// AmirCollider Games - The Frayed Red String
// ==========================================

public static class ConstantsConfig
{
    // ==========================================
    // Scene Names
    // ==========================================
    public const string SCENE_MAIN_MENU = "MainMenuScene";
    public const string SCENE_ACT_1 = "Act1Scene";
    public const string SCENE_ACT_2 = "Act2Scene";
    public const string SCENE_ACT_3 = "Act3Scene";
    public const string SCENE_ACT_4 = "Act4Scene";
    public const string SCENE_ACT_5 = "Act5Scene";
    public const string SCENE_CREDITS = "CreditsScene";

    // ==========================================
    // PlayerPrefs Keys
    // ==========================================
    public const string PREF_LANGUAGE = "SelectedLanguage";
    public const string PREF_GAME_COMPLETED = "GameCompleted";
    public const string PREF_MENU_VARIANT = "MenuVariant";
    public const string PREF_MASTER_VOLUME = "MasterVolume";
    public const string PREF_BGM_VOLUME = "BGMVolume";
    public const string PREF_SFX_VOLUME = "SFXVolume";

    // ==========================================
    // Language Keys
    // ==========================================
    public const string LANG_ENGLISH = "EN";
    public const string LANG_JAPANESE = "JP";

    // ==========================================
    // Speaker IDs — Match speakerId field in DialogueLine assets
    // ==========================================
    public const string SPEAKER_HARU = "Haru";
    public const string SPEAKER_YUA = "Yua";
    public const string SPEAKER_SYSTEM = "";   // Fourth-wall / narrator — no nameplate shown

    // ==========================================
    // AudioMixer Exposed Parameter Names
    // (must match exactly what you expose in the Mixer asset)
    // ==========================================
    public const string MIXER_MASTER = "MasterVol";
    public const string MIXER_BGM = "BGMVol";
    public const string MIXER_SFX = "SFXVol";
    public const string MIXER_AMBIENCE = "AmbienceVol";

    // ==========================================
    // Transition Durations (seconds)
    // ==========================================
    public const float FADE_DURATION_DEFAULT = 0.5f;
    public const float FADE_DURATION_FAST = 0.15f;
    public const float FADE_DURATION_SLOW = 1.2f;
    public const float TYPEWRITER_DEFAULT_CPS = 40f;

    // ==========================================
    // Base Resolution
    // ==========================================
    public const int BASE_WIDTH = 1920;
    public const int BASE_HEIGHT = 1080;
}