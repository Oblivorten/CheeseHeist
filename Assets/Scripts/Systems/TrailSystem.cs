using System.Collections.Generic;
using CheeseHeist.Core;

namespace CheeseHeist.Systems
{
    public class TrailSystem : ITickable
    {
        private readonly PlayerData _player;
        private readonly TrailSegmentData[] _segments;
        private readonly float _spawnDistance;
        private readonly float _lifetime;
        private readonly float _groundHeight;

        private Vector3Data _lastSpawnPosition;
        private int _writeIndex;
        private bool _hasSpawnedOnce;
        private int _spawnCounter;

        public IReadOnlyList<TrailSegmentData> Segments => _segments;
        public int SpawnCounter => _spawnCounter;

        public TrailSystem(PlayerData player, float spawnDistance, float lifetime, int capacity, float groundHeight)
        {
            _player = player;
            _spawnDistance = spawnDistance;
            _lifetime = lifetime;
            _groundHeight = groundHeight;

            _segments = new TrailSegmentData[capacity];
            for (int i = 0; i < capacity; i++)
            {
                _segments[i] = new TrailSegmentData { Id = i, IsActive = false };
            }
        }

        public void Tick(float deltaTime)
        {
            for (int i = 0; i < _segments.Length; i++)
            {
                if (!_segments[i].IsActive) continue;

                _segments[i].Age += deltaTime;
                if (_segments[i].Age >= _lifetime)
                {
                    _segments[i].IsActive = false;
                }
            }

            var groundedPosition = new Vector3Data(_player.Position.X, _groundHeight, _player.Position.Z);

            if (!_hasSpawnedOnce)
            {
                SpawnAt(groundedPosition);
                _hasSpawnedOnce = true;
                return;
            }

            if (Distance(_lastSpawnPosition, groundedPosition) >= _spawnDistance)
            {
                SpawnAt(groundedPosition);
            }
        }

        private void SpawnAt(Vector3Data position)
        {
            var segment = _segments[_writeIndex];
            segment.Position = position;
            segment.Age = 0f;
            segment.IsActive = true;
            segment.SpawnSequence = _spawnCounter++;

            _lastSpawnPosition = position;
            _writeIndex = (_writeIndex + 1) % _segments.Length;
        }

        private static float Distance(Vector3Data a, Vector3Data b)
        {
            float dx = a.X - b.X;
            float dz = a.Z - b.Z;
            return System.MathF.Sqrt(dx * dx + dz * dz);
        }
    }
}