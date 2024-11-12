using UnityEngine;

public class HideCursorScript : MonoBehaviour
{
    // Option to set whether the cursor should be hidden at the start
    public bool hideCursor = true;

    void Start()
    {
        // Immediately hide the cursor and lock it when the game starts
        SetCursorVisibility(hideCursor);
    }

    void Update()
    {
        // Optionally toggle the cursor visibility with the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))  // Press Escape to toggle cursor visibility
        {
            bool isCursorVisible = Cursor.visible;
            SetCursorVisibility(!isCursorVisible);
        }
    }

    // Method to handle cursor visibility and lock state
    private void SetCursorVisibility(bool shouldHide)
    {
        if (shouldHide)
        {
            Cursor.lockState = CursorLockMode.Locked;  // Lock the cursor to the center of the screen
            Cursor.visible = false;  // Hide the cursor
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;  // Allow the cursor to move freely
            Cursor.visible = true;  // Show the cursor
        }
    }
}