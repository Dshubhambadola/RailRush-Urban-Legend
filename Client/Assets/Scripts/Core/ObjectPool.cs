using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private Dictionary<string, GameObject> parentObjects;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            poolDictionary = new Dictionary<string, Queue<GameObject>>();
            parentObjects = new Dictionary<string, GameObject>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        string poolKey = prefab.name;

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<GameObject>());
            
            // Create a parent object to keep hierarchy clean
            GameObject parent = new GameObject(poolKey + "_Pool");
            parent.transform.SetParent(transform);
            parentObjects.Add(poolKey, parent);
        }

        GameObject objToSpawn;

        if (poolDictionary[poolKey].Count > 0)
        {
            objToSpawn = poolDictionary[poolKey].Dequeue();
        }
        else
        {
            objToSpawn = Instantiate(prefab);
            objToSpawn.name = prefab.name; // Keep name clean prevents "(Clone)" buildup issues if we used name for keys strictly
            objToSpawn.transform.SetParent(parentObjects[poolKey].transform);
        }

        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;
        objToSpawn.SetActive(true);

        return objToSpawn;
    }

    public void ReturnToPool(GameObject obj)
    {
        string poolKey = obj.name; // Assuming name matches key provided in Get (sanitized)
        
        // Remove (Clone) from name if present to find key, or ensure Get sets it correctly.
        // Simplified: We assume obj.name is the key. 
        // If Instantiate adds (Clone), we need to handle that.
        // Let's rely on the Get method stripping (Clone) or setting name.
        if (poolKey.EndsWith("(Clone)"))
        {
            poolKey = poolKey.Replace("(Clone)", "");
            obj.name = poolKey;
        }

        obj.SetActive(false);

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<GameObject>());
            GameObject parent = new GameObject(poolKey + "_Pool");
            parent.transform.SetParent(transform);
            parentObjects.Add(poolKey, parent);
            obj.transform.SetParent(parent.transform);
        }

        poolDictionary[poolKey].Enqueue(obj);
    }
}
