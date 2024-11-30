using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
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
}