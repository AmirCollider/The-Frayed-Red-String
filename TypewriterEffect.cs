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
    // TypeRoutine - Reveal Characters via TMP maxVisibleCharacters
    // ==========================================
    private IEnumerator TypeRoutine(string fullText, float cps)
    {
        _isTyping = true;
        _label.text = fullText;
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