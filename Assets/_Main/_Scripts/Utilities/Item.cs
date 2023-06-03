using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int gemPoints = 10;
    [SerializeField] private float gemWeight = 1.0f;
    [SerializeField] private LayerMask playerLayer;

    public float GemWeight => gemWeight;

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
                gameManager.AddGemScore(gemPoints);
            }

            Destroy(gameObject);
        }
    }

    private bool IsPlayerCollision(Collider other)
    {
        return (playerLayer.value & (1 << other.gameObject.layer)) != 0;
    }
}