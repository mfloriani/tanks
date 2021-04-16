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
    public GameObject[] tanks;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < playerAmount; i++)
        {
            tanks[i] = Instantiate(tank);
           
            tanks[i].GetComponent<TankManager>().Spawn();
           tanks[i].GetComponent<ControllerInput>().player = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    
    
    
}