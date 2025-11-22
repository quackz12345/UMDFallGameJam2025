using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Child Objects to Toggle")]
    public GameObject[] objectsToToggle;

    [Header("Chance to Disable Each Object (0 to 1)")]
    [Range(0f, 1f)]
    public float[] disableChances;

    void Start()
    {
        ToggleObjectsRandomly();
    }

    void ToggleObjectsRandomly()
    {
        // Auto-resize chance array if needed
        if (disableChances == null || disableChances.Length != objectsToToggle.Length)
        {
            disableChances = new float[objectsToToggle.Length];
            for (int i = 0; i < disableChances.Length; i++)
                disableChances[i] = 0.5f; // default
        }

        for (int i = 0; i < objectsToToggle.Length; i++)
        {
            GameObject obj = objectsToToggle[i];
            if (obj == null) continue;

            float disableChance = disableChances[i];

            // TRUE = disable, FALSE = enable
            bool disable = Random.value < disableChance;

            obj.SetActive(!disable); // invert
        }
    }
}
