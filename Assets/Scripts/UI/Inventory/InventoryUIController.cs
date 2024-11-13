using UnityEngine;

namespace UI.Inventory
{
    using global::Inventory;
    
    public class InventoryUIController : MonoBehaviour
    {
        public Inventory Inventory
        {
            get => _inventory;
            set => _inventory = value;
        }

        [SerializeField]
        private GameObject inventorySlotPrefab;
        private Inventory _inventory;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Debug.Log("Inventory size is " + Inventory.InventorySize);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetInventory(Inventory inventory)
        {
            _inventory = inventory;
        }
    }
}
