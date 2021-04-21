using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private GameObject[] walls;

    int count = 0;

    IEnumerator WallShrink()
    {
        foreach (var t in walls)
        {
            var Wall = t.transform.position;

            Vector3 dir = transform.position - Wall;
            dir = dir.normalized * (Time.deltaTime * 10);
            float dist = Vector3.Distance(Wall , transform.position);

            Wall += Vector3.ClampMagnitude(dir, dist);
            t.transform.position = Wall;
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(WallShrink());
        yield return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (var w in walls)
        {
            w.AddComponent<Rigidbody2D>();
            w.GetComponent<Rigidbody2D>().gravityScale = 0;
            w.GetComponent<Rigidbody2D>().isKinematic = false;
        }

        StartCoroutine(WallShrink());
    }

    
    // Update is called once per frame
    void Update()
    {
    }
}