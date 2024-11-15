using UnityEngine;

public class PowerCellHandler : MonoBehaviour
{
    public bool hasGreenPowerCell = false;
    public bool hasRedPowerCell = false;
    public bool hasYellowPowerCell = false;
    public float detectionRadius = 2f;   // Radius of the detection area
    public float rayHeightOffset = 1f;   // Adjust to center the detection area on the player's Y axis

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            InteractWithPowerCellOrContainer();
        }
    }

    void InteractWithPowerCellOrContainer()
    {
        // Center of the detection area
        Vector3 detectionCenter = transform.position + Vector3.up * rayHeightOffset;

        // Perform an overlap sphere to detect objects in all directions
        Collider[] hitColliders = Physics.OverlapSphere(detectionCenter, detectionRadius);

        foreach (Collider hit in hitColliders)
        {
            // Pick up a power cell
            if (hit.CompareTag("GreenPowerCell") && !hasGreenPowerCell)
            {
                Destroy(hit.gameObject);
                hasGreenPowerCell = true;
                Debug.Log("Picked up Green Power Cell!");
                return;  // Exit after picking up a cell
            }
            else if (hit.CompareTag("YellowPowerCell") && !hasYellowPowerCell)
            {
                Destroy(hit.gameObject);
                hasYellowPowerCell = true;
                Debug.Log("Picked up Yellow Power Cell!");
                return;  // Exit after picking up a cell
            }
            else if (hit.CompareTag("RedPowerCell") && !hasRedPowerCell)
            {
                Destroy(hit.gameObject);
                hasRedPowerCell = true;
                Debug.Log("Picked up Red Power Cell!");
                return;  // Exit after picking up a cell
            }
            // Place the power cell in a container if nearby and the player has the matching power cell
            else if (hit.CompareTag("PowerCellContainer"))
            {
                PowerCellContainer container = hit.GetComponent<PowerCellContainer>();
                if (container != null && !container.isPowered)
                {
                    switch (container.containerColor)
                    {
                        case PowerCellContainer.PowerCellColor.Green:
                            if (hasGreenPowerCell)
                            {
                                container.PowerOn();
                                hasGreenPowerCell = false;
                                Debug.Log("Powered Green Power Cell Container!");
                            }
                            break;
                        case PowerCellContainer.PowerCellColor.Yellow:
                            if (hasYellowPowerCell)
                            {
                                container.PowerOn();
                                hasYellowPowerCell = false;
                                Debug.Log("Powered Yellow Power Cell Container!");
                            }
                            break;
                        case PowerCellContainer.PowerCellColor.Red:
                            if (hasRedPowerCell)
                            {
                                container.PowerOn();
                                hasRedPowerCell = false;
                                Debug.Log("Powered Red Power Cell Container!");
                            }
                            break;
                    }
                    return;  // Exit after placing the power cell
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the detection area in the Scene view for debugging
        Gizmos.color = Color.green;
        Vector3 detectionCenter = transform.position + Vector3.up * rayHeightOffset;
        Gizmos.DrawWireSphere(detectionCenter, detectionRadius);
    }
}