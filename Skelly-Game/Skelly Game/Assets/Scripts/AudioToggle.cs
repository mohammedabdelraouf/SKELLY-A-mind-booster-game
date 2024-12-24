using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AudioToggle : MonoBehaviour
{
    public AudioSource audioSource; // Assign the AudioSource in Inspector

    private bool isMuted = false; // Flag to track mute state

    public void ToggleMute()
    {
        isMuted = !isMuted; // Toggle mute state on button click

        if (isMuted)
        {
            audioSource.mute = true; // Mute audio when button is on (selected)
        }
        else
        {
            audioSource.mute = false; // Unmute audio when button is off (deselected)
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            else
            {
                audioSource.Play();
            }
        }

    }
}
