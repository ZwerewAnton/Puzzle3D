using _1_LEVEL_REWORK.New.Instances;
using Cameras;
using Gameplay;
using UI.Mediators;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private DetailPrefabSpawner detailPrefabSpawner;
        [SerializeField] private LevelMediator levelMediator;
        
        public override void InstallBindings()
        {
            BindCameraMovement();
            BindDetailPrefabSpawner();
            BindLevelState();
            BindLevelService();
            BindLevelMediator();
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

        private void BindLevelMediator()
        {
            Container.Bind<LevelMediator>().FromInstance(levelMediator).AsSingle().NonLazy();
        }
    }
}