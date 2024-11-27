using UnityEngine;
using TMPro;

public class GameClockUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI timerText;

    private static GameClockUI instance;

    void Awake()
    {
        // Ensure only one instance of GameClockUI exists
        if (instance != null)
        {
            Destroy(gameObject); // Destroy duplicates
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Persist through scenes
    }

    void Update()
    {
        if (GameClockManager.Instance != null)
        {
            float elapsedTime = GameClockManager.Instance.GetElapsedTime();
            timerText.text = FormatTime(elapsedTime);
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}