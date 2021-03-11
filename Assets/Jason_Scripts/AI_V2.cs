using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_V2 : MonoBehaviour
{
    [SerializeField] int newLayer;


    RaycastHit2D ray2D;

    int randomIndex = 0;
    int newRandomIndex = 0;

    [SerializeField] List<GameObject> nodes = new List<GameObject>();

    Vector2 targetNodePos;
    Vector2 nextNode;
    Vector2 attackNode;

    [SerializeField] GameObject currentNode;
    [SerializeField] GameObject target;
    [SerializeField] GameObject newNode;
    [SerializeField] GameObject tankHead;

    [SerializeField] float timer = 0;
    [SerializeField] float timeTaken = 3.0f;
    
    [SerializeField] Vector2 AIPos;
    [SerializeField] GameObject playerPos;

    enum AIMovementMode
    {
        smooth,
        instant,
        Search,
    };

    /// <summary>
    /// This will handle how the AI behaves
    /// </summary>
    enum AIStates
    {
        Attack,
        Search,
        Wander,
    };

    [SerializeField] bool bCoroutineStart = false;
    [SerializeField] bool bPlayerFound = false;

    [SerializeField] AIStates aiStates;
    [SerializeField] AIMovementMode movement;



    // Start is called before the first frame update
    void Start()
    {
        newLayer = LayerMask.NameToLayer("PlayerTank");
        playerPos = GameObject.FindWithTag("Player");



        GameObject[] _nodeGameObject = GameObject.FindGameObjectsWithTag("Node");

        for (int i = 0; i < _nodeGameObject.Length; i++)
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

    IEnumerator MoveAI()
    {
        while(bCoroutineStart)
        {
            this.transform.position = nextNode;
            yield return new WaitForSeconds(1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        target.transform.position = targetNodePos;

        if(movement == AIMovementMode.instant)
        {
            StartMovement();
        }
        else if(movement == AIMovementMode.smooth)
        {
            bCoroutineStart = false;
            MoveAISmooth();
        }
        else if(movement == AIMovementMode.Search)
        {
            targetNodePos = LastPlayerPosition(playerPos.transform.position);
            bCoroutineStart = false;
            MoveAISmooth();
        }

        ray2D = Physics2D.Raycast(tankHead.transform.position, transform.TransformDirection(Vector3.down), Mathf.Infinity, 1 << newLayer);

        if(ray2D.collider != null && ray2D.collider.tag == "Player")
        {
            aiStates = AIStates.Attack;
        }
        else if(ray2D.collider == null && aiStates == AIStates.Attack || ray2D.collider != null && ray2D.collider.tag != "Player" && aiStates == AIStates.Attack)
        {
            Debug.Log("I am being activated " + aiStates);
        }

        if(aiStates == AIStates.Attack)
        {
            targetNodePos = LastPlayerPosition(playerPos.transform.position);
            Vector2 attackNode = new Vector2(500,500);

            for(int i = 0; i < nodes.Count; i++)
            {
                if(nodes[i].transform.position.x == targetNodePos.x && nodes[i].transform.position.y == targetNodePos.y)
                {
                    for (int j = 0; j < nodes[i].GetComponent<CurrentNode>().accessibleNodes2D.Count; j++)
                    {
                        if (Vector2.Distance(nodes[i].GetComponent<CurrentNode>().accessibleNodes2D[j].transform.position, targetNodePos) < Vector2.Distance(attackNode, targetNodePos))
                        {
                            attackNode = nodes[i].GetComponent<CurrentNode>().accessibleNodes2D[j].transform.position;
                        }
                    }
                }
            }
            targetNodePos = attackNode;


            float rotSpeed = 50.0f;

            Vector3 vectorToTarget = new Vector3(playerPos.transform.position.x - transform.position.x, playerPos.transform.position.y - transform.position.y, 0.0f);
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90.0f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            tankHead.transform.rotation = Quaternion.Slerp(tankHead.transform.rotation, q, Time.deltaTime * rotSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Node"))
        {
            this.transform.position = other.transform.position;

            currentNode = other.gameObject;

            if(other.transform.position.x == targetNodePos.x && other.transform.position.y == targetNodePos.y && aiStates != AIStates.Search)
            {
                while(newRandomIndex == randomIndex)
                {
                    randomIndex = Random.Range(0, nodes.Count);
                }
                newRandomIndex = randomIndex;

                targetNodePos = nodes[randomIndex].transform.position;
            }
            else if(other.transform.position.x == targetNodePos.x && other.transform.position.y == targetNodePos.y && aiStates == AIStates.Search)
            {
                StartCoroutine(Rotate360());
            }
            CalculateNextNode();
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

    void MoveAISmooth()
    {
        timer += Time.deltaTime;

        if(timer > timeTaken)
        {
            CalculateNextNode();
        }
        else
        {
            Vector3 headRot;

            float speed = timer / timeTaken;

            float rotSpeed = 5.0f;

            transform.position = Vector2.Lerp(AIPos, nextNode, speed);

            if (aiStates == AIStates.Attack)
            {
                headRot = tankHead.transform.rotation.eulerAngles;
            }
            else
            {
                headRot = transform.rotation.eulerAngles;
            }

            Vector3 vectorToTarget = new Vector3(nextNode.x - transform.position.x, nextNode.y - transform.position.y, 0.0f);
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90.0f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotSpeed);
        }
    }

    void StartMovement()
    {
        if (!bCoroutineStart)
        {
            bCoroutineStart = true;
            StartCoroutine(MoveAI());
        }
    }

    Vector2 LastPlayerPosition(Vector2 position)
    {
        Vector2 playerNode = nodes[0].transform.position;

        for(int i = 0; i < nodes.Count; i++)
        {
            if(Vector2.Distance(nodes[i].transform.position, position) < Vector2.Distance(playerNode, position))
            {
                playerNode = nodes[i].transform.position;
            }
        }
        return playerNode;
    }

    IEnumerator Rotate360()
    {
            float startRotation = transform.eulerAngles.z;
            float endRotation = startRotation + 360.0f;
            float t = 0.0f;

            while (t < 5)
            {
                t += Time.deltaTime;
                float zRotation = Mathf.Lerp(startRotation, endRotation, t / 5) % 360.0f;
                tankHead.transform.eulerAngles = new Vector3(tankHead.transform.eulerAngles.x, tankHead.transform.eulerAngles.y, zRotation);
                yield return null;
            }

        movement = AIMovementMode.smooth;
        targetNodePos = nodes[randomIndex].transform.position;
        target.transform.position = targetNodePos;
        CalculateNextNode();
        yield return null;
    }
}
