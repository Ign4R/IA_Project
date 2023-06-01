using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
   
    [SerializeField] private int gemPoints = 10; // Cantidad de puntos de la gema

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si la colisión fue con el jugador
        if (other.CompareTag("Player"))
        {
            // Obtener el componente GameManager
            GameManager gameManager = FindObjectOfType<GameManager>();

            // Verificar si se encontró el GameManager
            if (gameManager != null)
            {
                // Llamar al método AddGemScore del GameManager y pasarle el puntaje de la gema
                gameManager.AddGemScore(gemPoints);
            }
            else
            {
                //Tira warning si no encuentra game manager.
                Debug.LogWarning("No se encontró el componente GameManager en la escena.");
            }

           
            Destroy(gameObject);
        }
    }
}
