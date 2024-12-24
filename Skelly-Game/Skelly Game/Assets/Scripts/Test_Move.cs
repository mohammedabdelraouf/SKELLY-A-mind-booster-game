using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Move : MonoBehaviour
{
     public bool move = true ;
    public float speed = 10 ;

    Rigidbody Rb = new Rigidbody();
    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }

    private void  OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("checkpoint"))
        {
            // Perform actions specific to the collision with the object having the specified tag
            Debug.Log("Collision occurred with object tagged as 'Check point'"); 
            Rb.velocity = new Vector3(0,0,0);
           // die();
              
        }
        
    }


  
    private void OnCollisionExit(Collision other) {

        
    }
    // Update is called once per frame
    void Update()
    {
        if(move)
        {
            if(UnityEngine.Input.GetKey(KeyCode.UpArrow))
            {
               Rb.velocity= new Vector3(0,0,(-1*speed));
            }
            if(UnityEngine.Input.GetKey(KeyCode.DownArrow))
            {
             Rb.velocity= new Vector3(0,0,speed);
            }
            if(UnityEngine.Input.GetKey(KeyCode.LeftArrow))
            {
                Rb.velocity= new Vector3(speed,Rb.velocity.y,0);
            }
            if(UnityEngine.Input.GetKey(KeyCode.RightArrow))
            {
                Rb.velocity= new Vector3(-1*speed,Rb.velocity.y,0);
            }
        }
    }
}
