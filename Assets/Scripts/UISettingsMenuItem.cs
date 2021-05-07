using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsMenuItem : MonoBehaviour
{
    [HideInInspector]
    public Image img;
    public Sprite offImg;
    public Sprite onImg;
    bool isOn;
    public RectTransform trans;

    void Awake()
    {
        img = GetComponent<Image>();
        trans = (RectTransform)transform;
    }
    public void Click(){
        if(isOn){
            img.sprite = offImg;
            isOn = false;
        }
        else{
            img.sprite = onImg;
            isOn = true;
        }
    }
    
    
}
