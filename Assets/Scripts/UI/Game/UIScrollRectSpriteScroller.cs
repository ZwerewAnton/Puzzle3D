using System.Collections.Generic;
using System.Globalization;
using Gameplay;
using Gameplay.Movement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
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

        private  const int DefaultPointerId = -10;
        private Vector2 _startClickPosition;
        private bool _isInstantiate;
        private ListItem _listItem;
        private List<Detail> _allDetails;
        private List<ListItem> _listItems;
        private int _currentPointerId = DefaultPointerId;
        private readonly Color32 _enableColor = new(255, 255, 255, 255);
        private readonly Color32 _disableColor = new(255, 255, 255, 100);

        private void Start() 
        {
            InstantiateListItems();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_currentPointerId != DefaultPointerId) 
                return;
            
            _currentPointerId = eventData.pointerId;
            _listItem = eventData.pointerCurrentRaycast.gameObject.GetComponent<ListItem>();
        }
    
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_listItem != null && _listItem.isInteractable)
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
                if (_listItem == null) 
                    return;
                
                objectMagnet.InstantiateObject(_listItem.detail);
                PlayInstantiateDetailSound();
                _listItem.count--;
                _listItem.countText.text = _listItem.count.ToString(CultureInfo.InvariantCulture);

                switch (_listItem.count)
                {
                    case 1:
                        _listItem.countText.enabled = false;
                        break;
                    case 0:
                        _listItem.image.enabled = false;
                        _listItem.countText.enabled = false;
                        break;
                }
            }
            else if (!_isInstantiate && Mathf.Abs(eventData.position.y - _startClickPosition.y) > dragDistance)
            {   
                scrollRect.vertical = true;
            }
        }
 
        public void OnEndDrag(PointerEventData eventData)
        {
            _currentPointerId = DefaultPointerId;
            scrollRect.vertical = true;
            if (!_isInstantiate) 
                return;
            
            _isInstantiate = false;
            if (objectMagnet.InstallOrDropObject())
            {
                PlayInstallDetailSound();
                ScrollRectUpdate();
                if (!objectMagnet.IsLastDetail()) 
                    return;
                
                _listItems.Remove(_listItem);
                _listItem.DeleteDetail();
            }
            else
            {
                _listItem.image.enabled = true;
                _listItem.count++;
                if (_listItem.count > 1)
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


        private void InstantiateListItems()
        {
            if (objectMagnet == null) 
                return;
            
            _listItems = new List<ListItem>();
            _allDetails = objectMagnet.GetALlDetails();

            foreach (var detail in _allDetails)
            {
                var isInstalled = true;
                foreach (var pPC in detail.points)
                {
                    if (pPC.IsInstalled) 
                        continue;
                    isInstalled = false;
                    break;
                }

                if (isInstalled) 
                    continue;
                
                var itemList = Instantiate(listItemPrefab, contentPanel.transform, false);
                var listItem = itemList.GetComponent<ListItem>();
                listItem.detail = detail;
                listItem.count = detail.CurrentCount;
                if (listItem.count == 1)
                {
                    listItem.countText.enabled = false;
                }
                else
                {
                    listItem.countText.text = listItem.count.ToString();
                }
                itemList.GetComponent<Image>().sprite = detail.icon;
                _listItems.Add(listItem);
                ScrollRectUpdate();
            }
        }
        
        public void Restart()
        {
            foreach (var itemList in _listItems)
            {
                Destroy(itemList.gameObject);
            }
            _listItems.Clear();
            InstantiateListItems();
        }

        private void ScrollRectUpdate()
        {
            var availableDetails = objectMagnet.GetAvailableDetails();
            var openListItem = new List<ListItem>();

            foreach (var availableDetail in availableDetails)
            {
                foreach (var listItem in _listItems)
                {   
                    listItem.isInteractable = false;
                    listItem.image.color = _disableColor;
                    listItem.countText.color = _disableColor;
                
                    if (availableDetail == listItem.detail)
                    {
                        openListItem.Add(listItem);
                    }
                }
            }
            foreach (var listItem in openListItem)
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
}