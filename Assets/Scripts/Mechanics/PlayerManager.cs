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
        for (int i = 0; i < 4; i++)
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
        request = players[myId].transform.position.x + " " + players[myId].transform.position.y + " " + players[myId].transform.position.z;
        networking.updateMyPos(request);
    }
}
