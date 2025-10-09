using _1_LEVEL_REWORK.New.Instances;
using Cameras;
using Gameplay;
using Level;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private DetailPrefabSpawner detailPrefabSpawner;
        
        public override void InstallBindings()
        {
            BindCameraMovement();
            BindDetailPrefabSpawner();
            BindLevelState();
            BindLevelService();
        }

        private void BindCameraMovement()
        {
            Container.Bind<ICameraMovement>().To<MouseCameraMovement>().AsSingle();
        }
        
        private void BindDetailPrefabSpawner()
        {
            Container.Bind<DetailPrefabSpawner>().FromInstance(detailPrefabSpawner).AsSingle().NonLazy();
        }
        
        private void BindLevelState()
        {
            Container.Bind<LevelState>().AsSingle().NonLazy();
        }
        
        private void BindLevelService()
        {
            Container.BindInterfacesAndSelfTo<LevelService>().AsSingle();
        }
    }
}