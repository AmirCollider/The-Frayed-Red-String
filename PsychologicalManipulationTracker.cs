// ==========================================
// PsychologicalManipulationTracker - Logs Yua Escalation Milestones for Act 3 Payoff
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections.Generic;
using UnityEngine;

public class PsychologicalManipulationTracker : MonoBehaviour
{
    // ==========================================
    // Singleton Instance (per-scene — no DontDestroyOnLoad)
    // ==========================================
    public static PsychologicalManipulationTracker Instance { get; private set; }

    // ==========================================
    // Inspector — BranchRecord Key Prefix
    // ==========================================
    [Header("BranchRecord Key Prefix")]
    [SerializeField] private string branchKeyPrefix = "Act2_Milestone_";

    // ==========================================
    // Private State — Recorded Milestone Set
    // ==========================================
    private readonly HashSet<ManipulationMilestone> _recorded = new HashSet<ManipulationMilestone>();

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
    // RecordMilestone - Mark Milestone Reached and Persist to BranchRecord
    // ==========================================
    public void RecordMilestone(ManipulationMilestone milestone)
    {
        if (_recorded.Contains(milestone)) return;
        _recorded.Add(milestone);

        if (BranchRecord.Instance == null) return;
        BranchRecord.Instance.Record(branchKeyPrefix + milestone, 1);
    }

    // ==========================================
    // RecordMilestoneByName - Parse String to ManipulationMilestone and Record (for DialogueLine.gameEventId)
    // ==========================================
    public void RecordMilestoneByName(string milestoneName)
    {
        if (string.IsNullOrEmpty(milestoneName)) return;
        if (System.Enum.TryParse(milestoneName, true, out ManipulationMilestone parsed))
            RecordMilestone(parsed);
    }

    // ==========================================
    // HasMilestone - Query Whether a Milestone Was Reached This Session
    // ==========================================
    public bool HasMilestone(ManipulationMilestone milestone) => _recorded.Contains(milestone);

    // ==========================================
    // MilestoneCount - Total Number of Distinct Milestones Reached
    // ==========================================
    public int MilestoneCount => _recorded.Count;
}

// ==========================================
// ManipulationMilestone - Escalation Stages Tracked Across Act 2
// ==========================================
public enum ManipulationMilestone
{
    FriendTraumaConfessed = 0,
    FamilyTraumaConfessed = 1,
    FakeEvidencePlanted = 2,
    HaruDoubtSeeded = 3,
    HaruFullyIsolated = 4,
    FourthWallBreak = 5
}