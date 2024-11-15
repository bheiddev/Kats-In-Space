using UnityEngine;

public class LeverScript : MonoBehaviour
{
    public Animator leverAnimator;
    public float activationDistance = 2f;
    public string playerTag = "Player";
    public string switchBoolName = "switch";
    public GameObject door;  // Door GameObject with the DoorController script
    public GreenPowerCellContainer associatedContainer;  // Optional paired power cell container

    private GameObject player;
    private DoorController doorController;
    private bool isActivated = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag);

        if (door != null)
        {
            doorController = door.GetComponent<DoorController>();
            if (doorController != null)
            {
                doorController.enabled = false;
            }
        }
    }

    private void Update()
    {
        // Check if the player is within activation distance and the lever hasn't been activated yet
        if (!isActivated && player != null && Vector3.Distance(player.transform.position, transform.position) <= activationDistance)
        {
            // Allow activation when F is pressed
            if (Input.GetKeyDown(KeyCode.F))
            {
                // Activate the lever if there is no associated container, or if the container is powered
                if (associatedContainer == null || associatedContainer.isPowered)
                {
                    ActivateLever();
                }
                else
                {
                    Debug.Log("The associated container is not powered!");
                }
            }
        }
    }

    private void ActivateLever()
    {
        isActivated = true;
        leverAnimator.SetBool(switchBoolName, true);

        if (doorController != null)
        {
            doorController.enabled = true;
        }

        Debug.Log("Lever activated and door opened!");
    }
}