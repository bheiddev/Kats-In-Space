using UnityEngine;
using System.Collections.Generic;

public class LeverScript : MonoBehaviour
{
    public Animator leverAnimator;
    public float activationDistance = 2f;
    public string playerTag = "Player";
    public string switchBoolName = "switch";
    public GameObject door;  // Door GameObject with the DoorController script
    public List<PowerCellContainer> requiredContainers;  // List of required power cell containers

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
                // Activate the lever if all required containers are powered
                if (AllContainersPowered())
                {
                    ActivateLever();
                }
                else
                {
                    Debug.Log("Not all required containers are powered!");
                }
            }
        }
    }

    private bool AllContainersPowered()
    {
        // Check if every container in the list is powered
        foreach (PowerCellContainer container in requiredContainers)
        {
            if (container != null && !container.isPowered)
            {
                return false;
            }
        }
        return true;
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