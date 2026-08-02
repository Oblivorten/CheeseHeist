using System.Collections.Generic;
using UnityEngine;

namespace CheeseHeist.Adapters
{
    public class LevelSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _spawnContainer;

        private readonly List<Vector3> _occupiedPositions = new();

        public List<CheesePickup> SpawnLevel(LevelSpawnConfig config, Vector3 playerSpawnPoint)
        {
            _occupiedPositions.Clear();
            _occupiedPositions.Add(playerSpawnPoint);

            SpawnObstacles(config, playerSpawnPoint);
            return SpawnCheese(config, playerSpawnPoint);
        }

        private void SpawnObstacles(LevelSpawnConfig config, Vector3 playerSpawnPoint)
        {
            if (config.ObstaclePrefabs == null || config.ObstaclePrefabs.Length == 0) return;

            for (int i = 0; i < config.ObstacleCount; i++)
            {
                if (!TryFindPosition(config, playerSpawnPoint, out Vector3 position)) continue;

                var prefab = config.ObstaclePrefabs[Random.Range(0, config.ObstaclePrefabs.Length)];
                var rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                Instantiate(prefab, position, rotation, _spawnContainer);

                _occupiedPositions.Add(position);
            }
        }

        private List<CheesePickup> SpawnCheese(LevelSpawnConfig config, Vector3 playerSpawnPoint)
        {
            var spawned = new List<CheesePickup>();
            if (config.CheesePrefab == null) return spawned;

            for (int i = 0; i < config.CheeseCount; i++)
            {
                if (!TryFindPosition(config, playerSpawnPoint, out Vector3 position)) continue;

                var instance = Instantiate(config.CheesePrefab, position, Quaternion.identity, _spawnContainer);
                _occupiedPositions.Add(position);

                if (instance.TryGetComponent(out CheesePickup pickup))
                {
                    spawned.Add(pickup);
                }
            }

            return spawned;
        }

        private bool TryFindPosition(LevelSpawnConfig config, Vector3 playerSpawnPoint, out Vector3 position)
        {
            for (int attempt = 0; attempt < config.MaxPlacementAttempts; attempt++)
            {
                float x = Random.Range(-config.ArenaHalfExtents.x, config.ArenaHalfExtents.x);
                float z = Random.Range(-config.ArenaHalfExtents.y, config.ArenaHalfExtents.y);
                var candidate = new Vector3(x, 0f, z);

                if (Vector3.Distance(candidate, playerSpawnPoint) < config.SafeZoneRadius) continue;
                if (IsTooCloseToOccupied(candidate, config.MinSpacing)) continue;

                position = candidate;
                return true;
            }

            position = Vector3.zero;
            return false;
        }

        private bool IsTooCloseToOccupied(Vector3 candidate, float minSpacing)
        {
            foreach (var occupied in _occupiedPositions)
            {
                if (Vector3.Distance(candidate, occupied) < minSpacing) return true;
            }
            return false;
        }
    }
}