using Enums;
using ScriptableObjects;
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

        [SerializeField] 
        private UpgradeTypes slotType;

        private UpgradeSlotConsoleController _equippedSlot;
        
        private PlayerController _playerController;
        private Weapon _weapon;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                _playerController = player.GetComponent<PlayerController>();
                _weapon = player.GetComponentInChildren<Weapon>();
            }
            iconRenderer.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameObject obj = eventData.pointerDrag;
            UpgradeSlotConsoleController upgradeSlotConsoleSlot = obj.GetComponent<UpgradeSlotConsoleController>();
            if (!upgradeSlotConsoleSlot || !upgradeSlotConsoleSlot.UpgradeData || _equippedSlot == upgradeSlotConsoleSlot)
            {
                return;
            }
            
            // Check if it is the correct upgrade
            if (!IsCorrectType(upgradeSlotConsoleSlot.UpgradeData))
            {
                // Call delegate, wrong type
                return;
            }
            
            // Check if already has same upgrade equipped
            if (HasSameUpgrade(upgradeSlotConsoleSlot.UpgradeData))
            {
                // Call delegate, already equipped
                return;
            }

            // If already have an upgrade on the slot...
            if (_equippedSlot)
            {
                // ...Remove it first
                RemoveEquippedSlot();
            }
            
            // Equip upgrade
            // TODO: need to have feedback that upgrade is equipped
            _weapon.AddUpgrade(upgradeSlotConsoleSlot.UpgradeData);
            iconRenderer.sprite = upgradeSlotConsoleSlot.UpgradeData.Icon;
            iconRenderer.enabled = true;
            Debug.Log(obj.name);
        }

        private bool IsCorrectType(UpgradeData upgradeData)
        {
            switch (slotType)
            {
                case UpgradeTypes.Weapon when upgradeData.GetType() == typeof(WeaponUpgradeData):
                    return true;
                case UpgradeTypes.Character when upgradeData.GetType() == typeof(CharacterUpgradeData):
                    return true;
                default:
                    return false;
            }
        }

        private bool HasSameUpgrade(UpgradeData upgradeData)
        {
            return _weapon.HasUpgrade(upgradeData);
        }

        private void RemoveEquippedSlot()
        {
            _weapon.RemoveUpgrade(_equippedSlot.UpgradeData);
            _equippedSlot = null;
        }
    }
}
