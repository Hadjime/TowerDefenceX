using System;
using System.Collections;
using GRV.ToolsModule;
using InternalAssets.Scripts.Enemies;
using InternalAssets.Scripts.Infrastructure.Services.UpdateSystem;
using UnityEngine;
using Zenject;

namespace InternalAssets.Scripts.GamePlay.Bullets
{
	public class CannonProjectile : MonoBehaviour, IUpdatable
	{
		private Pool _pool;
		private Settings _settings;
		private Vector3 _startPosition;
		private Vector3 _targetPosition;
		private Vector3 _launchVelocity;
		private float _time;
		private float _gravity;
		private IUpdateManager _updateManager;


		public uint UpdateOrder { get; }
		
		
		[Inject]
		private void Construct(Pool pool, Settings settings, IUpdateManager updateManager)
		{
			_pool = pool;
			_settings = settings;
			_updateManager = updateManager;
		}

		private void OnEnable()
		{
			_updateManager.Add(this);
			StartCoroutine(DespawnAfterTime());
			
			_time = 0;
			_gravity = Mathf.Abs(Physics.gravity.y);
		}

		private void OnDisable()
		{
			_updateManager.Remove(this);
		}

		public void UpdateProcessing()
		{
			_time += Time.deltaTime;
			Vector3 position =  _startPosition + _launchVelocity * _time;
			position.y -= 0.5f * _gravity * Mathf.Pow(_time, 2);
			transform.localPosition = position;
		}
		
		private void OnTriggerEnter(Collider other) 
		{
			if(!other.gameObject.TryGetComponent(out IDamagable damagableObject))
				return;

			damagableObject.ApplyDamage(_settings.Damage);
			_pool.Despawn(this);
		}

		private IEnumerator DespawnAfterTime()
		{
			yield return Coroutines.GetWait(5);
			_pool.Despawn(this);
		}


		[Serializable]
		public class Settings
		{
			public CannonProjectile Prefab;
			public float Speed = 0.2f;
			public int Damage = 10;
		}
		
		public class Pool : MonoMemoryPool<Vector3, Vector3, CannonProjectile>
		{
			protected override void Reinitialize(Vector3 startPosition, Vector3 launchVelocity, CannonProjectile cannonProjectile)
			{
				cannonProjectile.transform.position = startPosition;
				cannonProjectile._startPosition = startPosition;
				cannonProjectile._launchVelocity = launchVelocity;
			}
		}
	}
}
