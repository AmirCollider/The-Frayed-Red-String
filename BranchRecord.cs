// ==========================================
// BranchRecord - Persistent Player Choice Log
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections.Generic;
using UnityEngine;

public class BranchRecord : MonoBehaviour
{
    // ==========================================
    // Singleton Instance
    // ==========================================
    public static BranchRecord Instance { get; private set; }

    // ==========================================
    // PlayerPrefs Key
    // ==========================================
    private const string PREF_KEY = "BranchRecord_Data";

    // ==========================================
    // State — Runtime Choice Dictionary
    // ==========================================
    private readonly Dictionary<string, int> _choices = new Dictionary<string, int>();

    // ==========================================
    // Awake - Singleton Enforcement, DontDestroyOnLoad, Load Saved Choices
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
        Load();
    }

    // ==========================================
    // Record - Store a Named Choice Result by 0-Based Index
    // ==========================================
    public void Record(string choiceId, int selectedIndex)
    {
        _choices[choiceId] = selectedIndex;
        Save();
    }

    // ==========================================
    // GetChoice - Return Recorded Index, or -1 if Not Found
    // ==========================================
    public int GetChoice(string choiceId)
        => _choices.TryGetValue(choiceId, out int val) ? val : -1;

    // ==========================================
    // Has - Returns True if a Choice Was Previously Recorded
    // ==========================================
    public bool Has(string choiceId) => _choices.ContainsKey(choiceId);

    // ==========================================
    // Clear - Erase All Stored Choices from Memory and PlayerPrefs
    // ==========================================
    public void Clear()
    {
        _choices.Clear();
        PlayerPrefs.DeleteKey(PREF_KEY);
        PlayerPrefs.Save();
    }

    // ==========================================
    // ExportJson - Serialize Current Choices (read by SaveSystem.CaptureCurrent)
    // ==========================================
    public string ExportJson()
    {
        return JsonUtility.ToJson(new BranchRecordData(_choices));
    }

    // ==========================================
    // ImportJson - Replace Choices from a Save Blob (called by Act managers on resume)
    // ==========================================
    public void ImportJson(string json)
    {
        _choices.Clear();
        if (!string.IsNullOrEmpty(json))
        {
            BranchRecordData data = JsonUtility.FromJson<BranchRecordData>(json);
            if (data != null)
                for (int i = 0; i < data.keys.Count && i < data.values.Count; i++)
                    _choices[data.keys[i]] = data.values[i];
        }
        Save();
    }

    // ==========================================
    // Save - Serialize Choice Dictionary to PlayerPrefs via JSON
    // ==========================================
    private void Save()
    {
        BranchRecordData data = new BranchRecordData(_choices);
        PlayerPrefs.SetString(PREF_KEY, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }

    // ==========================================
    // Load - Deserialize Choice Dictionary from PlayerPrefs
    // ==========================================
    private void Load()
    {
        string json = PlayerPrefs.GetString(PREF_KEY, "");
        if (string.IsNullOrEmpty(json)) return;

        BranchRecordData data = JsonUtility.FromJson<BranchRecordData>(json);
        if (data == null) return;

        _choices.Clear();
        for (int i = 0; i < data.keys.Count && i < data.values.Count; i++)
            _choices[data.keys[i]] = data.values[i];
    }
}

// ==========================================
// BranchRecordData - JSON-Serializable Wrapper for Choice Dictionary
// ==========================================
[System.Serializable]
public class BranchRecordData
{
    public List<string> keys = new List<string>();
    public List<int> values = new List<int>();

    public BranchRecordData() { }

    public BranchRecordData(Dictionary<string, int> dict)
    {
        foreach (KeyValuePair<string, int> kvp in dict)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }
}