using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class collectcoin : MonoBehaviour
{
    // Start is called before the first frame update
    int count = 0;
    public TextMeshProUGUI coinText;

    void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.CompareTag("coin"))
        {


            Destroy(other.gameObject);
            count++;
            Debug.Log("coins count " + count);
            coinText.text = "Coins: " + count.ToString();

        }


    }
   
}