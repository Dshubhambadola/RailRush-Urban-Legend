using UnityEngine;
using System;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }

    [SerializeField] private bool testMode = true;
    
    // In a real implementation, you would use UnityEngine.Advertisements;
    // string gameId = "1234567";

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

    private void Start()
    {
        Debug.Log("AdManager Initialized (Mock Mode: " + testMode + ")");
        // Advertisement.Initialize(gameId, testMode);
    }

    public void ShowInterstitial()
    {
        Debug.Log("Showing Interstitial Ad...");
        // In real app: Advertisement.Show("interstitial_placement");
    }

    public void ShowRewarded(Action<bool> onComplete)
    {
        Debug.Log("Showing Rewarded Ad...");
        // In real app: Use IUnityAdsShowListener logic
        
        // Mock success for prototype
        if(onComplete != null)
        {
             // Simulate delay
             StartCoroutine(MockAdRoutine(onComplete));
        }
    }

    private System.Collections.IEnumerator MockAdRoutine(Action<bool> onComplete)
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Ad Watched/Skipped.");
        onComplete(true); // Always success in mock
    }
}
