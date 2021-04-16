using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Scene = UnityEngine.SceneManagement.Scene;

public class GameManager : MonoBehaviour
{
    public int playerAmount;

    public Multiplayer1 _multiplayer1;

    public Multiplayer2 _multiplayer2;

    public UnityEngine.UI.Button _multi2;
    public UnityEngine.UI.Button play;
    public UnityEngine.UI.Button _multi1;

    // Start is called before the first frame update

    [SerializeField] Scene _scene;

    private void Start()
    {
    }

    public void loadScene()
    {
        SceneManager.LoadScene(1);
    }

    void SetGameMode1()
    {
        _multiplayer1.playerAmount = 4;
        loadScene();
    }

    void SetGameMode2()
    {
        _multiplayer2.playerAmount = 10;
        loadScene();
    }

    // Update is called once per frame
    void Update()
    {
        _multi1.onClick.AddListener(SetGameMode1);
        _multi2.onClick.AddListener(SetGameMode2);
    }
}