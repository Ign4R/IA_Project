using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance => instance;

    //SCORE VARIABLES:
    private int gemScore = 0;
    private int winScore = 150;
    public GameObject gameOver;


    private int currentScore;

  
    
    //SINGLETON--------------------
    private void Awake()
    {
        // Verificar si ya existe una instancia del GameManager
        if (instance != null && instance != this)
        {
            // Ya hay una instancia, destruir este objeto
            Destroy(gameObject);
            return;
        }

        // Establecer esta instancia como la instancia actual del GameManager
        instance = this;

        // Mantener este objeto GameManager en todas las escenas
        DontDestroyOnLoad(gameObject);
    }

    //SCORE DE GEMAS RECOLECTADAS DESDE ITEM:
    public void AddGemScore(int score)
    {
        gemScore += score;
        Debug.Log("Puntaje actual: " + gemScore);

        if (gemScore >= winScore)
        {
            WinGame();
        }
    }
    
    public void WinGame()
    {

       print("¡You Win!");
    }

    public int GetScore()
    {
        return currentScore;
    }
 
    public void GameOver()
    {

        gameOver.SetActive(true);
       print("¡Game over!.");

    }

}
