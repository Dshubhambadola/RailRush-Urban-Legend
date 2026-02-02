using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGamePlaying { get; private set; } = false;
    public float Score { get; private set; }
    public float CoinCount { get; private set; }

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
        // Trigger UI and potentially send score to backend
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
