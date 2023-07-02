
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //SINGLETON
    private static GameManager instance;

    //SINGLETON
    public static GameManager Instance => instance;

 

    //SCORE VARIABLES:
    int _currentScore = 0;
    [SerializeField] private int _scoreMax = 250;
    public GameObject _panOver;
    public GameObject _panWin;
    TextMeshProUGUI _currScoreUI;
    TextMeshProUGUI _maxScoreUI;
    TextMeshProUGUI _lifesUI;
    Toggle _saveSheepsUI;

    bool playerIsDie;
    bool sheepRescued;
    ///Player
    public bool PlayerIsDie { get => playerIsDie; set => playerIsDie = value; }


    //UI:
    private GameObject _miniMapUI;
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
        playerIsDie = false;
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
    }
    

    void Start()
    {
        // Buscar y asignar referencias a los objetos de la interfaz de usuario

        _currScoreUI = GameObject.Find("CurrentScore").GetComponent<TextMeshProUGUI>();
        _maxScoreUI = GameObject.Find("MaxScore").GetComponent<TextMeshProUGUI>();
        _lifesUI = GameObject.Find("PlayerLives").GetComponent<TextMeshProUGUI>();
        _miniMapUI = GameObject.Find("MiniMap");
        _saveSheepsUI = GameObject.Find("SavedSheeps").GetComponent<Toggle>();
        _maxScoreUI.text= "/ " + _scoreMax;
    }

    //SCORE DE GEMAS RECOLECTADAS DESDE ITEM:
    public void AddGemScore(int score)
    {
        _currentScore += score;
        _currScoreUI.text = "Score: " + _currentScore;
        CheckWin();
    }

    public void SavedSheeps()
    {
        _saveSheepsUI.interactable = true;
        _saveSheepsUI.isOn = true;
        sheepRescued = true;
        CheckWin();
    }
    private void CheckWin()
    {
        if (_currentScore >= _scoreMax && sheepRescued)
        {
            WinGame();
        }
    }
    public void UpdateLifes(int lifes)
    {
        _lifesUI.text = "Lifes: " + lifes;
    }
    
    //WIN GCONDITION
    public void WinGame()
    {

        Cursor.visible = true;
        _panWin.SetActive(true);
        _miniMapUI.SetActive(false);
        print("¡You Win!");


    }

    public int GetScore()
    {
        return _currentScore;
        //return _currentScore; ///TODO
    }
    
 //GAME OVER
    public void GameOver()
    {
        PlayerIsDie = true;
        Cursor.visible = true;
        _panOver.SetActive(true);
        _miniMapUI.SetActive(false);
        print("¡Game over!.");

    }


    //COMENTAR ESTOS METODOS?





}