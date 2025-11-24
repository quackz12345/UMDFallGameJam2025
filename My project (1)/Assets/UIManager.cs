using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject timerUI;
    public TMP_Text endTimeText;

    private void Awake()
    {
        Instance = this;
        endTimeText.gameObject.SetActive(false);
    }

    public void ShowEndScreen(float time)
    {
        if (timerUI != null)
            timerUI.SetActive(false);

        if (endTimeText != null)
        {
            endTimeText.gameObject.SetActive(true);

            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            endTimeText.text = $"Your Time: {minutes:00}:{seconds:00}";
        }
    }
}
