using UnityEngine;
using Cinemachine;
using System.Collections;

public class RedButton : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float detectionRadius = 1.5f; // Radius within which the player can interact
    [SerializeField] private GameObject uiPrompt; // UI element to display when in range
    [SerializeField] private Animator buttonAnimator; // Animator on the button
    [SerializeField] private string buttonAnimationBool = "Switch"; // Animation bool name
    [SerializeField] private DoorController[] doors; // Array of door controllers to activate
    [SerializeField] private CinemachineFreeLook freeLookCamera; // Reference to the Cinemachine FreeLook camera

    private bool isPlayerNear = false; // Tracks if the player is within range
    private bool isButtonActive = true; // Tracks if the button can be interacted with

    private float originalNoiseAmplitude;
    private float originalNoiseFrequency;

    void Start()
    {
        if (uiPrompt != null)
        {
            uiPrompt.SetActive(false); // Ensure UI prompt starts hidden
        }

        if (freeLookCamera != null)
        {
            // Store the original noise values for restoration
            CinemachineBasicMultiChannelPerlin topRigNoise = freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (topRigNoise != null)
            {
                originalNoiseAmplitude = topRigNoise.m_AmplitudeGain;
                originalNoiseFrequency = topRigNoise.m_FrequencyGain;
            }
        }
    }

    void Update()
    {
        if (isButtonActive)
        {
            DetectPlayer();

            if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
            {
                PressButton();
            }
        }
    }

    void DetectPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        isPlayerNear = false;

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                isPlayerNear = true;

                if (uiPrompt != null)
                {
                    uiPrompt.SetActive(true); // Show UI prompt when player is near
                }
                return;
            }
        }

        if (!isPlayerNear && uiPrompt != null)
        {
            uiPrompt.SetActive(false); // Hide UI prompt when player is out of range
        }
    }

    void PressButton()
    {
        if (!isButtonActive) return; // Prevent pressing an inactive button

        // Activate the button's animation
        if (buttonAnimator != null)
        {
            buttonAnimator.SetBool(buttonAnimationBool, true);
        }

        // Activate all assigned door controllers
        foreach (DoorController door in doors)
        {
            if (door != null)
            {
                door.ForceActivateDoor(); // Explicitly activate the door
            }
        }

        // Add explosion camera shake
        if (freeLookCamera != null)
        {
            StartCoroutine(ShakeCamera(3f, 3f, 3f));
        }

        // Hide the UI prompt and deactivate the button
        if (uiPrompt != null)
        {
            uiPrompt.SetActive(false);
        }

        isButtonActive = false; // Disable the button to prevent reactivation
        Debug.Log("Red button pressed! Doors activated.");
    }

    private IEnumerator ShakeCamera(float amplitude, float frequency, float duration)
    {
        if (freeLookCamera == null) yield break;

        // Apply noise to all rigs
        for (int i = 0; i < 3; i++)
        {
            CinemachineBasicMultiChannelPerlin rigNoise = freeLookCamera.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (rigNoise != null)
            {
                rigNoise.m_AmplitudeGain = amplitude;
                rigNoise.m_FrequencyGain = frequency;
            }
        }

        yield return new WaitForSeconds(duration);

        // Reset noise to original values
        for (int i = 0; i < 3; i++)
        {
            CinemachineBasicMultiChannelPerlin rigNoise = freeLookCamera.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (rigNoise != null)
            {
                rigNoise.m_AmplitudeGain = originalNoiseAmplitude;
                rigNoise.m_FrequencyGain = originalNoiseFrequency;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the detection radius in the Scene view for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}