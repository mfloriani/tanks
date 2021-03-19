using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour
{
    public TankManager tank;

    private float aimX;
    private float aimY;
    private float speedL;
    private float speedR;
    private bool reverseL;
    private bool reverseR;
    [SerializeField] private int _player = 0;

    private bool fire;
    private bool honk;
    private string playerprefix = "";

    public int player
    {
        get { return _player; }
        set { _player = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        tank.player = player;

        playerprefix = "Joy" + (player+1) + "_";
    }

    // Update is called once per frame
    void Update()
    {
        aimX = Input.GetAxis(playerprefix + "Xaxis");
        aimY = Input.GetAxis(playerprefix + "Yaxis");
        speedL = Input.GetAxis(playerprefix + "LT");
        speedR = Input.GetAxis(playerprefix + "RT");
        reverseL = Input.GetButton(playerprefix + "LB");
        reverseR = Input.GetButton(playerprefix + "RB");

        fire = Input.GetButton( playerprefix + "A");
        honk = Input.GetButtonDown(playerprefix + "B");

        int rL = reverseL ? 1 : 0;
        tank.lTrack = speedL-rL;

        int rR = reverseR ? 1 : 0;
        tank.rTrack = speedR - rR;

        tank.firing = fire;

        tank.honking = honk;

        tank.aimX = aimX;
        tank.aimY = aimY;

    }
}
