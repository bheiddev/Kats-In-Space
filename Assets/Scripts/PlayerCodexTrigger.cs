using UnityEngine;

public class PlayerCodexTrigger : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] public AudioClip codexAudioClip; // The audio clip to play when triggering "Codex"
    private AudioSource audioSource;

    private void Start()
    {
        // Ensure the player has an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Optional: Set default AudioSource properties
        audioSource.playOnAwake = false;
        audioSource.volume = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object the player collided with is tagged "Codex"
        if (other.CompareTag("Codex"))
        {
            // Play the audio clip if it is assigned
            if (codexAudioClip != null)
            {
                audioSource.PlayOneShot(codexAudioClip);
            }
            else
            {
                Debug.LogWarning("Codex AudioClip not assigned!");
            }
        }
    }
}