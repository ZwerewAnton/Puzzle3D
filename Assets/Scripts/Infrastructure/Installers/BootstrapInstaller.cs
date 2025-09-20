using Configs;
using Music;
using SaveSystem;
using Settings;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private ApplicationConfigs applicationConfigs;
        [SerializeField] private MusicPlayer musicPlayer;
        [SerializeField] private SfxPlayer sfxPlayer;
        
        public override void InstallBindings()
        {
            BindSaveLoadService();
            BindSettingsService();
            BindApplicationConfigs();
            BindMusicPlayer();
            BindSfxPlayer();
        }

        private void BindSaveLoadService()
        {
            Container.Bind<SaveLoadService>().AsSingle().NonLazy();
        }

        private void BindSettingsService()
        {
            Container.Bind<SettingsService>().AsSingle().NonLazy();
        }

        private void BindApplicationConfigs()
        {
            Container.Bind<ApplicationConfigs>().FromInstance(applicationConfigs).AsSingle().NonLazy();
        }

        private void BindMusicPlayer()
        {
            Container.Bind<MusicPlayer>().FromComponentInNewPrefab(musicPlayer).AsSingle().NonLazy();
        }

        private void BindSfxPlayer()
        {
            Container.Bind<SfxPlayer>().FromComponentInNewPrefab(sfxPlayer).AsSingle().NonLazy();
        }
    }
}