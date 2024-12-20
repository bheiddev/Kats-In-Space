using UnityEngine;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;

public class TerminalHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject uiPanel; // Original UI for terminal interaction
    [SerializeField] private GameObject collisionUIPanel; // New UI for collision
    [SerializeField] private GameObject codexUIPanel; // UI for the Codex
    [SerializeField] private CinemachineFreeLook thirdPersonCamera;
    [SerializeField] private CinemachineVirtualCamera firstPersonCamera;
    [SerializeField] private Transform playerHead;
    [SerializeField] private GameObject characterModel;

    [Header("Terminal Detection")]
    [SerializeField] private float raycastHeightOffset = 1f;
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private LayerMask terminalLayer;

    [Header("Audio Settings")]
    public AudioClip terminalInteractionClip;

    private AudioSource audioSource;

    // State tracking
    private bool isUsingTerminal;
    private GameObject currentTerminal;
    private bool wasMouseVisible;

    private bool isCollidingWithTerminal = false;

    private List<int> enteredCombination = new List<int>();

    public List<int> GetEnteredCombination()
    {
        return enteredCombination;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (firstPersonCamera != null)
        {
            firstPersonCamera.gameObject.SetActive(false);
        }

        uiPanel.SetActive(false);          // Ensure the interaction UI starts hidden
        collisionUIPanel.SetActive(false); // Ensure the collision UI starts hidden
        if (codexUIPanel != null)
        {
            codexUIPanel.SetActive(false); // Ensure the Codex UI starts hidden
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isUsingTerminal)
            {
                TryInteractWithTerminal();
            }
            else
            {
                ExitTerminal();
            }
        }
    }

    public void AddEnteredNumber(int number)
    {
        if (enteredCombination.Count < 4)
        {
            enteredCombination.Add(number);
        }
    }

    private void TryInteractWithTerminal()
    {
        // Center of the detection sphere
        Vector3 detectionCenter = characterModel.transform.position + (Vector3.up * raycastHeightOffset);

        // Find all objects within the interaction radius
        Collider[] hitColliders = Physics.OverlapSphere(detectionCenter, interactionRadius, terminalLayer);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Terminal"))
            {
                currentTerminal = hitCollider.gameObject;
                EnterTerminal();
                return;
            }
        }
    }

    private void EnterTerminal()
    {
        isUsingTerminal = true;

        // Play interaction sound
        PlayAudioClip();

        // Switch cameras
        thirdPersonCamera.gameObject.SetActive(false);
        firstPersonCamera.gameObject.SetActive(true);
        firstPersonCamera.transform.position = playerHead.position;

        // Show the original UI
        uiPanel.SetActive(true);

        // Lock cursor for terminal interaction
        wasMouseVisible = Cursor.visible;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Disable collision UI while interacting
        collisionUIPanel.SetActive(false);

        // Display Codex UI for 2 seconds
        if (codexUIPanel != null)
        {
            StartCoroutine(ShowCodexUI());
        }
    }

    private void ExitTerminal()
    {
        isUsingTerminal = false;

        // Play interaction sound
        PlayAudioClip();

        // Hide the original UI and restore cursor state
        uiPanel.SetActive(false);
        Cursor.visible = wasMouseVisible;
        Cursor.lockState = CursorLockMode.Locked;

        // Switch cameras back
        firstPersonCamera.gameObject.SetActive(false);
        thirdPersonCamera.gameObject.SetActive(true);

        currentTerminal = null;
        codexUIPanel.SetActive(false);

        // Re-enable collision UI if still colliding
        if (isCollidingWithTerminal)
        {
            collisionUIPanel.SetActive(true);
        }
    }

    private void PlayAudioClip()
    {
        if (terminalInteractionClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(terminalInteractionClip);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Terminal"))
        {
            isCollidingWithTerminal = true;

            // Show the collision-based UI
            collisionUIPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Terminal"))
        {
            isCollidingWithTerminal = false;

            // Hide the collision-based UI
            collisionUIPanel.SetActive(false);
            
        }
    }

    private IEnumerator ShowCodexUI()
    {
        codexUIPanel.SetActive(true); // Activate Codex UI
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        codexUIPanel.SetActive(false); // Deactivate Codex UI
    }

    private void OnDrawGizmosSelected()
    {
        if (characterModel != null)
        {
            Vector3 detectionCenter = characterModel.transform.position + (Vector3.up * raycastHeightOffset);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(detectionCenter, interactionRadius);
        }
    }
}