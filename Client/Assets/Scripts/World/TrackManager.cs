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
        GameObject go = Instantiate(trackPrefabs[prefabIndex], transform);
        go.transform.position = Vector3.forward * spawnZ;
        spawnZ += trackLength;
        activeTracks.Add(go);
    }

    private void DeleteTrack()
    {
        Destroy(activeTracks[0]);
        activeTracks.RemoveAt(0);
    }
}
