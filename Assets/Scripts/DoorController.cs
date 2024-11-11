using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform player;              // Reference to the player
    public float detectionRange = 4f;     // Distance within which the door opens
    private Animator doorAnimator;        // Reference to the door's Animator

    private void Start()
    {
        // Get the Animator component attached to this object
        doorAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check if the player is within the detection range
        bool isPlayerNearby = Vector3.Distance(transform.position, player.position) <= detectionRange;

        // Set the "character_nearby" parameter in the Animator
        doorAnimator.SetBool("character_nearby", isPlayerNearby);
    }
}
