using Inventory;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class InventorySlotController : MonoBehaviour
    {
        public int Slot
        {
            set => _slot = value;
        }
        
        [SerializeField]
        private TextMeshProUGUI itemNameText;
        [SerializeField]
        private Image iconRenderer;
        [SerializeField]
        private Sprite emptySprite;
        
        private ItemData _item;
        private int _slot;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void UpdateItem(int slot, Item? item)
        {
            if (slot != _slot) return;
            if (!item.HasValue || !item.Value.ItemData)
            {
                _item = null;
                iconRenderer.sprite = emptySprite;
                itemNameText.text = "";
                return;
            }
            
            _item = item.Value.ItemData;
            iconRenderer.sprite = _item.Icon;
            itemNameText.text = _item.CanStack ? item.Value.Amount.ToString() : "";
        }
    }
}
