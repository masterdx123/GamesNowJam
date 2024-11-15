using System;
using System.Collections.Generic;
using Enums;
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
        public delegate void ItemAddedDelegate(int index, Item? item);
        public delegate void ItemRemovedDelegate(List<Item> items);
        
        public int InventorySize => inventorySize;
        public ItemAddedDelegate AddedItem;
        public ItemRemovedDelegate RemovedItem;

        [SerializeField]
        private Canvas inventoryCanvas;
        [SerializeField] 
        private int inventorySize = 10;
        [SerializeField]
        public InputAction inventoryAction;

        private Canvas _instantiatedInventory;
        private List<Item> _items;
        private PlayerController _playerController;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _items = new List<Item>();
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
            RemovedItem?.Invoke(_items);
        }

        public Item GetItem(int id)
        {
            if (_items.Count > id && id >= 0)
            {
                return _items[id];
            }

            return new Item();
        }

        public Item? CheckAndGetIfItemExistsInInventory(ItemData inItemData)
        {
            Item item = _items.Find(i => i.ItemData == inItemData);
            return item.ItemData ? item : null;
        }
        
        public void ToggleInventory()
        {
            if (!_instantiatedInventory)
            {
                _instantiatedInventory = Instantiate(inventoryCanvas);
                InventoryUIController uiController = _instantiatedInventory.GetComponent<InventoryUIController>();
                if (!uiController) return;
                uiController.Inventory = this;
                UpdatePlayerStatus(true);
                return;
            }
            
            bool newActiveState = !_instantiatedInventory.gameObject.activeInHierarchy;
            if (!UpdatePlayerStatus(newActiveState)) return;
            _instantiatedInventory.gameObject.SetActive(newActiveState);
        }

        private bool UpdatePlayerStatus(bool visible)
        {
            if (_playerController)
            {
                if (visible && _playerController.currentPlayerGameState != PlayerStates.InGame)
                {
                    _instantiatedInventory.gameObject.SetActive(false);
                    return false;
                }
                
                _playerController.currentPlayerGameState = visible ? PlayerStates.InInventory : PlayerStates.InGame;
            }

            return true;
        }
    }
}
