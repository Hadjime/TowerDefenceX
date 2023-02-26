using UnityEngine;
using Zenject;

namespace InternalAssets.Scripts.GamePlay.Towers
{
    [CreateAssetMenu(fileName = "TowersInstaller", menuName = "Installers/TowersInstaller")]
    public class GroundTowersInstaller : ScriptableObjectInstaller<GroundTowersInstaller>
    {
        [SerializeField] private GroundTower.Settings _groundTowersSettings;


        public override void InstallBindings()
        {
            Container.BindInstance(_groundTowersSettings).IfNotBound();
            Container.BindFactory<GroundTower, GroundTower.Factory>().FromFactory<GroundTowerFactory>();
        }
    }
}