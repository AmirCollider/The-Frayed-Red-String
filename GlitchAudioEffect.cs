// ==========================================
// GlitchAudioEffect - Audio Distortion and Stutter Coroutine for Act 3
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using UnityEngine;

public class GlitchAudioEffect : MonoBehaviour
{
    // ==========================================
    // Inspector Fields
    // ==========================================
    [Header("Target Source (assign AudioManager's bgmSource child)")]
    [SerializeField] private AudioSource targetSource;

    [Header("Stutter")]
    [SerializeField] private float totalGlitchDuration = 2.5f;
    [SerializeField] private float minStutterPause = 0.04f;
    [SerializeField] private float maxStutterPause = 0.18f;
    [SerializeField] private float silenceDuration = 0.06f;

    [Header("Pitch Warp")]
    [SerializeField] private float pitchMin = 0.55f;
    [SerializeField] private float pitchMax = 1.45f;

    [Header("Volume Floor During Stutter")]
    [SerializeField][Range(0f, 0.5f)] private float volumeFloor = 0.08f;

    // ==========================================
    // Private State
    // ==========================================
    private float _originalPitch;
    private float _originalVolume;
    private Coroutine _glitchCo;

    // ==========================================
    // Awake - Cache Default Playback State
    // ==========================================
    private void Awake()
    {
        if (targetSource == null) return;
        _originalPitch = targetSource.pitch;
        _originalVolume = targetSource.volume;
    }

    // ==========================================
    // TriggerGlitch - Begin Distortion Sequence
    // ==========================================
    public void TriggerGlitch()
    {
        if (targetSource == null) return;
        if (_glitchCo != null) StopCoroutine(_glitchCo);
        _glitchCo = StartCoroutine(GlitchRoutine());
    }

    // ==========================================
    // StopGlitch - Halt Coroutine and Restore Audio
    // ==========================================
    public void StopGlitch()
    {
        if (_glitchCo != null)
        {
            StopCoroutine(_glitchCo);
            _glitchCo = null;
        }
        Restore();
    }

    // ==========================================
    // GlitchRoutine - Stutter and Pitch Scramble Over totalGlitchDuration
    // ==========================================
    private IEnumerator GlitchRoutine()
    {
        float elapsed = 0f;

        while (elapsed < totalGlitchDuration)
        {
            float wait = Random.Range(minStutterPause, maxStutterPause);
            yield return new WaitForSeconds(wait);
            elapsed += wait;

            // Volume dip
            targetSource.volume = volumeFloor;
            yield return new WaitForSeconds(silenceDuration);
            targetSource.volume = _originalVolume;
            elapsed += silenceDuration;

            // Pitch scramble
            targetSource.pitch = Random.Range(pitchMin, pitchMax);
        }

        Restore();
        _glitchCo = null;
    }

    // ==========================================
    // Restore - Reset AudioSource to Pre-Glitch State
    // ==========================================
    private void Restore()
    {
        if (targetSource == null) return;
        targetSource.pitch = _originalPitch;
        targetSource.volume = _originalVolume;
    }
}