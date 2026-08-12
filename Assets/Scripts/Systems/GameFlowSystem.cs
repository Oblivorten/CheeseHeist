using CheeseHeist.Core;

namespace CheeseHeist.Systems
{
    public class GameFlowSystem
    {
        private readonly GameSessionData _session;
        private readonly PlayerData _player;
        private readonly GameEvents _events;
        private readonly Loop _loop;
        private readonly ITimeController _timeController;

        public GameFlowSystem(GameSessionData session, PlayerData player, GameEvents events, Loop loop, ITimeController timeController)
        {
            _session = session;
            _player = player;
            _events = events;
            _loop = loop;
            _timeController = timeController;

            _session.State = GameState.MainMenu;
            _timeController.SetTimeScale(0f);

            events.OnGameOver += HandleDeath;
            events.OnCatCaught += HandleDeath;
            events.OnIdleTimeout += HandleDeath;
        }

        public void StartGame()
        {
            if (_session.State != GameState.MainMenu) return;

            _session.State = GameState.Playing;
            _timeController.SetTimeScale(1f);
            _events.RaiseGameStateChanged(GameState.Playing);
        }

        private void HandleDeath()
        {
            if (_session.State != GameState.Playing) return;

            _session.State = GameState.GameOver;
            _timeController.SetTimeScale(0f);
            _events.RaiseGameStateChanged(GameState.GameOver);

            _session.State = GameState.Results;
            _events.RaiseGameStateChanged(GameState.Results);

            _events.RaiseRunEnded();
        }

        public void TogglePause()
        {
            if (_session.State == GameState.Playing)
            {
                _session.State = GameState.Paused;
                _timeController.SetTimeScale(0f);
                _events.RaiseGameStateChanged(GameState.Paused);
            }
            else if (_session.State == GameState.Paused)
            {
                _session.State = GameState.Playing;
                _timeController.SetTimeScale(1f);
                _events.RaiseGameStateChanged(GameState.Playing);
            }
        }

        public void RequestRestart()
        {
            if (_session.State != GameState.Results) return;

            _loop.ResetAll(); 

            _session.State = GameState.Playing;
            _timeController.SetTimeScale(1f);
            _events.RaiseGameStateChanged(GameState.Playing);
        }
    }
}