using System;
using System.Collections.Generic;
using InternalAssets.Scripts.GamePlay.Weapons;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace InternalAssets.Scripts.GamePlay.Towers
{
    public class GroundTower : MonoBehaviour
    {
        [SerializeField] private Transform placeForWeapon;

        
        public void SetWeaponInPlace(Weapon weapon)
        {
            weapon.transform.SetParent(placeForWeapon,false);
        }

        public class Factory : PlaceholderFactory<GroundTower> { }

        [Serializable]
        public class Settings
        {
            public List<GroundTower> GroundTowersPrefab;
        }
    }
    
    public class GroundTowerFactory : IFactory<GroundTower>
    {
        private DiContainer _container;
        private GroundTower.Settings _settings;

        public GroundTowerFactory(DiContainer container, GroundTower.Settings settings)
        {
            _container = container;
            _settings = settings;
        }
		
        public GroundTower Create()
        {
            int maxIndex = _settings.GroundTowersPrefab.Count - 1;
            int randomIndex = Random.Range(0, maxIndex);
            return _container.InstantiatePrefabForComponent<GroundTower>(_settings.GroundTowersPrefab[randomIndex]);
        }
    }
}