using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Main.Combat
{
    public enum Direction
    {
        North,
        South,
        East,
        West
    }

    [System.Serializable]
    public class Wave
    {
        private WavesController controller;

        public int number;
        public int timeToWave;
        public int mobsAmount;
        public int direction;

        [Header("Debug")]
        [SerializeField, ReadOnly] private float _timer;
        [SerializeField, ReadOnly] private bool _spawnedMobs;


        public void Initialize(WavesController controller)
        {
            this.controller = controller;

            direction = Random.Range(0, 4);
            controller.UpdateDirectionText(((Direction)direction).ToString());
            controller.UpdateWaveNumber(number);
            _timer = timeToWave;
        }

        public void Update()
        {
            _timer -= Time.deltaTime;

            System.TimeSpan time = System.TimeSpan.FromSeconds(_timer);
            string formattedTime = time.ToString("mm':'ss");
            controller.UpdateTimeToWave($"{formattedTime}");

            if (_spawnedMobs && controller.GetEnemiesCount() == 0)
            {
                OnFinish();
            }
            if (_spawnedMobs) return;

            if (_timer <= 0f)
            {
                controller.SpawnWaveEnemies(direction, mobsAmount);
                _spawnedMobs = true;
            }
        }

        public void OnFinish()
        {
            controller.NextWave();
        }
    }

    public class WavesController : MonoBehaviour
    {
        private GameManager _gameManager;
        private MapManager _mapManager;

        public GameObject endingScreen;

        [Header("Components")]
        [SerializeField] private TextMeshProUGUI _waveNumberText; 
        [SerializeField] private TextMeshProUGUI _enemiesLeftText; 
        [SerializeField] private TextMeshProUGUI _timeToNextWaveText; 
        [SerializeField] private TextMeshProUGUI _directionText; 

        [Header("Values")]
        [SerializeField] private Enemy _enemyPrefab; 
        [SerializeField] private List<Wave> _waves; 

        [Header("Debug")]
        private List<Enemy> _enemies = new();
        [SerializeField, ReadOnly] private int _wave; 

        private float halfMapSize;


        private void Start()
        {
            _gameManager = GetComponent<GameManager>();
            _mapManager = _gameManager.mapManager;

            halfMapSize = _mapManager.mapSize / 2;

            UpdateEnemiesCount();

            _waves[_wave].Initialize(this);
        }
        private void Update()
        {
            if (_wave >= _waves.Count) return;

            _waves[_wave].Update();
        }

        public void SpawnWaveEnemies(int direction, int mobsAmount)
        {
            Vector2 position = Vector2.zero;

            for (int i = 0; i < mobsAmount; i++)
            {
                switch (direction)
                {
                    case 0: //N
                        position = new(Random.Range(-halfMapSize, halfMapSize), halfMapSize + Random.Range(-10, 10));
                        break;
                    case 1: //S
                        position = new(Random.Range(-halfMapSize, halfMapSize), -halfMapSize + Random.Range(-10, 10));
                        break;
                    case 2: //E
                        position = new(halfMapSize + Random.Range(-10, 10), Random.Range(-halfMapSize, halfMapSize));
                        break;
                    case 3: //W
                        position = new(-halfMapSize + Random.Range(-10, 10), Random.Range(-halfMapSize, halfMapSize));
                        break;
                }

                SpawnEnemy(position);
            }
        }
        private void SpawnEnemy(Vector2 position)
        {
            int random = Random.Range(0, 4);
            bool focusOnMainBase = random == 0;

            Enemy enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
            enemy.Setup(this, focusOnMainBase);
            _enemies.Add(enemy);

            UpdateEnemiesCount();
        }

        public void NextWave()
        {
            _wave++;
            if (_wave >= _waves.Count)
            {
                endingScreen.SetActive(true);
                return;
            }
            _waves[_wave].Initialize(this);
        }

        public void RemoveEnemy(Enemy enemy)
        {
            _enemies.Remove(enemy);
            UpdateEnemiesCount();
        }

        public void UpdateTimeToWave(string time)
        {
            _timeToNextWaveText.SetText($"Spawns in: {time}");
        }
        public void UpdateDirectionText(string direction)
        {
            _directionText.SetText($"Direction: {direction}");
        }
        public void UpdateEnemiesCount()
        {
            _enemiesLeftText.SetText($"left: {_enemies.Count}");
        }
        public void UpdateWaveNumber(int wave)
        {
            _waveNumberText.SetText($"Wave: {wave}");
        }

        public IDamageable GetClosestEnemy(Vector3 position, float range)
        {
            IDamageable closestEnemy = null;
            float currentDistance = int.MaxValue;

            for (int i = 0; i < _enemies.Count; i++)
            {
                var current = _enemies[i];
                float distance = (current.Position - position).sqrMagnitude;

                if ((closestEnemy == null || distance < currentDistance) && distance < range * range)
                {
                    closestEnemy = current;
                    currentDistance = distance;
                }
            }

            return closestEnemy;
        }

        public int GetEnemiesCount()
        {
            return _enemies.Count;
        }

        public void SpawnEnemyAtPlayer()
        {
            SpawnEnemy(_gameManager.playerController.Position);
        }
    }
}
