using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class Eachwall : MonoBehaviour
{
  [SerializeField]  private GameObject cent;
    // Start is called before the first frame update
    [UsedImplicitly]
    public  Eachwall(GameObject c)
    {
        cent = c;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.Translate(Vector3.MoveTowards(other.transform.position,cent.transform.position,3*Time.deltaTime));
            Debug.Log("PlayerIn");
        }
    }

}
