// ==========================================
// Act1Manager - Act 1 Pacing Controller and Affection Tracker
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;
using UnityEngine.Events;

public class Act1Manager : MonoBehaviour
{
    // ==========================================
    // Singleton Instance (per-scene — no DontDestroyOnLoad)
    // ==========================================
    public static Act1Manager Instance { get; private set; }

    // ==========================================
    // Inspector — Opening Background
    // ==========================================
    [Header("Opening Background")]
    [SerializeField] private BackgroundID openingBackground = BackgroundID.SunnyClassroomDay;
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
    [SerializeField] private DialogueSequence act1MainSequence;

    // ==========================================
    // Inspector — Red String Visual
    // ==========================================
    [Header("Red String")]
    [SerializeField] private RedStringVisual redString;

    // ==========================================
    // Inspector — Affection Tracking
    // ==========================================
    [Header("Affection Metric")]
    [SerializeField] private int affectionScore = 0;
    [SerializeField] private int affectionThresholdHigh = 5;
    [SerializeField] private int affectionThresholdLow = -3;

    // ==========================================
    // Inspector — BranchRecord Keys for Act 3 Payoff
    // ==========================================
    [Header("BranchRecord Keys")]
    [SerializeField] private string branchKeyAffectionLevel = "Act1_AffectionLevel";

    // ==========================================
    // Events
    // ==========================================
    [Header("Events")]
    public UnityEvent OnAct1Complete = new UnityEvent();

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
    // Start - Initialize Scene and Begin Act 1 Sequence
    // ==========================================
    private void Start()
    {
        InitializeBackground();
        InitializeCharacters();
        InitializeRedString();
        InitializeFrame();
        StartAct1();
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
    // InitializeRedString - Force Hidden Until a Story Beat Calls Show
    // ==========================================
    private void InitializeRedString()
    {
        if (redString == null) return;
        redString.ForceHide();
    }

    // ==========================================
    // InitializeFrame - Enter Normal Border State at Act 1 Scene Start
    // ==========================================
    private void InitializeFrame()
    {
        if (FrameController.Instance == null) return;
        FrameController.Instance.SetState(FrameState.Normal);
    }

    // ==========================================
    // StartAct1 - Register Completion Listener and Begin Main Sequence
    // ==========================================
    private void StartAct1()
    {
        if (act1MainSequence == null)
        {
            Debug.LogWarning("[Act1Manager] act1MainSequence is not assigned. Assign in Inspector.");
            return;
        }

        if (DialogueSystem.Instance == null)
        {
            Debug.LogError("[Act1Manager] DialogueSystem.Instance is null.");
            return;
        }

        DialogueSystem.Instance.OnSequenceComplete.AddListener(OnMainSequenceComplete);
        DialogueSystem.Instance.OnChoiceMade.AddListener(OnChoiceMadeHandler);
        DialogueSystem.Instance.OnGameEventTriggered.AddListener(OnGameEventTriggeredHandler);
        DialogueSystem.Instance.Play(act1MainSequence);
    }

    // ==========================================
    // OnMainSequenceComplete - Commit Score, Fire Event, Transition to Act 2
    // ==========================================
    private void OnMainSequenceComplete()
    {
        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.OnSequenceComplete.RemoveListener(OnMainSequenceComplete);
            DialogueSystem.Instance.OnChoiceMade.RemoveListener(OnChoiceMadeHandler);
            DialogueSystem.Instance.OnGameEventTriggered.RemoveListener(OnGameEventTriggeredHandler);
        }

        CommitAffectionScore();
        OnAct1Complete.Invoke();
        TransitionToAct2();
    }

    // ==========================================
    // CommitAffectionScore - Write Tier to BranchRecord for Act 3 Payoff
    // ==========================================
    private void CommitAffectionScore()
    {
        if (BranchRecord.Instance == null) return;

        int tier = affectionScore >= affectionThresholdHigh ? 2
                 : affectionScore <= affectionThresholdLow ? 0
                 : 1;

        BranchRecord.Instance.Record(branchKeyAffectionLevel, tier);
    }

    // ==========================================
    // ModifyAffection - Called Externally per Branch Choice Result
    // ==========================================
    public void ModifyAffection(int delta)
    {
        affectionScore += delta;
    }

    // ==========================================
    // ShowRedString - Reveal String Visual from Story Beat
    // ==========================================
    public void ShowRedString()
    {
        redString?.Show();
    }

    // ==========================================
    // HideRedString - Conceal String Visual from Story Beat
    // ==========================================
    public void HideRedString()
    {
        redString?.Hide();
    }

    // ==========================================
    // OnChoiceMadeHandler - Route Affection Delta from Player Choices to Score
    // ==========================================
    private void OnChoiceMadeHandler(string branchId, int index, int affectionDelta)
    {
        ModifyAffection(affectionDelta);
    }

    // ==========================================
    // OnGameEventTriggeredHandler - Dispatch Named Gameplay Events from Dialogue Lines
    // ==========================================
    private void OnGameEventTriggeredHandler(string eventId)
    {
        switch (eventId)
        {
            case "ShowRedString": ShowRedString(); break;
            case "HideRedString": HideRedString(); break;
        }
    }

    // ==========================================
    // TransitionToAct2 - Load Act 2 Scene via SceneController
    // ==========================================
    private void TransitionToAct2()
    {
        if (SceneController.Instance == null)
        {
            Debug.LogError("[Act1Manager] SceneController.Instance is null.");
            return;
        }
        SceneController.Instance.LoadAct(2);
    }

    // ==========================================
    // Accessors
    // ==========================================
    public int AffectionScore => affectionScore;
}