using System.Collections.Generic;
using Inventory;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Entity.UpgradeConsole;

namespace UI.Inventory
{
    public class InventorySlotController : MonoBehaviour, ITooltipable
    {
        public int Slot
        {
            set => _slot = value;
        }

        public ItemData ItemData => _item;
        
        [SerializeField]
        private TextMeshProUGUI itemNameText;
        [SerializeField]
        private Image iconRenderer;
        [SerializeField]
        private GameObject draggableObjectPrefab;
        
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

        public void OnItemRemoved(List<Item> items)
        {
            if (items.Count <= _slot)
            {
                UpdateItem(_slot, null);
            }
            else
            {
                UpdateItem(_slot, items[_slot]);
            }
        }

        public void UpdateItem(int slot, Item? item)
        {
            if (slot != _slot) return;
            if (!item.HasValue || !item.Value.ItemData)
            {
                _item = null;
                iconRenderer.enabled = false;
                itemNameText.text = "";
                return;
            }
            
            iconRenderer.enabled = true;
            _item = item.Value.ItemData;
            iconRenderer.sprite = _item.Icon;
            itemNameText.text = _item.CanStack ? item.Value.Amount.ToString() : "";
        }

        public string GetTitle()
        {
            return _item ? _item.ItemName : "";
        }

        public string GetDescription()
        {
            return _item ? _item.Description : "";
        }

        public MaterialRequirement[] GetMaterialRequirements() {
            return null;
        }

        public bool CanShow()
        {
            return _item;
        }
    }
}
