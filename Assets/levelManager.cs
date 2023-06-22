using UnityEngine;
using UnityEngine.SceneManagement;
public class levelManager : MonoBehaviour
{
    public void RetryGame()
    {
        //Retry game
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        //Retry game
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
