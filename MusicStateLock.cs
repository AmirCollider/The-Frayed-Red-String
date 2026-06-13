// ==========================================
// MusicStateLock - End Credits Theme Gate Until Act 5 End Event
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;

public class MusicStateLock : MonoBehaviour
{
    // ==========================================
    // Singleton Instance
    // ==========================================
    public static MusicStateLock Instance { get; private set; }

    // ==========================================
    // PlayerPrefs Key
    // ==========================================
    private const string PREF_END_CREDITS_UNLOCKED = "EndCreditsUnlocked";

    // ==========================================
    // State
    // ==========================================
    public bool EndCreditsUnlocked { get; private set; }

    // ==========================================
    // Awake - Singleton Enforcement & State Load
    // ==========================================
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        EndCreditsUnlocked = PlayerPrefs.GetInt(PREF_END_CREDITS_UNLOCKED, 0) == 1;
    }

    // ==========================================
    // Unlock - Called by EndingTrigger.cs at Act 5 End Beat
    // ==========================================
    public void Unlock(BGMTrack track)
    {
        if (track != BGMTrack.EndCreditsTheme) return;
        EndCreditsUnlocked = true;
        PlayerPrefs.SetInt(PREF_END_CREDITS_UNLOCKED, 1);
        PlayerPrefs.Save();
    }

    // ==========================================
    // IsAllowed - Returns False for Locked Tracks
    // ==========================================
    public bool IsAllowed(BGMTrack track)
    {
        if (track == BGMTrack.EndCreditsTheme)
            return EndCreditsUnlocked;
        return true;
    }
}