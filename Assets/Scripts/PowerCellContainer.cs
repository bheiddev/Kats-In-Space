using UnityEngine;

public class PowerCellContainer : MonoBehaviour
{
    public enum PowerCellColor { Green, Yellow, Red }
    public PowerCellColor containerColor;

    public bool isPowered = false;
    public GameObject powerCellPrefab; // Reference to the powercell prefab
    public Transform placementPoint; // Position to place the powercell

    public void PowerOn()
    {
        isPowered = true;
        Debug.Log($"{containerColor} Power Cell Container is now powered!");

        // Instantiate the powercell model at the placement point if available
        if (placementPoint != null && powerCellPrefab != null)
        {
            // Instantiate the powercell as a child of the placementPoint
            GameObject powerCellInstance = Instantiate(powerCellPrefab, placementPoint.position, Quaternion.identity, placementPoint);

            // Reset rotation to ensure correct orientation
            powerCellInstance.transform.localRotation = Quaternion.identity;

            // Increase the scale of the power cell instance by 3x
            powerCellInstance.transform.localScale *= 2.5f;
        }
    }
}