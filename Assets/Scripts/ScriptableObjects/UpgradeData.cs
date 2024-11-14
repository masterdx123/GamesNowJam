using System;
using UnityEngine;

namespace ScriptableObjects
{
    [Serializable]
    public abstract class UpgradeData : ItemData
    {
        public int id;
        
        public abstract void ExecuteUpgrade(WeaponProjectile target);
        public abstract void ExecuteUpgrade(Weapon target);
        public abstract void ExecuteUpgrade(PlayerController target);
    }
}
