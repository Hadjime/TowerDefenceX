using InternalAssets.Scripts.GamePlay.Towers;
using UnityEngine;
using Zenject;

namespace InternalAssets.Scripts.GamePlay.Weapons
{
    [CreateAssetMenu(fileName = "WeaponsInstaller", menuName = "Installers/WeaponsInstaller")]
    public class WeaponsInstaller : ScriptableObjectInstaller<GroundTowersInstaller>
    {
        [SerializeField] private Weapon.Settings _towersSettings;
    
    
        public override void InstallBindings()
        {
            Container.BindInstance(_towersSettings).IfNotBound();
            Container.BindFactory<Weapon, Weapon.Factory>().FromFactory<WeaponFactory>();
        }
    }
}