// ==========================================
// DialogueLine - Single Dialogue Line Data (ScriptableObject)
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueLine", menuName = "The Frayed Red String/Dialogue Line")]
public class DialogueLine : ScriptableObject
{
    // ==========================================
    // Speaker Identity
    // ==========================================
    [Header("Speaker")]
    public string speakerId = "";
    public CharacterPosition speakerPosition = CharacterPosition.Left;

    // ==========================================
    // Text Content — Bilingual
    // ==========================================
    [Header("Text — English / Japanese")]
    [TextArea(3, 8)] public string textEN = "";
    [TextArea(3, 8)] public string textJP = "";

    // ==========================================
    // Character Sprite State Override
    // ==========================================
    [Header("Character State (sprite name — leave blank to keep current)")]
    public string characterStateOverride = "";

    // ==========================================
    // Background Override
    // ==========================================
    [Header("Background (leave None to keep current)")]
    public BackgroundID backgroundId = BackgroundID.None;
    public BackgroundTransitionMode backgroundTransition = BackgroundTransitionMode.Crossfade;

    // ==========================================
    // Audio Events
    // ==========================================
    [Header("Audio")]
    public AudioEvent voiceEvent;
    public AudioEvent sfxEvent;

    // ==========================================
    // Behavioural Flags
    // ==========================================
    [Header("Flags")]
    public bool isInnerMonologue = false;
    public bool isFourthWall = false;
    public bool autoAdvance = false;
    public float autoAdvanceDelay = 1.5f;

    // ==========================================
    // Gameplay Event Hook (fires DialogueSystem.OnGameEventTriggered when line displays)
    // ==========================================
    [Header("Gameplay Event")]
    public string gameEventId = "";

    // ==========================================
    // GetActiveText - Returns Language-Appropriate Line Text
    // ==========================================
    public string GetActiveText()
    {
        if (GameManager.Instance == null) return textEN;
        if (GameManager.Instance.CurrentLanguage == ConstantsConfig.LANG_JAPANESE
            && !string.IsNullOrEmpty(textJP))
            return textJP;
        return textEN;
    }
}