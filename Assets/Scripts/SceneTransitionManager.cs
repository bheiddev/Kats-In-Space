using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class SceneTransitionManager : MonoBehaviour
{
    public Image fadeImage;
    public string loadingScreenSceneName = "LoadingScreen";
    public float fadeDuration = 1f;
    private bool isTransitioning = false;

    private static SceneTransitionManager instance;

    [Header("Game Clock Settings")]
    public float gameClockTime = 1800f; // Start at 30 minutes (1800 seconds)
    public TextMeshProUGUI gameClockText;
    private bool isClockRunning = false;

    private void Awake()
    {
        // Ensure only one instance exists and it persists across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize the game clock UI (if available)
        if (gameClockText != null)
        {
            UpdateGameClockUI();
        }
    }

    private void Update()
    {
        // Countdown the game clock if it's running
        if (isClockRunning)
        {
            gameClockTime -= Time.deltaTime;

            if (gameClockTime <= 0)
            {
                gameClockTime = 0;
                isClockRunning = false;
                Debug.Log("Game clock reached 0!");
            }

            // Update the clock UI
            UpdateGameClockUI();
        }
    }

    private void UpdateGameClockUI()
    {
        if (gameClockText != null)
        {
            int minutes = Mathf.FloorToInt(gameClockTime / 60);
            int seconds = Mathf.FloorToInt(gameClockTime % 60);
            gameClockText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    public void StartSceneTransition()
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionToNextScene());
        }
    }

    private IEnumerator TransitionToNextScene()
    {
        isTransitioning = true;

        yield return StartCoroutine(FadeToBlack());
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Pause the game clock during the loading screen
        if (currentSceneName.StartsWith("LoadingScreen"))
        {
            isClockRunning = false;
        }

        SceneManager.LoadScene(loadingScreenSceneName);
        yield return new WaitForSeconds(10f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        // Resume the game clock in SpaceShip levels
        string nextSceneName = SceneManager.GetActiveScene().name;
        if (nextSceneName.StartsWith("SpaceshipLevel"))
        {
            isClockRunning = true;
        }

        isTransitioning = false;
    }

    private IEnumerator FadeToBlack()
    {
        float elapsed = 0f;
        Color startColor = fadeImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsed < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, endColor, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = endColor;
    }
}