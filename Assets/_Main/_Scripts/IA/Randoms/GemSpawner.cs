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
    
    private void Start()
    {
        InitializeSpawnPoints();
        SpawnGems();
    }
    private void InitializeSpawnPoints()
    {
        availableSpawnPoints = new List<Transform>(gemSpawnPoints);
    }
    
    //MODIFICADO PARA QUE SPAWNEE CON EL INVOKEREPEATING CADA CIERTO TIEMPO.
    private void SpawnGems()
    {
        // Inicializa el contador de gemas spawneadas
        gemsSpawned = 0;
    
        // Llama al método SpawnGemAtInterval cada segundo ---->AJUSTAR ACÁ PARA CAMBIAR EL INVERVALO ENTRE SPAWN) 
        //3 SEGUNDOS PARA TESTEO.
        InvokeRepeating("SpawnGemAtInterval", 3.0f, 3.0f);
    }
    
    
   //AHORA UTILIZA COMO PESO EL SCORE DEL PLAYER.(ITEM --> GEM MULTIPLIER)
    private Dictionary<GameObject, float> BuildGemDictionary(int playerScore)
    {
        Dictionary<GameObject, float> gemDict = new Dictionary<GameObject, float>();
    
        for (int j = 0; j < gemPrefabs.Length; j++)
        {
            Item item = gemPrefabs[j].GetComponent<Item>();
            float adjustedGemScore = item.Score * gemWeightMultiplier;   //CAMBIAR ACÁ PARA MODIFICAR LAS PROBABILIDADES DE SPAWN SEGÚN PUNTAJE.
        
            // Ajusta el puntaje de las gemas en base al score del player
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
                    adjustedGemScore *= 2.5f;
                }
            }
        
            gemDict[gemPrefabs[j]] = adjustedGemScore;
        }
        return gemDict;
    }

    //UTILIZA EL METODO ROULETTE DE MY RANDOMS Y EL RANGE PARA SPAWN ALEATORIO.
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
    
    //SPAWN EN INTERVALO DE LAS GEMAS, PARA QUE VAYAN APARECIENDO CADA "X" TIEMPO. 
    private int gemsSpawned;
    // ReSharper disable Unity.PerformanceAnalysis
    private void SpawnGemAtInterval()
    {
        // Verifica si ya se  spawnearon todas las gemas necesarias
        if (gemsSpawned >= numberOfGemsToSpawn)
        {
            // Cancela el InvokeRepeating
            CancelInvoke("SpawnGemAtInterval");
            return;
        }
    
        // Incrementa el contador de gemas spawneadas
        gemsSpawned++;
    
        // Spawnea una gema
        int playerScore = GameManager.Instance.GetScore();
        var gemDict = BuildGemDictionary(playerScore);
        if (gemDict.Count > 0)
        {
            SpawnGem(gemDict);
        }
        else
        {
            //Debugeo de advertencia.
            Debug.LogWarning("No se encontraron gemas válidas para spawnear.");
            CancelInvoke("SpawnGemAtInterval");
        }
    }
}