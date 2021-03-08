using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to anything that will require firing mechanics
public class Firing : MonoBehaviour
{
    // Firerate. Set this in the inspector as required.
    public float cooldown = 1.5f;
    float currentCooldown;

    // The bullet - set this to the bullet prefab in the inspector
    public GameObject bulletObject;

    public List<GameObject> currentBullets;
    private float spawnOffset = 1; // testing value - not final

    // Start is called before the first frame update
    void Start()
    {
        // Set firing cooldown so firing can begin immediately
        currentBullets = new List<GameObject>();
        currentCooldown = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        currentCooldown += Time.deltaTime;
    }

    // Call this function when you want the object to fire a bullet.
    public void Fire()
    {
        if (currentCooldown >= cooldown)
        {
            // Reset fire cooldown
            currentCooldown = 0;
            // Instantiate new bullet
            GameObject newBullet = Instantiate(bulletObject);
            newBullet.transform.position = transform.position;
            newBullet.transform.rotation = transform.rotation;
            // Offset bullet from tank
            newBullet.transform.Translate(newBullet.transform.right * spawnOffset);
            // Set bullet move direction
            newBullet.GetComponent<Bullet>().moveDir = newBullet.transform.right;
            // Set bullet parent to this GameObject
            newBullet.GetComponent<Bullet>().SetParent(gameObject);
            // Add bullet to active bullet list
            currentBullets.Add(newBullet);
        }
    }
}
