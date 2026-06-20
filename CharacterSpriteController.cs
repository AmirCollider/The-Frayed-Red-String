// ==========================================
// CharacterSpriteController - Loads and Swaps Character Sprite by State and Position
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using UnityEngine;

public class CharacterSpriteController : MonoBehaviour
{
    // ==========================================
    // Inspector — Identity
    // ==========================================
    [Header("Character Identity")]
    [SerializeField] private string characterId = "";

    // ==========================================
    // Inspector — State Sprite Map (8 entries per character)
    // ==========================================
    [Header("State Sprite Map")]
    [SerializeField] private CharacterStateSpriteEntry[] stateSpriteMap;

    // ==========================================
    // Inspector — World-Space Position Anchors
    // ==========================================
    [Header("Position Anchors (World Space)")]
    [SerializeField] private Vector3 positionLeft = new Vector3(-4.5f, -1.5f, 0f);
    [SerializeField] private Vector3 positionCenter = new Vector3(0f, -1.5f, 0f);
    [SerializeField] private Vector3 positionRight = new Vector3(4.5f, -1.5f, 0f);
    [SerializeField] private Vector3 positionOffScreen = new Vector3(-14f, -1.5f, 0f);
    [SerializeField] private Vector3 positionOffScreenRight = new Vector3(14f, -1.5f, 0f);

    // ==========================================
    // Inspector — Slide Animation
    // ==========================================
    [Header("Slide Animation")]
    [SerializeField] private float slideInDuration = 0.22f;

    // ==========================================
    // Inspector — Speaker Focus (active-speaker spotlight / listener dim)
    // ==========================================
    [Header("Speaker Focus")]
    [SerializeField] private float focusLerpDuration = 0.18f;
    [SerializeField] private float activeSpeakerScale = 1.06f;
    [SerializeField] private float backgroundScale = 0.92f;
    [SerializeField] private Color activeSpeakerTint = Color.white;
    [SerializeField] private Color backgroundTint = new Color(0.55f, 0.55f, 0.62f, 1f);

    // ==========================================
    // Inspector — Transition Animator Reference
    // ==========================================
    [Header("Transition Animator")]
    [SerializeField] private SpriteTransitionAnimator transitionAnimator;

    // ==========================================
    // Private State
    // ==========================================
    private CharacterState _currentState = CharacterState.WaitingToTalk;
    private CharacterPosition _currentPosition = CharacterPosition.OffScreen;
    private Coroutine _slideCo;
    private Coroutine _focusCo;
    private Vector3 _baseScale = Vector3.one;
    private SpeakerFocusRole _focusRole = SpeakerFocusRole.Neutral;

    // ==========================================
    // Awake - Place at OffScreen Anchor
    // ==========================================
    private void Awake()
    {
        transform.position = positionOffScreen;
        _baseScale = transform.localScale;
    }

    // ==========================================
    // Start - Apply Initial Sprite to Animator
    // ==========================================
    private void Start()
    {
        Sprite initial = GetSpriteForState(_currentState);
        if (initial != null)
            transitionAnimator?.SetInstant(initial);
    }

    // ==========================================
    // SetState - Swap to New Emotional State with Optional Crossfade
    // ==========================================
    public void SetState(CharacterState state, bool instant = false)
    {
        if (_currentState == state) return;
        _currentState = state;

        Sprite target = GetSpriteForState(state);
        if (target == null) return;

        if (instant || transitionAnimator == null)
            transitionAnimator?.SetInstant(target);
        else
            transitionAnimator.CrossfadeTo(target);
    }

    // ==========================================
    // SetStateByName - Parse String to CharacterState and Apply (for DialogueLine.characterStateOverride)
    // ==========================================
    public void SetStateByName(string stateName, bool instant = false)
    {
        if (string.IsNullOrEmpty(stateName)) return;
        if (System.Enum.TryParse(stateName, true, out CharacterState parsed))
            SetState(parsed, instant);
    }

    // ==========================================
    // SetPosition - Move Character to Screen Anchor with Optional Slide
    // ==========================================
    public void SetPosition(CharacterPosition position, bool instant = false)
    {
        // ==========================================
        // تشخیص جهت خروج: کاراکتر به سمت لبه‌ی خودش خارج می‌شود
        // تا بازیگری که در سمت راست است، هرگز از جلوی کاراکتر دیگر رد نشود.
        // ==========================================
        Vector3 target = (position == CharacterPosition.OffScreen)
            ? GetOffScreenExit(_currentPosition)
            : GetWorldPosition(position);

        // ==========================================
        // ورود از خارج از صفحه: کاراکتر ابتدا فوراً به لنگر خارج از صفحه
        // در همان سمتِ مقصد منتقل می‌شود تا از نزدیک‌ترین لبه به داخل سر بخورد.
        // ==========================================
        if (!instant
            && _currentPosition == CharacterPosition.OffScreen
            && position != CharacterPosition.OffScreen)
        {
            if (_slideCo != null) { StopCoroutine(_slideCo); _slideCo = null; }
            transform.position = GetOffScreenExit(position);
        }

        _currentPosition = position;

        if (instant)
        {
            if (_slideCo != null) { StopCoroutine(_slideCo); _slideCo = null; }
            transform.position = target;
            return;
        }

        if (_slideCo != null) StopCoroutine(_slideCo);
        _slideCo = StartCoroutine(SlideToPosition(target));
    }

