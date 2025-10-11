﻿using System.Linq;
using UI.MainMenu.LevelScroll;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Scroll
{
    public class LevelScrollController : ScrollControllerBase<LevelItemModel, LevelItemView>
    {
        [Header("Snap")]
        [SerializeField, Range(0f, 20f)] protected float snapSpeed = 10f;
        [SerializeField] protected bool enableSnap = true;
        [SerializeField] protected  float snapThreshold = 0.5f;

        [Header("Animation")]
        [SerializeField, Range(0.5f, 1.5f)] protected float MaxScale = 1f;
        [SerializeField, Range(0.5f, 1.0f)] protected float MinScale = 0.8f;

        private TargetItemData _targetItemData;
        private float _lastDragDirection;
        private bool _shouldSnap;

        #region Unity Events
        
        protected void LateUpdate()
        {
            if (enableSnap)
                AnimateItemScaling();

            if (_shouldSnap)
                SmoothSnap();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            scrollRect.inertia = true;
            _shouldSnap = false;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            scrollRect.inertia = false;

            _lastDragDirection = Mathf.Sign(eventData.delta.x);
            _targetItemData = FindNearestItemData();
            _shouldSnap = true;
        }

        #endregion
        
        #region Content Management

        public LevelItemModel GetTargetItemModel()
        {
            return Models[_targetItemData.ItemIndex];
        }

        public void MoveToItem(string levelName)
        {
            var itemIndex = Models.FindIndex(model => model.levelName == levelName);
            if (itemIndex < 0)
            {
                return;
            }
            var targetX = (ItemSize + itemSpacing) * itemIndex;
            content.anchoredPosition = new Vector2(-targetX, content.anchoredPosition.y);
            _targetItemData = new TargetItemData(GetAnchoredPosition(itemIndex), itemIndex);
        }
        
        protected override void UpdateVisibleItems()
        {
            base.UpdateVisibleItems();

            if (enableSnap)
                AnimateItemScaling();
        }

        protected override float GetItemSize(RectTransform rect) => rect.rect.width;
        protected override float GetViewportSize() => scrollRect.viewport.rect.width;

        protected override Vector2 GetAnchoredPosition(int index)
        {
            var x = index * (ItemSize + itemSpacing) + BorderSpacing;
            return new Vector2(x, 0f);
        }

        protected override void OnItemClicked(int itemIndex)
        {
            base.OnItemClicked(itemIndex);

            var item = ActiveItems.FirstOrDefault(i => i.ItemIndex == itemIndex);
            if (item == null)
                return;

            _targetItemData = new TargetItemData(item.RectTransform.anchoredPosition, item.ItemIndex);
            _shouldSnap = true;
        }

        #endregion
        
        #region Snap and Animation
        
        private void AnimateItemScaling()
        {
            var center = -content.anchoredPosition.x;

            foreach (var item in ActiveItems)
            {
                var itemCenter = item.RectTransform.anchoredPosition.x - BorderSpacing;
                var distance = Mathf.Abs(center - itemCenter);

                var t = Mathf.Clamp01(distance / BorderSpacing);
                var scale = Mathf.Lerp(MaxScale, MinScale, t);

                item.RectTransform.localScale = new Vector3(scale, scale, 1f);
            }
        }

        private void SmoothSnap()
        {
            var currentX = content.anchoredPosition.x;
            var targetX = -_targetItemData.AnchoredPosition.x + BorderSpacing;

            var newX = Mathf.Lerp(currentX, targetX, snapSpeed * Time.deltaTime);
            content.anchoredPosition = new Vector2(newX, content.anchoredPosition.y);

            if (Mathf.Abs(newX - targetX) < snapThreshold)
            {
                content.anchoredPosition = new Vector2(targetX, content.anchoredPosition.y);
                _shouldSnap = false;
            }
        }

        private TargetItemData FindNearestItemData()
        {
            var center = -content.anchoredPosition.x;
            var closestDist = float.MaxValue;
            var closest = new TargetItemData();

            foreach (var item in ActiveItems)
            {
                var itemCenter = item.RectTransform.anchoredPosition.x - BorderSpacing;
                var distance = center - itemCenter;

                var validByDirection =
                    Mathf.Approximately(_lastDragDirection, 0f) ||
                    (_lastDragDirection > 0 && distance >= 0f) ||
                    (_lastDragDirection < 0 && distance <= 0f);

                if (!validByDirection) continue;

                var absDist = Mathf.Abs(distance);
                if (absDist < closestDist)
                {
                    closestDist = absDist;
                    closest.AnchoredPosition = item.RectTransform.anchoredPosition;
                    closest.ItemIndex = item.ItemIndex;
                }
            }

            return closest;
        }
        
        #endregion
    }
}