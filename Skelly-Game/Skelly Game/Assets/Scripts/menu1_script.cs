using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu1_script : MonoBehaviour
{
    int signal_iterator = 1;
    public int delay = 1;
    public string serverIP = "localhost";
    public int serverPort = 8080;
    public GameObject[] borders;
    
    public void menu1_back()
    {
        SceneManager.LoadScene(1);
    }
    public void LEVELS_MENU_back()
    {
        SceneManager.LoadScene(4);
    }
    public void maze1()
    {
        SceneManager.LoadScene(7);
        Time.timeScale = 1.0f;        
    }
    public void maze2()
    {
        SceneManager.LoadScene(8);
        Time.timeScale = 1.0f;
    }
    public void maze3()
    {
        SceneManager.LoadScene(9);
        Time.timeScale = 1.0f;
    }
    public void maze4()
    {
        SceneManager.LoadScene(10);

    }

    public static void DelayUsingWhileLoop(int milliseconds)
    {
        // Get the start time
        Stopwatch stopwatch = Stopwatch.StartNew();
        Time.timeScale = 0.0f;
        // Loop until the specified time has elapsed
        while (stopwatch.ElapsedMilliseconds < milliseconds)
        {
            // Optionally, you could do some light work or check for an abort condition here
        }
        Time.timeScale = 1.0f;
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


