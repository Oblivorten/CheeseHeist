using CheeseHeist.Core;

namespace CheeseHeist.Systems
{
    public class SlowdownSystem : ITickable, IResettable
    {
        private readonly PlayerData _player;
        private readonly float _speedMultiplier;
        private readonly float _duration;

        private float _timer;

        public SlowdownSystem(PlayerData player, GameEvents events, float speedMultiplier, float duration)
        {
            _player = player;
            _speedMultiplier = speedMultiplier;
            _duration = duration;

            events.OnObstacleHit += HandleObstacleHit;
        }

        private void HandleObstacleHit()
        {
            _timer = _duration;
            _player.SpeedMultiplier = _speedMultiplier;
        }

        public void Tick(float deltaTime)
        {
            if (_timer <= 0f) return;

            _timer -= deltaTime;
            if (_timer <= 0f)
            {
                _player.SpeedMultiplier = 1f;
            }
        }

        public void ResetState()
        {
            _timer = 0f;
            _player.SpeedMultiplier = 1f;
        }
    }
}