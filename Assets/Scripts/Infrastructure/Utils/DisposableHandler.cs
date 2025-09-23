using Infrastructure.SceneManagement;
using UnityEngine;
using Zenject;

namespace Infrastructure.Utils
{
    public class DisposableHandler : MonoBehaviour
    {
        private SceneSwitcher _sceneSwitcher;
        
        [Inject]
        private void Construct(SceneSwitcher sceneSwitcher)
        {
            _sceneSwitcher = sceneSwitcher;
        }
        
        private void OnDestroy()
        {
            _sceneSwitcher.Dispose();
        }
    }
}