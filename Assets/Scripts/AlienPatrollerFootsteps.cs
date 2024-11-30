using UnityEngine;

public class AlienPatrollerFootsteps : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip footstepAudioClip; // The footstep sound to play
    [Header("Footstep Settings")]
    public float footstepInterval = 0.5f; // Time interval between footsteps (public for easy adjustment)
    public float maxFootstepDistance = 10f; // Max distance the footsteps are audible (public for easy adjustment)
    public float footstepVolume = 1f; // Volume of the footstep sound (public for easy adjustment)

    private AudioSource audioSource;

    private void Start()
    {
        // Ensure the patroller has an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Optional: Set default AudioSource properties
        audioSource.spatialBlend = 1f; // Make the sound 3D spatialized
        audioSource.loop = false; // Footsteps should not loop

        // Set the max distance for the audio to be heard
        audioSource.maxDistance = maxFootstepDistance;

        // Use Linear rolloff to make the sound fade gradually
        audioSource.rolloffMode = AudioRolloffMode.Linear;

        // Start playing footsteps immediately when the game starts
        InvokeRepeating(nameof(PlayFootstep), 0f, footstepInterval);
    }

    private void PlayFootstep()
    {
        // Play the footstep sound if it is assigned
        if (footstepAudioClip != null)
        {
            audioSource.PlayOneShot(footstepAudioClip, footstepVolume); // Adjust volume when playing the clip
        }
        else
        {
            Debug.LogWarning("Footstep AudioClip not assigned!");
        }
    }
}