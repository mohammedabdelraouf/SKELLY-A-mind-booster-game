using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class winpanel : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        //if (transform.activeSelf) 
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey("enter"))
            {
                skip_panel();
            }
        }
        
    }
    public void skip_panel()
    {
        SceneManager.LoadScene(4);
    }
}
