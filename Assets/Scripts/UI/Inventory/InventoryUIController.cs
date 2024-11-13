using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Inventory
{
    using global::Inventory;
    
    public class InventoryUIController : MonoBehaviour
    {
        public Inventory Inventory
        {
            set => _inventory = value;
        }

        [SerializeField]
        private GameObject inventorySlotPrefab;
        [SerializeField]
        private GameObject inventorySlotContainer;
        
        private Inventory _inventory;
        private List<InventorySlotController> _inventorySlotControllers;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _inventorySlotControllers = new List<InventorySlotController>();
            UpdateInventory();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _inventorySlotControllers.Count; i++)
            {
                if (_inventorySlotControllers[i])
                {
                    _inventory.AddedItem -= _inventorySlotControllers[i].UpdateItem;
                }
            }
        }

        void UpdateInventory()
        {
            if (_inventory && inventorySlotPrefab)
            {
                for (int i = 0; i < _inventory.InventorySize; i++)
                {
                    GameObject newElement = Instantiate(inventorySlotPrefab, inventorySlotContainer.transform);
                    InventorySlotController slotController = newElement.GetComponent<InventorySlotController>();
                    if (slotController)
                    {
                        _inventorySlotControllers.Add(slotController);
                        slotController.Slot = i;
                        slotController.UpdateItem(i, _inventory.GetItem(i));
                        _inventory.AddedItem += slotController.UpdateItem;
                    }
                }
            }
        }
    }
}
