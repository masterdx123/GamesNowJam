using UnityEngine;

namespace ScriptableObjects
{
    public abstract class UpgradeData : ItemData
    {
        public abstract void ExecuteWeaponUpgrade(); // Should receive weapon as parameter
    }
}
