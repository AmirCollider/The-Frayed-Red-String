// ==========================================
// MenuAnimator - Idle Background Drift Animation
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;

public class MenuAnimator : MonoBehaviour
{
    // ==========================================
    // Inspector References
    // ==========================================
    [Header("Target")]
    [SerializeField] private RectTransform backgroundRect;

    [Header("Synchronized UI Elements")]
    [SerializeField] private RectTransform[] syncedRects;

    [Header("Drift Settings")]
    [SerializeField] private float driftAmplitudeX = 0.6f;
    [SerializeField] private float driftAmplitudeY = 0.4f;
    [SerializeField] private float driftFrequencyX = 0.18f;
    [SerializeField] private float driftFrequencyY = 0.12f;

    // ==========================================
    // Private State
    // ==========================================
    private Vector2 _originPos;
    private Vector2[] _syncedOriginPos;
    private float _timeOffset;

    // ==========================================
    // Start - Cache Origins, Randomize Phase & Cover Drift Margins
    // ==========================================
    private void Start()
    {
        if (backgroundRect == null) return;
        _originPos = backgroundRect.anchoredPosition;
        _timeOffset = Random.Range(0f, 100f);
        OverscaleForDrift();

        // ==========================================
        // Cache anchor origins for all synchronized UI elements
        // ==========================================
        if (syncedRects != null)
        {
            _syncedOriginPos = new Vector2[syncedRects.Length];
            for (int i = 0; i < syncedRects.Length; i++)
                if (syncedRects[i] != null)
                    _syncedOriginPos[i] = syncedRects[i].anchoredPosition;
        }
    }

    // ==========================================
    // OverscaleForDrift - Upscale Background to Prevent Edge Exposure
    // ==========================================
    private void OverscaleForDrift()
    {
        float w = backgroundRect.rect.width;
        float h = backgroundRect.rect.height;

        if (w < 1f) w = ConstantsConfig.BASE_WIDTH;
        if (h < 1f) h = ConstantsConfig.BASE_HEIGHT;

        float sx = (w + driftAmplitudeX * 2f + 2f) / w;
        float sy = (h + driftAmplitudeY * 2f + 2f) / h;
        backgroundRect.localScale = Vector3.one * Mathf.Max(sx, sy);
    }

    // ==========================================
    // Update - Apply Sine-Wave Drift to Background and All Synced Elements
    // ==========================================
    private void Update()
    {
        if (backgroundRect == null) return;

        float t = Time.time + _timeOffset;
        float dx = Mathf.Sin(t * driftFrequencyX * Mathf.PI * 2f) * driftAmplitudeX;
        float dy = Mathf.Cos(t * driftFrequencyY * Mathf.PI * 2f) * driftAmplitudeY;
        Vector2 offset = new Vector2(dx, dy);

        backgroundRect.anchoredPosition = _originPos + offset;

        // ==========================================
        // Propagate identical offset to all synchronized UI elements
        // ==========================================
        if (syncedRects == null || _syncedOriginPos == null) return;
        for (int i = 0; i < syncedRects.Length; i++)
            if (syncedRects[i] != null)
                syncedRects[i].anchoredPosition = _syncedOriginPos[i] + offset;
    }
}