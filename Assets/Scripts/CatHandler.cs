using UnityEngine;

public class CatHandler : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRadius = 2f;   // Radius of the detection area
    public float rayHeightOffset = 1f;   // Adjust to center the detection area on the player's Y axis

    [Header("UI Settings")]
    [SerializeField] private GameObject greyCatUI; // UI element showing a greyed-out cat
    [SerializeField] private GameObject colorCatUI; // UI element showing a collected colored cat
    [SerializeField] private GameObject catUIPanel; // UI panel to show when near a cat

    [Header("VFX Settings")]
    [SerializeField] private GameObject smokeVFXPrefab; // Reference to the smoke particle prefab

    [Header("Audio Settings")]
    [SerializeField] private AudioClip interactAudioClip; // Audio clip for cat interaction
    private AudioSource audioSource; // Reference to the audio source

    public bool IsCatCollected { get; private set; } = false; // Flag to check if the cat has been collected

    private bool isNearCat = false; // Tracks if the player is near a cat

    void Start()
    {
        // Initialize UI states
        if (greyCatUI != null)
        {
            greyCatUI.SetActive(true); // Grey cat visible at start
        }
        if (colorCatUI != null)
        {
            colorCatUI.SetActive(false); // Color'd cat hidden at start
        }
        if (catUIPanel != null)
        {
            catUIPanel.SetActive(false); // Hide interaction panel at start
        }

        // Get or add the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure the AudioSource
        audioSource.playOnAwake = false; // Don't play on start
        audioSource.spatialBlend = 1.0f; // Fully spatial for 3D sound
    }

    void Update()
    {
        DetectCats();

        if (isNearCat && Input.GetKeyDown(KeyCode.F))
        {
            InteractWithCat();
        }
    }

    void DetectCats()
    {
        // Center of the detection area
        Vector3 detectionCenter = transform.position + Vector3.up * rayHeightOffset;

        // Perform an overlap sphere to detect objects in all directions
        Collider[] hitColliders = Physics.OverlapSphere(detectionCenter, detectionRadius);

        isNearCat = false; // Reset detection flag

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Cat"))
            {
                isNearCat = true;
                if (catUIPanel != null)
                {
                    catUIPanel.SetActive(true); // Show interaction UI
                }
                break;
            }
        }

        if (!isNearCat && catUIPanel != null)
        {
            catUIPanel.SetActive(false); // Hide interaction UI if no cat is nearby
        }
    }

    void InteractWithCat()
    {
        // Center of the detection area
        Vector3 detectionCenter = transform.position + Vector3.up * rayHeightOffset;

        // Perform an overlap sphere to detect objects in all directions
        Collider[] hitColliders = Physics.OverlapSphere(detectionCenter, detectionRadius);

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Cat"))
            {
                // Play interaction audio
                if (interactAudioClip != null && audioSource != null)
                {
                    audioSource.clip = interactAudioClip;
                    audioSource.Play();
                }

                // Instantiate the smoke VFX at the cat's position
                if (smokeVFXPrefab != null)
                {
                    GameObject smokeVFX = Instantiate(smokeVFXPrefab, hit.transform.position, Quaternion.identity);
                    Destroy(smokeVFX, 2f); // Destroy the particle system after 2 seconds
                }

                Destroy(hit.gameObject);
                Debug.Log("Picked up a cat friend!");

                // Set the flag to indicate the cat has been collected
                IsCatCollected = true;

                // Update UI
                UpdateCatUI();

                // Hide the interaction UI panel
                if (catUIPanel != null)
                {
                    catUIPanel.SetActive(false);
                }

                isNearCat = false; // Update state
                return; // Exit after picking up a cat
            }
        }
    }

    private void UpdateCatUI()
    {
        // Toggle the UI states based on whether a cat is collected
        if (greyCatUI != null)
        {
            greyCatUI.SetActive(!IsCatCollected); // Grey cat becomes inactive when collected
        }
        if (colorCatUI != null)
        {
            colorCatUI.SetActive(IsCatCollected); // Color'd cat becomes active when collected
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the detection area in the Scene view for debugging
        Gizmos.color = Color.blue;
        Vector3 detectionCenter = transform.position + Vector3.up * rayHeightOffset;
        Gizmos.DrawWireSphere(detectionCenter, detectionRadius);
    }
}