using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // Check if the AudioSource component exists
        if (audioSource != null)
        {
            // Play the audio
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource component not found on GameObject: " + gameObject.name);
        }
    }
}
