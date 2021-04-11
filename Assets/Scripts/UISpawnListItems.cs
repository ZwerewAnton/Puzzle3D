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
                listItem.count = detail.count;
                if(listItem.count == 1)
                {
                    listItem.countText.enabled = false;
                }
                else
                {
                    listItem.countText.text = listItem.count.ToString();
                }
                //listItem.instObject = simpleDetail.gameObject;
                itemList.GetComponent<Image>().sprite = detail.icon;
            }
        }
    }
}