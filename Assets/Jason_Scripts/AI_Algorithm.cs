#define Testing

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AI_Algorithm : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] AIStates aiStates;

    [SerializeField] Vector3 targetPos;

    bool bPlayerFound;
    /// <summary>
    /// This will handle how the AI behaves
    /// </summary>
    [SerializeField] enum AIStates
    {
        Attack,
        Search,
        Wander,
    };

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        aiStates = AIStates.Wander;


    }

    IEnumerator StartMovement()
    {
        yield return null;
    }


    // Update is called once per frame
    void Update()
    {
        /// <summary>
        /// This block of code handles the player states, if the player is seen from within the AI camera it will change to attack mode
        /// otherwise if the player is not within the sight of the camera the AI will be record it's position to go to later
        if (player.GetComponent<Renderer>().IsVisibleFrom(this.GetComponent<Camera>()))
        {
            Debug.Log("Found Player at: " + player.transform.position);
            aiStates = AIStates.Attack;
        }
        else if (!player.GetComponent<Renderer>().IsVisibleFrom(this.GetComponent<Camera>()))
        {
            if (aiStates == AIStates.Attack && !bPlayerFound)
            {
                aiStates = AIStates.Search;
                bPlayerFound = true;
                LastPlayerPosition(player.transform.position);
            }
        }
        ///</ summary >
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    void LastPlayerPosition(Vector3 position)
    {
        targetPos = position;
    }

    void CalculateNode()
    {
        
    }
}
