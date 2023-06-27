
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //SINGLETON
    private static GameManager instance;

    //SINGLETON
    public static GameManager Instance => instance;

    //SCORE VARIABLES:
    private int _gemScore = 0;
    [SerializeField] private int _scoreMax = 250;
    public GameObject _panOver;
    public GameObject _panWin;
    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _lifesText;


    //UI:
    private GameObject _miniMap;
    public GameObject _fadeOut;
    
    //GEMS DICTIONARY:
    private Dictionary<string, int> gemValues = new Dictionary<string, int>
    {
        { "Diamondo", 10 },
        { "SphereGemLarge", 20 },
        { "BeveledStar", 40 },
        // Agrega más gemas y sus respectivos valores según sea necesario
    };

    //SINGLETON--------------------
    private void Awake()
    {
        _fadeOut.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
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
    

    void Start()
    {
        // Buscar y asignar referencias a los objetos de la interfaz de usuario

        _scoreText = GameObject.Find("PlayerScore").GetComponent<TextMeshProUGUI>();
        _lifesText = GameObject.Find("PlayerLives").GetComponent<TextMeshProUGUI>();
        _miniMap = GameObject.Find("MiniMap");
    }

    //SCORE DE GEMAS RECOLECTADAS DESDE ITEM:
    public void AddGemScore(int score)
    {
        _gemScore += score;
        _scoreText.text = "Score: " + _gemScore;


        if (_gemScore >= _scoreMax)
        {
            WinGame();
        }
    }
    
    public void UpdateLifes(int lifes)
    {
        _lifesText.text = "Lifes: " + lifes;
    }
    
    //WIN GCONDITION
    public void WinGame()
    {
        Cursor.visible = true;
        _panWin.SetActive(true);
        _miniMap.SetActive(false);
        print("¡You Win!");
    }

    public int GetScore()
    {
        Debug.LogWarning("TODO");
        return _gemScore;
        //return _currentScore; ///TODO
    }
    
 //GAME OVER
    public void GameOver()
    {
        Cursor.visible = true;
        _panOver.SetActive(true);
        _miniMap.SetActive(false);
        print("¡Game over!.");

    }

    //COMENTAR ESTOS METODOS?
   




}