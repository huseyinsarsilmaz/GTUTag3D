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
    public GameObject blurry;
    
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
        if(GameData.isSound == true){
            GameData.isSound = false;
            optionsScene.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);
        }
        else{
            GameData.isSound = true;
            optionsScene.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
        if(GameData.isSound == true) this.GetComponent<AudioSource>().Play();

    }

    public void changeMusicSetting(){
        if(GameData.isMusic == true){
            GameData.isMusic = false;
            optionsScene.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(false);
        }
        else{
            GameData.isMusic = true;
            optionsScene.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(true);
        }
        if(GameData.isSound == true) this.GetComponent<AudioSource>().Play();

    }

    public void openPlayModal(){
        playModal.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TMP_InputField>().text = "";
        playModal.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color = disabledColor;
        blurry.SetActive(true);
        playModal.SetActive(true);
        
    }

    public void closePlayModal() {
        blurry.SetActive(false);
        playModal.SetActive(false);
    }
}
