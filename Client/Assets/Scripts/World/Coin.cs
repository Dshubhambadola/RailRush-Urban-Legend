using UnityEngine;

public class Coin : Collectible
{
    public int coinValue = 1;
    public int scoreValue = 10;

    protected override void OnCollect(PlayerController player)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoin(coinValue);
            GameManager.Instance.AddScore(scoreValue);
            Debug.Log("Collected Coin!");
        }
    }
}
