using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    Transform _p1;
    Transform _p2;
    Transform _p3;
    Transform _p4;

    void Start()
    {
        ControllerManager.OnNewPlayerJoined += ActivatePlayerUI;

        _p1 = transform.Find("P1");
        _p2 = transform.Find("P2");
        _p3 = transform.Find("P3");
        _p4 = transform.Find("P4");

        if (ControllerManager.Instance.IsPlayerActive(PlayerId.Player1))
            ActivatePlayerUI(PlayerId.Player1);

        if (ControllerManager.Instance.IsPlayerActive(PlayerId.Player2))
            ActivatePlayerUI(PlayerId.Player2);

        if (ControllerManager.Instance.IsPlayerActive(PlayerId.Player3))
            ActivatePlayerUI(PlayerId.Player3);

        if (ControllerManager.Instance.IsPlayerActive(PlayerId.Player4))
            ActivatePlayerUI(PlayerId.Player4);
    }

    
    void Update()
    {
        //if(MenuManager.Instance.IsInGame())
        //{
        //    //if(ControllerManager.Instance.IsPlayerActive(PlayerId.Player1))
        //    //{

        //    //}
        //}
    }

    void ActivatePlayerUI(PlayerId id)
    {
        switch(id)
        {
            case PlayerId.Player1:

                _p1.Find("Controller").gameObject.SetActive(true);
                break;

            case PlayerId.Player2:

                _p2.Find("Controller").gameObject.SetActive(true);
                break;

            case PlayerId.Player3:

                _p3.Find("Controller").gameObject.SetActive(true);
                break;

            case PlayerId.Player4:

                _p4.Find("Controller").gameObject.SetActive(true);
                break;
        }
    }
}
