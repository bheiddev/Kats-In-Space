using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    //Script for opening/closing the pause menu and going one level deeper to the settings menu
    [Header("Panels and Buttons")]
    public GameObject menuPanel;
    public GameObject settingsPanel;
    public Button continueGameButton;

    private bool isPaused = false;


    void Start()
    {
        isPaused = false;
        menuPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseKeyboard();
        }
    }

    void TogglePauseKeyboard()
    {
        if (!isPaused)
        {
            PauseGameKeyboard();
        }
        else
        {
            ResumeGame();
        }
    }

    void PauseGameKeyboard()
    {
        Time.timeScale = 0f; 

        menuPanel.SetActive(true); 
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        EventSystem.current.SetSelectedGameObject(continueGameButton.gameObject);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        menuPanel.SetActive(false); 

        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 

        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SettingsMenu()
    {
        settingsPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void BackButton()
    {
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
}
