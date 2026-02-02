using UnityEngine;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour
{
    public GameObject[] trackPrefabs;
    public Transform playerTransform;
    public float spawnZ = 0.0f;
    public float trackLength = 100.0f; // Approx length of one segment
    public int simultaneousSegments = 5;
    public float safeZone = 150.0f;

    private List<GameObject> activeTracks;

    private void Start()
    {
        activeTracks = new List<GameObject>();
        
        // Spawn initial tracks
        for (int i = 0; i < simultaneousSegments; i++)
        {
            SpawnTrack(0); // Assuming index 0 is a safe/straight track
        }
    }

    private void Update()
    {
        if (playerTransform.position.z - safeZone > (spawnZ - simultaneousSegments * trackLength))
        {
            SpawnTrack(Random.Range(0, trackPrefabs.Length));
            DeleteTrack();
        }
    }

    private void SpawnTrack(int prefabIndex)
    {
        // Use ObjectPool instead of Instantiate
        // Ensure ObjectPool is in the scene. For prototype, we lazily add it if missing or assume it's there.
        if (ObjectPool.Instance == null)
        {
             GameObject poolObj = new GameObject("ObjectPool");
             poolObj.AddComponent<ObjectPool>();
        }

        GameObject go = ObjectPool.Instance.Get(trackPrefabs[prefabIndex], Vector3.forward * spawnZ, Quaternion.identity);
        go.transform.SetParent(transform); // Optional, but keeps hierarchy under TrackManager if desired, or let Pool handle it.
        // For pooling, usually better to let Pool manage hierarchy or just leave it. 
        // Let's re-parent to TrackManager for this specific component logic if needed, 
        // but ObjectPool.cs puts them in sub-parents. Let's respect ObjectPool's hierarchy for cleaner scene.
        
        spawnZ += trackLength;
        activeTracks.Add(go);
    }

    private void DeleteTrack()
    {
        ObjectPool.Instance.ReturnToPool(activeTracks[0]);
        activeTracks.RemoveAt(0);
    }
}
