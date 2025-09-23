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
        private LoadingScreenMediator _loadingScreenMediator;
        private SceneLoader _sceneLoader;
        private bool _isTransitioning;        
        
        public event Action<float> SceneLoadingUpdated
        {
            add => _sceneLoader.SceneLoadingUpdated += value;
            remove => _sceneLoader.SceneLoadingUpdated -= value;
        }
        
        [Inject]
        private void Construct(LoadingScreenMediator loadingScreenMediator, SceneLoader sceneLoader)
        {
            _loadingScreenMediator = loadingScreenMediator;
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
                await _loadingScreenMediator.ShowLoadingScreenAsync();
                await _sceneLoader.LoadSceneAsync(sceneType, _cts.Token);
                await _loadingScreenMediator.HideLoadingScreenAsync();
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