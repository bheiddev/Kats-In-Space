using UnityEngine;

public class GreenPowerCellContainer : MonoBehaviour
{
    public bool isPowered = false;
    public LeverScript associatedLever; // Reference to the associated lever in the room

    public void PowerOn()
    {
        isPowered = true;
        // No need to call SetContainerPowered, as it's not defined or required for functionality
        Debug.Log("Green Power Cell Container is now powered!");
    }
}