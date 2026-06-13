// ==========================================
// Plus18WarningController - +18 Badge with Neon Glow and Organic Animation
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Plus18WarningController : MonoBehaviour, IPointerClickHandler
{
    // ==========================================
    // Inspector Fields
    // ==========================================
    [Header("Idle Float")]
    [SerializeField] private float floatAmplitude = 6f;
    [SerializeField] private float floatSpeed = 0.8f;

    [Header("Rotation")]
    [SerializeField] private float maxRotationDegrees = 7f;
    [SerializeField] private float rotationNoiseSpeed = 0.55f;

    [Header("Glow Layers")]
    [SerializeField] private Color glowTint = new Color(1f, 0.18f, 0.08f, 1f);
    [SerializeField] private int glowLayerCount = 3;
    [SerializeField] private float glowLayerSpreadStep = 2.5f;
    [SerializeField] private float glowBaseAlpha = 0.52f;
    [SerializeField] private float glowPeakAlpha = 0.90f;
    [SerializeField] private float glowPulseSpeed = 1.2f;

    [Header("Flicker")]
    [SerializeField] private float minFlickerInterval = 2f;
    [SerializeField] private float maxFlickerInterval = 6.5f;
    [SerializeField] private float flickerDuration = 0.08f;
    [SerializeField] private float flickerBoost = 1.3f;

    [Header("Click Burst")]
    [SerializeField] private float burstDuration = 0.4f;
    [SerializeField] private float burstPeakAlphaMultiplier = 2f;
    [SerializeField] private float burstExpandScale = 2.2f;

    // ==========================================
    // Private State
    // ==========================================
    private Image _mainImage;
    private RectTransform _rectTransform;
    private Image[] _glowImages;
    private RectTransform[] _glowRects;
    private Vector2 _originAnchorPos;
    private float _noiseX;
    private float _noiseY;
    private float _noiseRot;
    private bool _burstActive;

    // ==========================================
    // Editor-Only Live Rebuild Toggle
    // ==========================================
#if UNITY_EDITOR
    [Header("Editor")]
    [SerializeField] private bool rebuildGlow = false;

    private void OnValidate()
    {
        if (!Application.isPlaying) return;

        if (rebuildGlow)
        {
            rebuildGlow = false;
            if (_glowImages != null)
                foreach (Image img in _glowImages)
                    if (img != null) Destroy(img.gameObject);
            BuildGlowLayers();
            return;
        }

        // ==========================================
        // Live-sync glowTint RGB to existing layers without full rebuild
        // ==========================================
        if (_glowImages == null) return;
        for (int i = 0; i < _glowImages.Length; i++)
        {
            if (_glowImages[i] == null) continue;
            Color c = _glowImages[i].color;
            c.r = glowTint.r;
            c.g = glowTint.g;
            c.b = glowTint.b;
            _glowImages[i].color = c;
        }
    }
#endif

    // ==========================================
    // Awake - Cache Components
    // ==========================================
    private void Awake()
    {
        _mainImage = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
    }

    // ==========================================
    // Start - Initialize State and Launch All Routines
    // ==========================================
    private void Start()
    {
        _originAnchorPos = _rectTransform.anchoredPosition;
        _noiseX = Random.Range(0f, 1000f);
        _noiseY = Random.Range(500f, 1500f);
        _noiseRot = Random.Range(1000f, 2000f);

        BuildGlowLayers();
        StartCoroutine(IdleRoutine());
        //StartCoroutine(GlowPulseRoutine());
        StartCoroutine(FlickerRoutine());
    }

    // ==========================================
    // BuildGlowLayers - Procedurally Create Sibling Glow Images Behind Badge
    // ==========================================
    private void BuildGlowLayers()
    {
        _glowImages = new Image[glowLayerCount];
        _glowRects = new RectTransform[glowLayerCount];

        int insertAtIndex = transform.GetSiblingIndex();

        for (int i = 0; i < glowLayerCount; i++)
        {
            // i=0: outermost layer — largest spread, lowest alpha, furthest back
            // i=glowLayerCount-1: innermost layer — smallest spread, highest alpha, just behind badge
            float spreadMult = (float)(glowLayerCount - i);
            float normalizedDepth = (float)i / Mathf.Max(glowLayerCount - 1, 1);
            float alpha = Mathf.Lerp(glowBaseAlpha * 0.2f, glowBaseAlpha, normalizedDepth);

            GameObject go = new GameObject($"Plus18Glow_{i}");
            go.transform.SetParent(transform.parent, false);
            go.transform.SetSiblingIndex(insertAtIndex + i);

            Image img = go.AddComponent<Image>();
            img.sprite = _mainImage.sprite;
            img.raycastTarget = false;

            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchorMin = _rectTransform.anchorMin;
            rt.anchorMax = _rectTransform.anchorMax;
            rt.pivot = _rectTransform.pivot;
            rt.anchoredPosition = _rectTransform.anchoredPosition;
            rt.sizeDelta = _rectTransform.sizeDelta + Vector2.one * (glowLayerSpreadStep * spreadMult);

            img.color = new Color(glowTint.r, glowTint.g, glowTint.b, alpha);

            _glowImages[i] = img;
            _glowRects[i] = rt;
        }

        // Badge must render on top of every glow layer
        transform.SetSiblingIndex(insertAtIndex + glowLayerCount);
    }

    // ==========================================
    // IdleRoutine - Perlin Noise Float and Rotation with Live Glow Sync
    // ==========================================
    private IEnumerator IdleRoutine()
    {
        while (true)
        {
            float t = Time.time * floatSpeed;
            float nx = (Mathf.PerlinNoise(_noiseX + t, 0f) * 2f - 1f) * floatAmplitude;
            float ny = (Mathf.PerlinNoise(0f, _noiseY + t) * 2f - 1f) * floatAmplitude;
            float nr = (Mathf.PerlinNoise(_noiseRot + t * rotationNoiseSpeed, 0f) * 2f - 1f) * maxRotationDegrees;

            Vector2 newPos = _originAnchorPos + new Vector2(nx, ny);
            Quaternion newRot = Quaternion.Euler(0f, 0f, nr);

            _rectTransform.anchoredPosition = newPos;
            _rectTransform.localRotation = newRot;

            for (int i = 0; i < _glowRects.Length; i++)
            {
                if (_glowRects[i] == null) continue;
                _glowRects[i].anchoredPosition = newPos;
                _glowRects[i].localRotation = newRot;
            }

            yield return null;
        }
    }

    // ==========================================
    // GlowPulseRoutine - Animate Glow Alpha and Sync Tint RGB Each Frame
    // ==========================================
    private IEnumerator GlowPulseRoutine()
    {
        float phase = Random.Range(0f, Mathf.PI * 2f);

        while (true)
        {
            if (!_burstActive)
            {
                phase += Time.deltaTime * glowPulseSpeed;
                float pulse = (Mathf.Sin(phase) + 1f) * 0.5f;

                for (int i = 0; i < _glowImages.Length; i++)
                {
                    if (_glowImages[i] == null) continue;
                    float nd = (float)i / Mathf.Max(_glowImages.Length - 1, 1);
                    float baseA = Mathf.Lerp(glowBaseAlpha * 0.2f, glowBaseAlpha, nd);
                    float peakA = Mathf.Lerp(glowPeakAlpha * 0.2f, glowPeakAlpha, nd);
                    _glowImages[i].color = new Color(
                        glowTint.r, glowTint.g, glowTint.b,
                        Mathf.Lerp(baseA, peakA, pulse));
                }
            }

            yield return null;
        }
    }

    // ==========================================
    // FlickerRoutine - Random Neon Sign Brightness Spike
    // ==========================================
    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minFlickerInterval, maxFlickerInterval));

            if (_burstActive) continue;

            Color original = _mainImage.color;
            Color boosted = new Color(
                Mathf.Min(original.r * flickerBoost, 1f),
                Mathf.Min(original.g * flickerBoost, 1f),
                Mathf.Min(original.b * flickerBoost, 1f),
                original.a);

            _mainImage.color = boosted;
            yield return new WaitForSeconds(flickerDuration * 0.25f);
            _mainImage.color = original;
            yield return new WaitForSeconds(flickerDuration * 0.15f);
            _mainImage.color = boosted;
            yield return new WaitForSeconds(flickerDuration * 0.60f);
            _mainImage.color = original;
        }
    }

    // ==========================================
    // OnPointerClick - Trigger Radial Burst on Click
    // ==========================================
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_burstActive)
            StartCoroutine(BurstRoutine());
    }

    // ==========================================
    // BurstRoutine - Expand and Fade Glow Layers Outward on Click
    // ==========================================
    private IEnumerator BurstRoutine()
    {
        _burstActive = true;
        float elapsed = 0f;

        // Cache start state for clean restore
        float[] startAlphas = new float[_glowImages.Length];
        Vector2[] startSizeDeltas = new Vector2[_glowRects.Length];
        for (int i = 0; i < _glowImages.Length; i++)
        {
            if (_glowImages[i] != null) startAlphas[i] = _glowImages[i].color.a;
            if (_glowRects[i] != null) startSizeDeltas[i] = _glowRects[i].sizeDelta;
        }

        try
        {
            while (elapsed < burstDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / burstDuration;
                float eased = 1f - Mathf.Pow(1f - t, 2.5f);

                for (int i = 0; i < _glowImages.Length; i++)
                {
                    if (_glowImages[i] == null || _glowRects[i] == null) continue;

                    float spreadMult = (float)(_glowImages.Length - i);
                    float expandedSpread = glowLayerSpreadStep * spreadMult * Mathf.Lerp(1f, burstExpandScale, eased);
                    _glowRects[i].sizeDelta = _rectTransform.sizeDelta + Vector2.one * expandedSpread;

                    float peakAlpha = startAlphas[i] * burstPeakAlphaMultiplier;
                    float burstAlpha = t < 0.15f
                        ? Mathf.Lerp(startAlphas[i], peakAlpha, t / 0.15f)
                        : Mathf.Lerp(peakAlpha, 0f, (t - 0.15f) / 0.85f);

                    Color c = _glowImages[i].color;
                    c.a = Mathf.Clamp01(burstAlpha);
                    _glowImages[i].color = c;
                }

                yield return null;
            }
        }
        finally
        {
            // ==========================================
            // Restore pre-burst sizes and release lock — runs even on StopCoroutine
            // ==========================================
            for (int i = 0; i < _glowRects.Length; i++)
                if (_glowRects[i] != null)
                    _glowRects[i].sizeDelta = startSizeDeltas[i];

            _burstActive = false;
        }
    }

    // ==========================================
    // OnDestroy - Clean Up Procedural Glow GameObjects
    // ==========================================
    private void OnDestroy()
    {
        if (_glowImages == null) return;
        foreach (Image img in _glowImages)
        {
            if (img != null)
                Destroy(img.gameObject);
        }
    }
}