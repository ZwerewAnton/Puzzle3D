using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    [SerializeField] 
    private UnityEvent RotateCameraOffInvoke;
    [SerializeField] 
    private UnityEvent RotateCameraOnInvoke;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject);
        RotateCameraOffInvoke.Invoke();
    }
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log(eventData.selectedObject);
    }
    

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject);
        RotateCameraOnInvoke.Invoke();
    }

    public void ContinueDrag()
    {
    }
}
