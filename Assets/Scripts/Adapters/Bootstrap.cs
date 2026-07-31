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
            TrailConfig trailConfig)
        {
            var loop = new Loop();
            var playerData = new PlayerData { MoveSpeed = 5f };

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
            loop.AddSystem(movementSystem);

            var trailSystem = new TrailSystem(
                playerData, trailConfig.SpawnDistance, trailConfig.Lifetime, trailConfig.Capacity);
            refs.TrailView.Initialize(trailConfig.Capacity);
            loop.AddSystem(trailSystem);

            return new GameContext { Loop = loop, TrailSystem = trailSystem };
        }
    }
}