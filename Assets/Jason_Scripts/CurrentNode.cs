using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentNode : MonoBehaviour
{
    [SerializeField] public List<GameObject> accessibleNodes = new List<GameObject>();

    Ray ray;
    RaycastHit hit;

    // Use this for initialization
    void Start()
    {

        //Calls our node function
        CreateNodeList();
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
}