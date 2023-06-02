
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
        if (other.CompareTag("Player") && !isOpened)
        {
            isOpened = true;

            PlayerScore playerScore = other.GetComponent<PlayerScore>();
            if (playerScore != null)
            {
                int gemsCollected = playerScore.GetGemCount();
                int gemIndex = GetGemIndex(gemsCollected);

                if (gemIndex != -1)
                {
                    Dictionary<GameObject, float> gemDict = new Dictionary<GameObject, float>();

                    for (int i = gemIndex; i < gemPrefabs.Length; i++)
                    {
                        gemDict[gemPrefabs[i]] = 1.0f;
                    }

                    GameObject selectedGem = MyRandoms.Roulette(gemDict);

                    Instantiate(selectedGem, spawnPoint.position, selectedGem.transform.rotation);
                }
                else
                {
                    Debug.LogWarning("No se encontró ninguna gema válida para el índice: " + gemIndex);
                }
            }
            else
            {
                Debug.LogWarning("No se encontró el componente PlayerScore en el jugador.");
            }

            Destroy(gameObject);
        }
    }
    private int GetGemIndex(int gemsCollected)
    {
        if (gemsCollected >= minGemsToCollect)
        {
            // Calcular el índice de la gema según las gemas recolectadas
            int gemIndex = Mathf.Clamp(gemsCollected - minGemsToCollect, 0, gemPrefabs.Length - 1);
            return gemIndex;
        }

        return -1;
    }
}
