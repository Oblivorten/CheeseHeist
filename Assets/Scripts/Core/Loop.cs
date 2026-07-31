using System.Collections.Generic;

namespace CheeseHeist.Core
{
    public class Loop
    {
        private readonly List<ITickable> _systems = new();

        public void AddSystem(ITickable system)
        {
            _systems.Add(system);
        }

        public void Tick(float dt)
        {
            foreach (var system in _systems)
            {
                system.Tick(dt);
            }
        }
    }
}