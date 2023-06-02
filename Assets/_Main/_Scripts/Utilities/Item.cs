using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int gemPoints = 10;
    [SerializeField] private float gemWeight = 1.0f;

    public float GemWeight => gemWeight;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerScore playerScore = FindObjectOfType<PlayerScore>();

            if (playerScore != null)
            {
                playerScore.AddGemScore(gemPoints);
            }
            else
            {
                Debug.LogWarning("No se encontró el componente PlayerScore en la escena.");
            }

            Destroy(gameObject);
        }
    }
}
