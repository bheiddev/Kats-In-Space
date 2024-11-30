using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeverScript : MonoBehaviour
{
    [Header("Lever Settings")]
    public Animator leverAnimator;
    public float activationDistance = 2f;
    public string playerTag = "Player";
    public string switchBoolName = "switch";
    public GameObject door;
    public List<PowerCellContainer> requiredContainers;

    [Header("Final Lever Settings")]
    public CombinationManager combinationManager;
    public TerminalInputDisplay terminalInputDisplay;
    public GameObject finalLeverObject;

    [Header("Error UI Panels")]
    [SerializeField] private GameObject incorrectCombinationUIPanel;
    [SerializeField] private GameObject notAllContainersPoweredUIPanel;
    [SerializeField] private GameObject catNotCollectedUIPanel;

    [Header("Cat Collection")]
    public CatHandler catHandler;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip errorAudioClip;    // Error audio
    private AudioSource audioSource;

    private GameObject player;
    private DoorController doorController;
    public bool isActivated = false;
    private bool isFinalLever = false;

    private void Start()
    {
        // Determine if this is the final lever
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

        // Setup AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (!isActivated && player != null)
        {
            // Show the Lever Panel if player is within range
            if (distanceToPlayer <= activationDistance)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    // For final lever, check all conditions
                    if (isFinalLever)
                    {
                        if (VerifyCombination() && AllContainersPowered() && catHandler.IsCatCollected)
                        {
                            ActivateLever();
                        }
                        else
                        {
                            if (!VerifyCombination())
                            {
                                Debug.Log("Incorrect combination entered!");
                                StartCoroutine(ShowErrorUI(incorrectCombinationUIPanel));
                            }
                            else if (!AllContainersPowered())
                            {
                                Debug.Log("Not all required containers are powered!");
                                StartCoroutine(ShowErrorUI(notAllContainersPoweredUIPanel));
                            }
                            else if (!catHandler.IsCatCollected)
                            {
                                Debug.Log("Cat has not been collected!");
                                StartCoroutine(ShowErrorUI(catNotCollectedUIPanel));
                            }
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
                        StartCoroutine(ShowErrorUI(notAllContainersPoweredUIPanel));
                    }
                }
            }
        }
    }

    private IEnumerator ShowErrorUI(GameObject errorUIPanel)
    {
        // Play error sound
        if (errorAudioClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(errorAudioClip);
        }

        errorUIPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        errorUIPanel.SetActive(false);
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