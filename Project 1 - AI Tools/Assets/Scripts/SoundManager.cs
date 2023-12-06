using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip bossFightSoundtrack;

    private AudioSource audioSource;

    void Start()
    {
        // Initialize the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add an AudioSource component if not present
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the boss fight soundtrack clip
        audioSource.clip = bossFightSoundtrack;

        // Adjust the volume to your liking
        audioSource.volume = 0.5f;

        // Loop the soundtrack
        audioSource.loop = true;

        // Play the soundtrack
        audioSource.Play();
    }
}

