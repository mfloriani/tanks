using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to anything that will require firing mechanics
public class Firing : MonoBehaviour
{

    private float testCooldown = 1.5f;
    private float currentTestCooldown = 0;

    // The bullet
    public GameObject bulletObject;
    public List<GameObject> currentBullets;
    private float spawnOffset = 1; // testing value - not final

    // Start is called before the first frame update
    void Start()
    {
        currentBullets = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTestCooldown >= testCooldown)
        {
            Fire();
            currentTestCooldown = 0;
        }
        currentTestCooldown += Time.deltaTime;
    }

    // Call this function when you want the object to fire a bullet.
    public void Fire()
    {
        GameObject newBullet = Instantiate(bulletObject, transform);
        newBullet.transform.Translate(newBullet.transform.right * spawnOffset);
        newBullet.GetComponent<Bullet>().moveDir = newBullet.transform.right;
    }
}
