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
        private double TimeBetweenWaves = 90.0f;
        
        [Header("Text References")]
        [SerializeField]
        private TextMeshProUGUI _timerText;
        [SerializeField]
        private TextMeshProUGUI _waveInfoText;
        
        private int _currentWaveCounter = 1;
        private double _currentTimeBetweenWaves = 10.0f;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _waveInfoText.text = "";
            _currentWaveCounter = 1;
            _currentTimeBetweenWaves = 10.0f;

            UpdateTimerText();
        }

        // Update is called once per frame
        void Update()
        {
            if (_currentTimeBetweenWaves > .0f)
            {
                _currentTimeBetweenWaves -= Time.deltaTime;
                UpdateTimerText();
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

        private void SpawnEnemies()
        {
            _currentTimeBetweenWaves = TimeBetweenWaves;
            _currentWaveCounter += 1;
            Debug.Log("Spawning enemies");
            
            GameObject spawnedEnemy = Instantiate(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Length)]);
            spawnedEnemy.transform.position = GetRandomPosOffScreen();
        }
        
        private Vector3 GetRandomPosOffScreen() {

            float x = Random.Range(-0.2f, 0.2f);
            float y = Random.Range(-0.2f, 0.2f);
            x += Mathf.Sign(x);
            y += Mathf.Sign(y);
            Vector3 randomPoint = new(x, y);

            randomPoint.z = 10f; // set this to whatever you want the distance of the point from the camera to be. Default for a 2D game would be 10.
            Camera mainCamera = Camera.main;
            if (!mainCamera) return new Vector3(0f, 0f, 0f);
            
            Vector3 worldPoint = mainCamera.ViewportToWorldPoint(randomPoint);

            return worldPoint;
        }
    }
}
