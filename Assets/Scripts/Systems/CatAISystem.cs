using CheeseHeist.Core;

namespace CheeseHeist.Systems
{
    public class CatAISystem : ITickable, IResettable
    {
        private readonly PlayerData _player;
        private readonly CatData _cat;
        private readonly GameSessionData _session;
        private readonly GameEvents _events;
        private readonly float _patrolDistance;
        private readonly float _lungeDistance;
        private readonly float _lungeWindowDuration;
        private readonly float _followSpeed;

        private Vector3Data _lastPlayerFacing = new Vector3Data(0f, 0f, 1f);
        private bool _initialized;

        public CatAISystem(PlayerData player, CatData cat, GameSessionData session, GameEvents events,
            float patrolDistance, float lungeDistance, float lungeWindowDuration, float followSpeed)
        {
            _player = player;
            _cat = cat;
            _session = session;
            _events = events;
            _patrolDistance = patrolDistance;
            _lungeDistance = lungeDistance;
            _lungeWindowDuration = lungeWindowDuration;
            _followSpeed = followSpeed;

            events.OnPlayerCollision += HandleCollision;
        }

        private void HandleCollision()
        {
            if (_cat.IsCaught) return;

            if (_cat.State == CatState.Lunge)
            {
                _cat.IsCaught = true;
                _events.RaiseCatCaught();
                return;
            }

            _cat.State = CatState.Lunge;
            _cat.StateTimer = _lungeWindowDuration;
        }

        public void Tick(float deltaTime)
        {
            if (_cat.IsCaught) return;

            if (_cat.State == CatState.Lunge)
            {
                _cat.StateTimer -= deltaTime;
                if (_cat.StateTimer <= 0f)
                {
                    _cat.State = CatState.Patrol;
                }
            }

            var playerVelocity = _player.Velocity;
            float velSq = playerVelocity.X * playerVelocity.X + playerVelocity.Z * playerVelocity.Z;
            if (velSq > 0.01f)
            {
                float mag = System.MathF.Sqrt(velSq);
                _lastPlayerFacing = new Vector3Data(playerVelocity.X / mag, 0f, playerVelocity.Z / mag);
            }

            float targetDistance = _cat.State == CatState.Lunge ? _lungeDistance : _patrolDistance;
            var desired = new Vector3Data(
                _player.Position.X - _lastPlayerFacing.X * targetDistance,
                _player.Position.Y,
                _player.Position.Z - _lastPlayerFacing.Z * targetDistance);

            if (!_initialized)
            {
                _cat.Position = desired;
                _initialized = true;
                return;
            }

            float effectiveFollowSpeed = _followSpeed * _session.DifficultyMultiplier;

            var previous = _cat.Position;
            _cat.Position = MoveTowards(_cat.Position, desired, effectiveFollowSpeed * deltaTime);

            _cat.Velocity = new Vector3Data(
                (_cat.Position.X - previous.X) / deltaTime,
                0f,
                (_cat.Position.Z - previous.Z) / deltaTime);
        }

        private static Vector3Data MoveTowards(Vector3Data current, Vector3Data target, float maxDelta)
        {
            float dx = target.X - current.X;
            float dz = target.Z - current.Z;
            float dist = System.MathF.Sqrt(dx * dx + dz * dz);

            if (dist <= maxDelta || dist == 0f) return target;

            return new Vector3Data(current.X + dx / dist * maxDelta, target.Y, current.Z + dz / dist * maxDelta);
        }

        public void ResetState()
        {
            _cat.State = CatState.Patrol;
            _cat.StateTimer = 0f;
            _cat.IsCaught = false;
            _cat.Velocity = new Vector3Data(0f, 0f, 0f);
            _initialized = false;
        }
    }
}