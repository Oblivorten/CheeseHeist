using CheeseHeist.Core;

namespace CheeseHeist.Systems
{
    public class ScoreSystem
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
    }
}