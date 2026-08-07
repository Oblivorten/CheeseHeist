using CheeseHeist.Core;

namespace CheeseHeist.Systems
{
    public class SkidSystem : ITickable, IResettable
    {
        private readonly PlayerData _player;
        private readonly float _speedMultiplier;
        private readonly float _minControlMultiplier;
        private readonly float _duration;

        private float _timer;

        public SkidSystem(PlayerData player, GameEvents events, float speedMultiplier, float minControlMultiplier, float duration)
        {
            _player = player;
            _speedMultiplier = speedMultiplier;
            _minControlMultiplier = minControlMultiplier;
            _duration = duration;

            events.OnTrailHit += HandleTrailHit;
        }

        private void HandleTrailHit()
        {
            _timer = _duration;
            _player.SpeedMultiplier = _speedMultiplier;
        }

        public void Tick(float deltaTime)
        {
            if (_timer <= 0f)
            {
                _player.ControlMultiplier = 1f;
                return;
            }

            _timer -= deltaTime;

            if (_timer <= 0f)
            {
                _timer = 0f;
                _player.SpeedMultiplier = 1f;
                _player.ControlMultiplier = 1f;
                return;
            }

            float recovered = 1f - (_timer / _duration);
            _player.ControlMultiplier = _minControlMultiplier + (1f - _minControlMultiplier) * recovered;
        }

        public void ResetState()
        {
            _timer = 0f;
            _player.SpeedMultiplier = 1f;
            _player.ControlMultiplier = 1f;
        }
    }
}