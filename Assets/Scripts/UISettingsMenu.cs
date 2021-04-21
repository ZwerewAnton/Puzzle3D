using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UISettingsMenu : MonoBehaviour
{
    [Header ("space between menu items")]
    [SerializeField] 
    private Vector2 _spacing;
    [SerializeField] 
    private Vector2 _offset;

    [Space]
    [Header ("Setting button rotation")]
    [SerializeField] float rotationDuration;
    [SerializeField] Ease rotationEase;

    [Space]
    [Header ("Animation")]
    [SerializeField] float expandDuration;
    [SerializeField] float collapseDuration;
    [SerializeField] Ease expandeEase;
    [SerializeField] Ease collapseEase;

    [Space]
    [Header ("Fading")]
    [SerializeField] float expandFadeDuration;
    [SerializeField] float collapseFadeDuration;

    public AudioSource gearSound;

    private Button settingButton;
    public UISettingsMenuItem[] menuItems;
    private bool isExpanded;

    private Vector2 settingButtonPosition;
    private int itemsCount;
    

    void Start()
    {
        itemsCount = transform.childCount - 1;/* 
        menuItems = new UISettingsMenuItem[itemsCount];
        for(int i = 0; i < itemsCount; i++){
            menuItems[i] = transform.GetChild(i + 1).GetComponent<UISettingsMenuItem>();
        } */
        settingButton = transform.GetChild(0).GetComponent<Button>();
        settingButton.transform.SetAsLastSibling();

        settingButtonPosition = settingButton.GetComponent<RectTransform>().localPosition;
        
        ResetPosition();
    }
    
    private void ResetPosition(){
        for(int i = 0; i < itemsCount; i++){
            menuItems[i].trans.localPosition =  settingButtonPosition;
        }
    }

    public void ToggleMenu()
    {
        gearSound.Play();
        isExpanded = !isExpanded;
        if(isExpanded){
            for(int i = 0; i < itemsCount; i++){
                //menuItems[i].trans.anchoredPosition = settingButtonPosition + _spacing * (i+1);
                //menuItems[i].trans.DOMove(settingButtonPosition + _spacing * (i+1), expandDuration).SetEase(expandeEase);
                Vector3 sss = settingButtonPosition + _spacing * (i+1);
                menuItems[i].trans.DOLocalMove(sss, expandDuration).SetEase(expandeEase);
                menuItems[i].img.DOFade(1f, expandFadeDuration).From(0f);
            }
        }
        else{
            for(int i = 0; i < itemsCount; i++){
                //menuItems[i].trans.anchoredPosition = settingButtonPosition;
                menuItems[i].trans.DOLocalMove(settingButtonPosition, collapseDuration).SetEase(collapseEase);   
                menuItems[i].img.DOFade(0f, collapseFadeDuration);
            }
        }
        settingButton.transform
            .DOLocalRotate(Vector3.forward * 180f, rotationDuration)
            .From(Vector3.zero)
            .SetEase(rotationEase);
        
        /* 
        for(int i = 0; i < itemsCount; i++){
            if(isExpanded){
                menuItems[i].trans.position = settingButtonPosition + _spacing * (i+1);
            }
            else{
                menuItems[i].trans.position = settingButtonPosition;
            }
        } */
    }

    public void Music(){
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
