using System;
using UnityEngine;

namespace ScriptableObjects
{
    [Serializable]
    public abstract class UpgradeData : ScriptableObject
    {
        public int id;
        public string UpgradeName => upgradeName;
        public string Description => description;
        public Sprite Icon => icon;

        [SerializeField]
        private string upgradeName;
        [SerializeField]
        private string description;
        
        [SerializeField]
        private Sprite icon;
        
        public abstract void ExecuteUpgrade(WeaponProjectile target);
        public abstract void ExecuteUpgrade(Weapon target);
        public abstract void ExecuteUpgrade(PlayerController target);
    }
}
