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
            throw new NotImplementedException();
        }

        public override void ExecuteUpgrade(PlayerController target)
        {
            throw new NotImplementedException();
        }

        public override void AddUpgrade(Weapon target)
        {
            foreach (StatValue statComp in statsToUpgrade)
            {
                ModifyStat(statComp, target, 1);
            }
        }

        public override void AddUpgrade(PlayerController target)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUpgrade(Weapon target)
        {
            foreach (StatValue statComp in statsToUpgrade)
            {
                ModifyStat(statComp, target, -1);
            }
        }

        public override void RemoveUpgrade(PlayerController target)
        {
            throw new NotImplementedException();
        }

        private void ModifyStat(StatValue statComp, Weapon target, int modifier)
        {
            float value = statComp.value * modifier;
            switch (statComp.stat)
            {
                case Stat.Damage:
                    target.DamageModifier += value;
                    break;
                case Stat.DamageFlat:
                    target.DamageFlatModifier += (int)Math.Round(value);
                    break;
                case Stat.Range:
                    target.RangeModifier += value;
                    break;
                case Stat.AttackInterval:
                    target.AttackIntervalModifier += value;
                    break;
                case Stat.BulletVelocity:
                    target.BulletVelocityModifier += value;
                    break;
            }
        }
    }
}
