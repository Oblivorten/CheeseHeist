using CheeseHeist.Core;

namespace CheeseHeist.Systems
{
    public class IdleTimeoutSystem : ITickable, IResettable
    {
        private readonly PlayerData _player;
        private readonly GameEvents _events;
        private readonly float _idleThreshold;
        private readonly float _movementEpsilon;

        private float _idleTimer;

        public IdleTimeoutSystem(PlayerData player, GameEvents events, float idleThreshold, float movementEpsilon)
        {
            _player = player;
            _events = events;
            _idleThreshold = idleThreshold;
            _movementEpsilon = movementEpsilon;
        }

        public void Tick(float deltaTime)
        {
            float speedSq = _player.Velocity.X * _player.Velocity.X + _player.Velocity.Z * _player.Velocity.Z;

            if (speedSq > _movementEpsilon * _movementEpsilon)
            {
                _idleTimer = 0f;
                return;
            }

            _idleTimer += deltaTime;
            if (_idleTimer >= _idleThreshold)
            {
                _events.RaiseIdleTimeout();
            }
        }

        public void ResetState()
        {
            _idleTimer = 0f;
        }
    }
}