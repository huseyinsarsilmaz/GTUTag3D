using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyStatus : MonoBehaviour
{
    private GameData gameData;
    private Networking networking;

    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        networking = FindObjectOfType<Networking>();
    }

    public void changeMyStatus()
    {
        networking.changeStatus();
    }
}
