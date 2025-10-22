using System;
using System.Collections.Generic;
using UI.Game.DetailsScroll;
using UnityEngine;

namespace UI.Mediators
{
    public class LevelMediator : MonoBehaviour
    {
        [SerializeField] private DetailsScrollController detailsScrollController;
        
        public event Action<DetailItemModel> DetailItemDragOutStarted
        {
            add => detailsScrollController.DragOutStarted += value;
            remove => detailsScrollController.DragOutStarted -= value;
        }
        
        public void InitializeDetailsScroll(List<DetailItemModel> models)
        {
            detailsScrollController.Initialize(models);
        }        
        
        public void MarkItemDragOutState(string detailId, bool isDragOut)
        {
            detailsScrollController.MarkItemDragOutState(detailId, isDragOut);
        }
        
        public void UpdateModels(List<DetailItemModel> models)
        {
            detailsScrollController.UpdateModels(models);
        }
    }
}