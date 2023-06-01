using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    // ASIGNAR LA ALEATORIZACION DE LOS SPAWNS SEGUN PUNTAJE DE LAS GEMAS PARTICUALRES, O SEGUN EL PUNTAJE ACTUAL DEL PLAYER
    //PARA QUE TENGA MÁS PESO EL rOULETTE.
   
    [SerializeField] private GameObject[] gemPrefabs;
    [SerializeField] private float[] gemScores;
    [SerializeField] private Transform[] gemSpawnPoints;
    [SerializeField] private int numberOfGemsToSpawn = 3;

    private List<Transform> availableSpawnPoints;

    private void Start()
    {
        InitializeSpawnPoints();
        SpawnGems();
    }

    private void InitializeSpawnPoints()
    {
        // Crear una lista de puntos de spawn disponibles
        availableSpawnPoints = new List<Transform>(gemSpawnPoints);
    }

    private void SpawnGems()
    {
        // Verificar si hay suficientes spawn points disponibles para el número de gemas a spawnear
        if (numberOfGemsToSpawn > availableSpawnPoints.Count)
        {
           print("No hay suficientes spawn points disponibles para spawnear todas las gemas.");
            return;
        }

        for (int i = 0; i < numberOfGemsToSpawn; i++)
        {
            // Crear un diccionario para almacenar las gemas y sus puntajes
            Dictionary<GameObject, float> gemDict = new Dictionary<GameObject, float>();

            // Agregar las gemas y sus puntajes al diccionario
            for (int j = 0; j < gemPrefabs.Length; j++)
            {
                gemDict[gemPrefabs[j]] = gemScores[j];
            }

            // Seleccionar una gema aleatoria usando el método Roulette de MyRandoms
            GameObject selectedGem = MyRandoms.Roulette(gemDict);

            // Seleccionar un punto de spawn aleatorio de los disponibles
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[randomIndex];
            availableSpawnPoints.RemoveAt(randomIndex);

            // Spawnear la gema seleccionada en el punto de spawn con la rotación predeterminada del prefab
            Instantiate(selectedGem, spawnPoint.position, selectedGem.transform.rotation);
        }
    }
    
}
