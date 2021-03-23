using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to anything that will require firing mechanics
public class Firing : MonoBehaviour
{
    // Firerate. Set this in the inspector as required.
    public float cooldown;
    float currentCooldown;
    int plr = 4;
    public bool hasMulti;
    Bullet.bulletState bulletType;

    // The bullet - set this to the bullet prefab in the inspector
    public GameObject placeholder;

    public List<GameObject> currentBullets;
    private float spawnOffset = 1; // testing value - not final

    // Start is called before the first frame update
    void Start()
    {
        // Set firing cooldown so firing can begin immediately
        currentBullets = new List<GameObject>();
        currentCooldown = cooldown;
        hasMulti = false;
        bulletType = Bullet.bulletState.standard;
    }

    // Update is called once per frame
    void Update()
    {
        currentCooldown += Time.deltaTime;
    }

    public void Fire(float zRotation, TankManager.type pUp, int player)

    {
        switch (pUp)
        {
            case TankManager.type.bounceBullet:
                bulletType = Bullet.bulletState.bounce;
                break;

            case TankManager.type.powerBullet:
                bulletType = Bullet.bulletState.power;
                break;

            case TankManager.type.multiShot:
                bulletType = Bullet.bulletState.standard;
                hasMulti = true;
                break;
            default:
                bulletType = Bullet.bulletState.standard;
                hasMulti = false;
                break;
        }
        Fire(zRotation, player);
    }
    // Call this function when you want the object to fire a bullet.
    public void Fire(float zRotation, int player) // Remove zRotation when convenient
    {
        plr = player;
        Fire(zRotation);
    }
    // Call this function when you want the object to fire a bullet.
    public void Fire(float zRotation) // Remove zRotation when convenient
    {

       
        foreach (GameObject b in currentBullets)
        {
            if (!b)
            {
                currentBullets.Remove(b);
            }
        }
        if (currentCooldown >= cooldown)
        {
            if (currentBullets.Count < 1 || hasMulti)
            {
                if (currentBullets.Count < 2)
                {
                    gameObject.GetComponent<AudioSource>().PlayOneShot(gameObject.GetComponent<AudioSource>().clip);
                    gameObject.GetComponent<ParticleSystem>().Play();
                    // Reset fire cooldown
                    currentCooldown = 0;
                    // Instantiate new bullet
                    GameObject newBullet = Instantiate(placeholder, placeholder.transform.position, placeholder.transform.rotation, null);

                    newBullet.GetComponent<Bullet>().SetOwner(plr);

                    newBullet.SetActive(true);
                    newBullet.GetComponent<Bullet>().SetBulletState(bulletType);
                    newBullet.GetComponent<Bullet>().moveDir = placeholder.transform.up;
                    // Set bullet parent to this GameObject
                    newBullet.GetComponent<Bullet>().SetParent(gameObject);
                    // Add bullet to active bullet list
                    currentBullets.Add(newBullet);
                }
            }
        }
    }

}
