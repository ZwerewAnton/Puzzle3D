using System;
using UI.Scroll;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Game.DetailsScroll
{
    public class DetailsScrollController : ScrollControllerBase<DetailItemModel, DetailItemView>
    {
        [Header("Drag Settings")]
        [SerializeField] private float dragThreshold = 30f;

        public event Action<DetailItemModel> ItemDragStarted;

        private bool _isDraggingItem;
        private int _activePointerId = -1;
        private Vector2 _startDragPos;
        private int _draggedItemIndex;

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (_isDraggingItem || _activePointerId != -1)
                return;

            base.OnBeginDrag(eventData);

            _activePointerId = eventData.pointerId;
            _startDragPos = eventData.position;

            var draggedItem = GetItemUnderPointer(eventData);
            if (draggedItem != null)
            {
                _draggedItemIndex = draggedItem.ItemIndex;
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerId != _activePointerId)
                return;

            if (_isDraggingItem)
            {
                eventData.Use();
                return;
            }

            var delta = eventData.position - _startDragPos;

            if (Mathf.Abs(delta.x) > dragThreshold && Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                StartItemDrag(eventData);
            }
            else
            {
                base.OnDrag(eventData);
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.pointerId != _activePointerId)
                return;

            if (_isDraggingItem)
                StopItemDrag();
            else
                base.OnEndDrag(eventData);

            _activePointerId = -1;
            _isDraggingItem = false;
        }

        private void StartItemDrag(PointerEventData eventData)
        {
            if (_draggedItemIndex < 0 || _draggedItemIndex >= Models.Count)
                return;

            _isDraggingItem = true;
            ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.endDragHandler);

            ItemDragStarted?.Invoke(Models[_draggedItemIndex]);
        }

        private void StopItemDrag()
        {
            _isDraggingItem = false;
        }

        private DetailItemView GetItemUnderPointer(PointerEventData eventData)
        {
            foreach (var item in ActiveItems)
            {
                var rect = item.RectTransform;
                if (RectTransformUtility.RectangleContainsScreenPoint(rect, eventData.position, eventData.pressEventCamera))
                    return item;
            }
            return null;
        }
    }
}