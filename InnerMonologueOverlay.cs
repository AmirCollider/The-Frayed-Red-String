// ==========================================
// InnerMonologueOverlay - Yua Inner Thought Styled Display Panel
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InnerMonologueOverlay : MonoBehaviour
{
    // ==========================================
    // Inspector — Panel
    // ==========================================
    [Header("Panel")]
    [SerializeField] private CanvasGroup overlayGroup;
    [SerializeField] private Image overlayBackground;

    // ==========================================
    // Inspector — Text
    // ==========================================
    [Header("Text")]
    [SerializeField] private TypewriterEffect typewriter;
    [SerializeField] private TextMeshProUGUI labelText;
    [SerializeField] private TextMeshProUGUI speakerNameLabel;

    // ==========================================
    // Inspector — Fade
    // ==========================================
    [Header("Fade")]
    [SerializeField] private float fadeInDuration = 0.25f;
    [SerializeField] private float fadeOutDuration = 0.20f;

    // ==========================================
    // Inspector — Auto Italic Wrap
    // ==========================================
    [Header("Style")]
    [SerializeField] private bool autoItalic = true;

    // ==========================================
    // Private State
    // ==========================================
    private bool _isVisible;
    private Coroutine _fadeCo;

    // ==========================================
    // Awake - Force Hidden on Scene Load
    // ==========================================
    private void Awake()
    {
        ForceHidden();
    }

    // ==========================================
    // Show - Activate Panel and Begin Typewriter Reveal
    // ==========================================
    public void Show(string speakerName, string text)
    {
        gameObject.SetActive(true);
        _isVisible = true;

        if (overlayGroup != null)
        {
            overlayGroup.interactable = false;
            overlayGroup.blocksRaycasts = false;
        }

        SetSpeakerName(speakerName);

        string display = autoItalic ? $"<i>{text}</i>" : text;

        if (typewriter != null)
            typewriter.Play(display);
        else if (labelText != null)
            labelText.text = display;

        FadeTo(1f, fadeInDuration);
    }

    // ==========================================
    // SetSpeakerName - Name the Thinker on the Thought Panel (hidden when blank/system)
    // ==========================================
    private void SetSpeakerName(string speakerName)
    {
        if (speakerNameLabel == null) return;
        bool show = !string.IsNullOrEmpty(speakerName);
        speakerNameLabel.text = show ? speakerName : "";
        speakerNameLabel.gameObject.SetActive(show);
    }

    // ==========================================
    // Hide - Fade Out Then Deactivate
    // ==========================================
    public void Hide()
    {
        if (!_isVisible) return;
        _isVisible = false;
        if (_fadeCo != null) StopCoroutine(_fadeCo);
        _fadeCo = StartCoroutine(HideRoutine());
    }

    // ==========================================
    // IsTyping - Delegate to TypewriterEffect
    // ==========================================
    public bool IsTyping => typewriter != null && typewriter.IsTyping;

    // ==========================================
    // Skip - Reveal Remaining Text Instantly
    // ==========================================
    public void Skip()
    {
        if (typewriter != null) typewriter.Skip();
    }

    // ==========================================
    // HideRoutine - Fade Out Then Deactivate
    // ==========================================
    private IEnumerator HideRoutine()
    {
        float start = overlayGroup != null ? overlayGroup.alpha : 1f;
        float t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            if (overlayGroup != null) overlayGroup.alpha = Mathf.Lerp(start, 0f, t / fadeOutDuration);
            yield return null;
        }
        ForceHidden();
        _fadeCo = null;
    }

    // ==========================================
    // FadeTo - Interrupt Active Fade and Start New One
    // ==========================================
    private void FadeTo(float target, float dur)
    {
        if (_fadeCo != null) StopCoroutine(_fadeCo);
        float from = overlayGroup != null ? overlayGroup.alpha : 0f;
        _fadeCo = StartCoroutine(FadeCoroutine(from, target, dur));
    }

    // ==========================================
    // FadeCoroutine - Lerp CanvasGroup Alpha Over Duration
    // ==========================================
    private IEnumerator FadeCoroutine(float from, float to, float dur)
    {
        if (overlayGroup == null) yield break;
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            overlayGroup.alpha = Mathf.Lerp(from, to, t / dur);
            yield return null;
        }
        overlayGroup.alpha = to;
        _fadeCo = null;
    }

    // ==========================================
    // ForceHidden - Instant Zero-Alpha and Deactivate
    // ==========================================
    private void ForceHidden()
    {
        if (overlayGroup != null)
        {
            overlayGroup.alpha = 0f;
            overlayGroup.interactable = false;
            overlayGroup.blocksRaycasts = false;
        }
        gameObject.SetActive(false);
    }
}