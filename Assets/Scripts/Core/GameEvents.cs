using System;

namespace CheeseHeist.Core
{
    public class GameEvents
    {
        public event Action OnTrailCollision;

        public void RaiseTrailCollision() => OnTrailCollision?.Invoke();
    }
}