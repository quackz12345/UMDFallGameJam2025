using UnityEngine;
using System.Collections.Generic;

public class LevelMangaer : MonoBehaviour
{
    public GameObject levelPrefab;    // Your level prefab
    public int initialPieces = 3;     // Number of pieces to spawn at start
    public float levelLength = 135f;  // Length of your prefab along Z

    private List<GameObject> activeLevels = new List<GameObject>();
    private Transform player;

    void Start()
    {
        // Find the player
        player = GameObject.FindWithTag("Player").transform;

        // Spawn initial pieces
        for (int i = 0; i < initialPieces; i++)
        {
            SpawnLevel(i);
        }
    }

    void Update()
    {
        // If player is approaching the end of the last piece, spawn a new one
        GameObject lastLevel = activeLevels[activeLevels.Count - 1];
        float lastLevelEndZ = lastLevel.transform.position.z + (levelLength / 2f);

        if (player.position.z > lastLevelEndZ - (levelLength * 1.5f))
        {
            SpawnLevel(activeLevels.Count);
            DestroyOldestLevel();
        }
    }

    void SpawnLevel(int index)
    {
        // Center pivot adjustment
        float spawnZ = index * levelLength;
        Vector3 spawnPos = new Vector3(0, 0, spawnZ);
        GameObject newLevel = Instantiate(levelPrefab, spawnPos, Quaternion.identity);
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
}
