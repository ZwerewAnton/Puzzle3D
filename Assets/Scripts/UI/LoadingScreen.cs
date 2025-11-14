using System;
using System.Collections;
using System.Threading.Tasks;
using Configs;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private Slider progressBar;
        [SerializeField] private CanvasGroup canvasGroup;

        private ApplicationConfigs _configs;
        private bool _isOn;

        [Inject]
        private void Construct(ApplicationConfigs configs)
        {
            _configs = configs;
        }
        
        private void Awake()
        {
            SetProgress(0f);
        }
        
        public void SetProgress(float value)
        {
            progressBar.value = value;
        }
        
        public void ShowLoadingScreenImmediately()
        {
            if (_isOn)
                return;
            
            gameObject.SetActive(true);
            SetProgress(0f);
            canvasGroup.alpha = 1f;
            _isOn = true;
        }
        
        public async Task ShowLoadingScreenAsync()
        {
            if (_isOn)
                return;
            
            gameObject.SetActive(true);
            SetProgress(0f);
            await FadeLoadingScreen(0f, 1f, () => _isOn = true)
                .AsyncWaitForCompletion();
        }
        
        public async Task HideLoadingScreenAsync()
        {
            await FadeLoadingScreen(1f, 0f, () =>
                {
                    gameObject.SetActive(false);
                    _isOn = false;
                })
                .AsyncWaitForCompletion();
        }
        
        private Tween FadeLoadingScreen(float startValue, float targetValue, Action completed = null)
        {
            canvasGroup.alpha = startValue;
            var duration = _configs.loadingScreenFadeTime;
            
            return canvasGroup.DOFade(targetValue, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => completed?.Invoke());
        }
    }
}