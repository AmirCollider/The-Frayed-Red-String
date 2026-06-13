// ==========================================
// Act3MusicStop - Hard-Silence BGM and Ambience Trigger (No Fade)
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;

public class Act3MusicStop : MonoBehaviour
{
    // ==========================================
    // Inspector Fields
    // ==========================================
    [Header("Auto-Trigger")]
    [SerializeField] private bool fireOnStart = false;

    // ==========================================
    // Start - Optional Auto-Trigger on Scene Load
    // ==========================================
    private void Start()
    {
        if (fireOnStart)
            TriggerStop();
    }

    // ==========================================
    // TriggerStop - Instantly Kill BGM and Ambience with No Fade
    // ==========================================
    public void TriggerStop()
    {
        if (AudioManager.Instance == null) return;
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.StopAmbience();
    }
}