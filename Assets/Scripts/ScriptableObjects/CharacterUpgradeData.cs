using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "CharacterUpgrade", menuName = "Scriptable Objects/CharacterUpgrade")]
    public class CharacterUpgradeData : UpgradeData
    {
        public override void AddUpgrade()
        {
            Debug.Log("Character upgrade");
        }

        public override void AddUpgrade(Weapon weapon)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveUpgrade()
        {
            Debug.Log("Character upgrade Removed");
        }

        public override void RemoveUpgrade(Weapon weapon)
        {
            throw new System.NotImplementedException();
        }

        public override void ExecuteUpgrade()
        {
            Debug.Log("Character upgrade Executed");
        }
    }
}
