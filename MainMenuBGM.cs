// ==========================================
// MainMenuBGM - Main Menu Background Music Trigger
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;

public class MainMenuBGM : MonoBehaviour
{
    // ==========================================
    // Inspector Fields
    // ==========================================
    [Header("Music")]
    [SerializeField] private AudioClip menuMusicClip;

    [Header("Fade In")]
    [SerializeField] private float fadeInDuration = 1.0f;

    // ==========================================
    // Start - Trigger BGM on Scene Load
    // ==========================================
    private void Start()
    {
        if (AudioManager.Instance == null) return;
        if (menuMusicClip == null)
        {
            Debug.LogWarning("[MainMenuBGM] menuMusicClip is not assigned.");
            return;
        }

        AudioManager.Instance.PlayBGM(menuMusicClip, fadeInDuration);
    }
}