using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private int gemScore = 0;

    public int GetScore()
    {
        return gemScore;
    }

    public void AddGemScore(int score)
    {
        gemScore += score;
        print("Puntaje actual del jugador: " + gemScore);

        // Verificar si se ha alcanzado la condición de victoria
        GameManager.Instance.CheckWinCondition(gemScore);
    }
    
}
