using System;
using System.Collections.Generic;
using Enums;
using ScriptableObjects;
using UI.Tooltip;
using UI.Upgrades;
using UI.MainConsole;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entity.UpgradeConsole
{
    using Inventory;

    [Serializable]
    public struct MaterialRequirement
    {
        public ItemData itemData;
        public int quantity;
    }
    
    [Serializable]
    public struct Upgrade : IEquatable<Upgrade>
    {
        public UpgradeData upgradeData;
        public bool isUnlocked;
        public MaterialRequirement[] materialRequirements;

        public Upgrade(UpgradeData inUpgradeData, bool inIsUnlocked)
        {
            upgradeData = inUpgradeData;
            isUnlocked = inIsUnlocked;
            materialRequirements = new MaterialRequirement[] { };
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

        public void SetUnlockable(bool unlockable)
        {
            isUnlocked = unlockable;
            materialRequirements = new MaterialRequirement[0];
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
        private Canvas weaponUpgradesUIControllerPrefab;
        
        [Header("Oxygen Bubble Energy")]
        [SerializeField]
        private ItemData energyData;
        [SerializeField]
        private float energyIncreasePerItem;
        [SerializeField] 
        private OxygenSystemStats oxygenSystemStats;
        [SerializeField]
        private Transform _gameManagerCanvas;
        [SerializeField] 
        private GameObject _machineOxygenBar;
        [SerializeField] 
        private GameObject _machineArrowPrefab; 
        private GameObject _machineArrow;
        [SerializeField] 
        private float _machineArrowOffset = 1f; 
        
        
        [Header("Upgrades")]
        [SerializeField]
        private Upgrade[] weaponUpgrades;
        [SerializeField]
        private Upgrade[] characterUpgrades;
        
        private Canvas _instantiatedWeaponUpgradesUI;
        private Canvas _instantiatedUpgradeUI;
        private UpgradeManagementUIController _uiController;
        private GameObject _player;
        private PlayerController _playerController;
        private Camera _mainCamera;
        [SerializeField]private OxygenSystem _oxygenSystem;

        [SerializeField] private AudioClip depositClip;
        private AudioSource audioSource;
        private bool _isGamePaused = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            audioSource = this.GetComponent<AudioSource>();
            _oxygenSystem = gameObject.GetComponentInChildren<OxygenSystem>();
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            if (!_instantiatedWeaponUpgradesUI)
            {
                _instantiatedWeaponUpgradesUI = Instantiate(weaponUpgradesUIControllerPrefab);
                WeaponUpgradesUIController controller = _instantiatedWeaponUpgradesUI.gameObject.GetComponentInChildren<WeaponUpgradesUIController>();
                if (!controller) return;
                Weapon playerWeapon = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Weapon>();
                controller.UpdateWeapon(playerWeapon);
                UpdatePlayerStatus(true);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (openUpgradeConsoleAction.triggered && _player)
            {
                ToggleUpgradeUI();
            }

            if (!IsObjectVisible(_mainCamera, gameObject))
            {
                _machineOxygenBar.SetActive(true);
                if(!_machineArrow) {
                    _machineArrow = Instantiate(_machineArrowPrefab, _gameManagerCanvas);
                    _machineArrow.GetComponent<MachineArrow>().UpgradeConsole = gameObject;
                    _machineArrow.GetComponent<MachineArrow>().Offset = _machineArrowOffset;
                    _machineArrow.GetComponent<MachineArrow>().Canvas = _gameManagerCanvas;
                } 
            } else {
                _machineOxygenBar.SetActive(false);
                if(_machineArrow) Destroy(_machineArrow);
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
            
            // Check if player can feed the oxygen energy
            Inventory inventory = _player.GetComponent<Inventory>();
            Item? item = inventory.CheckAndGetIfItemExistsInInventory(energyData);

            if (item.HasValue)
            {
                float amountToFill = oxygenSystemStats.Energy - _oxygenSystem.GetCurrentEnergy();
                int energyNeeded = (int) Math.Ceiling(amountToFill / energyIncreasePerItem);
                int energyInInventory = inventory.GetItemAmount(energyData);
                audioSource.clip = depositClip;
                audioSource.Play();

                if (energyInInventory >= energyNeeded) {
                    inventory.RemoveAmount(energyData, energyNeeded);
                    _oxygenSystem.RefillEnergy(amountToFill);
                } else {
                    inventory.RemoveAmount(energyData, energyInInventory);
                    _oxygenSystem.RefillEnergy(energyInInventory * energyIncreasePerItem);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _player = null;
            if (_instantiatedUpgradeUI) {

                //resets timeScale to normal if player exits the collider (somehow)
                _isGamePaused = false;
                Time.timeScale = 1f;
                SetUpgradeUIVisible(false);
            }
        }

        public void ToggleUpgradeUI()
        {

            //Stops time when entering upgrade ui
            checkPauseTime();

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

        public bool IsConsoleUpgradeOpen()
        {
            if (!_instantiatedUpgradeUI || !_uiController) return false;

            return _uiController.gameObject.activeInHierarchy;
        }

        private void checkPauseTime() {
            if (!_isGamePaused) {
                Time.timeScale = 0f;
                _isGamePaused = true;
            }
            else {
                Time.timeScale = 1f;
                _isGamePaused = false;
            }
        }

        private void SetUpgradeUIVisible(bool visible)
        {
            if (!UpdatePlayerStatus(visible)) return;
            if (!visible) TooltipSystem.Hide();
            
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
                    TooltipSystem.Hide();
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

        public void SetWeponUpgradeUnlock(Upgrade? upgrade, bool isUnlocked)
        {
            for (int i = 0; i < WeaponUpgrades.Length; i++) {
                if(WeaponUpgrades[i].Equals(upgrade)) {
                    weaponUpgrades[i].SetUnlockable(isUnlocked);
                }
            }
        }

        private bool IsObjectVisible(Camera camera, GameObject gameObject)
        {
            Vector3 viewportPoint = camera.WorldToViewportPoint(gameObject.transform.position);

            return viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                viewportPoint.y >= 0 && viewportPoint.y <= 1;
        }

        public MaterialRequirement[] GetUpgradeMaterialsRequirements(UpgradeData inUpgradeData)
        {
            for(int i = 0; i < weaponUpgrades.Length; i++) {
                if(weaponUpgrades[i].upgradeData == inUpgradeData) {
                    return weaponUpgrades[i].materialRequirements;
                }
            }
            return null;
        }
    }
}
