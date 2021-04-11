using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISettingButtonController : MonoBehaviour,  IPointerUpHandler, IPointerDownHandler
{
    public GameObject pauseMenuUI;


        //dropDetailEvent.Invoke();
    
    

    public void OnPointerDown(PointerEventData eventData)
    {
        GUIUtility.hotControl = 1;
    }

    public void OnPointerUp(PointerEventData eventData)
    { 
        GUIUtility.hotControl = 0;
    }
        
    public void SettingsButton(){
        pauseMenuUI.SetActive(true);
        //GUIUtility.hotControl = 0;
        this.gameObject.SetActive(false);
        Debug.Log("open");
    }

}
