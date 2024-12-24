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
using UnityEngine.SceneManagement;


public class chage_scene : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        
        string res = SendMessageToServer(SceneManager.GetActiveScene().name);
    }

    string SendMessageToServer(string message)
    {
        try
        {
            using (TcpClient client = new TcpClient("localhost", 8080))
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

}
