using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [Serializable]
    public struct StatValue
    {
        public Stat stat;
        public float value;
    }

    [CreateAssetMenu(fileName = "WeaponUpgrade", menuName = "Scriptable Objects/Upgrades/Weapon Upgrade")]
    public class WeaponUpgradeData : UpgradeData
    {
        [SerializeField]
        public StatValue[] statsToUpgrade;

        public override void ExecuteUpgrade(WeaponProjectile target)
        {
            Debug.Log("Executing Weapon Upgrade For Projectile!");
        }
        
        public override void ExecuteUpgrade(Weapon target)
        {
            foreach (StatValue statComp in statsToUpgrade)
            {
                switch (statComp.stat)
                {
                    case Stat.Damage:
                        target.DamageModifier += statComp.value;
                        break;
                    case Stat.DamageFlat:
                        target.DamageFlatModifier += (int)Math.Round(statComp.value);
                        break;
                    case Stat.Range:
                        target.RangeModifier += statComp.value;
                        break;
                    case Stat.AttackInterval:
                        target.AttackIntervalModifier += statComp.value;
                        break;
                    case Stat.BulletVelocity:
                        target.BulletVelocityModifier += statComp.value;
                        break;
                }
            }
        }

        public override void ExecuteUpgrade(PlayerController target)
        {
            throw new NotImplementedException();
        }
    }
}
