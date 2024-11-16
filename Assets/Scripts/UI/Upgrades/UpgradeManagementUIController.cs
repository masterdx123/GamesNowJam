using System.Collections.Generic;
using Entity.UpgradeConsole;
using Enums;
using UnityEngine;

namespace UI.Upgrades
{
    public class UpgradeManagementUIController : MonoBehaviour
    {
        public UpgradeConsole UpgradeConsole
        {
            get => _upgradeConsole;
            set => _upgradeConsole = value;
        }

        [SerializeField]
        private GameObject upgradeConsoleSlotPrefab;
        [SerializeField]
        private GameObject upgradeConsoleSlotContainer;
        
        private UpgradeConsole _upgradeConsole;
        private List<UpgradeSlotConsoleController> _upgradeSlotControllers;
        
        private PlayerController _playerController;
        private Weapon _playerWeapon;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        public void UpdateUpgrades()
        {
            if (!_playerController)
            {
                _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                if (_playerController)
                {
                    _playerWeapon = _playerController.gameObject.GetComponentInChildren<Weapon>();
                }
            }
            if (_upgradeSlotControllers == null) _upgradeSlotControllers = new List<UpgradeSlotConsoleController>();
            UpdateWeaponUpgradeList();
        }

        private void UpdateWeaponUpgradeList()
        {
            if (_upgradeConsole && upgradeConsoleSlotPrefab)
            {
                for (int i = 0; i < _upgradeConsole.WeaponUpgrades.Length; i++)
                {
                    UpgradeSlotConsoleController slotController;
                    if (i < _upgradeSlotControllers.Count && _upgradeSlotControllers[i])
                    {
                        slotController = _upgradeSlotControllers[i];
                    }
                    else
                    {
                        GameObject newElement = Instantiate(upgradeConsoleSlotPrefab, upgradeConsoleSlotContainer.transform);
                        slotController = newElement.GetComponent<UpgradeSlotConsoleController>();
                        _upgradeSlotControllers.Add(slotController);
                    }
                    if (slotController)
                    {
                        slotController.Slot = i;
                        slotController.UpdateUpgrade(i, _upgradeConsole.GetUpgrade(i, UpgradeTypes.Weapon));
                        // TODO: Add delegate call
                    }
                }
            }
        }
    }
}
