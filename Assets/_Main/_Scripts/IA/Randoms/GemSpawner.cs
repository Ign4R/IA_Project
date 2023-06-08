using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    
    //VARIABLES DE GEMA, PREFABS, SCORES, WEIGHT, ETC.
    [SerializeField] private GameObject[] gemPrefabs;
    [SerializeField] private Transform[] gemSpawnPoints;
    [SerializeField] private int numberOfGemsToSpawn;
    [SerializeField] private float gemWeightMultiplier = 1.0f;
    private List<Transform> availableSpawnPoints;

    //SPAWN EN INTERVALO DE GEMAS.
    // private float gemSpawnInterval = 5.0f; 
    // private float timeSinceLastGemSpawn = 0.0f;
    private void Start()
    {
        InitializeSpawnPoints();
        SpawnGems();
    }
    
    //SPAWN INTERVALO DE GEMAS
    // private void Update()
    // {
    //     timeSinceLastGemSpawn += Time.deltaTime;
    //
    //     if (timeSinceLastGemSpawn >= gemSpawnInterval)
    //     {
    //         timeSinceLastGemSpawn = 0.0f;
    //         SpawnGems();
    //     }
    // }
    private void InitializeSpawnPoints()
    {
        availableSpawnPoints = new List<Transform>(gemSpawnPoints);
    }
    
    //DICCIONARIO DE GEMAS
    private void SpawnGems()
    {
        int gemsToSpawn = Mathf.Min(numberOfGemsToSpawn, availableSpawnPoints.Count);
    
        // Get player's current score from GameManager
        int playerScore = GameManager.Instance.GetScore();
    
        // Pass in player's current score when building gem dictionary
        var gemDict = BuildGemDictionary(playerScore);

        for (int i = 0; i < gemsToSpawn; i++)
        {
            if (gemDict.Count > 0)
            {
                SpawnGem(gemDict);
            }
            else
            {
                Debug.LogWarning("No se encontraron gemas válidas para spawnear.");
                break;
            }
        }
    }
    
    //SPAWN EN INTERVALO DE GEMAS (REQUIERE EL UPDATE)
    // private void SpawnGems()
    // {
    //     if (numberOfGemsToSpawn > availableSpawnPoints.Count)
    //     {
    //         Debug.LogWarning("No hay suficientes puntos de spawn disponibles para spawnear todas las gemas.");
    //         return;
    //     }
    //
    //     // Get player's current score from GameManager
    //     int playerScore = GameManager.Instance.GetScore();
    //
    //     // Pass in player's current score when building gem dictionary
    //     var gemDict = BuildGemDictionary(playerScore);
    //
    //     for (int i = 0; i < numberOfGemsToSpawn; i++)
    //     {
    //         if (gemDict.Count > 0)
    //         {
    //             SpawnGem(gemDict);
    //         }
    //         else
    //         {
    //             Debug.LogWarning("No se encontraron gemas válidas para spawnear.");
    //             break;
    //         }
    //     }
    // }
    
    //MODIFICAR LOS GEMSCORES PARA QUE ESTÉN SOLO EN EL SCRIPT ITEM.
    private Dictionary<GameObject, float> BuildGemDictionary(int playerScore)
    {
        Dictionary<GameObject, float> gemDict = new Dictionary<GameObject, float>();
    
        for (int j = 0; j < gemPrefabs.Length; j++)
        {
            Item item = gemPrefabs[j].GetComponent<Item>();
            float adjustedGemScore = item.Score * gemWeightMultiplier;
        
            // Adjust gem scores based on player's current score
            if (playerScore >= 100)
            {
                // Decrementa la probabilidad de spawnear gemas de alto valor.
                if (adjustedGemScore >= 20)
                {
                    adjustedGemScore *= 0.5f;
                }
            }
            else
            {
                // Incrementa la probabilidad de spawnear gemas de alto valor
                if (adjustedGemScore >= 20)
                {
                    adjustedGemScore *= 2.0f;
                }
            }
        
            gemDict[gemPrefabs[j]] = adjustedGemScore;
        }
        return gemDict;
    }

    //UTILIZA EL METODO ROULETTE DE MYRANDOMS Y EL RANGE PARA SPAWN ALEATORIO.
    private void SpawnGem(Dictionary<GameObject, float> gemDict)
    {
        GameObject selectedGem = MyRandoms.Roulette(gemDict);
        int randomIndex = Random.Range(0, availableSpawnPoints.Count);
        Transform spawnPoint = availableSpawnPoints[randomIndex];
        availableSpawnPoints.RemoveAt(randomIndex);
    
        // Debugea el valor de la gema spawneada ( para testeo)
        Item item = selectedGem.GetComponent<Item>();
        Debug.Log("Spawned gem with value: " + item.Score);
    
        Instantiate(selectedGem, spawnPoint.position, selectedGem.transform.rotation);
    }
}