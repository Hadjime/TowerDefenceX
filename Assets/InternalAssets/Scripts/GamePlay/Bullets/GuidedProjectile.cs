using System;
using InternalAssets.Scripts.Enemies;
using UnityEngine;
using Zenject;

namespace InternalAssets.Scripts.GamePlay.Bullets
{
	public class GuidedProjectile : MonoBehaviour 
	{
		private Pool _pool;
		private Settings _settings;
		private GameObject _target;

		
		[Inject]
		private void Construct( Pool pool, Settings settings)
		{
			_pool = pool;
			_settings = settings;
		}
		private void Update () 
		{
			if (_target == null) {
				_pool.Despawn(this);
				return;
			}

			var translation = _target.transform.position - transform.position;
			if (translation.magnitude > _settings.Speed) {
				translation = translation.normalized * _settings.Speed;
			}
			transform.Translate (translation);
		}

		private void OnTriggerEnter(Collider other) 
		{
			if(!other.gameObject.TryGetComponent(out IDamagable monster))
				return;

			monster.ApplyDamage(_settings.Damage);
			_pool.Despawn(this);
		}
		
		
		[Serializable]
		public class Settings
		{
			public GuidedProjectile Prefab;
			public float Speed = 0.2f;
			public int Damage = 10;
		}
		
		public class Pool : MonoMemoryPool<GameObject, Vector3, GuidedProjectile>
		{
			protected override void Reinitialize(GameObject target, Vector3 position, GuidedProjectile guidedProjectile)
			{
				guidedProjectile._target = target;
				guidedProjectile.transform.position = position;
			}
		}
	}
}
