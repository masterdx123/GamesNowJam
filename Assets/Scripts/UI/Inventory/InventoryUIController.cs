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

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            UpdateInventory();
        }

        // Update is called once per frame
        void Update()
        {
        
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
                        slotController.UpdateItem(_inventory.GetItem(i));
                    }
                }
            }
        }
    }
}
