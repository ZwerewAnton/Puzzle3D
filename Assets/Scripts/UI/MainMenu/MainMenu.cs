using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Infrastructure.SceneManagement;
using Level;
using Music;
using SaveSystem;
using UI.Mediators;
using UI.Scroll;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Zenject;

namespace UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [FormerlySerializedAs("scrollController")] public LevelScrollRect scroll;
        [SerializeField] private LevelScrollController scrollController;

        public GameObject disassembleWindow;
        public GameObject disassembleButton;

        private SceneSwitcher _sceneSwitcher;
        
        private MusicPlayer _musicPlayer;
        private SfxPlayer _sfxPlayer;
        private MainMenuMediator _mainMenuMediator;
        private GameState _gameState;

        [Inject]
        private void Construct(
            MainMenuMediator mainMenuMediator, 
            GameState gameState,
            MusicPlayer musicPlayer)
        {
            _gameState = gameState;
            _mainMenuMediator = mainMenuMediator;
            _musicPlayer = musicPlayer;
        }
        
        private void Start()
        {
            SetPanelsVisibility();
            
            var items = new List<LevelItemModel>();
            for (var i = 0; i < 10; i++)
            {
                items.Add(new LevelItemModel { levelName = $"Item {i}", levelIcon = null, progressPercent = Random.Range(0f, 100f).ToString()});
            }
            scrollController.Initialize(items);
            scroll.SetItems(items);
        }
    
        public void FirstTap()
        {
            _mainMenuMediator.HideTapToPlayPanel();
            _musicPlayer.Play(MusicType.MainMenu);
        }
    
        public async Task Play()
        {
            LevelSaver.levelID = scroll.GetLevelID();
            _sfxPlayer.PlayStartGameClip();
            await _sceneSwitcher.LoadSceneAsync(SceneType.Level);
        }

        private void SetPanelsVisibility()
        {
            if (_gameState.IsFirstMenuLaunch)
            {
                _mainMenuMediator.ShowTapToPlayPanel();
            }
            else
            {
                _mainMenuMediator.HideTapToPlayPanel();
                _musicPlayer.Play(MusicType.MainMenu);
            }
        }
    
        public void ShowDisassembleWindow()
        {
            disassembleWindow.SetActive(true);
        }
    
        public void CloseDisassembleWindow()
        {
            disassembleWindow.SetActive(false);
        }
    
        public void DisassembleDetail()
        {
            var levelID = scroll.GetLevelID();
            LevelSaver.levelID = levelID;
            LevelContainer.currentLevelContainer.ResetLevel(levelID);
            scroll.UpdatePercents();
            disassembleWindow.SetActive(false);
        }
    }
}