using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControllerMenu : MonoBehaviour
{
    [SerializeField] Transform[] _players;
    [SerializeField] Transform _countdownText;
    [SerializeField] int _countdownMaxTime = 5;
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip[] clips;
    float _countdown;
    bool _starting = false;

    void Start()
    {
        //Debug.Log("ControllerMenu.Start()");

        if (MenuManager.Instance.ControllerAvatar.Length == 0)
        {
            Debug.LogError("No controller avatars set in the GameManager gameobject!");
        }

        for (int i = 0; i < _players.Length; ++i)
        {
            _players[i].Find("Controller").gameObject.GetComponent<Image>().sprite = MenuManager.Instance.ControllerAvatar[i];
        }
    }


    void Update()
    {
        if (MenuManager.Instance.IsInGame())
            return;

        int index = 0;
        var controllers = Input.GetJoystickNames();
        foreach (string s in controllers)
        {
            HandleJoinButton(index);
            HandleConfirmButton(index);
            HandleBackButton(index);
            ++index;
        }

        // countdown started?
        if (_starting)
        {
            _countdown -= Time.deltaTime;
            _countdownText.GetComponent<TextMeshProUGUI>().SetText(((int)_countdown).ToString());
            if (_countdown % 1 == 0) sfx.PlayOneShot(clips[0], 0.8f);   //plays a snare drum on countdown
            if (_countdown <= 0)
            {
                _countdown = 0;
                MenuManager.Instance.StartGame();
            }
        }
    }

    void OnEnable()
    {
        Reset();
    }

    void OnDisable()
    {

    }


    private static void HandleBackButton(int index)
    {
        string joyButtonB = "Joy" + (index + 1) + "_B";
        if (Input.GetButtonDown(joyButtonB))
        {
            FindObjectOfType<MenuManager>().LoadMainMenu();
        }
    }

    private void HandleConfirmButton(int index)
    {
        string joyButtonX = "Joy" + (index + 1) + "_X";
        if (Input.GetButtonDown(joyButtonX))
        {
            if (MenuManager.Instance.GetControllers()[index].Selected && !MenuManager.Instance.GetControllers()[index].Confirmed)
            {
                ToggleConfirmation(index, true);
            }
        }
    }

    private void HandleJoinButton(int index)
    {
        string joyButtonA = "Joy" + (index + 1) + "_A";
        if (Input.GetButtonDown(joyButtonA))
        {
            if (MenuManager.Instance.GetControllers()[index].Selected)
            {
                RemovePlayer(index);
            }
            else
            {
                AddPlayer(index);
                sfx.PlayOneShot(clips[index], 0.8f);
            }
        }
    }

    // check if all selected controllers pressed start
    bool StartCountdown()
    {
        int selected = 0;
        int confirmed = 0;

        foreach (var c in MenuManager.Instance.GetControllers())
        {
            if (c.Selected)
                selected++;

            if (c.Confirmed)
                confirmed++;
        }
        return selected == confirmed;
    }

    // add controller to the player list (moved to right)
    void AddPlayer(int index)
    {
        var controllers = MenuManager.Instance.GetControllers();
        if (!controllers[index].Selected)
        {
            controllers[index].Selected = true;

            _players[index].Find("Join Button").gameObject.SetActive(false);
            _players[index].Find("Controller").gameObject.SetActive(true);
            _players[index].Find("Confirmed").gameObject.SetActive(false);

            ToggleConfirmation(index, false);
        }
    }

    // remove controller from the player list (moved to left)
    void RemovePlayer(int index)
    {
        var controllers = MenuManager.Instance.GetControllers();
        if (controllers[index].Selected)
        {
            controllers[index].Selected = false;
        }
        _players[index].Find("Join Button").gameObject.SetActive(true);
        _players[index].Find("Controller").gameObject.SetActive(false);
        _players[index].Find("Confirmed").gameObject.SetActive(false);

        ToggleConfirmation(index, false);

    }

    // controller pressed start or was removed from the selected list
    void ToggleConfirmation(int index, bool enabled)
    {
        var controllers = MenuManager.Instance.GetControllers();
        controllers[index].Confirmed = enabled;
        _players[index].Find("Confirmed").gameObject.SetActive(enabled);

        // check if should start the countdown every time a controller confirms 
        if (enabled)
        {
            if (StartCountdown())
            {
                _starting = true;
                _countdown = _countdownMaxTime;
                _countdownText.gameObject.SetActive(true);
            }
        }
        else
        {
            _starting = false;
            _countdownText.gameObject.SetActive(false);
        }
    }

    // reset controller after the game started for the next time 
    void Reset()
    {
        _starting = false;
        try
        {
            var controllers = MenuManager.Instance.GetControllers();
            foreach (var p in controllers)
            {
                RemovePlayer(p.Id);
            }
        }
        catch (System.Exception e) { }
    }
}
