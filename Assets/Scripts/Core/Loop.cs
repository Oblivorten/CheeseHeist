using System.Collections.Generic;

namespace CheeseHeist.Core
{
    public class Loop
    {
        private readonly List<ITickable> _tickables = new();
        private readonly List<IResettable> _resettables = new();

        public void AddSystem(ITickable system)
        {
            _tickables.Add(system);
            if (system is IResettable resettable)
            {
                _resettables.Add(resettable);
            }
        }

        public void AddResettable(IResettable resettable)
        {
            _resettables.Add(resettable);
        }

        public void Tick(float deltaTime)
        {
            foreach (var system in _tickables)
            {
                system.Tick(deltaTime);
            }
        }

        public void ResetAll()
        {
            foreach (var resettable in _resettables)
            {
                resettable.ResetState();
            }
        }
    }
}