using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class CircleShrink : MonoBehaviour
{
    //https://www.youtube.com/watch?v=qGdf1T4Ew8I
    private static CircleShrink instance;
    public Transform cicleTranform;
    private Transform topTranform;
    private Transform bottomTransform;
    private Transform leftTransform;
    private Transform rightTransform;
    private Vector3 cirlceSize;
    private Vector3 circlePos;
    private Vector3 targetSize;
    private Vector3 targetPos;
    private void Awake()
    {
        cicleTranform = transform.Find("circle");
        // topTranform = transform.Find("top");
        // bottomTransform = transform.Find("bottom");
        // leftTransform = transform.Find("left");
        // rightTransform = transform.Find("right");
        SetSize(new Vector3(0,0),new Vector3(20, 20, 0));
    }

    public void SetSize(Vector3 pos,Vector3 size)
    {
        circlePos = pos;
        cirlceSize = size;
        transform.position = pos;
        cicleTranform.localScale = size;
        topTranform.localScale = new Vector3(1000,1000);
        topTranform.localPosition = new Vector3(0,topTranform.localScale.y*.5f+size.y*.5f);
        
        bottomTransform.localScale = new Vector3(1000,1000);
        bottomTransform.localPosition = new Vector3(0,-topTranform.localScale.y*.5f+size.y*.5f);
        
        rightTransform.localScale = new Vector3(1000,size.y);
        rightTransform.localPosition = new Vector3(leftTransform.localScale.x*.5f+size.x*.5f,0);
        
        leftTransform.localScale = new Vector3(1000,size.y);
        leftTransform.localPosition = new Vector3(-leftTransform.localScale.x*.5f-size.x*.5f,0);
    }

    private bool IsOutsideCircle(Vector3 position)
    {
        return Vector3.Distance(position, cicleTranform.position)>cirlceSize.x*.5f;
    }

    public static bool IsOutsideCicle_Static(Vector3 pos)
    {
        return instance.IsOutsideCircle(pos);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 sizeChange = (targetSize - cirlceSize).normalized;
        Vector3 newSize = cirlceSize + sizeChange * (Time.deltaTime * 2);
        Vector3 cirleDir = (targetPos - circlePos).normalized;
        Vector3 newPos = circlePos + cirleDir * (Time.deltaTime * 2);
        SetSize(newPos,newSize);
    }
}
