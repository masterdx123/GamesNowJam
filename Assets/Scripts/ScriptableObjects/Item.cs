using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField]
        private string itemName;
        [SerializeField]
        private string description;
        
        [SerializeField]
        private Sprite artwork;
    }
}
