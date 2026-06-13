// ==========================================
// Act1MasterSequenceGenerator - Assembles Scenes A-D into Act1Sequence Master Asset
// AmirCollider Games - The Frayed Red String
// ==========================================

using UnityEditor;
using UnityEngine;

public static class Act1MasterSequenceGenerator
{
    // ==========================================
    // Path Configuration
    // ==========================================
    private const string MASTER_PATH = "Assets/Data/Act1Sequence.asset";

    private static readonly string[] SUB_SEQUENCE_PATHS = new string[]
    {
        "Assets/Data/Act1/Act1SceneA_Sequence.asset",
        "Assets/Data/Act1/Act1SceneB_Sequence.asset",
        "Assets/Data/Act1/Act1SceneC_Sequence.asset",
        "Assets/Data/Act1/Act1SceneD_Sequence.asset",
    };

    // ==========================================
    // Generate - Merge All Scene Sub-Sequences into Act1Sequence Master
    // ==========================================
    [MenuItem("Frayed Red String/Act 1/Assemble Master Sequence (A-D)")]
    public static void Generate()
    {
        // ==========================================
        // Validate All Sub-Sequences Exist Before Assembling
        // ==========================================
        foreach (string path in SUB_SEQUENCE_PATHS)
        {
            if (AssetDatabase.LoadAssetAtPath<DialogueSequence>(path) == null)
            {
                Debug.LogWarning($"[Act1MasterSequenceGenerator] Missing: {path} — run all scene generators first.");
                return;
            }
        }

        DialogueAssetGenerator.EnsureFolder("Assets/Data");
        DialogueSequence master = DialogueAssetGenerator.LoadOrCreate<DialogueSequence>(MASTER_PATH);
        master.entries.Clear();

        int total = 0;
        foreach (string path in SUB_SEQUENCE_PATHS)
        {
            DialogueSequence sub = AssetDatabase.LoadAssetAtPath<DialogueSequence>(path);
            if (sub == null) continue;
            foreach (DialogueEntry entry in sub.entries)
                master.entries.Add(entry);
            total += sub.Count;
        }

        EditorUtility.SetDirty(master);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"[Act1MasterSequenceGenerator] Act1Sequence.asset assembled: {total} total entries from {SUB_SEQUENCE_PATHS.Length} scenes.");
    }
}