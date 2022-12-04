using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGroup : MonoBehaviour
{
    private List<Button> Buttons;
    private Color hoverColor = new Color(0.2509f,0.4509f,0.619f);

    private Color defaultColor = new Color(0.8823f,0.6941f,0.1725f);
    public GameObject startScene;
    public GameObject creditsScene;
    public GameObject optionsScene;
    
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
}
