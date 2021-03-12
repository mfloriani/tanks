using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

enum GameMode
{
    None,
    Normal,
    BattleRoyale
}

//enum SceneState
//{
//    MainMenu,
//    ControllerMenu,
//    PauseMenu,
//    Playing
//}

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] int _mainMenuSceneIndex = 0;
    [SerializeField] GameObject _firstSelectedMainMenu;
    [SerializeField] GameObject _firstSelectedPauseMenu;

    bool _isMainMenu = false;
    bool _isGamePaused = false;

    const string MAIN_MENU = "Main Menu";
    const string PAUSE_MENU = "Pause Menu";
    const string CONTROLLER_MENU = "Controller Menu";

    GameMode _selectedMode = GameMode.None;
    

    void Awake()
    {
        //Debug.Log("MenuManager Awake");
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //Debug.Log("MenuManager Start");

        EventSystem.current.firstSelectedGameObject = _firstSelectedMainMenu;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_firstSelectedMainMenu);
    }

    void Update()
    {
        _isMainMenu = SceneManager.GetActiveScene().buildIndex == _mainMenuSceneIndex;

        //Debug.Log(_isMainMenu + " - " + _isGamePaused);

        if (Input.GetButtonDown("Joys_Start") && !_isMainMenu)
        {
            if (_isGamePaused)
                Resume();
            else
                Pause();
        }
    }

    public bool IsInsideMainMenu()
    {
        return _isMainMenu;
    }

    public void Resume()
    {
        //Time.timeScale = 1f;
        gameObject.transform.Find(PAUSE_MENU).gameObject.SetActive(false);
        _isGamePaused = false;
    }

    public void Pause()
    {
        //Time.timeScale = 0f;
        gameObject.transform.Find(PAUSE_MENU).gameObject.SetActive(true);
        _isGamePaused = true;

        EventSystem.current.firstSelectedGameObject = _firstSelectedPauseMenu;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_firstSelectedPauseMenu);
    }

    public void SelectSkirmishMode()
    {
        _selectedMode = GameMode.Normal;

        gameObject.transform.Find(MAIN_MENU).gameObject.SetActive(false);
        gameObject.transform.Find(CONTROLLER_MENU).gameObject.SetActive(true);
        
    }

    public void SelectBattleRoyaleMode()
    {
        _selectedMode = GameMode.BattleRoyale;

        gameObject.transform.Find(MAIN_MENU).gameObject.SetActive(false);
        gameObject.transform.Find(CONTROLLER_MENU).gameObject.SetActive(true);        
    }

    public void StartGame()
    {
        gameObject.transform.Find(CONTROLLER_MENU).gameObject.SetActive(false);

        if (_selectedMode == GameMode.Normal)
            SceneManager.LoadScene("Skirmish_demo");
        else if (_selectedMode == GameMode.BattleRoyale)
            SceneManager.LoadScene("Battle_royale_demo");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        _isGamePaused = false;
        gameObject.transform.Find(MAIN_MENU).gameObject.SetActive(true);
        gameObject.transform.Find(PAUSE_MENU).gameObject.SetActive(false);
        gameObject.transform.Find(CONTROLLER_MENU).gameObject.SetActive(false);
        SceneManager.LoadScene(_mainMenuSceneIndex);

        EventSystem.current.firstSelectedGameObject = _firstSelectedMainMenu;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_firstSelectedMainMenu);
    }
}
