using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGroup : MonoBehaviour
{
    private List<Button> Buttons;
    private Color hoverColor = new Color(0.2509f,0.4509f,0.619f);

    private Color defaultColor = new Color(0.8823f,0.6941f,0.1725f);
    private Color disabledColor = new Color(0.4431f,0.5019f,0.5764f,0.2196f);
    public GameObject startScene;
    public GameObject creditsScene;
    public GameObject optionsScene;
    public GameObject playModal;
    public GameObject loginModal;
    public GameObject blurry;
    private GameData gameData;
    private Networking networking;
    public GameObject mainMenu;
    public GameObject lobby;

    public void Start()
    {
        gameData = FindObjectOfType<GameData>();
        networking = FindObjectOfType<Networking>();

    }
    
    public void subscribe(Button button)
    {
        if (Buttons == null)
        {
            Buttons = new List<Button>();
        }
        Buttons.Add(button);
    }

    public void onEnter(Button button)
    {
        resetButtons();
        if(button.isText)
        {
            button.text.color = hoverColor;
        }
        else
        {
            button.background.color = hoverColor;
        }
    }

    public void onExit(Button button)
    {
        resetButtons();
    }
    public void resetButtons()
    {
        foreach (Button button in Buttons)
        {
            if(button.isText)
            {
                button.text.color = defaultColor;
            }
            else
            {
                button.background.color = defaultColor;
            }
        }
    }

    public void goToCredits () {
        startScene.SetActive(false);
        creditsScene.SetActive(true);
    }

    public void goToOptions() {
        startScene.SetActive(false);
        optionsScene.SetActive(true);
    }

    public void returnFromCredits(){
        startScene.SetActive(true);
        creditsScene.SetActive(false);
    }
    
    public void returnFromOptions() {
        startScene.SetActive(true);
        optionsScene.SetActive(false);
    }

    public void changeSoundEffectSetting(){
        if (gameData.isSound == true)
        {
            gameData.isSound = false;
            optionsScene.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);
        }
        else{
            gameData.isSound = true;
            optionsScene.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
        if (gameData.isSound == true) this.GetComponent<AudioSource>().Play();

    }

    public void changeMusicSetting(){
        if (gameData.isMusic == true)
        {
            gameData.isMusic = false;
            optionsScene.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(false);
        }
        else{
            gameData.isMusic = true;
            optionsScene.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(true);
        }
        if (gameData.isSound == true) this.GetComponent<AudioSource>().Play();

    }

    public void openPlayModal(){
        if (gameData.userName == "")
        {
            loginModal.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            loginModal.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TMP_InputField>().text = "";
            loginModal.transform.GetChild(1).GetChild(1).GetComponent<TMPro.TMP_InputField>().text = "";
            loginModal.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color = disabledColor;
            loginModal.transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color = disabledColor;
            blurry.SetActive(true);
            loginModal.SetActive(true);
        }
        else
        {
            playModal.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            playModal.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TMP_InputField>().text = "";
            playModal.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color = disabledColor;
            blurry.SetActive(true);
            playModal.SetActive(true);
        }


    }

    public void closePlayModal() {
        blurry.SetActive(false);
        playModal.SetActive(false);
    }

    public void closeLoginModal()
    {
        openPlayModal();
        loginModal.SetActive(false);
    }

    public void signUp()
    {
        string text1 = loginModal.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TMP_InputField>().text;
        string text2 = loginModal.transform.GetChild(1).GetChild(1).GetComponent<TMPro.TMP_InputField>().text;
        if (text1 != "" && text2 != "")
        {
            if (networking.signUp(text1, text2))
            {

                loginModal.transform.GetChild(0).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = " Sucessful";
            }
            else
            {
                loginModal.transform.GetChild(0).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = "Username already exists";
            }
            loginModal.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);

        }
    }

    public void login()
    {
        if (loginModal.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TMP_InputField>().text != "" && loginModal.transform.GetChild(1).GetChild(1).GetComponent<TMPro.TMP_InputField>().text != "")
        {
            if (networking.login(loginModal.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TMP_InputField>().text, loginModal.transform.GetChild(1).GetChild(1).GetComponent<TMPro.TMP_InputField>().text))
            {
                gameData.userName = loginModal.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TMP_InputField>().text;
                gameData.password = loginModal.transform.GetChild(1).GetChild(1).GetComponent<TMPro.TMP_InputField>().text;
                closeLoginModal();
            }
            else
            {
                loginModal.transform.GetChild(0).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = "Wrong username or password";
                loginModal.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            }

        }
    }

    public void createGame()
    {
        string[] response = networking.createGame().Split(' ');
        gameData.gameId = response[0];
        gameData.myTeam = response[1];
        gameData.creator = true;
        changeToLobby();
    }

    public void joinGame()
    {
        string gameId = playModal.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TMP_InputField>().text;
        string[] response = networking.joinGame(gameId).Split(' ');
        if (response[0] == "Failed")
        {
            playModal.transform.GetChild(0).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = "Game not found";
            playModal.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            gameData.gameId = response[0];
            gameData.myTeam = response[1];
            changeToLobby();
        }

    }

    public void changeToLobby()
    {
        lobby.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "GAME ID: " + gameData.gameId;
        lobby.transform.GetChild(1).GetChild(1).GetChild(int.Parse(gameData.myTeam) - 1).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = gameData.userName;
        lobby.transform.GetChild(1).GetChild(1).GetChild(int.Parse(gameData.myTeam) - 1).GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(true);
        lobby.transform.GetChild(1).GetChild(1).GetChild(int.Parse(gameData.myTeam) - 1).GetChild(1).GetChild(0).GetChild(1).gameObject.SetActive(true);
        lobby.transform.GetChild(1).GetChild(1).GetChild(int.Parse(gameData.myTeam) - 1).GetChild(1).GetChild(0).GetChild(2).gameObject.SetActive(true);
        mainMenu.SetActive(false);
        lobby.SetActive(true);
        gameData.gameState = 1;
    }


}
