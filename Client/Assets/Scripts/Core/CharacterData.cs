using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "RailRush/Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterId; // Matches database item_id (e.g., "char_dash")
    public string characterName;
    public GameObject modelPrefab;
    
    [Header("Stats")]
    public float speedMultiplier = 1.0f;
    public float jumpForceMultiplier = 1.0f;
    
    [Header("Abilities")]
    public string abilityDescription;
    // In future, link to Ability Logic scripts here
}
