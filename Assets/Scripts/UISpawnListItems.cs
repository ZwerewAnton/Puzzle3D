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
    public List<Detail> detailsList;
 
    void Start () {
        foreach(Detail detail in detailsList){
            GameObject itemList = Instantiate(ListItemPrefab, ContentPanel.transform, false);
            ListItem listItem = itemList.GetComponent<ListItem>();
            listItem.detail = detail;
            //listItem.instObject = simpleDetail.gameObject;
            itemList.GetComponent<Image>().sprite = detail.icon;
        }
    }
}