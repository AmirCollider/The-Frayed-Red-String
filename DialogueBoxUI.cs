// ==========================================
// DialogueBoxUI - Dialogue Text Panel, Speaker Label, Continue Arrow
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxUI : MonoBehaviour
{
    // ==========================================
    // Inspector — Panel Root
    // ==========================================
    [Header("Panel")]
    [SerializeField] private CanvasGroup panelGroup;
    [SerializeField] private Image panelBackground;

    // ==========================================
    // Inspector — Speaker Label
    // ==========================================
    [Header("Speaker")]
    [SerializeField] private TextMeshProUGUI speakerLabel;
    [SerializeField] private Image speakerNameBacking;

    // ==========================================
    // Inspector — Typewriter
    // ==========================================
    [Header("Text")]
    [SerializeField] private TypewriterEffect typewriter;
    [SerializeField] private TextBeepSynthesizer beepSynth;

    // ==========================================
    // Inspector — Continue Arrow
    // ==========================================
    [Header("Continue Arrow")]
    [SerializeField] private GameObject continueArrow;
    [SerializeField] private float arrowBlinkInterval = 0.55f;

    // ==========================================
    // Inspector — Fade Durations
    // ==========================================
    [Header("Fade")]
    [SerializeField] private float fadeInDuration = 0.18f;
    [SerializeField] private float fadeOutDuration = 0.14f;

    // ==========================================
    // Inspector — Fourth-Wall Tint Override
    // ==========================================
    [Header("Fourth-Wall Tint")]
    [SerializeField] private Color fourthWallTint = new Color(0.05f, 0.05f, 0.10f, 0.92f);

    // ==========================================
    // Private State
    // ==========================================

    private bool _isVisible;
    private Coroutine _fadeCo;
    private Coroutine _blinkCo;
    private Color _defaultBgColor;

    // ==========================================
    // Private State — Address Style Override (FourthWallBreaker)
    // ==========================================
    private PlayerAddressStyle _addressStyle;
    private Coroutine _tintCo;

    // ==========================================
    // Awake - Cache Default Panel Color and Force Hidden
    // ==========================================
    private void Awake()
    {
        if (panelBackground != null)
            _defaultBgColor = panelBackground.color;

        ForceHidden();
    }

    // ==========================================
    // Show - Activate and Fade Panel In
    // ==========================================
    public void Show()
    {
        if (_isVisible) return;
        _isVisible = true;
        gameObject.SetActive(true);
        SetArrow(false);
        SetCG(panelGroup != null ? panelGroup.alpha : 0f, true);
        FadeTo(1f, fadeInDuration);
    }

    // ==========================================
    // Hide - Fade Out Then Deactivate
    // ==========================================
    public void Hide()
    {
        if (!_isVisible) return;
        _isVisible = false;
        SetArrow(false);
        if (_fadeCo != null) StopCoroutine(_fadeCo);
        _fadeCo = StartCoroutine(HideRoutine());
    }
    // ==========================================
    // DisplayLine - Apply Speaker, Fire Typewriter, Set Style
    // ==========================================
    public void DisplayLine(DialogueLine line)

    {
        if (line == null) return;
        SetSpeakerLabel(line.speakerId);
        ApplyStyle(line.isFourthWall);
        SetArrow(false);

        if (beepSynth != null)
            beepSynth.ConfigureForLine(line.speakerId);

        if (typewriter == null) return;

        if (_addressStyle != null)
            typewriter.Play(_addressStyle.WrapAddressText(line.GetActiveText()), _addressStyle.AddressTypewriterCPS);
        else if (line.isInnerMonologue)
            typewriter.Play($"<i>{line.GetActiveText()}</i>");
        else
            typewriter.Play(line.GetActiveText());
    }

    // ==========================================
    // SetArrow - Show or Hide the Continue Arrow with Blink
    // ==========================================
    public void SetArrow(bool visible)
    {
        if (continueArrow == null) return;

        if (_blinkCo != null) { StopCoroutine(_blinkCo); _blinkCo = null; }

        continueArrow.SetActive(visible);
        if (visible)
            _blinkCo = StartCoroutine(BlinkArrow());
    }

    // ==========================================
    // IsTyping - Delegate to TypewriterEffect
    // ==========================================
    public bool IsTyping => typewriter != null && typewriter.IsTyping;

    // ==========================================
    // SkipTypewriter - Reveal All Remaining Characters Instantly
    // ==========================================
    public void SkipTypewriter()
    {
        if (typewriter != null) typewriter.Skip();
    }

    // ==========================================
    // SetSpeakerLabel - Update Speaker Name Text and Backing Visibility
    // ==========================================
    private void SetSpeakerLabel(string id)
    {
        if (speakerLabel == null) return;
        speakerLabel.text = id;
        bool show = !string.IsNullOrEmpty(id);
        speakerLabel.gameObject.SetActive(show);
        if (speakerNameBacking != null) speakerNameBacking.gameObject.SetActive(show);
    }

    // ==========================================
    // ApplyStyle - Swap Panel Tint for Normal vs Fourth-Wall Lines
    // ==========================================
    private void ApplyStyle(bool isFourthWall)

    {

        if (panelBackground == null) return;

        if (_addressStyle != null) return;

        panelBackground.color = isFourthWall ? fourthWallTint : _defaultBgColor;

    }
    // ==========================================
    // ApplyAddressStyle - Activate Direct-to-Player Visual Profile (FourthWallBreaker)
    // ==========================================
    public void ApplyAddressStyle(PlayerAddressStyle addressStyle)
    {
        _addressStyle = addressStyle;
        if (_addressStyle == null) return;
        TintPanelTo(_addressStyle.AddressPanelTint, _addressStyle.PanelTintTransitionDuration);
    }
    // ==========================================
    // ClearAddressStyle - Deactivate Direct-to-Player Visual Profile, Restore Defaults
    // ==========================================
    public void ClearAddressStyle()
    {
        float duration = _addressStyle != null ? _addressStyle.PanelTintTransitionDuration : fadeOutDuration;
        _addressStyle = null;
        TintPanelTo(_defaultBgColor, duration);
    }
    // ==========================================
    // TintPanelTo - Lerp Panel Background Color to Target Over Duration
    // ==========================================
    private void TintPanelTo(Color target, float duration)
    {
        if (panelBackground == null) return;
        if (_tintCo != null) StopCoroutine(_tintCo);
        _tintCo = StartCoroutine(TintCoroutine(panelBackground.color, target, duration));
    }
    // ==========================================
    // TintCoroutine - Color Lerp Helper for Panel Background
    // ==========================================
    private IEnumerator TintCoroutine(Color from, Color to, float duration)
    {
        if (duration <= 0f)
        {
            panelBackground.color = to;
            _tintCo = null;
            yield break;
        }
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            panelBackground.color = Color.Lerp(from, to, t / duration);
            yield return null;
        }
        panelBackground.color = to;
        _tintCo = null;
    }

    // ==========================================
    // BlinkArrow - Alternating Show/Hide on Continue Arrow
    // ==========================================
    private IEnumerator BlinkArrow()
    {
        while (true)
        {
            yield return new WaitForSeconds(arrowBlinkInterval);
            if (continueArrow != null) continueArrow.SetActive(false);
            yield return new WaitForSeconds(arrowBlinkInterval * 0.6f);
            if (continueArrow != null) continueArrow.SetActive(true);
        }
    }

    // ==========================================
    // HideRoutine - Fade Out Alpha Then Deactivate
    // ==========================================
    private IEnumerator HideRoutine()
    {
        float start = panelGroup != null ? panelGroup.alpha : 1f;
        float t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            if (panelGroup != null) panelGroup.alpha = Mathf.Lerp(start, 0f, t / fadeOutDuration);
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
        float from = panelGroup != null ? panelGroup.alpha : 0f;
        _fadeCo = StartCoroutine(FadeCoroutine(from, target, dur));
    }

    // ==========================================
    // FadeCoroutine - Lerp CanvasGroup Alpha Over Duration
    // ==========================================
    private IEnumerator FadeCoroutine(float from, float to, float dur)
    {
        if (panelGroup == null) yield break;
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            panelGroup.alpha = Mathf.Lerp(from, to, t / dur);
            yield return null;
        }
        panelGroup.alpha = to;
        _fadeCo = null;
    }

    // ==========================================
    // SetCG - Apply Interaction State to CanvasGroup
    // ==========================================
    private void SetCG(float alpha, bool interact)
    {
        if (panelGroup == null) return;
        panelGroup.alpha = alpha;
        panelGroup.interactable = interact;
        panelGroup.blocksRaycasts = interact;
    }

    // ==========================================
    // ForceHidden - Instant Zero-Alpha, No Interaction, Deactivate
    // ==========================================
    private void ForceHidden()
    {
        SetCG(0f, false);
        if (continueArrow != null) continueArrow.SetActive(false);
        if (_blinkCo != null) { StopCoroutine(_blinkCo); _blinkCo = null; }
        gameObject.SetActive(false);
    }
}