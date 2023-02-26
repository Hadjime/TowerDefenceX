using InternalAssets.Scripts.GamePlay.MonstersSpawner;
using InternalAssets.Scripts.Infrastructure.Services.StaticData;
using InternalAssets.Scripts.Infrastructure.Services.UpdateSystem;
using InternalAssets.Scripts.StaticData;
using UnityEngine;
using Zenject;

namespace InternalAssets.Scripts.Enemies
{
	public class Monster : MonoBehaviour, IDamagable, IUpdatable
	{
		[SerializeField] private int _hp;
		private IMonstersSpawner _monstersSpawner;
		private IStaticDataService _staticDataService;
		private float _reachDistance = 0.3f;
		private float _sqrReachDistance;
		private float _speed = 0.1f;
		private float _doubleSpeed;
		private bool _isMoveTargetNull;
		private Vector3 _endPosition;
		private MonstersStaticData _monstersStaticData;
		private IUpdateManager _updateManager;

		public uint UpdateOrder { get; }
		
		
		[Inject]
		private void Construct(IStaticDataService staticDataService, IUpdateManager updateManager)
		{
			_staticDataService = staticDataService;
			_updateManager = updateManager;
		}

		private void OnEnable() => _updateManager.Add(this);

		private void OnDisable() => _updateManager.Remove(this);

		private void Start()
		{
			_sqrReachDistance = _reachDistance * 2;
			_isMoveTargetNull = _endPosition == Vector3.zero;
			_monstersStaticData = _staticDataService.GetMonsterData(MonsterType.Capsule);
			ResetData();
		}
		
		public void UpdateProcessing()
		{
			if (_isMoveTargetNull)
				return;
		
			var direction = _endPosition - transform.position;
			float directionSqrMagnitude = direction.sqrMagnitude;
			if (directionSqrMagnitude <= _sqrReachDistance) 
			{
				_monstersSpawner.ReturnMonster(this);
				return;
			}
			
			if (directionSqrMagnitude > _doubleSpeed) 
			{
				direction = direction.normalized * _speed;
			}
			transform.Translate (direction);
		}

		public void ApplyDamage(int damage)
		{
			_hp -= damage;

			if (_hp > 0) 
				return;
			
			_hp = 0;
			_monstersSpawner.ReturnMonster(this);
		}
		
		public void ResetData()
		{
			if (ReferenceEquals(_monstersStaticData, null))
				return;
			
			_hp = _monstersStaticData.MaxHP;
			_speed = _monstersStaticData.Speed;
			_doubleSpeed = _speed * 2;
			_reachDistance = _monstersStaticData.ReachDistance;
		}


		public class Pool : MonoMemoryPool<IMonstersSpawner, Vector3, Monster>
		{
			protected override void Reinitialize(IMonstersSpawner monstersSpawner, Vector3 endPosition, Monster monster)
			{
				monster._monstersSpawner = monstersSpawner;
				monster._endPosition = endPosition;
				monster.ResetData();
			}
		}
	}
}
