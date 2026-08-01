using CheeseHeist.Core;

namespace CheeseHeist.Systems
{
    public class SkidSystem : ITickable
    {
        private readonly PlayerData _player;
        private readonly float _speedMultiplier;
        private readonly float _duration;

        private float _timer;

        public bool IsSkidding { get; private set; }

        public SkidSystem(PlayerData player, GameEvents events, float speedMultiplier, float duration)
        {
            _player = player;
            _speedMultiplier = speedMultiplier;
            _duration = duration;

            events.OnTrailCollision += HandleTrailCollision;
        }

        private void HandleTrailCollision()
        {
            IsSkidding = true;
            _timer = _duration;
            _player.SpeedMultiplier = _speedMultiplier;
        }

        public void Tick(float deltaTime)
        {
            if (!IsSkidding) return;

            _timer -= deltaTime;
            if (_timer <= 0f)
            {
                IsSkidding = false;
                _player.SpeedMultiplier = 1f;
            }
        }
    }
}