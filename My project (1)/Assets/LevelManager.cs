using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [Header("Level Prefabs")]
    public List<GameObject> levelPrefabs; // Add all your prefabs here
    public int initialPieces = 3;         // Pieces spawned at start
    public float spawnOffset = 50f;        // Extra distance between pieces

    private List<GameObject> activeLevels = new List<GameObject>();
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        // Spawn initial pieces
        for (int i = 0; i < initialPieces; i++)
        {
            SpawnNextLevel();
        }
    }

    void Update()
    {
        if (activeLevels.Count == 0) return;

        GameObject lastLevel = activeLevels[activeLevels.Count - 1];
        float lastLevelEndZ = lastLevel.transform.position.z + GetPrefabLength(lastLevel) / 2f;

        float spawnBuffer = 20f; // How far ahead to trigger the next spawn
        if (player.position.z > lastLevelEndZ - spawnBuffer)
        {
            SpawnNextLevel();
            DestroyOldestLevel();
        }
    }

    void SpawnNextLevel()
    {
        // Pick a random prefab (repeats allowed)
        GameObject prefab = levelPrefabs[Random.Range(0, levelPrefabs.Count)];

        float spawnZ = 0f;
        if (activeLevels.Count > 0)
        {
            GameObject lastLevel = activeLevels[activeLevels.Count - 1];
            spawnZ = lastLevel.transform.position.z + GetPrefabLength(lastLevel) + spawnOffset;
        }

        GameObject newLevel = Instantiate(prefab, new Vector3(0, 0, spawnZ), Quaternion.identity);
        activeLevels.Add(newLevel);
    }

    void DestroyOldestLevel()
    {
        if (activeLevels.Count > initialPieces)
        {
            Destroy(activeLevels[0]);
            activeLevels.RemoveAt(0);
        }
    }

    float GetPrefabLength(GameObject level)
    {
        Collider col = level.GetComponent<Collider>();
        if (col != null) return col.bounds.size.z;

        Renderer rend = level.GetComponentInChildren<Renderer>();
        if (rend != null) return rend.bounds.size.z;

        Debug.LogWarning("Prefab has no collider or renderer! Defaulting to 135 units.");
        return 135f;
    }
}
