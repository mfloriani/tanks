using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_demo : MonoBehaviour
{
    [SerializeField] GameObject TankPrefab;

    // Start is called before the first frame update
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnNewPlayer(PlayerId id)
    {
        Debug.Log(id + " spawned");

        var t = Instantiate(TankPrefab);
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
