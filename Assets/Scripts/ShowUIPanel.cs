using UnityEngine;

public class ShowUIPanel : MonoBehaviour
{
    public float displayTime = 5f; // Time in seconds to display the panel

    private void Start()
    {
        // Start the coroutine to hide the panel after a delay
        StartCoroutine(HidePanelAfterDelay());
    }

    private System.Collections.IEnumerator HidePanelAfterDelay()
    {
        // Wait for the specified display time
        yield return new WaitForSeconds(displayTime);

        // Set the panel to inactive
        gameObject.SetActive(false);
    }
}