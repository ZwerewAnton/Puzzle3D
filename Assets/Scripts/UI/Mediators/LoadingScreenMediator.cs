using System.Threading.Tasks;
using Infrastructure.SceneManagement;
using UnityEngine;
using Zenject;

namespace UI.Mediators
{
    public class LoadingScreenMediator : MonoBehaviour, ILoadingScreen
    {
        [SerializeField] private LoadingScreen loadingScreen;
        private SceneSwitcher _sceneSwitcher;
        
        [Inject]
        private void Construct(SceneSwitcher sceneSwitcher)
        {
            _sceneSwitcher = sceneSwitcher;
        }

        private void Start()
        {
            _sceneSwitcher.SceneLoadingUpdated += loadingScreen.SetProgress;
        }

        private void OnDestroy()
        {
            _sceneSwitcher.SceneLoadingUpdated -= loadingScreen.SetProgress;
        }
        
        public void ShowLoadingScreenImmediately()
        {
            loadingScreen.ShowLoadingScreenImmediately();
        }

        public async Task ShowAsync()
        {
            await loadingScreen.ShowLoadingScreenAsync();
        }

        public async Task HideAsync()
        {
            await loadingScreen.HideLoadingScreenAsync();
        }
    }
}