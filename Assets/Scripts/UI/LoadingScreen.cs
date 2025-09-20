using System;
using System.Collections;
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
        
        public IEnumerator ShowLoadingScreen()
        {
            gameObject.SetActive(true);
            SetProgress(0f);
            yield return FadeLoadingScreen(0f, 1f)
                .WaitForCompletion();
        }
        
        public IEnumerator HideLoadingScreen()
        {
            yield return FadeLoadingScreen(1f, 0f, () => gameObject.SetActive(false))
                .WaitForCompletion();
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