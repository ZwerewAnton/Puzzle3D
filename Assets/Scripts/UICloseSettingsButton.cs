using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICloseSettingsButton : MonoBehaviour
{
    public GameObject settingsButton;
    public GameObject settigsMenu;
    private void OnEnable() {
        GUIUtility.hotControl = 1;
    }

    public void CloseSetings(){
        settingsButton.SetActive(true);
        GUIUtility.hotControl = 0;
        settigsMenu.SetActive(false);
    }

}
