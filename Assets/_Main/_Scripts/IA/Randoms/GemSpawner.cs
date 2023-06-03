using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] gemPrefabs;
    [SerializeField] private float[] gemScores;
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

    private void SpawnGems()
    {
        if (numberOfGemsToSpawn > availableSpawnPoints.Count)
        {
            Debug.LogWarning("No hay suficientes puntos de spawn disponibles para spawnear todas las gemas.");
            return;
        }

        Dictionary<GameObject, float> gemDict = BuildGemDictionary();

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

    private Dictionary<GameObject, float> BuildGemDictionary()
    {
        Dictionary<GameObject, float> gemDict = new Dictionary<GameObject, float>();

        for (int j = 0; j < gemPrefabs.Length; j++)
        {
            float adjustedGemScore = gemScores[j] * gemWeightMultiplier;
            gemDict[gemPrefabs[j]] = adjustedGemScore;
        }

        return gemDict;
    }

    private void SpawnGem(Dictionary<GameObject, float> gemDict)
    {
        GameObject selectedGem = MyRandoms.Roulette(gemDict);
        int randomIndex = Random.Range(0, availableSpawnPoints.Count);
        Transform spawnPoint = availableSpawnPoints[randomIndex];
        availableSpawnPoints.RemoveAt(randomIndex);

        Instantiate(selectedGem, spawnPoint.position, selectedGem.transform.rotation);
    }
    
}