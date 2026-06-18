// ==========================================
// FrameBootstrap - Per-Scene Frame Initializer (menu hides, acts show normal)
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEngine;

public class FrameBootstrap : MonoBehaviour
{
    // ==========================================
    // FrameBootMode - What This Scene Wants the Persistent Frame to Do on Load
    // ==========================================
    public enum FrameBootMode
    {
        Hide = 0,        // Main Menu — no gameplay border
        ShowNormal = 1   // Act scenes — normal gameplay border
    }

    // ==========================================
    // Inspector — Mode for This Scene
    // ==========================================
    [Header("Frame Boot Mode for This Scene")]
    [SerializeField] private FrameBootMode mode = FrameBootMode.ShowNormal;

    // ==========================================
    // Start - Apply This Scene's Frame Rule After the Persistent Frame Exists
    // ==========================================
    private void Start()
    {
        if (FrameController.Instance == null) return;

        switch (mode)
        {
            case FrameBootMode.Hide:
                FrameController.Instance.Hide();
                break;
            case FrameBootMode.ShowNormal:
                FrameController.Instance.ShowNormal();
                break;
        }
    }
}