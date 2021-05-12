using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider progressBar;
    public CanvasGroup canvasGroup;
    private const float MIN_TIME_TO_SHOW = 1f;
    private AsyncOperation currentLoadingOperation;
    private float timeElapsed;
    private int _nextSceneIndex;
    [SerializeField] private UnityEvent _onStartLoading;
    [SerializeField] private UnityEvent _onCompleteLoading;
    private bool _isSecondLaunch;
    // Start is called before the first frame update
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void LoadNextScene()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            _nextSceneIndex = 0;
            _isSecondLaunch = true;
        }
        else
        {
            _nextSceneIndex = 1;
        }
        progressBar.value = 0;
        StartCoroutine(StartLoad());
    }
    public void LoadMenuScene(){
        
        progressBar.value = 0;
        StartCoroutine(StartLoad());
    }
    

    IEnumerator StartLoad()
    {
        _onStartLoading.Invoke();
        loadingScreen.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1, 1));
        timeElapsed += Time.deltaTime;
        currentLoadingOperation = SceneManager.LoadSceneAsync(_nextSceneIndex);
        while (!currentLoadingOperation.isDone && timeElapsed <= MIN_TIME_TO_SHOW)
        {
            progressBar.value = Mathf.Clamp01(currentLoadingOperation.progress / 0.9f);
            yield return null;
        }

        yield return StartCoroutine(FadeLoadingScreen(0, 1));
        loadingScreen.SetActive(false);
        _onCompleteLoading.Invoke();
    }

    IEnumerator FadeLoadingScreen(float targetValue, float duration)
    {
        float startValue = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetValue;
    }

    public bool IsSecondLauch()
    {
        return _isSecondLaunch;
    }
}
