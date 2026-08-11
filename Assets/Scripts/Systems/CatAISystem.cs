using CheeseHeist.Core;

namespace CheeseHeist.Systems
{
    public class CatAISystem : ITickable, IResettable
    {
        private const float DegToRad = System.MathF.PI / 180f;

        private readonly PlayerData _player;
        private readonly CatData _cat;
        private readonly GameSessionData _session;
        private readonly GameEvents _events;
        private readonly Vector3Data _spawnPosition;
        private readonly float _baseSpeed;
        private readonly float _lungeSpeedMultiplier;
        private readonly float _lungeWindowDuration;
        private readonly float _catchRadius;
        private readonly float _velocityTurnRateDegrees;
        private readonly float _facingTurnRateDegrees;

        private float _velocityAngle;
        private float _facingAngle;
        private bool _hasDirection;

        public CatAISystem(PlayerData player, CatData cat, GameSessionData session, GameEvents events,
            Vector3Data spawnPosition, float baseSpeed, float lungeSpeedMultiplier, float lungeWindowDuration,
            float catchRadius, float velocityTurnRateDegrees, float facingTurnRateDegrees)
        {
            _player = player;
            _cat = cat;
            _session = session;
            _events = events;
            _spawnPosition = spawnPosition;
            _baseSpeed = baseSpeed;
            _lungeSpeedMultiplier = lungeSpeedMultiplier;
            _lungeWindowDuration = lungeWindowDuration;
            _catchRadius = catchRadius;
            _velocityTurnRateDegrees = velocityTurnRateDegrees;
            _facingTurnRateDegrees = facingTurnRateDegrees;

            events.OnPlayerCollision += HandleCollision;
        }

        private void HandleCollision()
        {
            if (_cat.IsCaught) return;

            _cat.State = CatState.Lunge;
            _cat.StateTimer = _lungeWindowDuration;
        }

        public void Tick(float deltaTime)
        {
            if (_cat.IsCaught) return;
            if (!_session.HasPlayerMovedOnce) return;

            if (_cat.State == CatState.Lunge)
            {
                _cat.StateTimer -= deltaTime;
                if (_cat.StateTimer <= 0f)
                {
                    _cat.State = CatState.Patrol;
                }
            }

            float lungeMultiplier = _cat.State == CatState.Lunge ? _lungeSpeedMultiplier : 1f;
            float speed = _baseSpeed * lungeMultiplier * _session.DifficultyMultiplier;

            float toPlayerX = _player.Position.X - _cat.Position.X;
            float toPlayerZ = _player.Position.Z - _cat.Position.Z;
            float distToPlayer = System.MathF.Sqrt(toPlayerX * toPlayerX + toPlayerZ * toPlayerZ);

            if (distToPlayer > 0.01f)
            {
                float desiredAngle = System.MathF.Atan2(toPlayerX, toPlayerZ);

                if (!_hasDirection)
                {
                    _velocityAngle = desiredAngle;
                    _facingAngle = desiredAngle;
                    _hasDirection = true;
                }
                else
                {

                    _facingAngle = RotateAngleTowards(_facingAngle, desiredAngle, _facingTurnRateDegrees * lungeMultiplier * DegToRad * deltaTime);
                    _velocityAngle = RotateAngleTowards(_velocityAngle, desiredAngle, _velocityTurnRateDegrees * lungeMultiplier * DegToRad * deltaTime);
                }
            }

            float dirX = System.MathF.Sin(_velocityAngle);
            float dirZ = System.MathF.Cos(_velocityAngle);

            var previous = _cat.Position;
            _cat.Position = new Vector3Data(
                previous.X + dirX * speed * deltaTime, previous.Y, previous.Z + dirZ * speed * deltaTime);

            _cat.Velocity = new Vector3Data(dirX * speed, 0f, dirZ * speed);
            _cat.FacingDirection = new Vector3Data(System.MathF.Sin(_facingAngle), 0f, System.MathF.Cos(_facingAngle));

            float catchDistSq = DistanceSq(_cat.Position, _player.Position);
            if (catchDistSq <= _catchRadius * _catchRadius)
            {
                _cat.IsCaught = true;
                _events.RaiseCatCaught();
            }
        }

        public void ResetState()
        {
            _cat.Position = _spawnPosition;
            _cat.Velocity = new Vector3Data(0f, 0f, 0f);
            _cat.FacingDirection = new Vector3Data(0f, 0f, 1f);
            _cat.State = CatState.Patrol;
            _cat.StateTimer = 0f;
            _cat.IsCaught = false;
            _hasDirection = false;
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

        private static float DistanceSq(Vector3Data a, Vector3Data b)
        {
            float dx = a.X - b.X;
            float dz = a.Z - b.Z;
            return dx * dx + dz * dz;
        }
    }
}