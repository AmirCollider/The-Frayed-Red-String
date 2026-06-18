// ==========================================
// SaveSlotRow - One Save Slot Card (auto-instantiated; minimal, clean layout)
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
    [SerializeField] private TextMeshProUGUI actionLabelText;

    [Header("Buttons")]
    [SerializeField] private Button actionButton;
    [SerializeField] private Button deleteButton;

    [Header("Empty State")]
    [SerializeField] private GameObject emptyOverlay;

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
    public void SetData(int index, bool hasData, string chapter,
                        string actionLabel, bool actionInteractable, bool showEmptyOverlay)
    {
        if (slotNumberText != null) slotNumberText.text = $"SLOT {index + 1}";
        if (chapterText != null) chapterText.text = hasData ? chapter : "— EMPTY —";
        if (actionLabelText != null) actionLabelText.text = actionLabel;

        if (actionButton != null) actionButton.interactable = actionInteractable;
        if (deleteButton != null) deleteButton.gameObject.SetActive(hasData);
        if (emptyOverlay != null) emptyOverlay.SetActive(showEmptyOverlay);
    }
}