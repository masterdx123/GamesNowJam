using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

        private bool _isGamePaused;

        void Start()
        {
            _openPauseMenuAction.Enable();
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
            _pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        public void ClosePauseMenu() {
            _pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }

        public void OpenOptions() {
            _pauseMenu.SetActive(false);
            _optionsMenu.SetActive(true);
        }
    }
}