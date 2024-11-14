using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class UpgradeSlot : MonoBehaviour, IDropHandler
    {
        [SerializeField]
        private Image iconRenderer;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameObject obj = eventData.pointerDrag;
            InventorySlotController inventorySlotController = obj.GetComponent<InventorySlotController>();
            if (!inventorySlotController || !inventorySlotController.ItemData)
            {
                return;
            }
            iconRenderer.sprite = inventorySlotController.ItemData.Icon;
            Debug.Log(obj.name);
        }
    }
}
