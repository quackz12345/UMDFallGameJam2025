using UnityEngine;

public class EndTriggerMessage : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        // Stop the timer
        if (TimerText.Instance != null)
            TimerText.Instance.StopTimer();

        // Tell UI Manager to show the end screen
        if (UIManager.Instance != null)
        {
            float time = TimerText.Instance != null ? TimerText.Instance.GetTime() : 0f;
            UIManager.Instance.ShowEndScreen(time);
        }
    }
}
