using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager gameManager;

    [SerializeField] GameObject AISpawn;
    [SerializeField] GameObject AI;
    [SerializeField] GameObject newAI;

    private void Awake()
    {
        /*
        if(gameManager == this)
        {

        }
        else if(gameManager != this)
        {
            Destroy(gameObject);
        }
        */
    }


    List<GameObject> AITanks = new List<GameObject>();

    private int score;

    int numSpawned;

    Text scoreText;

    public List<GameObject> playerSpawn = new List<GameObject>();
    public List<GameObject> playerCount = new List<GameObject>();

    /*
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
    */

    // Start is called before the first frame update
    void Start()
    {
        AISpawn = GameObject.FindWithTag("AI_Spawn");

        StartCoroutine(SpawnAI());

        /*

        GetNumberOfTanks();

        for(int i = 0; i < playerSpawn.Count; i++)
        {
            playerCount[i].transform.position = playerSpawn[i].transform.position;
            playerCount[i].SetActive(true);
        }
        */
    }

    IEnumerator SpawnAI()
    {
        while(numSpawned <= 3)
        {
            newAI = Instantiate(AI);
            newAI.transform.position = AISpawn.transform.position;
            newAI.SetActive(true);
            newAI = null;
            numSpawned++;
            yield return new WaitForSeconds(2.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(AITanks.Count <= 0)
        {
            //GameOver();
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
        //SceneManager.LoadScene("Main Menu");
    }
}
