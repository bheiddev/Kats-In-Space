using UnityEngine;
using UnityEngine.UI;

public class FadeInController : MonoBehaviour
{
    public Image fadeImage; // Reference to the black Image UI component
    public float fadeDuration = 2f; // Duration of the fade in seconds

    private void Start()
    {
        if (fadeImage != null)
        {
            // Start fully opaque
            fadeImage.color = new Color(0f, 0f, 0f, 1f);
            // Begin the fade-in effect
            StartCoroutine(FadeFromBlack());
        }
        else
        {
            Debug.LogWarning("Fade image is not assigned.");
        }
    }

    private System.Collections.IEnumerator FadeFromBlack()
    {
        float elapsedTime = 0f;

        // Gradually fade out the black image
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        // Ensure it's fully transparent at the end
        fadeImage.color = new Color(0f, 0f, 0f, 0f);
    }
}