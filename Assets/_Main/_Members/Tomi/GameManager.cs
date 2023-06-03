using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private int gemScore;
    private int winScore = 200;
    private Dictionary<string, int> gemValues = new Dictionary<string, int>
    {
        { "Diamondo", 10 },
        { "SphereGemLarge", 20 },
        { "BeveledStar", 40 },
        // Agrega más gemas y sus respectivos valores según sea necesario
    };

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public int GetScore()
    {
        return gemScore;
    }

    public void AddGemScore(int score)
    {
        gemScore += score;
        print("Puntaje actual del jugador: " + gemScore);

        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (gemScore >= winScore)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        print("¡You Win!");
    }

    public void GameOver()
    {
        print("¡Game Over!");
    }

    public void UpdateGemCount()
    {
        int gemCount = 0;
        foreach (var gem in gemValues)
        {
            int gemValue = gem.Value;
            gemCount += gemValue;
        }
    }

    public int GetGemCount()
    {
        return gemValues.Count;
    }

}