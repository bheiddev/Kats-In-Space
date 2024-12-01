using UnityEngine;
using Cinemachine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class RedButton : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float detectionRadius = 1.5f;
    [SerializeField] private GameObject uiPrompt;
    [SerializeField] private Animator buttonAnimator;
    [SerializeField] private string buttonAnimationBool = "Switch";
    [SerializeField] private DoorController[] doors;
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    private GameClockManager gameClockManager;

    [Header("Audio")]
    [SerializeField] private AudioSource backgroundMusic; // Reference to background music source
    [SerializeField] private AudioSource winnerMusic; // Reference to winner music source

    private bool isPlayerNear = false;
    private bool isButtonActive = true;

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
            uiPrompt.SetActive(false);
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
                    uiPrompt.SetActive(true);
                }
                return;
            }
        }

        if (!isPlayerNear && uiPrompt != null)
        {
            uiPrompt.SetActive(false);
        }
    }

    void PressButton()
    {
        if (!isButtonActive) return;

        // Unlock cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Handle audio
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }
        if (winnerMusic != null)
        {
            winnerMusic.Play();
        }

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
                door.ForceActivateDoor();
            }
        }

        if (gameClockManager != null) gameClockManager.PauseTimer();

        GameCompletionSequence();

        // Hide the UI prompt and deactivate the button
        if (uiPrompt != null)
        {
            uiPrompt.SetActive(false);
        }

        isButtonActive = false;
        Debug.Log("Red button pressed! Doors activated.");
    }

    private void OnDrawGizmosSelected()
    {
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
        skyboxMaterial.SetFloat("_Rotation", 252f);

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