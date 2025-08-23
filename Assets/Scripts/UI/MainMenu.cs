using Level;
using Music;
using SaveSystem;
using SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [FormerlySerializedAs("tapToPlayGO")] public GameObject tapToPlayGo;
        public GameObject playButton;
        public GameObject scrollRect;
        public GameObject miniHouse;
        public GameObject settingsPanel;
        public UIMainMenuScrollRectController scrollController;
        public AudioSource audioSource;
        public AudioClip tapToPlayClip;
        public AudioClip playClip;
        [FormerlySerializedAs("_onFirstTap")] [SerializeField] private UnityEvent onFirstTap;

        public GameObject disassembleWindow;
        public GameObject disassembleButton;

        private SceneLoader _sceneLoader;
        private MusicPlayer _musicPlayer;
    
        private void Start()
        {
            _sceneLoader = FindObjectOfType<SceneLoader>();
            _musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
            if (_sceneLoader.IsSecondLaunch())
            {
                HideStartScreen();
            }
        }
    
        public void FirstTap()
        {
            HideStartScreen();
            PlayTapToPlayClip();
            _musicPlayer.Play();
        }
    
        public void Play()
        {
            LevelSaver.levelID = scrollController.GetLevelID();
            PlayStartGameClip();
            _sceneLoader.LoadNextScene();
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
    
        private void PlayTapToPlayClip()
        {
            audioSource.PlayOneShot(tapToPlayClip);
        }
    
        private void PlayStartGameClip()
        {
            audioSource.PlayOneShot(playClip);
        }
    }
}