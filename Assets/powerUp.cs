using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour
{


    public Sprite[] pickupSprites;

    private int wallLayer = 9;
    public int playerLayer = 6;
    public int enemyLayer = 7;
    public int destructibleWallLayer = 11;
    public enum type         //enum for state of powerUp applied to tank
    {
        bounceBullet,
        powerBullet,
        mines
    }

    type effect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        int i = Random.Range(0, 2);
        effect = (type)i; 
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            if (collision.gameObject.GetComponent<TankManager>() != null) Debug.Log("powerup tripped");
           //     collision.gameObject.GetComponent<TankManager>();
        }   
    }
}
