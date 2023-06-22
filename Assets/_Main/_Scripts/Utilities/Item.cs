using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Item : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private AudioClip pickupSound; // The pickup sound

    public int Score => score;
    private GameManager gameManager;
    private AudioSource audioSource; // The audio source

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogWarning("No se encontró el componente GameManager en la escena.");
        }

        // Get the audio source component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("No se encontró el componente AudioSource en este objeto.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayerCollision(other))
        {
            if (gameManager != null)
            {
                gameManager.AddGemScore(score);
            }

            // Play the pickup sound
            if (audioSource != null && pickupSound != null)
            {
                audioSource.clip = pickupSound;
                audioSource.Play();
                Debug.Log("Playing pickup sound"); // Debug statement
            }
            else
            {
                Debug.Log("Failed to play pickup sound"); // Debug statement
            }

            Destroy(gameObject);
        }
    }


    private bool IsPlayerCollision(Collider other)
    {
        return (playerLayer.value & (1 << other.gameObject.layer)) != 0;
    }
}