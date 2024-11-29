using UnityEngine;
using TMPro;

public class GameClockUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private void Update()
    {
        if (GameClockManager.Instance != null)
        {
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        float timeInSeconds = GameClockManager.Instance.GetRemainingTime();
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}