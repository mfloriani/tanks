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
    public AudioClip sfx;
    TankManager.type effect;
    Sprite[] spritelist;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        int i = Random.Range(1, 4);
        effect = (TankManager.type)i;
        gameObject.GetComponent<SpriteRenderer>().sprite = spritelist[i];

    }

    // Update is called once per frame
    void Update()
    {
 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            if (collision.gameObject.GetComponent<TankManager>() != null && collision.gameObject.GetComponent<TankManager>().pUpState == TankManager.type.none)
            {
                Debug.Log("Powerup tripped! Tank with name " + collision.gameObject.name + " should now have the powerup " + effect);
                collision.gameObject.GetComponent<TankManager>().minecount = 5;
                collision.gameObject.GetComponent<TankManager>().pUpState = effect;
                gameObject.GetComponent<AudioSource>().PlayOneShot(sfx);
                gameObject.GetComponent<Collider2D>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;

            }
        }   
    }

    IEnumerable wait()
    {
            yield return new WaitForSeconds(1);         //wait for sound and explosion to play
        gameObject.GetComponent<Collider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
