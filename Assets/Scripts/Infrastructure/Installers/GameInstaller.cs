using Cameras;
using Level;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private LevelsRepository levelsRepository;
        
        public override void InstallBindings()
        {
            BindCameraMovement();
            BindApplicationConfigs();
        }

        private void BindCameraMovement()
        {
            Container.Bind<ICameraMovement>().To<MouseCameraMovement>().AsSingle();
        }
        
        private void BindApplicationConfigs()
        {
            Container.Bind<LevelsRepository>().FromInstance(levelsRepository).AsSingle().NonLazy();
        }
    }
}