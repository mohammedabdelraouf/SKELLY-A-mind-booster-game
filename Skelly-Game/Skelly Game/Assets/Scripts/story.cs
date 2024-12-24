using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class story : MonoBehaviour
{

    int signal_iterator = 1;
    public string serverIP = "localhost";
    public int serverPort = 8080;
    // Start is called before the first frame update
    void Start()
    {
        System.IO.File.WriteAllText("Assets/Scripts/back-end/signals.txt", "0");
    }

    private bool isCoroutineRunning = false;
     IEnumerator DelayedAction(float delay)
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(delay);
        
        // Your delayed code here
        UnityEngine.Debug.Log("Action executed after delay");

        isCoroutineRunning = false;
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKey(KeyCode.Return) || Input.GetKey("enter"))
        {
            skip_story();
        }

       
        //string action = SendMessageToServer("predict");
        /* if (!isCoroutineRunning)
        {
           // StartCoroutine(DelayedAction(40.0f)); // Delay of 2 seconds
        }   
        //if (action == "right") // right 
        {
           // skip_story();
        }
        System.IO.File.WriteAllText("Assets/Scripts/back-end/signals.txt", signal_iterator.ToString());
        signal_iterator++;

        if (!isCoroutineRunning)
        {
            //StartCoroutine(DelayedAction(1.0f)); // Delay of 2 seconds
        }  */
    }
    public void skip_story()
    {
        SceneManager.LoadScene(4);
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
        return null;
    }

    public int readFromFile(string filePath)
    {
        int integerValue = -1;
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
    public static void DelayUsingWhileLoop(int milliseconds)
    {
        // Get the start time
        Stopwatch stopwatch = Stopwatch.StartNew();
        // Loop until the specified time has elapsed
        while (stopwatch.ElapsedMilliseconds < milliseconds)
        {
            // Optionally, you could do some light work or check for an abort condition here
        }
        // Stop the stopwatch (optional, as it will stop when out of scope)
        stopwatch.Stop();
    }

    #endregion
}
