using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField]
        private Slider playerHealthBarSlider;
        [SerializeField]
        private Slider playerOxygenBarSlider;
        [SerializeField]
        private Image gameOverImage;

        private PlayerController _playerController;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if (!_playerController) return;
            _playerController.OnHealthChanged += OnHealthChanged;
            _playerController.OnOxygenChanged += OnOxygenChanged;
            _playerController.OnPlayerDeath += OnPlayerDeath;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnDestroy()
        {
            if (!_playerController) return;
            _playerController.OnHealthChanged -= OnHealthChanged;
            _playerController.OnOxygenChanged -= OnOxygenChanged;
            _playerController.OnPlayerDeath -= OnPlayerDeath;
        }

        private void OnHealthChanged(float health, float maxHealth)
        {
            playerHealthBarSlider.value = health / maxHealth;
        }

        private void OnOxygenChanged(float oxygen, float maxOxygen)
        {
            playerOxygenBarSlider.value = oxygen / maxOxygen;
        }

        private void OnPlayerDeath()
        {
            Debug.Log("Player Death");
            gameOverImage.gameObject.SetActive(true);
        }

        public void RestartLevel()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
