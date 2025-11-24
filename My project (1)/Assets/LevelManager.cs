using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [Header("Tutorial")]
    public GameObject tutorialPrefab;

    [Header("Levels")]
    public List<LevelPieceSet> levelPieceSets;  // Each level has its own list of prefabs
    public List<GameObject> endingPrefabs;      // Ending prefab per level

    [Header("Settings")]
    private List<GameObject> activePieces = new List<GameObject>();
    private Transform player;

    private int currentLevel = -1;                  // -1 = tutorial
    private List<GameObject> shuffledPieces;       // Shuffled list of prefabs for current level
    private int pieceIndex = 0;                     // Index in shuffledPieces
    private bool spawningEnding = false;           // Flag to know when to spawn ending

    [System.Serializable]
    public class LevelPieceSet
    {
        public List<GameObject> pieces;
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        // Spawn tutorial first
        SpawnTutorial();
    }

    void Update()
    {
        if (activePieces.Count == 0) return;

        GameObject lastPiece = activePieces[^1];
        Vector3 lastEndPos = GetEndPosition(lastPiece);

        if (player.position.z > lastEndPos.z - spawnBuffer)
        {
            SpawnNextPiece();
            CleanupOldPieces();
        }
    }

    // ------------------------------
    // SPAWN TUTORIAL
    // ------------------------------
    void SpawnTutorial()
    {
        GameObject tut = Instantiate(tutorialPrefab, Vector3.zero, Quaternion.identity);
        activePieces.Add(tut);

        // After tutorial, start level 0
        currentLevel = 0;
        PrepareLevel(currentLevel);
    }

    // ------------------------------
    // PREPARE SHUFFLED LEVEL
    // ------------------------------
    void PrepareLevel(int level)
    {
        if (level >= levelPieceSets.Count)
        {
            Debug.Log("All levels completed!");
            return;
        }

        // Copy the list of prefabs so we can shuffle
        shuffledPieces = new List<GameObject>(levelPieceSets[level].pieces);

        // Shuffle the list
        for (int i = 0; i < shuffledPieces.Count; i++)
        {
            int r = Random.Range(i, shuffledPieces.Count);
            (shuffledPieces[i], shuffledPieces[r]) = (shuffledPieces[r], shuffledPieces[i]);
        }

        pieceIndex = 0;
        spawningEnding = false;
    }

    // ------------------------------
    // SPAWN NEXT PIECE IN LEVEL
    // ------------------------------
    void SpawnNextPiece()
    {
        float spawnZ = 0f;
        if (activePieces.Count > 0)
        {
            GameObject last = activePieces[^1];
            spawnZ = GetEndPosition(last).z;
        }

        GameObject prefabToSpawn;

        // Check if we need to spawn the ending prefab
        if (spawningEnding)
        {
            prefabToSpawn = endingPrefabs[currentLevel];
            SpawnAligned(prefabToSpawn, spawnZ);

            // Move to next level
            currentLevel++;
            if (currentLevel < levelPieceSets.Count)
                PrepareLevel(currentLevel);

            return;
        }

        // If all pieces in this level are spawned, spawn ending next
        if (pieceIndex >= shuffledPieces.Count)
        {
            spawningEnding = true;
            SpawnNextPiece(); // Immediately spawn ending
            return;
        }

        // Spawn the next normal piece
        prefabToSpawn = shuffledPieces[pieceIndex];
        pieceIndex++;

        SpawnAligned(prefabToSpawn, spawnZ);
    }

    // ------------------------------
    // SPAWN WITH SEAMLESS ALIGNMENT
    // ------------------------------
    void SpawnAligned(GameObject prefab, float spawnZ)
    {
        GameObject last = activePieces.Count > 0 ? activePieces[^1] : null;
        Vector3 spawnPos;

        if (last == null)
        {
            spawnPos = new Vector3(0, 0, spawnZ);
        }
        else
        {
            Transform lastEnd = last.transform.Find("End");
            Transform nextStart = prefab.transform.Find("Start");

            if (lastEnd != null && nextStart != null)
            {
                spawnPos = lastEnd.position - (nextStart.position - prefab.transform.position);
            }
            else
            {
                spawnPos = last.transform.position + new Vector3(0, 0, GetPrefabLength(last));
            }
        }

        GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
        activePieces.Add(obj);
    }

    // ------------------------------
    // CLEANUP OLD PIECES
    // ------------------------------
    void CleanupOldPieces()
    {
        while (activePieces.Count > maxActivePieces)
        {
            Destroy(activePieces[0]);
            activePieces.RemoveAt(0);
        }
    }

    // ------------------------------
    // HELPER FUNCTIONS
    // ------------------------------
    Vector3 GetEndPosition(GameObject prefab)
    {
        Transform end = prefab.transform.Find("End");
        if (end != null) return end.position;

        Vector3 pos = prefab.transform.position;
        pos.z += GetPrefabLength(prefab);
        return pos;
    }

    float GetPrefabLength(GameObject prefab)
    {
        Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return 10f;

        float minZ = float.MaxValue;
        float maxZ = float.MinValue;

        foreach (Renderer rend in renderers)
        {
            minZ = Mathf.Min(minZ, rend.bounds.min.z);
            maxZ = Mathf.Max(maxZ, rend.bounds.max.z);
        }

        return maxZ - minZ;
    }
}
