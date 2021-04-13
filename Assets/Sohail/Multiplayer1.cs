using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector3 = UnityEngine.Vector3;

public class Multiplayer1 : MonoBehaviour
{
    public TankManager tank;
    public int playerAmount;
    private ControllerInput _controllerInput;


    private  GameObject[] _positions; 
    public TankManager[] tanks;

    // Start is called before the first frame update
    void Start()
    {
        
        _positions = GameObject.FindGameObjectsWithTag("Platform");
        for (int i = 0; i < playerAmount; i++)
        {
           tanks[i]= Instantiate(tank);
            for (int j = 0; j < _positions.Length; j++)
            {
                tanks[i].transform.position = _positions[j].transform.position;
                _positions.ToList().RemoveAt(j);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tanks.Length == 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
        
    }
}