using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumb : MonoBehaviour
{
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.Space))
        {
            if(rb.velocity.y== 0)
            {
                rb.velocity= new Vector3(rb.velocity.x,10,rb.velocity.z);
            }
        }
    }
}
