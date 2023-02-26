using InternalAssets.Scripts.Enemies;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace InternalAssets.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "Static Data/Monster")]
    public class MonstersStaticData: ScriptableObject
    {
        public MonsterType MonsterType;
        public float Speed = 0.1f;
        public int MaxHP = 30;
        public float ReachDistance = 0.3f;
        public AssetReferenceGameObject PrefabReference;
    }
}