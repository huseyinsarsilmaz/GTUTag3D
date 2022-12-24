using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


public class Networking : MonoBehaviour
{
    public string serverIP = "34.125.80.14";
    public int serverPort = 3389;
    public static Networking instance;
    IPAddress ipAddress;
    TcpClient client;
    string message;

    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        GameObject.DontDestroyOnLoad(this.gameObject);
        //connect to server
        ipAddress = IPAddress.Parse(serverIP);
        client = new TcpClient();
        client.Connect(ipAddress, serverPort);
        Debug.Log("Connected to server");
        NetworkStream stream = client.GetStream();
        string response = "Salam aleikum";
        byte[] responseData = Encoding.UTF8.GetBytes(response);
        stream.Write(responseData, 0, responseData.Length);
        byte[] data = new byte[256];
        int bytes = stream.Read(data, 0, data.Length);
        message = Encoding.UTF8.GetString(data, 0, bytes);
        Debug.Log(message);

    }

    void Update()
    {


    }
}
