using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private int winScore = 200;

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

    //WinCondition 1  del GAME MANAGER
    public void CheckWinCondition(int playerScore)
    {
        if (playerScore >= winScore)
        {
            WinGame();
        }
    }

    public void WinGame()
    {
        // Cambio de escena al ganar partida (Win screen)
        print("¡You Win!");
    }

    public void GameOver()
    {
        // Cambio a Game over Scene al perder la partida.
        print("¡Game over!.");
    }

}
