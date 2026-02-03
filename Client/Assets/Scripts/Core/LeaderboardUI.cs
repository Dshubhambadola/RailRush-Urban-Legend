using UnityEngine;
using System.Collections.Generic;

public class LeaderboardUI : MonoBehaviour
{
    public GameObject rowPrefab;
    public Transform contentArea;
    public GameObject loadingIndicator;

    private void OnEnable()
    {
        RefreshLeaderboard();
    }

    public void RefreshLeaderboard()
    {
        if(loadingIndicator) loadingIndicator.SetActive(true);
        
        // Clear existing
        foreach(Transform child in contentArea)
        {
            Destroy(child.gameObject);
        }

        APIManager.Instance.GetLeaderboard((entries) => {
            if(loadingIndicator) loadingIndicator.SetActive(false);

            if (entries != null)
            {
                for (int i = 0; i < entries.Count; i++)
                {
                    GameObject rowObj = Instantiate(rowPrefab, contentArea);
                    LeaderboardRow row = rowObj.GetComponent<LeaderboardRow>();
                    if (row != null)
                    {
                        row.SetData(i + 1, entries[i].username, entries[i].score);
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to load leaderboard.");
                // Optionally show error text
            }
        });
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
