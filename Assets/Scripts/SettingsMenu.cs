using UnityEngine;
using UnityEngine.UI;


public class SettingsMenu : MonoBehaviour
{
    //Settings Menu to toggle between audio, video, and keyboard panels

    [Header("Panels and Buttons")]
    public GameObject videoSettingsPanel;
    public GameObject audioSettingsPanel;
    public GameObject hotkeySettingsPanel;


    void Start()
    {
        ShowVideoSettings();
    }

    public void ShowVideoSettings()
    {
        videoSettingsPanel.SetActive(true);
        audioSettingsPanel.SetActive(false);
        hotkeySettingsPanel.SetActive(false);
    }

    public void ShowAudioSettings()
    {
        videoSettingsPanel.SetActive(false);
        audioSettingsPanel.SetActive(true);
        hotkeySettingsPanel.SetActive(false);
    }

    public void ShowHotkeySettings()
    {
        videoSettingsPanel.SetActive(false);
        audioSettingsPanel.SetActive(false);
        hotkeySettingsPanel.SetActive(true);
    }
}
