using UnityEngine;
using TMPro;

public class TimerText : MonoBehaviour
{
    public static TimerText Instance;   // Singleton for global access

    public TMP_Text timerText;          // UI text in scene
    private float timer = 0f;
    public bool isRunning = true;

    void Awake()
    {
        // Assign singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // ensure only one
    }

    void Update()
    {
        if (!isRunning) return;

        timer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void ResetTimer()
    {
        timer = 0f;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public float GetTime()
    {
        return timer;
    }
}
