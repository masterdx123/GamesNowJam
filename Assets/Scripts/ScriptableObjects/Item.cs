using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
    public class Item : ScriptableObject
    {
        public string ItemName => itemName;
        public string Description => description;
        public Sprite Icon => icon;

        [SerializeField]
        private string itemName;
        [SerializeField]
        private string description;
        
        [SerializeField]
        private Sprite icon;
    }
}
