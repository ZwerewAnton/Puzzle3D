using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISpawnListItems : MonoBehaviour
{
    public GameObject ContentPanel;
    public GameObject ListItemPrefab;
    private List<Detail> _detailsList;
    public ObjectMagnet objectMagnet;
 
    void Start () {
        if(objectMagnet != null){
            _detailsList = objectMagnet.detailsList;
            foreach(Detail detail in _detailsList){
                GameObject itemList = Instantiate(ListItemPrefab, ContentPanel.transform, false);
                ListItem listItem = itemList.GetComponent<ListItem>();
                listItem.detail = detail;
                listItem.countText.text = detail.count.ToString();
                //listItem.instObject = simpleDetail.gameObject;
                itemList.GetComponent<Image>().sprite = detail.icon;
            }
        }
    }
}