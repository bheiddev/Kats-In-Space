using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform player;              // Reference to the player
    public float detectionRange = 4f;     // Distance within which the door opens
    public AudioClip nearbyAudioClip;     // Audio clip to play when the bool switches

    private Animator doorAnimator;        // Reference to the door's Animator
    private AudioSource audioSource;      // Audio source for playing spatial audio
    private bool wasPlayerNearby = false; // Tracks the previous state of "isPlayerNearby"
    public float audioVolume = 0.5f;

    private void Start()
    {
        // Get the Animator component attached to this object
        doorAnimator = GetComponent<Animator>();

        // Initialize or add the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure the AudioSource for 3D spatial sound
        audioSource.spatialBlend = 1.0f; // Fully spatial (3D sound)
        audioSource.playOnAwake = false; // Prevent auto-playing
        audioSource.loop = false;       // Play once per trigger
        audioSource.volume = audioVolume; // Set the volume
    }

    private void Update()
    {
        // Check if the player is within the detection range
        bool isPlayerNearby = Vector3.Distance(transform.position, player.position) <= detectionRange;

        // Update the "character_nearby" parameter in the Animator
        doorAnimator.SetBool("character_nearby", isPlayerNearby);

        // Play the audio clip when the state changes (both true -> false and false -> true)
        if (wasPlayerNearby != isPlayerNearby && nearbyAudioClip != null)
        {
            audioSource.clip = nearbyAudioClip;
            audioSource.Play();
        }

        // Update the state tracker
        wasPlayerNearby = isPlayerNearby;
    }

    // New method to activate the door explicitly
    public void ForceActivateDoor()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("character_nearby", true); // Activate door
        }
    }
}