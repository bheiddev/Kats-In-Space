using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public Image fadeImage; // Reference to the black Image UI component
    public float fadeDuration = 1f; // Duration of the fade in seconds

    public void StartGame()
    {
        Debug.Log("Button is being pressed");
        StartCoroutine(FadeToBlackAndLoadNextScene());
    }

    private IEnumerator FadeToBlackAndLoadNextScene()
    {
        float elapsedTime = 0f;

        // Gradually fade in the black image
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        // Start loading the next scene asynchronously
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneIndex);
        asyncLoad.allowSceneActivation = true; // Scene activation when ready
    }
}