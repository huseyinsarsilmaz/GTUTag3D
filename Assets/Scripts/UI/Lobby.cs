using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    private GameData gameData;
    private Networking networking;

    private List<List<GameObject>> cards;

    private Color redColor = new Color(0.9098f, 0.2549f, 0.0941f);
    private Color greenColor = new Color(0.2666f, 0.7411f, 0.1960f);

    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        networking = FindObjectOfType<Networking>();
        cards = new List<List<GameObject>>();
        for (int i = 0; i < 4; i++)
        {
            cards.Add(new List<GameObject>());
            for (int j = 0; j < 3; j++)
            {
                cards[i].Add(this.transform.GetChild(i).GetChild(1).GetChild(j).gameObject);
            }
        }
        //create a timer to check lobby status
        InvokeRepeating("askLobbyStatus", 0.0f, 0.64f);
        //CancelInvoke("askLobbyStatus");
    }

    // Update is called once per frame

    void askLobbyStatus()
    {
        if (gameData.creator && networking.canGameBegin())
        {
            //sleep for 1 second
            System.Threading.Thread.Sleep(1000);
            gameData.gameState = 2;
            networking.startGame();
            Debug.Log("Burdayim");
            SceneManager.LoadScene("Game");
            return;
        }

        if (gameData.gameState == 1)
        {
            string response = networking.askLobbyStatus();
            if (response == "Begin")
            {
                gameData.gameState = 2;
                SceneManager.LoadScene("Game");
                return;
            }
            string[] teams = response.Split(' ');
            foreach (string team in teams)
            {
                string[] teamData = team.Split('-');
                string dbg = "";
                foreach (string s in teamData)
                {

                    dbg += s;

                }

                if (teamData[1] != "Empty")
                {
                    int iter = (teamData.Length - 1) / 2;
                    for (int i = 0; i < iter; i++)
                    {
                        GameObject card = cards[int.Parse(teamData[0]) - 1][i];
                        if (teamData[2 * i + 2] == "ready")
                        {
                            gameData.myStatus = 1;
                            card.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = greenColor;
                            card.transform.GetChild(2).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = "Ready";
                        }
                        else
                        {
                            gameData.myStatus = 0;
                            card.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = redColor;
                            card.transform.GetChild(2).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = "Not Ready";
                        }
                        if (card.transform.GetChild(0).gameObject.activeSelf == false)
                        {
                            card.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = teamData[2 * i + 1];
                            card.transform.GetChild(0).gameObject.SetActive(true);
                            card.transform.GetChild(1).gameObject.SetActive(true);
                            card.transform.GetChild(2).gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}