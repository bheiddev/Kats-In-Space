using UnityEngine;

public class PowerCellHandler : MonoBehaviour
{
    public bool hasGreenPowerCell = false;
    public bool hasRedPowerCell = false;
    public bool hasYellowPowerCell = false;
    public float detectionRadius = 2f;
    public float rayHeightOffset = 1f;

    // References to particle system prefabs for each color
    public GameObject greenSmokePrefab;
    public GameObject yellowSmokePrefab;
    public GameObject redSmokePrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            InteractWithPowerCellOrContainer();
        }
    }

    void InteractWithPowerCellOrContainer()
    {
        Vector3 detectionCenter = transform.position + Vector3.up * rayHeightOffset;
        Collider[] hitColliders = Physics.OverlapSphere(detectionCenter, detectionRadius);

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("GreenPowerCell") && !hasGreenPowerCell)
            {
                InstantiateAndDestroyParticle(greenSmokePrefab, hit.transform.position);
                Destroy(hit.gameObject);
                hasGreenPowerCell = true;
                Debug.Log("Picked up Green Power Cell!");
                return;
            }
            else if (hit.CompareTag("YellowPowerCell") && !hasYellowPowerCell)
            {
                InstantiateAndDestroyParticle(yellowSmokePrefab, hit.transform.position);
                Destroy(hit.gameObject);
                hasYellowPowerCell = true;
                Debug.Log("Picked up Yellow Power Cell!");
                return;
            }
            else if (hit.CompareTag("RedPowerCell") && !hasRedPowerCell)
            {
                InstantiateAndDestroyParticle(redSmokePrefab, hit.transform.position);
                Destroy(hit.gameObject);
                hasRedPowerCell = true;
                Debug.Log("Picked up Red Power Cell!");
                return;
            }
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
                    return;
                }
            }
        }
    }

    void InstantiateAndDestroyParticle(GameObject particlePrefab, Vector3 position)
    {
        if (particlePrefab != null)
        {
            // Add a 2f offset on the Y axis
            Vector3 offsetPosition = position + new Vector3(0, .40f, 0);
            GameObject particleInstance = Instantiate(particlePrefab, offsetPosition, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 detectionCenter = transform.position + Vector3.up * rayHeightOffset;
        Gizmos.DrawWireSphere(detectionCenter, detectionRadius);
    }
}