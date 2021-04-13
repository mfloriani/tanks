using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    [SerializeField] Vector2 spawnPoint;
    [SerializeField] GameObject tank;
    int numOfTanks = 10;

    void Start()
    {
        spawnPoint = tank.transform.position;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2.0f);
        tank.SetActive(true);
        tank.transform.position = spawnPoint;
        tank.GetComponent<AI_V2>().enabled = true;
    }

    public void AIRespawn()
    {
        tank.SetActive(false);
        tank.GetComponent<AI_V2>().enabled = false;
        StartCoroutine("Respawn");
    }
}
