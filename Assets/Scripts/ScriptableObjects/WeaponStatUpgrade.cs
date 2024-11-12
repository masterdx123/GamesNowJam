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
    public class WeaponStatUpgrade : Upgrade
    {
        [SerializeField]
        public StatValue[] statsToUpgrade;
        
        public override void ExecuteWeaponUpgrade()
        {
            // TODO: Implement functionality
            throw new System.NotImplementedException();
        }
    }
}
