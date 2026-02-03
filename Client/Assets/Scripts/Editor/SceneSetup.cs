using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SceneSetup : EditorWindow
{
    [MenuItem("RailRush/Setup Scene")]
    static void CreateScene()
    {
        // 0. Check for duplicates
        if (GameObject.Find("GameManagers") != null)
        {
            Debug.LogWarning("Scene already set up! Delete 'GameManagers', 'TrackManager', etc. if you want to reset.");
            return;
        }

        // 1. Game Managers
        GameObject managers = new GameObject("GameManagers");
        if (GameObject.Find("GameManagers") != null)
        {
            Debug.LogWarning("Scene already set up! Delete 'GameManagers', 'TrackManager', etc. if you want to reset.");
            return;
        }

        // 1. Game Managers
        managers.AddComponent<GameManager>();
        managers.AddComponent<APIManager>();
        managers.AddComponent<AdManager>();
        managers.AddComponent<CharacterManager>();
        
        // 2. Object Pool
        GameObject pool = new GameObject("ObjectPool");
        pool.AddComponent<ObjectPool>();

        // 3. Track Manager
        GameObject trackMgr = new GameObject("TrackManager");
        trackMgr.AddComponent<TrackManager>();

        // 4. Player Start Position
        GameObject startPos = new GameObject("PlayerStart");
        startPos.transform.position = new Vector3(0, 1, 0);
        
        // Link PlayerStart to CharacterManager
        CharacterManager charMgr = managers.GetComponent<CharacterManager>();
        charMgr.playerStartPoint = startPos.transform;

        // 5. Basic UI
        GameObject canvasGO = new GameObject("Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();
        
        // Leaderboard Panel
        GameObject leaderboardPanel = new GameObject("LeaderboardPanel");
        leaderboardPanel.transform.SetParent(canvasGO.transform, false);
        Image bg = leaderboardPanel.AddComponent<Image>();
        bg.color = new Color(0, 0, 0, 0.8f);
        leaderboardPanel.AddComponent<LeaderboardUI>();
        // Hide by default
        leaderboardPanel.SetActive(false);

        // 6. Camera Setup
        Camera.main.transform.position = new Vector3(0, 5, -10);
        Camera.main.transform.rotation = Quaternion.Euler(20, 0, 0);

        Debug.Log("RailRush Scene Setup Complete! Drag your Prefabs into the Inspectors now.");
    }
}
