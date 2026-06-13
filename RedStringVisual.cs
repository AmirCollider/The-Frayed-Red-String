// ==========================================
// RedStringVisual - Decorative Red String Motif via LineRenderer
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RedStringVisual : MonoBehaviour
{
    // ==========================================
    // Inspector — Anchor Points
    // ==========================================
    [Header("Anchors")]
    [SerializeField] private Transform anchorLeft;
    [SerializeField] private Transform anchorRight;

    // ==========================================
    // Inspector — Appearance
    // ==========================================
    [Header("Appearance")]
    [SerializeField] private Color stringColor = new Color(0.72f, 0.07f, 0.07f, 1f);
    [SerializeField] private float lineWidth = 0.04f;
    [SerializeField] private Material stringMaterial;

    // ==========================================
    // Inspector — Catenary Shape
    // ==========================================
    [Header("Catenary")]
    [SerializeField] private int segmentCount = 24;
    [SerializeField] private float sagAmount = 0.35f;

    // ==========================================
    // Inspector — Organic Wobble
    // ==========================================
    [Header("Wobble")]
    [SerializeField] private float wobbleFrequency = 0.6f;
    [SerializeField] private float wobbleAmplitude = 0.04f;

    // ==========================================
    // Inspector — Fade
    // ==========================================
    [Header("Fade")]
    [SerializeField] private float fadeInDuration = 0.6f;
    [SerializeField] private float fadeOutDuration = 0.4f;

    // ==========================================
    // Private State
    // ==========================================
    private LineRenderer _lr;
    private bool _isVisible;
    private Coroutine _fadeCo;
    private float _currentAlpha;
    private float _timeOffset;

    // ==========================================
    // Awake - Cache LineRenderer and Apply Initial Configuration
    // ==========================================
    private void Awake()
    {
        _lr = GetComponent<LineRenderer>();
        _timeOffset = Random.Range(0f, 100f);
        ConfigureLineRenderer();
        _currentAlpha = 0f;
        ApplyAlpha(0f);
    }

    // ==========================================
    // ConfigureLineRenderer - Set Count, Width, Material, Base Gradient
    // ==========================================
    private void ConfigureLineRenderer()
    {
        _lr.positionCount = segmentCount;
        _lr.startWidth = lineWidth;
        _lr.endWidth = lineWidth;
        _lr.useWorldSpace = true;
        _lr.numCapVertices = 4;
        _lr.numCornerVertices = 4;

        if (stringMaterial != null)
            _lr.material = stringMaterial;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(stringColor, 0f), new GradientColorKey(stringColor, 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0f, 0f), new GradientAlphaKey(0f, 1f) }
        );
        _lr.colorGradient = gradient;
    }

    // ==========================================
    // Update - Recompute Catenary Path with Wobble Each Frame
    // ==========================================
    private void Update()
    {
        if (!_isVisible || anchorLeft == null || anchorRight == null) return;
        RebuildPath();
    }

    // ==========================================
    // RebuildPath - Parabolic Catenary Approximation with Organic Wobble
    // ==========================================
    private void RebuildPath()
    {
        Vector3 left = anchorLeft.position;
        Vector3 right = anchorRight.position;
        float t = Time.time + _timeOffset;

        for (int i = 0; i < segmentCount; i++)
        {
            float frac = (float)i / (segmentCount - 1);
            float sag = -sagAmount * 4f * frac * (1f - frac);
            float wobble = Mathf.Sin(t * wobbleFrequency * Mathf.PI * 2f + frac * Mathf.PI) * wobbleAmplitude;

            Vector3 point = Vector3.Lerp(left, right, frac);
            point.y += sag + wobble;
            _lr.SetPosition(i, point);
        }
    }

    // ==========================================
    // Show - Fade In Red String
    // ==========================================
    public void Show()
    {
        if (_isVisible) return;
        _isVisible = true;
        gameObject.SetActive(true);
        KillFade();
        _fadeCo = StartCoroutine(FadeRoutine(_currentAlpha, 1f, fadeInDuration));
    }

    // ==========================================
    // Hide - Fade Out Then Deactivate
    // ==========================================
    public void Hide()
    {
        if (!_isVisible) return;
        _isVisible = false;
        KillFade();
        _fadeCo = StartCoroutine(FadeOutAndDeactivateRoutine());
    }

    // ==========================================
    // ForceShow - Instant Visible Without Fade
    // ==========================================
    public void ForceShow()
    {
        _isVisible = true;
        gameObject.SetActive(true);
        KillFade();
        ApplyAlpha(1f);
    }

    // ==========================================
    // ForceHide - Instant Hidden and Deactivated
    // ==========================================
    public void ForceHide()
    {
        _isVisible = false;
        KillFade();
        ApplyAlpha(0f);
        gameObject.SetActive(false);
    }

    // ==========================================
    // FadeRoutine - SmoothStep Alpha Lerp Over Duration
    // ==========================================
    private IEnumerator FadeRoutine(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            ApplyAlpha(Mathf.Lerp(from, to, Mathf.SmoothStep(0f, 1f, elapsed / duration)));
            yield return null;
        }
        ApplyAlpha(to);
        _fadeCo = null;
    }

    // ==========================================
    // FadeOutAndDeactivateRoutine - Fade to Zero Then Deactivate
    // ==========================================
    private IEnumerator FadeOutAndDeactivateRoutine()
    {
        float from = _currentAlpha;
        float elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            ApplyAlpha(Mathf.Lerp(from, 0f, Mathf.SmoothStep(0f, 1f, elapsed / fadeOutDuration)));
            yield return null;
        }
        ApplyAlpha(0f);
        gameObject.SetActive(false);
        _fadeCo = null;
    }

    // ==========================================
    // ApplyAlpha - Rebuild Gradient Alpha Keys and Reassign to LineRenderer
    // ==========================================
    private void ApplyAlpha(float alpha)
    {
        _currentAlpha = alpha;
        Gradient g = _lr.colorGradient;
        GradientAlphaKey[] ak = new GradientAlphaKey[]
        {
            new GradientAlphaKey(alpha, 0f),
            new GradientAlphaKey(alpha, 1f)
        };
        g.SetKeys(g.colorKeys, ak);
        _lr.colorGradient = g;
    }

    // ==========================================
    // KillFade - Stop Active Fade Coroutine
    // ==========================================
    private void KillFade()
    {
        if (_fadeCo != null)
        {
            StopCoroutine(_fadeCo);
            _fadeCo = null;
        }
    }

    // ==========================================
    // Accessors
    // ==========================================
    public bool IsVisible => _isVisible;
    public Transform AnchorLeft { get => anchorLeft; set => anchorLeft = value; }
    public Transform AnchorRight { get => anchorRight; set => anchorRight = value; }
}