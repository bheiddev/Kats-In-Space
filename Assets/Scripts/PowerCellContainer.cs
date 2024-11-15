using UnityEngine;

public class PowerCellContainer : MonoBehaviour
{
    public enum PowerCellColor { Green, Yellow, Red }
    public PowerCellColor containerColor;

    public bool isPowered = false;

    public void PowerOn()
    {
        isPowered = true;
        Debug.Log($"{containerColor} Power Cell Container is now powered!");
    }
}