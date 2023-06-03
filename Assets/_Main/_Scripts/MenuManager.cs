using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Game_scene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
