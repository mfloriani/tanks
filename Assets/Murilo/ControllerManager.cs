using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ControllerState
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

public enum PlayerId
{
    Player1 = 1,
    Player2,
    Player3,
    Player4
}

public class ControllerManager
{
    private static ControllerManager instance = null;
    public static ControllerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ControllerManager();
            }
            return instance;
        }
    }

    public delegate void NewPlayerJoined(PlayerId id);
    public static event NewPlayerJoined OnNewPlayerJoined;

    const int _maxPlayers = 4;

    ControllerState[] _controllers;

    public ControllerState[] GetControllers()
    {
        return _controllers;
    }

    private ControllerManager()
    {
        _controllers = new ControllerState[_maxPlayers];
        for (int i = 0; i < _maxPlayers; ++i)
            _controllers[i] = new ControllerState(i);
    }

    public void UpdateConnectedControllers()
    {
        int id = 0;
        foreach (string j in Input.GetJoystickNames())
        {
            // restrict max players
            if (id > _maxPlayers)
                break;

            _controllers[id].Connected = true;
            id++;
        }
    }

    public void HandleInGameJoinButton()
    {
        foreach (var c in _controllers)
        {
            if (c.Connected && !c.Selected)
            {
                //Debug.Log("[" + c.Id + "] => " + c.Connected + "|" + c.Selected + "|" + c.Confirmed);
                string joyButtonA = "Joy" + (c.Id + 1) + "_A";
                if (Input.GetButtonDown(joyButtonA))
                {
                    _controllers[c.Id].Selected = true;
                    _controllers[c.Id].Confirmed = true;
                    // dispatch event to inform the new player
                    if(OnNewPlayerJoined != null)
                    {
                        PlayerId id = (PlayerId)c.Id + 1;
                        OnNewPlayerJoined(id);
                    }
                }
            }
        }
    }

    public bool IsPlayerActive(PlayerId id)
    {
        int index = (int)id - 1;

        if(_controllers[index].Connected 
            && _controllers[index].Selected 
            && _controllers[index].Confirmed)
        {
            return true;
        }

        return false;
    }
}
