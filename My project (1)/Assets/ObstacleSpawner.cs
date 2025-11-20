using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Child Objects to Toggle")]
    public GameObject[] objectsToToggle;

    [Header("Chance for Each Object (0 to 1)")]
    [Range(0f, 1f)]
    public float[] enableChances;

    void Start()
    {
        ToggleObjectsRandomly();
    }

    void ToggleObjectsRandomly()
    {
        // Auto-resize chance array if needed
        if (enableChances == null || enableChances.Length != objectsToToggle.Length)
        {
            enableChances = new float[objectsToToggle.Length];
            for (int i = 0; i < enableChances.Length; i++)
                enableChances[i] = 0.5f; // default value
        }

        for (int i = 0; i < objectsToToggle.Length; i++)
        {
            GameObject obj = objectsToToggle[i];
            if (obj == null) continue;

            float chance = enableChances[i];
            bool enable = Random.value < chance;

            obj.SetActive(enable);
        }
    }
}