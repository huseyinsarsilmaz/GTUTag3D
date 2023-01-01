using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class PlayerManager : MonoBehaviour
{
    private GameData gameData;
    private Networking networking;
    private Dictionary<int, GameObject> players;
    public GameObject camera;
    private int myId;
    private Dictionary<int, Vector3> playerPositions;
    private Dictionary<int, Quaternion> playerRotations;

    private string request;
    private string[] positions;

    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        networking = FindObjectOfType<Networking>();
        players = new Dictionary<int, GameObject>();
        playerPositions = new Dictionary<int, Vector3>();
        playerRotations = new Dictionary<int, Quaternion>();
        getPlayers();
        //create a thread for updating player positions
        Thread t = new Thread(new ThreadStart(updatePlayerPos));
        t.Start();

    }

    void getPlayers()
    {
        string response = networking.getPlayerData();
        string[] playerlist = response.Split(' ');
        //FIXME change to 12
        for (int i = 0; i < 2; i++)
        {
            Debug.Log(playerlist[i]);
            string[] player = playerlist[i].Split('-');
            players.Add(int.Parse(player[0]), this.transform.GetChild(i).gameObject);
            players[int.Parse(player[0])].transform.GetChild(9).GetComponent<TMPro.TextMeshPro>().text = player[1];
            players[int.Parse(player[0])].transform.position = new Vector3(float.Parse(player[2]), float.Parse(player[3]), float.Parse(player[4]));
            players[int.Parse(player[0])].transform.rotation = new Quaternion(float.Parse(player[5]), float.Parse(player[6]), float.Parse(player[7]), 1);
            playerPositions.Add(int.Parse(player[0]), new Vector3(float.Parse(player[2]), float.Parse(player[3]), float.Parse(player[4])));
            playerRotations.Add(int.Parse(player[0]), new Quaternion(float.Parse(player[5]), float.Parse(player[6]), float.Parse(player[7]), 1));
        }
        myId = int.Parse(playerlist[2]);
        camera.transform.position = new Vector3(playerPositions[myId].x, playerPositions[myId].y + 18, playerPositions[myId].z - 20);
    }

    // fixed update is called once per frame
    void FixedUpdate()
    {
        //if w or s is pressed move forward or backward
        if (Input.GetKey(KeyCode.W))
        {
            players[myId].transform.position += players[myId].transform.forward * 0.3f;
            //move camera forward
            camera.transform.position += players[myId].transform.forward * 0.3f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            players[myId].transform.position -= players[myId].transform.forward * 0.3f;
            //move camera backward
            camera.transform.position -= players[myId].transform.forward * 0.3f;
        }
        //if a or d is pressed rotate left or right
        if (Input.GetKey(KeyCode.A))
        {
            players[myId].transform.Rotate(0, -1, 0);
            //rotate camera around player
            camera.transform.RotateAround(playerPositions[myId], Vector3.up, -1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            players[myId].transform.Rotate(0, 1, 0);
            //rotate camera around player
            camera.transform.RotateAround(playerPositions[myId], Vector3.up, 1);
        }

        playerPositions[myId] = players[myId].transform.position;
        playerRotations[myId] = players[myId].transform.rotation;

        foreach (KeyValuePair<int, GameObject> player in players)
        {
            if (player.Key != myId)
            {
                player.Value.transform.position = playerPositions[player.Key];
                player.Value.transform.rotation = playerRotations[player.Key];
            }
        }
    }

    void updatePlayerPos()
    {
        while (true)
        {
            request = playerPositions[myId].x.ToString().Replace(",", ".") + " " + playerPositions[myId].y.ToString().Replace(",", ".") + " " + playerPositions[myId].z.ToString().Replace(",", ".") + " " + playerRotations[myId].x.ToString().Replace(",", ".") + " " + playerRotations[myId].y.ToString().Replace(",", ".") + " " + playerRotations[myId].z.ToString().Replace(",", ".");
            positions = networking.updateMyPos(request).Split(' ');
            for (int i = 0; i < positions.Length; i++)
            {
                string[] player = positions[i].Split('|');
                playerPositions[int.Parse(player[0])] = new Vector3(float.Parse(player[1]), float.Parse(player[2]), float.Parse(player[3]));
                playerRotations[int.Parse(player[0])] = new Quaternion(float.Parse(player[4]), float.Parse(player[5]), float.Parse(player[6]), 1);
            }
        }
    }
}



