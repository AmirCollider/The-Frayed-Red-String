// ==========================================
// BackgroundManager - Scene Background Loading and Swap Controller
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    // ==========================================
    // Singleton Instance (per-scene — no DontDestroyOnLoad)
    // ==========================================
    public static BackgroundManager Instance { get; private set; }

    // ==========================================
    // Inspector — Sprite Map (assign all BackgroundID entries)
    // ==========================================
    [Header("Background Sprite Map")]
    [SerializeField] private BackgroundEntry[] backgroundMap;

    // ==========================================
    // Inspector — Transition Controller Reference
    // ==========================================
    [Header("Transition")]
    [SerializeField] private BackgroundTransition transition;

    // ==========================================
    // Private State
    // ==========================================
    private BackgroundID _currentBackground = BackgroundID.None;

    // ==========================================
    // Awake - Singleton Enforcement
    // ==========================================
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // ==========================================
    // SetBackground - Load Sprite by ID and Apply Transition Mode
    // ==========================================
    public void SetBackground(BackgroundID id, BackgroundTransitionMode mode = BackgroundTransitionMode.Crossfade)
    {
        if (id == BackgroundID.None || id == _currentBackground) return;
        _currentBackground = id;

        Sprite sprite = GetSprite(id);
        if (sprite == null)
        {
            Debug.LogWarning($"[BackgroundManager] No sprite mapped for BackgroundID.{id}.");
            return;
        }

        if (transition == null)
        {
            Debug.LogWarning("[BackgroundManager] BackgroundTransition reference is null.");
            return;
        }

        switch (mode)
        {
            case BackgroundTransitionMode.Instant: transition.InstantCut(sprite); break;
            case BackgroundTransitionMode.Crossfade: transition.Crossfade(sprite); break;
            case BackgroundTransitionMode.GlitchCut: transition.GlitchCut(sprite); break;
        }
    }

    // ==========================================
    // SetBackgroundInstant - Convenience Instant-Cut Wrapper
    // ==========================================
    public void SetBackgroundInstant(BackgroundID id)
    {
        SetBackground(id, BackgroundTransitionMode.Instant);
    }

    // ==========================================
    // ForceBackground - Bypass Equality Guard (use on Act scene first load)
    // ==========================================
    public void ForceBackground(BackgroundID id, BackgroundTransitionMode mode = BackgroundTransitionMode.Instant)
    {
        _currentBackground = BackgroundID.None;
        SetBackground(id, mode);
    }

    // ==========================================
    // CurrentBackground - Active Background ID Accessor
    // ==========================================
    public BackgroundID CurrentBackground => _currentBackground;

    // ==========================================
    // GetSprite - Linear Search Through Background Map
    // ==========================================
    private Sprite GetSprite(BackgroundID id)
    {
        if (backgroundMap == null) return null;
        foreach (BackgroundEntry entry in backgroundMap)
            if (entry.id == id) return entry.sprite;
        return null;
    }
}

// ==========================================
// BackgroundEntry - Serializable ID-to-Sprite Binding
// ==========================================
[System.Serializable]
public class BackgroundEntry
{
    public BackgroundID id;
    public Sprite sprite;
}