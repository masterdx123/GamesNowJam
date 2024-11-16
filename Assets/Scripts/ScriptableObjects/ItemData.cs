using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item/Item")]
    public class ItemData : ScriptableObject
    {
        public string ItemName => itemName;
        public string Description => description;
        public Sprite Icon => icon;
        public bool CanStack => canStack;
        public int MaxStackSize => maxStackSize;

        [SerializeField]
        private string itemName;
        [SerializeField]
        private string description;
        
        [SerializeField]
        private Sprite icon;
        
        [SerializeField]
        private bool canStack;
        [SerializeField]
        private int maxStackSize;
        
    }
}
