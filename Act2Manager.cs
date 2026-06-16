// ==========================================
// Act2Manager - Act 2 Pacing Controller and Escalation Tracker
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;
using UnityEngine.Events;

public class Act2Manager : MonoBehaviour
{
    // ==========================================
    // Singleton Instance (per-scene — no DontDestroyOnLoad)
    // ==========================================
    public static Act2Manager Instance { get; private set; }

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
    [SerializeField] private DialogueSequence act2MainSequence;

    // ==========================================
    // Inspector — Fourth-Wall Sequence Controller
    // ==========================================
    [Header("Fourth-Wall Sequence")]
    [SerializeField] private HaruBedroomScene haruBedroomScene;

    // ==========================================
    // Events
    // ==========================================
    [Header("Events")]
    public UnityEvent OnAct2Complete = new UnityEvent();

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
    // Start - Initialize Scene and Begin Act 2 Sequence
    // ==========================================
    private void Start()
    {
        InitializeBackground();
        InitializeCharacters();
        InitializeFrame();
        StartAct2();
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
    // InitializeFrame - Enter Normal Border State at Act 2 Scene Start
    // ==========================================
    private void InitializeFrame()
    {
        if (FrameController.Instance == null) return;
        FrameController.Instance.SetState(FrameState.Normal);
    }

    // ==========================================
    // StartAct2 - Register Completion and Event Listeners, Begin Main Sequence
    // ==========================================
    private void StartAct2()
    {
        if (act2MainSequence == null)
        {
            Debug.LogWarning("[Act2Manager] act2MainSequence is not assigned. Assign in Inspector.");
            return;
        }

        if (DialogueSystem.Instance == null)
        {
            Debug.LogError("[Act2Manager] DialogueSystem.Instance is null.");
            return;
        }

        DialogueSystem.Instance.OnSequenceComplete.AddListener(OnMainSequenceComplete);
        DialogueSystem.Instance.OnGameEventTriggered.AddListener(OnGameEventTriggeredHandler);
        DialogueSystem.Instance.Play(act2MainSequence);
    }

    // ==========================================
    // OnMainSequenceComplete - Unsubscribe, Fire Completion Event, Transition to Act 3
    // ==========================================
    private void OnMainSequenceComplete()
    {
        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.OnSequenceComplete.RemoveListener(OnMainSequenceComplete);
            DialogueSystem.Instance.OnGameEventTriggered.RemoveListener(OnGameEventTriggeredHandler);
        }

        OnAct2Complete.Invoke();
        TransitionToAct3();
    }

    // ==========================================
    // OnGameEventTriggeredHandler - Route Manipulation Milestones and Fourth-Wall Cues
    // ==========================================
    private void OnGameEventTriggeredHandler(string eventId)
    {
        switch (eventId)
        {
            case "BeginFourthWallBreak":
                PsychologicalManipulationTracker.Instance?.RecordMilestone(ManipulationMilestone.FourthWallBreak);
                haruBedroomScene?.BeginFourthWallSequence();
                break;

            case "EndFourthWallBreak":
                haruBedroomScene?.EndFourthWallSequence();
                break;

            default:
                if (System.Enum.TryParse(eventId, true, out ManipulationMilestone milestone))
                    PsychologicalManipulationTracker.Instance?.RecordMilestone(milestone);
                break;
        }
    }

    // ==========================================
    // TransitionToAct3 - Load Act 3 Scene via SceneController
    // ==========================================
    private void TransitionToAct3()
    {
        if (SceneController.Instance == null)
        {
            Debug.LogError("[Act2Manager] SceneController.Instance is null.");
            return;
        }
        SceneController.Instance.LoadAct(3);
    }
}