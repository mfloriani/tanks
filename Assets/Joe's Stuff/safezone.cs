using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class safezone : MonoBehaviour
{


    private int wallLayer = 9;
    public int playerLayer = 6;
    public int bulletLayer = 5;
    public int enemyLayer = 7;
    public int destructibleWallLayer = 11;
    private bool _hasPlayer = false;
    private int players = 0;
    public bool full
    {
        get { return _hasPlayer; }
        set { _hasPlayer = value; }
    }

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
            ++players;

        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            if (collision.gameObject.GetComponent<TankManager>() != null)
                collision.gameObject.GetComponent<TankManager>().safe = false;
            Debug.Log(collision.gameObject.name + " left the safezone!");
            --players;
        }
    }
    // Update is called once per frame
    void Update()
    {
        full = !(players == 0);
        
    }
}
