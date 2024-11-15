using Entity.UpgradeConsole;
using ScriptableObjects;
using UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Upgrades
{
    public class UpgradeSlotConsoleController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, ITooltipable
    {
        public int Slot
        {
            set => _slot = value;
        }

        public UpgradeData UpgradeData => _upgradeData;
        
        [SerializeField]
        private Image iconRenderer;
        [SerializeField]
        private GameObject draggableObjectPrefab;
        
        private UpgradeData _upgradeData;
        private int _slot;
        
        private GameObject _draggedObject;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void UpdateUpgrade(int slot, Upgrade? upgrade)
        {
            if (slot != _slot) return;
            if (!upgrade.HasValue || !upgrade.Value.upgradeData)
            {
                _upgradeData = null;
                iconRenderer.enabled = false;
                return;
            }
            
            iconRenderer.enabled = true;
            _upgradeData = upgrade.Value.upgradeData;
            iconRenderer.sprite = _upgradeData.Icon;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_upgradeData) return;
            iconRenderer.enabled = false;
            _draggedObject = Instantiate(draggableObjectPrefab, transform.root);
            DraggableItem draggableItem = _draggedObject.GetComponent<DraggableItem>();
            if (draggableItem)
            {
                draggableItem.SetDraggableData(_upgradeData);
            }
            _draggedObject.transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_upgradeData || !_draggedObject) return;
            _draggedObject.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_upgradeData || !_draggedObject) return;
            iconRenderer.enabled = true;
            Destroy(_draggedObject);
        }

        public string GetTitle()
        {
            return _upgradeData ? _upgradeData.UpgradeName : "";
        }

        public string GetDescription()
        {
            return _upgradeData ? _upgradeData.Description : "";
        }

        public bool CanShow()
        {
            return _upgradeData;
        }
    }
}
