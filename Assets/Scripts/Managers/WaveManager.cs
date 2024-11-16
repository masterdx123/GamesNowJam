using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class WaveManager : MonoBehaviour
    {
        [Header("Wave Settings")]
        [SerializeField]
        private GameObject[] enemiesToSpawn;
        [SerializeField, Tooltip("Time between waves in seconds")]
        private double firstWaveSpawnTime = 10.0f;
        [SerializeField, Tooltip("Time between waves in seconds")]
        private double timeBetweenWaves = 90.0f;
        [SerializeField, Tooltip("How close the time needs to be to show the warning about the next wave in seconds")]
        private double showWarningTime = 8.0f;
        [SerializeField] 
        private string firstWaveWarningText = "First wave is about to start!";
        [SerializeField] 
        private string nextWaveWarningText = "Next wave is about to start!";
        [SerializeField]
        private int startingNumberOfEnemies = 2;
        [SerializeField]
        private int enemyIncrementMultiplier = 1;
        [SerializeField, Space(15)]
        private int startingCredits = 5;
        [SerializeField]
        private int creditIncrement = 2;
        [SerializeField]
        private int currentCredits = 0;

        [Header("Text References")]
        [SerializeField]
        private TextMeshProUGUI _timerText;
        [SerializeField]
        private TextMeshProUGUI _waveInfoText;
        [SerializeField]
        private TextMeshProUGUI _waveCounterText;
        
        private int _currentWaveCounter = 1;
        private double _currentTimeBetweenWaves = 10.0f;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _waveInfoText.text = "";
            _currentWaveCounter = 0;
            _currentTimeBetweenWaves = firstWaveSpawnTime;
            currentCredits = startingCredits;

            UpdateTimerText();
            UpdateWaveText();
        }

        // Update is called once per frame
        void Update()
        {
            if (_currentTimeBetweenWaves > .0f)
            {
                _currentTimeBetweenWaves -= Time.deltaTime;
                UpdateTimerText();
                UpdateWaveInfoText();
            }

            if (_currentTimeBetweenWaves <= 0.0f)
            {
                SpawnEnemies();
            }
        }

        private void UpdateTimerText()
        {
            _timerText.text = TimeSpan.FromSeconds(_currentTimeBetweenWaves).ToString(@"mm\:ss");
        }

        private void UpdateWaveText()
        {
            _waveCounterText.text = _currentWaveCounter == 0 ? "" : "Wave " + _currentWaveCounter;
        }

        private void UpdateWaveInfoText()
        {
            _waveInfoText.enabled = _currentTimeBetweenWaves <= showWarningTime;
            _waveInfoText.text = _currentWaveCounter == 0 ? firstWaveWarningText : nextWaveWarningText;
        }

        private void SpawnEnemies()
        {
            _currentTimeBetweenWaves = timeBetweenWaves;
            int usableCredits = startingCredits + ((creditIncrement-1) * _currentWaveCounter);
            _currentWaveCounter += 1;
            UpdateWaveText();
            
            Debug.Log("Spawning enemies");

            while (currentCredits > 0)
            {
                GameObject possbileEnemy = enemiesToSpawn[Random.Range(0, enemiesToSpawn.Length)];
                EnemyBehaviour enemyBehaviour = possbileEnemy.GetComponent<EnemyBehaviour>();
                
                if (enemyBehaviour.CreditValue <= currentCredits) {
                    currentCredits -= enemyBehaviour.CreditValue;
                    GameObject spawnedEnemy = Instantiate(possbileEnemy);
                    spawnedEnemy.transform.position = GetRandomPosOffScreen();
                }
            }
        }

        private int CalculateNumberOfEnemiesToSpawn()
        {
            return startingNumberOfEnemies + (_currentWaveCounter - 1) * enemyIncrementMultiplier;
        }
        
        private Vector3 GetRandomPosOffScreen() {

            float x = Random.Range(-0.1f, 0.1f);
            float y = Random.Range(-0.1f, 0.1f);
            x += Mathf.Sign(x);
            y += Mathf.Sign(y);
            Vector3 randomPoint = new(x, y);

            randomPoint.z = 10f;
            Camera mainCamera = Camera.main;
            if (!mainCamera) return new Vector3(0f, 0f, 0f);
            
            Vector3 worldPoint = mainCamera.ViewportToWorldPoint(randomPoint);

            return worldPoint;
        }
    }
}
