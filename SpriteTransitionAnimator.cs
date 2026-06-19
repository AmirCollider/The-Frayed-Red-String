// ==========================================
// SpriteTransitionAnimator - Dual-SpriteRenderer Crossfade Between Character States
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using UnityEngine;

public class SpriteTransitionAnimator : MonoBehaviour
{
    // ==========================================
    // Inspector — Dual Renderers (A = current visible, B = incoming)
    // ==========================================
    [Header("Sprite Renderers")]
    [SerializeField] private SpriteRenderer spriteA;
    [SerializeField] private SpriteRenderer spriteB;

    // ==========================================
    // Inspector — Crossfade
    // ==========================================
    [Header("Crossfade")]
    [SerializeField] private float crossfadeDuration = 0.15f;

    // ==========================================
    // Private State
    // ==========================================
    private Coroutine _fadeCo;

    // ==========================================
    // Focus Tint State — RGB Spotlight/Dim Multiplier (alpha is owned by the crossfade)
    // ==========================================
    private Color _focusTint = Color.white;
    private Coroutine _tintCo;

    // ==========================================
    // Awake - Initialize Alpha State
    // ==========================================
    private void Awake()
    {
        if (spriteA != null) spriteA.color = Tinted(1f);
        if (spriteB != null) spriteB.color = Tinted(0f);
    }

    // ==========================================
    // Tinted - Compose the Current Focus RGB With a Supplied Alpha
    // ==========================================
    private Color Tinted(float alpha)
    {
        return new Color(_focusTint.r, _focusTint.g, _focusTint.b, alpha);
    }

    // ==========================================
    // SetInstant - Apply Sprite with No Transition
    // ==========================================
    public void SetInstant(Sprite sprite)
    {
        KillFade();
        if (spriteA != null)
        {
            spriteA.sprite = sprite;
            spriteA.color = Tinted(1f);
        }
        if (spriteB != null)
            spriteB.color = Tinted(0f);
    }

    // ==========================================
    // CrossfadeTo - Begin Alpha Crossfade to Target Sprite
    // ==========================================
    public void CrossfadeTo(Sprite sprite)
    {
        if (sprite == null) return;
        KillFade();
        _fadeCo = StartCoroutine(CrossfadeRoutine(sprite));
    }

    // ==========================================
    // CrossfadeRoutine - SmoothStep Alpha Swap, Then Commit to SpriteA
    // ==========================================
    private IEnumerator CrossfadeRoutine(Sprite targetSprite)
    {
        if (spriteA == null || spriteB == null) yield break;

        spriteB.sprite = targetSprite;

        float elapsed = 0f;
        while (elapsed < crossfadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / crossfadeDuration);
            spriteA.color = Tinted(1f - t);
            spriteB.color = Tinted(t);
            yield return null;
        }

        // ==========================================
        // Commit — Promote Incoming Sprite to Primary, Zero Secondary
        // ==========================================
        spriteA.sprite = spriteB.sprite;
        spriteA.color = Tinted(1f);
        spriteB.color = Tinted(0f);
        _fadeCo = null;

        // ==========================================
        // Commit — Promote Incoming Sprite to Primary, Zero Secondary
        // ==========================================
        spriteA.sprite = spriteB.sprite;
        spriteA.color = Color.white;
        spriteB.color = new Color(1f, 1f, 1f, 0f);
        _fadeCo = null;
    }

    // ==========================================
    // KillFade - Stop Active Coroutine and Snap to Clean State
    // ==========================================
    private void KillFade()
    {
        if (_fadeCo == null) return;
        StopCoroutine(_fadeCo);
        _fadeCo = null;
        if (spriteA != null) spriteA.color = Tinted(1f);
        if (spriteB != null) spriteB.color = Tinted(0f);
    }

    // ==========================================
    // SetFocusTint - Lerp the RGB Spotlight/Dim Multiplier (alpha preserved per renderer)
    // ==========================================
    public void SetFocusTint(Color rgb, float duration)
    {
        rgb.a = 1f;
        if (_tintCo != null) StopCoroutine(_tintCo);
        if (duration <= 0f || !gameObject.activeInHierarchy)
        {
            _focusTint = rgb;
            ApplyFocusTint();
            return;
        }
        _tintCo = StartCoroutine(FocusTintRoutine(rgb, duration));
    }

    // ==========================================
    // FocusTintRoutine - Smooth RGB Lerp; Re-Applies Tint Each Frame Preserving Alpha
    // ==========================================
    private IEnumerator FocusTintRoutine(Color target, float duration)
    {
        Color from = _focusTint;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            _focusTint = Color.Lerp(from, target, Mathf.SmoothStep(0f, 1f, t / duration));
            ApplyFocusTint();
            yield return null;
        }
        _focusTint = target;
        ApplyFocusTint();
        _tintCo = null;
    }

    // ==========================================
    // ApplyFocusTint - Push Current Focus RGB Onto Both Renderers, Keeping Their Alpha
    // ==========================================
    private void ApplyFocusTint()
    {
        if (spriteA != null) spriteA.color = Tinted(spriteA.color.a);
        if (spriteB != null) spriteB.color = Tinted(spriteB.color.a);
    }

    // ==========================================
    // Accessors
    // ==========================================
    public Sprite CurrentSprite => spriteA != null ? spriteA.sprite : null;
    public bool IsTransitioning => _fadeCo != null;
}