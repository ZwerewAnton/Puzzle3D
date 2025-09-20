using System;
using System.Threading.Tasks;
using Music;
using SaveSystem;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class Bootstrap : MonoBehaviour
    {
        private SaveLoadService _saveSystemService;
        
        [Inject]
        private void Construct(SaveLoadService saveSystemService)
        {
            _saveSystemService = saveSystemService;
        }

        private void Awake()
        {
            _ = InitializeServices(CompleteInitialization);
        }

        private async Task InitializeServices(Action onComplete)
        {
            try
            {
                SetTargetFrameRate();
                await _saveSystemService.LoadProgressDataAsync();
                onComplete?.Invoke();
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

        private void SetTargetFrameRate()
        {
            Application.targetFrameRate = 120;
        }
    }
}