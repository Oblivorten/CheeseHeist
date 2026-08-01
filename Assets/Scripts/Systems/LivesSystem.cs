using CheeseHeist.Core;

namespace CheeseHeist.Systems
{
    public class LivesSystem : ITickable
    {
        private readonly GameSessionData _session;
        private readonly GameEvents _events;
        private readonly float _invulnerabilityDuration;

        private float _invulnerabilityTimer;

        public LivesSystem(GameSessionData session, GameEvents events, int startingLives, float invulnerabilityDuration)
        {
            _session = session;
            _events = events;
            _invulnerabilityDuration = invulnerabilityDuration;

            _session.Lives = startingLives;
            events.OnPlayerCollision += HandleCollision;
        }

        private void HandleCollision()
        {
            if (_invulnerabilityTimer > 0f) return;
            if (_session.Lives <= 0) return;        

            _session.Lives--;
            _invulnerabilityTimer = _invulnerabilityDuration;
            _events.RaiseLivesChanged(_session.Lives);

            if (_session.Lives <= 0)
            {
                _events.RaiseGameOver();
            }
        }

        public void Tick(float deltaTime)
        {
            if (_invulnerabilityTimer > 0f)
            {
                _invulnerabilityTimer -= deltaTime;
            }
        }
    }
}