using System;
using System.Collections.Generic;
using Enums;
using ScriptableObjects;
using UI;
using UI.Inventory;
using UI.Tooltip;
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
        private bool _isGamePaused = false;        
    
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

        /**
         * This function will try to remove a specific amount of an item from the inventory
         *
         * returns true if able to remove the required amount and false if not.
         */
        public bool RemoveAmount(ItemData inItemData, int amount)
        {
            Item item = _items.Find(i => i.ItemData == inItemData);
            if (!item.ItemData) return false;
            int index = _items.IndexOf(item);

            // If the item doesn't have enough, return false
            if (item.Amount < amount) return false;
            // decrease amount to item
            item.Amount -= amount;

            // if its 0, just remove it since the inventory no longer has any amount of this item
            if (item.Amount == 0)
            {
                _items.RemoveAt(index);
                RemovedItem?.Invoke(_items);
            }
            else // else update the item
            {
                _items[index] = item;
                AddedItem?.Invoke(index, item);
            }
            return true;
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

        public bool CheckAndIfItemExistsInInventoryAndHasAmount(ItemData inItemData, int amount)
        {
            Item item = _items.Find(i => i.ItemData == inItemData);
            return item.ItemData && item.Amount >= amount;
        }

        public int GetItemAmount(ItemData inItemData)
        {
            Item item = _items.Find(i => i.ItemData == inItemData);
            return item.Amount;
        }
        
        public void ToggleInventory()
        {
            //Stops time when entering inventory ui
            checkPauseTime();

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
            if (!newActiveState) TooltipSystem.Hide();
            
            _instantiatedInventory.gameObject.SetActive(newActiveState);
        }
        
        public bool IsInventoryOpen()
        {
            if (!_instantiatedInventory) return false;
            
            InventoryUIController uiController = _instantiatedInventory.GetComponent<InventoryUIController>();
            if (!uiController) return false;

            return _instantiatedInventory.gameObject.activeInHierarchy;
        }

        private bool UpdatePlayerStatus(bool visible)
        {
            if (_playerController)
            {
                if (visible && _playerController.currentPlayerGameState != PlayerStates.InGame)
                {
                    _instantiatedInventory.gameObject.SetActive(false);
                    TooltipSystem.Hide();
                    return false;
                }
                
                _playerController.currentPlayerGameState = visible ? PlayerStates.InInventory : PlayerStates.InGame;
            }

            return true;
        }

        private void checkPauseTime() {
            if (!_isGamePaused) {
                Time.timeScale = 0f;
                _isGamePaused = true;
            }
            else {
                Time.timeScale = 1f;
                _isGamePaused = false;
            }
        }         
    }
}
