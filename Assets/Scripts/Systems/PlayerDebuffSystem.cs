using System.Collections.Generic;
using CheeseHeist.Core;

namespace CheeseHeist.Systems
{
    public class PlayerDebuffSystem : ITickable, IResettable
    {
        private readonly PlayerData _player;
        private readonly List<Debuff> _active = new();

        public PlayerDebuffSystem(PlayerData player)
        {
            _player = player;
        }

        public void Apply(float speedMultiplier, float minControlMultiplier, float duration)
        {
            _active.Add(new Debuff
            {
                SpeedMultiplier = speedMultiplier,
                MinControlMultiplier = minControlMultiplier,
                Duration = duration,
                Elapsed = 0f
            });
        }

        public void Tick(float deltaTime)
        {
            float speedFactor = 1f;
            float controlFactor = 1f;

            for (int i = _active.Count - 1; i >= 0; i--)
            {
                var debuff = _active[i];
                debuff.Elapsed += deltaTime;

                if (debuff.IsExpired)
                {
                    _active.RemoveAt(i);
                    continue;
                }

                if (debuff.SpeedMultiplier < speedFactor) speedFactor = debuff.SpeedMultiplier;
                if (debuff.CurrentControlMultiplier < controlFactor) controlFactor = debuff.CurrentControlMultiplier;
            }

            _player.SpeedMultiplier = speedFactor;
            _player.ControlMultiplier = controlFactor;
        }

        public void ResetState()
        {
            _active.Clear();
            _player.SpeedMultiplier = 1f;
            _player.ControlMultiplier = 1f;
        }
    }
}