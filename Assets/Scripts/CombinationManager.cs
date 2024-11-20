using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CombinationManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject combinationPanel;  // The main panel containing all images
    public NumberDisplay[] numberDisplays;  // Array of 4 NumberDisplay components

    [Header("Number Sprites")]
    public Sprite[] numberSprites;  // Array of 10 number sprites (0-9)

    private List<int> availableNumbers;
    private int collectiblesFound = 0;

    private void Start()
    {
        combinationPanel.SetActive(false);
        InitializeAvailableNumbers();

        // Subscribe to collectible events
        CollectibleObject.OnCollected += OnCollectibleFound;
    }

    private void OnDestroy()
    {
        CollectibleObject.OnCollected -= OnCollectibleFound;
    }

    private void InitializeAvailableNumbers()
    {
        availableNumbers = Enumerable.Range(0, 10).ToList();
    }

    private void OnCollectibleFound(CollectibleObject collectible)
    {
        if (collectiblesFound >= 4) return;

        // Get a random available number
        int randomIndex = Random.Range(0, availableNumbers.Count);
        int selectedNumber = availableNumbers[randomIndex];
        availableNumbers.RemoveAt(randomIndex);

        // Set the sprite for the current NumberDisplay
        numberDisplays[collectiblesFound].SetNumber(numberSprites[selectedNumber]);

        collectiblesFound++;
    }

    public void TogglePanel()
    {
        combinationPanel.SetActive(!combinationPanel.activeSelf);
    }
}