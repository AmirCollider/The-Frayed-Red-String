// ==========================================
// ColorGrader - Screen Color Grade via Full-Screen UI Image Tint Overlay
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ColorGrader : MonoBehaviour
{
    // ==========================================
    // Singleton Instance (per-scene — no DontDestroyOnLoad)
    // ==========================================
    public static ColorGrader Instance { get; private set; }

    // ==========================================
    // Inspector — Full-Screen Overlay (raycastTarget = false, Anchor = Stretch)
    // ==========================================
    [Header("Overlay")]
    [SerializeField] private Image overlayImage;

    // ==========================================
    // Inspector — Palette Tint Presets
    // ==========================================
    [Header("Palette Presets")]
    [SerializeField] private Color tintNormal = new Color(0f, 0f, 0f, 0f);
    [SerializeField] private Color tintCold = new Color(0.05f, 0.1f, 0.25f, 0.35f);
    [SerializeField] private Color tintSepia = new Color(0.4f, 0.25f, 0f, 0.35f);
    [SerializeField] private Color tintBloodred = new Color(0.4f, 0f, 0f, 0.45f);
    [SerializeField] private Color tintDesaturated = new Color(0.05f, 0.05f, 0.08f, 0.42f);

    // ==========================================
    // Inspector — Transition
    // ==========================================
    [Header("Transition")]
    [SerializeField] private float transitionDuration = 1.2f;

    // ==========================================
    // Private State
    // ==========================================
    private Coroutine _gradeCo;
    private ColorPalette _currentPalette = ColorPalette.Normal;

    // ==========================================
    // Awake - Singleton Enforcement and Initialize Overlay to Normal
    // ==========================================
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (overlayImage != null)
        {
            overlayImage.raycastTarget = false;
            overlayImage.color = tintNormal;
        }
    }

    // ==========================================
    // SetPalette - Transition to Target Color Grade Over Duration
    // ==========================================
    public void SetPalette(ColorPalette palette, float overrideDuration = -1f)
    {
        if (_currentPalette == palette) return;
        _currentPalette = palette;
        float dur = overrideDuration > 0f ? overrideDuration : transitionDuration;
        KillGrade();
        _gradeCo = StartCoroutine(GradeRoutine(GetTint(palette), dur));
    }

    // ==========================================
    // SetPaletteInstant - Apply Color Grade with No Transition
    // ==========================================
    public void SetPaletteInstant(ColorPalette palette)
    {
        KillGrade();
        _currentPalette = palette;
        if (overlayImage != null)
            overlayImage.color = GetTint(palette);
    }

    // ==========================================
    // GradeRoutine - SmoothStep Lerp Overlay Tint to Target Color
    // ==========================================
    private IEnumerator GradeRoutine(Color targetTint, float duration)
    {
        if (overlayImage == null) yield break;

        Color startTint = overlayImage.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            overlayImage.color = Color.Lerp(startTint, targetTint, Mathf.SmoothStep(0f, 1f, elapsed / duration));
            yield return null;
        }

        overlayImage.color = targetTint;
        _gradeCo = null;
    }

    // ==========================================
    // GetTint - Map ColorPalette Enum to Inspector Color Value
    // ==========================================
    private Color GetTint(ColorPalette palette)
    {
        switch (palette)
        {
            case ColorPalette.Cold: return tintCold;
            case ColorPalette.Sepia: return tintSepia;
            case ColorPalette.Bloodred: return tintBloodred;
            case ColorPalette.Desaturated: return tintDesaturated;
            default: return tintNormal;
        }
    }

    // ==========================================
    // KillGrade - Interrupt Active Transition
    // ==========================================
    private void KillGrade()
    {
        if (_gradeCo != null) { StopCoroutine(_gradeCo); _gradeCo = null; }
    }

    // ==========================================
    // CurrentPalette - Active Palette Accessor
    // ==========================================
    public ColorPalette CurrentPalette => _currentPalette;
}

// ==========================================
// ColorPalette - Available Color Grade Presets
// ==========================================
public enum ColorPalette
{
    Normal = 0,
    Cold = 1,
    Sepia = 2,
    Bloodred = 3,
    Desaturated = 4
}