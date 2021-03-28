using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDragSprite : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler,IPointerDownHandler, IPointerUpHandler
{
    public ScrollRect scrollRect;
    //public UIController uiController;
    public float pointerScale = 1.1f;
    public float dragDistance = 70f;

    
    public UnityEvent instantiateDetailEvent;
    public UnityEvent dropDetailEvent;
    
    [SerializeField] 
    private UnityEvent rotateCameraOnEvent;
    [SerializeField] 
    private UnityEvent rotateCameraOffEvent;
    
    //wtf
    public UnityEvent startDragListItemEvent;
    
    
    
    //TODO: Make a picture zooming on click method
    
    private bool passingEvent = false;
    private RectTransform _rectTransform;
    private Vector3 _originScale;
    private Vector2 _startClickPosition;
    private bool _isInstantiate;
    private void Start()
    {
        _rectTransform = gameObject.GetComponent<RectTransform>();
        _originScale = _rectTransform.localScale;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startClickPosition = eventData.position;

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isInstantiate && (eventData.position.x - _startClickPosition.x) > dragDistance)
        {
            _isInstantiate = true;
            //eventData.pointerDrag = null;
            _rectTransform.localScale = _originScale;
            if (_isInstantiate)
            {
                instantiateDetailEvent.Invoke();
            }
        }
        else if ((eventData.position.y - _startClickPosition.y) > dragDistance)
        {      
            ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.beginDragHandler);
            passingEvent = true;

            startDragListItemEvent.Invoke();
        }
        else
        {
            
        }



        if (passingEvent) // Don't send dragHandler before beginDragHandler has been called. It gives unwanted results...
        {
            ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.dragHandler);
        }

    }
 
    public void OnEndDrag(PointerEventData eventData)
    {
        ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.endDragHandler);
        passingEvent = false;


        dropDetailEvent.Invoke();
        _isInstantiate = false;
        //scrollRect.OnEndDrag(eventData);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        
        _rectTransform.localScale = _originScale*pointerScale;
        
        rotateCameraOnEvent.Invoke();
        //uiController.RotateCameraOn();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _rectTransform.localScale = _originScale;
        rotateCameraOnEvent.Invoke();
        //uiController.RotateCameraOn();
    }
}