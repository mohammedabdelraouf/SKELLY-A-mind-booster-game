using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Play_script : MonoBehaviour
{
    public float delay = 48.0f;
    int signal_iterator = 1;
    public string serverIP = "localhost";
    public int serverPort = 8080;

    void Start()
    {
        StartCoroutine(SwitchSceneAfterDelay());
        
    }
    IEnumerator SwitchSceneAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Load the second scene (scene index 1)
        SceneManager.LoadScene(4);
    }
    void Update()
    {
       
        DelayUsingWhileLoop(1000);
        string action = SendMessageToServer("predict");
        if (action == "left") // left
        {
            Exit_Game();
        }
        if (action == "right") // right 
        {
            SceneManager.LoadScene(2);
        }
        if (action == "down") // Down 
        {
            //sound-btn
        }
        if (action == "up") // Up
        {
            SceneManager.LoadScene(3);
        }
        System.IO.File.WriteAllText("Assets/Scripts/back-end/Main_menu/Signals.txt", signal_iterator.ToString());
        signal_iterator++;

        DelayUsingWhileLoop(1000);



    }
    // Start is called before the first frame update
    public void Play()
    {
        SceneManager.LoadScene(2); //SceneManager.GetActiveScene.buildIndex+1
    }
    public void How_To_Play()
    {
        SceneManager.LoadScene(3); //SceneManager.GetActiveScene.buildIndex+1
    }
   
    public void Exit_Game()
    {
        #if UNITY_STANDALONE
                 Application.Quit();
        #endif
        #if UNITY_EDITOR
                 UnityEditor.EditorApplication.isPlaying = false;
        #endif
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


   
    #endregion

}
