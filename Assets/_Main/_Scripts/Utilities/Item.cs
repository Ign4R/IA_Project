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
            // Imprimir el mensaje de recolección en la consola de Unity
            print("Gema recolectada! Puntos: " + gemPoints);
            // Destruir el objeto del item
            Destroy(gameObject);
        }
    }
}
