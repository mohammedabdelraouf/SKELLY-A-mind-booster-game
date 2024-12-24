using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatecoin : MonoBehaviour
{


    private float y_speed =2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 360 * y_speed * Time.deltaTime, 0);
    }
}
