// ==========================================
// Act3Manager - Act 3 Pacing Controller and Glitch Sequence Orchestrator
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;
using UnityEngine.Events;

public class Act3Manager : MonoBehaviour
{
    // ==========================================
    // Singleton Instance (per-scene — no DontDestroyOnLoad)
    // ==========================================
    public static Act3Manager Instance { get; private set; }

    // ==========================================
    // Inspector — Opening Background
    // ==========================================
    [Header("Opening Background")]
    [SerializeField] private BackgroundID openingBackground = BackgroundID.SchoolCorridorSunset;
    [SerializeField] private BackgroundTransitionMode openingTransition = BackgroundTransitionMode.Instant;

    // ==========================================
    // Inspector — Haru Starting Configuration
    // ==========================================
    [Header("Haru Start")]
    [SerializeField] private CharacterPosition haruStartPosition = CharacterPosition.Right;
    [SerializeField] private CharacterState haruStartState = CharacterState.WaitingToTalk;

    // ==========================================
    // Inspector — Yua Starting Configuration
    // ==========================================
    [Header("Yua Start")]
    [SerializeField] private CharacterPosition yuaStartPosition = CharacterPosition.Left;
    [SerializeField] private CharacterState yuaStartState = CharacterState.WaitingToTalk;

    // ==========================================
    // Inspector — Main Dialogue Sequence
    // ==========================================
    [Header("Dialogue Sequence")]
    [SerializeField] private DialogueSequence act3MainSequence;

    // ==========================================
    // Inspector — Fourth-Wall Breaker (Direct Player Address)
    // ==========================================
    [Header("Fourth-Wall Breaker")]
    [SerializeField] private FourthWallBreaker fourthWallBreaker;

    // ==========================================
    // Inspector — Frame Break Sequence (Photorealistic Reveal)
    // ==========================================
    [Header("Frame Break")]
    [SerializeField] private FrameBreakSequence frameBreakSequence;

    // ==========================================
    // Inspector — Audio Glitch Trigger
    // ==========================================
    [Header("Audio Glitch")]
    [SerializeField] private AudioGlitchTrigger audioGlitchTrigger;

    // ==========================================
    // Events
    // ==========================================
    [Header("Events")]
    public UnityEvent OnAct3Complete = new UnityEvent();

    // ==========================================
    // Awake - Singleton Enforcement
    // ==========================================
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // ==========================================
    // Start - Initialize Scene and Begin Act 3 Sequence
    // ==========================================
    private void Start()
    {
        InitializeBackground();
        InitializeCharacters();
        InitializeFrame();
        StartAct3();
    }

    // ==========================================
    // InitializeBackground - Apply Opening Background via BackgroundManager
    // ==========================================
    private void InitializeBackground()
    {
        if (BackgroundManager.Instance == null) return;
        BackgroundManager.Instance.ForceBackground(openingBackground, openingTransition);
    }

    // ==========================================
    // InitializeCharacters - Position Both Characters to Starting Configuration
    // ==========================================
    private void InitializeCharacters()
    {
        if (CharacterRegistry.Instance == null) return;

        CharacterRegistry.Instance.SetState(ConstantsConfig.SPEAKER_HARU, haruStartState, true);
        CharacterRegistry.Instance.SetPosition(ConstantsConfig.SPEAKER_HARU, haruStartPosition, true);

        CharacterRegistry.Instance.SetState(ConstantsConfig.SPEAKER_YUA, yuaStartState, true);
        CharacterRegistry.Instance.SetPosition(ConstantsConfig.SPEAKER_YUA, yuaStartPosition, true);
    }

    // ==========================================
    // InitializeFrame - Enter Normal Border State at Act 3 Scene Start
    // ==========================================
    private void InitializeFrame()
    {
        if (FrameController.Instance == null) return;
        FrameController.Instance.SetState(FrameState.Normal);
    }

    // ==========================================
    // StartAct3 - Register Listeners and Begin Main Sequence
    // ==========================================
    private void StartAct3()
    {
        if (act3MainSequence == null)
        {
            Debug.LogWarning("[Act3Manager] act3MainSequence is not assigned. Assign in Inspector.");
            return;
        }

        if (DialogueSystem.Instance == null)
        {
            Debug.LogError("[Act3Manager] DialogueSystem.Instance is null.");
            return;
        }

        DialogueSystem.Instance.OnSequenceComplete.AddListener(OnMainSequenceComplete);
        DialogueSystem.Instance.OnGameEventTriggered.AddListener(OnGameEventTriggeredHandler);
        DialogueSystem.Instance.Play(act3MainSequence);
    }

    // ==========================================
    // OnMainSequenceComplete - Unsubscribe Listeners and Fire Completion Event
    // ==========================================
    private void OnMainSequenceComplete()
    {
        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.OnSequenceComplete.RemoveListener(OnMainSequenceComplete);
            DialogueSystem.Instance.OnGameEventTriggered.RemoveListener(OnGameEventTriggeredHandler);
        }

        OnAct3Complete.Invoke();
    }

    // ==========================================
    // OnGameEventTriggeredHandler - Route Glitch Sequence Beats from Dialogue Lines
    // ==========================================
    private void OnGameEventTriggeredHandler(string eventId)
    {
        switch (eventId)
        {
            case "TriggerColdPalette":
                ColorGrader.Instance?.SetPalette(ColorPalette.Cold);
                break;

            case "TriggerDesaturatedPalette":
                ColorGrader.Instance?.SetPalette(ColorPalette.Desaturated);
                break;

            case "TriggerBloodredPalette":
                ColorGrader.Instance?.SetPalette(ColorPalette.Bloodred);
                break;

            case "BeginFourthWallBreak":
                fourthWallBreaker?.TriggerBreak();
                break;

            case "TriggerFrameBreak":
                frameBreakSequence?.TriggerBreak();
                break;

            case "TriggerAudioGlitch":
                audioGlitchTrigger?.TriggerSequence();
                break;

            case "TriggerBlackout":
                ScreenBlackout.Instance?.TriggerBlackout();
                break;
        }
    }
}