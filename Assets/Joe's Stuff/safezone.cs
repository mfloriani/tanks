using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class safezone : MonoBehaviour
{


    private int wallLayer = 9;
    public int playerLayer = 6;
    public int enemyLayer = 7;
    public int destructibleWallLayer = 11;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            if (collision.gameObject.GetComponent<TankManager>() != null)
                collision.gameObject.GetComponent<TankManager>().safe = true;
            Debug.Log(collision.gameObject.name + " entered the safezone!");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            if (collision.gameObject.GetComponent<TankManager>() != null)
                collision.gameObject.GetComponent<TankManager>().safe = false;
            Debug.Log(collision.gameObject.name + " left the safezone!");

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
