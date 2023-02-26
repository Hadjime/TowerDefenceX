using Zenject;

namespace InternalAssets.Scripts.Infrastructure.Services.UpdateSystem.Injecting
{
    public class UpdateProviderInstaller : MonoInstaller<UpdateProviderInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<UpdateManagerProvider>().AsSingle().NonLazy();
        }
    }
}