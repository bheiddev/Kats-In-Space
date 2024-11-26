using UnityEngine;

public class CatHandler : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRadius = 2f;   // Radius of the detection area
    public float rayHeightOffset = 1f;   // Adjust to center the detection area on the player's Y axis

    [Header("UI Settings")]
    [SerializeField] private GameObject catUIPanel; // UI panel to show when near a cat

    public bool IsCatCollected { get; private set; } = false; // Flag to check if the cat has been collected

    private bool isNearCat = false; // Tracks if the player is near a cat

    void Start()
    {
        // Ensure the cat UI panel is hidden at the start
        if (catUIPanel != null)
        {
            catUIPanel.SetActive(false);
        }
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

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Cat"))
            {
                isNearCat = true;
                catUIPanel.SetActive(true);
                break;
            }
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
                Destroy(hit.gameObject);
                Debug.Log("Picked up a cat friend!");

                // Set the flag to indicate the cat has been collected
                IsCatCollected = true;

                // Hide the UI panel since the cat was picked up
                if (catUIPanel != null)
                {
                    catUIPanel.SetActive(false);
                }

                isNearCat = false; // Update the state
                return;  // Exit after picking up a cat
            }
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