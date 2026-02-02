using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    public AudioClip collectSound;
    public GameObject collectEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                OnCollect(player);
                
                // Visual/Audio Feedback can go here
                // if(collectSound) AudioSource.PlayClipAtPoint(collectSound, transform.position);
                
                // Return to pool or Destroy
                // For now, assuming ObjectPool is used
                if(ObjectPool.Instance != null)
                {
                    ObjectPool.Instance.ReturnToPool(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    protected abstract void OnCollect(PlayerController player);
}
