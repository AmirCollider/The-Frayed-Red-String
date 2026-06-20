// ==========================================
// TitleCardController - Fullscreen Black Act Intro/Outro Title Card
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleCardController : MonoBehaviour
{
    // ==========================================
    // Singleton Instance (per-scene — no DontDestroyOnLoad)
    // ==========================================
    public static TitleCardController Instance { get; private set; }

    // ==========================================
    // Inspector — Root
    // ==========================================
    [Header("Root")]
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Image blackBackground;

    // ==========================================
    // Inspector — Text (heading = act/image name, body = author line)
    // ==========================================
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI headingText;
    [SerializeField] private TextMeshProUGUI bodyText;

    // ==========================================
    // Inspector — Colors
    // ==========================================
    [Header("Colors")]
    [SerializeField] private Color backgroundColor = Color.black;
    [SerializeField] private Color textColor = Color.white;

    // ==========================================
    // Inspector — Timing (seconds)
    // ==========================================
    [Header("Timing")]
    [SerializeField] private float fadeInDuration = 0.6f;
    [SerializeField] private float fadeOutDuration = 0.6f;
    [SerializeField] private float textRevealDelay = 0.25f;

    // ==========================================
    // Private State
    // ==========================================
    private Coroutine _fadeCo;

    // ==========================================
    // Awake - Singleton Enforcement + Force Hidden on Scene Load
    // ==========================================
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        ForceHidden();
    }

    // ==========================================
    // OnDestroy - Clear Singleton Reference
    // ==========================================
    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    // ==========================================
    // FadeIn - Show Black Card, Apply Act Font, Reveal Heading + Body
    // ==========================================
    public IEnumerator FadeIn(string heading, string body)
    {
        if (blackBackground != null) blackBackground.color = backgroundColor;
        ApplyText(headingText, heading);
        ApplyText(bodyText, body);

        if (group != null)
        {
            group.interactable = false;
            group.blocksRaycasts = true;
        }

        if (_fadeCo != null) StopCoroutine(_fadeCo);
        yield return _fadeCo = StartCoroutine(FadeRoutine(0f, 1f, fadeInDuration));

        if (textRevealDelay > 0f)
            yield return new WaitForSeconds(textRevealDelay);
    }

    // ==========================================
    // FadeOut - Fade the Card Out and Restore Non-Blocking Hidden State
    // ==========================================
    public IEnumerator FadeOut()
    {
        if (_fadeCo != null) StopCoroutine(_fadeCo);
        yield return _fadeCo = StartCoroutine(FadeRoutine(group != null ? group.alpha : 1f, 0f, fadeOutDuration));
        ForceHidden();
    }

    // ==========================================
    // ApplyText - Set String, Color, Auto Font (LocalizedFontController); Hide if Empty
    // ==========================================
    private void ApplyText(TextMeshProUGUI target, string value)
    {
        if (target == null) return;

        bool has = !string.IsNullOrEmpty(value);
        target.gameObject.SetActive(has);
        if (!has) return;

        // ==========================================
        // Font — Auto-Resolved by the Scene's LocalizedFontController
        // (this act's EN/JP pair + active language). Falls back to the TMP
        // object's existing font if no controller is present in the scene.
        // ==========================================
        if (LocalizedFontController.Instance != null && LocalizedFontController.Instance.CurrentFont != null)
            target.font = LocalizedFontController.Instance.CurrentFont;

        target.color = textColor;
        target.text = value;
    }

    // ==========================================
    // FadeRoutine - Lerp CanvasGroup Alpha Between Two Values
    // ==========================================
    private IEnumerator FadeRoutine(float from, float to, float duration)
    {
        if (group == null) yield break;

        if (duration <= 0f)
        {
            group.alpha = to;
            yield break;
        }

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        group.alpha = to;
        _fadeCo = null;
    }

    // ==========================================
    // ForceHidden - Snap to Fully Hidden, Non-Blocking State (root stays active)
    // ==========================================
    private void ForceHidden()
    {
        if (group != null)
        {
            group.alpha = 0f;
            group.interactable = false;
            group.blocksRaycasts = false;
        }
    }
}