using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_AI : MonoBehaviour
{
    [SerializeField] int tankLayer;
    [SerializeField] int wallLayer;
    [SerializeField] int dWallLayer;
    [SerializeField] AI_V2 AI;

    // Start is called before the first frame update
    void Start()
    {
        tankLayer = LayerMask.NameToLayer("PlayerTank");
        wallLayer = LayerMask.NameToLayer("Wall");
        dWallLayer = LayerMask.NameToLayer("DestructibleWall");
        AI = transform.parent.gameObject.GetComponent<AI_V2>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D ray2D = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.down), Mathf.Infinity, 1 << tankLayer | 1 << wallLayer | 1 << dWallLayer);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 5, Color.red);

        if (ray2D.collider != null)
        {
            AI.rayHitObject = ray2D.collider.gameObject;
        }

    }
}