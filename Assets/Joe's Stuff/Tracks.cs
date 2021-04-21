using UnityEngine;
using UnityEngine.AI;

public class Tracks : MonoBehaviour
{
    public ParticleSystem system;

    
    Vector3 lastEmit;

    public float delta = 0.65f;
    public float gap = 0.5f;
    int dir = 1;
    static Tracks selectedSystem;

    void Start()
    {
        lastEmit = transform.position;
//        GetComponent<MeshRenderer>().material = activeMat;
    }

    public void Update()
    {


        if (Vector3.Distance(lastEmit, transform.position) > delta)
        {

            var pos = transform.position + (new Vector3(0f,0f,0f)* gap * dir);
            ParticleSystem.EmitParams ep = new ParticleSystem.EmitParams();
            ep.position = pos;
            ep.rotation = -transform.rotation.eulerAngles.z;
            system.Emit(ep, 1);
            lastEmit = transform.position;
        }

    }

}