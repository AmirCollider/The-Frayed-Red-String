// ==========================================
// GlitchCutTransition - RGB-Split Flash Coroutine for Act 3 Transitions
// AmirCollider Games - The Frayed Red String
// ==========================================

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GlitchCutTransition : MonoBehaviour
{
    // ==========================================
    // Inspector — RGB Layer Images (full-screen, Anchor = Stretch)
    // ==========================================
    [Header("RGB Split Layers")]
    [SerializeField] private Image layerRed;
    [SerializeField] private Image layerGreen;
    [SerializeField] private Image layerBlue;

    // ==========================================
    // Inspector — Flash Settings
    // ==========================================
    [Header("Flash Settings")]
    [SerializeField] private float totalDuration = 0.35f;
    [SerializeField] private float maxOffsetX = 18f;
    [SerializeField] private float maxOffsetY = 8f;
    [SerializeField] private int flashCount = 4;
    [SerializeField] private float peakAlpha = 0.75f;

    // ==========================================
    // Inspector — Layer Tints
    // ==========================================
    [Header("Layer Tints")]
    [SerializeField] private Color redTint = new Color(1f, 0.1f, 0.1f, 1f);
    [SerializeField] private Color greenTint = new Color(0.1f, 1f, 0.1f, 1f);
    [SerializeField] private Color blueTint = new Color(0.1f, 0.1f, 1f, 1f);

    // ==========================================
    // Private State
    // ==========================================
    private Coroutine _glitchCo;
    private RectTransform _rtRed;
    private RectTransform _rtGreen;
    private RectTransform _rtBlue;

    // ==========================================
    // Awake - Cache RectTransforms, Disable Raycasts, Force Hidden
    // ==========================================
    private void Awake()
    {
        if (layerRed != null) { _rtRed = layerRed.GetComponent<RectTransform>(); layerRed.raycastTarget = false; }
        if (layerGreen != null) { _rtGreen = layerGreen.GetComponent<RectTransform>(); layerGreen.raycastTarget = false; }
        if (layerBlue != null) { _rtBlue = layerBlue.GetComponent<RectTransform>(); layerBlue.raycastTarget = false; }
        SetLayersHidden();
    }

    // ==========================================
    // TriggerGlitchCut - Begin RGB Flash Sequence Then Invoke Callback
    // ==========================================
    public void TriggerGlitchCut(Action onComplete = null)
    {
        if (_glitchCo != null) StopCoroutine(_glitchCo);
        _glitchCo = StartCoroutine(GlitchRoutine(onComplete));
    }

    // ==========================================
    // GlitchRoutine - Staggered RGB Offset Flashes Over totalDuration
    // ==========================================
    private IEnumerator GlitchRoutine(Action onComplete)
    {
        float flashInterval = totalDuration / Mathf.Max(flashCount, 1);

        for (int i = 0; i < flashCount; i++)
        {
            float alpha = Mathf.Lerp(peakAlpha, peakAlpha * 0.15f, (float)i / flashCount);

            SetLayerState(layerRed, _rtRed,
                new Vector2(UnityEngine.Random.Range(-maxOffsetX, maxOffsetX), UnityEngine.Random.Range(-maxOffsetY, maxOffsetY)),
                alpha);
            SetLayerState(layerGreen, _rtGreen,
                new Vector2(UnityEngine.Random.Range(-maxOffsetX, maxOffsetX), UnityEngine.Random.Range(-maxOffsetY, maxOffsetY)),
                alpha * 0.7f);
            SetLayerState(layerBlue, _rtBlue,
                new Vector2(UnityEngine.Random.Range(-maxOffsetX, maxOffsetX), UnityEngine.Random.Range(-maxOffsetY, maxOffsetY)),
                alpha * 0.5f);

            yield return new WaitForSeconds(flashInterval * 0.5f);
            SetLayersHidden();
            yield return new WaitForSeconds(flashInterval * 0.5f);
        }

        SetLayersHidden();
        _glitchCo = null;
        onComplete?.Invoke();
    }

    // ==========================================
    // SetLayerState - Apply Position Offset and Alpha to One Layer
    // ==========================================
    private void SetLayerState(Image img, RectTransform rt, Vector2 offset, float alpha)
    {
        if (img == null || rt == null) return;
        Color c = img.color;
        c.a = Mathf.Clamp01(alpha);
        img.color = c;
        rt.anchoredPosition = offset;
    }

    // ==========================================
    // SetLayersHidden - Zero Alpha and Reset Offsets on All Layers
    // ==========================================
    private void SetLayersHidden()
    {
        if (layerRed != null) { Color c = redTint; c.a = 0f; layerRed.color = c; }
        if (layerGreen != null) { Color c = greenTint; c.a = 0f; layerGreen.color = c; }
        if (layerBlue != null) { Color c = blueTint; c.a = 0f; layerBlue.color = c; }
        if (_rtRed != null) _rtRed.anchoredPosition = Vector2.zero;
        if (_rtGreen != null) _rtGreen.anchoredPosition = Vector2.zero;
        if (_rtBlue != null) _rtBlue.anchoredPosition = Vector2.zero;
    }
}