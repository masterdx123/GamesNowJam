using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI.Inventory
{
    public class InventorySlotController : MonoBehaviour
    {
        public Item Item { set => _item = value; }

        [SerializeField]
        private TextMeshProUGUI itemNameText;
        
        private Item _item;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            itemNameText.text = _item ? _item.ItemName : "";
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
