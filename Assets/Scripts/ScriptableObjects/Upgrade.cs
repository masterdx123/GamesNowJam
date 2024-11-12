using UnityEngine;

namespace ScriptableObjects
{
    public abstract class Upgrade : ScriptableObject
    {
        [SerializeField]
        private string upgradeName;
        [SerializeField]
        private string description;
        
        [SerializeField]
        private Sprite artwork;

        public abstract void ExecuteWeaponUpgrade(); // Should receive weapon as parameter
    }
}
