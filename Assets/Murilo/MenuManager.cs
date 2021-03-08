using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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

        if (Input.GetButtonDown("Start Button") && !_isMainMenu)
        {
            if (_isGamePaused)
                Resume();
            else
                Pause();
        }
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

    public void PlaySkirmish()
    {
        gameObject.transform.Find(MAIN_MENU).gameObject.SetActive(false);
        SceneManager.LoadScene("Skirmish_demo");
    }

    public void PlayBattleRoyale()
    {
        gameObject.transform.Find(MAIN_MENU).gameObject.SetActive(false);
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
        SceneManager.LoadScene(_mainMenuSceneIndex);

        EventSystem.current.firstSelectedGameObject = _firstSelectedMainMenu;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_firstSelectedMainMenu);
    }
}
