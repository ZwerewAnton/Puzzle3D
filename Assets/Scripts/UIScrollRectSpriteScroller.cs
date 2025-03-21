﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScrollRectSpriteScroller : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public float dragDistance = 70f;
    public ScrollRect scrollRect;
    public ObjectMagnet objectMagnet;
    public GameObject contentPanel;
    public GameObject listItemPrefab;
    public AudioSource audioSource;
    public AudioClip instantiateDetailClip;
    public AudioClip installDetailClip;

    private  const int DEFAULTPOINTERID = -10;
    private Vector2 _startClickPosition;
    private bool _isInstantiate;
    private ListItem _listItem;
    private GameObject _myObject;
    private List<Detail> _allDetails;
    private List<ListItem> _listItemList;
    private int _currentPointerId = DEFAULTPOINTERID;
    private Color32 _enableColor = new Color32(255, 255, 255, 255);
    private Color32 _disableColor = new Color32(255, 255, 255, 100);

    private void Start() 
    {
        InstantiateListItems();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(_currentPointerId == DEFAULTPOINTERID)
        {
            _currentPointerId = eventData.pointerId;
            _myObject = eventData.pointerCurrentRaycast.gameObject;
            _listItem = eventData.pointerCurrentRaycast.gameObject.GetComponent<ListItem>();
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(_listItem != null && _listItem.isInteractable)
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
            if (_listItem != null)
            {
                objectMagnet.InstantiateObject(_listItem.detail);
                PlayInstantiateDetailSound();
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
            }
        }
        else if (!_isInstantiate && (Mathf.Abs(eventData.position.y - _startClickPosition.y) > dragDistance))
        {   
            scrollRect.vertical = true;
        }
    }
 
    public void OnEndDrag(PointerEventData eventData)
    {
        _currentPointerId = DEFAULTPOINTERID;
        scrollRect.vertical = true;
        if(_isInstantiate)
        {
            _isInstantiate = false;
            if(objectMagnet.InstalOrDropObject())
            {
                PlayInstallDetailSound();
                ScrollRectUpdate(_listItem.detail);
                if(objectMagnet.IsLastDetail())
                {
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
    }


    private void InstantiateListItems()
    {
        if(objectMagnet != null)
        {
            _listItemList = new List<ListItem>();
            _allDetails = objectMagnet.GetALlDetails();

            foreach(Detail detail in _allDetails)
            {
                bool isInstalled = true;
                foreach(PointParentConnector pPC in detail.points)
                {
                    if(pPC.IsInstalled == false){
                        isInstalled = false;
                        break;
                    }
                }
                if(!isInstalled)
                {
                    GameObject itemList = Instantiate(listItemPrefab, contentPanel.transform, false);
                    ListItem listItem = itemList.GetComponent<ListItem>();
                    listItem.detail = detail;
                    listItem.count = detail.CurrentCount;
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
                    ScrollRectUpdate(detail);
                }
            }
        }
    }
    public void Restart()
    {
        for(int i =0; i < _listItemList.Count; i++)
        {
            Destroy(_listItemList[i].gameObject);
        }
        _listItemList.Clear();
        InstantiateListItems();
    }

    public void ScrollRectUpdate(Detail detail)
    {
        List<Detail> _availableDetails = objectMagnet.GetAvailableDetails();
        List<ListItem> openListItem = new List<ListItem>();

        foreach (Detail availableDetail in _availableDetails)
        {
            foreach(ListItem listItem in _listItemList)
            {   
                listItem.isInteractable = false;
                listItem.image.color = _disableColor;
                listItem.countText.color = _disableColor;
                
                if(availableDetail == listItem.detail)
                {
                    openListItem.Add(listItem);
                }
            }
        }
        foreach(ListItem listItem in openListItem)
        {
            listItem.isInteractable = true;
            listItem.image.color = _enableColor;
            listItem.countText.color = _enableColor;
        }
    }

    private void PlayInstantiateDetailSound()
    {
        audioSource.PlayOneShot(instantiateDetailClip);
    }    
    private void PlayInstallDetailSound()
    {
        audioSource.PlayOneShot(installDetailClip);
    }
}
