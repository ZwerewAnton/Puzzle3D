using System;
using System.Collections.Generic;
using UI.Game.DetailsScroll;
using UnityEngine;

namespace UI.Mediators
{
    public class LevelMediator : MonoBehaviour
    {
        [SerializeField] private DetailsScrollController detailsScrollController;
        
        public event Action<DetailItemModel> DetailItemDragStarted
        {
            add => detailsScrollController.ItemDragStarted += value;
            remove => detailsScrollController.ItemDragStarted -= value;
        }
        
        public void InitializeLevelScroll(List<DetailItemModel> models)
        {
            detailsScrollController.Initialize(models);
        }
    }
}