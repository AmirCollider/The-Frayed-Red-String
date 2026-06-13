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

    // ==========================================
    // Inspector — Slide Animation
    // ==========================================
    [Header("Slide Animation")]
    [SerializeField] private float slideInDuration = 0.22f;

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

    // ==========================================
    // Awake - Place at OffScreen Anchor
    // ==========================================
    private void Awake()
    {
        transform.position = positionOffScreen;
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
        _currentPosition = position;
        Vector3 target = GetWorldPosition(position);

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
    // Accessors
    // ==========================================
    public string CharacterId => characterId;
    public CharacterState CurrentState => _currentState;
    public CharacterPosition CurrentPosition => _currentPosition;
}

// ==========================================
// CharacterStateSpriteEntry - Serializable State-to-Sprite Binding
// ==========================================
[System.Serializable]
public class CharacterStateSpriteEntry
{
    public CharacterState state;
    public Sprite sprite;
}