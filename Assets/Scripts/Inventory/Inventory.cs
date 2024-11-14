using System;
using System.Collections.Generic;
using ScriptableObjects;
using UI;
using UI.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

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
        public delegate void ItemChangedDelegate(int index, Item? item);
        
        public int InventorySize => inventorySize;
        public ItemChangedDelegate AddedItem;
        public ItemChangedDelegate RemovedItem;

        [SerializeField]
        private Canvas inventoryCanvas;
        [SerializeField] 
        private int inventorySize = 10;
        [SerializeField]
        public InputAction inventoryAction;

        private Canvas _instantiatedInventory;
        private List<Item> _items;
        
        private List<WeaponStatUpgradeData> _weaponUpgrades;
        private List<CharacterUpgradeData> _characterUpgrades;
        private GameObject _player;
        private Weapon _weapon;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _items = new List<Item>();
            _player = GameObject.FindGameObjectWithTag("Player");
            _weapon = _player.GetComponentInChildren<Weapon>();
            if (!_weapon) Debug.Log("Weapon not found for inventory!");
        }

        // Update is called once per frame
        void Update()
        {
            if (inventoryAction.triggered)
            {
                ToggleInventory();
            }
        }

        private void OnEnable()
        {
            inventoryAction.Enable();
        }

        private void OnDisable()
        {
            inventoryAction.Disable();
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
                    int index = _items.IndexOf(item);
                    item.Amount += 1;
                    _items[index] = item;
                    AddedItem?.Invoke(index, item);
                    return true;
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
                AddedItem?.Invoke(_items.Count, item);
                _items.Add(item);
                return true;
            }

            return false;
        }

        public void RemoveItem(ItemData inItemData)
        {
            Item item = _items.Find(i => i.ItemData == inItemData);
            if (!item.ItemData) return;
            int index = _items.IndexOf(item);
            _items.RemoveAt(index);
            RemovedItem?.Invoke(index, null);
        }

        public Item GetItem(int id)
        {
            if (_items.Count > id && id >= 0)
            {
                return _items[id];
            }

            return new Item();
        }
        
        public void ToggleInventory()
        {
            if (!_instantiatedInventory)
            {
                _instantiatedInventory = Instantiate(inventoryCanvas);
                InventoryUIController uiController = _instantiatedInventory.GetComponent<InventoryUIController>();
                if (!uiController) return;
                uiController.Inventory = this;
                return;
            }
            
            _instantiatedInventory.gameObject.SetActive(!_instantiatedInventory.gameObject.activeInHierarchy);
        }
    }
}
