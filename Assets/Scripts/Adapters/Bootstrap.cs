using CheeseHeist.Core;
using CheeseHeist.Systems;

namespace CheeseHeist.Adapters
{
    public class Bootstrap
    {
        public GameContext CreateGame(
            GameEvents events,
            SceneReferences refs,
            InputSource activeInputSource,
            MovementConfig movementConfig,
            TrailConfig trailConfig,
            SkidConfig skidConfig,
            SlowdownConfig slowdownConfig,
            LivesConfig livesConfig,
            CatConfig catConfig,
            DifficultyConfig difficultyConfig,
            IdleConfig idleConfig)
        {
            var loop = new Loop();
            var playerData = new PlayerData { MoveSpeed = movementConfig.BaseSpeed };
            var session = new GameSessionData();
            var highScoreSystem = new HighScoreSystem(session, events, new PlayerPrefsHighScoreStore());
            var cameraData = new CameraData();
            var timeController = new UnityTimeController();

            var playerTransform = refs.PlayerBody.transform;
            var catSpawnWorld = playerTransform.position - playerTransform.forward * catConfig.InitialOffsetDistance;
            var catSpawnPosition = new Vector3Data(catSpawnWorld.x, catConfig.SpawnHeight, catSpawnWorld.z);
            var catData = new CatData { Position = catSpawnPosition };

            var keyboard = new KeyboardInputAdapter(refs.InputActions);
            var inputRouter = new PlayerInputRouter();

            IMoveInputProvider selectedProvider = activeInputSource switch
            {
                InputSource.Keyboard => keyboard,
                InputSource.VirtualJoystick => refs.Joystick,
                InputSource.Gyroscope => refs.Gyroscope,
                _ => keyboard
            };
            inputRouter.SetProvider(selectedProvider);

            var difficultySystem = new DifficultySystem(
                session, events, difficultyConfig.RampDuration, difficultyConfig.MaxMultiplier);

            var movementSystem = new MovementSystem(
                playerData, session, cameraData, inputRouter,
                movementConfig.Acceleration, movementConfig.Deceleration,
                movementConfig.VelocityTurnRateDegrees, movementConfig.FacingTurnRateDegrees);
            refs.PlayerBody.Initialize(playerData);

            if (refs.Camera != null)
            {
                refs.Camera.Initialize(cameraData, playerData);
            }

            float maxReachableSpeed = movementConfig.BaseSpeed * difficultyConfig.MaxMultiplier;
            int trailCapacity = (int)System.MathF.Ceiling(
                trailConfig.Lifetime * maxReachableSpeed / trailConfig.SpawnDistance * trailConfig.CapacitySafetyMargin);

            var trailSystem = new TrailSystem(
                playerData, trailConfig.SpawnDistance, trailConfig.Lifetime, trailCapacity, trailConfig.GroundHeight);
            refs.TrailView.Initialize(trailCapacity);

            var trailCollisionSystem = new TrailCollisionSystem(
                playerData, trailSystem, session, events, skidConfig.CollisionRadius, skidConfig.GraceSegments);

            refs.ObstacleCollision.Initialize(events, session);

            var playerDebuffSystem = new PlayerDebuffSystem(playerData);

            events.OnTrailHit += () => playerDebuffSystem.Apply(skidConfig.SpeedMultiplier, skidConfig.MinControlMultiplier, skidConfig.Duration);
            events.OnObstacleHit += () => playerDebuffSystem.Apply(slowdownConfig.SpeedMultiplier, 1f, slowdownConfig.Duration);

            var livesSystem = new LivesSystem(
                session, events, livesConfig.StartingLives, livesConfig.InvulnerabilityDuration);

            var catAISystem = new CatAISystem(
                playerData, catData, session, events, catSpawnPosition,
                catConfig.BaseSpeed, catConfig.LungeSpeedMultiplier, catConfig.LungeWindowDuration, catConfig.CatchRadius,
                catConfig.VelocityTurnRateDegrees, catConfig.FacingTurnRateDegrees);

            var scoreSystem = new ScoreSystem(session, events);

            var idleTimeoutSystem = new IdleTimeoutSystem(
                playerData, session, events, idleConfig.IdleThreshold, idleConfig.MovementEpsilon);

            loop.AddSystem(difficultySystem);
            loop.AddSystem(trailCollisionSystem);
            loop.AddSystem(playerDebuffSystem);
            loop.AddSystem(livesSystem);
            loop.AddSystem(movementSystem);
            loop.AddSystem(trailSystem);
            loop.AddSystem(idleTimeoutSystem);
            loop.AddSystem(catAISystem);
            loop.AddResettable(scoreSystem);
            loop.AddResettable(refs.PlayerBody);
            loop.AddResettable(refs.LevelSpawner);

            var gameFlowSystem = new GameFlowSystem(session, playerData, events, loop, timeController);

            return new GameContext
            {
                Loop = loop,
                TrailSystem = trailSystem,
                Events = events,
                Session = session,
                Player = playerData,
                Cat = catData,
                GameFlow = gameFlowSystem
            };
        }
    }
}