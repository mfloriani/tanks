using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum GameMode
{
    None,
    Arcade,
    Multiplayer,
    BattleRoyale
}

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    //public Image[] PlayerAvatar;
    public Sprite[] PlayerAvatar;
    public Sprite[] ControllerAvatar;
    
    [SerializeField] int _mainMenuSceneIndex = 0;
    [SerializeField] GameObject _firstSelectedMainMenu;
    [SerializeField] GameObject _firstSelectedPauseMenu;
    [SerializeField] GameObject _firstSelectedGameOverMenu;

    bool _isGamePaused = false;

    const string MAIN_MENU = "Main Menu";
    const string PAUSE_MENU = "Pause Menu";
    const string CONTROLLER_MENU = "Controller Menu";
    const string GAMEUI = "GameUI";
    const string GAMEOVER_MENU = "GameOver Menu";

    GameMode _selectedMode = GameMode.None;
    
    public GameMode GetSelectedMode()
    {
        return _selectedMode;
    }

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
        ControllerManager.Instance.UpdateConnectedControllers();

        //Debug.Log(_isMainMenu + " - " + _isGamePaused);
        if(IsInGame())
        {
            ControllerManager.Instance.HandleInGameJoinButton();

            if (Input.GetButtonDown("Joys_Start"))
            {
                if (_isGamePaused)
                    Resume();
                else
                    Pause();
            }
        }
    }

    public bool IsInGame()
    {
        return SceneManager.GetActiveScene().buildIndex != _mainMenuSceneIndex;
    }

    public void Resume()
    {
        gameObject.transform.Find(PAUSE_MENU).gameObject.SetActive(false);
        _isGamePaused = false;
    }

    public void Pause()
    {
        gameObject.transform.Find(PAUSE_MENU).gameObject.SetActive(true);
        _isGamePaused = true;

        EventSystem.current.firstSelectedGameObject = _firstSelectedPauseMenu;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_firstSelectedPauseMenu);
    }

    public void SelectArcadeMode()
    {
        _selectedMode = GameMode.Arcade;

        gameObject.transform.Find(MAIN_MENU).gameObject.SetActive(false);
        //gameObject.transform.Find(CONTROLLER_MENU).gameObject.SetActive(true);

        ControllerManager.Instance.PlayerJoinedArcadeMode(0);

        StartGame();
    }

    public void SelectMultiplayerMode()
    {
        _selectedMode = GameMode.Multiplayer;

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
        gameObject.transform.Find(GAMEUI).gameObject.SetActive(true);

        //if (_selectedMode == GameMode.Arcade)
            SceneManager.LoadScene(1);

        //else if (_selectedMode == GameMode.BattleRoyale)
        //    SceneManager.LoadScene(2);
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
        gameObject.transform.Find(GAMEUI).gameObject.SetActive(false);
        gameObject.transform.Find(GAMEOVER_MENU).gameObject.SetActive(false);

        SceneManager.LoadScene(_mainMenuSceneIndex);

        EventSystem.current.firstSelectedGameObject = _firstSelectedMainMenu;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_firstSelectedMainMenu);
    }

    public ControllerState[] GetControllers()
    {
        return ControllerManager.Instance.GetControllers();
    }

    public void ShowGameOverMenu()
    {
        gameObject.transform.Find(GAMEOVER_MENU).gameObject.SetActive(true);
        
        EventSystem.current.firstSelectedGameObject = _firstSelectedGameOverMenu;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_firstSelectedGameOverMenu);
    }

}
