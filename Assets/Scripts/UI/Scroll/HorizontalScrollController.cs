using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Scroll
{
    public class HorizontalScrollController<TModel, TItem> : ScrollControllerBase<TModel, TItem>
        where TItem : ScrollItemView<TModel>
    {
        [Header("Animation")]
        [Range(0.5f, 1.5f)] public float maxScale = 1.2f;
        [Range(0.5f, 1.0f)] public float minScale = 0.8f;
        
        private Vector2 _closestItemAnchoredPosition;
        private float _lastDragDeltaX = 0f;

        protected void LateUpdate()
        {
            UpdateItemsSize();
            SnapToItem();
        }
        
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            scrollRect.inertia = true;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            scrollRect.inertia = false;
            _lastDragDeltaX = eventData.delta.x;
            _closestItemAnchoredPosition = FindNearestItemPosition();
        }
        
        protected override float GetItemSize(RectTransform rect)
        {
            return rect.rect.width;
        }

        protected override float GetViewportSize()
        {
            return scrollRect.viewport.rect.width;
        }

        protected override Vector2 GetAnchoredPosition(int index)
        {
            var x = index * (itemSize + itemSpacing);
            return new Vector2(x, 0);
        }

        protected override void OnItemClicked(int itemIndex)
        {
            base.OnItemClicked(itemIndex);

            foreach (var item in activeItems.Where(item => item.ItemIndex == itemIndex))
            {
                _closestItemAnchoredPosition = item.RectTransform.anchoredPosition;
            }
        }

        protected override void UpdateVisibleItems()
        {
            base.UpdateVisibleItems();

            if (enableSnap)
            {
                UpdateItemsSize();
            }
        }

        private void UpdateItemsSize()
        {
            var center = -content.anchoredPosition.x;

            foreach (var item in activeItems)
            {
                var itemCenter = item.RectTransform.anchoredPosition.x - borderSpacing;
                var signDistance = center - itemCenter;
                var distance = Mathf.Abs(signDistance);

                var t = Mathf.Clamp01(distance / borderSpacing);
                var scale = Mathf.Lerp(maxScale, minScale, t);
                item.RectTransform.localScale = new Vector3(scale, scale, 1f);
            }
        }

        private void SnapToItem()
        {
            if (!isScrolling && _closestItemAnchoredPosition != Vector2.zero)
            {
                var targetX = -_closestItemAnchoredPosition.x + borderSpacing;
                content.anchoredPosition = Vector2.Lerp(content.anchoredPosition, new Vector2(targetX, content.anchoredPosition.y), snapSpeed * Time.deltaTime);
            }
        }

        private Vector2 FindNearestItemPosition()
        {
            var center = -content.anchoredPosition.x;
            var closestDistance = float.MaxValue;
            var closestItem = Vector2.zero;

            foreach (var item in activeItems)
            {
                var itemCenter = item.RectTransform.anchoredPosition.x - borderSpacing;
                var signDistance = center - itemCenter;
                var distance = Mathf.Abs(signDistance);

                if (_lastDragDeltaX != 0f)
                {
                    if (_lastDragDeltaX > 0f)
                    {
                        if (signDistance >= 0f)
                        {
                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                closestItem = item.RectTransform.anchoredPosition;
                            }
                        }
                    }
                    else
                    {
                        if (signDistance <= 0f)
                        {
                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                closestItem = item.RectTransform.anchoredPosition;
                            }
                        }
                    }
                }
                else
                {
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestItem = item.RectTransform.anchoredPosition;
                    }
                }
            }

            return closestItem;
        }
    }
}
