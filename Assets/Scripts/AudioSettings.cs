using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    [Header("AudioSliders and Volume Text")]
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public TMP_Text masterVolumeText;
    public Slider musicSlider;
    public TMP_Text musicVolumeText;
    public Slider sfxSlider;
    public TMP_Text sfxVolumeText;

    // Update these strings to match the exact names of the exposed parameters
    private const string MasterVolumeParam = "Master";
    private const string MusicVolumeParam = "Music";
    private const string SFXVolumeParam = "SFX";

    void Start()
    {
        // Load saved settings or use default (100 corresponds to 0 dB in AudioMixer)
        masterSlider.value = PlayerPrefs.GetFloat("Master", 100);
        musicSlider.value = PlayerPrefs.GetFloat("Music", 100);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX", 100);

        // Apply the saved settings to the audio mixer
        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);

        // Add listeners to handle slider value changes
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float value)
    {
        float dBValue = SliderToDecibels(value);
        audioMixer.SetFloat(MasterVolumeParam, dBValue);
        PlayerPrefs.SetFloat("Master", value);
        UpdateMasterVolumeText(value);
    }

    public void SetMusicVolume(float value)
    {
        float dBValue = SliderToDecibels(value);
        audioMixer.SetFloat(MusicVolumeParam, dBValue);
        PlayerPrefs.SetFloat("Music", value);
        UpdateMusicVolumeText(value);
    }

    public void SetSFXVolume(float value)
    {
        float dBValue = SliderToDecibels(value);
        audioMixer.SetFloat(SFXVolumeParam, dBValue);
        PlayerPrefs.SetFloat("SFX", value);
        UpdateSFXVolumeText(value);
    }

    private float SliderToDecibels(float value)
    {
        // Convert 0-100 slider range to a decibel range (-80dB to 0dB)
        return (value > 0) ? Mathf.Log10(value / 100) * 20 : -80f;
    }

    private void UpdateMasterVolumeText(float value)
    {
        masterVolumeText.text = value.ToString("F0");
    }

    private void UpdateMusicVolumeText(float value)
    {
        musicVolumeText.text = value.ToString("F0");
    }

    private void UpdateSFXVolumeText(float value)
    {
        sfxVolumeText.text = value.ToString("F0");
    }
}
