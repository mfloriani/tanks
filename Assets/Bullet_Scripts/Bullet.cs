using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject parent;

    // Variables
    public float moveSpeed;
    public Vector2 moveDir;
    GameObject explosion;

    [SerializeField] public bulletState currentState;

    public enum bulletState
    {
        standard,
        power,
        bounce
    }

    bulletState currentState;

    private int wallLayer = 9;
    public int playerLayer = 6;
    public int enemyLayer = 7;
    public int destructibleWallLayer = 11;

    private void Awake()
    {
        explosion = (GameObject)Resources.Load("DeathBoom");
    }


    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void SetBulletState(bulletState newState)
    {
        currentState = newState;
    }

    private void FixedUpdate()
    {
        transform.Translate(gameObject.transform.up * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == playerLayer)
        {        
            if(collision.gameObject.GetComponent<TankManager>() != null)
                collision.gameObject.GetComponent<TankManager>().Die();
            Die();
        }
        if(collision.gameObject.layer == enemyLayer)
        {
            if (collision.gameObject.GetComponent<TankManager>() != null)
                collision.gameObject.GetComponent<TankManager>().Die();
            Die();
        }
        if(collision.gameObject.layer == wallLayer)
        {
            Die();
        }
        if(collision.gameObject.layer == destructibleWallLayer)
        {
            if(currentState == bulletState.power)
            {
                // Insert more elaborate wall destruction here
                Destroy(collision.gameObject);

                GameObject newExplosion = Instantiate(explosion, collision.gameObject.transform.position, Quaternion.identity);
                newExplosion.GetComponent<ParticleSystem>().Play();

            }
            Die();
        }
    }

    public void SetParent(GameObject newParent)
    {
        parent = newParent;
    }

    private void Die()
    {
        //parent.GetComponent<Firing>().currentBullets.Remove(gameObject);
        Destroy(gameObject);
    }
}