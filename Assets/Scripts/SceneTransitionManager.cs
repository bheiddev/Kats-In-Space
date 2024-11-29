using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public Image fadeImage;
    public string loadingScreenSceneName = "LoadingScreen";
    public float fadeDuration = 1f;
    public float loadingScreenDuration = 10f;

    private bool isTransitioning = false;
    private static SceneTransitionManager instance;
    private int originSceneIndex;

    private void Awake()
    {
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

    public void StartSceneTransition()
    {
        if (!isTransitioning)
        {
            originSceneIndex = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(TransitionToNextScene());
        }
    }

    private IEnumerator TransitionToNextScene()
    {
        isTransitioning = true;

        // Fade to black
        yield return StartCoroutine(FadeToBlack());

        // Store next scene index
        int nextSceneIndex = originSceneIndex + 1;

        // Load loading screen
        SceneManager.LoadScene(loadingScreenSceneName);

        // Wait in loading screen
        yield return new WaitForSeconds(loadingScreenDuration);

        // Start loading next scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneIndex);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
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