using UnityEngine;

public class GameClockManager : MonoBehaviour
{
    public static GameClockManager Instance; // Singleton instance

    private float elapsedTime = 0f; // Tracks total elapsed time
    private bool isRunning = false; // Tracks if the clock is running

    void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist through scenes
        }
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime; // Increment timer when running
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void StopTimer()
    {
        isRunning = false;
        Debug.Log($"Final Game Time: {elapsedTime:F2} seconds");
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}