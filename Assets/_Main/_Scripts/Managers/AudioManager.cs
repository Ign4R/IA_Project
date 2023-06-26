using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource audioSource;

    //SHEEP SFX
    [SerializeField] private AudioClip sheepSound;
    [SerializeField] private float soundInterval = 5f;
    private float timer = 0f;
    
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
    
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= soundInterval)
        {
            PlaySheepSound();
            timer = 0f;
        }
    }
    
    private void PlaySheepSound()
    {
        if (audioSource != null && sheepSound != null)
        {
            audioSource.PlayOneShot(sheepSound);
        }
    }

    public void PlayAmbienceMusic()
    {
        
    }
}