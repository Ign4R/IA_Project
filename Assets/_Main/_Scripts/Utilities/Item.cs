using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Item : MonoBehaviour
{
     [SerializeField] private int score;
    [SerializeField] private LayerMask playerLayer;
    
    public int Score => score;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogWarning("No se encontró el componente GameManager en la escena.");
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

            Destroy(gameObject);
        }
    }

    private bool IsPlayerCollision(Collider other)
    {
        return (playerLayer.value & (1 << other.gameObject.layer)) != 0;
    }
}