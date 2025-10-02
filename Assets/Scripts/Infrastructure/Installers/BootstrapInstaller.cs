using System.ComponentModel;
using Common;
using Configs;
using Infrastructure.SceneManagement;
using Infrastructure.Utils;
using Input;
using Music;
using SaveSystem;
using Settings;
using UI;
using UI.Mediators;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private ApplicationConfigs applicationConfigs;
        [SerializeField] private MusicPlayer musicPlayer;
        [SerializeField] private SfxPlayer sfxPlayer;
        [SerializeField] private LoadingScreenMediator loadingScreenMediator;
        
        public override void InstallBindings()
        {
            BindApplicationConfigs();
            BindSceneLoader();
            BindSceneSwitcher();
            BindSaveLoadService();
            BindSettingsService();
            BindMusicPlayer();
            BindSfxPlayer();
            BindLoadingScreenMediator();
            BindGameState();
            BindInputHandler();
        }

        private void BindApplicationConfigs()
        {
            Container.Bind<ApplicationConfigs>().FromInstance(applicationConfigs).AsSingle().NonLazy();
        }
        
        private void BindSceneLoader()
        {
            Container.Bind<SceneLoader>().AsSingle().WhenInjectedInto<SceneSwitcher>().NonLazy();
        }
        
        private void BindSceneSwitcher()
        {
            Container.BindInterfacesAndSelfTo<SceneSwitcher>().AsSingle();
        }

        private void BindSaveLoadService()
        {
            Container.Bind<SaveLoadService>().AsSingle().NonLazy();
        }

        private void BindSettingsService()
        {
            Container.Bind<SettingsService>().AsSingle().NonLazy();
        }

        private void BindMusicPlayer()
        {
            Container.Bind<MusicPlayer>().FromComponentInNewPrefab(musicPlayer).AsSingle().NonLazy();
        }

        private void BindSfxPlayer()
        {
            Container.Bind<SfxPlayer>().FromComponentInNewPrefab(sfxPlayer).AsSingle().NonLazy();
        }

        private void BindLoadingScreenMediator()
        {
            Container.Bind<LoadingScreenMediator>().FromComponentInNewPrefab(loadingScreenMediator).AsSingle().NonLazy();
        }

        private void BindGameState()
        {
            Container.Bind<GameState>().AsSingle().NonLazy();
        }

        private void BindInputHandler()
        {
            Container.Bind<InputHandler>().AsSingle().NonLazy();
        }

        private void BindDisposableHandler()
        {
            Container.Bind<DisposableHandler>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("DisposableHandler")
                .AsSingle()
                .NonLazy();
        }
    }
}