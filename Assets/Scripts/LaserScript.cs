using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public Transform laserOrigin; 
    public GameObject laserPrefab;  
    public float laserDistance = 10f; 
    public GameObject lever;  

    private GameObject activeLaser;
    private LeverScript leverScript;

    private void Start()
    {
        activeLaser = Instantiate(laserPrefab, laserOrigin.position, laserOrigin.rotation);

        if (lever != null)
        {
            leverScript = lever.GetComponent<LeverScript>();
            if (leverScript != null)
            {
                leverScript.enabled = false; 
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

        // Perform a raycast without any layer mask to detect any collider
        if (Physics.Raycast(laserOrigin.position, direction, out hit, laserDistance))
        {
            // Activate the laser and set its position and orientation to point at the hit point
            activeLaser.SetActive(true);
            activeLaser.transform.position = laserOrigin.position;
            activeLaser.transform.LookAt(hit.point);

            // Calculate the distance to the hit point and set the laser length accordingly
            float hitDistance = Vector3.Distance(laserOrigin.position, hit.point);
            activeLaser.transform.localScale = new Vector3(1, 1, hitDistance);

            // Activate the LeverScript if the hit was successful
            if (leverScript != null)
            {
                leverScript.enabled = true;
            }
        }
        else
        {
            // Set laser at max distance if no collider is hit
            Debug.Log("laser at max distance");
            activeLaser.SetActive(true);
            activeLaser.transform.position = laserOrigin.position;
            activeLaser.transform.rotation = laserOrigin.rotation;
            activeLaser.transform.localScale = new Vector3(1, 1, laserDistance);
        }
    }
}

