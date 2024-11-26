using UnityEngine;

public class LeverPanelScript : MonoBehaviour
{
    public float detectionRadius = 2f; // Radius to detect levers
    public GameObject leverPanel;     // Reference to the Lever Panel UI

    private LeverScript nearestLever; // Tracks the nearest lever

    void Update()
    {
        CheckForNearbyLevers();
    }

    private void CheckForNearbyLevers()
    {
        nearestLever = null; // Reset nearest lever each frame
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Lever"))
            {
                LeverScript lever = hit.GetComponent<LeverScript>();
                if (lever != null && !lever.isActivated)
                {
                    nearestLever = lever;
                    break; // Exit loop when the first valid lever is found
                }
            }
        }

        // Update Lever Panel UI based on proximity to a valid lever
        if (nearestLever != null)
        {
            leverPanel.SetActive(true); // Show the Lever Panel UI
        }
        else
        {
            leverPanel.SetActive(false); // Hide the Lever Panel UI
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the detection radius in the Scene view
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}