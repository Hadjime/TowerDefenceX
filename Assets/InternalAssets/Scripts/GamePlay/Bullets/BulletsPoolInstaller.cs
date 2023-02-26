using InternalAssets.Scripts.GamePlay.Bullets;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "BulletsPoolInstaller", menuName = "Installers/BulletsPoolInstaller")]
public class BulletsPoolInstaller : ScriptableObjectInstaller<BulletsPoolInstaller>
{
    private const string TRANSFORM_GROUP_NAME = "BulletsPool";
    
    [SerializeField] private CannonProjectile.Settings cannonProjectileSettings;
    [SerializeField] private GuidedProjectile.Settings guidedProjectileSettings;
    
    
    public override void InstallBindings()
    {
        Container.BindInstance(cannonProjectileSettings).IfNotBound();
        Container.BindInstance(guidedProjectileSettings).IfNotBound();
        
        Container.BindMemoryPool<CannonProjectile, CannonProjectile.Pool>().WithInitialSize(10)
            .FromComponentInNewPrefab(cannonProjectileSettings.Prefab).UnderTransformGroup(TRANSFORM_GROUP_NAME);
        
        Container.BindMemoryPool<GuidedProjectile, GuidedProjectile.Pool>().WithInitialSize(10)
            .FromComponentInNewPrefab(guidedProjectileSettings.Prefab).UnderTransformGroup(TRANSFORM_GROUP_NAME);
    }
}