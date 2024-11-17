using Entity.UpgradeConsole;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Inv = Inventory;
using UIInv = UI.Inventory;

namespace UI.Upgrades
{
    public class UpgradeSlotConsoleController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, ITooltipable
    {
        public int Slot
        {
            set => _slot = value;
        }

        public UpgradeData UpgradeData => _upgradeData;
        public bool IsUnlocked => _isUnlocked;
        
        [SerializeField]
        private Image iconRenderer;
        [SerializeField]
        private GameObject draggableObjectPrefab;
        
        private Upgrade _upgrade;
        private UpgradeData _upgradeData;
        private bool _isUnlocked;
        private MaterialRequirement[] _materialRequirements;
        private int _slot;
        private Inv.Inventory _inventory;
        
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
            iconRenderer.color = new Color(1,1,1,0.25f);
            _upgrade = upgrade.Value;
            _upgradeData = upgrade.Value.upgradeData;
            _isUnlocked = upgrade.Value.isUnlocked;
            _materialRequirements = upgrade.Value.materialRequirements;
            iconRenderer.sprite = _upgradeData.Icon;

            _inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inv.Inventory>();

            if(IsCraftable() && !IsUnlocked) {
                GetComponent<Image>().color = Color.green;
                GetComponent<Button>().onClick.AddListener(UnlockUpgrade);
            } else if(IsUnlocked) {
                GetComponent<Image>().color = Color.white;
                iconRenderer.color = Color.white;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_upgradeData) return;
            if(_isUnlocked) {
                iconRenderer.enabled = false;
                _draggedObject = Instantiate(draggableObjectPrefab, transform.root);
                DraggableItem draggableItem = _draggedObject.GetComponent<DraggableItem>();
                if (draggableItem)
                {
                    draggableItem.SetDraggableData(_upgradeData);
                }
                _draggedObject.transform.SetAsLastSibling();
            }
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

        public void UnlockUpgrade()
        {
            GetComponent<Image>().color = Color.white;
            transform.root.GetComponent<UpgradeManagementUIController>().UpgradeConsole.SetWeponUpgradeUnlock(_upgrade, true);
            for (int i = 0; i < _materialRequirements.Length; i++) 
            {
                _inventory.RemoveAmount(_materialRequirements[i].itemData, _materialRequirements[i].quantity);
            }
            GetComponent<Button>().onClick.RemoveListener(UnlockUpgrade);
            _isUnlocked = true;
            iconRenderer.color = Color.white;
        }

        public string GetTitle()
        {
            return _upgradeData ? _upgradeData.UpgradeName : "";
        }

        public string GetDescription()
        {
            return _upgradeData ? _upgradeData.Description : "";
        }

        public MaterialRequirement[] GetMaterialRequirements()
        {
            UpgradeConsole mainConsole = GameObject.FindGameObjectWithTag("MainConsole").GetComponent<UpgradeConsole>();
            if(_upgradeData) {
                return mainConsole.GetUpgradeMaterialsRequirements(_upgradeData);
            }
            return null;
        }

        public bool CanShow()
        {
            return _upgradeData;
        }

        private bool IsCraftable() 
        {
            if (_materialRequirements == null || _materialRequirements.Length == 0) return false;

            for (int i = 0; i < _materialRequirements.Length; i++) 
            {
                if(!_inventory.CheckAndIfItemExistsInInventoryAndHasAmount(_materialRequirements[i].itemData, _materialRequirements[i].quantity)) return false;
            }
            return true;
        }
    }
}
