using System.Collections.Generic;
using Music;
using UI.Common;
using UI.MainMenu;
using UI.MainMenu.LevelScroll;
using UI.Scroll;
using UnityEngine;
using Zenject;

namespace UI.Mediators
{
    public class MainMenuMediator : MonoBehaviour
    {
        [SerializeField] private TapToPlayPanel tapToPlayPanel;
        [SerializeField] private SimplePanel menuPanel;
        [SerializeField] private SimplePanel miniHousePanel;
        [SerializeField] private MainMenu.MainMenu mainMenu;
        [SerializeField] private LevelsScrollController scrollController;
        [SerializeField] private ActionButton playButton;

        private SfxPlayer _sfxPlayer;

        [Inject]
        private void Construct(SfxPlayer sfxPlayer)
        {
            _sfxPlayer = sfxPlayer;
        }

        private void Awake()
        {
            tapToPlayPanel.Clicked += mainMenu.FirstTap;
            tapToPlayPanel.Clicked += _sfxPlayer.PlayTapToPlayClip;
            playButton.Clicked += _sfxPlayer.PlayStartGameClip;
            playButton.Clicked += mainMenu.Play;
        }

        private void OnDestroy()
        {
            tapToPlayPanel.Clicked -= mainMenu.FirstTap;
            tapToPlayPanel.Clicked -= _sfxPlayer.PlayTapToPlayClip;
            playButton.Clicked -= _sfxPlayer.PlayStartGameClip;
            playButton.Clicked -= mainMenu.Play;
        }

        public void InitializeLevelScroll(List<LevelItemModel> models)
        {
            scrollController.Initialize(models);
        }

        public void MoveLevelScrollToItem(string levelName)
        {
            scrollController.MoveToItem(levelName);
        }

        public LevelItemModel GetScrollTargetItem()
        {
            return scrollController.GetTargetItemModel();
        }

        public void ShowTapToPlayPanel()
        {
            tapToPlayPanel.Show();
            miniHousePanel.Show();
            menuPanel.Hide();
        }
        
        public void HideTapToPlayPanel()
        {
            tapToPlayPanel.Hide();
            miniHousePanel.Hide();
            menuPanel.Show();
        }
    }
}