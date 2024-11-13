using System;
using UnityEngine;

namespace ScriptableObjects
{
    [Serializable]
    public abstract class UpgradeData : ItemData
    {
        public abstract void AddUpgrade(); // Should receive Character as parameter
        public abstract void AddUpgrade(Weapon weapon); // Should receive Character as parameter
        public abstract void RemoveUpgrade();
        public abstract void RemoveUpgrade(Weapon weapon); // Should receive Character as parameter
        
        public abstract void ExecuteUpgrade();
    }
}
