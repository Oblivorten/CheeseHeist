using CheeseHeist.Core;

namespace CheeseHeist.Systems
{
    public class DifficultySystem : ITickable
    {
        private readonly GameSessionData _session;
        private readonly GameEvents _events;
        private readonly float _rampDuration;
        private readonly float _maxMultiplier;

        public DifficultySystem(GameSessionData session, GameEvents events, float rampDuration, float maxMultiplier)
        {
            _session = session;
            _events = events;
            _rampDuration = rampDuration;
            _maxMultiplier = maxMultiplier;

            _session.DifficultyMultiplier = 1f;
        }

        public void Tick(float deltaTime)
        {
            _session.ElapsedTime += deltaTime;

            float t = _session.ElapsedTime / _rampDuration;
            if (t > 1f) t = 1f;
            if (t < 0f) t = 0f;

            float multiplier = 1f + t * (_maxMultiplier - 1f);

            if (multiplier != _session.DifficultyMultiplier)
            {
                _session.DifficultyMultiplier = multiplier;
                _events.RaiseDifficultyChanged(multiplier);
            }
        }
    }
}