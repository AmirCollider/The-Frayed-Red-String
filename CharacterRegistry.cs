// ==========================================
// CharacterRegistry - Scene-Local Lookup Table for Active Character Controllers
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections.Generic;
using UnityEngine;

public class CharacterRegistry : MonoBehaviour
{
    // ==========================================
    // Singleton Instance (per-scene — no DontDestroyOnLoad)
    // ==========================================
    public static CharacterRegistry Instance { get; private set; }

    // ==========================================
    // Inspector — Pre-Assigned Character Controllers
    // ==========================================
    [Header("Characters")]
    [SerializeField] private CharacterSpriteController[] characters;

    // ==========================================
    // Private State
    // ==========================================
    private readonly Dictionary<string, CharacterSpriteController> _registry
        = new Dictionary<string, CharacterSpriteController>();

    // ==========================================
    // Awake - Singleton Enforcement and Initial Registration
    // ==========================================
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (characters == null) return;
        foreach (CharacterSpriteController c in characters)
            if (c != null && !string.IsNullOrEmpty(c.CharacterId))
                _registry[c.CharacterId] = c;
    }

    // ==========================================
    // Register - Manually Add Controller at Runtime
    // ==========================================
    public void Register(CharacterSpriteController controller)
    {
        if (controller == null || string.IsNullOrEmpty(controller.CharacterId)) return;
        _registry[controller.CharacterId] = controller;
    }

    // ==========================================
    // Get - Retrieve Controller by Character ID String
    // ==========================================
    public CharacterSpriteController Get(string characterId)
    {
        if (string.IsNullOrEmpty(characterId)) return null;
        _registry.TryGetValue(characterId, out CharacterSpriteController result);
        return result;
    }

    // ==========================================
    // SetState - Apply Emotional State to Named Character
    // ==========================================
    public void SetState(string characterId, CharacterState state, bool instant = false)
    {
        Get(characterId)?.SetState(state, instant);
    }

    // ==========================================
    // SetPosition - Apply Position to Named Character
    // ==========================================
    public void SetPosition(string characterId, CharacterPosition position, bool instant = false)
    {
        Get(characterId)?.SetPosition(position, instant);
    }

    // ==========================================
    // HideAll - Move All Registered Characters to OffScreen
    // ==========================================
    public void HideAll(bool instant = false)
    {
        foreach (CharacterSpriteController c in _registry.Values)
            c?.Hide(instant);
    }
}