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
            var playerData = new PlayerData { MoveSpeed = 5f };
            var session = new GameSessionData();
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
                refs.Camera.Initialize(cameraData);
            }

            var trailSystem = new TrailSystem(
                playerData, trailConfig.SpawnDistance, trailConfig.Lifetime, trailConfig.Capacity, trailConfig.GroundHeight);
            refs.TrailView.Initialize(trailConfig.Capacity);

            var trailCollisionSystem = new TrailCollisionSystem(
                playerData, trailSystem, events, skidConfig.CollisionRadius, skidConfig.GraceSegments);
            var skidSystem = new SkidSystem(
                playerData, events, skidConfig.SpeedMultiplier, skidConfig.MinControlMultiplier, skidConfig.Duration);

            refs.ObstacleCollision.Initialize(events);
            var slowdownSystem = new SlowdownSystem(playerData, events, slowdownConfig.SpeedMultiplier, slowdownConfig.Duration);

            var livesSystem = new LivesSystem(
                session, events, livesConfig.StartingLives, livesConfig.InvulnerabilityDuration);

            var catAISystem = new CatAISystem(
                playerData, catData, session, events, catSpawnPosition,
                catConfig.BaseSpeed, catConfig.LungeSpeedMultiplier, catConfig.LungeWindowDuration, catConfig.CatchRadius,
                catConfig.VelocityTurnRateDegrees, catConfig.FacingTurnRateDegrees);

            var scoreSystem = new ScoreSystem(session, events);

            var idleTimeoutSystem = new IdleTimeoutSystem(
                playerData, events, idleConfig.IdleThreshold, idleConfig.MovementEpsilon);

            loop.AddSystem(difficultySystem);
            loop.AddSystem(trailCollisionSystem);
            loop.AddSystem(skidSystem);
            loop.AddSystem(slowdownSystem);
            loop.AddSystem(livesSystem);
            loop.AddSystem(movementSystem);
            loop.AddSystem(trailSystem);
            loop.AddSystem(catAISystem);
            loop.AddSystem(idleTimeoutSystem);
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