using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour
{
    public TankManager tank;

    float aimX;
    float aimY;
    float speedL;
    float speedR;
    bool reverseL;
    bool reverseR;
    int player = 0;

    bool fire;
    bool honk;

    // Start is called before the first frame update
    void Start()
    {
        tank.player = player;
    }

    // Update is called once per frame
    void Update()
    {
        aimX = Input.GetAxis("LeftStickX");
        aimY = Input.GetAxis("LeftStickY");
        speedL = Input.GetAxis("LeftTrigger");
        speedR = Input.GetAxis("RightTrigger");
        reverseL = Input.GetButton("LeftBumper");
        reverseR = Input.GetButton("RightBumper");

        fire = Input.GetButton("Fire");
        honk = Input.GetButtonDown("Honk");

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
