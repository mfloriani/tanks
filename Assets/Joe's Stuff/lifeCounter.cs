using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifeCounter : MonoBehaviour
{
    float fadeTime = 0f;
    bool fade = false;
    public Sprite[] icons;
    SpriteRenderer sprite;
    int player;
    Quaternion up;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = icons[3];
        transform.rotation = Quaternion.identity;
        transform.position = (transform.parent.position + new Vector3(0f, 1f, 0f));
        up = transform.rotation;
        
    }
    public void setPlayer(int plr) {
        player = plr;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        switch (player)
        {
            case 0:
                sprite.color = new Color(0.905f,0.298f,0.235f);
                break;
            case 1:
                sprite.color = new Color(0.254f,0.623f,0.867f);
                break;
            case 2:
                sprite.color = new Color(0.960f, 0.882f, 0.705f);
                break;
            case 3:
                sprite.color = new Color(0.180f, 0.8f, 0.443f);
                break;
            default:
                sprite.color = new Color(0.6f, 0.6f, 0.6f);
                break;
        }
}

public void updateCount(int lives)
    {
        if (lives < 0) lives = 0;
        sprite.sprite = icons[lives];
        //Debug.Log("Life counter updated to have " + lives + " lives");
    }
    // Update is called once per frame
    void Update()
    {
       
    }


    public void changeVis(bool endVis)
    {
        if (endVis)
        {
            sprite.enabled = true;
        }
        else
        {
            fadeTime += 3.5f;
        fade = true;
        }  //turnOnThenOff();
    }
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.identity;
        gameObject.transform.position = (transform.parent.position + new Vector3(0f,1f,0f)) ;

        if (fade)
        {
            sprite.enabled = true;
            fadeTime -= Time.deltaTime;
            if (fadeTime <= 0f)
            {
                sprite.enabled = false;
                fade = false;
            }
        }
    }
}
