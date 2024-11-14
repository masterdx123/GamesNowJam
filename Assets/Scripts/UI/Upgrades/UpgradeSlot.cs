using UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Upgrades
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
            UpgradeSlotConsoleController upgradeSlotConsoleSlot = obj.GetComponent<UpgradeSlotConsoleController>();
            if (!upgradeSlotConsoleSlot || !upgradeSlotConsoleSlot.UpgradeData)
            {
                return;
            }
            iconRenderer.sprite = upgradeSlotConsoleSlot.UpgradeData.Icon;
            Debug.Log(obj.name);
        }
    }
}
