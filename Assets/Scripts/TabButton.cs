using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public TabGroup tabGroup;
    public Image background;
    
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        tabGroup.onTabSelected(this);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.onTabEnter(this);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        tabGroup.onTabExit(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<Image>();
        tabGroup.subscribe(this);
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }


}
