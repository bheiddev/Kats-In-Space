using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [Header("Footstep Audio Clips")]
    public AudioClip[] footstepSounds;

    [Header("Time Between Footsteps")]
    public float minTimeBetweenFootsteps = 0.3f;
    public float maxTimeBetweenFootsteps = 0.6f;

    private AudioSource audioSource;
    private bool isWalking = false;
    private float timeSinceLastFootstep;
    private Collider characterCollider;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        characterCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (isWalking)
        {
            // Calculate the origin of the raycast as the center of the character's collider
            Vector3 rayOrigin = characterCollider.bounds.center;

            // Check if there's ground below the character using a raycast
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hitInfo, characterCollider.bounds.extents.y + 0.1f))
            {
                if (Time.time - timeSinceLastFootstep >= Random.Range(minTimeBetweenFootsteps, maxTimeBetweenFootsteps))
                {
                    // Play a random footstep sound
                    AudioClip footstepSound = footstepSounds[Random.Range(0, footstepSounds.Length)];
                    audioSource.PlayOneShot(footstepSound);

                    // Update the time of the last footstep
                    timeSinceLastFootstep = Time.time;
                }
            }
        }
    }

    public void StartWalking()
    {
        isWalking = true;
    }

    public void StopWalking()
    {
        isWalking = false;
    }
}