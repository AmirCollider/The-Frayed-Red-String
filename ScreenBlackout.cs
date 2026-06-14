// ==========================================
// ScreenBlackout - Hard Cut to Black, Configurable Hold, Triggers Next Act Load
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScreenBlackout : MonoBehaviour
{
    // ==========================================
    // Singleton Instance (per-scene — no DontDestroyOnLoad)
    // ==========================================
    public static ScreenBlackout Instance { get; private set; }

    // ==========================================
    // Inspector - Overlay Image (full-stretch, Color=(0,0,0,0), RaycastTarget=false at rest)
    // ==========================================
    [Header("Blackout Overlay")]
    [SerializeField] private Image blackoutImage;

    // ==========================================
    // Inspector - Fade In (0 = instant hard cut)
    // ==========================================
    [Header("Fade In")]
    [SerializeField] private float fadeInDuration = 0f;

    // ==========================================
    // Inspector - Hold Before Transition
    // ==========================================
    [Header("Hold")]
    [SerializeField] private float holdDuration = 2.5f;

    // ==========================================
    // Inspector - Next Act Transition
    // ==========================================
    [Header("Next Act Transition")]
    [SerializeField] private int nextAct = 4;
    [SerializeField] private bool autoLoadNextAct = true;

    // ==========================================
    // Events
    // ==========================================
    [Header("Events")]
    public UnityEvent OnBlackoutEngaged = new UnityEvent();
    public UnityEvent OnHoldComplete = new UnityEvent();

    // ==========================================
    // Private State
    // ==========================================
    private Coroutine _routineCo;

    // ==========================================
    // Awake - Singleton Enforcement, Force Overlay Transparent and Non-Blocking
    // ==========================================
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SetAlpha(0f);
        if (blackoutImage != null) blackoutImage.raycastTarget = false;
    }

    // ==========================================
    // TriggerBlackout - External Entry Point Called by Act3Manager
    // ==========================================
    public void TriggerBlackout()
    {
        if (_routineCo != null) StopCoroutine(_routineCo);
        _routineCo = StartCoroutine(BlackoutRoutine());
    }

    // ==========================================
    // BlackoutRoutine - Cut/Fade to Black, Hold, Then Load Next Act
    // ==========================================
    private IEnumerator BlackoutRoutine()
    {
        if (blackoutImage != null) blackoutImage.raycastTarget = true;

        if (fadeInDuration <= 0f)
        {
            SetAlpha(1f);
        }
        else
        {
            float elapsed = 0f;
            while (elapsed < fadeInDuration)
            {
                elapsed += Time.deltaTime;
                SetAlpha(Mathf.Lerp(0f, 1f, elapsed / fadeInDuration));
                yield return null;
            }
            SetAlpha(1f);
        }

        OnBlackoutEngaged.Invoke();

        if (holdDuration > 0f)
            yield return new WaitForSeconds(holdDuration);

        OnHoldComplete.Invoke();

        if (autoLoadNextAct && SceneController.Instance != null)
            SceneController.Instance.LoadAct(nextAct);

        _routineCo = null;
    }

    // ==========================================
    // SetAlpha - Apply Alpha to Blackout Overlay Color
    // ==========================================
    private void SetAlpha(float alpha)
    {
        if (blackoutImage == null) return;
        Color c = blackoutImage.color;
        c.a = Mathf.Clamp01(alpha);
        blackoutImage.color = c;
    }

    // ==========================================
    // IsEngaged - Whether the Overlay is Currently Fully Opaque
    // ==========================================
    public bool IsEngaged => blackoutImage != null && blackoutImage.color.a >= 0.999f;
}