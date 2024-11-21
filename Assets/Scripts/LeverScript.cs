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

    // New variables for combination verification
    public CombinationManager combinationManager;
    public TerminalInputDisplay terminalInputDisplay;
    public GameObject finalLeverObject;  // Reference to the final lever's GameObject

    private GameObject player;
    private DoorController doorController;
    private bool isActivated = false;
    private bool isFinalLever = false;

    private void Start()
    {
        // Determine if this is the final lever in the level
        isFinalLever = (finalLeverObject != null && finalLeverObject == this.gameObject);

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
                // For final lever, check combination match
                if (isFinalLever)
                {
                    if (VerifyCombination())
                    {
                        ActivateLever();
                    }
                    else
                    {
                        Debug.Log("Incorrect combination entered!");
                    }
                }
                // For other levers, check power cell containers
                else if (AllContainersPowered())
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

    private bool VerifyCombination()
    {
        // Ensure we have references to both combination manager and terminal input display
        if (combinationManager == null || terminalInputDisplay == null)
        {
            Debug.LogError("Combination Manager or Terminal Input Display is not assigned!");
            return false;
        }

        // Get the combination from the combination manager
        List<int> generatedCombination = GetGeneratedCombination();

        // Get the entered combination from the terminal input display
        List<int> enteredCombination = GetEnteredCombination();

        // Debug log for generated combination
        Debug.Log("Generated Combination:");
        foreach (int num in generatedCombination)
        {
            Debug.Log($"Generated Number: {num}");
        }

        // Debug log for entered combination
        Debug.Log("Entered Combination:");
        foreach (int num in enteredCombination)
        {
            Debug.Log($"Entered Number: {num}");
        }

        // Compare the two combinations
        if (generatedCombination.Count != 4 || enteredCombination.Count != 4)
        {
            Debug.LogError($"Combination lengths do not match! Generated: {generatedCombination.Count}, Entered: {enteredCombination.Count}");
            return false;
        }

        for (int i = 0; i < 4; i++)
        {
            if (generatedCombination[i] != enteredCombination[i])
            {
                Debug.Log($"Mismatch at index {i}. Generated: {generatedCombination[i]}, Entered: {enteredCombination[i]}");
                return false;
            }
        }

        Debug.Log("Combination verified successfully!");
        return true;
    }

    private List<int> GetGeneratedCombination()
    {
        // You'll need to modify CombinationManager to expose the generated numbers
        return combinationManager.GetGeneratedCombination();
    }

    private List<int> GetEnteredCombination()
    {
        // Since currentSymbols is private, you'll need to modify TerminalInputDisplay
        // to expose a method that returns the entered numbers
        List<int> enteredNumbers = new List<int>();

        // You'll need to add a method in TerminalInputDisplay to get the current symbols
        // For example:
        foreach (GameObject symbolObj in terminalInputDisplay.GetCurrentSymbols())
        {
            // Extract the number from the symbol's name
            int number = int.Parse(symbolObj.name.Split('_')[1]);
            enteredNumbers.Add(number);
        }

        return enteredNumbers;
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