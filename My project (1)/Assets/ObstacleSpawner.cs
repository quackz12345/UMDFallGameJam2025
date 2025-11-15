using UnityEngine;

public class ObstacleSpawner: MonoBehaviour
{
    [Header("Child Objects to Toggle")]
    public GameObject[] objectsToToggle;

    [Header("Chance to Enable Each Object (0 to 1)")]
    [Range(0f, 1f)]
    public float enableChance = 0.5f;

    void Start()
    {
        ToggleObjectsRandomly();
    }

    void ToggleObjectsRandomly()
    {
        foreach (GameObject obj in objectsToToggle)
        {
            if (obj == null) continue;

            bool enable = Random.value < enableChance;
            obj.SetActive(enable);
        }
    }
}
