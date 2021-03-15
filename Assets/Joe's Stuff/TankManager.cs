using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankManager : MonoBehaviour
{
    public Firing turret;



    [SerializeField]
    private float _rTrack;      //speed for right track
    [SerializeField]
    private float _lTrack;      //speed for left track
        [SerializeField]
    private float _speed;
    private bool _firing;
    private bool _honking;
    private float _aimX;
    private float _aimY;
    private float _aim;
    private Rigidbody2D _body;
    [SerializeField]
    private Vector2 _movement;
    [SerializeField]
    private int _player;

    private GameObject _gun;
    private ParticleSystem _smoke;

    public Sprite[] bodySprites;
    public Sprite[] turretSprites;
    public Sprite[] shellSprites;
    public AudioClip[] hornSounds;
    public AudioClip deathSound;

    public enum powerUp         //enum for state of powerUp applied to tank
    {
        none,
        bounceBullet,
        powerBullet,
        mines
    }

    private powerUp _state;     //storage for powered up state
    private int _lives;         //integer for lives of tank
    private int _health;        //integer for health of tank

    private bool _readyToFire;  //boolean for readiness to fire
    [SerializeField]
    private bool _safe;         //boolean for use with safe zones
    private bool _dead;         //boolean for if tank is dead and waiting to respawn


    public powerUp state        //getters and setters for powerup state to be used by other classes
    {
        get { return _state; }
        set { _state = value; }
    }
    public bool safe            //getters and setters for safe state to be used by other classes
    {
        get { return _safe; }
        set { _safe = value; }
    }
    public bool dead            //getters and setters for dead state to be used by other classes
    {
        get { return _dead; }
        set { _dead = value; }
    }

    public bool firing            //getters and setters for dead state to be used by other classes
    {
        get { return _firing; }
        set { _firing = value; }
    }

    public bool honking            //getters and setters for dead state to be used by other classes
    {
        get { return _honking; }
        set { _honking = value; }
    }

    public float rTrack            //getters and setters for right track speed to be used by other classes
    {
        get { return _rTrack; }
        set { _rTrack = value; }
    }
    public float lTrack           //getters and setters for left track speed to be used by other classes
    {
        get { return _lTrack; }
        set { _lTrack = value; }
    }

    public float aim          //getters and setters for target angle to be used by other classes
    {
        get { return _aim; }
        set { _aim = value; }
    }


    public float smoke           //getters and setters for target angle to be used by other classes
    {
        get { return _smoke.emissionRate; }
        set { _smoke.emissionRate = value; }
    }

    public int player
    {
        get { return _player; }
        set { _player = value; }
    }




    private void setPlayer()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = bodySprites[_player];
        gameObject.GetComponent<AudioSource>().clip = hornSounds[_player];
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = turretSprites[_player];
        gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = shellSprites[_player];
        
    }

    private void Drive()    //this DOES work
    {

        float turn = -(rTrack - lTrack) * _speed * 5f;
        gameObject.transform.RotateAround(gameObject.transform.position,new Vector3(0f,0f,1f), turn);

        _movement = -(gameObject.transform.up) * (lTrack + rTrack) * _speed;
        _smoke.emissionRate = (_movement.magnitude * 100f);
        gameObject.transform.Translate(_movement * Time.deltaTime, Space.World);

    }

    private void Target()
    {


        aim = ((Mathf.Round(aim / 45) * 45));
        aim = -aim + 180;
        _gun.transform.eulerAngles = new Vector3(0f, 0f, aim);
    }
    
    public void Die()
    {
        if (safe)
        {
            Debug.Log("Die has been called, but tank with name \"" + this.name + "\" is safe! Spawncampers, eh?");
        }
        else
        {
            Debug.Log("Die has been called, tank with name \"" + this.name + "\" should now be dead");
            gameObject.GetComponent<AudioSource>().PlayOneShot(deathSound);                     //play the sound given in the editor to tankmanager
            gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().Play();             //access deathboom and play its particles
            gameObject.transform.GetChild(0).gameObject.SetActive(false);                       //disable the turret sprite renderer
            gameObject.GetComponent<SpriteRenderer>().sprite = null;                            //disables the tank body sprite renderer by setting its sprite to null
            gameObject.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(WaitForDeath());                                                     //waits two seconds for sound and explosion to play before destroying tank
        }
        }

    IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(2);         //wait for sound and explosion to play
        Destroy(gameObject);                        //delete the tank
    }
    void Awake()
    {
        _state = powerUp.none;
        _lives = 3;
        _health = 1;
        _body = gameObject.GetComponent<Rigidbody2D>();
        _readyToFire = false;
        _speed = 2.0f;
        _gun = gameObject.transform.GetChild(0).gameObject;
        _smoke = gameObject.GetComponent<ParticleSystem>();
        if (_player == null) _player = 0;
        setPlayer();
    }
    // Start is called before the first frame update
    void Start()
    {
        _state = powerUp.none;
        _lives = 3;
        _health = 1;
        _body = gameObject.GetComponent<Rigidbody2D>();
        _readyToFire = false;
        _speed = 2.0f;
        _gun = gameObject.transform.GetChild(0).gameObject;
        _smoke = gameObject.GetComponent<ParticleSystem>();
        if (_player == null) _player = 0;
        setPlayer();
        aim = 0;
    }

    void FixedUpdate()
    {
        Drive();

        if (aim <= 0.05)
        Target();

    }

    void Update()
    {
        if (_health <= 0)
            Die();

        if(firing)
        {
            turret.Fire(aim);
        }

        if (honking)
            gameObject.GetComponent<AudioSource>().PlayOneShot(gameObject.GetComponent<AudioSource>().clip);
    }


}