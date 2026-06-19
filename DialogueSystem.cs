// ==========================================
// DialogueSystem - Visual Novel Engine Central Orchestrator
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DialogueSystem : MonoBehaviour
{
    // ==========================================
    // Singleton Instance (per-scene — no DontDestroyOnLoad)
    // ==========================================
    public static DialogueSystem Instance { get; private set; }

    // ==========================================
    // Inspector — UI References
    // ==========================================
    [Header("UI References")]
    [SerializeField] private DialogueBoxUI dialogueBox;
    [SerializeField] private InnerMonologueOverlay innerMonologue;
    [SerializeField] private ChoiceUI choiceUI;

    // ==========================================
    // Inspector — Choice Deception Router (GDD §1.3 — Blue/Green/White)
    // ==========================================
    [Header("Choice Interceptor")]
    [SerializeField] private ChoiceInterceptor choiceInterceptor;

    // ==========================================
    // Inspector — Advance SFX (UIClick AudioEvent)
    // ==========================================
    [Header("Advance SFX")]
    [SerializeField] private AudioEvent advanceSfx;

    // ==========================================
    // Events
    // ==========================================
    [Header("Events")]
    public UnityEvent OnSequenceStarted = new UnityEvent();
    public UnityEvent OnSequenceComplete = new UnityEvent();
    public UnityEvent<DialogueLine> OnLineDisplayed = new UnityEvent<DialogueLine>();
    // ==========================================
    // Extended Events — Choice Outcome and Per-Line Gameplay Hooks
    // ==========================================
    public UnityEvent<string, int, int> OnChoiceMade = new UnityEvent<string, int, int>();
    public UnityEvent<string> OnGameEventTriggered = new UnityEvent<string>();

    // ==========================================
    // Private State
    // ==========================================
    private bool _isRunning;
    private bool _waitingForInput;
    private DialogueSequence _pendingSubSequence;
    private Coroutine _sequenceCo;

    // ==========================================
    // Save/Resume Cursor — Master Sequence Position (MetaFileSystem Save Support)
    // ==========================================
    private DialogueSequence _currentSequence;
    private int _currentIndex;
    private bool _atSaveableWait;
    private bool _suppressSideEffectsOnce;

    // ==========================================
    // Awake - Singleton Enforcement
    // ==========================================
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // ==========================================
    // Update - Poll Advance Input While Waiting for Player
    // ==========================================
    private void Update()
    {
        // ==========================================
        // Pause Gate — Ignore Advance Input While Frozen (PauseMenu sets Time.timeScale = 0)
        // No reference to PauseMenuController, so this never breaks the compile.
        // ==========================================
        if (Time.timeScale == 0f) return;

        // Poll input while typing (to skip) OR while waiting (to advance).
        bool typing = (dialogueBox != null && dialogueBox.IsTyping);
 
        if (!typing && !_waitingForInput) return;

        // Modern polling using the New Input System API with null checks
        bool advance = (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                    || (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                    || (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
                    || (Keyboard.current != null && Keyboard.current.zKey.wasPressedThisFrame);

        if (advance) Advance();
    }

    // ==========================================
    // Play - Begin a Dialogue Sequence from External Caller
    // ==========================================
    public void Play(DialogueSequence sequence)
    {
        Play(sequence, 0);
    }

    // ==========================================
    // Play (Resume Overload) - Begin or Resume a Sequence at a Specific Entry Index
    // ==========================================
    public void Play(DialogueSequence sequence, int startIndex)
    {
        if (_isRunning)
        {
            Debug.LogWarning("[DialogueSystem] Play called while sequence already running. Ignored.");
            return;
        }
        if (sequence == null || sequence.Count == 0)
        {
            OnSequenceComplete.Invoke();
            return;
        }

        _currentSequence = sequence;
        _currentIndex = Mathf.Clamp(startIndex, 0, sequence.Count - 1);
        _suppressSideEffectsOnce = _currentIndex > 0;
        _isRunning = true;
        if (_sequenceCo != null) StopCoroutine(_sequenceCo);
        _sequenceCo = StartCoroutine(RunSequence(sequence, _currentIndex));
        OnSequenceStarted.Invoke();
    }

    // ==========================================
    // Stop - Immediately Halt Active Sequence and Clean Up
    // ==========================================
    public void Stop()
    {
        if (!_isRunning) return;
        if (_sequenceCo != null) { StopCoroutine(_sequenceCo); _sequenceCo = null; }
        _isRunning = false;
        _waitingForInput = false;
        _atSaveableWait = false;
        _suppressSideEffectsOnce = false;
        _currentSequence = null;
        dialogueBox?.Hide();
        innerMonologue?.Hide();
    }

    // ==========================================
    // IsRunning - Returns True While a Sequence is Active
    // ==========================================
    public bool IsRunning => _isRunning;

    // ==========================================
    // Save/Resume Accessors — Read by SaveSystem and PauseMenuController
    // ==========================================
    public DialogueSequence CurrentSequence => _currentSequence;
    public int CurrentIndex => _currentIndex;
    public bool CanSaveNow => _isRunning && !_suppressSideEffectsOnce;

    // ==========================================
    // Advance - Handle Player Input: Skip Typewriter or Proceed to Next Line
    // ==========================================
    private void Advance()
    {
        if (dialogueBox != null && dialogueBox.IsTyping)
        {
            dialogueBox.SkipTypewriter();
            return;
        }
        if (innerMonologue != null && innerMonologue.IsTyping)
        {
            innerMonologue.Skip();
            return;
        }

        advanceSfx?.Play();
        _waitingForInput = false;
    }

    // ==========================================
    // RunSequence - Coroutine: Iterate and Dispatch Each Entry
    // ==========================================
    private IEnumerator RunSequence(DialogueSequence sequence, int startIndex)
    {
        dialogueBox?.Show();

        for (int i = startIndex; i < sequence.Count; i++)
        {
            _currentIndex = i;
            DialogueEntry entry = sequence.GetEntry(i);
            if (entry == null) continue;

            if (entry.type == DialogueEntryType.Line)
            {
                yield return StartCoroutine(ProcessLine(entry.line, true));
            }
            else if (entry.type == DialogueEntryType.Branch)
            {
                _pendingSubSequence = null;
                yield return StartCoroutine(ProcessBranch(entry));

                if (_pendingSubSequence != null)
                {
                    DialogueSequence sub = _pendingSubSequence;
                    _pendingSubSequence = null;
                    yield return StartCoroutine(RunSubSequence(sub));
                }
            }
        }

        dialogueBox?.Hide();
        _isRunning = false;
        _sequenceCo = null;
        _currentSequence = null;
        OnSequenceComplete.Invoke();
    }

    // ==========================================
    // RunSubSequence - Consequence Sequence After Branch Choice (No Hide on End)
    // ==========================================
    private IEnumerator RunSubSequence(DialogueSequence sequence)
    {
        for (int i = 0; i < sequence.Count; i++)
        {
            DialogueEntry entry = sequence.GetEntry(i);
            if (entry == null) continue;

            if (entry.type == DialogueEntryType.Line)
                yield return StartCoroutine(ProcessLine(entry.line, false));
        }
    }

    // ==========================================
    // ProcessLine - Show Single Line, Wait for Typewriter, Then Wait for Input
    // ==========================================
    private IEnumerator ProcessLine(DialogueLine line, bool topLevel)
    {
        if (line == null) yield break;

        // ==========================================
        // Resume Guard — On the First Resumed Line the SaveSystem Already Restored
        // the Visual Snapshot, So Re-Apply Nothing and Re-Fire No Game Events
        // ==========================================
        bool suppress = _suppressSideEffectsOnce;
        _suppressSideEffectsOnce = false;

        if (!suppress)
        {
            // ==========================================
            // Background Change — Trigger Before Text Display
            // ==========================================
            if (line.backgroundId != BackgroundID.None && BackgroundManager.Instance != null)
                BackgroundManager.Instance.SetBackground(line.backgroundId, line.backgroundTransition);

            // ==========================================
            // Character State Override — Apply Before Line Render
            // ==========================================
            if (!string.IsNullOrEmpty(line.characterStateOverride) && CharacterRegistry.Instance != null)
                CharacterRegistry.Instance.Get(line.speakerId)?.SetStateByName(line.characterStateOverride);

            // ==========================================
            // Speaker Position — Slide / Place the Speaking Character (dialogue-driven)
            // ==========================================
            if (!string.IsNullOrEmpty(line.speakerId) && CharacterRegistry.Instance != null)
            {
                CharacterSpriteController speaker = CharacterRegistry.Instance.Get(line.speakerId);
                if (speaker != null && speaker.CurrentPosition != line.speakerPosition)
                    speaker.SetPosition(line.speakerPosition);
            }
        }

        if (line.isInnerMonologue)
        {
            innerMonologue?.Show(line.GetActiveText());
        }
        else
        {
            dialogueBox?.DisplayLine(line);
            line.voiceEvent?.Play();
            line.sfxEvent?.Play();
        }

        OnLineDisplayed.Invoke(line);

        // ==========================================
        // Gameplay Event Hook — Fire Per-Line Game Event if Set (skipped on resume)
        // ==========================================
        if (!suppress && !string.IsNullOrEmpty(line.gameEventId))
            OnGameEventTriggered.Invoke(line.gameEventId);

        // ==========================================
        // Wait Until All Active Typewriters Finish
        // ==========================================
        yield return new WaitUntil(() =>
        {
            bool boxDone = dialogueBox == null || !dialogueBox.IsTyping;
            bool overlayDone = innerMonologue == null || !innerMonologue.IsTyping;
            return boxDone && overlayDone;
        });

        dialogueBox?.SetArrow(true);

        if (line.autoAdvance)
        {
            yield return new WaitForSeconds(line.autoAdvanceDelay);
        }
        else
        {
            _atSaveableWait = topLevel && !line.isInnerMonologue && string.IsNullOrEmpty(line.gameEventId);
            _waitingForInput = true;
            yield return new WaitUntil(() => !_waitingForInput);
            _atSaveableWait = false;
        }

        dialogueBox?.SetArrow(false);

        if (line.isInnerMonologue)
            innerMonologue?.Hide();
    }

    // ==========================================
    // ProcessBranch - Present Choices and Await Player Selection
    // ==========================================
    private IEnumerator ProcessBranch(DialogueEntry entry)
    {
        bool chosen = false;

        choiceUI?.Present(entry.choices, entry.branchId, (idx) =>
        {
            if (idx >= 0 && idx < entry.choices.Count)
            {
                DialogueChoice picked = entry.choices[idx];

                // ==========================================
                // Deception Routing — Interceptor decides the real sequence (Blue→Green)
                // ==========================================
                _pendingSubSequence = choiceInterceptor != null
                    ? choiceInterceptor.Resolve(entry.branchId, idx, picked)
                    : picked.consequenceSequence;

                OnChoiceMade.Invoke(entry.branchId, idx, picked.affectionDelta);
            }
            chosen = true;
        });

        yield return new WaitUntil(() => chosen);
    }
}