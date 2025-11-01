using System;
using System.Threading.Tasks;
using Gameplay;
using Infrastructure.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LevelMenu : MonoBehaviour
    {
        [FormerlySerializedAs("_endClip")] [SerializeField] private AudioClip endClip;
        [FormerlySerializedAs("_homeButton")] [SerializeField] private Button homeButton;
        [FormerlySerializedAs("_scrollRect")] [SerializeField] private ScrollRect scrollRect;
        [FormerlySerializedAs("_audioSource")] [SerializeField] private AudioSource audioSource;

        private LevelSaverMono _levelSaverMono;
        private SceneSwitcher _sceneSwitcher;

        [Inject]
        private void Construct(LevelSaverMono levelSaverMono, SceneSwitcher sceneSwitcher)
        {
            _levelSaverMono = levelSaverMono;
            _sceneSwitcher = sceneSwitcher;
        }
        
        public void PlayEndClip()
        {
            audioSource.PlayOneShot(endClip);
        }
        
        public void ShowEndScreen()
        {
            homeButton.gameObject.SetActive(true);
            scrollRect.gameObject.SetActive(false);
        }

        public void BackToMainMenu()
        {
            _levelSaverMono.SaveProgress();
            _ = LoadMainMenuScene();
        }
        
        private async Task LoadMainMenuScene()
        {
            try
            {
                await _sceneSwitcher.LoadSceneAsync(SceneType.MainMenu);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load main menu scene: {ex}");
            }
        }
    }
}