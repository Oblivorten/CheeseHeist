using System;

namespace CheeseHeist.Core
{
    public class GameEvents
    {
        public event Action OnPlayerCollision;
        public event Action<int> OnLivesChanged;
        public event Action OnGameOver;
        public event Action OnCatCaught;

        public void RaisePlayerCollision() => OnPlayerCollision?.Invoke();
        public void RaiseLivesChanged(int lives) => OnLivesChanged?.Invoke(lives);
        public void RaiseGameOver() => OnGameOver?.Invoke();
        public void RaiseCatCaught() => OnCatCaught?.Invoke();
    }
}