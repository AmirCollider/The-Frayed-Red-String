// ==========================================
// FrameAnimator - Coroutine Tween of Frame Border Sizes per State Change
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using UnityEngine;

public class FrameAnimator : MonoBehaviour
{
    // ==========================================
    // Inspector - Controller Reference
    // ==========================================
    [Header("Frame Controller")]
    [SerializeField] private FrameController controller;

    // ==========================================
    // Inspector - Per-State Size Profiles
    // ==========================================
    [Header("State Profiles (one per FrameState)")]
    [SerializeField] private FrameStateProfile[] profiles;

    // ==========================================
    // Private State
    // ==========================================
    private Coroutine _tweenCo;

    // ==========================================
    // OnEnable - Subscribe to Frame State Changed Event
    // ==========================================
    private void OnEnable()
    {
        if (controller != null)
            controller.OnFrameStateChanged.AddListener(OnStateChanged);
    }

    // ==========================================
    // OnDisable - Unsubscribe from Frame State Changed Event
    // ==========================================
    private void OnDisable()
    {
        if (controller != null)
            controller.OnFrameStateChanged.RemoveListener(OnStateChanged);
    }

    // ==========================================
    // OnStateChanged - Look Up Profile and Start Tween
    // ==========================================
    private void OnStateChanged(FrameState newState)
    {
        FrameStateProfile profile = GetProfile(newState);
        if (profile == null) return;

        if (_tweenCo != null) StopCoroutine(_tweenCo);
        _tweenCo = StartCoroutine(TweenToProfile(profile));
    }

    // ==========================================
    // GetProfile - Linear Search by FrameState
    // ==========================================
    private FrameStateProfile GetProfile(FrameState state)
    {
        if (profiles == null) return null;
        foreach (FrameStateProfile p in profiles)
            if (p.state == state) return p;
        return null;
    }

    // ==========================================
    // TweenToProfile - Lerp All Four Border Dimensions to Target Profile
    // ==========================================
    private IEnumerator TweenToProfile(FrameStateProfile target)
    {
        if (controller == null) yield break;

        RectTransform top = controller.FrameTop;
        RectTransform bottom = controller.FrameBottom;
        RectTransform left = controller.FrameLeft;
        RectTransform right = controller.FrameRight;

        if (top == null || bottom == null || left == null || right == null) yield break;

        float startTopH = top.sizeDelta.y;
        float startBottomH = bottom.sizeDelta.y;
        float startLeftW = left.sizeDelta.x;
        float startRightW = right.sizeDelta.x;

        float elapsed = 0f;
        float duration = Mathf.Max(target.transitionDuration, 0.0001f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);

            Vector2 topSD = top.sizeDelta; topSD.y = Mathf.Lerp(startTopH, target.topBarHeight, t); top.sizeDelta = topSD;
            Vector2 bottomSD = bottom.sizeDelta; bottomSD.y = Mathf.Lerp(startBottomH, target.bottomBarHeight, t); bottom.sizeDelta = bottomSD;
            Vector2 leftSD = left.sizeDelta; leftSD.x = Mathf.Lerp(startLeftW, target.leftBarWidth, t); left.sizeDelta = leftSD;
            Vector2 rightSD = right.sizeDelta; rightSD.x = Mathf.Lerp(startRightW, target.rightBarWidth, t); right.sizeDelta = rightSD;

            yield return null;
        }

        // ==========================================
        // Snap to Final Target Values
        // ==========================================
        Vector2 tf = top.sizeDelta; tf.y = target.topBarHeight; top.sizeDelta = tf;
        Vector2 bf = bottom.sizeDelta; bf.y = target.bottomBarHeight; bottom.sizeDelta = bf;
        Vector2 lf = left.sizeDelta; lf.x = target.leftBarWidth; left.sizeDelta = lf;
        Vector2 rf = right.sizeDelta; rf.x = target.rightBarWidth; right.sizeDelta = rf;

        _tweenCo = null;
    }
}

// ==========================================
// FrameStateProfile - Serializable Border Dimensions for a Single Frame State
// ==========================================
[System.Serializable]
public class FrameStateProfile
{
    public FrameState state;

    [Header("Border Sizes (pixels at 1920x1080)")]
    public float topBarHeight = 60f;
    public float bottomBarHeight = 60f;
    public float leftBarWidth = 80f;
    public float rightBarWidth = 80f;

    [Header("Tween")]
    public float transitionDuration = 0.5f;
}