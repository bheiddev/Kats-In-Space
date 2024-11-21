using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class TerminalHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private CinemachineFreeLook thirdPersonCamera;
    [SerializeField] private CinemachineVirtualCamera firstPersonCamera;
    [SerializeField] private Transform playerHead;
    [SerializeField] private GameObject characterModel; // Reference to the character model

    [Header("Terminal Detection")]
    [SerializeField] private float raycastHeightOffset = 1f;
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private LayerMask terminalLayer;

    // State tracking
    private bool isUsingTerminal;
    private GameObject currentTerminal;
    private bool wasMouseVisible;

    private void Start()
    {
        // Ensure FP camera starts disabled
        if (firstPersonCamera != null)
        {
            firstPersonCamera.gameObject.SetActive(false);
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

    private void TryInteractWithTerminal()
    {
        // Calculate ray origin with vertical offset
        Vector3 rayOrigin = characterModel.transform.position + (Vector3.up * raycastHeightOffset);

        // Create a ray from the character model's forward direction
        Ray ray = new Ray(rayOrigin, characterModel.transform.forward);
        RaycastHit hitInfo;

        // Perform the raycast
        if (Physics.Raycast(ray, out hitInfo, interactionDistance, terminalLayer))
        {
            if (hitInfo.collider.CompareTag("Terminal"))
            {
                currentTerminal = hitInfo.collider.gameObject;
                EnterTerminal();
            }
        }
    }

    private void EnterTerminal()
    {
        isUsingTerminal = true;

        // Switch cameras
        thirdPersonCamera.gameObject.SetActive(false);
        firstPersonCamera.gameObject.SetActive(true);

        // Position first person camera at player's head
        firstPersonCamera.transform.position = playerHead.position;

        // Show UI and unlock cursor
        uiPanel.SetActive(true);
        wasMouseVisible = Cursor.visible;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void ExitTerminal()
    {
        isUsingTerminal = false;

        // Hide UI and restore cursor state
        uiPanel.SetActive(false);
        Cursor.visible = wasMouseVisible;
        Cursor.lockState = CursorLockMode.Locked;

        // Switch cameras back
        firstPersonCamera.gameObject.SetActive(false);
        thirdPersonCamera.gameObject.SetActive(true);

        currentTerminal = null;
    }

    private void OnDrawGizmosSelected()
    {
        // Visual debugging of interaction ray
        if (characterModel != null)
        {
            // Calculate ray origin with vertical offset
            Vector3 rayOrigin = characterModel.transform.position + (Vector3.up * raycastHeightOffset);

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(rayOrigin, characterModel.transform.forward * interactionDistance);
        }
    }
}