using _1_LEVEL_REWORK.New.Instances;
using Cameras;
using Gameplay;
using Gameplay.Movement;
using UI.Mediators;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private DetailPrefabSpawner detailPrefabSpawner;
        [SerializeField] private LevelMediator levelMediator;
        [SerializeField] private DetailViewMover detailViewMover;
        [SerializeField] private CameraHandler cameraHandler;
        [SerializeField] private LevelSaverMono levelSaverMono;
        
        public override void InstallBindings()
        {
            BindLevelSaverMono();
            BindCameraHandler();
            BindCameraMovement();
            BindDetailPrefabSpawner();
            BindDetailViewMoverInput();
            BindDetailViewMover();
            BindLevelState();
            BindLevelService();
            BindLevelMediator();
            BindLevelInteractableCoordinator();
        }

        private void BindLevelSaverMono()
        {
            Container.Bind<LevelSaverMono>().FromInstance(levelSaverMono).AsSingle().NonLazy();
        }

        private void BindCameraHandler()
        {
            Container.Bind<CameraHandler>().FromInstance(cameraHandler).AsSingle().NonLazy();
        }

        private void BindCameraMovement()
        {
            Container.Bind<ICameraMovement>().To<MouseCameraMovement>().AsSingle();
        }
        
        private void BindDetailPrefabSpawner()
        {
            Container.Bind<DetailPrefabSpawner>().FromInstance(detailPrefabSpawner).AsSingle().NonLazy();
        }

        private void BindDetailViewMoverInput()
        {
            Container.Bind<IDetailViewMoverInput>().To<DetailViewMoverInput>().AsSingle();
        }
        
        private void BindDetailViewMover()
        {
            Container.Bind<DetailViewMover>().FromInstance(detailViewMover).AsSingle().NonLazy();
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

        private void BindLevelInteractableCoordinator()
        {
            Container.Bind<LevelInteractableCoordinator>().AsSingle().NonLazy();
        }
    }
}