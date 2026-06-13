// ==========================================
// HaruBedroomScene - Act 2 Climax: Haru's Direct-Address Fourth-Wall Sequence
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;

public class HaruBedroomScene : MonoBehaviour
{
    // ==========================================
    // Singleton Instance (per-scene — no DontDestroyOnLoad)
    // ==========================================
    public static HaruBedroomScene Instance { get; private set; }

    // ==========================================
    // Inspector — Fourth-Wall Breaker Reference
    // ==========================================
    [Header("Fourth-Wall Breaker")]
    [SerializeField] private FourthWallBreaker fourthWallBreaker;

    // ==========================================
    // Inspector — Bedroom Background Override (None = retain current Act 2 background)
    // ==========================================
    [Header("Bedroom Background (Optional)")]
    [SerializeField] private BackgroundID bedroomBackground = BackgroundID.None;
    [SerializeField] private BackgroundTransitionMode bedroomTransition = BackgroundTransitionMode.Crossfade;

    // ==========================================
    // Inspector — Haru Repositioning for Direct-Address Framing
    // ==========================================
    [Header("Haru Repositioning")]
    [SerializeField] private CharacterPosition haruAddressPosition = CharacterPosition.Center;
    [SerializeField] private CharacterState haruAddressState = CharacterState.SadImploring;

    // ==========================================
    // Inspector — Yua Visibility During Sequence
    // ==========================================
    [Header("Yua Visibility")]
    [SerializeField] private bool hideYuaDuringSequence = true;

    // ==========================================
    // Private State — Yua Restore Cache
    // ==========================================
    private CharacterPosition _yuaPositionBeforeSequence;
    private CharacterState _yuaStateBeforeSequence;
    private bool _sequenceActive;

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
    // BeginFourthWallSequence - Isolate Haru on Screen and Trigger Fourth-Wall Break
    // ==========================================
    public void BeginFourthWallSequence()
    {
        if (_sequenceActive) return;
        _sequenceActive = true;

        if (bedroomBackground != BackgroundID.None && BackgroundManager.Instance != null)
            BackgroundManager.Instance.SetBackground(bedroomBackground, bedroomTransition);

        CacheAndHideYua();

        CharacterRegistry.Instance?.SetState(ConstantsConfig.SPEAKER_HARU, haruAddressState, false);
        CharacterRegistry.Instance?.SetPosition(ConstantsConfig.SPEAKER_HARU, haruAddressPosition, false);

        fourthWallBreaker?.TriggerBreak();
    }

    // ==========================================
    // EndFourthWallSequence - Restore Camera, Dialogue Style, and Yua Visibility
    // ==========================================
    public void EndFourthWallSequence()
    {
        if (!_sequenceActive) return;
        _sequenceActive = false;

        fourthWallBreaker?.EndBreak();
        RestoreYua();
    }

    // ==========================================
    // CacheAndHideYua - Store Current Yua Transform State and Move Her Off Screen
    // ==========================================
    private void CacheAndHideYua()
    {
        if (!hideYuaDuringSequence || CharacterRegistry.Instance == null) return;

        CharacterSpriteController yua = CharacterRegistry.Instance.Get(ConstantsConfig.SPEAKER_YUA);
        if (yua == null) return;

        _yuaPositionBeforeSequence = yua.CurrentPosition;
        _yuaStateBeforeSequence = yua.CurrentState;
        yua.Hide();
    }

    // ==========================================
    // RestoreYua - Return Yua to Her Pre-Sequence Position and State
    // ==========================================
    private void RestoreYua()
    {
        if (!hideYuaDuringSequence || CharacterRegistry.Instance == null) return;

        CharacterSpriteController yua = CharacterRegistry.Instance.Get(ConstantsConfig.SPEAKER_YUA);
        if (yua == null) return;

        yua.SetState(_yuaStateBeforeSequence, true);
        yua.SetPosition(_yuaPositionBeforeSequence, false);
    }

    // ==========================================
    // IsSequenceActive - Whether the Bedroom Fourth-Wall Sequence Is Currently Running
    // ==========================================
    public bool IsSequenceActive => _sequenceActive;
}