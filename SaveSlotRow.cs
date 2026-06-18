// ==========================================
// SaveSlotRow - One Save Slot Card (empty-state or saved-state prefab variant)
// AmirCollider Games - The Frayed Red String
// ==========================================

using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotRow : MonoBehaviour
{
    // ==========================================
    // Inspector — Buttons (Load present on both; Delete only on the saved prefab)
    // ==========================================
    [Header("Buttons")]
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;

    // ==========================================
    // Inspector — Screenshot (only on the saved prefab)
    // ==========================================
    [Header("Screenshot (saved prefab only)")]
    [SerializeField] private Image screenshotImage;

    // ==========================================
    // Inspector — Optional Label
    // ==========================================
    [Header("Optional Label")]
    [SerializeField] private TextMeshProUGUI slotNumberText;

    // ==========================================
    // Bind - Hook Buttons to the Panel Callbacks (once, at instantiation)
    // ==========================================
    public void Bind(int index, Action onLoad, Action onDelete)
    {
        if (slotNumberText != null) slotNumberText.text = $"SLOT {index + 1}";

        if (loadButton != null)
        {
            loadButton.onClick.RemoveAllListeners();
            loadButton.onClick.AddListener(() => onLoad?.Invoke());
        }
        if (deleteButton != null)
        {
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(() => onDelete?.Invoke());
        }
    }

    // ==========================================
    // SetLoadInteractable - Empty Prefab Keeps Its Load Button Disabled
    // ==========================================
    public void SetLoadInteractable(bool interactable)
    {
        if (loadButton != null) loadButton.interactable = interactable;
    }

    // ==========================================
    // ApplyScreenshot - Load a PNG From Disk Into the Saved Row's Image
    // ==========================================
    public void ApplyScreenshot(string path)
    {
        if (screenshotImage == null) return;

        if (!string.IsNullOrEmpty(path) && File.Exists(path))
        {
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGB24, false);
            tex.LoadImage(bytes);
            screenshotImage.sprite = Sprite.Create(
                tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            screenshotImage.enabled = true;
        }
        else
        {
            // no screenshot on disk — leave whatever placeholder the prefab carries
            screenshotImage.enabled = screenshotImage.sprite != null;
        }
    }
}