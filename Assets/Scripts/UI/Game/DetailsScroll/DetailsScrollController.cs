using System;
using System.Collections.Generic;
using UI.Scroll;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Game.DetailsScroll
{
    public class DetailsScrollController : ScrollControllerBase<DetailItemModel, DetailItemView>
    {
        [Header("Drag Settings")]
        [SerializeField] private float dragThreshold = 30f;

        public event Action<DetailItemModel> DragOutStarted;

        private bool _isDragOutStarted;
        private int _activePointerId = -1;
        private Vector2 _startDragPos;
        private int _draggedItemIndex;

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (_isDragOutStarted || _activePointerId != -1)
                return;

            base.OnBeginDrag(eventData);

            _activePointerId = eventData.pointerId;
            _startDragPos = eventData.position;

            var draggedItem = GetItemUnderPointer(eventData);
            
            if (draggedItem != null)
                _draggedItemIndex = draggedItem.ItemIndex;
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerId != _activePointerId)
                return;

            if (_isDragOutStarted)
            {
                eventData.Use();
                return;
            }

            var delta = eventData.position - _startDragPos;

            if (Mathf.Abs(delta.x) > dragThreshold && Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                StartDragOut(eventData);
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
            
            base.OnEndDrag(eventData);

            _activePointerId = -1;
            _isDragOutStarted = false;
        }

        public void MarkItemDragOutState(string detailId, bool isDragOut)
        {
            var model = Models.Find(itemModel => itemModel.ID == detailId);
            if (model == null)
                return;
            
            model.IsDragOut = isDragOut;

            MarkToUpdate();
        }

        public void UpdateModels(List<DetailItemModel> models)
        {
            Models.Clear();
            Models.AddRange(models);
            
            MarkToUpdate();
        }

        private void StartDragOut(PointerEventData eventData)
        {
            if (_draggedItemIndex < 0 || _draggedItemIndex >= Models.Count)
                return;

            _isDragOutStarted = true;
            ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.endDragHandler);

            DragOutStarted?.Invoke(Models[_draggedItemIndex]);
        }
        
        private void UpdateInstalledDetailModel(DetailItemModel model, int count)
        {
            if (model.Count == 1)
            {
                Models.Remove(model);
            }
            else
            {
                model.Count = count;
            }
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