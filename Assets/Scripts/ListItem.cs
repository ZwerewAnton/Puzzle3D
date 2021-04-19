using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ListItem : MonoBehaviour
{
     //public GameObject instObject;
     public Detail detail;
     public TMP_Text countText;
     public Image image;
     [HideInInspector]
     public bool isInteractable = false;
     


     public float count = 0;
     
     public void DeleteDelatil(){
          Destroy(this.gameObject);
     }

     

}
