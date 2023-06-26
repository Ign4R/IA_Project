using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
   public void PlayGame()
   {
      SceneManager.LoadScene("Game_scene");
   }
   
   public void MainMenu()
   {
      SceneManager.LoadScene("Menu_scene");
   }
    
   public void QuitGame()
   {
     Application.Quit();
   }
}