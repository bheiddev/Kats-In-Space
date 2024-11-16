using UnityEngine;

public class SceneTransitionTrigger : MonoBehaviour
{
    private SceneTransitionManager transitionManager;

    private void Start()
    {
        transitionManager = FindObjectOfType<SceneTransitionManager>();
        if (transitionManager == null)
        {
            Debug.LogError("SceneTransitionManager not found in the scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && transitionManager != null)
        {
            transitionManager.StartSceneTransition();
        }
    }
}