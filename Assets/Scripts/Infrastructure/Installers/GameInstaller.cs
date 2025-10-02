using Cameras;
using Zenject;

namespace Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindCameraMovement();
        }

        private void BindCameraMovement()
        {
            Container.Bind<ICameraMovement>().To<MouseCameraMovement>().AsSingle();
        }
    }
}