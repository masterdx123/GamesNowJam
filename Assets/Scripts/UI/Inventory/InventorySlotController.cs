using Inventory;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI.Inventory
{
    public class InventorySlotController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI itemNameText;
        
        private ItemData _item;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void UpdateItem(Item item)
        {
            _item = item.ItemData;
            itemNameText.text = _item ? _item.ItemName : "";
        }
    }
}
