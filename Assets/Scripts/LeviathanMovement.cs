using UnityEngine;

public class LeviathanMovement : MonoBehaviour
{
    private float startZ = 22f;
    private float endZ = 44f;
    private float duration = 10f;
    private float elapsedTime = 0f;
    private bool movingForward = true;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / duration;

        Vector3 currentPosition = transform.position;

        if (movingForward)
        {
            currentPosition.z = Mathf.Lerp(startZ, endZ, percentageComplete);
        }

        transform.position = currentPosition;

        // When one movement is complete, reverse direction and reset timer
        if (percentageComplete >= 1f)
        {
            movingForward = !movingForward;
            elapsedTime = 0f;
        }
    }
}