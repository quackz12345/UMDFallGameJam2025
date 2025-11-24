using UnityEngine;  // needed for MonoBehaviour, Collider, etc.

public class LevelTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            LevelManager lm = FindObjectOfType<LevelManager>();
            if (lm != null)
            {
                lm.ApplyNextLevelSetting(); // Apply the next in the list
            }

            triggered = true;
        }
    }
}
