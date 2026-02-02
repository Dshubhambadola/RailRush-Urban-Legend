using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    public CharacterData[] allCharacters;
    public Transform playerStartPoint;
    
    // Default to Dash if nothing selected
    private string selectedCharacterId = "char_dash";
    private GameObject currentPlayerObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectCharacter(string charId)
    {
        // Add verification logic here (check Inventory via APIManager)
        selectedCharacterId = charId;
        Debug.Log("Selected Character: " + charId);
    }

    public void SpawnPlayer()
    {
        if (currentPlayerObject != null)
        {
            Destroy(currentPlayerObject);
        }

        CharacterData data = GetCharacterById(selectedCharacterId);
        if (data != null && data.modelPrefab != null)
        {
            currentPlayerObject = Instantiate(data.modelPrefab, playerStartPoint.position, playerStartPoint.rotation);
            
            // Initialize PlayerController with stats
            PlayerController controller = currentPlayerObject.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.forwardSpeed *= data.speedMultiplier;
                controller.jumpForce *= data.jumpForceMultiplier;
                
                // Assign Camera to follow this new player
                // CameraFollow.Instance.SetTarget(currentPlayerObject.transform); // Placeholder
            }
        }
        else
        {
            Debug.LogError("Failed to spawn character: Data or Prefab missing.");
        }
    }

    private CharacterData GetCharacterById(string id)
    {
        foreach (var c in allCharacters)
        {
            if (c.characterId == id) return c;
        }
        return null;
    }
}
