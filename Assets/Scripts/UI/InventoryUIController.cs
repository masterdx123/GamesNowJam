using UnityEngine;

namespace UI
{
    public class InventoryUIController : MonoBehaviour
    {
        public Inventory.Inventory Inventory { get; set; }

        [SerializeField]
        private GameObject inventorySlotPrefab;
        private Inventory.Inventory _inventory;
            
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Debug.Log("Inventory size is " + Inventory.InventorySize);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetInventory(Inventory.Inventory inventory)
        {
            _inventory = inventory;
        }
    }
}
