using UnityEngine;
using Cinemachine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class RedButton : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float detectionRadius = 1.5f; // Radius within which the player can interact
    [SerializeField] private GameObject uiPrompt; // UI element to display when in range
    [SerializeField] private Animator buttonAnimator; // Animator on the button
    [SerializeField] private string buttonAnimationBool = "Switch"; // Animation bool name
    [SerializeField] private DoorController[] doors; // Array of door controllers to activate
    [SerializeField] private CinemachineFreeLook freeLookCamera; // Reference to the Cinemachine FreeLook camera
    private GameClockManager gameClockManager;

    private bool isPlayerNear = false; // Tracks if the player is within range
    private bool isButtonActive = true; // Tracks if the button can be interacted with

    [Header("Game Completion")]
    [SerializeField] private CinemachineVirtualCamera completionCamera;
    [SerializeField] private GameObject gameUIPanel;
    [SerializeField] private GameObject gameCompletePanel;
    [SerializeField] private TextMeshProUGUI completionTimeText;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float skyboxRotationSpeed = 1f;
    private Material skyboxMaterial;

    void Start()
    {
        gameClockManager = FindObjectOfType<GameClockManager>();
        skyboxMaterial = RenderSettings.skybox;
        if (gameCompletePanel != null) gameCompletePanel.SetActive(false);

        if (uiPrompt != null)
        {
            uiPrompt.SetActive(false); // Ensure UI prompt starts hidden
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

        if (gameClockManager != null) gameClockManager.PauseTimer();

        GameCompletionSequence();

        // Hide the UI prompt and deactivate the button
        if (uiPrompt != null)
        {
            uiPrompt.SetActive(false);
        }

        isButtonActive = false; // Disable the button to prevent reactivation
        Debug.Log("Red button pressed! Doors activated.");
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the detection radius in the Scene view for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void GameCompletionSequence()
    {
        freeLookCamera.Priority = 0;
        completionCamera.Priority = 11;

        // Update UI
        gameUIPanel.SetActive(false);
        gameCompletePanel.SetActive(true);

        // Set completion time
        float remainingTime = gameClockManager.GetRemainingTime();
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        completionTimeText.text = $"Time: {minutes:00}:{seconds:00}";

        // Start skybox rotation
        StartCoroutine(RotateSkybox());
    }

    private IEnumerator RotateSkybox()
    {
        // Set initial rotation
        skyboxMaterial.SetFloat("_Rotation", 252f);

        // Small delay before starting rotation
        yield return new WaitForSeconds(0.5f);

        float currentRotation = 252f;

        while (true)
        {
            currentRotation += skyboxRotationSpeed * Time.deltaTime;
            skyboxMaterial.SetFloat("_Rotation", currentRotation);
            yield return null;
        }
    }
}