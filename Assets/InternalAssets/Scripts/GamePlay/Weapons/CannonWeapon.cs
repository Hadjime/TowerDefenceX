using UnityEngine;

namespace InternalAssets.Scripts.GamePlay.Weapons
{
	public class CannonWeapon : Weapon 
	{
		[SerializeField] private float spedRotation = 10f;
		private float _launchSpeed;
		private float _gravity;

		protected override void OnEnable()
		{
			base.OnEnable();
			
			_gravity = Mathf.Abs(Physics.gravity.y);
			float x = range;
			float y = -transform.position.y;
			_launchSpeed = Mathf.Sqrt(_gravity * (y + Mathf.Sqrt(x * x + y * y)));
		}

		public override void UpdateProcessing()
		{
			if (_lastShotTime + shootInterval > Time.time)
				return;
			
			if (shootPoint == null)
				return;

			if (TrySearchNearestEnemy(out GameObject targetMonster)) 
				target = targetMonster;

			if (target == null)
				return;
			
			Vector3 direction = target.transform.position - transform.position;
			if (direction.sqrMagnitude > sqrRange)
			{
				target = null;
				return;
			}
			
			RotationToTarget(direction);
			_cannonBulletsPool.Spawn(shootPoint.position, GetLaunchVelocity());
			
			_lastShotTime = Time.time;
		}

		private Vector3 GetLaunchVelocity()
		{
			Vector3 launchPoint = shootPoint.position;
			Vector3 targetPoint = target.transform.position;
			targetPoint.y = 0f;

			Vector2 dir;
			dir.x = targetPoint.x - launchPoint.x;
			dir.y = targetPoint.z - launchPoint.z;
			float x = dir.magnitude;
			float y = -launchPoint.y;
			dir /= x;
			
			float s = _launchSpeed;
			float s2 = s * s;

			float r = s2 * s2 - _gravity * (_gravity * x * x + 2f * y * s2);
			float tanTheta = (s2 + Mathf.Sqrt(r)) / (_gravity * x);
			float cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
			float sinTheta = cosTheta * tanTheta;

			DrawGizmoLine(launchPoint, s, cosTheta, sinTheta, _gravity, dir);

			Vector3 launchVelocity = new Vector3(s * cosTheta * dir.x, s * sinTheta, s * cosTheta * dir.y);
			return launchVelocity;
		}


		private void DrawGizmoLine(Vector3 launchPoint, float s, float cosTheta, float sinTheta, float gravity, Vector2 dir)
		{
			float pointCount = 50;
			Vector3 prev = launchPoint;
			for (int i = 1; i <= pointCount; i++)
			{
				float t = i / 10f;
				float dx = s * cosTheta * t;
				float dy = s * sinTheta * t - 0.5f * gravity * t * t;
				var next = launchPoint + new Vector3(dir.x * dx, dy, dir.y * dx);
				Debug.DrawLine(prev, next, Color.green);
				prev = next;
			}
		}

		private void RotationToTarget(Vector3 direction)
		{
			Quaternion toRotation = Quaternion.LookRotation(direction);
			toRotation.x = 0;
			transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, spedRotation * Time.deltaTime);
		}
	}
}
