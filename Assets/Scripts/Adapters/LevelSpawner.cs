using System.Collections.Generic;
using UnityEngine;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    public class LevelSpawner : MonoBehaviour, IResettable
    {
        [SerializeField] private Transform _spawnContainer;

        private LevelSpawnConfig _config;
        private CheeseConfig _cheeseConfig;
        private GameEvents _events;
        private Transform _playerTransform;

        private readonly List<Vector3> _obstaclePositions = new();
        private readonly List<GameObject> _spawnedObstacles = new();
        private readonly List<(GameObject go, Vector3 pos)> _cheeseEntries = new();

        private float _cheeseSpawnTimer;

        public void Initialize(GameEvents events, LevelSpawnConfig config, CheeseConfig cheeseConfig, Transform playerTransform)
        {
            _events = events;
            _config = config;
            _cheeseConfig = cheeseConfig;
            _playerTransform = playerTransform;
        }

        public void SpawnInitialLevel()
        {
            SpawnObstacles();
            for (int i = 0; i < _config.CheeseCount; i++)
            {
                SpawnOneCheese();
            }
            _cheeseSpawnTimer = 0f;
        }

        public void TickCheeseSpawner(float deltaTime)
        {
            _cheeseEntries.RemoveAll(e => e.go == null); 

            if (_cheeseEntries.Count >= _cheeseConfig.MaxConcurrentCheese) return;

            _cheeseSpawnTimer += deltaTime;
            if (_cheeseSpawnTimer < _cheeseConfig.SpawnInterval) return;

            _cheeseSpawnTimer = 0f;
            SpawnOneCheese();
        }

        public void ResetState() 
        {
            foreach (var obstacle in _spawnedObstacles)
            {
                if (obstacle != null) Destroy(obstacle);
            }
            _spawnedObstacles.Clear();
            _obstaclePositions.Clear();

            foreach (var (go, _) in _cheeseEntries)
            {
                if (go != null) Destroy(go);
            }
            _cheeseEntries.Clear();

            SpawnInitialLevel();
        }

        private void SpawnObstacles()
        {
            if (_config.ObstaclePrefabs == null || _config.ObstaclePrefabs.Length == 0) return;

            for (int i = 0; i < _config.ObstacleCount; i++)
            {
                if (!TryFindPosition(out Vector3 position)) continue;

                var prefab = _config.ObstaclePrefabs[Random.Range(0, _config.ObstaclePrefabs.Length)];
                var rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                var instance = Instantiate(prefab, position, rotation, _spawnContainer);

                _spawnedObstacles.Add(instance);
                _obstaclePositions.Add(position);
            }
        }

        private void SpawnOneCheese()
        {
            if (_config.CheesePrefab == null) return;
            if (!TryFindPosition(out Vector3 position)) return;

            var instance = Instantiate(_config.CheesePrefab, position, Quaternion.identity, _spawnContainer);
            _cheeseEntries.Add((instance, position));

            if (instance.TryGetComponent(out CheesePickup pickup))
            {
                pickup.Initialize(_events, _cheeseConfig.PointsPerCheese);
            }
        }

        private bool TryFindPosition(out Vector3 position)
        {
            Vector3 playerPos = _playerTransform != null ? _playerTransform.position : Vector3.zero;

            for (int attempt = 0; attempt < _config.MaxPlacementAttempts; attempt++)
            {
                float x = Random.Range(-_config.ArenaHalfExtents.x, _config.ArenaHalfExtents.x);
                float z = Random.Range(-_config.ArenaHalfExtents.y, _config.ArenaHalfExtents.y);
                var candidate = new Vector3(x, 0f, z);

                if (Vector3.Distance(candidate, playerPos) < _config.SafeZoneRadius) continue;
                if (IsTooClose(candidate)) continue;

                position = candidate;
                return true;
            }

            position = Vector3.zero;
            return false;
        }

        private bool IsTooClose(Vector3 candidate)
        {
            foreach (var pos in _obstaclePositions)
            {
                if (Vector3.Distance(candidate, pos) < _config.MinSpacing) return true;
            }
            foreach (var (_, pos) in _cheeseEntries)
            {
                if (Vector3.Distance(candidate, pos) < _config.MinSpacing) return true;
            }
            return false;
        }
    }
}