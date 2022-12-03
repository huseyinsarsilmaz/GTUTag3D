using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    
    public void subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }

    public void onTabEnter(TabButton button)
    {
        resetTabs();
        button.background.color = Color.yellow;
    }

    public void onTabExit(TabButton button)
    {
        resetTabs();
    }

    public void onTabSelected(TabButton button)
    {
        resetTabs();
        button.background.color = Color.red;
    }

    public void resetTabs()
    {
        foreach (TabButton button in tabButtons)
        {
            button.background.color = Color.white;
        }
    }

    
}
