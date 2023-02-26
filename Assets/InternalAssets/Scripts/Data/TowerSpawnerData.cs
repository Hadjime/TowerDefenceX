using System;
using UnityEngine;

namespace InternalAssets.Scripts.Data
{
    [Serializable]
    public class TowerSpawnerData
    {
        public string Id;
        public Vector3 Position;


        public TowerSpawnerData(string id, Vector3 position)
        {
            Id = id;
            Position = position;
        }
    }
}