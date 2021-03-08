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

    bool fire;

    // Start is called before the first frame update
    void Start()
    {

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


        int rL = reverseL ? 1 : 0;
        tank.lTrack = speedL-rL;

        int rR = reverseR ? 1 : 0;
        tank.rTrack = speedR - rR;

        tank.firing = fire;

        tank.aimX = aimX;
        tank.aimY = aimY;

    }
}
