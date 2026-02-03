using UnityEngine;
using UnityEditor;

public class TestAssets : EditorWindow
{
    [MenuItem("RailRush/Create Test Assets")]
    static void CreateAssets()
    {
        // 1. Create Folders
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
            AssetDatabase.CreateFolder("Assets", "Prefabs");

        // 2. Create Player Prefab
        GameObject playerGO = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        playerGO.name = "Player_Dash";
        playerGO.AddComponent<PlayerController>();
        playerGO.AddComponent<Rigidbody>().useGravity = true;
        playerGO.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        
        // Save as Prefab
        string playerPath = "Assets/Prefabs/Player_Dash.prefab";
        GameObject playerPrefab = PrefabUtility.SaveAsPrefabAsset(playerGO, playerPath);
        DestroyImmediate(playerGO);

        // 3. Create Track Prefab (Straight)
        GameObject trackGO = new GameObject("Track_Straight");
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.parent = trackGO.transform;
        floor.transform.localScale = new Vector3(1, 1, 6); // 60m long
        floor.transform.localPosition = new Vector3(0, 0, 30); // Center it
        trackGO.AddComponent<TrackSegment>();
        // Add Collider
        BoxCollider box = trackGO.AddComponent<BoxCollider>();
        box.center = new Vector3(0, 0, 30);
        box.size = new Vector3(10, 0.1f, 60);
        
        string trackPath = "Assets/Prefabs/Track_Straight.prefab";
        GameObject trackPrefab = PrefabUtility.SaveAsPrefabAsset(trackGO, trackPath);
        DestroyImmediate(trackGO);

        // 4. Update CharacterData
        CharacterData charData = ScriptableObject.CreateInstance<CharacterData>();
        charData.characterId = "char_dash";
        charData.characterName = "Dash";
        charData.modelPrefab = playerPrefab;
        AssetDatabase.CreateAsset(charData, "Assets/Resources/Dash_Data.asset");

        // 5. Auto-Assign to Scene
        CharacterManager cm = FindObjectOfType<CharacterManager>();
        if (cm != null)
        {
            cm.allCharacters = new CharacterData[] { charData };
        }

        TrackManager tm = FindObjectOfType<TrackManager>();
        if (tm != null)
        {
            tm.trackPrefabs = new GameObject[] { trackPrefab };
        }

        Debug.Log("Test Assets Created & Assigned! You can now press PLAY.");
    }
}
