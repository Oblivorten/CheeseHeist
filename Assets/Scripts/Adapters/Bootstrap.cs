using CheeseHeist.Core;
using CheeseHeist.Systems;

namespace CheeseHeist.Adapters
{
    public class Bootstrap
    {
        public GameContext CreateGame(
            SceneReferences refs,
            InputSource activeInputSource,
            MovementConfig movementConfig,
            TrailConfig trailConfig,
            SkidConfig skidConfig,
            LivesConfig livesConfig,
            CatConfig catConfig)
        {
            var loop = new Loop();
            var playerData = new PlayerData { MoveSpeed = 5f };
            var session = new GameSessionData();
            var events = new GameEvents();
            var catData = new CatData { Position = new Vector3Data(0f, catConfig.SpawnHeight, 0f) };

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

            var movementSystem = new MovementSystem(
                playerData, inputRouter, movementConfig.Acceleration, movementConfig.Deceleration);
            refs.PlayerBody.Initialize(playerData);

            var trailSystem = new TrailSystem(
                playerData, trailConfig.SpawnDistance, trailConfig.Lifetime, trailConfig.Capacity, trailConfig.GroundHeight);
            refs.TrailView.Initialize(trailConfig.Capacity);

            var trailCollisionSystem = new TrailCollisionSystem(
                playerData, trailSystem, events, skidConfig.CollisionRadius, skidConfig.GraceSegments);
            var skidSystem = new SkidSystem(
                playerData, events, skidConfig.SpeedMultiplier, skidConfig.Duration);

            refs.ObstacleCollision.Initialize(events);
            var livesSystem = new LivesSystem(
                session, events, livesConfig.StartingLives, livesConfig.InvulnerabilityDuration);

            var catAISystem = new CatAISystem(
                playerData, catData, events,
                catConfig.PatrolDistance, catConfig.LungeDistance, catConfig.LungeWindowDuration, catConfig.FollowSpeed);

            loop.AddSystem(trailCollisionSystem);
            loop.AddSystem(skidSystem);
            loop.AddSystem(livesSystem);
            loop.AddSystem(movementSystem);
            loop.AddSystem(trailSystem);
            loop.AddSystem(catAISystem);

            return new GameContext { Loop = loop, TrailSystem = trailSystem, Events = events, Session = session, Cat = catData };
        }
    }
}