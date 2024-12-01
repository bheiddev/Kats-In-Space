using UnityEngine;
using UnityEngine.UI;


public class SettingsMenu : MonoBehaviour
{
    //Settings Menu to toggle between audio, video, and keyboard panels

    [Header("Panels")]
    public GameObject videoSettingsPanel;


    void Start()
    {
        ShowVideoSettings();
    }

    public void ShowVideoSettings()
    {
        videoSettingsPanel.SetActive(true);
    }
}
