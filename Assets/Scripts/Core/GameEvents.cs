using System;

namespace CheeseHeist.Core
{
    public class GameEvents
    {
        public event Action OnPlayerCollision;
        public event Action<int> OnLivesChanged;
        public event Action OnGameOver;
        public event Action OnCatCaught;
        public event Action<int> OnCheeseCollected;
        public event Action<int> OnScoreChanged;
        public event Action<float> OnDifficultyChanged;
        public event Action<GameState> OnGameStateChanged;
        public event Action OnRestartRequested;
        public event Action OnIdleTimeout;

        public void RaisePlayerCollision() => OnPlayerCollision?.Invoke();
        public void RaiseLivesChanged(int lives) => OnLivesChanged?.Invoke(lives);
        public void RaiseGameOver() => OnGameOver?.Invoke();
        public void RaiseCatCaught() => OnCatCaught?.Invoke();
        public void RaiseCheeseCollected(int points) => OnCheeseCollected?.Invoke(points);
        public void RaiseScoreChanged(int score) => OnScoreChanged?.Invoke(score);
        public void RaiseDifficultyChanged(float multiplier) => OnDifficultyChanged?.Invoke(multiplier);
        public void RaiseGameStateChanged(GameState state) => OnGameStateChanged?.Invoke(state);
        public void RaiseRestartRequested() => OnRestartRequested?.Invoke();
        public void RaiseIdleTimeout() => OnIdleTimeout?.Invoke();
    }
}