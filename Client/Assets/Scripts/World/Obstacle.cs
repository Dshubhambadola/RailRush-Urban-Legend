using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit Obstacle!");
            // In the future:
            // - Play Crash Animation
            // - Show Game Over Screen
            // - GameManager.Instance.GameOver();
            
            // For now, simple Game Over call
            if(GameManager.Instance != null && GameManager.Instance.IsGamePlaying)
            {
               GameManager.Instance.GameOver();
            }
        }
    }
}
