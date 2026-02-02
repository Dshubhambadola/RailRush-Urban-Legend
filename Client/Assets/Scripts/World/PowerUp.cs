using UnityEngine;
using System.Collections;

public abstract class PowerUp : Collectible
{
    public float duration = 10f;

    protected override void OnCollect(PlayerController player)
    {
        // Start the power-up effect routine on the Player or GameManager
        // For simplicity, let's start it on the player so it persists if this object is pooled/disabled
        player.StartCoroutine(PowerUpRoutine(player));
    }

    private IEnumerator PowerUpRoutine(PlayerController player)
    {
        Activate(player);
        Debug.Log($"PowerUp {gameObject.name} Activated!");
        
        yield return new WaitForSeconds(duration);
        
        Deactivate(player);
        Debug.Log($"PowerUp {gameObject.name} Deactivated!");
    }

    protected abstract void Activate(PlayerController player);
    protected abstract void Deactivate(PlayerController player);
}
