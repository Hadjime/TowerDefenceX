using System;
using System.Collections.Generic;
using InternalAssets.Scripts.GamePlay.Bullets;
using InternalAssets.Scripts.GamePlay.MonstersSpawner;
using InternalAssets.Scripts.Infrastructure.Services.UpdateSystem;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace InternalAssets.Scripts.GamePlay.Weapons
{
	public class Weapon : MonoBehaviour, IUpdatable
	{
		[SerializeField] protected Transform shootPoint;
		[SerializeField] protected float shootInterval = 0.5f;
		[SerializeField] protected float range = 4f;


		protected IMonstersSpawner _monstersSpawner;
		protected CannonProjectile.Pool _cannonBulletsPool;
		protected GuidedProjectile.Pool _guidedProjectile;
		protected GameObject target;
		protected float _lastShotTime = -0.5f;
		protected float sqrRange;
		private IUpdateManager _updateManager;
		
		
		public uint UpdateOrder { get; }


		[Inject]
		private void Construct(
			IMonstersSpawner monstersSpawner,
			CannonProjectile.Pool cannonBulletsPool,
			GuidedProjectile.Pool guidedProjectile,
			IUpdateManager updateManager)
		{
			_monstersSpawner = monstersSpawner;
			_cannonBulletsPool = cannonBulletsPool;
			_guidedProjectile = guidedProjectile;
			_updateManager = updateManager;
		}

		protected virtual void OnEnable()
		{
			sqrRange = range * range;
			_updateManager.Add(this);
		}

		private void OnDisable()
		{
			_updateManager.Remove(this);
		}

		public virtual void UpdateProcessing() { }

		protected bool TrySearchNearestEnemy(out GameObject targetMonster)
		{
			if (target != null)
			{
				targetMonster = null;
				return false;
			}
			
			foreach (var monster in _monstersSpawner.GetActiveMonsters())
			{
				Vector3 direction = monster.transform.position - transform.position;
				if (direction.sqrMagnitude > sqrRange)
					continue;

				targetMonster = monster.gameObject;
				return true;
			}

			targetMonster = null;
			return false;
		}

		
		public class Factory : PlaceholderFactory<Weapon> { }

		[Serializable]
		public class Settings
		{
			public List<Weapon> WeaponsPrefab;
		}
	}

	public class WeaponFactory : IFactory<Weapon>
	{
		private DiContainer _container;
		private Weapon.Settings _settings;

		public WeaponFactory(DiContainer container, Weapon.Settings settings)
		{
			_container = container;
			_settings = settings;
		}
		
		public Weapon Create()
		{
			int maxIndex = _settings.WeaponsPrefab.Count - 1;
			int randomIndex = Random.Range(0, maxIndex);
			return _container.InstantiatePrefabForComponent<Weapon>(_settings.WeaponsPrefab[randomIndex]);
		}
	}
}


