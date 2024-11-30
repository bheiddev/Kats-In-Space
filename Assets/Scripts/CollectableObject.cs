using UnityEngine;

// Place this on collectible objects in the level
public class CollectibleObject : MonoBehaviour
{
    public static event System.Action<CollectibleObject> OnCollected;

    [Header("VFX Settings")]
    [SerializeField] private GameObject smokeVFXPrefab; // Reference to the smoke particle prefab

    [Header("Audio Settings")]
    [SerializeField] private AudioClip collectAudioClip; // Audio clip to play when collected
    private AudioSource audioSource;


    private void Start()
    {
        // Ensure there's an AudioSource on the object
        audioSource = GetComponent<AudioSource>();

        // Log the result
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource not found, adding one...");
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Debug.Log("AudioSource successfully found.");
        }

        // Ensure the AudioSource is not playing and set the volume
        audioSource.volume = 1f;
        audioSource.playOnAwake = false; // Don't play automatically on Awake

        Debug.Log("AudioSource Set Up: " + audioSource.isPlaying + ", Volume: " + audioSource.volume);
    }

    private void Update()
    {
        // Rotate the object by 0.25 degrees on the Y-axis every frame
        transform.Rotate(0, 0.25f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Trigger the particle effect
            if (smokeVFXPrefab != null)
            {
                GameObject smokeVFX = Instantiate(smokeVFXPrefab, transform.position, Quaternion.identity);
                Destroy(smokeVFX, 2f); // Destroy the particle system after 2 seconds
            }

            // Play the collection sound if available
            if (collectAudioClip != null)
            {
                Debug.Log("Playing collection sound...");
                audioSource.PlayOneShot(collectAudioClip);
            }

            // Trigger the collected event and destroy the object
            OnCollected?.Invoke(this);
            Destroy(gameObject);
        }
    }
}