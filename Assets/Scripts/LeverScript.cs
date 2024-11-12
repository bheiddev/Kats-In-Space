using UnityEngine;

public class LeverScript : MonoBehaviour
{
    public Animator leverAnimator;  // Animator for the lever's animation
    public float activationDistance = 2f;  // Distance within which the player can activate the lever
    public string playerTag = "Player";  // Tag assigned to the player
    public string switchBoolName = "switch";  // Name of the animation bool in the Animator
    public GameObject door;  // Door GameObject with the DoorController script

    private GameObject player;
    private DoorController doorController;  // Reference to the DoorController script on the door

    private void Start()
    {
        // Find the player by tag
        player = GameObject.FindGameObjectWithTag(playerTag);

        // Get the DoorController component from the door GameObject
        if (door != null)
        {
            doorController = door.GetComponent<DoorController>();

            // Make sure the DoorController is disabled initially
            if (doorController != null)
            {
                doorController.enabled = false;
            }
        }
    }

    private void Update()
    {
        // Check if the player is within activation distance
        if (player != null && Vector3.Distance(player.transform.position, transform.position) <= activationDistance)
        {
            // Check if the F key is pressed
            if (Input.GetKeyDown(KeyCode.F))
            {
                // Set the Animator bool to true to trigger the lever animation
                leverAnimator.SetBool(switchBoolName, true);

                // Enable the DoorController script
                if (doorController != null)
                {
                    doorController.enabled = true;
                }
            }
        }
    }
}