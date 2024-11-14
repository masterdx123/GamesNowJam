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

    [CreateAssetMenu(fileName = "WeaponStatUpgrade", menuName = "Scriptable Objects/Upgrades/Weapon Stat Upgrade")]
    public class WeaponStatUpgradeData : UpgradeData
    {
        [SerializeField]
        public StatValue[] statsToUpgrade;

        public override void ExecuteUpgrade(WeaponProjectile target)
        {
            Debug.Log("Executing Weapon Upgrade!");
        }
        
        public override void ExecuteUpgrade(Weapon target)
        {
            throw new NotImplementedException();
        }

        public override void ExecuteUpgrade(PlayerController target)
        {
            throw new NotImplementedException();
        }
    }
}
