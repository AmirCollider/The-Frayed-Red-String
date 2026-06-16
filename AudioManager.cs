// ==========================================
// AudioManager - BGM / SFX / Ambience Controller
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // ==========================================
    // Singleton Instance
    // ==========================================
    public static AudioManager Instance { get; private set; }

    // ==========================================
    // Inspector References
    // ==========================================
    [Header("Audio Mixer (assign GameAudioMixer asset)")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambienceSource;

    [Header("BGM Track Library (map BGMTrack enum to AudioClip)")]
    [SerializeField] private BGMTrackEntry[] bgmLibrary;

    // ==========================================
    // Awake - Resolve Persistent BGM Source and Cache Default Playback State
    // ==========================================
    private void Awake()
    {
        ResolveTargetSource();
        if (targetSource == null) return;
        _originalPitch = targetSource.pitch;
        _originalVolume = targetSource.volume;
    }

    // ==========================================
    // ResolveTargetSource - Fall Back to Persistent AudioManager BGM Source
    // ==========================================
    private void ResolveTargetSource()
    {
        if (targetSource != null) return;
        if (AudioManager.Instance != null)
            targetSource = AudioManager.Instance.BgmSource;
    }

    // ==========================================
    // LoadVolumeSettings - Apply Saved Volumes on Start
    // ==========================================
    private void LoadVolumeSettings()
    {
        SetMasterVolume(PlayerPrefs.GetFloat(ConstantsConfig.PREF_MASTER_VOLUME, 1f));
        SetBGMVolume(PlayerPrefs.GetFloat(ConstantsConfig.PREF_BGM_VOLUME, 1f));
        SetSFXVolume(PlayerPrefs.GetFloat(ConstantsConfig.PREF_SFX_VOLUME, 1f));
    }

    // ==========================================
    // PlayBGM - Crossfade to New BGM Track
    // ==========================================
    public void PlayBGM(AudioClip clip, float fadeDuration = 0.5f)
    {
        if (clip == null) return;
        StartCoroutine(CrossfadeBGM(clip, fadeDuration));
    }
    // ==========================================
    // StopBGM - Immediate Hard Stop (used by Act3MusicStop)
    // ==========================================
    public void StopBGM()
    {
        StopAllCoroutines();
        if (bgmSource != null) bgmSource.Stop();

    }
    // ==========================================
    // FadeOutBGM - Smoothly Fade BGM Volume to Zero and Stop (used by FourthWallBreaker)
    // ==========================================
    public void FadeOutBGM(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutBGMRoutine(duration));
    }
    // ==========================================
    // FadeOutBGMRoutine - Volume Lerp to Zero Then Hard Stop
    // ==========================================
    private IEnumerator FadeOutBGMRoutine(float duration)
    {
        if (bgmSource == null) yield break;
        float start = bgmSource.volume;
        if (duration <= 0f)
        {
            bgmSource.Stop();
            bgmSource.volume = start;
            yield break;
        }
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(start, 0f, elapsed / duration);
            yield return null;
        }
        bgmSource.Stop();
        bgmSource.volume = start;
    }

    // ==========================================
    // PlaySFX - One-Shot Sound Effect
    // ==========================================
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }

    // ==========================================
    // PlayAmbience - Looping Ambience Track
    // ==========================================
    public void PlayAmbience(AudioClip clip)
    {
        if (clip == null || ambienceSource == null) return;
        ambienceSource.clip = clip;
        ambienceSource.Play();
    }

    // ==========================================
    // StopAmbience - Stop Ambience Track
    // ==========================================
    public void StopAmbience()
    {
        if (ambienceSource != null) ambienceSource.Stop();
    }

    // ==========================================
    // SetMasterVolume - Apply to Mixer or Source Fallback
    // ==========================================
    public void SetMasterVolume(float normalized)
    {
        if (audioMixer != null)
            audioMixer.SetFloat(ConstantsConfig.MIXER_MASTER, NormalizedToDecibel(normalized));
        PlayerPrefs.SetFloat(ConstantsConfig.PREF_MASTER_VOLUME, normalized);
    }

    // ==========================================
    // SetBGMVolume - Apply to Mixer or Source Fallback
    // ==========================================
    public void SetBGMVolume(float normalized)
    {
        if (audioMixer != null)
            audioMixer.SetFloat(ConstantsConfig.MIXER_BGM, NormalizedToDecibel(normalized));
        else if (bgmSource != null)
            bgmSource.volume = normalized;
        PlayerPrefs.SetFloat(ConstantsConfig.PREF_BGM_VOLUME, normalized);
    }

    // ==========================================
    // SetSFXVolume - Apply to Mixer or Source Fallback
    // ==========================================
    public void SetSFXVolume(float normalized)
    {
        if (audioMixer != null)
            audioMixer.SetFloat(ConstantsConfig.MIXER_SFX, NormalizedToDecibel(normalized));
        else if (sfxSource != null)
            sfxSource.volume = normalized;
        PlayerPrefs.SetFloat(ConstantsConfig.PREF_SFX_VOLUME, normalized);
    }

    // ==========================================
    // CrossfadeBGM - Smooth BGM Transition Coroutine
    // ==========================================
    private IEnumerator CrossfadeBGM(AudioClip newClip, float duration)
    {
        float half = duration * 0.5f;
        float elapsed = 0f;
        float startVolume = bgmSource != null ? bgmSource.volume : 1f;

        while (elapsed < half)
        {
            elapsed += Time.deltaTime;
            if (bgmSource != null)
                bgmSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / half);
            yield return null;
        }

        if (bgmSource != null)
        {
            bgmSource.Stop();
            bgmSource.clip = newClip;
            bgmSource.volume = 0f;
            bgmSource.Play();
        }

        elapsed = 0f;

        while (elapsed < half)
        {
            elapsed += Time.deltaTime;
            if (bgmSource != null)
                bgmSource.volume = Mathf.Lerp(0f, 1f, elapsed / half);
            yield return null;
        }

        if (bgmSource != null)
            bgmSource.volume = 1f;
    }

    // ==========================================
    // NormalizedToDecibel - Linear to dB Conversion
    // ==========================================
    private float NormalizedToDecibel(float value)
    {
        return value > 0.0001f ? Mathf.Log10(value) * 20f : -80f;
    }

    // ==========================================
    // PlayBGM (BGMTrack Overload) - Enum Lookup with MusicStateLock Gate
    // ==========================================
    public void PlayBGM(BGMTrack track, float fadeDuration = 0.5f)
    {
        if (MusicStateLock.Instance != null && !MusicStateLock.Instance.IsAllowed(track))
        {
            Debug.LogWarning($"[AudioManager] BGMTrack.{track} is gated by MusicStateLock.");
            return;
        }

        AudioClip clip = GetBGMClip(track);
        if (clip == null)
        {
            Debug.LogWarning($"[AudioManager] No clip mapped for BGMTrack.{track}.");
            return;
        }

        PlayBGM(clip, fadeDuration);
    }

    // ==========================================
    // GetBGMClip - Linear Search Through bgmLibrary Array
    // ==========================================
    private AudioClip GetBGMClip(BGMTrack track)
    {
        if (bgmLibrary == null) return null;
        foreach (BGMTrackEntry entry in bgmLibrary)
            if (entry.track == track) return entry.clip;
        return null;
    }

    // ==========================================
    // BgmSource - Public Accessor for the Persistent BGM AudioSource
    // (used by GlitchAudioEffect to target the cross-scene DontDestroyOnLoad source)
    // ==========================================
    public AudioSource BgmSource => bgmSource;
}

// ==========================================
// BGMTrackEntry - Serializable Enum-to-Clip Mapping
// ==========================================
[System.Serializable]
public class BGMTrackEntry
{
    public BGMTrack track;
    public AudioClip clip;
}