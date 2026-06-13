// ==========================================
// MenuButtonAnimator - Idle Breath, Hover Pop, Press Punch
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class MenuButtonAnimator : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler
{
    // ==========================================
    // Inspector — Idle Breath
    // ==========================================
    [Header("Idle Breath")]
    [SerializeField] private float breathAmplitude = 0.018f;
    [SerializeField] private float breathFrequency = 0.85f;
    [SerializeField][Range(0f, 1f)] private float breathPhaseOffset = 0f;

    // ==========================================
    // Inspector — Hover
    // ==========================================
    [Header("Hover")]
    [SerializeField] private float hoverScaleMultiplier = 1.08f;
    [SerializeField] private float hoverInDuration = 0.11f;
    [SerializeField] private float hoverOutDuration = 0.20f;

    // ==========================================
    // Inspector — Press Punch
    // ==========================================
    [Header("Press")]
    [SerializeField] private float pressScaleMultiplier = 0.91f;
    [SerializeField] private float pressDuration = 0.06f;
    [SerializeField] private float releaseBounceMultiplier = 1.05f;
    [SerializeField] private float releaseBounceDuration = 0.10f;
    [SerializeField] private float releaseSettleDuration = 0.09f;

    // ==========================================
    // Inspector — Tint
    // ==========================================
    [Header("Tint")]
    [SerializeField] private Color idleTint = new Color(0.88f, 0.88f, 0.88f, 1f);
    [SerializeField] private Color hoverTint = Color.white;

    // ==========================================
    // Private State
    // ==========================================
    private RectTransform _rt;
    private Image _img;
    private Vector3 _baseScale;
    private float _breathClock;
    private bool _isHovered;
    private Coroutine _scaleCo;

    // ==========================================
    // Awake — Cache Components
    // ==========================================
    private void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _img = GetComponent<Image>();
    }

    // ==========================================
    // Start — Store Base Scale and Seed Phase Offset
    // ==========================================
    private void Start()
    {
        _baseScale = _rt.localScale;
        _breathClock = breathPhaseOffset / Mathf.Max(breathFrequency, 0.01f);
        if (_img != null) _img.color = idleTint;
    }

    // ==========================================
    // Update — Idle Breath (suppressed during hover or press)
    // ==========================================
    private void Update()
    {
        if (_isHovered || _scaleCo != null) return;

        _breathClock += Time.deltaTime;
        float s = 1f + Mathf.Sin(_breathClock * breathFrequency * Mathf.PI * 2f) * breathAmplitude;
        _rt.localScale = _baseScale * s;
    }

    // ==========================================
    // OnPointerEnter — Scale Pop + Brighten Tint
    // ==========================================
    public void OnPointerEnter(PointerEventData e)
    {
        _isHovered = true;
        SetTint(hoverTint);
        TweenTo(hoverScaleMultiplier, hoverInDuration);
    }

    // ==========================================
    // OnPointerExit — Return to Idle Scale + Tint
    // ==========================================
    public void OnPointerExit(PointerEventData e)
    {
        _isHovered = false;
        SetTint(idleTint);
        TweenTo(1f, hoverOutDuration);
    }

    // ==========================================
    // OnPointerDown — Squish Press
    // ==========================================
    public void OnPointerDown(PointerEventData e)
    {
        TweenTo(pressScaleMultiplier, pressDuration);
    }

    // ==========================================
    // OnPointerUp — Overshoot Bounce Release
    // ==========================================
    public void OnPointerUp(PointerEventData e)
    {
        KillScale();
        _scaleCo = StartCoroutine(BounceRelease());
    }

    // ==========================================
    // TweenTo — Interrupt and Lerp to Multiplied Base Scale
    // ==========================================
    private void TweenTo(float multiplier, float duration)
    {
        KillScale();
        _scaleCo = StartCoroutine(LerpScale(_rt.localScale, _baseScale * multiplier, duration));
    }

    // ==========================================
    // KillScale — Stop Active Scale Coroutine
    // ==========================================
    private void KillScale()
    {
        if (_scaleCo != null) { StopCoroutine(_scaleCo); _scaleCo = null; }
    }

    // ==========================================
    // LerpScale — Smooth Coroutine Scale Tween
    // ==========================================
    private IEnumerator LerpScale(Vector3 from, Vector3 to, float dur)
    {
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            _rt.localScale = Vector3.Lerp(from, to, t / dur);
            yield return null;
        }
        _rt.localScale = to;
        _scaleCo = null;
    }

    // ==========================================
    // BounceRelease — Overshoot Peak then Settle to Current Pointer State
    // ==========================================
    private IEnumerator BounceRelease()
    {
        Vector3 from = _rt.localScale;
        Vector3 peak = _baseScale * releaseBounceMultiplier;
        float t = 0f;

        while (t < releaseBounceDuration)
        {
            t += Time.deltaTime;
            _rt.localScale = Vector3.Lerp(from, peak, t / releaseBounceDuration);
            yield return null;
        }

        Vector3 settle = _baseScale * (_isHovered ? hoverScaleMultiplier : 1f);
        from = _rt.localScale;
        t = 0f;

        while (t < releaseSettleDuration)
        {
            t += Time.deltaTime;
            _rt.localScale = Vector3.Lerp(from, settle, t / releaseSettleDuration);
            yield return null;
        }

        _rt.localScale = settle;
        _scaleCo = null;
    }

    // ==========================================
    // SetTint — Apply Color to Image Component
    // ==========================================
    private void SetTint(Color c)
    {
        if (_img != null) _img.color = c;
    }
}