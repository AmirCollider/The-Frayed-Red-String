// ==========================================
// Plus18WarningController - Idle Wobble Badge (Float + Rotation + Click Reaction)
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Plus18WarningController : MonoBehaviour, IPointerClickHandler
{
    // ==========================================
    // Inspector - Idle Float
    // ==========================================
    [Header("Idle Float")]
    [SerializeField] private float floatAmplitude = 6f;
    [SerializeField] private float floatSpeed = 0.8f;

    // ==========================================
    // Inspector - Idle Rotation
    // ==========================================
    [Header("Idle Rotation")]
    [SerializeField] private float maxRotationDegrees = 7f;
    [SerializeField] private float rotationNoiseSpeed = 0.55f;

    // ==========================================
    // Inspector - Click Wobble Reaction
    // ==========================================
    [Header("Click Wobble")]
    [SerializeField] private float clickWobbleDuration = 0.4f;
    [SerializeField] private float clickWobbleAngle = 12f;
    [SerializeField] private float clickWobbleFrequency = 18f;
    [SerializeField] private float clickPunchScale = 0.12f;

    // ==========================================
    // Private State
    // ==========================================
    private RectTransform _rectTransform;
    private Vector2 _originAnchorPos;
    private Vector3 _originScale;
    private float _noiseX;
    private float _noiseY;
    private float _noiseRot;
    private float _clickWobble;   // additive rotation from the click reaction
    private float _clickScale;    // additive uniform scale from the click reaction
    private bool _wobbleActive;

    // ==========================================
    // Awake - Cache RectTransform
    // ==========================================
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // ==========================================
    // Start - Seed Noise Offsets and Launch Idle Loop
    // ==========================================
    private void Start()
    {
        _originAnchorPos = _rectTransform.anchoredPosition;
        _originScale = _rectTransform.localScale;
        _noiseX = Random.Range(0f, 1000f);
        _noiseY = Random.Range(500f, 1500f);
        _noiseRot = Random.Range(1000f, 2000f);

        StartCoroutine(IdleRoutine());
    }

    // ==========================================
    // IdleRoutine - Perlin Noise Float and Rotation, Plus Active Click Reaction
    // ==========================================
    private IEnumerator IdleRoutine()
    {
        while (true)
        {
            float t = Time.time * floatSpeed;
            float nx = (Mathf.PerlinNoise(_noiseX + t, 0f) * 2f - 1f) * floatAmplitude;
            float ny = (Mathf.PerlinNoise(0f, _noiseY + t) * 2f - 1f) * floatAmplitude;
            float nr = (Mathf.PerlinNoise(_noiseRot + t * rotationNoiseSpeed, 0f) * 2f - 1f) * maxRotationDegrees;

            _rectTransform.anchoredPosition = _originAnchorPos + new Vector2(nx, ny);
            _rectTransform.localRotation = Quaternion.Euler(0f, 0f, nr + _clickWobble);
            _rectTransform.localScale = _originScale * (1f + _clickScale);

            yield return null;
        }
    }

    // ==========================================
    // OnPointerClick - Trigger Wobble Reaction on Click
    // ==========================================
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_wobbleActive)
            StartCoroutine(ClickWobbleRoutine());
    }

    // ==========================================
    // ClickWobbleRoutine - Damped Oscillating Tilt and Scale Punch
    // ==========================================
    private IEnumerator ClickWobbleRoutine()
    {
        _wobbleActive = true;
        float elapsed = 0f;

        while (elapsed < clickWobbleDuration)
        {
            elapsed += Time.deltaTime;
            float norm = elapsed / clickWobbleDuration;
            float damping = 1f - norm;

            _clickWobble = Mathf.Sin(norm * clickWobbleFrequency) * clickWobbleAngle * damping;
            _clickScale = Mathf.Sin(norm * Mathf.PI) * clickPunchScale;

            yield return null;
        }

        _clickWobble = 0f;
        _clickScale = 0f;
        _wobbleActive = false;
    }
}