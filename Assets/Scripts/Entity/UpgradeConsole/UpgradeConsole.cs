using System;
using System.Collections.Generic;
using Enums;
using ScriptableObjects;
using UI.Upgrades;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entity.UpgradeConsole
{
    [Serializable]
    public struct Upgrade : IEquatable<Upgrade>
    {
        public UpgradeData upgradeData;
        public bool isUnlocked;

        public Upgrade(UpgradeData inUpgradeData, bool inIsUnlocked)
        {
            upgradeData = inUpgradeData;
            isUnlocked = inIsUnlocked;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(upgradeData, isUnlocked);
        }

        public bool Equals(Upgrade other)
        {
            return Equals(upgradeData, other.upgradeData) && isUnlocked == other.isUnlocked;
        }

        public override bool Equals(object obj)
        {
            return obj is Upgrade other && Equals(other);
        }
    }
    public class UpgradeConsole : MonoBehaviour
    {
        public Upgrade[] WeaponUpgrades => weaponUpgrades;
        public Upgrade[] CharacterUpgrades => characterUpgrades;
        
        [SerializeField]
        private InputAction openUpgradeConsoleAction;

        [SerializeField]
        private Canvas upgradeManagementCanvas;
        
        [SerializeField]
        private Upgrade[] weaponUpgrades;
        [SerializeField]
        private Upgrade[] characterUpgrades;
        
        private Canvas _instantiatedUpgradeUI;
        private UpgradeManagementUIController _uiController;
        private GameObject _player;
        private PlayerController _playerController;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (openUpgradeConsoleAction.triggered && _player)
            {
                ToggleUpgradeUI();
            }
        }

        private void OnEnable()
        {
            openUpgradeConsoleAction.Enable();
        }

        private void OnDisable()
        {
            openUpgradeConsoleAction.Disable();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _player = other.gameObject;
            _playerController = _player.GetComponent<PlayerController>();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _player = null;
            if (_instantiatedUpgradeUI) SetUpgradeUIVisible(false);
        }

        public void ToggleUpgradeUI()
        {
            if (!_instantiatedUpgradeUI)
            {
                _instantiatedUpgradeUI = Instantiate(upgradeManagementCanvas);
                _uiController = _instantiatedUpgradeUI.GetComponent<UpgradeManagementUIController>();
                if (!_uiController) return;
                _uiController.UpgradeConsole = this;
                _uiController.UpdateUpgrades();
                UpdatePlayerStatus(true);
                return;
            }
            
            SetUpgradeUIVisible(!_instantiatedUpgradeUI.gameObject.activeInHierarchy);
        }

        private void SetUpgradeUIVisible(bool visible)
        {
            if (!UpdatePlayerStatus(visible)) return;
            
            _instantiatedUpgradeUI.gameObject.SetActive(visible);
            if (visible && _uiController)
            {
                _uiController.UpdateUpgrades();
            }
        }

        private bool UpdatePlayerStatus(bool visible)
        {
            if (_playerController)
            {
                if (visible && _playerController.currentPlayerGameState != PlayerStates.InGame)
                {
                    _instantiatedUpgradeUI.gameObject.SetActive(false);
                    return false;
                }
                
                _playerController.currentPlayerGameState = visible ? PlayerStates.InUpgradeConsole : PlayerStates.InGame;
            }

            return true;
        }

        public Upgrade GetUpgrade(int id, UpgradeTypes upgradeType)
        {
            Upgrade[] upgradeArray = upgradeType == UpgradeTypes.Weapon ? weaponUpgrades : characterUpgrades;
            if (upgradeArray.Length > id && id >= 0)
            {
                return upgradeArray[id];
            }

            return new Upgrade();
        }
    }
}