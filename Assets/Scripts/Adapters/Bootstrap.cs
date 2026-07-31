using CheeseHeist.Core;
using CheeseHeist.Systems;
using UnityEngine.InputSystem;

namespace CheeseHeist.Adapters
{
    public class Bootstrap
    {
        public Loop CreateLoop(
            InputActionAsset inputActions,
            PlayerBody playerBody,
            VirtualJoystickInputAdapter joystick,
            GyroscopeInputAdapter gyroscope,
            InputSource activeInputSource,
            float acceleration,
            float deceleration)
        {
            var loop = new Loop();

            var playerData = new PlayerData
            {
                MoveSpeed = 5f
            };

            var keyboard = new KeyboardInputAdapter(inputActions);
            var inputRouter = new PlayerInputRouter();

            IMoveInputProvider selectedProvider = activeInputSource switch
            {
                InputSource.Keyboard => keyboard,
                InputSource.VirtualJoystick => joystick,
                InputSource.Gyroscope => gyroscope,
                _ => keyboard
            };

            inputRouter.SetProvider(selectedProvider);

            var movementSystem = new MovementSystem(playerData, inputRouter, acceleration, deceleration);

            playerBody.Initialize(playerData);
            loop.AddSystem(movementSystem);

            return loop;
        }
    }
}