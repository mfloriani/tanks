using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentNode : MonoBehaviour
{
    [SerializeField] public List<GameObject> accessibleNodes = new List<GameObject>();
    [SerializeField] public List<GameObject> accessibleNodes2D = new List<GameObject>();
    float distance = 2.5f;

    Ray ray;
    RaycastHit hit;

    RaycastHit2D[] ray2D;

    // Use this for initialization
    void Start()
    {

        //Calls our node function
        CreateNodeList();

        CreateNodeList2D();
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 100, Color.white);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * 100, Color.white);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * 100, Color.white);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * 100, Color.white);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 100, Color.white);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 100, Color.white);
    }

    /// <summary>
    /// Shoots a raycast out of every side of our block to allow the nodes to add themselves to a list.
    /// </summary>
    public void CreateNodeList()
    {
        accessibleNodes.Clear();

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            if (hit.collider.tag == "Node")
            {
                accessibleNodes.Add(hit.collider.gameObject);
            }
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hit))
        {
            if (hit.collider.tag == "Node")
            {
                accessibleNodes.Add(hit.collider.gameObject);
            }
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit))
        {
            if (hit.collider.tag == "Node")
            {
                accessibleNodes.Add(hit.collider.gameObject);
            }
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit))
        {
            if (hit.collider.tag == "Node")
            {
                accessibleNodes.Add(hit.collider.gameObject);
            }
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit))
        {
            if (hit.collider.tag == "Node")
            {
                accessibleNodes.Add(hit.collider.gameObject);
            }
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit))
        {
            if (hit.collider.tag == "Node")
            {
                accessibleNodes.Add(hit.collider.gameObject);
            }
        }
    }

    public void CreateNodeList2D()
    {
        distance = 2.5f;
        transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, 0.0f);
        accessibleNodes2D.Clear();

        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.left), distance);

        AddToList2D();

        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.right), distance);

        AddToList2D();

        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.up), distance);

        AddToList2D();

        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.down), distance);

        AddToList2D();

        CreateNodeListDiagonal2D();

        for (int i = 0; i < accessibleNodes2D.Count; i++)
        {
            if (accessibleNodes2D[i] == this.gameObject)
            {
                accessibleNodes2D.RemoveAt(i);
                i = -1;
            }
        }

        Debug.Log("List Made");
    }

    void CreateNodeListDiagonal2D()
    {
        transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + 45.0f);

        distance = 5.0f;
        
        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.left), distance);

        AddToList2D();

        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.right), distance);

        AddToList2D();

        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.up), distance);

        AddToList2D();

        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.down), distance);

        AddToList2D();

        //transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void AddToList2D()
    {
        for (int i = 0; i < ray2D.Length; i++)
        {
            accessibleNodes2D.Add(ray2D[i].collider.gameObject);
        }
    }
}