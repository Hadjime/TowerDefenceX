using UnityEngine;

namespace InternalAssets.Scripts.GamePlay.Weapons
{
    public class CrystalWeapon : Weapon
    {
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
            
            _guidedProjectile.Spawn(target, shootPoint.position);
			
            _lastShotTime = Time.time;
        }
    }
}