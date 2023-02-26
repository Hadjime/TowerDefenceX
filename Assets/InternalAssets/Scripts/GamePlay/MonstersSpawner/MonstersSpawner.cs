using System;
using System.Collections.Generic;
using InternalAssets.Scripts.Enemies;
using UnityEngine;
using Zenject;

namespace InternalAssets.Scripts.GamePlay.MonstersSpawner
{
	public class MonstersSpawner : IMonstersSpawner, ITickable
	{
		private Settings _settings;
		private Monster.Pool _monsterPool;
		private List<Monster> _activeMonsters = new List<Monster>();
		private int _monstersCounter;
		private float _lastSpawn = -1;
		
		
		public MonstersSpawner(Monster.Pool monsterPool, Settings settings)
		{
			_settings = settings;
			_monsterPool = monsterPool;
		}
	
		public void Tick()
		{
			if (!(Time.time > _lastSpawn + _settings.spawnInterval)) 
				return;
			
			Monster monster = _monsterPool.Spawn(this, _settings.EndPositionForMonsters);
			monster.transform.position = _settings.StartPositionForMonsters;
			monster.gameObject.name = $"Monster_{_monstersCounter:00}";
			_monstersCounter++;
			
			_activeMonsters.Add(monster);
			
			_lastSpawn = Time.time;
		}

		public IEnumerable<Monster> GetActiveMonsters() => _activeMonsters;

		public void ReturnMonster(Monster monster)
		{
			_monsterPool.Despawn(monster);
			_activeMonsters.Remove(monster);
		}

		[Serializable]
		public class Settings
		{
			public float spawnInterval = 3;
			public Vector3 StartPositionForMonsters;
			public Vector3 EndPositionForMonsters;
		}
	}
}
