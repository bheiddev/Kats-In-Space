using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class VideoSettings : MonoBehaviour
{
    [Header("Dropdowns, Icon, Button")]
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown displayModeDropdown;
    public Button applyButton;

    private List<Resolution> commonResolutions;


    void Start()
    {
        InitializeVideoSettings();
    }

    void InitializeVideoSettings()
    {
        commonResolutions = new List<Resolution>
        {
            new Resolution { width = 1280, height = 720 },
            new Resolution { width = 1366, height = 768 },
            new Resolution { width = 1600, height = 900 },
            new Resolution { width = 1920, height = 1080 },
            new Resolution { width = 2560, height = 1080 },
            new Resolution { width = 3440, height = 1440 },
        };

        List<string> resolutionOptions = new List<string>();
        List<string> displayModeOptions = new List<string> { "Fullscreen", "Windowed" };

        int currentResolutionIndex = 0;
        for (int i = 0; i < commonResolutions.Count; i++)
        {
            string option = commonResolutions[i].width + " x " + commonResolutions[i].height;
            resolutionOptions.Add(option);

            if (commonResolutions[i].width == Screen.currentResolution.width &&
                commonResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        displayModeDropdown.ClearOptions();
        displayModeDropdown.AddOptions(displayModeOptions);
        displayModeDropdown.value = Screen.fullScreen ? 0 : 1;

        applyButton.onClick.AddListener(ApplySettings);
    }

    public void ApplySettings()
    {
        int resolutionIndex = resolutionDropdown.value;
        Resolution resolution = commonResolutions[resolutionIndex];

        bool isFullscreen = displayModeDropdown.value == 0;

        Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
    }
}
