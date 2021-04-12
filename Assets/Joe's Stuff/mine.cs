using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mine : MonoBehaviour
{
    public const int playerLayer = 6;
    public const int enemyLayer = 7;

    private int _player;
    public int player
    {
        get { return _player; }
        set { _player = value; }
    }
    bool armed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (armed)
        {

        }
        else
            StartCoroutine(Wait());
    }
        IEnumerator Wait()
    {
            yield return new WaitForSeconds(1);         //wait for sound and explosion to play
            armed = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (armed)
        {
            switch (collision.gameObject.layer)
            {
                case playerLayer:
                    if (collision.gameObject.GetComponent<TankManager>() != null)
                    if (collision.gameObject.GetComponent<TankManager>() != null && collision.gameObject.GetComponent<TankManager>().player != player)
                    {
                        collision.gameObject.GetComponent<TankManager>().Die();
                        GetComponent<Collider2D>().enabled = false;
                            ++GameObject.Find("Tank " + player).GetComponent<TankManager>().score;
                            Debug.Log("Tank " + player + " has mined another player, and scored a point. Their score is now " + GameObject.Find("Tank " + player).GetComponent<TankManager>().score + "!");
                            Destroy(gameObject);
                    }
                    else
                        {
                            collision.gameObject.GetComponent<TankManager>().Die();
                            --GameObject.Find("Tank " + player).GetComponent<TankManager>().score;
                            GetComponent<Collider2D>().enabled = false;
                            Debug.Log("A mine went off!");
                            Debug.Log("Tank " + player + " has ran over their own mine, and lost a point. Their score is now " + GameObject.Find("Tank " + player).GetComponent<TankManager>().score + "... what an idiot!");
                            Destroy(gameObject);

                        }
                    break;
                case enemyLayer:
                    if (collision.gameObject.GetComponent<TankManager>() != null)
                    //if (collision.gameObject.GetComponent<TankManager>() != null && collision.gameObject.GetComponent<TankManager>().player != player)
                    {
                        GetComponent<Collider2D>().enabled = false;
                        collision.gameObject.GetComponent<TankManager>().Die();
                        Debug.Log("A mine went off!");
                        Destroy(gameObject);
                    }
                    break;
                default:
                    Debug.Log("Something hit the mine, but it didn't go off!");
                    break;
            }
            
        }
    }
}
