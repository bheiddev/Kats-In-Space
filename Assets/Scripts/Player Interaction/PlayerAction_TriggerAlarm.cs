using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction_TriggerAlarm : PlayerAction
{
    private AudioSource audioSource;
    public AudioClip alertAudioClip; // Audio clip to play during alert mode
    public bool loopAudio = true; // Whether the audio should loop
    public float audioVolume = 1f; // Volume of the audio (0 to 1)

    private void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // Set the initial volume
        audioSource.volume = audioVolume;
    }

    public override void Action()
    {
        AlertModeManager.SwitchToAlertMode();

        // Find and change all ceiling light colors
        Color alertColor;
        ColorUtility.TryParseHtmlString("#A8261D", out alertColor);

        foreach (GameObject lightObject in GameObject.FindGameObjectsWithTag("CeilingLight"))
        {
            Light light = lightObject.GetComponent<Light>();
            if (light != null)
            {
                // Change both the light color and emission color if using emission
                light.color = alertColor;

                // If the light has a material with emission
                MeshRenderer renderer = lightObject.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    Material material = renderer.material;
                    if (material.HasProperty("_EmissionColor"))
                    {
                        material.SetColor("_EmissionColor", alertColor);
                    }
                }
            }
        }

        // Play audio clip repeatedly
        if (alertAudioClip != null && !audioSource.isPlaying)
        {
            Debug.Log("Attempting to play audio");
            audioSource.clip = alertAudioClip;
            audioSource.loop = loopAudio;
            audioSource.Play();
        }
    }
}