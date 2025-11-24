using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [Header("Tutorial")]
    public GameObject tutorialPrefab;

    [Header("Levels")]
    public List<LevelPieceSet> levelPieceSets;  // Level 0 = tutorial? Or starting at 1
    public List<GameObject> endingPrefabs;      // Ending prefab per level
    public List<int> piecesPerLevel;            // How many random pieces to spawn per level

    [Header("Settings")]
    public int maxActivePieces = 6;
    public float spawnBuffer = 300f;

    private List<GameObject> activePieces = new List<GameObject>();
    private Transform player;

    private int currentLevel = -1;  
    private int piecesSpawnedThisLevel = 0;
    private bool spawningEnding = false;

    [System.Serializable]
    public class LevelPieceSet
    {
        public List<GameObject> pieces;   // Pool for that level
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        SpawnTutorial();
    }

    void Update()
    {
        if (activePieces.Count == 0) return;

        Vector3 lastEnd = GetEndPosition(activePieces[^1]);

        if (player.position.z > lastEnd.z - spawnBuffer)
        {
            SpawnNextPiece();
            CleanupOldPieces();
        }
    }

    // -----------------------------
    // TUTORIAL FIRST
    // -----------------------------
    void SpawnTutorial()
    {
        GameObject tut = Instantiate(tutorialPrefab, Vector3.zero, Quaternion.identity);
        activePieces.Add(tut);

        currentLevel = 0;  
        piecesSpawnedThisLevel = 0;
        spawningEnding = false;
    }

    // -----------------------------
    // SPAWN NEXT PIECE
    // -----------------------------
    void SpawnNextPiece()
    {
        float spawnZ = activePieces.Count > 0 ? GetEndPosition(activePieces[^1]).z : 0f;

        // Spawn ending?
        if (spawningEnding)
        {
            GameObject endPrefab = endingPrefabs[currentLevel];
            SpawnAligned(endPrefab, spawnZ);

            // Move to next level
            currentLevel++;
            piecesSpawnedThisLevel = 0;
            spawningEnding = false;

            // Finished all levels
            if (currentLevel >= levelPieceSets.Count)
            {
                Debug.Log("All levels complete!");
            }

            return;
        }

        // Check if we should spawn END next
        if (piecesSpawnedThisLevel >= piecesPerLevel[currentLevel])
        {
            spawningEnding = true;
            SpawnNextPiece();
            return;
        }

        // Spawn random piece from this level's pool
        List<GameObject> pool = levelPieceSets[currentLevel].pieces;
        GameObject prefabToSpawn = pool[Random.Range(0, pool.Count)];

        piecesSpawnedThisLevel++;
        SpawnAligned(prefabToSpawn, spawnZ);
    }

    // -----------------------------
    // SEAMLESS ALIGNMENT
    // -----------------------------
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

    // -----------------------------
    // CLEANUP
    // -----------------------------
    void CleanupOldPieces()
    {
        while (activePieces.Count > maxActivePieces)
        {
            Destroy(activePieces[0]);
            activePieces.RemoveAt(0);
        }
    }

    // -----------------------------
    // HELPERS
    // -----------------------------
    Vector3 GetEndPosition(GameObject prefab)
    {
        Transform end = prefab.transform.Find("End");
        if (end != null) return end.position;

        Vector3 p = prefab.transform.position;
        p.z += GetPrefabLength(prefab);
        return p;
    }

    float GetPrefabLength(GameObject prefab)
    {
        Renderer[] r = prefab.GetComponentsInChildren<Renderer>();
        if (r.Length == 0) return 10f;

        float minZ = float.MaxValue;
        float maxZ = float.MinValue;

        foreach (Renderer rr in r)
        {
            minZ = Mathf.Min(minZ, rr.bounds.min.z);
            maxZ = Mathf.Max(maxZ, rr.bounds.max.z);
        }

        return maxZ - minZ;
    }
}