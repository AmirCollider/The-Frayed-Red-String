// ==========================================
// FrameController - Visible Border/Frame State and Panel Manager
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;
using UnityEngine.Events;

public class FrameController : MonoBehaviour
{
    // ==========================================
    // Singleton Instance
    // ==========================================
    public static FrameController Instance { get; private set; }

    // ==========================================
    // Inspector - Frame Border Panels
    // ==========================================
    [Header("Frame Border Panels")]
    [SerializeField] private RectTransform frameTop;
    [SerializeField] private RectTransform frameBottom;
    [SerializeField] private RectTransform frameLeft;
    [SerializeField] private RectTransform frameRight;

    // ==========================================
    // Inspector - Initialization
    // ==========================================
    [Header("Initialization")]
    [SerializeField] private bool startHidden = true;

    // ==========================================
    // State
    // ==========================================
    public FrameState CurrentState { get; private set; } = FrameState.Normal;

    // ==========================================
    // Events
    // ==========================================
    public UnityEvent<FrameState> OnFrameStateChanged = new UnityEvent<FrameState>();

    // ==========================================
    // Awake - Singleton Enforcement, Persistence, and Optional Hidden Init
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

        if (startHidden)
            SetAllBarsToZero();
    }

    // ==========================================
    // SetState - Transition to New Frame State and Broadcast Event
    // ==========================================
    public void SetState(FrameState newState)
    {
        CurrentState = newState;
        OnFrameStateChanged.Invoke(newState);
    }

    // ==========================================
    // SetAllBarsToZero - Collapse All Panels to Zero Size
    // ==========================================
    private void SetAllBarsToZero()
    {
        SetBarSize(frameTop, 0f, false);
        SetBarSize(frameBottom, 0f, false);
        SetBarSize(frameLeft, 0f, true);
        SetBarSize(frameRight, 0f, true);
    }

    // ==========================================
    // SetBarSize - Apply Dimension to a Single Border Panel
    // ==========================================
    private void SetBarSize(RectTransform rt, float size, bool isHorizontal)
    {
        if (rt == null) return;
        Vector2 sd = rt.sizeDelta;
        if (isHorizontal) sd.x = size;
        else sd.y = size;
        rt.sizeDelta = sd;
    }

    // ==========================================
    // Accessors - Expose Panels for FrameAnimator
    // ==========================================
    public RectTransform FrameTop => frameTop;
    public RectTransform FrameBottom => frameBottom;
    public RectTransform FrameLeft => frameLeft;
    public RectTransform FrameRight => frameRight;
}