using Infrastructure.SceneManagement;
using Level;
using Music;
using SaveSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [FormerlySerializedAs("tapToPlayGO")] public GameObject tapToPlayGo;
        public GameObject playButton;
        public GameObject scrollRect;
        public GameObject miniHouse;
        public GameObject settingsPanel;
        public UIMainMenuScrollRectController scrollController;
        
        public AudioClip tapToPlayClip;
        public AudioClip playClip;
        
        [FormerlySerializedAs("_onFirstTap")] [SerializeField] private UnityEvent onFirstTap;

        public GameObject disassembleWindow;
        public GameObject disassembleButton;

        private SceneSwitcher _sceneSwitcher;
        private MusicPlayer _musicPlayer;
        private SfxPlayer _sfxPlayer;
    
        private void Start()
        {
            _sceneSwitcher = FindObjectOfType<SceneSwitcher>();
            _musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
            // if (_sceneSwitcher.IsSecondLaunch())
            // {
            //     HideStartScreen();
            // }
        }
    
        public void FirstTap()
        {
            HideStartScreen();
            _sfxPlayer.PlayTapToPlayClip();
            _musicPlayer.Play(MusicType.MainMenu);
        }
    
        public void Play()
        {
            LevelSaver.levelID = scrollController.GetLevelID();
            _sfxPlayer.PlayStartGameClip();
            _sceneSwitcher.LoadNextScene();
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
            var levelID = scrollController.GetLevelID();
            LevelSaver.levelID = levelID;
            LevelContainer.currentLevelContainer.ResetLevel(levelID);
            scrollController.UpdatePercents();
            disassembleWindow.SetActive(false);
        }

        private void HideStartScreen()
        {
            onFirstTap.Invoke();
            tapToPlayGo.SetActive(false);
            miniHouse.SetActive(false);
            playButton.SetActive(true);
            disassembleButton.SetActive(true);
            scrollRect.SetActive(true);
            settingsPanel.SetActive(true);
        }
    }
}