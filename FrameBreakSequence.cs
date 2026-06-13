// ==========================================
// FrameBreakSequence - Act 3 Haru Overflow and Photorealistic Reveal Trigger
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using UnityEngine;

public class FrameBreakSequence : MonoBehaviour
{
    // ==========================================
    // Inspector - Frame System (falls back to FrameController.Instance if null)
    // ==========================================
    [Header("Frame Controller")]
    [SerializeField] private FrameController frameController;

    // ==========================================
    // Inspector - Haru World-Space Overflow (wire when Phase 3 is complete)
    // ==========================================
    [Header("Haru Transform Override")]
    [SerializeField] private Transform haruTransform;
    [SerializeField] private Vector3 haruBreakWorldPosition;
    [SerializeField] private float haruMoveToBreakDuration = 0.35f;

    // ==========================================
    // Inspector - Photorealistic Switch (falls back to PhotorealisticSwitch.Instance if null)
    // ==========================================
    [Header("Photorealistic Switch")]
    [SerializeField] private PhotorealisticSwitch photoSwitch;

    // ==========================================
    // Inspector - Timing
    // ==========================================
    [Header("Timing")]
    [SerializeField] private float delayBeforePhotoSwap = 0.5f;

    // ==========================================
    // Private State
    // ==========================================
    private Coroutine _breakCo;

    // ==========================================
    // TriggerBreak - External Entry Point Called by Act3Manager
    // ==========================================
    public void TriggerBreak()
    {
        if (_breakCo != null) StopCoroutine(_breakCo);
        _breakCo = StartCoroutine(BreakRoutine());
    }

    // ==========================================
    // BreakRoutine - Move Haru to Overflow → Delay → Photorealistic Reveal
    // ==========================================
    private IEnumerator BreakRoutine()
    {
        // ==========================================
        // Step 1: Slide Haru to world-space overflow position
        // ==========================================
        if (haruTransform != null)
        {
            Vector3 startPos = haruTransform.position;
            float elapsed = 0f;

            while (elapsed < haruMoveToBreakDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, elapsed / haruMoveToBreakDuration);
                haruTransform.position = Vector3.Lerp(startPos, haruBreakWorldPosition, t);
                yield return null;
            }

            haruTransform.position = haruBreakWorldPosition;
        }

        // ==========================================
        // Step 2: Hold beat before photorealistic reveal
        // ==========================================
        yield return new WaitForSeconds(delayBeforePhotoSwap);

        // ==========================================
        // Step 3: Activate photorealistic image above frame
        // ==========================================
        PhotorealisticSwitch ps = photoSwitch != null ? photoSwitch : PhotorealisticSwitch.Instance;
        if (ps != null) ps.SwapToPhotorealistic();

        // ==========================================
        // Step 4: Set frame to PhotorealisticBreak state
        // ==========================================
        FrameController fc = frameController != null ? frameController : FrameController.Instance;
        if (fc != null) fc.SetState(FrameState.PhotorealisticBreak);

        _breakCo = null;
    }
}