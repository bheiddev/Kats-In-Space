using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartOnPlayerCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Restart the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}