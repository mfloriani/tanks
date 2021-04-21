using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    Transform _p1;
    Transform _p2;
    Transform _p3;
    Transform _p4;

    void Start()
    {
        ControllerManager.OnNewPlayerJoined += ActivatePlayerUI;
    }

    
    void Update()
    {
        UpdateScores();
    }

    public void UpdateScores()
    {
        GameManager gm = GameObject.FindObjectOfType<GameManager>();
        if (gm)
        {
            var scores = gm.GetScore();

            _p1.Find("Joined").Find("Score").GetComponent<TextMeshProUGUI>().text = scores[0].ToString();
            _p2.Find("Joined").Find("Score").GetComponent<TextMeshProUGUI>().text = scores[1].ToString();
            _p3.Find("Joined").Find("Score").GetComponent<TextMeshProUGUI>().text = scores[2].ToString();
            _p4.Find("Joined").Find("Score").GetComponent<TextMeshProUGUI>().text = scores[3].ToString();

        }
    }

    void OnEnable()
    {
        _p1 = transform.Find("PlayerGameUI 1");
        _p2 = transform.Find("PlayerGameUI 2");
        _p3 = transform.Find("PlayerGameUI 3");
        _p4 = transform.Find("PlayerGameUI 4");

        if(MenuManager.Instance.PlayerAvatar.Length == 0)
        {
            Debug.LogError("No player avatars set in the GameManager gameobject!");
        }

        _p1.Find("Joined").Find("Controller").gameObject.GetComponent<Image>().sprite = MenuManager.Instance.PlayerAvatar[0];
        _p2.Find("Joined").Find("Controller").gameObject.GetComponent<Image>().sprite = MenuManager.Instance.PlayerAvatar[1];
        _p3.Find("Joined").Find("Controller").gameObject.GetComponent<Image>().sprite = MenuManager.Instance.PlayerAvatar[2];
        _p4.Find("Joined").Find("Controller").gameObject.GetComponent<Image>().sprite = MenuManager.Instance.PlayerAvatar[3];

        if (MenuManager.Instance.GetSelectedMode() == GameMode.Arcade)
        {
            ActivatePlayerUI(PlayerId.Player1);
            DeactivateMultiplayerUI();
        }
        else
        {
            ActivatePlayersUI();
        }
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

                _p1.Find("Join").gameObject.SetActive(false);
                _p1.Find("Joined").gameObject.SetActive(true);
                break;

            case PlayerId.Player2:

                _p2.Find("Join").gameObject.SetActive(false);
                _p2.Find("Joined").gameObject.SetActive(true);
                break;

            case PlayerId.Player3:

                _p3.Find("Join").gameObject.SetActive(false);
                _p3.Find("Joined").gameObject.SetActive(true);
                break;

            case PlayerId.Player4:

                _p4.Find("Join").gameObject.SetActive(false);
                _p4.Find("Joined").gameObject.SetActive(true);
                break;
        }
    }

    void DeactivateMultiplayerUI()
    {
        _p2.gameObject.SetActive(false);
        _p2.Find("Join").gameObject.SetActive(false);
        _p2.Find("Joined").gameObject.SetActive(false);

        _p3.gameObject.SetActive(false);
        _p3.Find("Join").gameObject.SetActive(false);
        _p3.Find("Joined").gameObject.SetActive(false);

        _p4.gameObject.SetActive(false);
        _p4.Find("Join").gameObject.SetActive(false);
        _p4.Find("Joined").gameObject.SetActive(false);
    }

    public void ResetPlayersUI()
    {
        _p1.gameObject.SetActive(true);
        _p1.Find("Join").gameObject.SetActive(true);
        _p1.Find("Joined").gameObject.SetActive(false);

        _p2.gameObject.SetActive(true);
        _p2.Find("Join").gameObject.SetActive(true);
        _p2.Find("Joined").gameObject.SetActive(false);

        _p3.gameObject.SetActive(true);
        _p3.Find("Join").gameObject.SetActive(true);
        _p3.Find("Joined").gameObject.SetActive(false);

        _p4.gameObject.SetActive(true);
        _p4.Find("Join").gameObject.SetActive(true);
        _p4.Find("Joined").gameObject.SetActive(false);
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
