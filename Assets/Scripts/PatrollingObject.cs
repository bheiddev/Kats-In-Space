using UnityEngine;

public class PatrollingObject : MonoBehaviour
{
    public float minZ = 6.5f;
    public float maxZ = 31f;
    public float speed = 5f;

    private bool movingForward = true;

    void Update()
    {
        // Get the current position
        Vector3 position = transform.position;

        // Move in the z direction
        if (movingForward)
        {
            position.z += speed * Time.deltaTime;
            if (position.z >= maxZ)
            {
                position.z = maxZ;
                movingForward = false;
            }
        }
        else
        {
            position.z -= speed * Time.deltaTime;
            if (position.z <= minZ)
            {
                position.z = minZ;
                movingForward = true;
            }
        }

        // Set the position, keeping x and y the same
        transform.position = position;
    }
}
