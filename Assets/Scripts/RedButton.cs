using UnityEngine;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;

public class RedButton : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float detectionRadius = 1.5f; // Radius within which the player can interact
    [SerializeField] private GameObject uiPrompt; // UI element to display when in range
    [SerializeField] private Animator buttonAnimator; // Animator on the button
    [SerializeField] private string buttonAnimationBool = "Switch"; // Animation bool name
    [SerializeField] private DoorController[] doors; // Array of door controllers to activate

    private bool isPlayerNear = false; // Tracks if the player is within range

    void Start()
    {
        if (uiPrompt != null)
        {
            uiPrompt.SetActive(false); // Ensure UI prompt starts hidden
        }
    }

    void Update()
    {
        DetectPlayer();

        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            PressButton();
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

        // Hide the UI prompt after the button is pressed
        if (uiPrompt != null)
        {
            uiPrompt.SetActive(false);
        }

        Debug.Log("Red button pressed! Doors activated.");
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the detection radius in the Scene view for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}