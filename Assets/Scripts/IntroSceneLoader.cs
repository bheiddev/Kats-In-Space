using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroSceneLoader : MonoBehaviour
{
    public float delayBeforeLoading = 30f; // Delay in seconds before loading the next scene

    private void Start()
    {
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    private IEnumerator LoadNextSceneAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayBeforeLoading);

        // Start loading the next scene asynchronously
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneIndex);
        asyncLoad.allowSceneActivation = true; // Scene activation when ready
    }
}