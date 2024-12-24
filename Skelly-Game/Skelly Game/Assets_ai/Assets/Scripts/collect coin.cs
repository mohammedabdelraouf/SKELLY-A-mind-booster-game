using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectcoin : MonoBehaviour
{
    // Start is called before the first frame update
    int count = 0;


    void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.CompareTag("coin"))
        {


            Destroy(other.gameObject);
            count++;
            Debug.Log("coins count " + count);

        }


    }
}
