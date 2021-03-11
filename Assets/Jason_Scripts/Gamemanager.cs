using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetList()
    {
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");

        for(int i = 0; i < nodes.Length; i++)
        {
            nodes[i].GetComponent<CurrentNode>().CreateNodeList2D();
        }
    }
}
