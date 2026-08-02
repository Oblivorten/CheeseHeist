using CheeseHeist.Core;

namespace CheeseHeist.Systems
{
    public class ScoreSystem : IResettable
    {
        private readonly GameSessionData _session;
        private readonly GameEvents _events;

        public ScoreSystem(GameSessionData session, GameEvents events)
        {
            _session = session;
            _events = events;
            events.OnCheeseCollected += HandleCheeseCollected;
        }

        private void HandleCheeseCollected(int points)
        {
            _session.Score += points;
            _events.RaiseScoreChanged(_session.Score);
        }

        public void ResetState()
        {
            _session.Score = 0;
            _events.RaiseScoreChanged(0);
        }
    }
}