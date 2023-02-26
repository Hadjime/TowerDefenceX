using InternalAssets.Scripts.GamePlay.MonstersSpawner;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "MonstersSpawnerInstaller", menuName = "Installers/MonstersSpawnerInstaller")]
public class MonstersSpawnerInstaller : ScriptableObjectInstaller<MonstersSpawnerInstaller>
{
    [SerializeField] private MonstersSpawner.Settings _settings;
    
    
    public override void InstallBindings()
    {
        Container.BindInstance(_settings).IfNotBound();
        Container.BindInterfacesAndSelfTo<MonstersSpawner>().AsSingle();
    }
}