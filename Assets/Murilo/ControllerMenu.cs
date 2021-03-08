using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var controllers = Input.GetJoystickNames();
        foreach(string s in controllers)
        {
            Debug.Log(s);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
