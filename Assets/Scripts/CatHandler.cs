using UnityEngine;

public class CatHandler : MonoBehaviour
{
    public float detectionRadius = 2f;   // Radius of the detection area
    public float rayHeightOffset = 1f;   // Adjust to center the detection area on the player's Y axis

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            InteractWithCat();
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
            // Pick up a cat friend
            if (hit.CompareTag("Cat"))
            {
                Destroy(hit.gameObject);
                Debug.Log("Picked up a cat friend!");
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
