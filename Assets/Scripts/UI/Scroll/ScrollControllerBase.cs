using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Scroll
{
    public abstract class ScrollControllerBase<TModel, TItem> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
        where TItem : ScrollItemView<TModel>
    {
        [SerializeField] protected ScrollRect scrollRect;
        [SerializeField] protected RectTransform content;
        [SerializeField] protected GameObject itemPrefab;

        [Header("Items")]
        [Range(0f, 500f)] [SerializeField] protected float itemSpacing = 50f;
        [Range(0f, 500f)] [SerializeField] protected float borderSpacing = 50f;

        protected readonly List<TModel> Models = new();
        protected readonly List<TItem> ActiveItems = new();

        protected float ItemSize;
        protected int VisibleItemCount;
        protected bool Initialized;
        protected float BorderSpacing;

        #region Unity Events

        protected virtual void OnEnable()
        {
            if (scrollRect != null)
                scrollRect.onValueChanged.AddListener(OnScrollChanged);
        }

        protected virtual void OnDisable()
        {
            if (scrollRect != null)
                scrollRect.onValueChanged.RemoveListener(OnScrollChanged);
        }

        protected virtual void OnDestroy()
        {
            ClearPool();
        }

        public virtual void OnBeginDrag(PointerEventData eventData) {}
        
        public virtual void OnEndDrag(PointerEventData eventData) {}
        
        public virtual void OnDrag(PointerEventData eventData) {}
        
        #endregion

        #region Initialization

        public virtual void Initialize(List<TModel> newModels)
        {
            if (newModels == null || newModels.Count == 0)
                return;

            if (Initialized)
                ClearPool();

            Initialized = true;

            Models.Clear();
            Models.AddRange(newModels);

            var itemRect = itemPrefab.GetComponent<RectTransform>();
            ItemSize = GetItemSize(itemRect);
            BorderSpacing = GetBorderSpacing();

            var cellSize = ItemSize + itemSpacing;
            var totalSize = cellSize * Models.Count + BorderSpacing * 2f - cellSize;
            SetContentSize(totalSize);
            VisibleItemCount = Mathf.CeilToInt(GetViewportSize() / cellSize) + 2;

            CreatePool();
            UpdateVisibleItems();
        }
        
        protected virtual void SetContentSize(float totalSize)
        {
            if (scrollRect.horizontal)
                content.sizeDelta = new Vector2(totalSize, content.sizeDelta.y);
            else
                content.sizeDelta = new Vector2(content.sizeDelta.x, totalSize);
        }

        #endregion

        #region Pool Management

        protected virtual void CreatePool()
        {
            ClearPool();

            for (var i = 0; i < VisibleItemCount; i++)
            {
                var go = Instantiate(itemPrefab, content);
                var item = go.GetComponent<TItem>();
                item.Clicked += OnItemClicked;
                ActiveItems.Add(item);
            }
        }

        protected virtual void ClearPool()
        {
            foreach (var item in ActiveItems)
            {
                if (item == null) 
                    continue;
                
                item.Clicked -= OnItemClicked;
                Destroy(item.gameObject);
            }
            ActiveItems.Clear();
        }

        #endregion

        #region Content Management

        protected virtual void OnScrollChanged(Vector2 _)
        {
            UpdateVisibleItems();
        }

        protected virtual void UpdateVisibleItems()
        {
            if (!Initialized || Models.Count == 0)
                return;

            var viewportSize = GetViewportSize();
            var cellSize = ItemSize + itemSpacing;

            var offset = GetScrollOffset() - viewportSize / 2f;
            var firstVisibleIndex = Mathf.FloorToInt(offset / cellSize);
            firstVisibleIndex = Mathf.Max(0, firstVisibleIndex);

            for (var i = 0; i < ActiveItems.Count; i++)
            {
                var modelIndex = firstVisibleIndex + i;
                var item = ActiveItems[i];

                if (modelIndex < 0 || modelIndex >= Models.Count)
                {
                    item.gameObject.SetActive(false);
                    continue;
                }

                if (!item.gameObject.activeSelf)
                    item.gameObject.SetActive(true);

                item.SetData(modelIndex, Models[modelIndex]);
                item.RectTransform.anchoredPosition = GetAnchoredPosition(modelIndex);
            }
        }

        protected virtual void OnItemClicked(int itemIndex) {}

        protected virtual float GetItemSize(RectTransform rect)
        { 
            return scrollRect.horizontal
                ? rect.rect.width
                : rect.rect.height;
        }

        protected virtual float GetViewportSize() => scrollRect.viewport.rect.width;
        
        protected virtual float GetBorderSpacing() => borderSpacing;

        protected virtual Vector2 GetAnchoredPosition(int index)
        {
            var position = index * (ItemSize + itemSpacing) + BorderSpacing;
            return scrollRect.horizontal
                ? new Vector2(position, 0f)
                : new Vector2(0f, -position);
        }

        protected virtual float GetScrollOffset()
        {
            return scrollRect.horizontal
                ? -content.anchoredPosition.x
                : content.anchoredPosition.y;
        }
        
        #endregion
    }
}
