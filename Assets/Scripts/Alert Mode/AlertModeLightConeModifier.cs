using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertModeLightConeModifier : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Color colorToSwitchTo = Color.red;
    public bool copyAlphaFromOriginal = true;

    [Header("Set Dynamically")]
    public LightCone[] cones;
    public Color[] originalColors;
    public AudioClip alertAudioClip; // Audio clip to play during alert mode
    public bool loopAudio = true; // Whether the audio should loop
    public float audioVolume = 1f; // Volume of the audio (0 to 1)

    private bool inited = false;
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        cones = GetComponentsInChildren<LightCone>();
        if (cones.Length == 0)
        {
            Debug.LogError("GameObject " + gameObject.name + " and children do not contain " +
                           "a LightCone component");
            return;
        }

        originalColors = new Color[cones.Length];
        for (int i = 0; i < cones.Length; i++)
        {
            originalColors[i] = cones[i].color;
        }

        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Register this to know when AlertMode is entered (or exited)
        AlertModeManager.alertModeStatusChangeDelegate += AlertModeStatusChange;

        // Set the initial volume
        audioSource.volume = audioVolume;

        inited = true;
    }

    private void OnDestroy()
    {
        // De-register this to know when AlertMode is entered (or exited)
        AlertModeManager.alertModeStatusChangeDelegate -= AlertModeStatusChange;
    }

    void AlertModeStatusChange(bool alertMode)
    {
        if (!inited)
        {
            // If this did not init properly. Do not try to do this
            return;
        }

        if (alertMode)
        {
            // Change light cone colors
            Color color;
            for (int i = 0; i < cones.Length; i++)
            {
                color = colorToSwitchTo;
                if (copyAlphaFromOriginal)
                {
                    color.a = originalColors[i].a;
                }
                cones[i].color = color;
            }

            // Play audio clip repeatedly
            if (alertAudioClip != null && !audioSource.isPlaying)
            {
                audioSource.clip = alertAudioClip;
                audioSource.loop = loopAudio;
                audioSource.Play();
            }
        }
        else
        {
            // Reset light cone colors to their original state
            for (int i = 0; i < cones.Length; i++)
            {
                cones[i].color = originalColors[i];
            }

            // Stop audio when exiting alert mode
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}