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

    const float _deadZone = 0.8f;
    const int _maxPlayers = 4;
    float _countdown;
    bool _starting = false;

    struct ControllerState
    {
        public ControllerState(int id)
        {
            this.Id = id;
            this.Selected = false;
            this.Confirmed = false;
            this.Connected = false;
            this.Player = 0;
        }
        public int Id { get; set; }
        public bool Selected { get; set; }
        public bool Confirmed { get; set; }
        public bool Connected { get; set; }
        public int Player { get; set; }
    }

    ControllerState[] _controllers;

    void Start()
    {
        // init the list of max controllers allowed
        _controllers = new ControllerState[_maxPlayers];
        for(int i=0; i < _maxPlayers; ++i)
            _controllers[i] = new ControllerState(i);

        UpdateConnectedControllers();
    }

    
    void Update()
    {
        UpdateConnectedControllers();
        
        if (!MenuManager.Instance.IsInsideMainMenu())
            return;

        int index = 0;
        var controllers = Input.GetJoystickNames();
        foreach (string s in controllers)
        {
            string joyButtonA = "Joy" + (index + 1) + "_A";
            if (Input.GetButtonDown(joyButtonA))
            {
                if(_controllers[index].Selected)
                {
                    RemovePlayer(index);
                }
                else
                {
                    AddPlayer(index);
                }                
            }

            string joyButtonX = "Joy" + (index + 1) + "_X";
            if (Input.GetButtonDown(joyButtonX))
            {
                if (_controllers[index].Selected && !_controllers[index].Confirmed)
                {
                    ToggleConfirmation(index, true);
                }
            }

            string joyButtonB = "Joy" + (index + 1) + "_B";
            if (Input.GetButtonDown(joyButtonB))
            {
                Reset();
                FindObjectOfType<MenuManager>().LoadMainMenu();
            }
            
            ++index;
        }

        // countdown started?
        if(_starting)
        {
            _countdown -= Time.deltaTime;
            _countdownText.GetComponent<TextMeshProUGUI>().SetText(((int)_countdown).ToString());
            if (_countdown <= 0)
            {
                _countdown = 0;
                Reset();
                MenuManager.Instance.StartGame();
            }
        }
    }

    // check if all selected controllers pressed start
    bool StartCountdown()
    {
        int selected = 0;
        int confirmed = 0;
        foreach(var c in _controllers)
        {
            if (c.Selected)
                selected++;

            if (c.Confirmed)
                confirmed++;
        }
        return selected == confirmed;
    }

    // update the controller list with the connected ones
    void UpdateConnectedControllers()
    {
        int id = 0;
        foreach (string s in Input.GetJoystickNames())
        {
            // restrict to the max controllers
            if (id > _maxPlayers)
                break;

            //Debug.Log("Adding controller: [" + id + "] => " + s);
            _controllers[id].Connected = true;
            id++;
        }
    }

    // add controller to the player list (moved to right)
    void AddPlayer(int index)
    {
        if(!_controllers[index].Selected)
        {
            _controllers[index].Selected = true;

            _players[index].Find("Join Button").gameObject.SetActive(false);
            _players[index].Find("Controller").gameObject.SetActive(true);
            _players[index].Find("Confirmed").gameObject.SetActive(false);

            ToggleConfirmation(index, false);
        }
    }

    // remove controller from the player list (moved to left)
    void RemovePlayer(int index)
    {
        if (_controllers[index].Selected)
        {
            _controllers[index].Selected = false;
            
            _players[index].Find("Join Button").gameObject.SetActive(true);
            _players[index].Find("Controller").gameObject.SetActive(false);
            _players[index].Find("Confirmed").gameObject.SetActive(false);

            ToggleConfirmation(index, false);
        }
    }

    // controller pressed start or was removed from the selected list
    void ToggleConfirmation(int index, bool enabled)
    {
        _controllers[index].Confirmed = enabled;
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
        foreach (var p in _controllers)
        {
            RemovePlayer(p.Id);
        }
    }
}
