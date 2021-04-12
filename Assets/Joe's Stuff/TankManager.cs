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
    private bool _mining;
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
    public int score;
    private float _target;
    private GameObject _gun;
    private ParticleSystem _smoke;
    public GameObject mine;
    public Sprite[] bodySprites;
    public Sprite[] turretSprites;
    public Sprite[] shellSprites;
    public AudioClip[] hornSounds;
    public AudioClip deathSound;
    public AudioClip spawnSound;
    public AudioClip cooldownSound;
    public AudioClip dropSound;
    public GameObject[] spawnPoints;
    public Sprite[] lifecounter;
    private float frac = 0;

    public bool ai;

    Vector3 spawnPos;
    Vector3 deathPos;
    private int _minecount;
public bool hot;

    public enum type         //enum for state of powerUp applied to tank
    {
        none,
        bounceBullet,
        powerBullet,
        mines,
        multiShot
    }

    private type _state;     //storage for powered up state
    private int _lives;         //integer for lives of tank
    private int _health;        //integer for health of tank

    private bool _readyToFire;  //boolean for readiness to fire
    private bool _safe;         //boolean for use with safe zones
    private bool _dead;         //boolean for if tank is dead and waiting to respawn



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

    public type pUpState            //getters and setters for dead state to be used by other classes
    {
        get { return _state; }
        set { _state = value; }
    }
    public bool mining           //getters and setters for dead state to be used by other classes
    {
        get { return _mining; }
        set { _mining = value; }
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

    public float aimX          //getters and setters for left track speed to be used by other classes
    {
        get { return _aimX; }
        set { _aimX = value; }
    }
    public float aimY           //getters and setters for left track speed to be used by other classes
    {
        get { return _aimY; }
        set { _aimY = value; }
    }



    public float target           //getters and setters for target angle to be used by other classes
    {
        get { return _target; }
        set { _target = value; }
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

    public int minecount
    {
        get { return _minecount; }
        set { _minecount = value; }
    }


    private float AimAngle()              //convert thumbstick axes data into angle (degrees) for aiming turret
    {
        float radAngle = Mathf.Atan2(aimX, aimY);
        if (radAngle < 0.0f) radAngle += (Mathf.PI * 2.0f);
        float angle = (180.0f * radAngle / Mathf.PI);

        return angle;
    }

    private void setPlayer()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = bodySprites[_player];
        gameObject.GetComponent<AudioSource>().clip = hornSounds[_player];
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = turretSprites[_player];
        gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = shellSprites[_player];
        gameObject.name = ("Tank " + player);
        if(!ai)
        gameObject.GetComponentInChildren<lifeCounter>().setPlayer(player);
        else
            Destroy(gameObject.GetComponentInChildren<lifeCounter>().gameObject);



    }

    private void Drive()    //this DOES work
    {

        float turn = -(rTrack - lTrack) * _speed * 5f;
        gameObject.transform.RotateAround(gameObject.transform.position, new Vector3(0f, 0f, 1f), turn);

        _movement = -(gameObject.transform.up) * (lTrack + rTrack) * _speed;
        _smoke.emissionRate = (_movement.magnitude * 100f);
        gameObject.transform.Translate(_movement * Time.deltaTime, Space.World);

    }

    private void Target()
    {
        target = ((Mathf.Round(AimAngle() / 45) * 45));
        target = -target + 180;
        _gun.transform.eulerAngles = new Vector3(0f, 0f, target);
    }

    public void Die()
    {
        if (!safe && !dead)
        {
            Debug.Log("Die has been called, tank with name \"" + this.name + "\" should now be dead");
            gameObject.GetComponent<AudioSource>().PlayOneShot(deathSound);                     //play the sound given in the editor to tankmanager
            GameObject deathBoom = Instantiate(gameObject.transform.GetChild(1).gameObject, gameObject.transform);             //access deathboom and play its particles
            deathBoom.GetComponent<ParticleSystem>().Play();
            gameObject.transform.GetChild(0).gameObject.SetActive(false);                       //disable the turret sprite renderer
            gameObject.GetComponent<SpriteRenderer>().enabled = false;                            //disables the tank body sprite renderer by setting its sprite to null
            gameObject.GetComponent<ControllerInput>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            rTrack = 0;
            lTrack = 0;
            StartCoroutine(WaitForRespawn(deathBoom));                                                     //waits two seconds for sound and explosion to play before destroying tank
            if (!ai)
            {
                --_lives;
                Debug.Log(gameObject.name + " is dead, and will respawn with " + _lives + " lives. Try to dodge next time!");
            }
            }
        else
        {
            Debug.Log("Die has been called, tank with name \"" + this.name + "\" - but he was safe! Spawncampers, eh?");
        }
    }

    IEnumerator WaitForRespawn(GameObject db)
    {
        GetComponentInChildren<lifeCounter>().changeVis(true); // make the life counter visible
        yield return new WaitForSeconds(1);         //wait for sound and explosion to play
        gameObject.GetComponentInChildren<lifeCounter>().updateCount(_lives);
        yield return new WaitForSeconds(1);         //wait for sound and explosion to play
        
        Destroy(db);                        //delete the explosion
        if (_lives <= 0 )
        {
            Debug.Log(gameObject.name + " is dead, and won't be coming back. Game over man, game over!");
            try { GetComponentInChildren<lifeCounter>().changeVis(false); }
            catch { Debug.Log(gameObject + " tried to access its lifecounter, but it had an error - maybe it's intentional that " +gameObject.name+ " didn't have one?");  }
        }
        else
        {
            GameObject spawn = spawnPoints[(int)Random.Range(0, spawnPoints.Length)].gameObject;
            while (spawn.GetComponent<safezone>() != null && spawn.GetComponent<safezone>().full == true)
            {
                spawn = spawnPoints[(int)Random.Range(0, spawnPoints.Length)].gameObject;
            }
            spawnPos = spawn.transform.position;
            deathPos = gameObject.transform.position;
            dead = true;
        }
    }

    public void Spawn()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);                       //disable the turret sprite renderer
        gameObject.GetComponent<SpriteRenderer>().enabled = false;                            //disables the tank body sprite renderer by setting its sprite to null
        gameObject.GetComponent<ControllerInput>().enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        rTrack = 0;
        lTrack = 0;

        GameObject spawn = spawnPoints[(int)Random.Range(0, spawnPoints.Length)].gameObject;
        while (spawn.GetComponent<safezone>() != null && spawn.GetComponent<safezone>().full == true)
        {
            spawn = spawnPoints[(int)Random.Range(0, spawnPoints.Length)].gameObject;
        }
        spawnPos = spawn.transform.position;
        gameObject.transform.position = new Vector3(0,0,0);
        deathPos = gameObject.transform.position;
        setPlayer();
        dead = true;

    }

    void Awake()
    {
        _state = type.none;
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
        _state = type.none;
        _lives = 3;
        _health = 1;
        _body = gameObject.GetComponent<Rigidbody2D>();
        _readyToFire = false;
        _speed = 2.0f;
        _gun = gameObject.transform.GetChild(0).gameObject;
        _smoke = gameObject.GetComponent<ParticleSystem>();
        if (_player == null) _player = 0;

        if (ai == null) ai = false;

        if (ai)
        {
            spawnPoints = GameObject.FindGameObjectsWithTag("AI_Spawn");
            _lives = 10;
        }
        else
            spawnPoints = GameObject.FindGameObjectsWithTag("Platform");


        setPlayer();

        GetComponentInChildren<lifeCounter>().changeVis(false);
    }

    void FixedUpdate()
    {
        Drive();
        if (aimX + aimY != 0)
            Target();

        if (dead)
        {
            frac += Time.deltaTime;
            transform.position = Vector3.Lerp(deathPos, spawnPos, frac);
            if (frac >= 1)
                {
                frac = 0f;
                dead = false;

                    gameObject.GetComponent<Collider2D>().enabled = true;
               
                    gameObject.GetComponent<SpriteRenderer>().enabled = true;                            //disables the tank body sprite renderer by setting its sprite to null
                    gameObject.GetComponent<ControllerInput>().enabled = true;
                    gameObject.transform.GetChild(0).gameObject.SetActive(true);                       //disable the turret sprite renderer
                    GetComponentInChildren<lifeCounter>().changeVis(false);
                    gameObject.GetComponent<AudioSource>().PlayOneShot(spawnSound, 0.7f);                     //play the sound given in the editor to tankmanager
            }
        }
    }

    IEnumerator Wait(int secs)
    {
        yield return new WaitForSeconds(secs);
    }
    void Update()
    {
        if (_health <= 0)
            Die();

        if (firing)
        {
            turret.Fire(target, pUpState, player);
        }

        if (mining && pUpState == type.mines && !(minecount <= 0))
        {
            Debug.Log("A mine has been laid by tank " + gameObject.name);
            GameObject newMine = Instantiate(mine, gameObject.transform);
            gameObject.GetComponent<AudioSource>().PlayOneShot(dropSound, 0.8f);
            newMine.SetActive(true);
            newMine.GetComponent<mine>().player = player;
            newMine.transform.parent = null;
            --minecount;
            if (minecount <= 0)
                pUpState = type.none;
        }

        if(pUpState != type.none && hot)
        {
            hot = false;
            
            StartCoroutine(cooldown());
        }


        if (honking)
            gameObject.GetComponent<AudioSource>().PlayOneShot(gameObject.GetComponent<AudioSource>().clip);


    }

    IEnumerator cooldown()
    {
        GameObject ticker = new GameObject();
        ticker.AddComponent<AudioSource>();
        GameObject clock = Instantiate(ticker);
        Debug.Log("Cooling down after pickup!");
        clock.GetComponent<AudioSource>().PlayOneShot(cooldownSound, 0.8f);
        yield return new WaitForSeconds(10);         //wait for sound and explosion to play
            this.pUpState = type.none;
        Destroy(ticker);
        Destroy(clock);
            Debug.Log("Tank has cooled down!");
    }
}

