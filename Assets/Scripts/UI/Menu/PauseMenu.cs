using Entity.UpgradeConsole;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI.Menu
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField]
        private InputAction _openPauseMenuAction;
        [SerializeField]
        private GameObject _pauseMenu;
        [SerializeField]
        private GameObject _optionsMenu;
        [SerializeField]
        private GameObject _crosshairCanvas;

        private global::Inventory.Inventory _inventory;
        private UpgradeConsole _upgradeConsole;
        private bool _isGamePaused;

        void Start()
        {
            _openPauseMenuAction.Enable();
            _inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<global::Inventory.Inventory>();
            _upgradeConsole = GameObject.FindGameObjectWithTag("MainConsole").GetComponent<UpgradeConsole>();
        }

        void Update()
            {
                if (_openPauseMenuAction.triggered && !_isGamePaused)
                {
                    OpenPauseMenu();
                }
                else if (_openPauseMenuAction.triggered && _isGamePaused) {
                    ClosePauseMenu();
                }
            }

        private void OpenPauseMenu() {
            if (_inventory.IsInventoryOpen())
            {
                _inventory.ToggleInventory();
            }

            if (_upgradeConsole.IsConsoleUpgradeOpen())
            {
                _upgradeConsole.ToggleUpgradeUI();
            }

            _isGamePaused = true;
            _pauseMenu.SetActive(true);
            _crosshairCanvas.SetActive(false);            
            Time.timeScale = 0f;
            Cursor.visible = true;
        }

        public void ClosePauseMenu() {
            _pauseMenu.SetActive(false);
            _crosshairCanvas.SetActive(true);        
            _isGamePaused = false;    
            Time.timeScale = 1f;
            Cursor.visible = false;
        }

        public void OpenOptions() {
            _pauseMenu.SetActive(false);
            _optionsMenu.SetActive(true);
        }

        public void ReturnToMainMenu() {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");            
        }
    }
}