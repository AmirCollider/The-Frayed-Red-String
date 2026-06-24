// ==========================================
// SaveScreenshot - Captures a Clean Scene Frame (no dialogue box, no menu overlays)
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveScreenshot : MonoBehaviour
{
    // ==========================================
    // Singleton Instance (persistent)
    // ==========================================
    public static SaveScreenshot Instance { get; private set; }

    // ==========================================
    // Inspector — Thumbnail Size
    // ==========================================
    [Header("Thumbnail Size")]
    [SerializeField] private int captureWidth = 1245;
    [SerializeField] private int captureHeight = 120;

    // ==========================================
    // Inspector — Overlay Cull Threshold
    // Any Canvas with sortingOrder >= this is hidden for the capture frame
    // (dialogue box = 3, pause/load = 15, fade = 20). Scene art (0..2) stays.
    // ==========================================
    [Header("Overlay Cull Threshold (hide UI at/above this sort order)")]
    [SerializeField] private int overlaySortThreshold = 5;

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
    // Capture - Hide Dialogue Box + UI Overlays, Grab the Frame, Restore, Write PNG
    // ==========================================
    public void Capture(int slot)
    {
        StartCoroutine(CaptureRoutine(slot));
    }

    // ==========================================
    // CaptureRoutine - End-of-Frame Capture of Pure Scene Art
    // ==========================================
    private IEnumerator CaptureRoutine(int slot)
    {
        // ==========================================
        // 1) Hide the dialogue box (sort 3 — kept out of the shot for cleanliness)
        // ==========================================
        DialogueBoxUI box = FindAnyObjectByType<DialogueBoxUI>(FindObjectsInactive.Include);
        bool boxWasActive = box != null && box.gameObject.activeSelf;
        if (box != null) box.gameObject.SetActive(false);

        // ==========================================
        // 2) Hide every overlay Canvas at/above the threshold (pause menu, load panel, fade)
        // ==========================================
        List<Canvas> hidden = new List<Canvas>();
        Canvas[] all = FindObjectsByType<Canvas>(FindObjectsInactive.Exclude);
        for (int i = 0; i < all.Length; i++)
        {
            Canvas c = all[i];
            if (c == null || !c.isRootCanvas) continue;
            if (c.sortingOrder >= overlaySortThreshold && c.enabled)
            {
                c.enabled = false;
                hidden.Add(c);
            }
        }

        // ==========================================
        // 3) Wait for the frame to render WITHOUT those overlays, then capture
        // ==========================================
        yield return new WaitForEndOfFrame();

        Texture2D full = ScreenCapture.CaptureScreenshotAsTexture();

        // ==========================================
        // 4) Restore everything immediately
        // ==========================================
        for (int i = 0; i < hidden.Count; i++)
            if (hidden[i] != null) hidden[i].enabled = true;
        if (box != null) box.gameObject.SetActive(boxWasActive);

        // ==========================================
        // 5) Downscale to a thumbnail and write the PNG
        // ==========================================
        Texture2D thumb = ScaleTexture(full, captureWidth, captureHeight);
        byte[] png = thumb.EncodeToPNG();
        File.WriteAllBytes(PathForSlot(slot), png);

        Destroy(full);
        Destroy(thumb);
    }

    // ==========================================
    // ScaleTexture - Bilinear Downscale to Thumbnail Dimensions
    // ==========================================
    private static Texture2D ScaleTexture(Texture2D src, int w, int h)
    {
        RenderTexture rt = RenderTexture.GetTemporary(w, h, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
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