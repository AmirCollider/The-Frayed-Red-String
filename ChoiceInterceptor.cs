// ==========================================
// ChoiceInterceptor - Blue/Green/White Deception Router (GDD §1.3, R4)
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using UnityEngine;

public class ChoiceInterceptor : MonoBehaviour
{
    // ==========================================
    // Inspector — Manipulator Identity (the character whose sprite reacts)
    // ==========================================
    [Header("Manipulator")]
    [SerializeField] private string manipulatorSpeakerId = ConstantsConfig.SPEAKER_YUA;

    // ==========================================
    // Inspector — Glitch Flash (sub-0.2s manipulator sprite blip)
    //   Green flashes InsaneSmile, Blue flashes Pokerface — both held under
    //   0.2 seconds, like one corrupted frame, before the forced route plays.
    // ==========================================
    [Header("Glitch Flash (sub-0.2s)")]
    [SerializeField] private float glitchFlashDuration = 0.15f;

    // ==========================================
    // Inspector — Logging
    // ==========================================
    [Header("Logging")]
    [SerializeField] private bool recordForcedOutcome = true;
    [SerializeField] private string forcedOutcomeKeySuffix = "_forced";

    // ==========================================
    // Resolve - Apply Color Side-Effects, Return the Sequence That Will Actually Play
    //   Green : true manipulative route + InsaneSmile flash
    //   Blue  : empathetic decoy — Pokerface swap, empathetic branch suppressed,
    //           forced onto the Green/forced route anyway
    //   White : cosmetic, zero logic drift
    // ==========================================
    public DialogueSequence Resolve(string branchId, int chosenIndex, DialogueChoice choice)
    {
        if (choice == null) return null;

        switch (choice.color)

        {
            case ChoiceColor.Green:
                FlashGlitch(CharacterState.InsaneSmile);
                LogForced(branchId, chosenIndex);
                return PickRoute(choice);
            case ChoiceColor.Blue:
                FlashGlitch(CharacterState.Pokerface);
                LogForced(branchId, chosenIndex);
                return PickRoute(choice);
            default:
                return choice.consequenceSequence;
        }
    }

    // ==========================================
    // PickRoute - Forced Route Takes Priority, Else the Choice's Own Consequence
    // ==========================================
    private DialogueSequence PickRoute(DialogueChoice choice)
        => choice.forcedRouteSequence != null ? choice.forcedRouteSequence : choice.consequenceSequence;
    // ==========================================
    // LogForced - Persist the True (Forced) Outcome to BranchRecord
    // ==========================================
    private void LogForced(string branchId, int chosenIndex)
    {
        if (!recordForcedOutcome || BranchRecord.Instance == null || string.IsNullOrEmpty(branchId)) return;
        BranchRecord.Instance.Record(branchId + forcedOutcomeKeySuffix, chosenIndex);
    }
    // ==========================================
    // FlashGlitch - Sub-0.2s Manipulator Sprite Blip
    //   Green = InsaneSmile, Blue = Pokerface. Both colors glitch the girl for
    //   a single beat, then the forced (green) route proceeds.
    // ==========================================
    private void FlashGlitch(CharacterState flashState)
    {
        CharacterSpriteController c = CharacterRegistry.Instance?.Get(manipulatorSpeakerId);
        if (c == null) return;
        StartCoroutine(FlashRoutine(c, flashState, glitchFlashDuration));
    }
    // ==========================================
    // FlashRoutine - Instant Swap to Glitch State, Hold < 0.2s, Instant Restore
    //   Instant in + instant out (no crossfade) so it reads as a hard glitch.
    // ==========================================
    private IEnumerator FlashRoutine(CharacterSpriteController c, CharacterState flashState, float hold)
    {
        CharacterState previous = c.CurrentState;
        if (previous == flashState) yield break;
        c.SetState(flashState, true);
        yield return new WaitForSeconds(hold);
        c.SetState(previous, true);
    }
}