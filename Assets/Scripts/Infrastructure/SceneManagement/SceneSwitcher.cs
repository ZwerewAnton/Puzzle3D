using System.Collections;
using System.Threading.Tasks;
using SaveSystem;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Infrastructure.SceneManagement
{
    public class SceneSwitcher : MonoBehaviour
    {
        [FormerlySerializedAs("_onStartLoading")] [SerializeField] private UnityEvent onStartLoading;
        [FormerlySerializedAs("_onCompleteLoading")] [SerializeField] private UnityEvent onCompleteLoading;

        [SerializeField] private LoadingScreen loadingScreen;

        private void Awake() 
        {
            DontDestroyOnLoad(gameObject);
        }

        public void SwitchScene(SceneType sceneTypeName)
        {
            StartCoroutine(SwitchSceneCoroutine(sceneTypeName));
        }

        public void LoadNextScene()
        {
            
        }

        private IEnumerator SwitchSceneCoroutine(SceneType sceneTypeName)
        {
            yield return loadingScreen.ShowLoadingScreen();
            
            var operation = SceneManager.LoadSceneAsync(sceneTypeName.ToString());
            if (operation != null)
            {
                operation.allowSceneActivation = false;

                while (!operation.isDone)
                {
                    var progress = Mathf.Clamp01(operation.progress / 0.9f);
                    loadingScreen.SetProgress(progress);

                    if (progress >= 0.9f)
                        operation.allowSceneActivation = true;

                    yield return null;
                }

                loadingScreen.SetProgress(1f);
                yield return new WaitForSeconds(0.1f);
            }
            
            yield return loadingScreen.HideLoadingScreen();
        }
        
        public static int GetSceneIndex()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }
    }
}