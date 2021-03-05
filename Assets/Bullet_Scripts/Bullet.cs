using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Variables
    public float moveSpeed;
    public Vector2 moveDir;

    private int wallLayer = 8;
    public int playerLayer = 6;
    public int enemyLayer = 7;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == playerLayer || collision.gameObject.layer == enemyLayer)
        {

        }
    }
}
