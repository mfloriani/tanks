using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentNode : MonoBehaviour
{
    [SerializeField] public List<GameObject> accessibleNodes = new List<GameObject>();
    [SerializeField] public List<GameObject> accessibleNodes2D = new List<GameObject>();

    Ray ray;
    RaycastHit hit;

    RaycastHit2D[] ray2D;

    LayerMask nodeLayer;

    // Use this for initialization
    void Start()
    {
        nodeLayer = LayerMask.NameToLayer("Nodes");
        //Calls our node function
        //CreateNodeList();

        CreateNodeList2D();
    }

    private void Update()
    {

    }

    /// <summary>
    /// Shoots a raycast out of every side of our block to allow the nodes to add themselves to a list 3D.
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

    /// <summary>
    /// Shoots a raycast out of every side of our block to allow the nodes to add themselves to a list 2D.
    /// </summary>
    public void CreateNodeList2D()
    {
        transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, 0.0f);

        accessibleNodes2D.Clear();

        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.left), Mathf.Infinity, 1 << nodeLayer);

        AddToList2D();

        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.right), Mathf.Infinity, 1 << nodeLayer);

        AddToList2D();

        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.up), Mathf.Infinity, 1 << nodeLayer);

        AddToList2D();

        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.down), Mathf.Infinity, 1 << nodeLayer);

        AddToList2D();


        CreateNodeListDiagonal2D();

        Debug.Log("List Made");
    }

    void CreateNodeListDiagonal2D()
    {
        transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + 45.0f);
        
        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.left), Mathf.Infinity, 1 << nodeLayer);

        AddToList2D();

        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.right), Mathf.Infinity, 1 << nodeLayer);

        AddToList2D();

        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.up), Mathf.Infinity, 1 << nodeLayer);

        AddToList2D();

        ray2D = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector3.down), Mathf.Infinity, 1 << nodeLayer);

        AddToList2D();
    }

    void AddToList2D()
    {
        if (ray2D.Length > 1)
        {
            accessibleNodes2D.Add(ray2D[1].collider.gameObject);
        }
    }
}