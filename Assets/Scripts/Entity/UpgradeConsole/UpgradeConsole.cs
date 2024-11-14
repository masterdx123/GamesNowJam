using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entity.UpgradeConsole
{
    public class UpgradeConsole : MonoBehaviour
    {
        [SerializeField]
        private InputAction openUpgradeConsoleAction;

        [SerializeField]
        private Canvas upgradeManagementCanvas;
        
        private Canvas _instantiatedUpgradeUI;
        
        private GameObject _player;
        
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
            Debug.Log(other.gameObject.name);
            if (!other.CompareTag("Player")) return;
            _player = other.gameObject;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log(other.gameObject.name + "Exit");
            if (!other.CompareTag("Player")) return;
            _player = null;
        }

        public void ToggleUpgradeUI()
        {
            if (!_instantiatedUpgradeUI)
            {
                _instantiatedUpgradeUI = Instantiate(upgradeManagementCanvas);
                
                return;
            }
            
            _instantiatedUpgradeUI.gameObject.SetActive(!_instantiatedUpgradeUI.gameObject.activeInHierarchy);
        }
    }
}
