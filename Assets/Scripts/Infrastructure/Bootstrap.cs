using System;
using System.Threading;
using System.Threading.Tasks;
using Configs;
using SaveSystem;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class Bootstrap : MonoBehaviour
    {
        private SaveLoadService _saveSystemService;
        private CancellationTokenSource _cts;
        public event Action LoadingCompleted;
        
        [Inject]
        private void Construct(SaveLoadService saveSystemService, ApplicationConfigs configs)
        {
            SetTargetFrameRate(configs.targetFrameRate);
            _saveSystemService = saveSystemService;
        }

        private void Start()
        {
            _ = InitializeServices();
        }
        
        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
        
        private async Task InitializeServices()
        {
            try
            {
                _cts = new CancellationTokenSource();
                await _saveSystemService.LoadProgressDataAsync(_cts.Token);
                LoadingCompleted?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Initialization failed: {ex}");
            }
        }

        private void CompleteInitialization()
        {
            //TODO
        }

        private static void SetTargetFrameRate(int targetFrameRate)
        {
            Application.targetFrameRate = 120;
        }
    }
}