using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;

[System.Serializable]
public class LoginResponse
{
    public string message;
    public UserData user;
}

[System.Serializable]
public class UserData
{
    public string id;
    public string username;
}

[System.Serializable]
public class RunResponse
{
    public string message;
    public string runId;
}

[System.Serializable]
public class LeaderboardEntry
{
    public string username;
    public int score;
}

[System.Serializable]
public class LeaderboardResponse
{
    public LeaderboardEntry[] entries;
}

public class APIManager : MonoBehaviour
{
    public static APIManager Instance { get; private set; }
    
    // Replace with your local IP if testing on device, or localhost for Editor
    private const string BASE_URL = "http://localhost:3000/api"; 
    
    public string CurrentUserId { get; private set; }
    public string CurrentUsername { get; private set; }

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

    public void Login(string email, string password, Action<bool> callback)
    {
        StartCoroutine(LoginRoutine(email, password, callback));
    }

    private IEnumerator LoginRoutine(string email, string password, Action<bool> callback)
    {
        string json = JsonUtility.ToJson(new { email = email, password = password });
        
        using (UnityWebRequest www = new UnityWebRequest(BASE_URL + "/auth/login", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Login Error: " + www.error);
                callback?.Invoke(false);
            }
            else
            {
                Debug.Log("Login Success: " + www.downloadHandler.text);
                LoginResponse res = JsonUtility.FromJson<LoginResponse>(www.downloadHandler.text);
                
                CurrentUserId = res.user.id;
                CurrentUsername = res.user.username;
                
                callback?.Invoke(true);
            }
        }
    }

    public void Register(string username, string email, string password, Action<bool, string> callback)
    {
         StartCoroutine(RegisterRoutine(username, email, password, callback));
    }
    
    private IEnumerator RegisterRoutine(string username, string email, string password, Action<bool, string> callback)
    {
         string json = JsonUtility.ToJson(new { username = username, email = email, password = password });
          using (UnityWebRequest www = new UnityWebRequest(BASE_URL + "/auth/register", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                callback?.Invoke(false, www.error);
            }
            else
            {
                 // Crude parsing for prototype, ideally define a RegisterResponse struct
                 // Assuming it returns userId
                Debug.Log("Register Success: " + www.downloadHandler.text);
                callback?.Invoke(true, null);
            }
        }
    }

    public void SubmitRun(float distance, int score, int coins, Action<bool> callback)
    {
        if (string.IsNullOrEmpty(CurrentUserId))
        {
            Debug.LogWarning("Cannot submit run: No User Logged In");
            callback?.Invoke(false);
            return;
        }

        StartCoroutine(SubmitRunRoutine(distance, score, coins, callback));
    }

    private IEnumerator SubmitRunRoutine(float distance, int score, int coins, Action<bool> callback)
    {
        string json = JsonUtility.ToJson(new { userId = CurrentUserId, distance = distance, score = score, coins = coins });
        
        using (UnityWebRequest www = new UnityWebRequest(BASE_URL + "/runs", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Run Submit Error: " + www.error);
                callback?.Invoke(false);
            }
            else
            {
                Debug.Log("Run Submitted Successfully: " + www.downloadHandler.text);
                callback?.Invoke(true);
            }
        }
    }

    public void GetLeaderboard(Action<List<LeaderboardEntry>> callback)
    {
        StartCoroutine(GetLeaderboardRoutine(callback));
    }

    private IEnumerator GetLeaderboardRoutine(Action<List<LeaderboardEntry>> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(BASE_URL + "/leaderboard"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Leaderboard Error: " + www.error);
                callback?.Invoke(null);
            }
            else
            {
                // Unity's JsonUtility cannot parse top-level arrays directly.
                // We need to wrap it or use a helper. 
                // For simplicity, let's assume the server returns an object { "entries": [...] } 
                // or we use a wrapper hack.
                // Let's change the server to return an array and use a simple wrapper hack here.
                string json = "{\"entries\":" + www.downloadHandler.text + "}";
                LeaderboardResponse res = JsonUtility.FromJson<LeaderboardResponse>(json);
                
                callback?.Invoke(new List<LeaderboardEntry>(res.entries));
            }
        }
    }

    public void CreateCrew(string name, string tag, Action<bool> callback)
    {
        StartCoroutine(CreateCrewRoutine(name, tag, callback));
    }

    private IEnumerator CreateCrewRoutine(string name, string tag, Action<bool> callback)
    {
        string json = JsonUtility.ToJson(new { name = name, tag = tag, userId = CurrentUserId });
        
        using (UnityWebRequest www = new UnityWebRequest(BASE_URL + "/crews", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Create Crew Error: " + www.error);
                callback?.Invoke(false);
            }
            else
            {
                Debug.Log("Crew Created: " + www.downloadHandler.text);
                callback?.Invoke(true);
            }
        }
    }

    public void JoinCrew(string crewId, Action<bool> callback)
    {
        StartCoroutine(JoinCrewRoutine(crewId, callback));
    }

    private IEnumerator JoinCrewRoutine(string crewId, Action<bool> callback)
    {
        string json = JsonUtility.ToJson(new { userId = CurrentUserId });
        
        using (UnityWebRequest www = new UnityWebRequest(BASE_URL + "/crews/" + crewId + "/join", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Join Crew Error: " + www.error);
                callback?.Invoke(false);
            }
            else
            {
                Debug.Log("Joined Crew: " + www.downloadHandler.text);
                callback?.Invoke(true);
            }
        }
    }
}
