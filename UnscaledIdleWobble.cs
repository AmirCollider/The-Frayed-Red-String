// ==========================================
// UnscaledIdleWobble - Idle Float/Tilt/Pulse That Runs While Paused (unscaled time)
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;

public class UnscaledIdleWobble : MonoBehaviour
{
    // ==========================================
    // Inspector — Motion Amounts
    // ==========================================
    [Header("Float (pixels)")]
    [SerializeField] private float floatAmplitude = 6f;
    [SerializeField] private float floatSpeed = 1.4f;

    [Header("Tilt (degrees)")]
    [SerializeField] private float tiltAmplitude = 2.5f;
    [SerializeField] private float tiltSpeed = 1.1f;

    [Header("Scale Pulse")]
    [SerializeField] private float scaleAmplitude = 0.03f;
    [SerializeField] private float scaleSpeed = 1.7f;

    // ==========================================
    // Private — Cached Base Transform (captured at enable)
    // ==========================================
    private RectTransform _rt;
    private Vector2 _baseAnchoredPos;
    private Vector3 _baseScale;
    private float _baseZ;
    private float _phase;

    // ==========================================
    // OnEnable - Cache Base Pose and Randomize Phase per Button
    // ==========================================
    private void OnEnable()
    {
        _rt = transform as RectTransform;
        if (_rt != null)
        {
            _baseAnchoredPos = _rt.anchoredPosition;
            _baseScale = _rt.localScale;
            _baseZ = _rt.localEulerAngles.z;
        }
        _phase = Random.Range(0f, Mathf.PI * 2f);
    }

    // ==========================================
    // Update - Drive Float/Tilt/Pulse Using unscaledTime (works at Time.timeScale = 0)
    // ==========================================
    private void Update()
    {
        if (_rt == null) return;

        float t = Time.unscaledTime + _phase;

        float y = Mathf.Sin(t * floatSpeed) * floatAmplitude;
        _rt.anchoredPosition = _baseAnchoredPos + new Vector2(0f, y);

        float z = _baseZ + Mathf.Sin(t * tiltSpeed) * tiltAmplitude;
        Vector3 e = _rt.localEulerAngles;
        e.z = z;
        _rt.localEulerAngles = e;

        float s = 1f + Mathf.Sin(t * scaleSpeed) * scaleAmplitude;
        _rt.localScale = _baseScale * s;
    }

    // ==========================================
    // OnDisable - Restore the Captured Base Pose
    // ==========================================
    private void OnDisable()
    {
        if (_rt == null) return;
        _rt.anchoredPosition = _baseAnchoredPos;
        _rt.localScale = _baseScale;
        Vector3 e = _rt.localEulerAngles;
        e.z = _baseZ;
        _rt.localEulerAngles = e;
    }
}