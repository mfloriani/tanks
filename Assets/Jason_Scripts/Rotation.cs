using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    GameObject node;

    [SerializeField] int nodeNum = 0;

    Vector3 direction;
    GameObject[] nodes;

    // Start is called before the first frame update
    void Start()
    {
        nodes = GameObject.FindGameObjectsWithTag("Node");
    }

    // Update is called once per frame
    void Update()
    {
        direction = (nodes[nodeNum].transform.position - this.transform.position).normalized;

        Quaternion _lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 5.0f);

    }
}
