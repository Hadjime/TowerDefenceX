using InternalAssets.Scripts.Enemies;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "MonstersPoolInstaller", menuName = "Installers/MonstersPoolInstaller")]
public class MonstersPoolInstaller : ScriptableObjectInstaller<MonstersPoolInstaller>
{
    private const string TRANSFORM_GROUP_NAME = "MonstersPool";
    [SerializeField] private GameObject monsterPrefab;
    
    
    public override void InstallBindings()
    {
        Container.BindMemoryPool<Monster, Monster.Pool>().WithInitialSize(10)
            .FromComponentInNewPrefab(monsterPrefab).UnderTransformGroup(TRANSFORM_GROUP_NAME);
    }
}