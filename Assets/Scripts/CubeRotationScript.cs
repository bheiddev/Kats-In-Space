using UnityEngine;

public class CubeRotationScript : MonoBehaviour
{
    public float rotationSpeed = 10f;  // Speed of rotation

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            RotateCube();
        }
    }

    private void RotateCube()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}