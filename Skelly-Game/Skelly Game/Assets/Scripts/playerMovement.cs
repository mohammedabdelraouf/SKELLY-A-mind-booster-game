using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.IO;
using UnityEditor.Scripting.Python;
using System.Diagnostics;

public class playerMovement : MonoBehaviour
{
    // intialize needed variables 
    public float speed = 50f ;
    public float speed2 = 10f ;
    private Rigidbody Rb ;
    private List <int> previous = new List<int>();
    [SerializeField] GameObject winpanel;
    private GameObject[] checkPoints ;
    private List<List<GameObject>> branches = new List<List<GameObject>>();

    public string serverIP = "localhost";
    public int serverPort = 8080;
    int signal_iterator = 1 ;
    private float start_time = 0 ;
    // Start is called before the first frame update

    private bool isCoroutineRunning = false;
     IEnumerator DelayedAction(float delay)
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(delay);
        
        // Your delayed code here
        UnityEngine.Debug.Log("Action executed after delay");

        isCoroutineRunning = false;
    }
    void Start()
    {
        System.IO.File.WriteAllText("Assets/Scripts/back-end/signals.txt", "0"); 
        start_time = Time.time ;
       
        // store all path detectors and checkpoints in the map in its corresponding data structure
        Rb = GetComponent<Rigidbody>();
        checkPoints= GameObject.FindGameObjectsWithTag("checkpoint");
        

        for (int i = 1 ; i <= GameObject.FindGameObjectsWithTag("pathDetector").Length ; i++)
        {
            branches.Add(GameObject.FindGameObjectsWithTag("branch"+i).OrderBy(go => int.Parse(go.name)).ToList());
            previous.Add(-1); 
        }
       
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("checkpoint"))
        {
            Rb.velocity = new Vector3(0,0,0);
            if(other.gameObject.name == "End")
            {
               UnityEngine.Debug.Log("Winner::Winner");
                winpanel.SetActive(true);
                Time.timeScale = 0.0f;
            } 
            
        }
    }
    private void  OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("checkpoint"))
        {
           
            Rb.velocity = new Vector3(0,0,0);
            string movement = SendMessageToServer("predict");
           
            if(other.gameObject.name == "Start")
            {
                previous[0] = -1;
            } 
           if (!isCoroutineRunning)
            {
                StartCoroutine(DelayedAction(5.0f)); // Delay of 2 seconds
            }  
            if(movement == "left") // left
            {
                Rb.velocity= new Vector3(2*speed2,0,0);
                //UnityEngine.Debug.Log(Rb.velocity.ToString());
            }  
             if(movement == "right") // right 
            {
                Rb.velocity= new Vector3(-2*speed2,0,0);
                //UnityEngine.Debug.Log(Rb.velocity.ToString());
            }
            if(movement == "down") // Down 
            {
                Rb.velocity= new Vector3(0,0,2*speed2);
                //UnityEngine.Debug.Log(Rb.velocity.ToString());
            }
            if(movement == "up") // Up
            {
                Rb.velocity= new Vector3(0,0,-2*speed2);
                //UnityEngine.Debug.Log(Rb.velocity.ToString());
            }
            
           System.IO.File.WriteAllText("Assets/Scripts/back-end/signals.txt", signal_iterator.ToString());
           signal_iterator++; 
            
        }
        
        if (other.transform.parent.gameObject.CompareTag("pathDetector"))
        {
            int branch_number = int.Parse(other.gameObject.tag[6].ToString())-1;
            int index = int.Parse(other.gameObject.name)-1; 
            int LastIndex = branches[branch_number].Count-1;
            // UnityEngine.Debug.Log(previous[branch_number] + " // " + index + " ///"  + LastIndex );
            if((index == 0 && previous[branch_number] == 1 )||( index == LastIndex && previous[branch_number]== LastIndex-1))
            {     
                GameObject nearest = getNearestCheckPoint();
                previous[branch_number] = -1;
                Vector3  dir  = nearest.transform.position - transform.position;
                Rb.velocity = speed * Time.deltaTime * dir;
                //transform.position = Vector3.MoveTowards(transform.position,nearest.transform.position, speed );
            }
            else
            {
                if(previous[branch_number]< 0 && index== LastIndex)
                {
                    previous[branch_number] = index;
                    Vector3  dir  = branches[branch_number][index-1].transform.position - transform.position ;
                    Rb.velocity = speed * Time.deltaTime * dir;
                    //transform.Translate(dir * speed * Time.deltaTime);
                    //transform.position = Vector3.MoveTowards(transform.position,branches[branch_number][index-1].transform.position, speed * Time.deltaTime);
                }
                else 
                {
                    if(index <= LastIndex)
                    {
                       
                        if(previous[branch_number] < index)
                        {
                            
                            previous[branch_number] = index;
                            Vector3  dir  = branches[branch_number][index+1].transform.position - transform.position ;
                            //transform.Translate(dir * speed * Time.deltaTime);
                             Rb.velocity = speed * Time.deltaTime * dir;

                        }
                        else if (previous[branch_number] > index)
                        {
                            previous[branch_number] = index;
                            Vector3  dir  = branches[branch_number][index-1].transform.position - transform.position ;
                            //transform.Translate(dir * speed * Time.deltaTime);
                            Rb.velocity = speed * Time.deltaTime * dir;
                          
                        }
                        
                    }                
                }
            }
            
           
        }
        
    }

    #region  helper functions    
    private GameObject getNearestCheckPoint()
    {
        // Initialize variables to store the shortest distance and nearest object
        float shortestDistance = Mathf.Infinity;
        GameObject nearest = null;

        // Loop through each object with the specified tag
        foreach (GameObject obj in checkPoints)
        {
            // Calculate the distance between the current object and the object that this script is attached to
            float distance = Vector3.Distance(transform.position, obj.transform.position);

            // Check if the current object is closer than the previously found nearest object
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = obj;
            }
        }

        // Update reference to the nearest object
        return nearest;

    }
    private void Update()
    {
        

        if(Time.time - start_time  >= 120)
        {
            // do some thing to exit the level and go back to levels menu 
        }
    }

   
    #region  handel connection 
    
    string SendMessageToServer(string message)
    {
        try
        {
            using (TcpClient client = new TcpClient(serverIP, serverPort))
            {
                NetworkStream stream = client.GetStream();

                // Send data to the server
                byte[] dataToSend = Encoding.UTF8.GetBytes(message);
                stream.Write(dataToSend, 0, dataToSend.Length);
                UnityEngine.Debug.Log("Sent: " + message);

                // Buffer to store the response bytes
                byte[] responseBuffer = new byte[1024];
                int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
                //DelayUsingWhileLoop(2000);
                // Convert bytes to string
                string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
                UnityEngine.Debug.Log("Received: " + response);

                return response;
            }
        }
        catch (SocketException e)
        {
            UnityEngine.Debug.LogError("SocketException: " + e);
        }
        return null ;
    }

    public int readFromFile(string filePath)
    {
        int integerValue = -1 ;
        try
        {
            // Read all text from the file
            string fileContent = System.IO.File.ReadAllText(filePath);

            // Parse the text content to an integer
            integerValue = int.Parse(fileContent);

            // Output the integer value
            Console.WriteLine("The integer value is: " + integerValue);
        }
        catch (FormatException e)
        {
            Console.WriteLine("The file does not contain a valid integer: " + e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
        return integerValue;
    }


#endregion
    #endregion     
}


