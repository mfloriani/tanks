using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControllerMenu : MonoBehaviour
{
    [SerializeField] Transform _availableControllers;
    [SerializeField] Transform _selectedPlayers;
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
        }
        public int Id { get; set; }
        public bool Selected { get; set; }
        public bool Confirmed { get; set; }
        public bool Connected { get; set; }
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
        //foreach(var p in _controllers)
        //    Debug.Log(p.Id + ": " + p.Connected + " - " + p.Selected + " - " + p.Confirmed);


        if (!MenuManager.Instance.IsInsideMainMenu())
            return;

        int index = 0;
        var controllers = Input.GetJoystickNames();
        foreach (string s in controllers)
        {
            // controller moved left our right?
            float joyXaxis = Input.GetAxis("Joy" + (index+1) + "_Xaxis");
            if (joyXaxis > _deadZone)
                AddPlayer(index);
            else if (joyXaxis < -_deadZone)
                RemovePlayer(index);

            // controller was confirmed?
            string joyStartButton = "Joy" + (index + 1) + "_Start";
            if (Input.GetButtonDown(joyStartButton) && _controllers[index].Selected)
            {
                ToggleConfirmation(index, true);
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
                Debug.Log("START THE GAME");
                MenuManager.Instance.StartGame();
                Reset();
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
            _availableControllers.GetChild(index).gameObject.SetActive(false);
            _selectedPlayers.GetChild(index).gameObject.SetActive(true);
            ToggleConfirmation(index, false);
        }
    }

    // remove controller from the player list (moved to left)
    void RemovePlayer(int index)
    {
        if (_controllers[index].Selected)
        {
            _controllers[index].Selected = false;
            _availableControllers.GetChild(index).gameObject.SetActive(true);
            _selectedPlayers.GetChild(index).gameObject.SetActive(false);
            ToggleConfirmation(index, false);
        }
    }

    // controller pressed start or was removed from the selected list
    void ToggleConfirmation(int index, bool enabled)
    {
        var go = _selectedPlayers.GetChild(index).Find("Confirmed").gameObject;
        go.GetComponent<Image>().enabled = enabled;
        _controllers[index].Confirmed = enabled;

        // check if should start the countdown every time a controller confirms 
        if(enabled)
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
