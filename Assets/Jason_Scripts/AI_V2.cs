using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_V2 : MonoBehaviour
{
    [SerializeField] int tankLayer;
    [SerializeField] int wallLayer;
    [SerializeField] int dWallLayer;
    [SerializeField] Firing turret;
    [SerializeField] bool bHasFired = false;

    int randomIndex = 0;
    int newRandomIndex = 0;

    [SerializeField] List<GameObject> nodes = new List<GameObject>();

    Vector2 targetNodePos;
    Vector2 nextNode;
    Vector2 attackNode;

    [SerializeField] GameObject currentNode;
    [SerializeField] GameObject tankHead;

    public GameObject rayHitObject;

    [SerializeField] float timer = 0;
    [SerializeField] float timeTaken = 3.0f;

    [SerializeField] Vector2 AIPos;
    [SerializeField] GameObject playerPos;

    /// <summary>
    /// This will handle how the AI behaves
    /// </summary>
    enum AIStates
    {
        Attack,
        Search,
        Wander,
    };

    [SerializeField] bool bCanMove = true;

    [SerializeField] AIStates aiStates;



    // Start is called before the first frame update
    void Start()
    {
        tankLayer = LayerMask.NameToLayer("PlayerTank");
        wallLayer = LayerMask.NameToLayer("Wall");
        dWallLayer = LayerMask.NameToLayer("DestructibleWall");
        playerPos = GameObject.FindWithTag("Player");

        turret = gameObject.transform.GetChild(0).GetComponent<Firing>();

        GameObject[] _nodeGameObject = GameObject.FindGameObjectsWithTag("Node");

        float minDistance = Mathf.Infinity;
        int closestNodeId = 0;
        for (int i = 0; i < _nodeGameObject.Length; i++)
        {
            nodes.Add(_nodeGameObject[i]);

            float dist = Vector3.Distance(_nodeGameObject[i].transform.position, transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestNodeId = i;
            }
        }
        Debug.Log("Closest node: " + closestNodeId + " - "+ nodes[closestNodeId].name + " - " + transform.position);
        currentNode = nodes[closestNodeId];

        //currentNode = nodes[0];

        //randomIndex = Random.Range(0, nodes.Count);
        //targetNodePos = nodes[randomIndex].transform.position;

        var accessibleNodes = nodes[closestNodeId].GetComponent<CurrentNode>().accessibleNodes2D;
        randomIndex = Random.Range(0, accessibleNodes.Count);

        targetNodePos = accessibleNodes[randomIndex].transform.position;

        Debug.Log(transform.name + " -> targetNodePos: " + targetNodePos + " - closestId: "+ closestNodeId + " - randomIndex: "+ randomIndex);


        CalculateNextNode();

        for (int i = 0; i < nodes.Count; i++)
        {
            if (Vector2.Distance(nodes[i].transform.position, this.transform.position) < Vector2.Distance(nextNode, this.transform.position))
            {
                nextNode = nodes[i].transform.position;
                timer = 0;
                AIPos = this.transform.position;
            }
        }


        

    }

    // Update is called once per frame
    void Update()
    {
        //target.transform.position = targetNodePos;

        MoveAISmooth();


        if (rayHitObject != null)
        {
            if (rayHitObject.layer == tankLayer)
            {
                playerPos = rayHitObject;
                aiStates = AIStates.Attack;
            }
            else if (rayHitObject.layer == wallLayer || rayHitObject.layer == dWallLayer)
            {
                if (aiStates == AIStates.Attack)
                {
                    aiStates = AIStates.Search;
                    bCanMove = true;
                }
            }
        }

        if (turret.currentBullets.Count == 0)
        {
            bHasFired = false;
        }

        if (aiStates == AIStates.Attack)
        {
            targetNodePos = LastPlayerPosition(playerPos.transform.position);
            Vector2 attackNode = new Vector2(500, 500);

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].transform.position.x == targetNodePos.x && nodes[i].transform.position.y == targetNodePos.y)
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

            if (targetNodePos.x != transform.position.x || targetNodePos.y != transform.position.y)
            {
                bCanMove = true;
                CalculateNextNode();
            }

            float rotSpeed = 50.0f;

            Vector3 vectorToTarget = new Vector3(playerPos.transform.position.x - transform.position.x, playerPos.transform.position.y - transform.position.y, 0.0f);
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90.0f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            tankHead.transform.rotation = Quaternion.Slerp(tankHead.transform.rotation, q, Time.deltaTime * rotSpeed);


            if (!bHasFired)
            {
                turret.Fire(transform.rotation.eulerAngles.z, 4);
                bHasFired = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Node"))
        {
            currentNode = other.gameObject;

            if (other.transform.position.x == targetNodePos.x && other.transform.position.y == targetNodePos.y && aiStates == AIStates.Wander)
            {
                while (newRandomIndex == randomIndex)
                {
                    randomIndex = Random.Range(0, nodes.Count);
                }
                newRandomIndex = randomIndex;
                if (!bHasFired)
                {
                    turret.Fire(tankHead.transform.rotation.eulerAngles.z, 4);
                    bHasFired = true;
                }
                targetNodePos = nodes[randomIndex].transform.position;
            }
            else if (other.transform.position.x == targetNodePos.x && other.transform.position.y == targetNodePos.y && aiStates == AIStates.Search)
            {
                StartCoroutine(Rotate360());
            }
            else if (other.transform.position.x == targetNodePos.x && other.transform.position.y == targetNodePos.y && aiStates == AIStates.Attack)
            {
                bCanMove = false;
            }
        }
    }

    void CalculateNextNode()
    {
        for (int i = 0; i < currentNode.GetComponent<CurrentNode>().accessibleNodes2D.Count; i++)
        {
            if (Vector2.Distance(currentNode.GetComponent<CurrentNode>().accessibleNodes2D[i].transform.position, targetNodePos) < Vector2.Distance(nextNode, targetNodePos))
            {
                nextNode = currentNode.GetComponent<CurrentNode>().accessibleNodes2D[i].transform.position;
                timer = 0;
                AIPos = transform.position;
            }
        }
    }

    void MoveAISmooth()
    {
        if (bCanMove)
        {
            timer += Time.deltaTime;

            if (timer > timeTaken)
            {
                //Debug.Log("timer > timeTaken");
                CalculateNextNode();

                //bCanMove = false;
                //StartCoroutine(AIRot());
            }
            else
            {
                float speed = timer / timeTaken;

                float rotSpeed = 5.0f;

                transform.position = Vector2.Lerp(AIPos, nextNode, speed);


                Vector3 vectorToTarget = new Vector3(nextNode.x - transform.position.x, nextNode.y - transform.position.y, 0.0f);
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90.0f;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotSpeed);
            }
        }
    }

    Vector2 LastPlayerPosition(Vector2 position)
    {
        Vector2 playerNode = nodes[0].transform.position;

        for (int i = 0; i < nodes.Count; i++)
        {
            if (Vector2.Distance(nodes[i].transform.position, position) < Vector2.Distance(playerNode, position))
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

        while (t < 5 && aiStates != AIStates.Attack)
        {
            t += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, t / 5) % 360.0f;
            tankHead.transform.eulerAngles = new Vector3(tankHead.transform.eulerAngles.x, tankHead.transform.eulerAngles.y, zRotation);
            yield return null;
        }

        aiStates = AIStates.Wander;
        targetNodePos = nodes[randomIndex].transform.position;
        CalculateNextNode();
        yield return null;
    }

    IEnumerator AIRot()
    {
        CalculateNextNode();

        float rotSpeed = 5.0f;

        Vector3 vectorToTarget = new Vector3(nextNode.x - transform.position.x, nextNode.y - transform.position.y, 0.0f);
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90.0f;

        //Debug.Log(angle);

        float time = 0;

        while (time <= 3 && tankHead.transform.eulerAngles != this.transform.eulerAngles)
        {
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            tankHead.transform.rotation = Quaternion.Slerp(tankHead.transform.rotation, q, Time.deltaTime * rotSpeed);
            time += Time.deltaTime;
            //Debug.Log(time);
            yield return null;
        }
        //Debug.Log("Out");
        bCanMove = true;
        yield return null;
    }

    public void ResetValues()
    {
        randomIndex = 0;
        newRandomIndex = 0;
        bHasFired = false;
        timer = 0;
        timeTaken = 3.0f;

        targetNodePos = new Vector2(0, 0);
        nextNode = new Vector2(0, 0);
        attackNode = new Vector2(0, 0);

        currentNode = null;
        tankHead = null;

        rayHitObject = null;


        AIPos = new Vector2(0, 0);
        playerPos = null;
        bCanMove = true;

        Start();
    }
}