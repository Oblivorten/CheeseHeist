using CheeseHeist.Core;

namespace CheeseHeist.Systems
{
    public class HighScoreSystem
    {
        private readonly GameSessionData _session;
        private readonly GameEvents _events;
        private readonly IHighScoreStore _store;

        public HighScoreSystem(GameSessionData session, GameEvents events, IHighScoreStore store)
        {
            _session = session;
            _events = events;
            _store = store;

            session.HighScore = _store.Load();
            events.OnRunEnded += HandleRunEnded; 
        }

        private void HandleRunEnded()
        {
            if (_session.Score > _session.HighScore)
            {
                _session.HighScore = _session.Score;
                _store.Save(_session.HighScore);
                _events.RaiseNewHighScore(_session.HighScore);
            }
        }
    }
}