// ==========================================
// DialogueSequence - Ordered Dialogue Entry Collection (ScriptableObject)
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueSequence", menuName = "The Frayed Red String/Dialogue Sequence")]
public class DialogueSequence : ScriptableObject
{
    // ==========================================
    // Entry List
    // ==========================================
    [Header("Entries")]
    public List<DialogueEntry> entries = new List<DialogueEntry>();

    // ==========================================
    // Count - Total Entry Count
    // ==========================================
    public int Count => entries != null ? entries.Count : 0;

    // ==========================================
    // GetEntry - Safe Index Accessor
    // ==========================================
    public DialogueEntry GetEntry(int index)
    {
        if (entries == null || index < 0 || index >= entries.Count) return null;
        return entries[index];
    }
}

// ==========================================
// DialogueEntry - Discriminated Union: Line or Branch Node
// ==========================================
[System.Serializable]
public class DialogueEntry
{
    public DialogueEntryType type = DialogueEntryType.Line;

    // ==========================================
    // Line Variant
    // ==========================================
    [Header("Line (active when type = Line)")]
    public DialogueLine line;

    // ==========================================
    // Branch Variant
    // ==========================================
    [Header("Branch (active when type = Branch)")]
    public string branchId = "";
    public List<DialogueChoice> choices = new List<DialogueChoice>();
}

// ==========================================
// DialogueChoice - One Option in a Branch Node
// ==========================================
[System.Serializable]
public class DialogueChoice
{
    [TextArea(1, 3)] public string labelEN = "";
    [TextArea(1, 3)] public string labelJP = "";
    public DialogueSequence consequenceSequence;
    // ==========================================
    // Affection Delta — Applied by Act1Manager.OnChoiceMadeHandler
    // ==========================================
    public int affectionDelta = 0;

    // ==========================================
    // GetLabel - Language-Appropriate Choice Label
    // ==========================================
    public string GetLabel()
    {
        if (GameManager.Instance == null) return labelEN;
        if (GameManager.Instance.CurrentLanguage == ConstantsConfig.LANG_JAPANESE
            && !string.IsNullOrEmpty(labelJP))
            return labelJP;
        return labelEN;
    }
}

// ==========================================
// DialogueEntryType - Discriminator Enum
// ==========================================
public enum DialogueEntryType
{
    Line = 0,
    Branch = 1
}