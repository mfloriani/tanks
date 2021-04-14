using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

enum GameMode
{
    None,
    Normal,
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

    bool _isGamePaused = false;

    const string MAIN_MENU = "Main Menu";
    const string PAUSE_MENU = "Pause Menu";
    const string CONTROLLER_MENU = "Controller Menu";
    const string GAMEUI = "GameUI";

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
        gameObject.transform.Find(GAMEUI).gameObject.SetActive(true);

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
        gameObject.transform.Find(GAMEUI).gameObject.SetActive(false);

        SceneManager.LoadScene(_mainMenuSceneIndex);

        EventSystem.current.firstSelectedGameObject = _firstSelectedMainMenu;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_firstSelectedMainMenu);
    }

    public ControllerState[] GetControllers()
    {
        return ControllerManager.Instance.GetControllers();
    }
}
