using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private GameData gameData;
    private Networking networking;
    private Dictionary<int, GameObject> players;
    private int myId;

    private string request;
    private string[] positions;

    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        networking = FindObjectOfType<Networking>();
        players = new Dictionary<int, GameObject>();
        getPlayers();

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
        }
        myId = int.Parse(playerlist[4]);
    }

    // fixed update is called once per frame
    void FixedUpdate()
    {
        //detect w a s d and move player
        if (Input.GetKey(KeyCode.W))
        {
            players[myId].transform.position += new Vector3(0, 0, 0.1f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            players[myId].transform.position += new Vector3(-0.1f, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            players[myId].transform.position += new Vector3(0, 0, -0.1f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            players[myId].transform.position += new Vector3(0.1f, 0, 0);
        }
        request = players[myId].transform.position.x + " " + players[myId].transform.position.y + " " + players[myId].transform.position.z;
        positions = networking.updateMyPos(request).Split(' ');
        for (int i = 0; i < positions.Length; i++)
        {
            string[] player = positions[i].Split('-');
            players[int.Parse(player[0])].transform.position = new Vector3(float.Parse(player[1]), float.Parse(player[2]), float.Parse(player[3]));
        }
    }
}
