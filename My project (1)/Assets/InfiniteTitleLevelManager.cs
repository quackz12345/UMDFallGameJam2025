using UnityEngine;
using System.Collections.Generic;

public class InfiniteTitleLevelManager : MonoBehaviour
{
    [Header("Level Prefabs")]
    public List<GameObject> levelPrefabs; // Prefabs to spawn
    public Transform cameraTransform;      // Camera that moves forward
    public float spawnOffset = 50f;        // Space between pieces
    public int initialPieces = 10;         // Pieces to spawn at start
    public float spawnBuffer = 100f;       // How far ahead to spawn new pieces

    private List<GameObject> activeLevels = new List<GameObject>();
    private float lastSpawnZ = 0f;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        // Spawn initial pieces
        for (int i = 0; i < initialPieces; i++)
        {
            SpawnNextLevel();
        }
    }

    void Update()
    {
        // Spawn new pieces if the camera is getting close to the end of the last piece
        if (cameraTransform.position.z + spawnBuffer > lastSpawnZ)
        {
            SpawnNextLevel();
            DestroyOldestLevel(); // optional: keep scene light
        }
    }

    void SpawnNextLevel()
    {
        GameObject prefab = levelPrefabs[Random.Range(0, levelPrefabs.Count)];
        float prefabLength = GetPrefabLength(prefab);

        GameObject newLevel = Instantiate(prefab, new Vector3(0, 0, lastSpawnZ + prefabLength / 2f + spawnOffset), Quaternion.identity);
        activeLevels.Add(newLevel);

        lastSpawnZ += prefabLength + spawnOffset;
    }

    void DestroyOldestLevel()
    {
        if (activeLevels.Count > initialPieces)
        {
            Destroy(activeLevels[0]);
            activeLevels.RemoveAt(0);
        }
    }

    float GetPrefabLength(GameObject prefab)
    {
        Collider col = prefab.GetComponent<Collider>();
        if (col != null) return col.bounds.size.z;

        Renderer rend = prefab.GetComponentInChildren<Renderer>();
        if (rend != null) return rend.bounds.size.z;

        Debug.LogWarning("Prefab has no collider or renderer! Defaulting to 50 units.");
        return 50f;
    }
}
