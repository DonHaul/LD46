using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLimiter : MonoBehaviour
{

    public Vector4 bounds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y>bounds.x)
        {
            transform.position = new Vector3(transform.position.x, bounds.x, transform.position.z);
        }else if(transform.position.y < bounds.z)
        {
            transform.position = new Vector3(transform.position.x, bounds.z, transform.position.z);
        }

        if (transform.position.x > bounds.y)
        {
            transform.position = new Vector3( bounds.y, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < bounds.w)
        {
            transform.position = new Vector3(bounds.w, transform.position.y, transform.position.z);
        }
    }
}
