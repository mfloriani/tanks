using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] Transform _p1;
    [SerializeField] Transform _p2;
    [SerializeField] Transform _p3;
    [SerializeField] Transform _p4;

    void Start()
    {
        ControllerManager.OnNewPlayerJoined += ActivatePlayerUI;
    }

    
    void Update()
    {
        
    }

    void OnEnable()
    {
        //_p1 = transform.Find("P1");
        //_p2 = transform.Find("P2");
        //_p3 = transform.Find("P3");
        //_p4 = transform.Find("P4");
        ActivatePlayersUI();
    }

    void OnDisable()
    {
        ResetPlayersUI();
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

    public void ResetPlayersUI()
    {
        _p1.Find("Controller").gameObject.SetActive(false);
        _p2.Find("Controller").gameObject.SetActive(false);
        _p3.Find("Controller").gameObject.SetActive(false);
        _p4.Find("Controller").gameObject.SetActive(false);
    }

    public void ActivatePlayersUI()
    {
        if (ControllerManager.Instance.IsPlayerActive(PlayerId.Player1))
            ActivatePlayerUI(PlayerId.Player1);

        if (ControllerManager.Instance.IsPlayerActive(PlayerId.Player2))
            ActivatePlayerUI(PlayerId.Player2);

        if (ControllerManager.Instance.IsPlayerActive(PlayerId.Player3))
            ActivatePlayerUI(PlayerId.Player3);

        if (ControllerManager.Instance.IsPlayerActive(PlayerId.Player4))
            ActivatePlayerUI(PlayerId.Player4);
    }
}
