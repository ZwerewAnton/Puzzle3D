using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider progressBar;
    public CanvasGroup canvasGroup;
    private const float MIN_TIME_TO_SHOW = 1f;
    private AsyncOperation currentLoadingOperation;
    private float timeElapsed;
    private int nextSceneIndex;
    // Start is called before the first frame update
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
    }
    public void LoadScene()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            nextSceneIndex = 0;
        }
        else
        {
            nextSceneIndex = 1;
        }
        progressBar.value = 0;
        StartCoroutine(StartLoad());
    }
    

    IEnumerator StartLoad()
    {
        loadingScreen.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1, 1));
        timeElapsed += Time.deltaTime;
        currentLoadingOperation = SceneManager.LoadSceneAsync(nextSceneIndex);
        while (!currentLoadingOperation.isDone && timeElapsed <= MIN_TIME_TO_SHOW)
        {
            progressBar.value = Mathf.Clamp01(currentLoadingOperation.progress / 0.9f);
            yield return null;
        }

        yield return StartCoroutine(FadeLoadingScreen(0, 1));
        loadingScreen.SetActive(false);
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
}
