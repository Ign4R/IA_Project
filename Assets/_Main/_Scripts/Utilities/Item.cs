using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Item : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private AudioClip pickupSound;

    public int Score => score;
    private GameManager gameManager;
    private AudioManager audioManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
        
        }

        audioManager = AudioManager.Instance;
        if (audioManager == null)
        {

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

            if (audioManager != null && pickupSound != null)
            {
                audioManager.PlayPickupSound(pickupSound);

            }
            else
            {

            }

            Destroy(gameObject);
        }
    }

    private bool IsPlayerCollision(Collider other)
    {
        return (playerLayer.value & (1 << other.gameObject.layer)) != 0;
    }
}