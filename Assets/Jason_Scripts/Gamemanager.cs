using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    List<GameObject> AITanks = new List<GameObject>();

    private int score;

    Text scoreText;

    public List<GameObject> playerSpawn = new List<GameObject>();
    public List<GameObject> playerCount = new List<GameObject>();

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            scoreText.text = "Score: " + score.ToString();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetNumberOfTanks();

        for(int i = 0; i < playerSpawn.Count; i++)
        {
            playerCount[i].transform.position = playerSpawn[i].transform.position;
            playerCount[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(AITanks.Count <= 0)
        {
            GameOver();
        }
    }

    void GetNumberOfTanks()
    {
        GameObject[] numOfTanks = GameObject.FindGameObjectsWithTag("Enemy");

        for(int i = 0; i < numOfTanks.Length; i++)
        {
            AITanks.Add(numOfTanks[i]);
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
