using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodexPanel : MonoBehaviour
{
    private CombinationManager combinationManager;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip toggleAudioClip; // Audio clip to play when the codex is toggled
    [Range(0f, 1f)] [SerializeField] private float audioVolume = 1f; // Volume of the audio clip
    private AudioSource audioSource;

    private void Start()
    {
        combinationManager = FindObjectOfType<CombinationManager>();

        // Ensure the CodexPanel has an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Optional: Set AudioSource properties (if needed)
        audioSource.spatialBlend = 0f; // Make it 2D sound (not affected by position)
        audioSource.loop = false; // The sound should not loop
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            combinationManager.TogglePanel();

            // Play the audio clip when the codex is toggled
            if (toggleAudioClip != null && audioSource != null)
            {
                audioSource.PlayOneShot(toggleAudioClip, audioVolume); // Use the volume parameter
            }
            else
            {
                Debug.LogWarning("Toggle AudioClip not assigned!");
            }
        }
    }
}