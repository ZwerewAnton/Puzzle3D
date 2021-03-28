using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ListItem : MonoBehaviour
{
     public GameObject instObject;
     public Detail detail;

     public void DeleteDelatil(){
          if(detail.IsLastDetail()){
               Destroy(this.gameObject);
          }
     }

}
