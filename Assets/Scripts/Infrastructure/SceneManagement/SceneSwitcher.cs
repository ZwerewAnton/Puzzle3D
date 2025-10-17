using System;
using System.Threading;
using System.Threading.Tasks;
using UI.Mediators;
using UnityEngine;
using Zenject;

namespace Infrastructure.SceneManagement
{
    public class SceneSwitcher : IDisposable
    {
        private CancellationTokenSource _cts;
        private ILoadingScreen _loadingScreen;
        private SceneLoader _sceneLoader;
        private bool _isTransitioning;
        
        public event Action<float> SceneLoadingUpdated
        {
            add => _sceneLoader.SceneLoadingUpdated += value;
            remove => _sceneLoader.SceneLoadingUpdated -= value;
        }
        
        [Inject]
        private void Construct(ILoadingScreen loadingScreen, SceneLoader sceneLoader)
        {
            _loadingScreen = loadingScreen;
            _sceneLoader = sceneLoader;
        }

        public async Task LoadSceneAsync(SceneType sceneType)
        {
            if (_isTransitioning) 
                return;
            _isTransitioning = true;
            
            _cts = new CancellationTokenSource();

            try
            {
                await _loadingScreen.ShowAsync();
                await _sceneLoader.LoadSceneAsync(sceneType, _cts.Token);
                await _loadingScreen.HideAsync();
            }
            finally
            {
                _isTransitioning = false;
            }
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}