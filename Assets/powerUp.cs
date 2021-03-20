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

    // Start is called before the first frame update
    void Start()
    {
        int i = Random.Range(0, 3);
        effect = (TankManager.type)(i + 1);
        gameObject.GetComponent<SpriteRenderer>().sprite = pickupSprites[i];
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(wait());
    }

    private void Awake()
    {


    }

    // Update is called once per frame
    void Update()
    {
           
    }

    private void FixedUpdate()
    {
        gameObject.transform.Rotate(0,0,Time.deltaTime*15);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            if (collision.gameObject.GetComponent<TankManager>() != null && collision.gameObject.GetComponent<TankManager>().pUpState == TankManager.type.none)
            {
                Debug.Log("Powerup tripped! Tank with name " + collision.gameObject.name + " should now have the powerup " + effect);
                if(effect == TankManager.type.mines)collision.gameObject.GetComponent<TankManager>().minecount = 5;
                collision.gameObject.GetComponent<TankManager>().pUpState = effect;
                collision.gameObject.GetComponent<TankManager>().hot = true;
                gameObject.GetComponent<AudioSource>().PlayOneShot(sfx);
                gameObject.GetComponent<Collider2D>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                int i = Random.Range(0, 3);
                effect = (TankManager.type)(i + 1);
                gameObject.GetComponent<SpriteRenderer>().sprite = pickupSprites[i];
                StartCoroutine(wait());

            }
        }   
    }

    IEnumerator wait()
    {
            yield return new WaitForSeconds(15);         //wait for sound and explosion to play
        gameObject.GetComponent<Collider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;

    }
}
