using UnityEngine;
using Zenject;

namespace InternalAssets.Scripts.GamePlay.EntryPointInWorld
{
    [CreateAssetMenu(fileName = "WorldGameInstaller", menuName = "Installers/WorldGameInstaller")]
    public class WorldGameInstaller : ScriptableObjectInstaller<WorldGameInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<WorldGame>().AsSingle();
        }
    }
}