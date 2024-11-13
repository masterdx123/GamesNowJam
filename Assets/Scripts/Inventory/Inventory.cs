using System.Collections.Generic;
using ScriptableObjects;
using UI;
using UI.Inventory;
using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        public int InventorySize => inventorySize;

        public List<Item> Items => _items;

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
