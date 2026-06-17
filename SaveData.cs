// ==========================================
// SaveData - Serializable Mid-Scene Save Snapshot
// AmirCollider Games - The Frayed Red String
// ==========================================

using System;

[Serializable]
public class SaveData
{
    // ==========================================
    // Master Sequence Resume Point
    // ==========================================
    public int actNumber = 1;
    public int entryIndex = 0;

    // ==========================================
    // Act 1 Affection Metric (0 outside Act 1)
    // ==========================================
    public int affectionScore = 0;

    // ==========================================
    // Language at Save Time
    // ==========================================
    public string language = "EN";

    // ==========================================
    // Visual Snapshot — Restored on Load, Then the Sequence Resumes (cast from enums)
    // ==========================================
    public int backgroundId = 0;     // (int)BackgroundID
    public int frameState = 0;        // (int)FrameState
    public int colorPalette = 0;      // (int)ColorPalette
    public int haruState = 0;         // (int)CharacterState
    public int haruPosition = 3;      // (int)CharacterPosition (Right)
    public int yuaState = 0;          // (int)CharacterState
    public int yuaPosition = 1;       // (int)CharacterPosition (Left)

    // ==========================================
    // BranchRecord Snapshot (JSON of BranchRecordData)
    // ==========================================
    public string branchRecordJson = "";

    // ==========================================
    // Metadata for the Load Panel Card
    // ==========================================
    public int playMinutes = 0;
    public string dateLabel = "";
    public string chapterLabel = "";
}