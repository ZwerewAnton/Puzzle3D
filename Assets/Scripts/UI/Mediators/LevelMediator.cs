using System;
using System.Collections.Generic;
using UI.Common;
using UI.Game.DetailsScroll;
using UnityEngine;
using Zenject;

namespace UI.Mediators
{
    public class LevelMediator : MonoBehaviour
    {
        [SerializeField] private DetailsScrollController detailsScrollController;
        [SerializeField] private ActionButton backButton;
        [SerializeField] private ActionButton homeButton;

        private LevelMenu _levelMenu;

        [Inject]
        private void Construct(LevelMenu levelMenu)
        {
            _levelMenu = levelMenu;
        }

        private void OnEnable()
        {
            backButton.Clicked += OnBackButton;
        }

        private void OnDisable()
        {
            backButton.Clicked -= OnBackButton;
        }

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
        
        public void UpdateScrollController(List<DetailItemModel> models)
        {
            detailsScrollController.UpdateModels(models);
        }

        public void OnBackButton()
        {
            _levelMenu.BackToMainMenu();
        }
    }
}