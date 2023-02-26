using System.Collections.Generic;
using InternalAssets.Scripts.Data;
using UnityEngine;


namespace InternalAssets.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
    public class LevelStaticData: ScriptableObject
    {
        public int NumberLevel;
        public List<TowerSpawnerData> TowerSpawners;
    }
}