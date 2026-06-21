// ==========================================
// TypewriterEffect - Character-by-Character TMP Text Reveal
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TypewriterEffect : MonoBehaviour
{
    // ==========================================
    // Inspector — Speed
    // ==========================================
    [Header("Speed")]
    [SerializeField] private float charsPerSecond = ConstantsConfig.TYPEWRITER_DEFAULT_CPS;
    [SerializeField] private float punctuationPauseMult = 3f;

    // ==========================================
    // Inspector — Adaptive Font Sizing (R5)
    //   Short lines render large; long lines shrink. Sizes scale with screen
    //   height so 1440p/4K read identically to the 1080p baseline.
    // ==========================================
    [Header("Adaptive Font Size")]
    [SerializeField] private bool enableAutoSize = true;
    [SerializeField] private float minFontSize = 28f;
    [SerializeField] private float maxFontSizeShort = 60f;
    [SerializeField] private float maxFontSizeLong = 40f;
    [SerializeField] private int longLineCharThreshold = 90;

    // ==========================================
    // Arrow / Edge Clearance (left, top, right, bottom) in TMP local units.
    // Extra right + bottom keep the last line clear of the blinking ▼ arrow.
    // ==========================================
    [SerializeField] private Vector4 textMargin = new Vector4(28f, 18f, 60f, 44f);

    // ==========================================
    // Inspector — Punctuation Set
    // ==========================================
    [Header("Punctuation Pause Characters")]
    [SerializeField] private string pauseChars = ".,!?。、…—";

    // ==========================================
    // Events
    // ==========================================
    [Header("Events")]
    public UnityEvent OnTypingComplete = new UnityEvent();
    public UnityEvent<char> OnCharacterRevealed = new UnityEvent<char>();

    // ==========================================
    // Private State
    // ==========================================
    private TextMeshProUGUI _label;
    private Coroutine _typeCo;
    private bool _isTyping;

    // ==========================================
    // Awake - Cache TMP Component
    // ==========================================
    private void Awake()
    {
        _label = GetComponent<TextMeshProUGUI>();
    }

    // ==========================================
    // Play - Begin Typewriter Reveal for Given String
    // ==========================================
    public void Play(string fullText, float overrideCPS = -1f)
    {
        if (_typeCo != null) StopCoroutine(_typeCo);
        float cps = overrideCPS > 0f ? overrideCPS : charsPerSecond;
        _typeCo = StartCoroutine(TypeRoutine(fullText, cps));
    }

    // ==========================================
    // Skip - Reveal All Remaining Characters Instantly
    // ==========================================
    public void Skip()
    {
        if (!_isTyping) return;
        if (_typeCo != null) { StopCoroutine(_typeCo); _typeCo = null; }
        if (_label != null) _label.maxVisibleCharacters = int.MaxValue;
        _isTyping = false;
        OnTypingComplete.Invoke();
    }

    // ==========================================
    // IsTyping - Returns True While Reveal is In Progress
    // ==========================================
    public bool IsTyping => _isTyping;

    // ==========================================
    // ApplyAdaptiveFontSize - Length + Screen-Scaled Ceiling, Auto-Shrink to Fit (R5, Batch 2)
    // The length/screen curve is now the UPPER bound (fontSizeMax): short lines
    // still render large, long lines start smaller. TMP auto-sizing shrinks the
    // line toward minFontSize ONLY when it would otherwise exceed the real
    // DialogueText rect, so text can never spill past the panel or under the ▼
    // arrow on small windows / narrow aspect ratios. textMargin reserves the
    // arrow corner; layout is computed for the full string (maxVisibleCharacters
    // does not affect auto-size), so the box does not jitter during the reveal.
    // ==========================================
    private void ApplyAdaptiveFontSize(string fullText)

    {
        if (_label == null || !enableAutoSize) return;
        int len = string.IsNullOrEmpty(fullText) ? 0 : fullText.Length;

        float screenScale = (float)Screen.height / ConstantsConfig.BASE_HEIGHT;
        screenScale = Mathf.Clamp(screenScale, 0.85f, 2.2f);
        float lengthT = longLineCharThreshold > 0
            ? Mathf.Clamp01((float)len / longLineCharThreshold)
            : 0f;
        float lengthSize = Mathf.Lerp(maxFontSizeShort, maxFontSizeLong, lengthT);
        float ceiling = Mathf.Max(lengthSize, minFontSize) * screenScale;
        float floor = Mathf.Min(minFontSize, ceiling);

        _label.textWrappingMode = TMPro.TextWrappingModes.Normal;
        _label.overflowMode = TextOverflowModes.Truncate;
        _label.margin = textMargin;
        _label.enableAutoSizing = true;
        _label.fontSizeMin = floor;
        _label.fontSizeMax = ceiling;
    }

    // ==========================================
    // TypeRoutine - Reveal Characters via TMP maxVisibleCharacters
    // ==========================================
    private IEnumerator TypeRoutine(string fullText, float cps)
    {
        _isTyping = true;
        _label.text = fullText;
        ApplyAdaptiveFontSize(fullText);
        _label.ForceMeshUpdate();

        int total = _label.textInfo.characterCount;
        _label.maxVisibleCharacters = 0;

        float delay = cps > 0f ? 1f / cps : 0f;

        for (int i = 0; i < total; i++)
        {
            _label.maxVisibleCharacters = i + 1;

            // ==========================================
            // Read actual visible character via textInfo to bypass rich-text tags
            // ==========================================
            char c = (i < _label.textInfo.characterInfo.Length)
                ? _label.textInfo.characterInfo[i].character
                : ' ';

            OnCharacterRevealed.Invoke(c);

            float wait = (delay > 0f && pauseChars.IndexOf(c) >= 0)
                ? delay * punctuationPauseMult
                : delay;

            if (wait > 0f) yield return new WaitForSeconds(wait);
        }

        _label.maxVisibleCharacters = int.MaxValue;
        _isTyping = false;
        _typeCo = null;
        OnTypingComplete.Invoke();
    }
}