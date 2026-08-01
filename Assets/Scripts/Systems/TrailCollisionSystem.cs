using CheeseHeist.Core;

namespace CheeseHeist.Systems
{
    public class TrailCollisionSystem : ITickable
    {
        private readonly PlayerData _player;
        private readonly TrailSystem _trail;
        private readonly GameEvents _events;
        private readonly float _collisionRadius;
        private readonly int _graceSegments;
        private bool _wasColliding;

        public TrailCollisionSystem(PlayerData player, TrailSystem trail, GameEvents events, float collisionRadius, int graceSegments)
        {
            _player = player;
            _trail = trail;
            _events = events;
            _collisionRadius = collisionRadius;
            _graceSegments = graceSegments;
        }

        public void Tick(float deltaTime)
        {
            bool collidingNow = false;
            var segments = _trail.Segments;
            int currentSequence = _trail.SpawnCounter;

            for (int i = 0; i < segments.Count; i++)
            {
                var segment = segments[i];
                if (!segment.IsActive) continue;

                if (currentSequence - segment.SpawnSequence <= _graceSegments) continue;

                if (DistanceXZ(_player.Position, segment.Position) <= _collisionRadius)
                {
                    collidingNow = true;
                    break;
                }
            }

            if (collidingNow && !_wasColliding)
            {
                _events.RaisePlayerCollision();
            }

            _wasColliding = collidingNow;
        }

        private static float DistanceXZ(Vector3Data a, Vector3Data b)
        {
            float dx = a.X - b.X;
            float dz = a.Z - b.Z;
            return System.MathF.Sqrt(dx * dx + dz * dz);
        }
    }
}