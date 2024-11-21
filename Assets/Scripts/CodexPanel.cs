using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodexPanel : MonoBehaviour
{
    private CombinationManager combinationManager;

    private void Start()
    {
        combinationManager = FindObjectOfType<CombinationManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            combinationManager.TogglePanel();
        }
    }
}