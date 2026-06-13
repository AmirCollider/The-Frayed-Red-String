// ==========================================
// AspectRatioEnforcer - 16:9 Letterbox / Pillarbox via Camera.rect at Runtime
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatioEnforcer : MonoBehaviour
{
    // ==========================================
    // Private State
    // ==========================================
    private Camera _cam;
    private int _lastScreenWidth;
    private int _lastScreenHeight;

    // ==========================================
    // Awake - Cache Camera Reference and Apply Initial Constraint
    // ==========================================
    private void Awake()
    {
        _cam = GetComponent<Camera>();
        _lastScreenWidth = Screen.width;
        _lastScreenHeight = Screen.height;
        Enforce();
    }

    // ==========================================
    // Update - Re-enforce on Screen Resolution Change
    // ==========================================
    private void Update()
    {
        if (Screen.width == _lastScreenWidth && Screen.height == _lastScreenHeight) return;
        _lastScreenWidth = Screen.width;
        _lastScreenHeight = Screen.height;
        Enforce();
    }

    // ==========================================
    // Enforce - Compute and Apply Camera.rect to Maintain 16:9
    // ==========================================
    public void Enforce()
    {
        if (_cam == null) _cam = GetComponent<Camera>();

        float targetAspect = (float)ConstantsConfig.BASE_WIDTH / ConstantsConfig.BASE_HEIGHT;
        float screenAspect = (float)Screen.width / Screen.height;
        float scaleHeight = screenAspect / targetAspect;

        if (Mathf.Approximately(scaleHeight, 1f))
        {
            _cam.rect = new Rect(0f, 0f, 1f, 1f);
            return;
        }

        if (scaleHeight < 1f)
        {
            // Letterbox: horizontal black bars top and bottom
            _cam.rect = new Rect(0f, (1f - scaleHeight) * 0.5f, 1f, scaleHeight);
        }
        else
        {
            // Pillarbox: vertical black bars left and right
            float scaleWidth = 1f / scaleHeight;
            _cam.rect = new Rect((1f - scaleWidth) * 0.5f, 0f, scaleWidth, 1f);
        }
    }
}