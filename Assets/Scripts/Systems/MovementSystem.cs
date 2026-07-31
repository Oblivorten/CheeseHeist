using CheeseHeist.Core;

namespace CheeseHeist.Systems
{
    public class MovementSystem : ITickable
    {
        private readonly PlayerData _player;
        private readonly IMoveInputProvider _input;

        public MovementSystem(PlayerData player, IMoveInputProvider input)
        {
            _player = player;
            _input = input;
        }

        public void Tick(float deltaTime)
        {
            float x = _input.Horizontal;
            float z = _input.Vertical;

            float magnitudeSquared = x * x + z * z;

            if (magnitudeSquared > 1f)
            {
                float magnitude = System.MathF.Sqrt(magnitudeSquared);

                x /= magnitude;
                z /= magnitude;
            }

            _player.Velocity = new Vector3Data(
                x * _player.MoveSpeed,
                0f,
                z * _player.MoveSpeed
            );
        }
    }
}