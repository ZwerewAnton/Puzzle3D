using UI.Mediators;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private SettingsMediator settingsMediator;
        
        public override void InstallBindings()
        {
            BindSettingsMediator();
        }

        private void BindSettingsMediator()
        {
            Container.Bind<SettingsMediator>().FromInstance(settingsMediator).AsSingle().NonLazy();
        }
    }
}