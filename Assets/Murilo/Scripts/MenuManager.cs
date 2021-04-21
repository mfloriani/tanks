using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

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


    [SerializeField] AudioSource sfx;   //audiosource for playing select/confirm sounds
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioClip[] clips; //clip array for storing select/confirm sounds
    [SerializeField] AudioClip win;
    [SerializeField] AudioClip lose;
    [SerializeField] AudioClip menumusic;
    [SerializeField] AudioClip gamemusic;
    
    bool _isGamePaused = false;

    GameObject last;

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
        if (IsInGame())
        {
            // cannot join the arcade mode 
            if (GetSelectedMode() != GameMode.Arcade)
                ControllerManager.Instance.HandleInGameJoinButton();

            if (Input.GetButtonDown("Joys_Start"))
            {
                if (_isGamePaused)
                    Resume();
                else
                    Pause();
            }
        }

        if (last && last != EventSystem.current.currentSelectedGameObject)
            sfx.PlayOneShot(clips[0], 1f);  //play a move cursor sound - JG

        last = EventSystem.current.currentSelectedGameObject;
    }

    public bool IsInGame()
    {

        return SceneManager.GetActiveScene().buildIndex != _mainMenuSceneIndex;
    }

    public void Resume()
    {
        sfx.PlayOneShot(clips[2]);
        gameObject.transform.Find(PAUSE_MENU).gameObject.SetActive(false);
        last = null;
        _isGamePaused = false;
    }

    public void Pause()
    {
        sfx.PlayOneShot(clips[2]);
        gameObject.transform.Find(PAUSE_MENU).gameObject.SetActive(true);
        _isGamePaused = true;
        last = null;
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

        sfx.PlayOneShot(clips[1], 0.8f);  //play a confirm selection sound - JG

        gameObject.transform.Find(MAIN_MENU).gameObject.SetActive(false);
        gameObject.transform.Find(CONTROLLER_MENU).gameObject.SetActive(true);
    }

    public void SelectBattleRoyaleMode()
    {
        _selectedMode = GameMode.BattleRoyale;

        sfx.PlayOneShot(clips[1], 0.8f);  //play a confirm selection sound - JG

        gameObject.transform.Find(MAIN_MENU).gameObject.SetActive(false);
        gameObject.transform.Find(CONTROLLER_MENU).gameObject.SetActive(true);
    }

    public void StartGame()
    {
        gameObject.transform.Find(CONTROLLER_MENU).gameObject.SetActive(false);
        gameObject.transform.Find(GAMEUI).gameObject.SetActive(true);
        bgm.Stop();
        last = null;
        bgm.clip = gamemusic;   //set the music to game music
        bgm.Play();             //play the game music - JG
        SceneManager.LoadScene(1);

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

        if (!bgm.isPlaying) { bgm.clip = menumusic; bgm.Play(); } // set bgm to menu music and play it - JG

        EventSystem.current.firstSelectedGameObject = _firstSelectedMainMenu;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_firstSelectedMainMenu);

        ControllerManager.Instance.ResetConfirmed();
    }

    public ControllerState[] GetControllers()
    {
        return ControllerManager.Instance.GetControllers();
    }

    public void ShowGameOverMenu(string msg, Color color)
    {
        gameObject.transform.Find(GAMEOVER_MENU).gameObject.SetActive(true);
        last = null;
        EventSystem.current.firstSelectedGameObject = _firstSelectedGameOverMenu;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_firstSelectedGameOverMenu);

        var winner = gameObject.transform.Find(GAMEOVER_MENU).gameObject.transform.Find("Winner");
        var winnerMsg = winner.Find("Message").GetComponent<TextMeshProUGUI>();
        
        winnerMsg.text = msg;
        winnerMsg.color = color;
        last = null;
        if (winnerMsg.text != "YOU FAILED") bgm.clip = win;     //set soundclip to the win if someone won
        else bgm.clip = lose;           // if no winner, set it to a sad tuba
        bgm.PlayOneShot(bgm.clip);         //play the win sound - JG
    }

    public void HideGameOverMenu()
    {
        gameObject.transform.Find(GAMEOVER_MENU).gameObject.SetActive(false);
        last = null;
    }
}
