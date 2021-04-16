using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector3 = UnityEngine.Vector3;

public class Multiplayer1 : MonoBehaviour
{
    public GameObject tank;
    public int playerAmount;
    private ControllerInput _controllerInput;


    public GameObject[] _positions;
    public GameObject[] tanks;

    // Start is called before the first frame update
    void Start()
    {
        _positions = GameObject.FindGameObjectsWithTag("Platform");
        for (int i = 0; i < playerAmount; i++)
        {
            tanks[i] = Instantiate(tank);
           
            tanks[i].GetComponent<TankManager>().Spawn();
           tanks[i].GetComponent<ControllerInput>().setPlayer(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}