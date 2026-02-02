using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGamePlaying { get; private set; } = false;
    public float Score { get; private set; }
    public float CoinCount { get; private set; }

    private void Start()
    {
        // Auto-login for prototype testing
        // In a real app, you'd show a UI or load saved creds
        APIManager.Instance.Login("player@test.com", "password123", (success) => {
            if (success)
            {
                Debug.Log($"Welcome back, {APIManager.Instance.CurrentUsername}!");
            }
            else
            {
                Debug.LogWarning("Auto-login failed. Use RegisterUI to create account.");
                // Fallback: Register a temp user (Optional for prototype)
                APIManager.Instance.Register("PlayerOne", "player@test.com", "password123", (regSuccess, err) => {
                     if(regSuccess) 
                     {
                         APIManager.Instance.Login("player@test.com", "password123", null);
                     }
                });
            }
        });
    }

    public void StartGame()
    {
        IsGamePlaying = true;
        Score = 0;
        CoinCount = 0;
    }

    public void GameOver()
    {
        IsGamePlaying = false;
        Debug.Log("Game Over! Score: " + Score);
        
        // Submit Run Data
        APIManager.Instance.SubmitRun(Score, (int)Score, (int)CoinCount, (success) => {
            if(success) Debug.Log("Run data saved to server.");
        });
    }

    public void AddScore(float amount)
    {
        if (IsGamePlaying)
        {
            Score += amount;
        }
    }

    public void AddCoin(int amount)
    {
        CoinCount += amount;
    }
    
    private void Update()
    {
        if(IsGamePlaying)
        {
            // Passive score increase over time/distance could go here
            AddScore(Time.deltaTime * 5); // Example
        }
    }
}
