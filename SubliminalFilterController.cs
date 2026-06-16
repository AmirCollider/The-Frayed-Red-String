// ==========================================
// SubliminalFilterController - Imperceptible Blur/Grain Layer + Act 3 Hard-Strip (GDD §1.1)
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SubliminalFilterController : MonoBehaviour
{
    // ==========================================
    // Singleton Instance (per-scene — Act 3 strips the active scene's filter)
    // ==========================================
    public static SubliminalFilterController Instance { get; private set; }

    // ==========================================
    // Inspector — URP Global Volume (Film Grain override lives in its profile)
    // ==========================================
    [Header("URP Volume")]
    [SerializeField] private Volume subliminalVolume;

    // ==========================================
    // Inspector — Optional Full-Screen Blur Overlay (Image with a blur material)
    // ==========================================
    [Header("Optional Blur Overlay")]
    [SerializeField] private CanvasGroup blurOverlayGroup;
    [SerializeField] private Material blurMaterial;
    [SerializeField] private string blurRadiusProperty = "_Radius";

    // ==========================================
    // Inspector — Subliminal Parameters (Acts 1–2)
    // ==========================================
    [Header("Subliminal Parameters")]
    [SerializeField] private float blurRadius = 0.45f;
    [SerializeField][Range(0f, 1f)] private float grainIntensity = 0.03f;
    [SerializeField][Range(0f, 1f)] private float alphaBlend = 0.05f;
    [SerializeField] private bool enableOnAwake = true;

    // ==========================================
    // Private State
    // ==========================================
    private FilmGrain _grain;
    private bool _stripped;

    // ==========================================
    // Awake - Register Instance, Resolve Overrides, Apply Subliminal Layer
    // ==========================================
    private void Awake()
    {
        Instance = this;
        ResolveOverrides();
        if (enableOnAwake) EnableSubliminal();
    }

    // ==========================================
    // ResolveOverrides - Cache Film Grain Override and Configure Blur Material
    // ==========================================
    private void ResolveOverrides()
    {
        if (subliminalVolume != null && subliminalVolume.profile != null)
            subliminalVolume.profile.TryGet(out _grain);

        if (blurMaterial != null && blurMaterial.HasProperty(blurRadiusProperty))
            blurMaterial.SetFloat(blurRadiusProperty, blurRadius);
    }

    // ==========================================
    // EnableSubliminal - Apply the Imperceptible Acts 1–2 Profile
    // ==========================================
    public void EnableSubliminal()
    {
        _stripped = false;

        if (subliminalVolume != null) subliminalVolume.weight = 1f;

        if (_grain != null)
        {
            _grain.active = true;
            _grain.intensity.overrideState = true;
            _grain.intensity.value = grainIntensity;
        }

        if (blurOverlayGroup != null) blurOverlayGroup.alpha = alphaBlend;
    }

    // ==========================================
    // StripInstant - Act 3 Shock-Strip: Full Deactivation in a Single Frame (DeactivationTime 0.0s)
    // ==========================================
    public void StripInstant()
    {
        if (_stripped) return;
        _stripped = true;

        if (subliminalVolume != null) subliminalVolume.weight = 0f;
        if (_grain != null) _grain.intensity.value = 0f;
        if (blurOverlayGroup != null) blurOverlayGroup.alpha = 0f;
    }

    // ==========================================
    // IsStripped - Whether the Filter Has Been Permanently Stripped This Scene
    // ==========================================
    public bool IsStripped => _stripped;
}