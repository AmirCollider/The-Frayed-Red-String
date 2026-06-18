// ==========================================
// SaveSlotRow - One Save Slot Card (lives on the auto-instantiated row prefab)
// AmirCollider Games - The Frayed Red String
// ==========================================

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotRow : MonoBehaviour
{
    // ==========================================
    // Inspector — Widgets on This Row
    // ==========================================
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI slotNumberText;
    [SerializeField] private TextMeshProUGUI chapterText;
    [SerializeField] private TextMeshProUGUI timestampText;
    [SerializeField] private TextMeshProUGUI actionLabelText;

    [Header("Buttons")]
    [SerializeField] private Button actionButton;
    [SerializeField] private Button deleteButton;

    [Header("Empty State")]
    [SerializeField] private GameObject emptyOverlay;
    [SerializeField] private Image screenshotImage;

    // ==========================================
    // Bind - Hook the Action/Delete Buttons to the Panel's Callbacks (once)
    // ==========================================
    public void Bind(int index, Action onAction, Action onDelete)
    {
        if (actionButton != null)
        {
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(() => onAction?.Invoke());
        }
        if (deleteButton != null)
        {
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(() => onDelete?.Invoke());
        }
    }

    // ==========================================
    // SetData - Push the Current Slot State Into This Row's Widgets
    // ==========================================
    public void SetData(int index, bool hasData, string chapter, string timestamp,
                         string actionLabel, bool actionInteractable, bool showEmptyOverlay)
    {
        if (slotNumberText != null) slotNumberText.text = $"SLOT  {index + 1}";
        if (chapterText != null) chapterText.text = chapter;
        if (timestampText != null) timestampText.text = timestamp;
        if (actionLabelText != null) actionLabelText.text = actionLabel;

        if (actionButton != null) actionButton.interactable = actionInteractable;
        if (deleteButton != null) deleteButton.gameObject.SetActive(hasData);
        if (emptyOverlay != null) emptyOverlay.SetActive(showEmptyOverlay);
        if (screenshotImage != null) screenshotImage.gameObject.SetActive(false);
    }
}