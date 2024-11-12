using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public Transform laserOrigin;  // Origin point of the laser
    public GameObject laserPrefab;  // Laser particle system prefab
    public float laserDistance = 10f;  // Max distance of the laser
    public LayerMask targetLayer;  // Layer for target detection
    public GameObject lever;  // Reference to the lever GameObject with LeverScript

    private GameObject activeLaser;
    private LeverScript leverScript;

    private void Start()
    {
        // Instantiate the laser particle system and keep it inactive initially
        activeLaser = Instantiate(laserPrefab, laserOrigin.position, laserOrigin.rotation);
        activeLaser.SetActive(false);

        // Get the LeverScript component on the lever GameObject
        if (lever != null)
        {
            leverScript = lever.GetComponent<LeverScript>();
            if (leverScript != null)
            {
                leverScript.enabled = false;  // Initially disable the LeverScript
            }
        }
    }

    private void Update()
    {
        UpdateLaser();
    }

    private void UpdateLaser()
    {
        RaycastHit hit;
        Vector3 direction = laserOrigin.forward;  // Ensure direction is based on laserOrigin's forward direction

        // Draw a debug line from the laserOrigin, following the raycast direction and distance
        Debug.DrawRay(laserOrigin.position, direction * laserDistance, Color.red);

        // Check for a hit within the specified distance on the target layer
        if (Physics.Raycast(laserOrigin.position, direction, out hit, laserDistance, targetLayer))
        {
            // Activate the laser and set its position and orientation to point at the hit point
            activeLaser.SetActive(true);
            activeLaser.transform.position = laserOrigin.position;
            activeLaser.transform.LookAt(hit.point);

            Debug.Log("Target hit!");

            // Activate the LeverScript if the hit was successful
            if (leverScript != null)
            {
                leverScript.enabled = true;
            }
        }
        else
        {
            // Set laser at max distance if no target is hit
            activeLaser.SetActive(true);
            activeLaser.transform.position = laserOrigin.position;
            activeLaser.transform.rotation = laserOrigin.rotation;
            activeLaser.transform.localScale = new Vector3(1, 1, laserDistance);
        }
    }
}