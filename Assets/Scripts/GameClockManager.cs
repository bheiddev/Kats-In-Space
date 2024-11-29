// GameClockManager.cs - Attach to an empty GameObject in your first scene
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClockManager : MonoBehaviour
{
    public static GameClockManager Instance { get; private set; }

    private float remainingTime;
    private bool isRunning;
    private const float STARTING_TIME = 1800f; // 30 minutes in seconds
    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize timer
        remainingTime = STARTING_TIME;

        // Subscribe to scene loading events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if we're in a level scene or loading screen
        if (scene.name.Contains("SpaceshipLevel"))
        {
            ResumeTimer();
        }
        else if (scene.name.Contains("LoadingScreen"))
        {
            PauseTimer();
        }
    }

    private void Update()
    {
        if (isRunning && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                OnTimeExpired();
            }
        }
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        isRunning = true;
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }

    private void OnTimeExpired()
    {
        Time.timeScale = 0; // Pause game
        gameOverPanel.SetActive(true);
    }
}
