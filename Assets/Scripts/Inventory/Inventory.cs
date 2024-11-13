using System;
using System.Collections.Generic;
using ScriptableObjects;
using UI;
using UI.Inventory;
using UnityEngine;

namespace Inventory
{
    public struct Item : IEquatable<Item>
    {
        public ItemData ItemData;
        public int Amount;

        public Item(ItemData itemData, int amount)
        {
            ItemData = itemData;
            Amount = amount;
        }

        public bool Equals(Item other)
        {
            return Amount != 0 && (Equals(ItemData, other.ItemData) && Amount == other.Amount);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ItemData, Amount);
        }
    }
    
    public class Inventory : MonoBehaviour
    {
        public int InventorySize => inventorySize;

        [SerializeField]
        private Canvas inventoryCanvas;
        [SerializeField] 
        private int inventorySize = 10;

        private Canvas _instantiatedInventory;
        private List<Item> _items;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _items = new List<Item>();
        }

        // Update is called once per frame
        void Update()
        {
            // TEST INPUT TODO: Remove it
            if (Input.GetKeyDown(KeyCode.I))
            {
                ToggleInventory();
            }
        }

        public bool AddItem(ItemData inItemData)
        {
            Item item = _items.Find(i => i.ItemData == inItemData);
            
            // We already have the item
            if (item.ItemData)
            {
                // if we can, increase the stack size
                if (item.ItemData.CanStack && item.Amount < item.ItemData.MaxStackSize)
                {
                    item.Amount += 1;
                    _items[_items.IndexOf(item)] = item;
                }
                else
                {
                    // Trying to add an item that we no longer can add
                    return false;
                }
            }
            else if (_items.Count < inventorySize) // Item not in inventory
            {
                item.ItemData = inItemData;
                item.Amount = 1;
                _items.Add(item);
                return true;
            }

            return false;
        }

        public void RemoveItem(ItemData inItemData)
        {
            Item item = _items.Find(i => i.ItemData == inItemData);
            RemoveItem(_items.IndexOf(item));
        }

        public void RemoveItem(int index)
        {
            _items.RemoveAt(index);
        }

        public Item GetItem(int id)
        {
            if (_items.Count > id && id >= 0)
            {
                return _items[id];
            }

            return new Item();
        }

        // TODO: This should be called from player controller or something
        public void ToggleInventory()
        {
            if (!_instantiatedInventory)
            {
                _instantiatedInventory = Instantiate(inventoryCanvas);
                InventoryUIController uiController = _instantiatedInventory.GetComponent<InventoryUIController>();
                if (uiController)
                {
                    uiController.Inventory = this;
                }
                return;
            }
            
            _instantiatedInventory.gameObject.SetActive(!_instantiatedInventory.gameObject.activeInHierarchy);
        }
    }
}
