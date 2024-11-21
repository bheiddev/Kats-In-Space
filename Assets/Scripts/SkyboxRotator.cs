using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    public float rotationSpeed = 1f;  // Speed of the rotation in degrees per second

    private void Update()
    {
        // Rotate the skybox around the Y-axis over time
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }
}