using CheeseHeist.Core;

namespace CheeseHeist.Systems
{
    public class MovementSystem : ITickable
    {
        private readonly PlayerData _player;
        private readonly IMoveInputProvider _input;
        private readonly float _acceleration;
        private readonly float _deceleration;

        public MovementSystem(PlayerData player, IMoveInputProvider input, float acceleration, float deceleration)
        {
            _player = player;
            _input = input;
            _acceleration = acceleration;
            _deceleration = deceleration;
        }

        public void Tick(float deltaTime)
        {
            float x = _input.Horizontal;
            float z = _input.Vertical;

            float magSq = x * x + z * z;
            if (magSq > 1f)
            {
                float mag = System.MathF.Sqrt(magSq);
                x /= mag;
                z /= mag;
            }

            float effectiveSpeed = _player.MoveSpeed * _player.SpeedMultiplier;
            var target = new Vector3Data(x * effectiveSpeed, 0f, z * effectiveSpeed);

            bool speedingUp = SqrMag(target) > SqrMag(_player.Velocity);
            float rate = speedingUp ? _acceleration : _deceleration;

            _player.Velocity = MoveTowards(_player.Velocity, target, rate * deltaTime);
        }

        private static float SqrMag(Vector3Data v) => v.X * v.X + v.Z * v.Z;

        private static Vector3Data MoveTowards(Vector3Data current, Vector3Data target, float maxDelta)
        {
            float dx = target.X - current.X;
            float dz = target.Z - current.Z;
            float dist = System.MathF.Sqrt(dx * dx + dz * dz);

            if (dist <= maxDelta || dist == 0f) return target;

            return new Vector3Data(current.X + dx / dist * maxDelta, 0f, current.Z + dz / dist * maxDelta);
        }
    }
}