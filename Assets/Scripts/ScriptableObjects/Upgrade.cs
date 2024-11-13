using UnityEngine;

namespace ScriptableObjects
{
    public abstract class Upgrade : Item
    {
        public abstract void ExecuteWeaponUpgrade(); // Should receive weapon as parameter
    }
}
