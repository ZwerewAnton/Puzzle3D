using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider progressBar;
    public CanvasGroup canvasGroup;
    [SerializeField] private UnityEvent _onStartLoading;
    [SerializeField] private UnityEvent _onCompleteLoading;
    public static SceneLoader sceneLoader;
    private const float MIN_TIME_TO_SHOW = 1f;
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
        Application.targetFrameRate = 60;
    }
    
    public void LoadNextScene()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            _nextSceneIndex = 0;
            SaveLevel.SaveGame();
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
        _onStartLoading.Invoke();
        loadingScreen.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1, 1));
        _timeElapsed += Time.deltaTime;
        _currentLoadingOperation = SceneManager.LoadSceneAsync(_nextSceneIndex);
        while (_currentLoadingOperation is { isDone: false } && _timeElapsed <= MIN_TIME_TO_SHOW)
        {
            progressBar.value = Mathf.Clamp01(_currentLoadingOperation.progress / 0.9f);
            yield return null;
        }

        yield return StartCoroutine(FadeLoadingScreen(0, 1));
        loadingScreen.SetActive(false);
        _onCompleteLoading.Invoke();
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
        SaveLevel.SaveGame();
    }
}
