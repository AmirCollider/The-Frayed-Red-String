// ==========================================
// PlayerAddressStyle - Visual/Audio Profile for Direct-to-Player Dialogue
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;

public class PlayerAddressStyle : MonoBehaviour
{
    // ==========================================
    // Inspector — Camera Stare
    // ==========================================
    [Header("Camera Stare")]
    [SerializeField] private float staredCameraSize = 4f;
    [SerializeField] private float cameraTransitionDuration = 1.5f;

    // ==========================================
    // Inspector — Dialogue Panel Tint
    // ==========================================
    [Header("Dialogue Panel Tint")]
    [SerializeField] private Color addressPanelTint = new Color(0.05f, 0.05f, 0.10f, 0.92f);
    [SerializeField] private float panelTintTransitionDuration = 0.5f;

    // ==========================================
    // Inspector — Typewriter Override
    // ==========================================
    [Header("Typewriter Override")]
    [SerializeField] private float addressTypewriterCPS = 20f;

    // ==========================================
    // Inspector — Rich Text Wrap
    // ==========================================
    [Header("Rich Text Wrap")]
    [SerializeField] private string richTextPrefix = "<b>";
    [SerializeField] private string richTextSuffix = "</b>";

    // ==========================================
    // Inspector — Tears Overlay
    // ==========================================
    [Header("Tears Overlay")]
    [SerializeField] private Color tearsOverlayColor = Color.white;
    [SerializeField] private float tearsTargetAlpha = 0.35f;
    [SerializeField] private float tearsFadeInDuration = 2f;

    // ==========================================
    // Inspector — Music Cut
    // ==========================================
    [Header("Music Cut")]
    [SerializeField] private float musicFadeOutDuration = 0.4f;

    // ==========================================
    // Accessors — Camera Stare
    // ==========================================
    public float StaredCameraSize => staredCameraSize;
    public float CameraTransitionDuration => cameraTransitionDuration;

    // ==========================================
    // Accessors — Dialogue Panel Tint
    // ==========================================
    public Color AddressPanelTint => addressPanelTint;
    public float PanelTintTransitionDuration => panelTintTransitionDuration;

    // ==========================================
    // Accessors — Typewriter Override
    // ==========================================
    public float AddressTypewriterCPS => addressTypewriterCPS;

    // ==========================================
    // Accessors — Tears Overlay
    // ==========================================
    public Color TearsOverlayColor => tearsOverlayColor;
    public float TearsTargetAlpha => tearsTargetAlpha;
    public float TearsFadeInDuration => tearsFadeInDuration;

    // ==========================================
    // Accessors — Music Cut
    // ==========================================
    public float MusicFadeOutDuration => musicFadeOutDuration;

    // ==========================================
    // WrapAddressText - Apply Rich-Text Emphasis Tags for Direct-Address Lines
    // ==========================================
    public string WrapAddressText(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;
        return $"{richTextPrefix}{text}{richTextSuffix}";
    }
}