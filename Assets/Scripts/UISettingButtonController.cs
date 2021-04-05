using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISettingButtonController : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public GameObject pauseMenuUI;
    public void OnPointerDown(PointerEventData eventData)
    {
        GUIUtility.hotControl = 1;
    }

    public void OnPointerUp(PointerEventData eventData)
    { 
        pauseMenuUI.SetActive(true);
        GUIUtility.hotControl = 0;
        this.gameObject.SetActive(false);
    }

    private void Update() {
        Debug.Log(GUIUtility.hotControl);
    }
}
