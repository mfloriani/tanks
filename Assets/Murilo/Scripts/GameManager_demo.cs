using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Playing,
    GameOver
}

public class GameManager_demo : MonoBehaviour
{
    [SerializeField] GameObject TankPrefab;
    List<GameObject> _Players = new List<GameObject>();

    GameState _currentGameState;

    void Start()
    {
        ControllerManager.OnNewPlayerJoined += SpawnNewPlayer;

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
        if(_currentGameState == GameState.Playing)
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if(enemies.Length > 0)
            {
                int enemiesDead = 0;
            
                foreach(GameObject e in enemies)
                {
                    if(!e.GetComponent<SpriteRenderer>().enabled)
                    {
                        ++enemiesDead;
                    }
                }

                // All enemies are dead
                if(enemiesDead == enemies.Length)
                {
                    _currentGameState = GameState.GameOver;
                    Debug.LogWarning("GameOver! No enemies left in the game.");

                    MenuManager.Instance.ShowGameOverMenu();
                }
            }
        }
    }

    void SpawnNewPlayer(PlayerId id)
    {
        Debug.Log(id + " spawned");

        var t = Instantiate(TankPrefab);
        
        _Players.Add(t);

        t.GetComponent<ControllerInput>().player = ((int)id) - 1;
        t.GetComponent<ControllerInput>().setPlayer(((int)id)-1);


        WaitToSpawn(t);
    }

    IEnumerator WaitToSpawn(GameObject tank)
    {
        yield return new WaitForSeconds(1);
        tank.GetComponent<TankManager>().Spawn();
    }
}
