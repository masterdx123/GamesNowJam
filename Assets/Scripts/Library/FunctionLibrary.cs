using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Library
{
    public class FunctionLibrary : MonoBehaviour
    {
        public static WeaponProjectile[] ShootSpread(int numOfShotsNonInclusive, float spreadAngle, GameObject projectile, float angleToTarget, Vector3 position, GameObject owner, float angle, [CanBeNull] Weapon senderWeapon)
        {
            float leftMostPoint = angleToTarget - (spreadAngle/2);
            List<WeaponProjectile> createdWeapon = new List<WeaponProjectile>();

            if (numOfShotsNonInclusive > 1)
            {
                float deltaSpread = spreadAngle/numOfShotsNonInclusive;
                for (int i = 0; i < numOfShotsNonInclusive; i++)
                {
                    var attackGo = Instantiate(projectile, position, Quaternion.Euler(0, 0, leftMostPoint + deltaSpread * i));
                    WeaponProjectile attackProjectileComponent = attackGo.GetComponent<WeaponProjectile>();
                    attackProjectileComponent.Owner = owner;
                    attackProjectileComponent.angle = angle;
                    if(senderWeapon) attackProjectileComponent.senderWeapon = senderWeapon;
                    createdWeapon.Add(attackProjectileComponent);
                }
            }
            else
            {
                var attackGo = Instantiate(projectile, position, Quaternion.Euler(0, 0, angle));
                WeaponProjectile attackProjectileComponent = attackGo.GetComponent<WeaponProjectile>();
                attackProjectileComponent.Owner = owner;
                attackProjectileComponent.angle = angle;
                if(senderWeapon) attackProjectileComponent.senderWeapon = senderWeapon;
                createdWeapon.Add(attackProjectileComponent);
            }

            return createdWeapon.ToArray();
        }
    }
}
