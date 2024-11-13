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
        
        public override void AddUpgrade()
        {
            // TODO: Implement functionality
            throw new NotImplementedException();
        }

        public override void RemoveUpgrade()
        {
            throw new NotImplementedException();
        }

        public override void AddUpgrade(Weapon weapon)
        {
            Debug.Log("Upgrade Weapon");
        }

        public override void RemoveUpgrade(Weapon weapon)
        {
            Debug.Log("Remove Upgrade");
        }

        public override void ExecuteUpgrade()
        {
            throw new NotImplementedException();
        }
    }
}
