using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScrollRectSpriteScroller : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public float pointerScale = 1.1f;
    public float dragDistance = 70f;
    public ScrollRect scrollRect;
    public ObjectMagnet objectMagnet;
    
    //public InstantiateEvent instantiateDetailEvent;
    //public UnityEvent instantiateDetailEvent;
    //public UnityEvent dropDetailEvent;

    [SerializeField] 
    private UnityEvent rotateCameraOffEvent;
    [SerializeField] 
    private UnityEvent rotateCameraOnEvent;

    private Vector2 _startClickPosition;
    private bool _isInstantiate;
    private ListItem listItem;
    private GameObject myObject;


    public void OnPointerDown(PointerEventData eventData)
    {
        /*
        //if(eventData.pointerEnter.gameObject.GetComponent<ListItem>)
        if(eventData.pointerCurrentRaycast.gameObject.TryGetComponent<ListItem>(out ListItem _listItem)){
            listItem = _listItem;
        }
        else{
            listItem = null;
        }
        */
        myObject = eventData.pointerCurrentRaycast.gameObject;
        listItem = eventData.pointerCurrentRaycast.gameObject.GetComponent<ListItem>();
        rotateCameraOffEvent.Invoke();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(listItem != null){
            scrollRect.vertical = false;
        }
        _startClickPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!scrollRect.vertical && !_isInstantiate && (eventData.position.x - _startClickPosition.x) > dragDistance)
        {
            _isInstantiate = true;
            //_rectTransform.localScale = _originScale;
            if (_isInstantiate && (listItem != null))
            {
                objectMagnet.InstantiateObject(listItem.detail);
                //instantiateDetailEvent.Invoke();
            }
        }
        else if (!_isInstantiate && (Mathf.Abs(eventData.position.y - _startClickPosition.y) > dragDistance))
        {   
            scrollRect.vertical = true;
        }
    }
 
    public void OnEndDrag(PointerEventData eventData)
    {
        scrollRect.vertical = true;

        if(_isInstantiate)
        {
            _isInstantiate = false;
            if(objectMagnet.IsLastDetail())
            {
                listItem.DeleteDelatil();
            }
        }
        //dropDetailEvent.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    { 
        rotateCameraOnEvent.Invoke();
    }
}
