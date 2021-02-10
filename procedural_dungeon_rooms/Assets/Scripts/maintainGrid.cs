using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maintainGrid : MonoBehaviour
{
    void Start()
    {
        InvokeRepeating("justUpdate",0.1f,0.2f);
    }
    void Update()
    {
    }
    void justUpdate()
    {
        transform.position= new Vector3(fw(transform.position.x),fw(transform.position.y),0);
    }
    
        float fw(float a, float b = 0.5f)
        {
            return Mathf.Floor(a/b)*b;
        }
}
