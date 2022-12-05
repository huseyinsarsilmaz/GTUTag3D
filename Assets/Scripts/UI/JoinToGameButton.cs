using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinToGameButton : MonoBehaviour
{

    public TMPro.TMP_InputField inputField;
    public TMPro.TextMeshProUGUI text;
    private Color defaultColor = new Color(0.8823f,0.6941f,0.1725f);
    private Color disabledColor = new Color(0.4431f,0.5019f,0.5764f,0.2196f);
    // Update is called once per frame
    void Update()
    {
        if(inputField.text != "")
        {
            text.color = defaultColor;
        }
        else{
            text.color = disabledColor;
        }
    }
}
