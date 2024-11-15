using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "CharacterUpgrade", menuName = "Scriptable Objects/CharacterUpgrade")]
    public class CharacterUpgradeData : UpgradeData
    {
        public override void ExecuteUpgrade(WeaponProjectile target)
        {
            throw new System.NotImplementedException();
        }

        public override void ExecuteUpgrade(PlayerController target)
        {
            Debug.Log("Executing Upgrade");
        }

        public override void AddUpgrade(Weapon target)
        {
            throw new System.NotImplementedException();
        }

        public override void AddUpgrade(PlayerController target)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveUpgrade(Weapon target)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveUpgrade(PlayerController target)
        {
            throw new System.NotImplementedException();
        }

        public override void ExecuteUpgrade(Weapon target)
        {
            throw new System.NotImplementedException();
        }
    }
}
