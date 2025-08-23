using System.Collections;
using SaveSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader sceneLoader;
        
        public GameObject loadingScreen;
        public Slider progressBar;
        public CanvasGroup canvasGroup;
        [FormerlySerializedAs("_onStartLoading")] [SerializeField] private UnityEvent onStartLoading;
        [FormerlySerializedAs("_onCompleteLoading")] [SerializeField] private UnityEvent onCompleteLoading;
    
        private const float MinTimeToShow = 1f;
        private AsyncOperation _currentLoadingOperation;
        private float _timeElapsed;
        private int _nextSceneIndex;
        private bool _isSecondLaunch;

        private void Awake() 
        {
            if (sceneLoader == null) 
            {
                sceneLoader = this;
            } 
            else 
            {
                Destroy(gameObject);
            }
        }
    
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = 120;
        }
    
        public void LoadNextScene()
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                _nextSceneIndex = 0;
                LevelSaver.SaveGame();
                _isSecondLaunch = true;
            }
            else
            {
                _nextSceneIndex = 1;
            }
            progressBar.value = 0;
            StartCoroutine(StartLoad());
        }

        private IEnumerator StartLoad()
        {
            onStartLoading.Invoke();
            loadingScreen.SetActive(true);
            yield return StartCoroutine(FadeLoadingScreen(1, 1));
            _timeElapsed += Time.deltaTime;
            _currentLoadingOperation = SceneManager.LoadSceneAsync(_nextSceneIndex);
            while (_currentLoadingOperation is { isDone: false } && _timeElapsed <= MinTimeToShow)
            {
                progressBar.value = Mathf.Clamp01(_currentLoadingOperation.progress / 0.9f);
                yield return null;
            }

            yield return new WaitForSeconds(1000f);
            yield return StartCoroutine(FadeLoadingScreen(0, 1));
            loadingScreen.SetActive(false);
            onCompleteLoading.Invoke();
        }

        private IEnumerator FadeLoadingScreen(float targetValue, float duration)
        {
            var startValue = canvasGroup.alpha;
            float time = 0;

            while (time < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = targetValue;
        }
    
        public static int GetSceneIndex()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }
    
        public bool IsSecondLaunch()
        {
            return _isSecondLaunch;
        }
    
        private void OnDestroy()
        {
            LevelSaver.SaveGame();
        }
    }
}