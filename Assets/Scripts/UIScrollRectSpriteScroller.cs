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
    public GameObject contentPanel;
    public GameObject listItemPrefab;

    public LevelContainer levelContainer;
    
    [SerializeField] 
    private UnityEvent rotateCameraOffEvent;
    [SerializeField] 
    private UnityEvent rotateCameraOnEvent;

    private Vector2 _startClickPosition;
    private bool _isInstantiate;
    private ListItem _listItem;
    private GameObject _myObject;

    private List<Detail> _detailsList;
    private List<ListItem> _listItemList;

    private Color32 _enableColor = new Color32(255, 255, 255, 255);
    private Color32 _disableColor = new Color32(255, 255, 255, 100);




    private void Start() 
    {
        if(objectMagnet != null)
        {
            _listItemList = new List<ListItem>();
            //_detailsList = objectMagnet.detailsList;
            _detailsList = levelContainer._currentLevel;

            foreach(Detail detail in _detailsList)
            {
                GameObject itemList = Instantiate(listItemPrefab, contentPanel.transform, false);
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
                itemList.GetComponent<Image>().sprite = detail.icon;
                _listItemList.Add(listItem);
            }
            
            ScrollRectUpdate(objectMagnet.ground);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _myObject = eventData.pointerCurrentRaycast.gameObject;
        _listItem = eventData.pointerCurrentRaycast.gameObject.GetComponent<ListItem>();
        rotateCameraOffEvent.Invoke();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(_listItem != null)
        {
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
            if (_isInstantiate && (_listItem != null))
            {
                objectMagnet.InstantiateObject(_listItem.detail);


                _listItem.count--;
                _listItem.countText.text = _listItem.count.ToString();

                if(_listItem.count == 1)
                {
                    _listItem.countText.enabled = false;
                }
                else if(_listItem.count == 0)
                {
                    _listItem.image.enabled = false;
                    _listItem.countText.enabled = false;
                }
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
            if(objectMagnet.InstalOrDropObject())
            {
                ScrollRectUpdate(_listItem.detail);
                if(objectMagnet.IsLastDetail())
                {
                    //TODO Take this up 
                    _listItemList.Remove(_listItem);
                    _listItem.DeleteDelatil();
                }
            }
            else
            {
                _listItem.image.enabled = true;

                _listItem.count++;
                if(_listItem.count > 1)
                {
                    _listItem.countText.enabled = true;
                    _listItem.countText.text = _listItem.count.ToString();
                }
                else
                {
                    _listItem.countText.enabled = false;
                }
            }
        }
        //dropDetailEvent.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    { 
        rotateCameraOnEvent.Invoke();
    }

    public void ScrollRectUpdate(Detail detail)
    {
        List<Detail> openDetailList = objectMagnet.GetAvailableDetails();
        List<ListItem> openListItem = new List<ListItem>();

        foreach (Detail openDetail in openDetailList)
        {
            foreach(ListItem listItem in _listItemList)
            {   
                listItem.enabled = false;
                listItem.image.color = _disableColor;
                listItem.countText.color = _disableColor;
                
                if(openDetail == listItem.detail)
                {
                    openListItem.Add(listItem);
                }
            }
        }
        foreach(ListItem listItem in openListItem)
        {
            listItem.enabled = true;
            listItem.image.color = _enableColor;
            listItem.countText.color = _enableColor;
        }
    }
}
