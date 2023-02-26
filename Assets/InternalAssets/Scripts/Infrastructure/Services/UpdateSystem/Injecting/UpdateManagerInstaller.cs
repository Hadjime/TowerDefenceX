using UnityEngine;
using Zenject;

namespace InternalAssets.Scripts.Infrastructure.Services.UpdateSystem.Injecting
{
    [CreateAssetMenu(fileName = "UpdateManagerInstaller", menuName = "Installers/UpdateManagerInstaller")]
    public class UpdateManagerInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private UpdateManager managerPrefab = default;

        public override void InstallBindings()
        {
            UpdateManager instance = Instantiate(managerPrefab);
            DontDestroyOnLoad(instance.transform);
            Container.Bind<IUpdateManager>().FromInstance(instance).AsSingle().NonLazy();
        }
    }
}