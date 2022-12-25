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
    NetworkStream stream;
    string message;
    byte[] data = new byte[256];

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
        stream = client.GetStream();
        string response = "Hello";
        byte[] responseData = Encoding.UTF8.GetBytes(response);
        stream.Write(responseData, 0, responseData.Length);
        byte[] data = new byte[256];
        int bytes = stream.Read(data, 0, data.Length);
        message = Encoding.UTF8.GetString(data, 0, bytes);
        Debug.Log(message);

    }

    public bool signUp(string username, string password)
    {
        string request = "Signup " + username + " " + password;
        byte[] requestData = Encoding.UTF8.GetBytes(request);
        stream.Write(requestData, 0, requestData.Length);
        int bytes = stream.Read(data, 0, data.Length);
        message = Encoding.UTF8.GetString(data, 0, bytes);
        if (message == "Taken")
        {
            return false;
        }
        else if (message == "Done")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool login(string username, string password)
    {
        string request = "Login " + username + " " + password;
        byte[] requestData = Encoding.UTF8.GetBytes(request);
        stream.Write(requestData, 0, requestData.Length);
        int bytes = stream.Read(data, 0, data.Length);
        message = Encoding.UTF8.GetString(data, 0, bytes);
        if (message == "Done")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string createGame()
    {
        string request = "Create";
        byte[] requestData = Encoding.UTF8.GetBytes(request);
        stream.Write(requestData, 0, requestData.Length);
        int bytes = stream.Read(data, 0, data.Length);
        message = Encoding.UTF8.GetString(data, 0, bytes);
        return message;
    }

    public string joinGame(string gameId)
    {
        string request = "Join " + gameId;
        byte[] requestData = Encoding.UTF8.GetBytes(request);
        stream.Write(requestData, 0, requestData.Length);
        int bytes = stream.Read(data, 0, data.Length);
        message = Encoding.UTF8.GetString(data, 0, bytes);
        return message;
    }

    public string askLobbyStatus()
    {
        string request = "Lobby";
        byte[] requestData = Encoding.UTF8.GetBytes(request);
        stream.Write(requestData, 0, requestData.Length);
        int bytes = stream.Read(data, 0, data.Length);
        message = Encoding.UTF8.GetString(data, 0, bytes);
        return message;
    }


    void Update()
    {


    }
}
