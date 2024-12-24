using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;


public class menu1_test : MonoBehaviour
{
    // Start is called before the first frame update
   
    int signal_iterator = 1;
    public int delay = 1;
    public string serverIP = "localhost";
    public int serverPort = 8080;
    public GameObject[] borders;
    int selector = 0;
    private bool isCoroutineRunning = false;


    private void Start()
    {
        selector = 0;
        //borders = GameObject.FindGameObjectsWithTag("border-tag");
        System.IO.File.WriteAllText("Assets/Scripts/back-end/signals.txt", "0");
        UnityEngine.Debug.Log("borders count " + borders.Length);
    }

    IEnumerator DelayedAction(float delay)
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(delay);
        
        // Your delayed code here
        UnityEngine.Debug.Log("Action executed after delay");

        isCoroutineRunning = false;
    }
    private void Update()
    {
        if (!isCoroutineRunning)
        {
            StartCoroutine(DelayedAction(3.0f)); // Delay of 2 seconds
        }  
        string action = SendMessageToServer("predict");
        if (action == "left") // left
        {
            SceneManager.LoadScene(1);
            Time.timeScale = 1.0f;
        }
        if (action == "right") // right 
        {
            UnityEngine.Debug.Log(" on decide "+selector);
            SceneManager.LoadScene(selector+5);
            Time.timeScale = 1.0f;
        }
        
        if (action == "down") // Down 
        {
            UnityEngine.Debug.Log("Down : " + selector);
            borders[selector].SetActive(false);
            selector++;
            borders[selector].SetActive(true);

        }
        if (action == "up") // Up
        {
           
            borders[selector].SetActive(false);
            selector--;
            borders[selector].SetActive(true);
        }
        System.IO.File.WriteAllText("Assets/Scripts/back-end/signals.txt", signal_iterator.ToString());
        ++signal_iterator;

        if (!isCoroutineRunning)
        {
            StartCoroutine(DelayedAction(1.0f)); // Delay of 2 seconds
        }  
    }

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
}
