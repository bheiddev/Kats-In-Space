using UnityEngine;
using System.Collections;
using UnityEngine.UI;  // Make sure this is included for working with Image

public class SceneTransitionTrigger : MonoBehaviour
{
    private SceneTransitionManager transitionManager;
    public Image fadeImage; // Reference to the fade image
    public float fadeDuration = 1f; // Duration for fading to black

    private void Start()
    {
        transitionManager = FindObjectOfType<SceneTransitionManager>();
        if (transitionManager == null)
        {
            Debug.LogError("SceneTransitionManager not found in the scene!");
        }

        if (fadeImage == null)
        {
            Debug.LogError("FadeImage not assigned! Please assign an Image component for fading.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && transitionManager != null)
        {
            // Start the fade to black before starting the scene transition
            StartCoroutine(FadeToBlackAndTransition());
        }
    }

    private IEnumerator FadeToBlackAndTransition()
    {
        // Fade to black
        yield return StartCoroutine(FadeToBlack());

        // After fade is complete, start the scene transition
        transitionManager.StartSceneTransition();
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

        fadeImage.color = endColor;  // Ensure the final color is fully black
    }
}