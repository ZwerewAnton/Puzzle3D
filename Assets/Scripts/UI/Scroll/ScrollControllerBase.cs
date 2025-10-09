using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Scroll
{
    public abstract class ScrollControllerBase<TModel, TItem> : MonoBehaviour, IBeginDragHandler, IEndDragHandler 
        where TItem : ScrollItemView<TModel>
    {
        [SerializeField] protected ScrollRect scrollRect;
        [SerializeField] protected RectTransform content;
        [SerializeField] protected GameObject itemPrefab;

        [Header("Config")]
        [Range(0f, 20f)] [SerializeField] protected float snapSpeed = 10f;
        [Range(0f, 500f)] [SerializeField] protected float itemSpacing = 50f;
        [SerializeField] protected bool enableSnap = true;

        protected readonly List<TModel> models = new();
        protected readonly List<TItem> activeItems = new();
        protected float itemSize;
        protected int visibleItemCount;
        protected bool initialized;
        protected float borderSpacing = 0f;
        protected bool isScrolling;

        protected virtual void OnEnable()
        {
            scrollRect.onValueChanged.AddListener(OnScrollChanged);
        }

        protected virtual void OnDisable()
        {
            scrollRect.onValueChanged.RemoveListener(OnScrollChanged);
        }

        public virtual void Initialize(List<TModel> newModels)
        {
            initialized = true;
            models.Clear();
            models.AddRange(newModels);

            var itemRect = itemPrefab.GetComponent<RectTransform>();
            itemSize = GetItemSize(itemRect);

            borderSpacing = GetViewportSize() / 2f;
            
            var cellWidth = itemSize + itemSpacing;
            var contendWidth = cellWidth * newModels.Count + borderSpacing * 2f - cellWidth;
            content.sizeDelta = new Vector2(contendWidth, content.sizeDelta.y);

            visibleItemCount = Mathf.CeilToInt(GetViewportSize() / (itemSize + itemSpacing)) + 2;
            CreatePool();
            
            UpdateVisibleItems();
        }

        protected virtual void CreatePool()
        {
            for (int i = 0; i < visibleItemCount; i++)
            {
                var go = Instantiate(itemPrefab, content);
                var item = go.GetComponent<TItem>();
                item.Clicked += OnItemClicked;
                activeItems.Add(item);
            }
        }

        protected virtual void OnScrollChanged(Vector2 _)
        {
            UpdateVisibleItems();
        }

        protected virtual void UpdateVisibleItems()
        {
            if (!initialized || models.Count == 0)
            {
                return;
            }
            
            var viewportWidth = scrollRect.viewport.rect.width;
            var cellWidth = itemSize + itemSpacing;
            var contentOffset = -content.anchoredPosition.x - viewportWidth / 2f;

            var firstVisibleIndex = Mathf.FloorToInt((contentOffset / cellWidth));
            firstVisibleIndex = Mathf.Max(0, firstVisibleIndex);

            for (var i = 0; i < activeItems.Count; i++)
            {
                var item = activeItems[i];
                var modelIndex = firstVisibleIndex + i;
                if (modelIndex < 0 || modelIndex >= models.Count)
                {
                    item.gameObject.SetActive(false);
                    continue;
                }

                if (!item.gameObject.activeSelf)
                {
                    item.gameObject.SetActive(true);
                }
                
                item.SetData(modelIndex, models[modelIndex]);

                var x = modelIndex * cellWidth + borderSpacing;
                item.RectTransform.anchoredPosition = new Vector2(x, 0f);
            }
        }
        
        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            isScrolling = true;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            isScrolling = false;
        }
        
        protected virtual void OnItemClicked(int itemIndex) {}
        protected abstract float GetItemSize(RectTransform rect);
        protected abstract float GetViewportSize();
        protected abstract Vector2 GetAnchoredPosition(int index);
    }
}
