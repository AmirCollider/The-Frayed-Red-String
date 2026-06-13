// ==========================================
// FourthWallBreaker - Camera Stare, Music Cut, Tears Overlay, Direct-Address Styling
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FourthWallBreaker : MonoBehaviour
{
    // ==========================================
    // Inspector — Style Profile
    // ==========================================
    [Header("Style Profile")]
    [SerializeField] private PlayerAddressStyle style;

    // ==========================================
    // Inspector — Camera
    // ==========================================
    [Header("Camera")]
    [SerializeField] private Camera targetCamera;

    // ==========================================
    // Inspector — Dialogue Box
    // ==========================================
    [Header("Dialogue Box")]
    [SerializeField] private DialogueBoxUI dialogueBox;

    // ==========================================
    // Inspector — Tears Overlay
    // ==========================================
    [Header("Tears Overlay")]
    [SerializeField] private CanvasGroup tearsGroup;
    [SerializeField] private Image tearsImage;

    // ==========================================
    // Events
    // ==========================================
    [Header("Events")]
    public UnityEvent OnBreakStarted = new UnityEvent();
    public UnityEvent OnBreakEnded = new UnityEvent();

    // ==========================================
    // Private State
    // ==========================================
    private float _defaultCameraSize;
    private Coroutine _cameraCo;
    private Coroutine _tearsCo;
    private bool _isActive;

    // ==========================================
    // Awake - Cache Default Camera Size, Resolve Main Camera Fallback, Force Tears Hidden
    // ==========================================
    private void Awake()
    {
        if (targetCamera == null) targetCamera = Camera.main;
        if (targetCamera != null) _defaultCameraSize = targetCamera.orthographicSize;

        if (tearsGroup != null) tearsGroup.alpha = 0f;
        if (tearsImage != null)
        {
            Color c = tearsImage.color;
            c.a = 0f;
            tearsImage.color = c;
        }
    }

    // ==========================================
    // TriggerBreak - Begin Fourth-Wall Address: Music Cut, Camera Stare, Dialogue Tint, Tears
    // ==========================================
    public void TriggerBreak()
    {
        if (_isActive) return;
        _isActive = true;

        AudioManager.Instance?.FadeOutBGM(style != null ? style.MusicFadeOutDuration : 0.4f);
        dialogueBox?.ApplyAddressStyle(style);

        if (targetCamera != null)
        {
            float target = style != null ? style.StaredCameraSize : targetCamera.orthographicSize;
            float dur = style != null ? style.CameraTransitionDuration : 0f;
            ZoomCameraTo(target, dur);
        }

        if (style != null)
        {
            if (tearsImage != null)
            {
                Color c = style.TearsOverlayColor;
                c.a = tearsImage.color.a;
                tearsImage.color = c;
            }
            FadeTears(style.TearsTargetAlpha, style.TearsFadeInDuration);
        }

        OnBreakStarted.Invoke();
    }

    // ==========================================
    // EndBreak - Restore Camera, Clear Dialogue Tint, Fade Out Tears
    // ==========================================
    public void EndBreak()
    {
        if (!_isActive) return;
        _isActive = false;

        dialogueBox?.ClearAddressStyle();

        float camDur = style != null ? style.CameraTransitionDuration : 0f;
        ZoomCameraTo(_defaultCameraSize, camDur);

        float tearsDur = style != null ? style.TearsFadeInDuration : 0.5f;
        FadeTears(0f, tearsDur);

        OnBreakEnded.Invoke();
    }

    // ==========================================
    // IsActive - Whether the Fourth-Wall Address Is Currently Engaged
    // ==========================================
    public bool IsActive => _isActive;

    // ==========================================
    // ZoomCameraTo - Start Orthographic Size Lerp Coroutine
    // ==========================================
    private void ZoomCameraTo(float target, float duration)
    {
        if (targetCamera == null) return;
        if (_cameraCo != null) StopCoroutine(_cameraCo);
        _cameraCo = StartCoroutine(ZoomCameraRoutine(target, duration));
    }

    // ==========================================
    // ZoomCameraRoutine - Lerp Orthographic Size to Target Over Duration
    // ==========================================
    private IEnumerator ZoomCameraRoutine(float target, float duration)
    {
        if (duration <= 0f)
        {
            targetCamera.orthographicSize = target;
            _cameraCo = null;
            yield break;
        }

        float start = targetCamera.orthographicSize;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            targetCamera.orthographicSize = Mathf.Lerp(start, target, t / duration);
            yield return null;
        }
        targetCamera.orthographicSize = target;
        _cameraCo = null;
    }

    // ==========================================
    // FadeTears - Start Tears Overlay Alpha Lerp Coroutine
    // ==========================================
    private void FadeTears(float target, float duration)
    {
        if (_tearsCo != null) StopCoroutine(_tearsCo);
        _tearsCo = StartCoroutine(FadeTearsRoutine(target, duration));
    }

    // ==========================================
    // FadeTearsRoutine - Lerp CanvasGroup/Image Alpha to Target Over Duration
    // ==========================================
    private IEnumerator FadeTearsRoutine(float target, float duration)
    {
        float startGroup = tearsGroup != null ? tearsGroup.alpha : 0f;
        float startImage = tearsImage != null ? tearsImage.color.a : 0f;

        if (duration <= 0f)
        {
            SetTearsAlpha(target);
            _tearsCo = null;
            yield break;
        }

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;
            if (tearsGroup != null) tearsGroup.alpha = Mathf.Lerp(startGroup, target, lerp);
            if (tearsImage != null)
            {
                Color c = tearsImage.color;
                c.a = Mathf.Lerp(startImage, target, lerp);
                tearsImage.color = c;
            }
            yield return null;
        }

        SetTearsAlpha(target);
        _tearsCo = null;
    }

    // ==========================================
    // SetTearsAlpha - Apply Final Alpha to Tears Group and Image
    // ==========================================
    private void SetTearsAlpha(float alpha)
    {
        if (tearsGroup != null) tearsGroup.alpha = alpha;
        if (tearsImage != null)
        {
            Color c = tearsImage.color;
            c.a = alpha;
            tearsImage.color = c;
        }
    }
}