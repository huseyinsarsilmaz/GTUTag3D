using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ServerConnection : MonoBehaviour
{
    public string serverIP = "192.168.65.34";
    public int serverPort = 25001;
    IPAddress ipAddress;
    TcpClient client;
    string message;

    void Start()
    {
        //connect to server
        ipAddress = IPAddress.Parse(serverIP);
        client = new TcpClient();
        client.Connect(ipAddress, serverPort);
        Debug.Log("Connected to server");

    }

    void Update()
    {
        NetworkStream stream = client.GetStream();
        string response = "Salam aleikum";
        byte[] responseData = Encoding.UTF8.GetBytes(response);
        stream.Write(responseData, 0, responseData.Length);
        byte[] data = new byte[256];
        int bytes = stream.Read(data, 0, data.Length);
        message = Encoding.UTF8.GetString(data, 0, bytes);
        Debug.Log(message);

    }




}
