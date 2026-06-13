// ==========================================
// AudioEvent - Decoupled SFX ScriptableObject
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioEvent", menuName = "The Frayed Red String/Audio Event")]
public class AudioEvent : ScriptableObject
{
    // ==========================================
    // Inspector Fields
    // ==========================================
    [Header("Clip")]
    [SerializeField] private AudioClip clip;

    [Header("Playback")]
    [SerializeField][Range(0f, 1f)] private float volume = 1f;
    [SerializeField][Range(0.5f, 2f)] private float pitch = 1f;
    [SerializeField][Range(0f, 0.5f)] private float pitchVariance = 0f;

    // ==========================================
    // Play - Fire One-Shot via AudioManager
    // ==========================================
    public void Play()
    {
        if (AudioManager.Instance == null || clip == null) return;
        AudioManager.Instance.PlaySFX(clip);
    }

    // ==========================================
    // Properties - Read Access for External Systems
    // ==========================================
    public AudioClip Clip => clip;
    public float Volume => volume;
    public float Pitch => pitch;
    public float PitchVariance => pitchVariance;
}