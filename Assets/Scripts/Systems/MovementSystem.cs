using CheeseHeist.Core;

namespace CheeseHeist.Systems
{
    public class MovementSystem : ITickable, IResettable
    {
        private const float DegToRad = System.MathF.PI / 180f;

        private readonly PlayerData _player;
        private readonly GameSessionData _session;
        private readonly CameraData _camera;
        private readonly IMoveInputProvider _input;
        private readonly float _acceleration;
        private readonly float _deceleration;
        private readonly float _velocityTurnRateDegrees;
        private readonly float _facingTurnRateDegrees;

        private float _velocityAngle;
        private float _facingAngle;
        private float _speed;
        private bool _hasDirection;

        public MovementSystem(PlayerData player, GameSessionData session, CameraData camera, IMoveInputProvider input,
            float acceleration, float deceleration, float velocityTurnRateDegrees, float facingTurnRateDegrees)
        {
            _player = player;
            _session = session;
            _camera = camera;
            _input = input;
            _acceleration = acceleration;
            _deceleration = deceleration;
            _velocityTurnRateDegrees = velocityTurnRateDegrees;
            _facingTurnRateDegrees = facingTurnRateDegrees;
        }

        public void Tick(float deltaTime)
        {
            float x = _input.Horizontal;
            float z = _input.Vertical;

            float magSq = x * x + z * z;
            bool hasInput = magSq > 0.0001f;
            if (magSq > 1f)
            {
                float mag = System.MathF.Sqrt(magSq);
                x /= mag;
                z /= mag;
            }

            float dirX = _camera.Right.X * x + _camera.Forward.X * z;
            float dirZ = _camera.Right.Z * x + _camera.Forward.Z * z;

            float effectiveMaxSpeed = _player.MoveSpeed * _player.SpeedMultiplier * _session.DifficultyMultiplier;

            if (hasInput)
            {
                float desiredAngle = System.MathF.Atan2(dirX, dirZ);

                if (!_hasDirection)
                {
                    _velocityAngle = desiredAngle;
                    _facingAngle = desiredAngle;
                    _hasDirection = true;
                }
                else
                {
                    _facingAngle = RotateAngleTowards(_facingAngle, desiredAngle, _facingTurnRateDegrees * DegToRad * deltaTime);
                    float velocityTurnRate = _velocityTurnRateDegrees * _player.ControlMultiplier * DegToRad * deltaTime;
                    _velocityAngle = RotateAngleTowards(_velocityAngle, desiredAngle, velocityTurnRate);
                }

                float speedRate = (effectiveMaxSpeed > _speed ? _acceleration : _deceleration) * _player.ControlMultiplier;
                _speed = MoveTowardsFloat(_speed, effectiveMaxSpeed, speedRate * deltaTime);
            }
            else
            {
                float speedRate = _deceleration * _player.ControlMultiplier;
                _speed = MoveTowardsFloat(_speed, 0f, speedRate * deltaTime);
            }

            _player.Velocity = new Vector3Data(
                System.MathF.Sin(_velocityAngle) * _speed, 0f, System.MathF.Cos(_velocityAngle) * _speed);

            _player.FacingDirection = new Vector3Data(
                System.MathF.Sin(_facingAngle), 0f, System.MathF.Cos(_facingAngle));
        }

        public void ResetState()
        {
            _speed = 0f;
            _velocityAngle = 0f;
            _facingAngle = 0f;
            _hasDirection = false;
            _player.Velocity = new Vector3Data(0f, 0f, 0f);
            _player.FacingDirection = new Vector3Data(0f, 0f, 1f);
        }

        private static float MoveTowardsFloat(float current, float target, float maxDelta)
        {
            if (System.MathF.Abs(target - current) <= maxDelta) return target;
            return current + System.MathF.Sign(target - current) * maxDelta;
        }

        private static float RotateAngleTowards(float current, float target, float maxDelta)
        {
            float diff = NormalizeAngle(target - current);
            if (diff > maxDelta) diff = maxDelta;
            else if (diff < -maxDelta) diff = -maxDelta;
            return current + diff;
        }

        private static float NormalizeAngle(float angle)
        {
            while (angle > System.MathF.PI) angle -= 2f * System.MathF.PI;
            while (angle < -System.MathF.PI) angle += 2f * System.MathF.PI;
            return angle;
        }
    }
}