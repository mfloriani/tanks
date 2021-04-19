using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Playing,
    GameOver
}

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject TankPrefab;
    [SerializeField] GameObject AITankPrefab;
    GameState _currentGameState;
    HashSet<PlayerId> _NewPlayers = new HashSet<PlayerId>();

    void Start()
    {
        ControllerManager.OnNewPlayerJoined += NewPlayerInTheGame;

        if (ControllerManager.Instance.IsPlayerActive(PlayerId.Player1))
            SpawnNewPlayer(PlayerId.Player1);
        if (ControllerManager.Instance.IsPlayerActive(PlayerId.Player2))
            SpawnNewPlayer(PlayerId.Player2);
        if (ControllerManager.Instance.IsPlayerActive(PlayerId.Player3))
            SpawnNewPlayer(PlayerId.Player3);
        if (ControllerManager.Instance.IsPlayerActive(PlayerId.Player4))
            SpawnNewPlayer(PlayerId.Player4);

        _currentGameState = GameState.Playing;
    }

    void Update()
    {
        HandleNewPlayer();

        if (_currentGameState == GameState.Playing)
        {
            GameMode mode = GameMode.None;
            //try catch added to avoid error when level not loaded from the menu
            try
            {
                mode = MenuManager.Instance.GetSelectedMode();
            }
            catch { }

            switch(mode)
            {
                case GameMode.Arcade:
                    HandleArcadeMode();
                    break;

                case GameMode.Multiplayer:
                    HandleMultiplayerMode();
                    break;

                case GameMode.BattleRoyale:
                    HandleBattleRoyaleMode();
                    break;

                default:
                    break;
            }
        }
    }

    void HandleNewPlayer()
    {
        if (_NewPlayers.Count > 0)
        {
            //Debug.Log("Total new players: " + _NewPlayers.Count);
            foreach (var id in _NewPlayers)
            {
                SpawnNewPlayer(id);
            }
            _NewPlayers.Clear();
        }
    }

    int TotalPlayersInScene()
    {
        int total = 0;
        for (int i = 0; i < 4; ++i)
        {
            var tank = GameObject.Find("/Tank " + i);
            if (tank) // tank is in the game?
            {
                total += 1;
            }
        }
        return total;
    }

    int TotalPlayersAlive()
    {
        int total = 0;
        for (int i = 0; i < 4; ++i)
        {
            var tank = GameObject.Find("/Tank " + i);
            if (tank) // tank is in the game?
            {
                bool isInHell = tank.GetComponent<TankManager>().IsTankInHell();
                if (!isInHell) // is it dead for good?
                {
                    total += 1;
                }
            }
        }
        return total;
    }

    int GetLastPlayerAlive()
    {
        int playerId = -1;
        for (int i = 0; i < 4; ++i)
        {
            var tank = GameObject.Find("/Tank " + i);
            if (tank) // tank is in the game?
            {
                bool isInHell = tank.GetComponent<TankManager>().IsTankInHell();
                if (!isInHell) // is it dead for good?
                {
                    playerId = i;
                }
            }
        }
        return playerId;
    }

    private void HandleBattleRoyaleMode()
    {
        int totalAliveInTheGame = 0;

        int totalEnemies = GetTotalEnemies();
        if(totalEnemies > 0)
        {
            int totalDeadEnemies = GetTotalDeadEnemies();
            totalAliveInTheGame += (totalEnemies - totalDeadEnemies);
        }

        int playersAlive = TotalPlayersAlive();
        totalAliveInTheGame += playersAlive;
        
        bool hasPlayerAlive = (playersAlive > 0);
        Debug.Log("hasPlayerAlive: " + hasPlayerAlive + " - totalPlayersAlive: "+ playersAlive);

        int playerId = -1;
        if (hasPlayerAlive && totalAliveInTheGame == 1)
            playerId = GetLastPlayerAlive();

        //Debug.Log(totalAliveInTheGame + " - " + hasPlayerAlive);

        if(totalAliveInTheGame == 0)
        {
            _currentGameState = GameState.GameOver;
            MenuManager.Instance.ShowGameOverMenu("WTF JUST HAPPENED?", Color.white);
        }
        else if (!hasPlayerAlive && TotalPlayersInScene() > 0)
        {
            _currentGameState = GameState.GameOver;
            MenuManager.Instance.ShowGameOverMenu("YOU LOST", Color.white);
        }
        if (hasPlayerAlive && totalAliveInTheGame == 1)
        {
            _currentGameState = GameState.GameOver;
            MenuManager.Instance.ShowGameOverMenu("PLAYER "+ (playerId+1) + " WON", GetPlayerColor(playerId));
        }
        //else
        //{
        //    MenuManager.Instance.HideGameOverMenu();
        //}
    }

    private void HandleMultiplayerMode()
    {
        int totalEnemies = GetTotalEnemies();
        if (totalEnemies > 0)
        {
            int totalDeadEnemies = GetTotalDeadEnemies();
            // All enemies are dead? if so, GAMEOVER
            if (totalEnemies == totalDeadEnemies)
            {
                _currentGameState = GameState.GameOver;

                var scores = GetScore();

                // check who has the biggest score
                int maxScoreId = -1;
                int maxScore = 0;
                for (int i = 0; i < scores.Length; ++i)
                {
                    if (scores[i] > maxScore)
                    {
                        maxScore = scores[i];
                        maxScoreId = i;
                    }
                }

                // check if more than one player has the highest score
                bool tie = false;
                for (int i = 0; i < scores.Length; ++i)
                {
                    // avoid comparing with itself
                    if (i != maxScoreId && scores[i] == maxScore)
                    {
                        tie = true;
                    }
                }
                                
                if (maxScoreId == -1) // do we have a winner?
                {
                    MenuManager.Instance.ShowGameOverMenu("YOU FAILED", Color.white);
                }
                if (tie) // more than one player with the highest score
                {
                    MenuManager.Instance.ShowGameOverMenu("TIE", Color.white);
                }
                else // someone was better
                {
                    MenuManager.Instance.ShowGameOverMenu("PLAYER " + (maxScoreId + 1) + " WON", GetPlayerColor(maxScoreId));
                }
            }
            else // there are enemies, but are there players?
            {
                int playersAlive = TotalPlayersAlive();
                if(playersAlive == 0 && TotalPlayersInScene() > 0)
                {
                    _currentGameState = GameState.GameOver;
                    MenuManager.Instance.ShowGameOverMenu("YOU FAILED", Color.white);
                }
                else
                {
                    MenuManager.Instance.HideGameOverMenu();
                }
            }
        }
    }

    Color GetPlayerColor(int id)
    {
        Color color;
        switch (id)
        {
            case 0: color = Color.red; break;
            case 1: color = Color.blue; break;
            case 2: color = Color.green; break;
            case 3: color = Color.yellow; break;
            default: color = Color.magenta; break;
        }

        return color;
    }

    void HandleArcadeMode()
    {
        int totalEnemies = GetTotalEnemies();
        if (totalEnemies > 0)
        {
            int totalDeadEnemies = GetTotalDeadEnemies();
            // All enemies are dead? if so, GAMEOVER
            if (totalEnemies == totalDeadEnemies)
            {
                _currentGameState = GameState.GameOver;
                MenuManager.Instance.ShowGameOverMenu("YOU WON", Color.red);
            }
            else // there are enemies, but are there players?
            {
                int playersAlive = TotalPlayersAlive();
                if (playersAlive == 0 && TotalPlayersInScene() > 0)
                {
                    Debug.Log("GAMEOVER FAILED");
                    _currentGameState = GameState.GameOver;
                    MenuManager.Instance.ShowGameOverMenu("YOU FAILED", Color.white);
                }
                else
                {
                    MenuManager.Instance.HideGameOverMenu();
                }
            }
        }        
    }

    int GetTotalEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    int GetTotalDeadEnemies()
    {
        int enemiesDead = 0;
        foreach (GameObject e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (!e.GetComponent<SpriteRenderer>().enabled)
            {
                ++enemiesDead;
            }
        }
        return enemiesDead;
    }

    public int[] GetScore()
    {
        int[] scores = new int[4];
        for(int i=0; i < 4; ++i)
        {
            scores[i] = 0; // initialize
            var go = GameObject.Find("/Tank " + i); //check if the tanks is in the scene
            if(go)
            {
                scores[i] = go.GetComponent<TankManager>().score;
            }
        }

        return scores;
    }


    void NewPlayerInTheGame(PlayerId id)
    {
        //Debug.Log("New player: " + (id - 1));
        _NewPlayers.Add(id);
    }

    void SpawnNewPlayer(PlayerId id)
    {
        int idFromZero = ((int)id) -1;

        var go = GameObject.Find("/Tank " + idFromZero);
        if(go)
        {
            Debug.LogError("Player " + idFromZero + " already in the game");
            return;
        }

        var t = Instantiate(TankPrefab);
        t.GetComponent<ControllerInput>().player = idFromZero;
        t.GetComponent<ControllerInput>().setPlayer(idFromZero);

        WaitToSpawn(t);
    }

    IEnumerator WaitToSpawn(GameObject tank)
    {
        yield return new WaitForSeconds(1);
        tank.GetComponent<TankManager>().Spawn();
    }
}
