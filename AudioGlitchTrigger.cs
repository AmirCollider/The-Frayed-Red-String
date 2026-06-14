// ==========================================
// AudioGlitchTrigger - Chains Audio Distortion into Hard Silence for Act 3
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;
using UnityEngine.Events;

public class AudioGlitchTrigger : MonoBehaviour
{
    // ==========================================
    // Inspector - Glitch Audio Effect Reference
    // ==========================================
    [Header("Glitch Audio Effect")]
    [SerializeField] private GlitchAudioEffect glitchEffect;

    // ==========================================
    // Inspector - Hard Stop Reference
    // ==========================================
    [Header("Hard Stop")]
    [SerializeField] private Act3MusicStop musicStop;

    // ==========================================
    // Events
    // ==========================================
    [Header("Events")]
    public UnityEvent OnSequenceComplete = new UnityEvent();

    // ==========================================
    // Awake - Subscribe to Glitch Completion
    // ==========================================
    private void Awake()
    {
        if (glitchEffect != null)
            glitchEffect.OnGlitchComplete.AddListener(OnGlitchFinished);
    }

    // ==========================================
    // OnDestroy - Unsubscribe from Glitch Completion
    // ==========================================
    private void OnDestroy()
    {
        if (glitchEffect != null)
            glitchEffect.OnGlitchComplete.RemoveListener(OnGlitchFinished);
    }

    // ==========================================
    // TriggerSequence - External Entry Point Called by Act3Manager
    // ==========================================
    public void TriggerSequence()
    {
        if (glitchEffect != null)
            glitchEffect.TriggerGlitch();
        else
            HardStop();
    }

    // ==========================================
    // OnGlitchFinished - Apply Hard Silence Once Distortion Completes
    // ==========================================
    private void OnGlitchFinished()
    {
        HardStop();
    }

    // ==========================================
    // HardStop - Trigger Act3MusicStop or Fallback Directly to AudioManager
    // ==========================================
    private void HardStop()
    {
        if (musicStop != null)
            musicStop.TriggerStop();
        else if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopBGM();
            AudioManager.Instance.StopAmbience();
        }

        OnSequenceComplete.Invoke();
    }
}