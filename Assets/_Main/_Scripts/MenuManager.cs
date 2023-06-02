using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Animator anim;
    public static bool fade_In = false;
    public GameObject Fade_In;

    public void FadeInAnimation()
    {
        Fade_In.SetActive(true);
        anim.Play("Fade_In");

        StartGame();
    }

    public void FadeOut()
    {
        anim.SetTrigger("Fade_Out");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
