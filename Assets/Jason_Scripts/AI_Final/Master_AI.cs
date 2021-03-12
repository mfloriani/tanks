using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master_AI : MonoBehaviour
{

    int randomIndex = 0;
    int newRandomIndex = 0;

    [SerializeField] List<GameObject> nodes = new List<GameObject>();

    Vector2 targetNodePos;
    Vector2 nextNode;

    [SerializeField] GameObject currentNode;
    [SerializeField] GameObject target;
    [SerializeField] GameObject newNode;

    [SerializeField] float timer = 0;
    [SerializeField] float timeTaken = 3.0f;

    [SerializeField] Vector2 AIPos;
    [SerializeField] GameObject playerPos;

    enum AIStates
    {
        Attack,
        Search,
        Wander,
    };

    [SerializeField] AIStates aiStates;


    // Start is called before the first frame update
    void Start()
    {
        GameObject[] _nodeGameObject = GameObject.FindGameObjectsWithTag("Node");

        for(int i = 0; i < _nodeGameObject.Length; i++)
        {
            nodes.Add(_nodeGameObject[i]);
        }

        currentNode = nodes[0];
        CalculateNextNode();

        for(int i = 0; i < nodes.Count; i++)
        {
            if (Vector2.Distance(nodes[i].transform.position, this.transform.position) < Vector2.Distance(nextNode, this.transform.position))
            {
                nextNode = nodes[i].transform.position;
                timer = 0;
                AIPos = this.transform.position;
            }
        }
        targetNodePos = nodes[randomIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        target.transform.position = targetNodePos;

        MoveAISmooth();
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Node"))
        {
            this.transform.position = other.transform.position;

            currentNode = other.gameObject;

            if (other.transform.position.x == targetNodePos.x && other.transform.position.y == targetNodePos.y && aiStates == AIStates.Wander)
            {
                while (newRandomIndex == randomIndex)
                {
                    randomIndex = Random.Range(0, nodes.Count);
                }
                newRandomIndex = randomIndex;
                /*if (!bHasFired)
                {
                    turret.Fire(tankHead.transform.rotation.eulerAngles.z);
                }*/
                targetNodePos = nodes[randomIndex].transform.position;
            }
            else if (other.transform.position.x == targetNodePos.x && other.transform.position.y == targetNodePos.y && aiStates == AIStates.Search)
            {
                //StartCoroutine(Rotate360());
            }
            else if (other.transform.position.x == targetNodePos.x && other.transform.position.y == targetNodePos.y && aiStates == AIStates.Attack)
            {
                //bCanMove = false;
            }
            CalculateNextNode();
        }
    }



    void MoveAISmooth()
    {
        //if (bCanMove)
        //{
            timer += Time.deltaTime;

            if (timer > timeTaken)
            {
                CalculateNextNode();
            }
            else
            {
                Vector3 headRot;

                float speed = timer / timeTaken;

                float rotSpeed = 5.0f;

                transform.position = Vector2.Lerp(AIPos, nextNode, speed);

                /*
                if (aiStates == AIStates.Attack)
                {
                    headRot = tankHead.transform.rotation.eulerAngles;
                }
                else
                {
                    headRot = transform.rotation.eulerAngles;
                }
                */

                Vector3 vectorToTarget = new Vector3(nextNode.x - transform.position.x, nextNode.y - transform.position.y, 0.0f);
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90.0f;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotSpeed);
           // }
        }
    }

    void CalculateNextNode()
    {
        for (int i = 0; i < currentNode.GetComponent<CurrentNode>().accessibleNodes2D.Count; i++)
        {
            if (Vector2.Distance(currentNode.GetComponent<CurrentNode>().accessibleNodes2D[i].transform.position, targetNodePos) < Vector2.Distance(nextNode, targetNodePos))
            {
                nextNode = currentNode.GetComponent<CurrentNode>().accessibleNodes2D[i].transform.position;
                newNode.transform.position = nextNode;
                timer = 0;
                AIPos = transform.position;
            }
        }
    }
}
