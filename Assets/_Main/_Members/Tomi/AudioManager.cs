using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPickupSound(AudioClip sound)
    {
        if (audioSource != null && sound != null)
        {
            audioSource.PlayOneShot(sound);
        }
    }

    public void PlayAmbienceMusic()
    {
        
    }
}