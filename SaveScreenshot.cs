// ==========================================
// SaveScreenshot - Captures a Dialogue-Box-Free Frame for the Save Card
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using System.IO;
using UnityEngine;

public class SaveScreenshot : MonoBehaviour
{
    // ==========================================
    // Singleton Instance (persistent)
    // ==========================================
    public static SaveScreenshot Instance { get; private set; }

    // ==========================================
    // Inspector — Capture Size (downscaled thumbnail)
    // ==========================================
    [Header("Thumbnail Size")]
    [SerializeField] private int captureWidth = 480;
    [SerializeField] private int captureHeight = 270;

    // ==========================================
    // Awake - Singleton Enforcement and Persistence
    // ==========================================
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ==========================================
    // PathForSlot - Persistent File Path for a Slot's Screenshot
    // ==========================================
    public static string PathForSlot(int slot)
        => Path.Combine(Application.persistentDataPath, $"save_shot_{slot}.png");

    // ==========================================
    // Capture - Hide the Dialogue Box, Grab the Frame, Restore, Write PNG
    // ==========================================
    public void Capture(int slot)
    {
        StartCoroutine(CaptureRoutine(slot));
    }

    // ==========================================
    // CaptureRoutine - End-of-Frame Capture With the Dialogue UI Hidden for Cleanliness
    // ==========================================
    private IEnumerator CaptureRoutine(int slot)
    {
        // Hide the dialogue box so the screenshot is pure scene art
        DialogueBoxUI box = FindAnyObjectByType<DialogueBoxUI>(FindObjectsInactive.Include);
        bool boxWasActive = box != null && box.gameObject.activeSelf;
        if (box != null) box.gameObject.SetActive(false);

        yield return new WaitForEndOfFrame();

        Texture2D full = ScreenCapture.CaptureScreenshotAsTexture();

        // Restore the dialogue box immediately
        if (box != null) box.gameObject.SetActive(boxWasActive);

        // Downscale to a thumbnail
        Texture2D thumb = ScaleTexture(full, captureWidth, captureHeight);
        byte[] png = thumb.EncodeToPNG();
        File.WriteAllBytes(PathForSlot(slot), png);

        Destroy(full);
        Destroy(thumb);
    }

    // ==========================================
    // ScaleTexture - Simple Bilinear Downscale to Thumbnail Dimensions
    // ==========================================
    private static Texture2D ScaleTexture(Texture2D src, int w, int h)
    {
        RenderTexture rt = RenderTexture.GetTemporary(w, h);
        Graphics.Blit(src, rt);

        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D result = new Texture2D(w, h, TextureFormat.RGB24, false);
        result.ReadPixels(new Rect(0, 0, w, h), 0, 0);
        result.Apply();

        RenderTexture.active = prev;
        RenderTexture.ReleaseTemporary(rt);
        return result;
    }
}