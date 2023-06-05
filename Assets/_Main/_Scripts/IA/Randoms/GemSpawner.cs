using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    //VARIABLES DE GEMA, PREFABS, SCORES, WEIGHT, ETC.
    [SerializeField] private GameObject[] gemPrefabs;
    [SerializeField] private Transform[] gemSpawnPoints;
    [SerializeField] private int numberOfGemsToSpawn = 3;
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

    //DICCIONARIO DE GEMAS, UTILIZACIÓN DE LISTA.
    private void SpawnGems()
    {
        if (numberOfGemsToSpawn > availableSpawnPoints.Count)
        {
            Debug.LogWarning("No hay suficientes puntos de spawn disponibles para spawnear todas las gemas.");
            return;
        }

        var gemDict = BuildGemDictionary();

        for (int i = 0; i < numberOfGemsToSpawn; i++)
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

    //MODIFICAR LOS GEMSCORES PARA QUE ESTÉN SOLO EN EL SCRIPT ITEM.
    private Dictionary<GameObject, float> BuildGemDictionary()
    {
        Dictionary<GameObject, float> gemDict = new Dictionary<GameObject, float>();
        
        for (int j = 0; j < gemPrefabs.Length; j++)
        {
            Item item = gemPrefabs[j].GetComponent<Item>();
            float adjustedGemScore = item.Score * gemWeightMultiplier;
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
        
        Instantiate(selectedGem, spawnPoint.position, selectedGem.transform.rotation);
    }
}