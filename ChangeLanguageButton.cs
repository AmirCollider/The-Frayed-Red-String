// ==========================================
// ChangeLanguageButton - Circular Flag Toggle with Idle Animation
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class ChangeLanguageButton : MonoBehaviour
{
    // ==========================================
    // Inspector References
    // ==========================================
    [Header("Flag Sprites")]
    [SerializeField] private Sprite ukFlagSprite;
    [SerializeField] private Sprite japanFlagSprite;

    [Header("Flag Display Image (child of this button)")]
    [SerializeField] private Image flagDisplayImage;

    [Header("Idle Animation")]
    [SerializeField] private float bobAmplitude = 5f;
    [SerializeField] private float bobFrequency = 1.1f;
    [SerializeField] private float scalePulseRange = 0.035f;
    [SerializeField] private float idleDriftX = 3f;
    [SerializeField] private float idleRotationAmp = 7f;
    [SerializeField] private float idleRotationFreq = 0.65f;

    // ==========================================
    // Private State
    // ==========================================
    private Button _button;
    private bool _isEnglish = true;
    private Vector3 _originPos;
    private Vector3 _originScale;

    // ==========================================
    // Awake - Get Components & Wire Click
    // ==========================================
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnToggleClicked);

        // Ensure Mask component exists for circular clipping
        if (GetComponent<Mask>() == null)
            gameObject.AddComponent<Mask>().showMaskGraphic = true;
    }

    // ==========================================
    // Start - Sync State with GameManager & Begin Idle
    // ==========================================
    private void Start()
    {
        _originPos = transform.localPosition;
        _originScale = transform.localScale;

        if (GameManager.Instance != null)
            _isEnglish = GameManager.Instance.CurrentLanguage == ConstantsConfig.LANG_ENGLISH;

        ApplyFlag();
        StartCoroutine(IdleAnimationRoutine());
    }

    // ==========================================
    // OnToggleClicked - Switch Language & Play Flip
    // ==========================================
    private void OnToggleClicked()
    {
        _isEnglish = !_isEnglish;

        string newLang = _isEnglish ? ConstantsConfig.LANG_ENGLISH : ConstantsConfig.LANG_JAPANESE;
        if (GameManager.Instance != null)
            GameManager.Instance.SetLanguage(newLang);

        StopAllCoroutines();
        StartCoroutine(FlipRoutine());
    }

    // ==========================================
    // ApplyFlag - Set Sprite Based on Current Language
    // ==========================================
    private void ApplyFlag()
    {
        if (flagDisplayImage == null) return;
        flagDisplayImage.sprite = _isEnglish ? ukFlagSprite : japanFlagSprite;
    }

    // ==========================================
    // IdleAnimationRoutine - Organic Bob, Horizontal Drift, Tilt Rotation, Scale Pulse
    // ==========================================
    private IEnumerator IdleAnimationRoutine()
    {
        float time = 0f;

        while (true)
        {
            time += Time.deltaTime;

            float yOffset = Mathf.Sin(time * bobFrequency * Mathf.PI * 2f) * bobAmplitude;
            float xOffset = Mathf.Cos(time * bobFrequency * 0.53f * Mathf.PI * 2f) * idleDriftX;
            float scalePulse = 1f + Mathf.Sin(time * bobFrequency * Mathf.PI * 2f * 0.6f) * scalePulseRange;
            float zRotation = Mathf.Sin(time * idleRotationFreq * Mathf.PI * 2f) * idleRotationAmp;

            transform.localPosition = _originPos + new Vector3(xOffset, yOffset, 0f);
            transform.localScale = _originScale * scalePulse;
            transform.localRotation = Quaternion.Euler(0f, 0f, zRotation);
            yield return null;
        }
    }

    // ==========================================
    // FlipRoutine - X-Scale Flip on Language Toggle
    // ==========================================
    private IEnumerator FlipRoutine()
    {
        float halfDuration = 0.1f;
        float elapsed = 0f;

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            transform.localScale = new Vector3(_originScale.x * (1f - t), _originScale.y, _originScale.z);
            yield return null;
        }

        ApplyFlag();
        elapsed = 0f;

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            transform.localScale = new Vector3(_originScale.x * t, _originScale.y, _originScale.z);
            yield return null;
        }

        transform.localScale = _originScale;
        StartCoroutine(IdleAnimationRoutine());
    }
}