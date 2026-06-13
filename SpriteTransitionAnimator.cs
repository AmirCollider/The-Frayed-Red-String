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
    // Awake - Initialize Alpha State
    // ==========================================
    private void Awake()
    {
        if (spriteA != null) spriteA.color = Color.white;
        if (spriteB != null) spriteB.color = new Color(1f, 1f, 1f, 0f);
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
            spriteA.color = Color.white;
        }
        if (spriteB != null)
            spriteB.color = new Color(1f, 1f, 1f, 0f);
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
            spriteA.color = new Color(1f, 1f, 1f, 1f - t);
            spriteB.color = new Color(1f, 1f, 1f, t);
            yield return null;
        }

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
        if (spriteA != null) spriteA.color = Color.white;
        if (spriteB != null) spriteB.color = new Color(1f, 1f, 1f, 0f);
    }

    // ==========================================
    // Accessors
    // ==========================================
    public Sprite CurrentSprite => spriteA != null ? spriteA.sprite : null;
    public bool IsTransitioning => _fadeCo != null;
}