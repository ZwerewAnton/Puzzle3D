using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
     public class ListItem : MonoBehaviour
     {
          public Detail detail;
          public TMP_Text countText;
          public Image image;
          public int count;
          [HideInInspector]
          public bool isInteractable;
     
          public void DeleteDetail()
          {
               Destroy(gameObject);
          }
     }
}
