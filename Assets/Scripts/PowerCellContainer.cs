using UnityEngine;

public class PowerCellContainer : MonoBehaviour
{
    public enum PowerCellColor { Green, Yellow, Red }
    public PowerCellColor containerColor;
    public bool isPowered = false;
    public GameObject powerCellPrefab; // Reference to the powercell prefab
    public Transform placementPoint; // Position to place the powercell
    private Animation crateAnimation; // Reference to crate animation
    private bool isCrateOpen = false; // Track if crate is open

    void Start()
    {
        crateAnimation = GetComponent<Animation>();
        OpenCrate(); // Automatically open the crate at the start
    }

    // Open the crate animation automatically at start
    private void OpenCrate()
    {
        if (crateAnimation != null && !isCrateOpen)
        {
            crateAnimation.Play("Crate_Open");
            isCrateOpen = true;
        }
    }

    // Close the crate animation when the power cell is placed
    public void CloseCrate()
    {
        if (crateAnimation != null && isCrateOpen)
        {
            crateAnimation.Play("Crate_Close");
            isCrateOpen = false;
        }
    }

    // Place the power cell inside the container and power it on
    public void PowerOn()
    {
        isPowered = true;
        Debug.Log($"{containerColor} Power Cell Container is now powered!");

        // Instantiate the powercell model at the placement point if available
        if (placementPoint != null && powerCellPrefab != null)
        {
            GameObject powerCellInstance = Instantiate(powerCellPrefab, placementPoint.position, Quaternion.identity, placementPoint);
            powerCellInstance.transform.localRotation = Quaternion.identity; // Correct orientation
            powerCellInstance.transform.localScale *= 2.5f; // Scale the powercell

            // Close the crate after placing the power cell
            CloseCrate();
        }
    }
}