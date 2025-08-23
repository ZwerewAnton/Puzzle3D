using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class UISettingButtonController : MonoBehaviour,  IPointerUpHandler, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            GUIUtility.hotControl = 1;
        }

        public void OnPointerUp(PointerEventData eventData)
        { 
            GUIUtility.hotControl = 0;
        }
    }
}