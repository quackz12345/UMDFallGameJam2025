using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Child Objects to Toggle")]
    public GameObject[] objectsToToggle;

    [Header("Chance to Disable Each Object (0 to 1)")]
    [Range(0f, 1f)]
    public float[] disableChances;

    // ------------------------------
    // Auto-fill button for Inspector
    // ------------------------------
    [ContextMenu("Auto Fill From Children")]
    void AutoFillFromChildren()
    {
        int count = transform.childCount;
        objectsToToggle = new GameObject[count];

        for (int i = 0; i < count; i++)
            objectsToToggle[i] = transform.GetChild(i).gameObject;

        // Also resize disable chances to match
        disableChances = new float[count];
        for (int i = 0; i < count; i++)
            disableChances[i] = 0.5f;

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

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
                disableChances[i] = 0.5f;
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