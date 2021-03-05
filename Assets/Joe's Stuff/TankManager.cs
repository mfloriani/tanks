using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankManager : MonoBehaviour
{

    private float _rTrack;      //speed for right track
    private float _lTrack;      //speed for left track
    private float _speed;
    private bool _firing;
    private float _aimX;
    private float _aimY;
    private float _aim;
    private Rigidbody2D _body;

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

    private float aimAngle()              //convert thumbstick axes data into angle (degrees) for aiming turret
    {
        float radAngle = Mathf.Atan2(aimX, aimY);
        if (radAngle < 0.0f) radAngle += (Mathf.PI * 2.0f);
        float angle = (180.0f * radAngle / Mathf.PI);

        return angle;
    }

    private void reload()
    {

    }

    private void drive()    //this don't work
    {
        float turn = (lTrack - rTrack) * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, 1f, 0f);
        _body.MoveRotation(Quaternion.Euler(_body.transform.eulerAngles) * turnRotation);
        Vector2 movement = transform.forward * (lTrack + rTrack) * _speed;
        _body.MovePosition(_body.position * 2 * Time.fixedDeltaTime);


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
    }

    private void FixedUpdate()
    {
        drive();
    }
    void Die()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //drive();

        if (firing && _readyToFire)
        {

        }
        
        reload();

        if (_health <= 0)
            Die();


    }


}