    // ==========================================
    // Show - Set State and Slide Into Target Position
    // ==========================================
    public void Show(CharacterPosition position, CharacterState state, bool instant = false)
    {
        SetState(state, true);
        SetPosition(position, instant);
    }

    // ==========================================
    // Hide - Slide Off to OffScreen Anchor
    // ==========================================
    public void Hide(bool instant = false)
    {
        SetPosition(CharacterPosition.OffScreen, instant);
    }

    // ==========================================
    // SlideToPosition - SmoothStep Tween to World-Space Target
    // ==========================================
    private IEnumerator SlideToPosition(Vector3 target)
    {
        Vector3 from = transform.position;
        float elapsed = 0f;

        while (elapsed < slideInDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / slideInDuration);
            transform.position = Vector3.Lerp(from, target, t);
            yield return null;
        }

        transform.position = target;
        _slideCo = null;
    }

    // ==========================================
    // GetSpriteForState - Linear Search Through Sprite Map
    // ==========================================
    private Sprite GetSpriteForState(CharacterState state)
    {
        if (stateSpriteMap == null) return null;
        foreach (CharacterStateSpriteEntry entry in stateSpriteMap)
            if (entry.state == state) return entry.sprite;
        return null;
    }

    // ==========================================
    // GetWorldPosition - Map CharacterPosition Enum to World Vector3
    // ==========================================
    private Vector3 GetWorldPosition(CharacterPosition position)
    {
        switch (position)
        {
            case CharacterPosition.Left: return positionLeft;
            case CharacterPosition.Center: return positionCenter;
            case CharacterPosition.Right: return positionRight;
            case CharacterPosition.OffScreen: return positionOffScreen;
            default: return positionOffScreen;
        }
    }

    // ==========================================
    // GetOffScreenExit - انتخاب لنگر خارج از صفحه بر اساس سمت خود کاراکتر
    // (سمت راست از لبه راست وارد/خارج می‌شود؛ چپ و وسط به صورت پیش‌فرض از چپ)
    // ==========================================
    private Vector3 GetOffScreenExit(CharacterPosition side)
    {
        return side == CharacterPosition.Right ? positionOffScreenRight : positionOffScreen;
    }

    // ==========================================
    // SetFocusRole - Apply Speaker Spotlight / Listener Dim / Monologue Self / Neutral
    // ==========================================
    public void SetFocusRole(SpeakerFocusRole role)
    {
        _focusRole = role;

        float spk = activeSpeakerScale > 0f ? activeSpeakerScale : 1.06f;
        float bg = backgroundScale > 0f ? backgroundScale : 0.92f;
        Color spkTint = activeSpeakerTint.a > 0f ? activeSpeakerTint : Color.white;
        Color bgTint = backgroundTint.a > 0f ? backgroundTint : new Color(0.55f, 0.55f, 0.62f, 1f);

        float targetScale;
        Color targetTint;
        switch (role)
        {
            // The thinker stays bright and emphasized (so it is unmistakably them);
            // "this is a private thought" is carried by the centered italic overlay and
            // its named attribution, NOT by dimming the speaker.
            case SpeakerFocusRole.ActiveSpeaker: targetScale = spk; targetTint = spkTint; break;
            case SpeakerFocusRole.InnerMonologueSelf: targetScale = spk; targetTint = spkTint; break;
            case SpeakerFocusRole.Background: targetScale = bg; targetTint = bgTint; break;
            default: targetScale = 1f; targetTint = Color.white; break;
        }

        float dur = focusLerpDuration > 0f ? focusLerpDuration : 0.18f;
        transitionAnimator?.SetFocusTint(targetTint, dur);

        if (_focusCo != null) StopCoroutine(_focusCo);
        Vector3 to = _baseScale * targetScale;
        if (!gameObject.activeInHierarchy)
            transform.localScale = to;
        else
            _focusCo = StartCoroutine(FocusScaleRoutine(to, dur));
    }

    // ==========================================
    // FocusScaleRoutine - SmoothStep localScale Toward the Focus Target
    // ==========================================
    private IEnumerator FocusScaleRoutine(Vector3 target, float duration)
    {
        Vector3 from = transform.localScale;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(from, target, Mathf.SmoothStep(0f, 1f, t / duration));
            yield return null;
        }
        transform.localScale = target;
        _focusCo = null;
    }

    // ==========================================
    // Accessors
    // ==========================================
    public string CharacterId => characterId;
    public CharacterState CurrentState => _currentState;
    public CharacterPosition CurrentPosition => _currentPosition;
}

[System.Serializable]
public class CharacterStateSpriteEntry
{
    public CharacterState state;
    public Sprite sprite;
}

// ==========================================
// SpeakerFocusRole - Per-Character Spotlight Role Driven by the Dialogue System
// ==========================================
public enum SpeakerFocusRole
{
    Neutral = 0,
    ActiveSpeaker = 1,
    Background = 2,
    InnerMonologueSelf = 3
}