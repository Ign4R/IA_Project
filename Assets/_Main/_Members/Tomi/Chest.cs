
using System.Collections.Generic;
using UnityEngine;
public class Chest : MonoBehaviour
{
   [SerializeField] private GameObject[] gemPrefabs;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int minGemsToCollect = 3;

    private bool isOpened;

    private void OnTriggerEnter(Collider other)
    {
        if (!isOpened && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isOpened = true;

            PlayerScore playerScore = other.GetComponent<PlayerScore>();
            if (playerScore != null)
            {
                CollectGems(playerScore);
            }
            else
            {
                Debug.LogWarning("No se encontró el componente PlayerScore en el jugador.");
            }

            Destroy(gameObject);
        }
    }

    private void CollectGems(PlayerScore gameManager)
    {
        int gemsCollected = GameManager.Instance.GetGemCount();
        int gemIndex = GetGemIndex(gemsCollected);

        if (gemIndex != -1)
        {
            Dictionary<GameObject, float> gemDict = BuildGemDictionary(gemIndex);

            if (gemDict.Count > 0)
            {
                SpawnGem(gemDict);
            }
            else
            {
                Debug.LogWarning("No se encontró ninguna gema válida para el índice: " + gemIndex);
            }
        }
    }

    private int GetGemIndex(int gemsCollected)
    {
        return gemsCollected >= minGemsToCollect ? Mathf.Clamp(gemsCollected - minGemsToCollect, 0, gemPrefabs.Length - 1) : -1;
    }

    private Dictionary<GameObject, float> BuildGemDictionary(int startIndex)
    {
        Dictionary<GameObject, float> gemDict = new Dictionary<GameObject, float>();

        for (int i = startIndex; i < gemPrefabs.Length; i++)
        {
            Item gem = gemPrefabs[i].GetComponent<Item>();
            if (gem != null)
            {
                gemDict[gemPrefabs[i]] = gem.GemWeight;
            }
            else
            {
                Debug.LogWarning("No se encontró el componente Item en el prefab de gema: " + gemPrefabs[i].name);
            }
        }

        return gemDict;
    }

    private void SpawnGem(Dictionary<GameObject, float> gemDict)
    {
        GameObject selectedGem = MyRandoms.Roulette(gemDict);
        Instantiate(selectedGem, spawnPoint.position, selectedGem.transform.rotation);
    }
    }