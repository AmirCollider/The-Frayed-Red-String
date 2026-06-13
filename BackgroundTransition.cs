// ==========================================
// BackgroundTransition - Dual-Image Crossfade, Instant-Cut, and Glitch-Cut Modes
// AmirCollider Games - The Frayed Red String
// ==========================================

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundTransition : MonoBehaviour
{
    // ==========================================
    // Inspector — Dual Images (A = current visible, B = incoming)
    // ==========================================
    [Header("Background Images")]
    [SerializeField] private Image imageA;
    [SerializeField] private Image imageB;

    // ==========================================
    // Inspector — Crossfade Settings
    // ==========================================
    [Header("Crossfade")]
    [SerializeField] private float crossfadeDuration = 0.4f;

    // ==========================================
    // Inspector — Glitch-Cut Controller Reference
    // ==========================================
    [Header("Glitch-Cut")]
    [SerializeField] private GlitchCutTransition glitchCut;

    // ==========================================
    // Private State
    // ==========================================
    private Coroutine _fadeCo;

    // ==========================================
    // Awake - Initialize Alpha State
    // ==========================================
    private void Awake()
    {
        if (imageA != null) SetAlpha(imageA, 1f);
        if (imageB != null) SetAlpha(imageB, 0f);
    }

    // ==========================================
    // InstantCut - Apply Sprite with No Transition
    // ==========================================
    public void InstantCut(Sprite sprite)
    {
        KillFade();
        if (imageA != null)
        {
            imageA.sprite = sprite;
            SetAlpha(imageA, 1f);
        }
        if (imageB != null) SetAlpha(imageB, 0f);
    }

    // ==========================================
    // Crossfade - Smooth Alpha Transition to New Sprite
    // ==========================================
    public void Crossfade(Sprite sprite, float overrideDuration = -1f)
    {
        if (sprite == null) return;
        KillFade();
        float dur = overrideDuration > 0f ? overrideDuration : crossfadeDuration;
        _fadeCo = StartCoroutine(CrossfadeRoutine(sprite, dur));
    }

    // ==========================================
    // GlitchCut - RGB-Split Flash Then InstantCut to New Sprite
    // ==========================================
    public void GlitchCut(Sprite sprite)
    {
        if (sprite == null) return;
        KillFade();

        if (glitchCut != null)
            glitchCut.TriggerGlitchCut(() => InstantCut(sprite));
        else
            InstantCut(sprite);
    }

    // ==========================================
    // CrossfadeRoutine - SmoothStep Alpha Swap Then Commit to ImageA
    // ==========================================
    private IEnumerator CrossfadeRoutine(Sprite targetSprite, float duration)
    {
        if (imageA == null || imageB == null) yield break;

        imageB.sprite = targetSprite;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            SetAlpha(imageA, 1f - t);
            SetAlpha(imageB, t);
            yield return null;
        }

        // ==========================================
        // Commit — Promote Incoming to Primary, Zero Secondary
        // ==========================================
        imageA.sprite = imageB.sprite;
        SetAlpha(imageA, 1f);
        SetAlpha(imageB, 0f);
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
        if (imageA != null) SetAlpha(imageA, 1f);
        if (imageB != null) SetAlpha(imageB, 0f);
    }

    // ==========================================
    // SetAlpha - Apply Alpha to Image Component Color
    // ==========================================
    private static void SetAlpha(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }

    // ==========================================
    // IsTransitioning - Returns True While Crossfade is Active
    // ==========================================
    public bool IsTransitioning => _fadeCo != null;
}

// ==========================================
// BackgroundTransitionMode - Available Transition Types
// ==========================================
public enum BackgroundTransitionMode
{
    Instant = 0,
    Crossfade = 1,
    GlitchCut = 2
}