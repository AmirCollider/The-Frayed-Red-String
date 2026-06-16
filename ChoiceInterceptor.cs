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
    // Inspector — Green Assertive Flash (sub-second InsaneSmile)
    // ==========================================
    [Header("Green — Assertive Flash")]
    [SerializeField] private float insaneSmileFlashDuration = 0.45f;

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
                FlashInsaneSmile();
                LogForced(branchId, chosenIndex);
                return PickRoute(choice);

            case ChoiceColor.Blue:
                ForcePokerface();
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
    // FlashInsaneSmile - Sub-Second Manipulator Sprite Flash (Green)
    // ==========================================
    private void FlashInsaneSmile()
    {
        CharacterSpriteController c = CharacterRegistry.Instance?.Get(manipulatorSpeakerId);
        if (c == null) return;
        StartCoroutine(FlashRoutine(c, CharacterState.InsaneSmile, insaneSmileFlashDuration));
    }

    // ==========================================
    // ForcePokerface - Instant Manipulator Pokerface Swap (Blue Deception)
    // ==========================================
    private void ForcePokerface()
    {
        CharacterRegistry.Instance?.Get(manipulatorSpeakerId)?.SetState(CharacterState.Pokerface, true);
    }

    // ==========================================
    // FlashRoutine - Swap to State, Hold, Restore Previous State
    // ==========================================
    private IEnumerator FlashRoutine(CharacterSpriteController c, CharacterState flashState, float hold)
    {
        CharacterState previous = c.CurrentState;
        c.SetState(flashState, true);
        yield return new WaitForSeconds(hold);
        c.SetState(previous, false);
    }
}