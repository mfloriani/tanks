using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject parent;

    // Variables
    public float moveSpeed;
    public Vector2 moveDir;
    public GameObject explosion;

    public enum bulletState
    {
        standard,
        power,
        bounce
    }

    [SerializeField] public bulletState currentState;

    public int wallLayer = 9;
    public int playerLayer = 6;
    public int enemyLayer = 7;
    public int destructibleWallLayer = 11;
    public int safeLayer = 15;

    int bounces = 0;

    

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
        if (currentState == bulletState.bounce)
            bounces = 4;
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
            if (bounces <= 0)
                Die();
            else
                --bounces;
        }
        if(collision.gameObject.layer == safeLayer)
        {
            Die();
        }
        if (collision.gameObject.layer == destructibleWallLayer)
        {
            if (currentState == bulletState.power)
            {
                // Insert more elaborate wall destruction here
                Destroy(collision.gameObject);
                GameObject newExplosion = Instantiate(explosion, collision.gameObject.transform.position, Quaternion.identity);
                newExplosion.GetComponent<ParticleSystem>().Play();
            }

            if (bounces <= 0)
                Die();
            else
            {
                //ROTATE THE BULLET HERE
            Vector2 wallNormal = collision.contacts[0].normal;  //get normalised vector of contact
            moveDir = Vector2.Reflect(gameObject.transform.up, wallNormal);     //figure out the angle of incidence from where the object has contacted the wall

                gameObject.transform.up = moveDir; //????
                Debug.Log(moveDir);
            --bounces;
        }
        }
    }

    public void SetParent(GameObject newParent)
    {
        parent = newParent;
    }

    public void Die()
    {
        parent.GetComponent<Firing>().currentBullets.Remove(gameObject);
        Destroy(gameObject);
    }
}