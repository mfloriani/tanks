using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentNode : MonoBehaviour
{
    [SerializeField] public List<GameObject> accessibleNodes = new List<GameObject>();
    [SerializeField] public List<GameObject> accessibleNodes2D = new List<GameObject>();
    [SerializeField] float distance = 2.5f;

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
        /*
        accessibleNodes.Clear();

        hit = Physics2D.Raycast(transform.position, Vector2.up);

        if(hit.collider.tag == "Node")
        {
            accessibleNodes.Add(hit.collider.gameObject);
        }

        hit = Physics2D.Raycast(transform.position, Vector2.down);

        if (hit.collider.tag == "Node")
        {
            accessibleNodes.Add(hit.collider.gameObject);
        }

        hit = Physics2D.Raycast(transform.position, Vector2.left);

        if (hit.collider.tag == "Node")
        {
            accessibleNodes.Add(hit.collider.gameObject);
        }

        hit = Physics2D.Raycast(transform.position, Vector2.right);

        if (hit.collider.tag == "Node")
        {
            accessibleNodes.Add(hit.collider.gameObject);
        }
        */

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
        accessibleNodes2D.Clear();

        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.left), distance);
        
        AddToList2D();

        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.right), distance);

        AddToList2D();

        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.up), distance);

        AddToList2D();

        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.down), distance);

        AddToList2D();

        for(int i = 0; i < accessibleNodes2D.Count; i++)
        {
            if(accessibleNodes2D[i] == this.gameObject)
            {
                accessibleNodes2D.RemoveAt(i);
                i = -1;
            }
        }
    }

    void AddToList2D()
    {
        for (int i = 0; i < ray2D.Length; i++)
        {
            accessibleNodes2D.Add(ray2D[i].collider.gameObject);
        }
    }
}